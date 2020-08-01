using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class BlockDistrictTime
    {
        public Guid Id { get; set; }
        public string BlockId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string DistrictId { get; set; }

        public virtual Block Block { get; set; }
        public virtual District District { get; set; }
    }
}
