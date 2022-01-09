using DangerMap.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.Dtos
{
    /// <summary>
    /// 事件資訊
    /// </summary>
    public class InstantEventOutputDto
    {
        /// <summary>
        /// 事件 ID
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// 上傳者 ID
        /// </summary>
        public string UploaderAccountId { get; set; }

        /// <summary>
        /// 上傳者暱稱
        /// </summary>
        public string UploaderAccountName { get; set; }

        /// <summary>
        /// 上傳者大頭貼
        /// </summary>
        public string UploaderPropicLink { get; set; }

        /// <summary>
        /// 事件類型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 事件標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 事件敘述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 地點額外描述
        /// </summary>
        public string LocationDetails { get; set; }

        /// <summary>
        /// 上傳時間
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 事件照片
        /// </summary>
        public string ShotLink { get; set; }

        /// <summary>
        /// 該事件總目擊數
        /// </summary>
        public int TotalWitness { get; set; }

        /// <summary>
        /// 該事件總未目擊數
        /// </summary>
        public int TotalNotWitness { get; set; }

        /// <summary>
        /// 是否目擊 0=沒參與此事件，1=有目擊,2=沒看到
        /// </summary>
        public byte IsWitness { get; set; }
    }
}
