using DangerMap.Dtos;
using DangerMap.Models;
using DangerMap.Models.db;
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
    public class NewsTickerController : ControllerBase
    {
        private readonly NewsTickerService newsTickerService;

        public NewsTickerController(NewsTickerService service)
        {
            newsTickerService = service;
        }

        /// <summary>
        /// 抓取全部跑馬燈內容 【回傳值】: 200=>成功
        /// </summary>
        /// <returns></returns>
        // GET: api/<NewsTicker>
        [HttpGet]
        public ActionResult<List<NewsTicker>> Get()
        {
            return Ok(newsTickerService.GetAllNews());
        }
    }
}
