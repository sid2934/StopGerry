using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class BlockCountyTime
    {
        public Guid Id { get; set; }
        public string Blockid { get; set; }
        public DateTime Timestart { get; set; }
        public DateTime? Timeend { get; set; }
        public string Countyid { get; set; }

        public virtual Block Block { get; set; }
        public virtual County County { get; set; }
    }
}
