using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class Result
    {
        public Guid Id { get; set; }
        public Guid RaceId { get; set; }
        public int CandidateId { get; set; }
        public int NumberOfVotesRecieved { get; set; }
        public string Source { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual Race Race { get; set; }
    }
}
