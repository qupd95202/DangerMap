using DangerMap.Dtos;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class CriminalIncidentService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;   
            public CriminalIncidentService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 指定行政區之犯罪資料
        /// </summary>
        /// <param name="longitude">使用者經度</param>
        /// <param name="latitude">使用者緯度</param>
        /// <returns>犯罪統計表</returns>
        public IncidentOutPutDto getDataByArea(double longitude, double latitude)
        {
            string area = ProcessRequest(latitude, longitude);
            if (area == string.Empty)
            {
                return null;
            }
            //處理混字
            if (area.Contains("台"))
            {
                area = area.Replace("台", "臺");
            }

            var district = database.CriminalIncidents.Where(x => x.Location.Contains(area) || x.Location == area || area.Contains(x.Location));
            if (district.Count() == 0)
            {
                return null;
            }
            var result = district.GroupBy(x => x.Type);

            return new IncidentOutPutDto()
            {
                District = district.Select(x => x.Location).ToList()[0],
                Rape = result.Count(x => x.Key == "強制性交"),
                Rob = result.Count(x => x.Key == "竊盜"),
                Drug = result.Count(x => x.Key == "毒品"),
                CarTheft = result.Count(x => x.Key == "汽車竊盜"),
                ScooterTheft = result.Count(x => x.Key == "機車竊盜"),
                Snatch = result.Count(x => x.Key == "搶奪"),
                HouseTheft = result.Count(x => x.Key == "住宅竊盜")
            };
        }

        /// <summary>
        /// 座標轉行政區
        /// </summary>
        /// <param name="lat">使用者緯度</param>
        /// <param name="lng">使用者經度</param>
        /// <returns>行政區</returns>
        public static string ProcessRequest(double lat, double lng)
        {
           
            string latlng = $"{lat},{lng}";

            if (!string.IsNullOrEmpty(latlng))
            {
                string url =
                "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + latlng + "&sensor=true&key=AIzaSyC0VIiHOC25zr_k-VYZpXqAvQbV1dahVQQ";
                string json = String.Empty;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                //指定語言，否則Google預設回傳英文
                request.Headers.Add("Accept-Language", "zh-tw");

                using (var response = request.GetResponse())
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {

                    json = sr.ReadToEnd();

                }
                JObject obj = (JObject)JsonConvert.DeserializeObject(json);
                string status = obj["status"].ToString();
                if (status == "OK")
                {
                    JArray ary = (JArray)obj["results"];
                    ary = (JArray)ary[0]["address_components"];

                    var q = from o in ary
                            where o["types"].Count() >= 2 && o["types"][1].ToString() == "political" && o["types"][0].ToString() == "administrative_area_level_1" || o["types"][0].ToString() == "administrative_area_level_2" || o["types"][0].ToString() == "administrative_area_level_3"
                            select o;

                    if (q.Count() == 0)
                    {
                        return string.Empty;
                    }
                    var district = q.LastOrDefault()["long_name"].ToString() + q.FirstOrDefault()["long_name"].ToString();
                    return district;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
