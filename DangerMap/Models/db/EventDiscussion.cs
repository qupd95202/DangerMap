using System;
using System.Collections.Generic;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class EventDiscussion
    {
        public Guid EventId { get; set; }
        public string AccountId { get; set; }
        public DateTime CommentTime { get; set; }
        public string Comment { get; set; }

        public virtual AccountProfile Account { get; set; }
        public virtual InstantEvent Event { get; set; }
    }
}
