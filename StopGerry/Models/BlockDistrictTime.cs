using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class BlockDistrictTime
    {
        public Guid Id { get; set; }
        public string Blockid { get; set; }
        public DateTime Timestart { get; set; }
        public DateTime? Timeend { get; set; }
        public string Districtid { get; set; }

        public virtual Block Block { get; set; }
        public virtual District District { get; set; }
    }
}
