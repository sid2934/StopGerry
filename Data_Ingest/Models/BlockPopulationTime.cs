using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class BlockPopulationTime
    {
        public BlockPopulationTime()
        {
            Demographic = new HashSet<Demographic>();
        }

        public Guid Id { get; set; }
        public string Blockid { get; set; }
        public DateTime Reportingdate { get; set; }
        public int Population { get; set; }

        public virtual ICollection<Demographic> Demographic { get; set; }
    }
}
