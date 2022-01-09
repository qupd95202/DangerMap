using DangerMap.Dtos;
using DangerMap.Models.db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Services
{
    public class NewsTickerService
    {
        /// <summary>
        /// 資料庫
        /// </summary>
        private readonly uploadtestv1Context database;
        public NewsTickerService(uploadtestv1Context db)
        {
            database = db;
        }

        /// <summary>
        /// 全部跑馬燈資料
        /// </summary>
        /// <returns>事件List</returns>
        public List<NewsTicker> GetAllNews()
        {
            return database.NewsTickers.ToList();
        }

        /// <summary>
        /// 輸入一筆跑馬燈資料
        /// </summary>
        /// <returns>事件List</returns>
        public async Task<ActionResult> PutNews(NewsTickerDto news)
        {
            var result = new NewsTicker()
            {           
               Title = news.Title,               
            };

            await database.NewsTickers.AddAsync(result);          

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

        /// <summary>
        /// 刪除所有跑馬燈資料
        /// </summary>
        /// <returns>事件List</returns>
        public void DeleteAllNews()
        {
            var delete = from a in database.NewsTickers
                         select a;

            if (delete != null)
            {
                foreach (var item in delete)
                {
                    database.NewsTickers.Remove(item);       
                }         
            }
            database.SaveChanges();
        }
    }
}
