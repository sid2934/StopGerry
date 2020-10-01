using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class Race
    {
        public Race()
        {
            Result = new HashSet<Result>();
            VoterTurnout = new HashSet<VoterTurnout>();
        }

        public Guid Id { get; set; }
        public Guid Countyelectionid { get; set; }
        public int Racetypeid { get; set; }

        public virtual CountyElection Countyelection { get; set; }
        public virtual RaceType Racetype { get; set; }
        public virtual ICollection<Result> Result { get; set; }
        public virtual ICollection<VoterTurnout> VoterTurnout { get; set; }
    }
}
