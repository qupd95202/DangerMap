
using DangerMap.Models.db;
using DangerMap.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.Dtos
{
    /// <summary>
    /// 註冊所需資料
    /// </summary>
    public class AccountProfileInputDto
    {   
        /// <summary>
        /// 帳號 (15個字，只能包含數字英文底線)
        /// </summary>
        [RegisterID]
        public string AccountId { get; set; }

        /// <summary>
        /// 密碼 (只剩暗碼)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 暱稱 (8個字)
        /// </summary>
        [AccountName]
        public string AccountName { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        [EmailAddress]
        public string AccountEmail { get; set; }

        /// <summary>
        /// 大頭貼URL
        /// </summary>
        [Url]
        public string PropicLink { get; set; }
    }
}
