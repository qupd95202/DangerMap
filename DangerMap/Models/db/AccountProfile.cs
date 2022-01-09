using DangerMap.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class AccountProfile
    {
        public AccountProfile()
        {
            EventDiscussions = new HashSet<EventDiscussion>();
            EventWitnesses = new HashSet<EventWitness>();
            InstantEvents = new HashSet<InstantEvent>();
        }
        public string AccountId { get; set; }
        public string Password { get; set; }
        [AccountName]
        public string AccountName { get; set; }
        [EmailAddress]
        public string AccountEmail { get; set; }
        public bool Validation { get; set; }
        [Url]
        public string PropicLink { get; set; }
        public string RefreshToken { get; set; }
        public string NotificationToken { get; set; }

        public virtual ICollection<EventDiscussion> EventDiscussions { get; set; }
        public virtual ICollection<EventWitness> EventWitnesses { get; set; }
        public virtual ICollection<InstantEvent> InstantEvents { get; set; }
    }
}
