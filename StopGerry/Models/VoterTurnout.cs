using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class VoterTurnout
    {
        public Guid Id { get; set; }
        public int Registeredvoters { get; set; }
        public int? Totalvoters { get; set; }
        public Guid Electionraceid { get; set; }

        public virtual Electionrace Electionrace { get; set; }
    }
}
