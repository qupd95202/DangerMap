using DangerMap.Dtos;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DangerMap.JWT
{
    /// <summary>
    /// 生成JWT之服務
    /// </summary>
    public class JwtAuthenticationManager : IJwtAuthenticationManger
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        /// <summary>
        /// 設定檔
        /// </summary>
        private readonly IConfiguration configuration;
        /// <summary>
        /// 生成更新Token之服務
        /// </summary>
        private readonly IRefreshTokenGenerator refreshTokenGenertor;

        /// <summary>
        /// 建構子
        /// </summary>
        public JwtAuthenticationManager(uploadtestv1Context db, IConfiguration configuration, IRefreshTokenGenerator refreshTokenGenertor)
        {
            database = db;
            this.configuration = configuration;
            this.refreshTokenGenertor = refreshTokenGenertor;
        }

        /// <summary>
        /// 登入生成JWT
        /// </summary>
        /// <param name="idPassword">帳密</param>
        /// <returns>回傳Token</returns>
        public AuthenticationResponse Authenticate(AccountLoginDto idPassword)
        {
            var user = database.AccountProfiles.Where(x => x.AccountId == idPassword.AccountId && x.Password == idPassword.Password).SingleOrDefault();
            if (user == null)
            {
                return null;
            }
            else
            {
                //設定使用者資訊
                var claims = new List<Claim>
                {
                    new Claim("Name", user.AccountName),
                    new Claim("Email", user.AccountEmail),
                    new Claim("ID", user.AccountId),
                    new Claim("Validation",user.Validation.ToString())
                };

                //取出appsettings.json裡的KEY處理
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:KEY"]));

                //設定jwt相關資訊
                var jwt = new JwtSecurityToken
                (
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(5),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                //產生JWT Token、Refresh JWT
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                var refreshToken = refreshTokenGenertor.GenerateToken();

                //存進資料庫
                user.RefreshToken = refreshToken;
                database.SaveChanges();

                //回傳JWT Token給認證通過的使用者
                return new AuthenticationResponse
                {
                    JwtToken = token,
                    RefreshToken = refreshToken
                };
            }
        }


        /// <summary>
        /// Refresh 新的JWT
        /// </summary>
        /// <param name="refreshCred">舊的</param>
        /// <returns>新的</returns>
        public AuthenticationResponse Refresh(AuthenticationResponse refreshCred)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            ClaimsPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(refreshCred.JwtToken,
                new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:KEY"]))
                }, out validatedToken);
            }
            catch (Exception)
            {
                return null;
            }

            var jwtToken = validatedToken as JwtSecurityToken;

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return null;
            }

            var accountID = principal.Claims.Where(x => x.Type == "ID").Select(x => x.Value).SingleOrDefault();
            if (string.IsNullOrEmpty(accountID))
            {
                return null;
            }

            var currentUser = database.AccountProfiles.Where(x => x.AccountId == accountID && x.RefreshToken == refreshCred.RefreshToken).FirstOrDefault();
            if (currentUser == null)
            {
                return null;
            }

            //取出appsettings.json裡的KEY處理
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:KEY"]));

            //設定jwt相關資訊
            var newJwt = new JwtSecurityToken
            (
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: jwtToken.Claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            //產生JWT Token、Refresh JWT
            var token = new JwtSecurityTokenHandler().WriteToken(newJwt);
            var refreshToken = refreshTokenGenertor.GenerateToken();

            currentUser.RefreshToken = refreshToken;

            database.SaveChanges();


            return new AuthenticationResponse
            {
                JwtToken = token,
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// 清除RefreshToken
        /// </summary>
        /// <param name="accountID">要清除之用戶ID</param>
        /// <returns>成功或失敗</returns>
        public ActionResult CleanRefreshToken(string accountID)
        {
            var user = database.AccountProfiles.Where(x => x.AccountId == accountID).Select(x => x).FirstOrDefault();
            if (user != null)
            {
                user.RefreshToken = null;
                database.AccountProfiles.Update(user);
                try
                {
                    database.SaveChanges();
                    return new OkResult();
                }
                catch (Exception)
                {
                    return new BadRequestResult();
                }
            }
            return new NotFoundResult();
        }
    }
}

