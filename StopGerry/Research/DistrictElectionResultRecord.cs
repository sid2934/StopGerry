using System;

namespace StopGerry.Research
{
    internal class DistrictElectionResultRecord
    {
        public string DistrictCode { get; set; }

        public DateTime ElectionDate { get; set; }

        public string Office { get; set; }

        public string Source { get; set; }

        public string Party { get; set; }

        public int NumberOfVotesRecieved { get; set; }

        public string ResultResolution { get; set; }
    }
}
