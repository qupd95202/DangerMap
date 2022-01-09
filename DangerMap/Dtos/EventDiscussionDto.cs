using DangerMap.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{   
    /// <summary>
    /// 留言板資訊
    /// </summary>
    public class EventDiscussionDto
    {
        public Guid EventId { get; set; }
        [AccountID]
        public string AccountId { get; set; }
        public DateTime CommentTime { get; set; }
        [UserMessage]
        public string Comment { get; set; }
    }
}
