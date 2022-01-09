using DangerMap.Dtos;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DangerMap.Services
{
    public class InstantEventService
    {
        /// <summary>
        /// 事件編號
        /// </summary>

        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;

        public InstantEventService(uploadtestv1Context context)
        {
            database = context;
        }

        /// <summary>
        /// 全部事件
        /// </summary>
        /// <returns>事件List</returns>
        public List<InstantEvent> getAllData()
        {
            return database.InstantEvents.ToList();
        }

        /// <summary>
        /// 取得一筆事件
        /// </summary>
        /// <param name="accountID">查詢者帳號</param>
        /// <param name="id">事件id</param>
        /// <returns>資料</returns>
        public InstantEventOutputDto getData(string accountID, Guid id)
        {
            var qEvent = database.InstantEvents.Where(x => x.EventId == id).FirstOrDefault();
            if (qEvent == null)
            {
                return null;
            }
            var account = database.AccountProfiles.Where(x => x.AccountId == qEvent.AccountId).FirstOrDefault();
            var witness = database.EventWitnesses.Where(x => x.EventId == id);
            var isWitness = witness.Where(x => x.AccountId == accountID).FirstOrDefault();
            return new InstantEventOutputDto()
            {
                UploaderAccountId = account.AccountId,
                UploaderAccountName = account.AccountName,
                Description = qEvent.Description,
                EventId = qEvent.EventId,
                Latitude = qEvent.Latitude,
                LocationDetails = qEvent.LocationDetails,
                Longitude = qEvent.Longitude,
                ShotLink = qEvent.ShotLink,
                Title = qEvent.Title,
                Type = qEvent.Type,
                UploaderPropicLink = account.PropicLink,
                UploadTime = qEvent.UploadTime,
                IsWitness = isWitness == null ? (byte)0 : isWitness.Witness,
                TotalNotWitness = witness.Count() == 0 ? 0 : witness.Count(x => x.Witness == 2),
                TotalWitness = witness.Count() == 0 ? 0 : witness.Count(x => x.Witness == 1)
            };
        }

        /// <summary>
        /// 定點的一定範圍內之事件
        /// </summary>
        /// <param name="accountID">查詢者之ID</param>
        /// <param name="longitude">查詢者經度</param>
        /// <param name="latitude">查詢者緯度</param>
        /// <returns></returns>
        public List<InstantEventOutputDto> getDataByDistance(string accountID, double longitude, double latitude)
        {
            return getDataByDistance(accountID, longitude, latitude, Global.SEARCH_RANGE_DOUBLE);
        }

        /// <summary>
        /// 定點的一定範圍內之事件
        /// </summary>
        /// <param name="accountID">查詢者之ID</param>
        /// <param name="longitude">查詢者經度</param>
        /// <param name="latitude">查詢者緯度</param>
        /// <param name="searchRange">範圍大小(經緯度)</param>
        /// <returns></returns>
        public List<InstantEventOutputDto> getDataByDistance(string accountID, double longitude, double latitude, double searchRange)
        {
            var instantEvents = database.InstantEvents.Where(x => x.Latitude <= latitude + searchRange &&
                                                            x.Latitude >= latitude - searchRange &&
                                                            x.Longitude <= longitude + searchRange &&
                                                            x.Longitude >= longitude - searchRange);
            var newEvents = instantEvents.Join(database.AccountProfiles, events => events.AccountId, account => account.AccountId, (events, account) => new InstantEventOutputDto()
            {
                EventId = events.EventId,
                UploaderAccountId = events.AccountId,
                UploaderPropicLink = account.PropicLink,
                Type = events.Type,
                Title = events.Title,
                Description = events.Description,
                Longitude = events.Longitude,
                Latitude = events.Latitude,
                LocationDetails = events.LocationDetails,
                UploadTime = events.UploadTime,
                ShotLink = events.ShotLink,
                UploaderAccountName = account.AccountName,
                IsWitness = 0
            });
            var right = database.EventWitnesses.Where(x => x.AccountId == accountID);
            var witness = from o in newEvents
                          join p in right
                          on o.EventId equals p.EventId
                          into groupjoin
                          from a in groupjoin.DefaultIfEmpty()
                          select new InstantEventOutputDto()
                          {
                              EventId = o.EventId,
                              UploaderAccountId = o.UploaderAccountId,
                              UploaderPropicLink = o.UploaderPropicLink,
                              Type = o.Type,
                              Title = o.Title,
                              Description = o.Description,
                              Longitude = o.Longitude,
                              Latitude = o.Latitude,
                              LocationDetails = o.LocationDetails,
                              UploadTime = o.UploadTime,
                              ShotLink = o.ShotLink,
                              UploaderAccountName = o.UploaderAccountName,
                              IsWitness = a.Witness == null ? (byte)0 : a.Witness
                          };
            var right2 = database.EventWitnesses.GroupBy(x => x.EventId).Select(x => new WitnessAmountDto { EventID = x.Key, WitnessAmount = x.Count(y => y.Witness == 1), NotWitness = x.Count(y => y.Witness == 2) });
            var result = from o in witness
                         join p in right2
                         on o.EventId equals p.EventID
                         into groupjoin
                         from a in groupjoin.DefaultIfEmpty()
                         select new InstantEventOutputDto()
                         {
                             EventId = o.EventId,
                             UploaderAccountId = o.UploaderAccountId,
                             UploaderPropicLink = o.UploaderPropicLink,
                             Type = o.Type,
                             Title = o.Title,
                             Description = o.Description,
                             Longitude = o.Longitude,
                             Latitude = o.Latitude,
                             LocationDetails = o.LocationDetails,
                             UploadTime = o.UploadTime,
                             ShotLink = o.ShotLink,
                             UploaderAccountName = o.UploaderAccountName,
                             IsWitness = o.IsWitness,
                             TotalNotWitness = a.NotWitness == null ? 0 : a.NotWitness,
                             TotalWitness = a.WitnessAmount == null ? 0 : a.WitnessAmount
                         };
            return result.ToList();
        }

        /// <summary>
        /// 定點的一定範圍內之事件 (免登入)
        /// </summary>
        /// <param name="accountID">查詢者之ID</param>
        /// <param name="longitude">查詢者經度</param>
        /// <param name="latitude">查詢者緯度</param>
        /// <returns></returns>
        public List<InstantEventOutputDto> getDataByDistanceNoLogin(double longitude, double latitude)
        {
            return getDataByDistanceNoLogin(longitude, latitude, Global.SEARCH_RANGE_DOUBLE);
        }

        /// <summary>
        /// 定點的一定範圍內之事件 (免登入)
        /// </summary>
        /// <param name="accountID">查詢者之ID</param>
        /// <param name="longitude">查詢者經度</param>
        /// <param name="latitude">查詢者緯度</param>
        /// <param name="searchRange">範圍大小(經緯度)</param>
        /// <returns></returns>
        public List<InstantEventOutputDto> getDataByDistanceNoLogin(double longitude, double latitude, double searchRange)
        {
            var instantEvents = database.InstantEvents.Where(x => x.Latitude <= latitude + searchRange &&
                                                            x.Latitude >= latitude - searchRange &&
                                                            x.Longitude <= longitude + searchRange &&
                                                            x.Longitude >= longitude - searchRange);
            var newEvents = instantEvents.Join(database.AccountProfiles, events => events.AccountId, account => account.AccountId, (events, account) => new InstantEventOutputDto()
            {
                EventId = events.EventId,
                UploaderAccountId = events.AccountId,
                UploaderPropicLink = account.PropicLink,
                Type = events.Type,
                Title = events.Title,
                Description = events.Description,
                Longitude = events.Longitude,
                Latitude = events.Latitude,
                LocationDetails = events.LocationDetails,
                UploadTime = events.UploadTime,
                ShotLink = events.ShotLink,
                UploaderAccountName = account.AccountName
            });

            var right2 = database.EventWitnesses.GroupBy(x => x.EventId).Select(x => new WitnessAmountDto { EventID = x.Key, WitnessAmount = x.Count(y => y.Witness == Convert.ToByte(1)), NotWitness = x.Count(y => y.Witness == Convert.ToByte(2)) });

            var result = from o in newEvents
                         join p in right2
                         on o.EventId equals p.EventID
                         into groupjoin
                         from a in groupjoin.DefaultIfEmpty()
                         select new InstantEventOutputDto()
                         {
                             EventId = o.EventId,
                             UploaderAccountId = o.UploaderAccountId,
                             UploaderPropicLink = o.UploaderPropicLink,
                             Type = o.Type,
                             Title = o.Title,
                             Description = o.Description,
                             Longitude = o.Longitude,
                             Latitude = o.Latitude,
                             LocationDetails = o.LocationDetails,
                             UploadTime = o.UploadTime,
                             ShotLink = o.ShotLink,
                             UploaderAccountName = o.UploaderAccountName,
                             IsWitness = o.IsWitness,
                             TotalNotWitness = a.NotWitness == null ? 0 : a.NotWitness,
                             TotalWitness = a.WitnessAmount == null ? 0 : a.WitnessAmount
                         };

            return result.ToList();
        }

        /// <summary>
        /// 指定會員所上傳過的事件
        /// </summary>
        /// <param name="accountID">指定會員帳號</param>
        /// <returns>事件List</returns>
        public List<InstantEvent> getDataByAccount(string accountID)
        {
            var result = database.InstantEvents.Where(x => x.AccountId == accountID);
            return result.ToList();
        }

        /// <summary>
        /// 新增事件至資料庫
        /// </summary>
        /// <param name="input">事件</param>
        public ActionResult UploadData(InstantEventInputDto input)
        {

            var result = new InstantEvent()
            {
                EventId = Guid.NewGuid(),
                Description = input.Description,
                ShotLink = input.ShotLink,
                LocationDetails = input.LocationDetails,
                AccountId = input.AccountId,
                Title = input.Title,
                UploadTime = input.UploadTime,
                Longitude = input.Longitude,
                Latitude = input.Latitude,
                Type = input.Type
            };
            database.InstantEvents.Add(result);
            try
            {
                database.SaveChanges();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// 刪除超過兩天的事件
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteExpiredData()
        {
            var deleteEvent = database.InstantEvents.Where(x => x.UploadTime < DateTime.Today.AddDays(-2)).ToList();
            var deleteWitnee = new List<EventWitness>();
            var deleteDiscussion = new List<EventDiscussion>();
            foreach (var eventID in deleteEvent)
            {
                deleteWitnee.AddRange(database.EventWitnesses.Where(x => x.EventId == eventID.EventId));
                deleteDiscussion.AddRange(database.EventDiscussions.Where(x => x.EventId == eventID.EventId));
            }

            database.EventWitnesses.RemoveRange(deleteWitnee);
            database.EventDiscussions.RemoveRange(deleteDiscussion);
            database.InstantEvents.RemoveRange(deleteEvent);

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
    }
}
