using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class VoterTurnout
    {
        public Guid Id { get; set; }
        public int RegisteredVoters { get; set; }
        public int? TotalVoters { get; set; }
        public Guid RaceId { get; set; }

        public virtual Race Race { get; set; }
    }
}
