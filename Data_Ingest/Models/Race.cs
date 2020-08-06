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
        public Guid CountyElectionId { get; set; }
        public int RaceTypeId { get; set; }

        public virtual CountyElection CountyElection { get; set; }
        public virtual RaceType RaceType { get; set; }
        public virtual ICollection<Result> Result { get; set; }
        public virtual ICollection<VoterTurnout> VoterTurnout { get; set; }
    }
}
