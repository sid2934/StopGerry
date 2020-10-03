using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class ElectionraceType
    {
        public ElectionraceType()
        {
            Electionrace = new HashSet<Electionrace>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Positionlevelid { get; set; }

        public virtual PositionLevel Positionlevel { get; set; }
        public virtual ICollection<Electionrace> Electionrace { get; set; }
    }
}
