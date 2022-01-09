using DangerMap.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.Dtos
{
    public class NewsTickerDto
    {   
        public string Title { get; set; }
        [Url]
        public string InfoLink { get; set; }
    }
}
