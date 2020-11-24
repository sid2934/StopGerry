using System;
using System.Collections.Generic;
using StopGerry.Models;

namespace StopGerry.Research
{
    /// <summary>
    /// Provides utilities to generate the efficiency gap for a given District
    /// </summary>
    public class EfficiencyGap
    {
        public double GenerateEfficiencyGap(District d)
        {
            throw new NotImplementedException();
        } 


        /// <summary>
        /// This method will search for all results that are associated with this district
        /// </summary>
        /// <returns></returns>
        private List<Result> GetElectionInfo()
        {
            /*
            *   Districts are tied to a specific time.
            *   This means that we need to look for only elections that happend within the districts time frame
            */
            var dbContext = new stopgerryContext(); 

            return null;
        }

        private void CalculatedWastedVotes()
        {

        }
    }
}