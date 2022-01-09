using DangerMap.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.Dtos
{
    /// <summary>
    /// 事件資訊
    /// </summary>
    public class InstantEventInputDto
    {
        [AccountID]
        public string AccountId { get; set; }

        /// <summary>
        /// 事件類型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 事件標題 (25字)
        /// </summary>
        [EventTitle]
        public string Title { get; set; }

        /// <summary>
        /// 事件描述 (200字)
        /// </summary>
        [EventDescription]
        public string Description { get; set; }

        /// <summary>
        /// 事件經度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 事件緯度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 事件地點描述 (25字)
        /// </summary>
        [LocationDetail]
        public string LocationDetails { get; set; }

        /// <summary>
        /// 上傳時間
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 照片URL
        /// </summary>
        [Url]
        public string ShotLink { get; set; }
    }
}
