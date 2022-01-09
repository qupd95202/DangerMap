using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{
    public class IncidentOutPutDto
    {
        /// <summary>
        /// 行政區
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 搶奪
        /// </summary>
        public int Snatch { get; set; }
        /// <summary>
        /// 強制性交
        /// </summary>
        public int Rape { get; set; }
        /// <summary>
        /// 竊盜
        /// </summary>
        public int Rob { get; set; }
        /// <summary>
        /// 汽車竊盜
        /// </summary>
        public int CarTheft { get; set; }
        /// <summary>
        /// 機車竊盜
        /// </summary>
        public int ScooterTheft { get; set; }
        /// <summary>
        /// 毒品
        /// </summary>
        public int Drug { get; set; }
        /// <summary>
        /// 住宅竊盜
        /// </summary>
        public int HouseTheft { get; set; }
    }
}
