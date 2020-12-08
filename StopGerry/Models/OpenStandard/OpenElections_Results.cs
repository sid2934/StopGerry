using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using StopGerry.Utilities;

namespace StopGerry.Models.OpenStandard
{
    public class OpenElections_Results
    {
        public string county { get; set; }
        public string precinct { get; set; }
        public string office { get; set; }
        public string district { get; set; }
        public string party { get; set; }
        public string candidate { get; set; }
        public string votes { get; set; }



        private OpenElections_Info Info { get; set; }

        private State LinkedState { get; set; }
        private County LinkedCounty { get; set; }

        private Candidate LinkedCandidate { get; set; }

        private District LinkedDistrict { get; set; }

        private Electionrace LinkedElectionrace { get; set; }


        private bool FullLinkToDatabase(stopgerryContext dbContext)
        {


            if (LinkedCandidate == null)
            {
                LinkCandidateData(dbContext);
            }
            if (LinkedElectionrace == null)
            {
                LinkElectionData(dbContext);
            }
            if (LinkedDistrict == null)
            {
                LinkLocationData(dbContext);
            }

            return true;

        }

        private void LinkCandidateData(stopgerryContext dbContext)
        {

            //Look for a matching candidate, if one is not found create it
            LinkedCandidate = dbContext.Candidate.Where(c => c.Name == candidate && c.Party == party).FirstOrDefault();
            if (LinkedCandidate == null)
            {
                LinkedCandidate = new Candidate()
                {
                    Name = candidate,
                    Party = party,
                };
                SimpleLogger.Info($"Candidate was not found. Created new record {ObjectDumper.Dump(LinkedCandidate)}");
                dbContext.Add(LinkedCandidate);
                dbContext.SaveChanges(); //Changes need to be saved otherwise the foreign keys complain later
            }

        }

        private void LinkElectionData(stopgerryContext dbContext)
        {
            //Find the linked State
            //ToDo: DO NOT DO A DATABASE LOOK UP READ ALL DATA INTO A HASHTABLE OR DICTIONARY AND GET IT FROM THERE
            LinkedState = dbContext.State.Where(s => s.Abbreviation == Info.StateAbbreviation).FirstOrDefault();
            if (LinkedState == null)
            {
                throw new ArgumentException($"No state with the abbreviation {Info.StateAbbreviation} could be found in the database");
            }

            //Find the linked County
            //ToDo: DO NOT DO A DATABASE LOOK UP READ ALL DATA INTO A HASHTABLE OR DICTIONARY AND GET IT FROM THERE
            LinkedCounty = dbContext.County.ToList().Where(c => Convert.ToInt32(c.Id.Slice(0, 2)) == LinkedState.Id && c.Description.ToLower() == county.ToLower()).FirstOrDefault();
            if (LinkedCounty == null)
            {
                throw new ArgumentException($"Could not find an existing match for this county {county}");
            }

        }

        private void LinkLocationData(stopgerryContext dbContext)
        {
            if (string.IsNullOrWhiteSpace(district))
            {
                LinkedDistrict = null;
            }
            else
            {
                //First reconstruct the format for the district id
                //{stateFips}{District #} !! with leading zerons
                string evaluatedDistrictCode = LinkedState.Id.ToString("00") + Convert.ToInt32(Regex.Match(district, @"\d+").Value).ToString("000");

                if (evaluatedDistrictCode.Length != 5)
                {
                    SimpleLogger.Error($"While trying to find the linked district there was an error parsing the district code.\n\t Calculated District code <stateFips><District #> was {evaluatedDistrictCode}");
                }
                else
                {
                    LinkedDistrict = dbContext.District.Where(d => d.DistrictCode == evaluatedDistrictCode).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// This method is used to find the Electionrace, Candidate, and District associcated with this result
        /// </summary>
        /// <returns></returns>
        private Tuple<Electionrace, Candidate, District> GetMinimalTuple()
        {
            //We no nothing
            return new Tuple<Electionrace, Candidate, District>(LinkedElectionrace, LinkedCandidate, LinkedDistrict);
        }


        public Tuple<Electionrace, Candidate, District> SaveToDB(OpenElections_Info electionInfo, stopgerryContext dbContext, Tuple<Electionrace, Candidate, District> shortcut = null)
        {
            Info = electionInfo;
            if (shortcut != null)
            {
                LinkedElectionrace = shortcut.Item1;
                LinkedCandidate = shortcut.Item2;
                LinkedDistrict = shortcut.Item3;
            }

            if (FullLinkToDatabase(dbContext))
            {
                //Create the county election id if not exists
                if (true == true)
                {
                    dbContext.Result.Add(new Result()
                    {
                        //Todo Actually insert the new result
                        // Id = Guid.NewGuid(),
                        // CandidateId = LinkedCandidate.Id,
                        // NumberOfVotesRecieved = int.Parse(votes, NumberStyles.AllowThousands),
                        // ElectionRaceId = LinkedElectionrace.Id,
                        // ResultResolution = Info.ResultsResolution,
                        // Precinct = Info.ResultsResolution == "precinct" ? precinct : null,
                        // DistrictCode = LinkedDistrict != null ? LinkedDistrict.Districtcode : null,
                        // Source = Info.Url,
                    });
                }
                else
                {
                    SimpleLogger.Info($"Skipped the record due to a matching record existing {ObjectDumper.Dump(this)}\n");
                }
            }
            else
            {
                SimpleLogger.Error($"Failed to link the election data {ObjectDumper.Dump(this)}\n");
            };
            dbContext.SaveChanges();

            return GetMinimalTuple();
            //Check if this election already exists.

        }
    }
}