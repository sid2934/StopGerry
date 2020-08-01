using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class BlockPopulationTime
    {
        public Guid Id { get; set; }
        public string BlockId { get; set; }
        public DateTime ReportingDate { get; set; }
        public int Population { get; set; }
    }
}
