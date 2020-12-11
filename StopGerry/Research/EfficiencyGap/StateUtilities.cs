using System;
using System.Collections.Generic;
using System.Linq;
using StopGerry.Models;
using StopGerry.Utilities;
using static StopGerry.Research.EfficiencyGap.DistrictsUtilities;

namespace StopGerry.Research.EfficiencyGap
{
    public class StateEfficiencyInfo
    {

        /// <summary>
        /// A list of aggregate result data by party
        /// </summary>
        /// <value>A list of aggregate result data by party</value>
        public List<PartyAggregateResult> PartyAggregateList { get; set; }

        /// <summary>
        /// ToDo: This is currently only the total from the top two parties NOT all parties
        /// </summary>
        /// <value>The sum of the votes cast for the top two parties in the district in given election</value>
        public int TotalVotes { get; set; }


        /// <summary>
        /// The winner of this election (The party with the most votes)
        /// </summary>
        /// <value>Aggregate data containing the winners information</value>
        public PartyAggregateResult Winner
        {
            get
            {
                return PartyAggregateList?[0];
            }
        }


        public PartyAggregateResult RunnerUp
        {
            get
            {
                return PartyAggregateList?[1];
            }
        }

    }

    public class StateUtilities
    {
        public static Dictionary<string,Dictionary<string, StateEfficiencyInfo>> ProcessStates(string states, DateTime electionDate, string source, string[] districtTypes)
        {

            var returnDictionary = new Dictionary<string,Dictionary<string, StateEfficiencyInfo>>();
            var stateList = states.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();

            foreach(var state in stateList)
            {
                returnDictionary.Add(state, CalculateStateEG(state, electionDate, source, districtTypes));
            }

            return returnDictionary;
        }


        // <summary>
        /// Calculates the Efficiency Gap of the given district/office using data from the given election date and source
        /// </summary>
        /// <param name="stateAbbreviation">The desired state's abbreviation</param>
        /// <param name="electionDate">The data of the election to use data from</param>
        /// <param name="source">The desired data source. Prevents the same election data from being loaded and counted twice</param>
        /// <returns>Returns an object containing summary data from the calculation process</returns>
        public static Dictionary<string, StateEfficiencyInfo> CalculateStateEG(string stateAbbreviation, DateTime electionDate, string source, string[] districtTypes = null)
        {
            //handle default parameter
            if(districtTypes == null)
            {
                districtTypes = new[] { "U.S. House", "State House of Representatives", "State Senate" };
            }

            var dbContext = new stopgerryContext();
            var givenState = dbContext.State.Where(s => s.Abbreviation == stateAbbreviation).FirstOrDefault();
            if (givenState == null)
            {
                throw new Exception($"State with the abbreivation of {stateAbbreviation} could not be found");
            }

            //There are three different district types
            //  1) State House
            //  2) State Senate
            //  3) U.S. House
            //Each of these should be calculated independently of one another 

            //To calculate each of the three district types' EG we must
            //  1) Process each district of type X and keep results on hand
            //  2) Sum up all votes from the two parties with the most votes
            //  3) Sum up all of the wasted votes and efficient votes
            //  4) This should allow us to get a overall idea of district type X's plan

            var returnDictionary = new Dictionary<string, StateEfficiencyInfo>();
            foreach (var currentDistrictType in districtTypes)
            {

                //Query for the disticnt district of this district type
                var districtsOfDistrictType = dbContext.Result.Where
                (
                    r =>
                    r.DistrictCode.Substring(0, 2) == givenState.Id.ToString()
                    && r.ElectionDate == electionDate
                    && r.Source == source
                    && r.Office == currentDistrictType
                ).Select
                (
                    r => new
                    {
                        r.DistrictCode,
                        r.Office,
                        r.ElectionDate,
                        r.Source
                    }
                ).Distinct().ToList();

                //  1) Process each district of type X and keep results on hand
                //For each distict of current type det the DistrictEfficiencyInfo
                var districtEfficiencyInfos = new List<DistrictEfficiencyInfo>();


                //localPartyAggregateList creates a single sum of each parties aggregate. An aggregate of the district aggregates
                var localPartyAggregateList = new List<PartyAggregateResult>();


                foreach (var district in districtsOfDistrictType)
                {


                    var effInfo = CalculateDistrictEG(district.DistrictCode, district.Office, district.ElectionDate, district.Source);
                    foreach (var partyAggregate in effInfo.PartyAggregateList)
                    {

                        //If the localPartyAggregateList does not have a 
                        PartyAggregateResult refToCorrectPartyAggregate;
                        if ((refToCorrectPartyAggregate = localPartyAggregateList.Where(l => l.Party == partyAggregate.Party).FirstOrDefault()) == null)
                        {
                            refToCorrectPartyAggregate = new PartyAggregateResult()
                            {
                                Party = partyAggregate.Party,
                                WastedVotes = 0
                            };

                            localPartyAggregateList.Add(refToCorrectPartyAggregate);
                        }

                        //Add the votes (and wasted votes) for this district to the localPartyAggregateList
                        refToCorrectPartyAggregate.Votes += partyAggregate.Votes;
                        refToCorrectPartyAggregate.WastedVotes += partyAggregate.WastedVotes;
                    }
                }

                localPartyAggregateList.Sort((a, b) => b.Votes.CompareTo(a.Votes));
                var topParty = localPartyAggregateList[0];
                var secondParty = localPartyAggregateList[1];

                int totalVotes = topParty.Votes + secondParty.Votes;

                double topWastedPercentage = (double)(topParty.WastedVotes / (double)topParty.Votes);
                double secondWastedPercentage = (double)(secondParty.WastedVotes / (double)secondParty.Votes);

                returnDictionary.Add(currentDistrictType, new StateEfficiencyInfo()
                {
                    PartyAggregateList = localPartyAggregateList,
                    TotalVotes = totalVotes

                });
            }
            return returnDictionary;
        }
    }
}