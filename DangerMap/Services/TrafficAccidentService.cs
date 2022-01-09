using DangerMap.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class TrafficAccidentService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        public TrafficAccidentService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 指定座標範圍內之交通事故資料
        /// </summary>
        /// <param name="longitude">使用者經度</param>
        /// <param name="latitude">使用者緯度</param>
        /// <returns>交通事故List</returns>
        public List<TrafficAccident> getDataByRange(double longitude, double latitude)
        {
            var result = database.TrafficAccidents.Where(x => x.Latitude <= latitude + Global.SEARCH_RANGE_DOUBLE &&
                                                            x.Latitude >= latitude - Global.SEARCH_RANGE_DOUBLE &&
                                                            x.Longitude <= longitude + Global.SEARCH_RANGE_DOUBLE &&
                                                            x.Longitude >= longitude - Global.SEARCH_RANGE_DOUBLE);
            return result.ToList();
        }
    }
}
