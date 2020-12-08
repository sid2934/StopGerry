using System;
using System.Collections.Generic;

namespace StopGerry.Models.OpenStandard
{
    public class OpenElections_Info
    {

        public string Url { get; set; }

        public DateTime ElectionDate { get; set; }

        public State State { get; set; }

        /// <summary>
        /// Used to reduce the number of database queries 
        /// The key is the county name
        /// </summary>
        /// <value>CountyId (FIPS)</value>
        public Dictionary<string,string> CountyDictionary { get; set;}

        public string ElectionType { get; set; }

        public string ResultsResolution { get; set; }

        public string FileName { get; set; }
    }
}