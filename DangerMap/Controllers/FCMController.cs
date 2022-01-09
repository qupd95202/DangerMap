using DangerMap.Dtos;
using DangerMap.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FCMController : ControllerBase
    {
        private readonly FCMNotificationManager service;

        public FCMController(FCMNotificationManager service)
        {
            this.service = service;
        }

        /// <summary>
        /// 傳送裝置Token 【回傳值】 200=>成功，其他=>失敗
        /// </summary>
        /// <param name="value">存入之帳戶、裝置Token</param>
        /// <returns></returns>
        [HttpPost("Token")]
        public ActionResult PostToken([FromBody] NotifyTokenDto value)
        {
            return service.PostNotifyToken(value.AccountId, value.NotificationToken);
        }

        /// <summary>
        /// 若周遭有事件，會推播 【回傳值】 200成功(有可能沒事)，其他=>失敗
        /// </summary>
        /// <param name="id">要推播的帳戶ID</param>
        /// <param name="longitude">當時座標經度</param>
        /// <param name="latitude">當時座標緯度</param>
        /// <returns></returns>
        [HttpGet("Notice/{id}")]
        public ActionResult NotifyByDistance([FromRoute] string id, [FromQuery] double longitude, [FromQuery] double latitude)
        {
            service.SendMessage(id, longitude, latitude);
            return Ok();
        }

        /// <summary>
        /// 排程專用夜歸提醒推播
        /// </summary>
        /// <returns></returns>
        [HttpGet("AutoNotice")]
        public ActionResult NotifyAtTime()
        {
            service.SendMessageAtTime();
            return Ok();
        }
    }
}
