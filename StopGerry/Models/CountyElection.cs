using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class CountyElection
    {
        public CountyElection()
        {
            Electionrace = new HashSet<Electionrace>();
        }

        public Guid Id { get; set; }
        public string Countyid { get; set; }
        public string Description { get; set; }
        public DateTime Electiondate { get; set; }
        public int Electiontypeid { get; set; }

        public virtual County County { get; set; }
        public virtual ElectionType Electiontype { get; set; }
        public virtual ICollection<Electionrace> Electionrace { get; set; }
    }
}
