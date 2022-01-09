using DangerMap.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 換發Token專用
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [AccountID]
        public string AccountID { get; set; }
        /// <summary>
        /// 舊的Token
        /// </summary>
        public string Token { get; set; }
    }
}
