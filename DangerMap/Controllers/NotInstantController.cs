using DangerMap.Dtos;
using DangerMap.Models.db;
using DangerMap.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DangerMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotInstantController : ControllerBase
    {
        /// <summary>
        /// 事件之服務
        /// </summary>
        private readonly CriminalIncidentService criminalIncidentService;

        /// <summary>
        /// 事故之服務
        /// </summary>
        private readonly TrafficAccidentService trafficAccidentService;

        public NotInstantController(CriminalIncidentService service, TrafficAccidentService service2)
        {
            criminalIncidentService = service;
            trafficAccidentService = service2;
        }

        /// <summary>
        /// 取得特定範圍犯罪事件統計 【使用方式】:網址後直接加上【?longitude=X{and}latitude=Y】 【回傳值】: 200=>成功 ， 204 => 沒有資料 ，其他=>失敗
        /// </summary>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>犯罪事件統計</returns>
        // GET: api/<ValuesController>
        [HttpGet("CriminalIncident")]
        public ActionResult<IncidentOutPutDto> GetAreaCriminal([FromQuery] double longitude, [FromQuery] double latitude)
        {
            var result = criminalIncidentService.getDataByArea(longitude, latitude);
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }

        /// <summary>
        /// 取得特定範圍交通事故  【回傳值】: 200=>成功 ， 204 => 沒有資料 ，其他=>失敗
        /// </summary>
        /// <param name="longitude">使用者座標經度</param>
        /// <param name="latitude">使用者座標緯度</param>
        /// <returns>犯罪事件List</returns>
        // GET: api/<ValuesController>
        [HttpGet("TrafficAccident")]
        public ActionResult<List<TrafficAccident>> GetAreaAccident([FromQuery] double longitude, [FromQuery] double latitude)
        {
            var result = trafficAccidentService.getDataByRange(longitude, latitude);
            if (result.Count != 0)
            {
                return Ok(result);
            }
            return NoContent();
        }
    }
}
