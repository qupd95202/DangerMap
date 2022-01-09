using DangerMap.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 改密碼專用
    /// </summary>
    public class AccountPasswordPutDto
    {
        
        public string OldPassword { get; set; }
        
        public string NewPassword { get; set; }
    }
}
