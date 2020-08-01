using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class BlockCountyTime
    {
        public Guid Id { get; set; }
        public string BlockId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string CountyId { get; set; }

        public virtual Block Block { get; set; }
        public virtual County County { get; set; }
    }
}
