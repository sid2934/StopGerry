using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class Demographic
    {
        public Guid Id { get; set; }
        public Guid PopulationTimeId { get; set; }

        public virtual BlockPopulationTime PopulationTime { get; set; }
    }
}
