using DangerMap.Dtos;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class EventDiscussionService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;

        public EventDiscussionService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 取得指定事件之所有留言
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <returns>留言List</returns>
        public List<DiscussionOutputDto> GetAllDatas(Guid eventID)
        {
            var ids = database.EventDiscussions.Where(x => x.EventId == eventID).Select(x => x);
            var joins = ids.Join(database.AccountProfiles, x => x.AccountId, y => y.AccountId, (x, y) => new DiscussionOutputDto()
            {
                AccountId = x.AccountId,
                AccountName = y.AccountName,
                PropicLink = y.PropicLink,
                Comment = x.Comment,
                CommentTime = x.CommentTime
            });
            var result = joins.OrderBy(x => x.CommentTime).ToList();
            return result;
        }

        /// <summary>
        /// 取得指定事件之固定數量留言
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="amount">數量</param>
        /// <returns>留言List</returns>
        public List<DiscussionOutputDto> GetDatas(Guid eventID, int amount)
        {
            var ids = database.EventDiscussions.Where(x => x.EventId == eventID).Select(x => x).OrderBy(x => x.CommentTime).Take(amount);
            var joins = ids.Join(database.AccountProfiles, x => x.AccountId, y => y.AccountId, (x, y) => new DiscussionOutputDto()
            {
                AccountId = x.AccountId,
                AccountName = y.AccountName,
                PropicLink = y.PropicLink,
                Comment = x.Comment,
                CommentTime = x.CommentTime
            });
            var result = joins.OrderBy(x => x.CommentTime).ToList();
            return result;
        }

        /// <summary>
        /// 新增一筆留言
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <returns>留言List</returns>
        public async Task<ActionResult> PutOneData(EventDiscussionDto comment)
        {
            var result = new EventDiscussion()
            {
                EventId = comment.EventId,
                AccountId = comment.AccountId,
                CommentTime = comment.CommentTime,
                Comment = comment.Comment
            };

            await database.EventDiscussions.AddAsync(result);

            try
            {
                database.SaveChanges();
                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }
    }
}
