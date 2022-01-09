using System;
using System.Collections.Generic;

#nullable disable

namespace DangerMap.Models.db
{
    public partial class TrafficAccident
    {
        public int AccidentId { get; set; }
        public DateTime Time { get; set; }
        public short Death { get; set; }
        public short Injury { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
