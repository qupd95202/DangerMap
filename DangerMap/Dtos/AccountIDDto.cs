using DangerMap.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 操作帳號之指派
    /// </summary>
    public class AccountIDDto
    {   
        /// <summary>
        /// 要操作的帳戶ID
        /// </summary>
        [AccountID]
        public string AccountId { get; set; }
    }
}
