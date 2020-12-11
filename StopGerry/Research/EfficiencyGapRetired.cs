using System;
using System.Collections.Generic;
using System.Linq;
using StopGerry.Models;
using StopGerry.Utilities;

namespace StopGerry.Research
{

    public record DistrictResultRecord
    {
        public DistrictResultRecord(string districtCode, DateTime electionDate, string office, string source, string party, int numberOfVotesRecieved, string resultResolution)
        {
            DistrictCode = districtCode;
            ElectionDate = electionDate;
            Office = office;
            Source = source;
            Party = party;
            NumberOfVotesRecieved = numberOfVotesRecieved;
            ResultResolution = resultResolution;
        }

        public string DistrictCode { get; set; }

        public DateTime ElectionDate { get; set; }

        public string Office { get; set; }

        public string Source { get; set; }

        public string Party { get; set; }

        public int NumberOfVotesRecieved { get; set; }

        public string ResultResolution { get; set; }
    }


    /// <summary>
    /// Provides utilities to generate the efficiency gap for a given District
    /// </summary>
    public static class EfficiencyGapUtility
    {

        /// <summary>
        /// Will load existing data from the database for the given district and year and calculate the efficiency gap.
        /// Can only operate on a single election and data source at a time
        /// </summary>
        /// <returns></returns>
        public static double GenerateEfficiencyGap(string stateAbbreviation, DateTime electionDate, string source)
        {
            var districtResults = GetElectionInfo(stateAbbreviation, electionDate, source);
            var districtEG = new List<DistrictEfficiencyGap>();


            var stateWidePartyResults = new List<PartyVoteCounter>();
            int stateWideTotalVotes = 0;





            //get a list of unique district codes from the resulting set
            foreach (var uniqueDistrict in districtResults.Select(d => new { d.DistrictCode, d.ElectionDate, d.Office, d.Source, d.ResultResolution }).Distinct())
            {
                var neededResults = districtResults.Where(
                        r => r.DistrictCode == uniqueDistrict.DistrictCode
                        && r.ElectionDate == uniqueDistrict.ElectionDate
                        && r.Office == uniqueDistrict.Office
                        && r.Source == uniqueDistrict.Source
                        && r.ResultResolution == uniqueDistrict.ResultResolution
                        ).ToList();

                //add the district to the state totals
                var currentDistrictEG = new DistrictEfficiencyGap(neededResults);
                PartyVoteCounter counter;
                foreach (var partyResult in currentDistrictEG.PartyResults)
                {
                    if ((counter = stateWidePartyResults.Where(c => c.Party == partyResult.Party).FirstOrDefault()) == null)
                    {
                        counter = new PartyVoteCounter(partyResult.Party);
                        stateWidePartyResults.Add(counter);
                    }
                    stateWideTotalVotes += partyResult.Votes;
                    counter.Votes += partyResult.Votes;
                    counter.WastedVotes += partyResult.WastedVotes;
                    counter.Win += partyResult.Win;
                }

            }

            //For now I will only be using the two parties that recieved the most votes
            stateWidePartyResults.Sort((a, b) => b.Votes.CompareTo(a.Votes));
            var topParty = stateWidePartyResults[0];
            var secondParty = stateWidePartyResults[1];
            
            int topPartyWasted = topParty.WastedVotes;
            int topPartyEfficient = topParty.Votes - topParty.WastedVotes;
            int secondPartyWasted = secondParty.WastedVotes;
            int secondPartyEfficient = secondParty.Votes - secondParty.WastedVotes;

            return -1;
        }


        /// <summary>
        /// This method will search for all results that are associated with this district
        /// </summary>
        /// <returns></returns>
        //! ToDo: Add some method arguments to allow for more control of what is being requested
        private static List<DistrictResultRecord> GetElectionInfo(string stateAbbreviation, DateTime electionDate, string source)
        {

            var dbContext = new stopgerryContext();

            var state = dbContext.State.Where(s => s.Abbreviation.ToLower() == stateAbbreviation.ToLower()).FirstOrDefault();
            if (state == null)
            {
                throw new Exception($"State could not be found with abbreviation: {stateAbbreviation}");
            }

            //get all data from a single election
            //Retrieve a list of district results for each district in the state indicated
            // ? This may miss votes case for a party ticket instead of individual candidates 
            return dbContext.Result.Where
                (
                    r => r.DistrictCode.Substring(0, 2) == state.Id.ToString()
                    && r.ElectionDate == electionDate
                    && r.Source == source
                ).Join
                (
                    dbContext.Candidate,
                    r => r.CandidateId,
                    c => c.Id,
                    (r, c) => new DistrictResultRecord
                    (
                        r.DistrictCode,
                        r.ElectionDate,
                        r.Office,
                        r.Source,
                        c.Party,
                        r.NumberOfVotesRecieved,
                        r.ResultResolution

                    )
                ).ToList();

        }



        private class PartyVoteCounter
        {
            public PartyVoteCounter(string party)
            {
                Party = party;
            }

            public string Party { get; set; }

            public int Votes { get; set; } = 0;

            public int WastedVotes { get; set; }

            public int Win = 0;

        }

        private class DistrictEfficiencyGap
        {

            /// <summary>
            /// The calculated efficiency gap of this district for this election
            /// </summary>
            /// <value>A double such that abs(EfficiencyGap) <= 1. Negative value means that the race winner had an advantage </value>
            public double EfficiencyGap { get; set; }

            /// <summary>
            /// A list of party vote counters to keep track of how each party did in the election
            /// </summary>
            /// <value></value>
            public List<PartyVoteCounter> PartyResults { get; set; }

            /// <summary>
            /// Stores the raw district results that are passed to the constructor
            /// </summary>
            public List<DistrictResultRecord> RawDistrictRecords { get; set; }

            /// <summary>
            /// The total number of votes cast in the election (Sum of all DistrictResultRecord.NumberOfVotesCast)
            /// </summary>
            /// <value></value>
            public int TotalVotes { get; set; }

            /// <summary>
            /// Create a new DistrictEfficiencyGap object by calculating the needed information for the Efficiency Gap of the district
            /// </summary>
            /// <param name="districtResults">A list of all district result records a given election and office</param>
            public DistrictEfficiencyGap(List<DistrictResultRecord> districtResults)
            {

                RawDistrictRecords = districtResults;
                int TotalVotes = 0;
                PartyResults = new List<PartyVoteCounter>();


                //calc total votes
                //calc votes per party
                PartyVoteCounter counter;
                foreach (var r in districtResults)
                {
                    if ((counter = PartyResults.Where(c => c.Party == r.Party).FirstOrDefault()) == null)
                    {
                        counter = new PartyVoteCounter(r.Party);
                        PartyResults.Add(counter);
                    }

                    TotalVotes += r.NumberOfVotesRecieved;
                    counter.Votes += r.NumberOfVotesRecieved;

                }

                // ? When does a third party become significant enought to be included

                //For now I will only be using the two parties that recieved the most votes
                PartyResults.Sort((a, b) => b.Votes.CompareTo(a.Votes));
                var topParty = PartyResults[0];
                var secondParty = PartyResults[1];

                // ? Should I use the full total vote or only the sum of the top two parties?
                //For now I will only use the sum of the top two parties
                //! This needs to be changed 
                TotalVotes = topParty.Votes + secondParty.Votes;
                int winThreashold = (TotalVotes / 2) + 1;

                //calc win threshold [min(50%+1,ceil(50%))]
                if (topParty.Votes == secondParty.Votes)
                {
                    SimpleLogger.Warning($"There was a tie in the election for district\n{ObjectDumper.Dump(districtResults[0])}");
                }

                topParty.Win = 1;
                topParty.WastedVotes = topParty.Votes - winThreashold;
                secondParty.WastedVotes = secondParty.Votes;

                double efficiencyGap = (topParty.WastedVotes - secondParty.WastedVotes) / (double)TotalVotes;
                if (efficiencyGap >= 0)
                {
                    SimpleLogger.Info($"Detected a efficency gap of {efficiencyGap} for the district\n{ObjectDumper.Dump(districtResults[0])}");
                }

                this.EfficiencyGap = efficiencyGap;

            }



        }

    }
}