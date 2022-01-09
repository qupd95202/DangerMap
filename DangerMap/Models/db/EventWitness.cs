using System;
using System.Collections.Generic;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class EventWitness
    {
        public Guid EventId { get; set; }
        public string AccountId { get; set; }
        public byte Witness { get; set; }

        public virtual AccountProfile Account { get; set; }
        public virtual InstantEvent Event { get; set; }
    }
}
