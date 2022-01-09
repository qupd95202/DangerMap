using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 留言之回傳資料
    /// </summary>
    public class DiscussionOutputDto
    {   
        /// <summary>
        /// 留言者帳號
        /// </summary>
        public string AccountId { get; set; }
        /// <summary>
        /// 留言者暱稱
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 留言者大頭
        /// </summary>
        public string PropicLink { get; set; }
        /// <summary>
        /// 留言時間
        /// </summary>
        public DateTime CommentTime { get; set; }
        /// <summary>
        /// 劉言內容
        /// </summary>
        public string Comment { get; set; }
    }
}
