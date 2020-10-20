using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class CountyTime
    {
        public Guid Id { get; set; }
        public string Countyid { get; set; }
        public DateTime Timestart { get; set; }
        public DateTime? Timeend { get; set; }
        public int Stateid { get; set; }

        public virtual County County { get; set; }
        public virtual State State { get; set; }
    }
}
