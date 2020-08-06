using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class CountyElection
    {
        public CountyElection()
        {
            Race = new HashSet<Race>();
        }

        public Guid Id { get; set; }
        public string CountyId { get; set; }
        public string Description { get; set; }
        public DateTime ElectionDate { get; set; }
        public int ElectionTypeId { get; set; }

        public virtual County County { get; set; }
        public virtual ElectionType ElectionType { get; set; }
        public virtual ICollection<Race> Race { get; set; }
    }
}
