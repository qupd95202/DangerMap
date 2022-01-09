using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 會員資訊
    /// </summary>
    public class AccountProfileDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string AccountEmail { get; set; }

        /// <summary>
        /// 大頭貼URL
        /// </summary>
        public string PropicLink { get; set; }

        /// <summary>
        /// 是否驗證了，1=是，0=否
        /// </summary>
        public bool Validation { get; set; }
    }
}
