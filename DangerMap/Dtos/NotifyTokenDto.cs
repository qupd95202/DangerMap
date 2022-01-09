using DangerMap.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{
    public class NotifyTokenDto
    {
        /// <summary>
        /// 要操作的帳戶ID
        /// </summary>
        [AccountID]
        public string AccountId { get; set; }

        /// <summary>
        /// 存入的通知推播裝置ID TOKEN
        /// </summary>
        public string NotificationToken { get; set; }
    }
}
