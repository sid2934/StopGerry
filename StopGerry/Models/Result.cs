using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Result
    {
        public Guid Id { get; set; }
        public string CountyId { get; set; }
        public string Precinct { get; set; }
        public int OfficeId { get; set; }
        public string DistrictCode { get; set; }
        public int CandidateId { get; set; }
        public int NumberOfVotesRecieved { get; set; }
        public string Source { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual County County { get; set; }
        public virtual Office Office { get; set; }
    }
}
