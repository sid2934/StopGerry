using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Result
    {
        public Guid Id { get; set; }
        public Guid Electionraceid { get; set; }
        public int Candidateid { get; set; }
        public int Numberofvotesrecieved { get; set; }
        public string Source { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual Electionrace Electionrace { get; set; }
    }
}
