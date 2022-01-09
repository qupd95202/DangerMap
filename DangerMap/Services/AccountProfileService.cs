using DangerMap.Dtos;
using System.Net.Mail;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class AccountProfileService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;

        public AccountProfileService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 上傳會員資料(註冊)
        /// </summary>
        public async Task<ActionResult> UploadData(AccountProfileInputDto value)
        {
            var name = value.AccountName;
            //沒有輸入名字的話，預設暱稱等於帳號ID
            if (value.AccountName == string.Empty)
            {
                name = value.AccountId;
            }
            var result = new AccountProfile()
            {
                PropicLink = value.PropicLink,
                Password = value.Password,
                AccountEmail = value.AccountEmail,
                AccountId = value.AccountId,
                AccountName = name,
                Validation = false
            };
            await database.AccountProfiles.AddAsync(result);
            try
            {
                database.SaveChanges();
                return new OkResult();
            }
            catch (Exception)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// 修改會員暱稱和大頭貼
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult EditNameAndPropic(string accountId, [FromBody] AccountPutDto value)
        {
            try
            {
                var update = (from a in database.AccountProfiles
                              where a.AccountId == accountId
                              select a).SingleOrDefault();

                if (update != null)
                {
                    update.AccountName = value.AccountName;
                    update.PropicLink = value.PropicLink;
                    database.SaveChanges();
                    return new OkResult();
                }
                return new NoContentResult();
            }
            catch (Exception)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// 修改會員密碼
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult EditPassword(string accountId, [FromBody] AccountPasswordPutDto value)
        {
            var update = (from a in database.AccountProfiles
                          where a.AccountId == accountId
                          select a).SingleOrDefault();

            if (update != null)
            {
                if (value.OldPassword.Equals(update.Password))
                {
                    try
                    {
                        update.Password = value.NewPassword;
                        database.SaveChanges();
                        return new OkResult();
                    }
                    catch (Exception)
                    {
                        return new BadRequestResult();
                    }
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            else
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// 取得一筆特定會員資料
        /// </summary>
        /// <param name="AccountID">要查詢會員的帳號</param>
        /// <returns>回傳該會員資料或null</returns>
        public AccountProfile GetAccountProfile(string AccountID)
        {
            var result = database.AccountProfiles.Where(self => self.AccountId == AccountID).ToList().FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 驗證用戶
        /// </summary>
        /// <param name="accountID">要驗證的用戶ID</param>
        /// <returns>是否成功</returns>
        public ActionResult Verify(string accountID)
        {
            var result = database.AccountProfiles.Where(x => x.AccountId == accountID).SingleOrDefault();
            if (result == null)
            {
                return new NotFoundResult();
            }
            result.Validation = true;
            database.AccountProfiles.Update(result);
            try
            {
                database.SaveChanges();
                return new OkObjectResult("你已完成驗證，請記得重新登入");
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// 寄出驗證信 
        /// </summary>
        /// <param name="accountID">要驗證的用戶ID</param>
        /// <returns>是否成功</returns>
        public ActionResult SendMailByGmail(string accountID)
        {
            var mailReceiver = database.AccountProfiles.Where(x => x.AccountId == accountID).Select(x => x.AccountEmail).SingleOrDefault();
            if (mailReceiver == null)
            {
                return new NotFoundResult();
            }
            string sender = "認證郵件";
            string subject = "危機地圖會員認證信";
            string body = $"請點擊https://finaldangermap.appspot.com/api/Member/Verify/{accountID}，來認證您的身分，並請重新登入。\n謝謝。";

            Global.SendMail(mailReceiver, sender, subject, body);
            return new OkResult();
        }


        /// <summary>
        /// 寄出忘記密碼之信
        /// </summary>
        /// <param name="accountID">忘記密碼之使用者ID</param>
        /// <returns>是否成功</returns>
        public ActionResult ForgetPassword(string accountID)
        {
            var user = database.AccountProfiles.Where(x => x.AccountId == accountID).FirstOrDefault();
            if (user == null)
            {
                return new NotFoundResult();
            }

            var randomNumber = new byte[1];
            RandomNumberGenerator.Create().GetBytes(randomNumber);
            var newPassword = Convert.ToBase64String(randomNumber).GetHashCode().ToString();
            newPassword = newPassword.Substring(1);
            string sender = "密碼忘記郵件";
            string subject = "危機地圖會員新密碼";
            string body = $"您的新密碼為{newPassword}，請登入後儘速更改密碼，謝謝。";
            Global.SendMail(user.AccountEmail, sender, subject, body);

            user.Password = Global.ToMD5(newPassword);
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
    }
}
