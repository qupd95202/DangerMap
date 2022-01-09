using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;

namespace DangerMap.Services
{
    public class FCMNotificationManager
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        public FCMNotificationManager(uploadtestv1Context database)
        {
            this.database = database;
        }

        /// <summary>
        /// 存入裝置ID(for推播)
        /// </summary>
        /// <param name="accountID">要存入之帳戶ID</param>
        /// <param name="token">TOKEN</param>
        /// <returns></returns>
        public ActionResult PostNotifyToken(string accountID, string token)
        {
            var user = database.AccountProfiles.Where(x => x.AccountId == accountID).FirstOrDefault();
            if (user == null)
            {
                return new BadRequestResult();
            }
            user.NotificationToken = token;
            database.AccountProfiles.Update(user);
            try
            {
                database.SaveChanges();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
            return new OkResult();
        }


        /// <summary>
        /// 即時性推播
        /// </summary>
        /// <param name="accountID">要推播的帳戶</param>
        /// <param name="longitude">使用者經度</param>
        /// <param name="latitude">使用者緯度</param>
        public void SendMessage(string accountID, double longitude, double latitude)
        {
            string token = database.AccountProfiles.Where(x => x.AccountId == accountID).Select(x => x.NotificationToken).FirstOrDefault();
            if (token == null)
            {
                return;
            }
            token = token.Trim();
            int eventNumber = database.InstantEvents.Where(x => x.Latitude <= latitude + Global.SEARCH_RANGE_DOUBLE &&
                                              x.Latitude >= latitude - Global.SEARCH_RANGE_DOUBLE &&
                                              x.Longitude <= longitude + Global.SEARCH_RANGE_DOUBLE &&
                                              x.Longitude >= longitude - Global.SEARCH_RANGE_DOUBLE).Count();
            int accidentNumber = database.TrafficAccidents.Where(x => x.Latitude <= latitude + Global.SEARCH_RANGE_DOUBLE &&
                                              x.Latitude >= latitude - Global.SEARCH_RANGE_DOUBLE &&
                                              x.Longitude <= longitude + Global.SEARCH_RANGE_DOUBLE &&
                                              x.Longitude >= longitude - Global.SEARCH_RANGE_DOUBLE).Count();
            int incidentNumber;
            string area = CriminalIncidentService.ProcessRequest(latitude, longitude);
            if (area == string.Empty)
            {
                incidentNumber = 0;
            }
            else
            {
                //處理混字
                if (area.Contains("台"))
                {
                    area = area.Replace("台", "臺");
                }
                incidentNumber = database.CriminalIncidents.Where(x => x.Location.Contains(area) || x.Location == area || area.Contains(x.Location)).Count();
            }

            if (eventNumber >= 5)
            {
                Global.SendNotification($"你身邊有{eventNumber}起事件正在發生", "危機地圖警報！", token);
            }
            if (accidentNumber >= 5)
            {
                Global.SendNotification("請小心，這裡是交通事故熱點", "危機地圖警報！", token);
            }
            if (incidentNumber >= 5)
            {
                Global.SendNotification($"你已抵達{area}，請注意安全", "危機地圖警報！", token);
            }
        }

        /// <summary>
        /// 排程專用API 【夜歸提醒】
        /// </summary>
        public void SendMessageAtTime()
        {
            string[] ids = database.AccountProfiles.Where(x => x.NotificationToken != null).Select(x => x.NotificationToken.Trim()).ToList().ToArray();
            Global.SendNotification("醉不上路生命安全有保障，危機地圖關心您", "夜歸提醒", ids);
        }
    }
}
