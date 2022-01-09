using DangerMap.Dtos;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{   /// <summary>
    ///// 事件與會員之控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InstantController : ControllerBase
    {
        /// <summary>
        /// 事件之服務
        /// </summary>
        private readonly InstantEventService eventService;

        /// <summary>
        /// 帳戶之服務
        /// </summary>
        private readonly AccountProfileService accountService;

        /// <summary>
        /// 目擊之服務
        /// </summary>
        private readonly EventWitnessService witnessService;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="service">事件服務</param>
        /// <param name="service2">會員服務</param>
        /// <param name="service3">目擊服務</param>
        public InstantController(InstantEventService service, AccountProfileService service2, EventWitnessService service3)
        {
            eventService = service;
            accountService = service2;
            witnessService = service3;
        }

        /// <summary>
        /// 取得全部事件 【回傳值】: 200=>成功(事件{InstantEvent})，其他=>失敗
        /// </summary>
        /// <returns>事件List</returns>
        // GET: api/<InstantController>AllEvent
        [HttpGet("AllEvent")]
        public List<InstantEvent> GetAllEvent()
        {
            return eventService.getAllData();
        }

        /// <summary>
        /// 取得一筆事件 【回傳值】: 200=>成功(事件{InstantEventOutputDto})，其他=>失敗
        /// </summary>
        /// <param name="accountID">查詢者ID</param>
        /// <param name="eventID">查詢的資料</param>
        /// <returns>事件</returns>
        [HttpGet("Event/{accountID}")]
        public ActionResult<InstantEventOutputDto> GetEvent([FromRoute] string accountID, [FromQuery] Guid eventID)
        {
            var result = eventService.getData(accountID, eventID);
            if (result == null)
            {
                return BadRequest();
            }
            return result;
        }

        /// <summary>
        /// 取得固定範圍事件 【回傳值】: 200=>成功(事件{InstantEventOutputDto})，204=>範圍內沒事件，其他=>失敗
        /// </summary>
        /// <param name="accountID">使用者ID</param>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>事件Dto List</returns>
        // GET api/<InstantController>/Event
        [HttpGet("RangeEvent/{id}")]
        public ActionResult<List<InstantEventOutputDto>> GetEventByRange([FromRoute] string id, [FromQuery] double longitude, [FromQuery] double latitude)
        {
            var result = eventService.getDataByDistance(id, longitude, latitude);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return result;
        }

        /// <summary>
        /// 取得固定範圍事件(免登入) 【回傳值】: 200=>成功(事件{InstantEventOutputDto})，204=>範圍內沒事件，其他=>失敗
        /// </summary>
        /// <param name="accountID">使用者ID</param>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>事件Dto List</returns>
        // GET api/<InstantController>/Event
        [HttpGet("RangeEvent")]
        public ActionResult<List<InstantEventOutputDto>> GetEventByRangeNoLogin([FromQuery] double longitude, [FromQuery] double latitude)
        {
            var result = eventService.getDataByDistanceNoLogin(longitude, latitude);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return result;
        }

        /// <summary>
        /// 取得特定範圍事件 【回傳值】: 200=>成功(事件{InstantEventOutputDto})，204=>範圍內沒事件，其他=>失敗
        /// </summary>
        /// <param name="accountID">使用者ID</param>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>事件Dto List</returns>
        // GET api/<InstantController>/Event
        [HttpGet("OptionalRangeEvent/{id}/{meter}")]
        public ActionResult<List<InstantEventOutputDto>> GetEventByOptionalRange([FromRoute] string id, [FromRoute] double meter, [FromQuery] double longitude, [FromQuery] double latitude)
        {
            var result = eventService.getDataByDistance(id, longitude, latitude, Global.MeterChange2Tude(meter));
            if (result.Count == 0)
            {
                return NoContent();
            }
            return result;
        }

        /// <summary>
        /// 取得特定範圍事件(免登入) 【回傳值】: 200=>成功(事件{InstantEventOutputDto})，204=>範圍內沒事件，其他=>失敗
        /// </summary>
        /// <param name="accountID">使用者ID</param>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>事件Dto List</returns>
        // GET api/<InstantController>/Event
        [HttpGet("OptionalRangeEvent/{meter}")]
        public ActionResult<List<InstantEventOutputDto>> GetEventByRangeOptionalNoLogin([FromRoute] double meter, [FromQuery] double longitude, [FromQuery] double latitude)
        {
            var result = eventService.getDataByDistanceNoLogin(longitude, latitude,Global.MeterChange2Tude(meter));
            if (result.Count == 0)
            {
                return NoContent();
            }
            return result;
        }

        /// <summary>
        /// 取得特定事件目擊數 【回傳值】: 200=>成功(目擊數{int})，其他=>失敗
        /// </summary>
        /// <param name="Id">事件 ID</param>
        /// <returns>目擊數</returns>
        // GET api/<InstantController>/Event
        [HttpGet("IsWitness")]
        public ActionResult<int> GetWitness([FromQuery] Guid Id)
        {
            try
            {
                return witnessService.GetWitnessAmount(Id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 取得特定事件未目擊數 【回傳值】: 200=>成功(未目擊數{int})，其他=>失敗
        /// </summary>
        /// <param name="Id">事件 ID</param>
        /// <returns>未目擊數</returns>
        // GET api/<InstantController>/Event
        [HttpGet("NoWitness")]
        public ActionResult<int> GetNoWitness([FromQuery] Guid Id)
        {
            try
            {
                return witnessService.GetNoWitnessAmount(Id);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        /// <summary>
        /// 上傳事件【需要有token(登入)+已驗證】 Header寫法 : "Authorization : Bearer  + {Token字串}" 【回傳值】: 200=>成功，401=>Token有誤或缺失，404=>沒有驗證，其他=>失敗
        /// </summary>
        /// <param name="value">傳入格式:InstantEventInputDto【事件資訊】</param>
        // POST api/<InstantController>/Event
        [Authorize]
        [HttpPost("Event")]
        public ActionResult PostEvent([FromBody] InstantEventInputDto value)
        {
            if (!IsValidation())
            {
                return NotFound();
            }
            return eventService.UploadData(value);
        }

        /// <summary>
        /// 是否目擊事件【需要有token(登入)+已驗證】 【回傳值】: 200=>成功，401=>Token有誤或缺失，404=>該會員沒有驗證，其他=>失敗
        /// </summary>
        /// <param name="value">傳入格式:WitnessDto【目擊資訊】</param>
        /// <returns>是否成功</returns>
        // POST api/<InstantController>/Event
        [Authorize]
        [HttpPost("IsWitness")]
        public ActionResult PostWitness([FromBody] WitnessDto value)
        {
            if (!IsValidation())
            {
                return Unauthorized();
            }
            return witnessService.PostWitness(value);
        }

        /// <summary>
        /// 刪除過了兩天之事件 【前端無須使用，GCP掛載排程專用】
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult DeleteEvent()
        {
            return eventService.DeleteExpiredData();
        }

        /// <summary>
        /// 用token判斷用戶是否驗證
        /// </summary>
        /// <returns>有"1"或沒有"0"驗證</returns>
        private bool IsValidation()
        {
            return HttpContext.User.FindFirstValue("Validation") == "True";
        }
    }
}