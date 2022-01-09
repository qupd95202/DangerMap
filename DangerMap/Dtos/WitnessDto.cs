using DangerMap.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.Dtos
{
    /// <summary>
    /// 目擊資訊
    /// </summary>
    public class WitnessDto
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public Guid EventId { get; set; }
        /// <summary>
        /// 按是否目擊者之用戶ID
        /// </summary>
        [AccountID]
        public string AccountId { get; set; }
        /// <summary>
        /// 是否目擊 0=沒參與此事件，1=有目擊,2=沒看到
        /// </summary>
        [RegularExpression("[0-2]")]
        public byte IsWitness { get; set; }
    }
}
