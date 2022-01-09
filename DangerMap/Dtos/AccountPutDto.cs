
using DangerMap.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 修改【暱稱或大頭貼】
    /// </summary>
    public class AccountPutDto
    {
        public string AccountName { get; set; }
        [Url]
        public string PropicLink { get; set; }
    }
}
