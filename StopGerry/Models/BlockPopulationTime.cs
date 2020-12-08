using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class BlockPopulationTime
    {
        public BlockPopulationTime()
        {
            Demographic = new HashSet<Demographic>();
        }

        public Guid Id { get; set; }
        public string BlockId { get; set; }
        public DateTime ReportingDate { get; set; }
        public int Population { get; set; }

        public virtual ICollection<Demographic> Demographic { get; set; }
    }
}
