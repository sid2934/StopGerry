using System;

namespace StopGerry.Models.OpenStandard
{
    public class OpenElections_Info
    {

        public string Url {get; set;}
        
        public DateTime ElectionDate { get; set; }

        public string StateAbbreviation { get; set; }

        public string ElectionType { get; set; }

        public string ResultsResolution { get; set; }

        public string FileName { get; set; }
    }
}