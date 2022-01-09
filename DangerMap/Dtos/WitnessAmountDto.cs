using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{
    public class WitnessAmountDto
    {
        public Guid EventID { get; set; }

        public int WitnessAmount { get; set; }

        public int NotWitness { get; set; }

    }
}
