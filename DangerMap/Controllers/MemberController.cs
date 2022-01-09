using DangerMap.Dtos;
using DangerMap.JWT;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        /// <summary>
        /// 帳戶之服務
        /// </summary>
        private readonly AccountProfileService accountService;
        private readonly JwtAuthenticationManager jwtService;
        /// <summary>
        /// 取得回傳值之服務
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        /// <summary>
        /// 建構子
        /// </summary>
        public MemberController(AccountProfileService service, JwtAuthenticationManager jwtService, uploadtestv1Context db, IHttpContextAccessor httpContextAccessor)
        {
            accountService = service;
            this.jwtService = jwtService;
            database = db;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 取得會員資訊 【需要Token】 【回傳值】: 200=>成功(附帶會員資訊{AccountProfileDto})，204 =>查無此人，其他=>失敗
        /// </summary>
        /// <param name="id">要取得的ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<AccountProfileDto> GetAccount([FromRoute] string id)
        {
            var result = database.AccountProfiles.Where(x => x.AccountId == id).SingleOrDefault();
            if (result == null)
            {
                return NoContent();
            }
            return new AccountProfileDto
            {
                AccountEmail = result.AccountEmail,
                AccountId = result.AccountId,
                AccountName = result.AccountName,
                PropicLink = result.PropicLink,
                Validation = Convert.ToBoolean(httpContextAccessor.HttpContext.User.FindFirstValue("Validation"))
            };
        }


        /// <summary>
        /// 登入 【回傳值】: 200=>成功(附帶Token{string})，其他=>帳號或密碼有誤
        /// </summary>
        /// <param name="idAndPassword">帳密:AccountLoginDto</param>
        /// <returns>成功給token，失敗回傳帳密錯誤</returns>
        [HttpPost("Login")]
        public ActionResult<string> Login([FromBody] AccountLoginDto idAndPassword)
        {
            var token = jwtService.Authenticate(idAndPassword);
            if (token == null)
            {
                return BadRequest();
            }
            return token.JwtToken;
        }

        /// <summary>
        /// 上傳會員資料(註冊) 【回傳值】: 200=>成功，404 => 伺服器找不到(錯誤)，其他=>失敗
        /// </summary>
        /// <param name="value">傳入格式:AccountProfileInputDto【註冊所需資料】</param>
        // POST api/Member/Register
        [HttpPost("Register")]
        public ActionResult PostMember([FromBody] AccountProfileInputDto value)
        {
            return accountService.UploadData(value).Result;
        }

        /// <summary>
        /// 換發token 【回傳值】: 200=>成功(附帶Token{string})，401 => 沒有權限，重新登入，400=>token有誤
        /// </summary>
        /// <param name="token">AccountID、舊的Token</param>
        /// <returns>400:token給錯 401:請重新登入 200: 回傳新的token</returns>
        [HttpPost("RefreshToken")]
        public ActionResult<string> Refresh([FromBody] TokenDto token)
        {
            var accountID = token.AccountID;
            var oldRefreshToken = database.AccountProfiles.Where(x => x.AccountId == accountID).Select(x => x.RefreshToken).FirstOrDefault();
            if (oldRefreshToken == null)
            {
                return BadRequest();
            }
            var result = jwtService.Refresh(new AuthenticationResponse { JwtToken = token.Token, RefreshToken = oldRefreshToken });
            if (result == null)
            {
                return Unauthorized();
            }
            return result.JwtToken;
        }

        /// <summary>
        /// 修改帳號暱稱和大頭貼 【回傳值】: 200=>成功，204=>查無此人，其他=>失敗
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Edit/Account/{id}")]
        public ActionResult EditNameAndPropic(string id, [FromBody] AccountPutDto input)
        {
            return accountService.EditNameAndPropic(id, input);
        }

        /// <summary>
        /// 更改使用者密碼 【需要登入】 【回傳值】: 200=>成功，204=>查無此人，其他=>失敗
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("Edit/Password/{id}")]
        public ActionResult EditPassword(string id, [FromBody] AccountPasswordPutDto input)
        {
            return accountService.EditPassword(id, input);
        }

        /// <summary>
        /// 登出 【回傳值】: 200=>成功，其他=>失敗
        /// </summary>
        /// <param name="id">要登出的AccountID</param>
        /// <returns></returns>
        [HttpPost("Logout")]
        public ActionResult Logout([FromBody] AccountIDDto id)
        {
            return jwtService.CleanRefreshToken(id.AccountId);
        }

        /// <summary>
        /// 將用戶變成已驗證 【前端無需使用】
        /// </summary>
        /// <param name="id">用戶ID</param>
        /// <returns>是否成功</returns>
        // GET api/Login/Verify/id
        [HttpGet("Verify/{id}")]
        public ActionResult Verify([FromRoute] string id)
        {
            return accountService.Verify(id);
        }

        /// <summary>
        /// 寄出驗證信 【回傳值】: 200=>成功(如果信箱是錯的，無從考證)，其他=>失敗
        /// </summary>
        /// <param name="id">用戶ID</param>
        /// <returns>是否成功</returns>
        [HttpGet("Mail/{id}")]
        public ActionResult SendValidationMail([FromRoute] string id)
        {
            return accountService.SendMailByGmail(id);
        }

        /// <summary>
        /// 寄出忘記密碼信 【回傳值】: 200=>成功(如果信箱是錯的，無從考證)，404=>查無此ID，其他=>失敗
        /// </summary>
        /// <param name="account">用戶ID</param>
        /// <returns>是否成功</returns>
        [HttpPost("PasswordMail")]
        public ActionResult SendPasswordMail([FromBody] AccountIDDto account)
        {
            return accountService.ForgetPassword(account.AccountId);
        }
    }
}
