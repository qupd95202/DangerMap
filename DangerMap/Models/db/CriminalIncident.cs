using System;
using System.Collections.Generic;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class CriminalIncident
    {
        public int IncidentId { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
    }
}
