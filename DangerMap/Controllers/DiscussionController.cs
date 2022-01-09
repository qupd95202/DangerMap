using DangerMap.Dtos;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        /// <summary>
        /// 帳戶之服務
        /// </summary>
        private readonly EventDiscussionService discussionService;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="service">注入之服務</param>
        public DiscussionController(EventDiscussionService service)
        {
            discussionService = service;
        }

        /// <summary>
        /// 取得指定事件之所有留言 【回傳值】: 200=>成功，204=>沒有留言，其他=>失敗
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <returns>留言資訊</returns>
        // GET api/<DiscussionController>/
        [HttpGet("{eventId}")]
        public ActionResult<List<DiscussionOutputDto>> GetAll(Guid eventId)
        {
            var result = discussionService.GetAllDatas(eventId);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return result;
        }

        /// <summary>
        /// 取得指定事件之特定筆數留言 【回傳值】: 200=>成功，204=>沒有留言，其他=>失敗
        /// </summary>
        /// <param name="eventId">事件 ID</param>
        /// <param name="amount">要取多少筆</param>
        /// <returns>留言資訊</returns>
        // GET api/<DiscussionController>/
        [HttpGet("{eventId}/{amount}")]
        public ActionResult<List<DiscussionOutputDto>> Get(Guid eventId, int amount)
        {
            var result = discussionService.GetDatas(eventId, amount);
            if (result.Count == 0)
            {
                return NoContent();
            }
            return result;
        }

        /// <summary>
        /// 新增一筆留言【需要有token(登入)】 【回傳值】: 200=>成功，401=>Token失效或有誤，其他=>失敗
        /// </summary>
        /// <param name="value">傳入格式:EventDiscussionDto【留言板資訊】</param>
        // POST api/<DiscussionController>
        [Authorize]
        [HttpPost]
        public ActionResult Post([FromBody] EventDiscussionDto value)
        {
            try
            {
                return Ok(discussionService.PutOneData(value));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
