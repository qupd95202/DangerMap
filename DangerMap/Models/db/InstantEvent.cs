using System;
using System.Collections.Generic;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class InstantEvent
    {
        public InstantEvent()
        {
            EventDiscussions = new HashSet<EventDiscussion>();
            EventWitnesses = new HashSet<EventWitness>();
        }

        public Guid EventId { get; set; }
        public string AccountId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string LocationDetails { get; set; }
        public DateTime UploadTime { get; set; }
        public string ShotLink { get; set; }

        public virtual AccountProfile Account { get; set; }
        public virtual ICollection<EventDiscussion> EventDiscussions { get; set; }
        public virtual ICollection<EventWitness> EventWitnesses { get; set; }
    }
}
