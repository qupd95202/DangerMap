using DangerMap.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 登入帳號密碼
    /// </summary>
    public class AccountLoginDto
    {
        [AccountID]
        public string AccountId { get; set; }
                
        public string Password { get; set; }
    }
}
