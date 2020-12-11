using System;
using System.Collections.Generic;
using System.Linq;
using StopGerry.Models;
using StopGerry.Utilities;

namespace StopGerry.Research.EfficiencyGap
{




    public class DistrictsUtilities
    {

        public class PartyAggregateDistrictResult
        {
            public string Party { get; set; }

            public int Votes { get; set; } = 0;

            public int? WastedVotes { get; set; }

            public int? EfficientVotes
            {
                get
                {
                    return WastedVotes == null ? null : Votes - WastedVotes;
                }
            }

            public bool? Win { get; set; }

        }

        
        public class DistrictEfficiencyInfo
        {

            /// <summary>
            /// A list of aggregate result data by party
            /// </summary>
            /// <value>A list of aggregate result data by party</value>
            public List<PartyAggregateDistrictResult> PartyAggregateList { get; set; }

            /// <summary>
            /// ToDo: This is currently only the total from the top two parties NOT all parties
            /// </summary>
            /// <value>The sum of the votes cast for the top two parties in the district in given election</value>
            public int TotalVotes { get; set; }

            /// <summary>
            /// The calculated winning threshold totalVotes / 2
            /// </summary>
            /// <value></value>
            public int WinningThreshold { get; set; }

            /// <summary>
            /// Calculated Efficieny Gap of this district. Negative value means that the race winner had an advantage
            /// </summary>
            /// <value>double such that abs(EfficiencyGap) <= 1. </value>
            public double EfficiencyGap { get; set; }

            /// <summary>
            /// The winner of this election (The party with the most votes)
            /// </summary>
            /// <value>Aggregate data containing the winners information</value>
            public PartyAggregateDistrictResult Winner
            {
                get
                {
                    return PartyAggregateList?[0];
                }
            }


            public PartyAggregateDistrictResult RunnerUp
            {
                get
                {
                    return PartyAggregateList?[1];
                }
            }

        }


        /// <summary>
        /// Calculates the Efficiency Gap of the given district/office using data from the given election date and source
        /// </summary>
        /// <param name="districtCode">The District code of the desired district {State Fips}+{District Number} </param>
        /// <param name="office">The Office of the desired district, this ensure that the correct district type is examined. Otherwise 1st Senate and 1st House districts can collide</param>
        /// <param name="electionDate">The data of the election to use data from</param>
        /// <param name="source">The desired data source. Prevents the same election data from being loaded and counted twice</param>
        /// <returns>Returns an object containing summary data from the calculation process</returns>
        public static DistrictEfficiencyInfo CalculateDistrictEG(string districtCode, string office, DateTime electionDate, string source)
        {
            var affregateDistrictResults = GetAggregateDistrictResults(districtCode, office, electionDate, source);

            //Data need for EG calculation
            int totalVotes = 0;
            int winningThreshold;
            double eg;

            //For now I will be completely ignoring anything outside of the top two parties

            affregateDistrictResults.Sort((a, b) => b.Votes.CompareTo(a.Votes));
            var topParty = affregateDistrictResults[0];
            var secondParty = affregateDistrictResults[1];

            totalVotes = topParty.Votes + secondParty.Votes;
            winningThreshold = totalVotes / 2;

            bool tieWinnerOverride = false;
            if (topParty.Votes == secondParty.Votes)
            {
                SimpleLogger.Warning($"There was a tie in the election for district:\n{districtCode}, Office:{office}, ElectionDate: {electionDate}, Source: {source}");
                tieWinnerOverride = true;
            }


            topParty.WastedVotes = topParty.Votes - winningThreshold;
            topParty.Win = true ^ tieWinnerOverride;

            secondParty.WastedVotes = secondParty.Votes;
            secondParty.Win = false;

            //Negative value means that the race winner had an advantage
            eg = (double)((topParty.WastedVotes - secondParty.WastedVotes) / (double)totalVotes);


            return new DistrictEfficiencyInfo()
            {
                EfficiencyGap = eg,
                PartyAggregateList = affregateDistrictResults,
                TotalVotes = totalVotes,
                WinningThreshold = winningThreshold
            };

        }


        /// <summary>
        /// Queries the database for election results that match the given critera and retuns an aggregate vote count for each party
        /// </summary>
        /// <param name="districtCode">The disctict code {State Fips}+{District Number} to look for</param>
        /// <param name="office">The office of the given district. Needed to avoid district number colisions </param>
        /// <param name="electionDate">The date of the election to look at</param>
        /// <param name="source">The source of election results to use. Avoids getting the same results from different sources at the same time</param>
        /// <returns>A list containing a record for each party that earned votes in the given district in the desired election</returns>
        private static List<PartyAggregateDistrictResult> GetAggregateDistrictResults(string districtCode, string office, DateTime electionDate, string source)
        {
            var dbContext = new stopgerryContext();


            return dbContext.Result.Where
            (
                r => r.DistrictCode == districtCode
                && r.Office == office
                && r.ElectionDate == electionDate
                && r.Source == source
            ).Join
                (
                    dbContext.Candidate,
                    r => r.CandidateId,
                    c => c.Id,
                    (r, c) => new
                    {
                        r.DistrictCode,
                        r.ElectionDate,
                        r.Office,
                        r.Source,
                        c.Party,
                        r.NumberOfVotesRecieved,
                        r.ResultResolution

                    }
                ).GroupBy(r => r.Party).Select(r => new PartyAggregateDistrictResult()
                {
                    Party = r.Key,
                    Votes = r.Sum(dr => dr.NumberOfVotesRecieved)
                }).ToList();
        }
    

    }
}