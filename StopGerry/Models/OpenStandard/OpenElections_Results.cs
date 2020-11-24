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

        private Party LinkedParty { get; set; }

        private Candidate LinkedCandidate { get; set; }

        private ElectionType LinkedElectionType { get; set; }

        private CountyElection LinkedCountyElection { get; set; }

        private ElectionraceType LinkedElectionraceType { get; set; }

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
            //Look for a matching Party, if one is not found create it
            LinkedParty = dbContext.Party.Where(p => p.Name == party || p.Abbreviation == party).FirstOrDefault();
            if (LinkedParty == null)
            {
                LinkedParty = new Party()
                {
                    Name = party,
                    Abbreviation = party.Length <= 5 ? party : party.Substring(0, 5),
                };
                SimpleLogger.Info($"Party was not found. Created new record {ObjectDumper.Dump(LinkedParty)}");
                dbContext.Add(LinkedParty);
                dbContext.SaveChanges(); //Changes need to be saved otherwise the foreign keys complain later
            }

            //Look for a matching candidate, if one is not found create it
            LinkedCandidate = dbContext.Candidate.Where(c => c.Name == candidate && c.Partyid == LinkedParty.Id).FirstOrDefault();

            if (LinkedCandidate == null)
            {
                LinkedCandidate = new Candidate()
                {
                    Name = candidate,
                    Partyid = LinkedParty.Id,
                };
                SimpleLogger.Info($"Candidate was not found. Created new record {ObjectDumper.Dump(LinkedCandidate)}");
                dbContext.Add(LinkedCandidate);
                dbContext.SaveChanges(); //Changes need to be saved otherwise the foreign keys complain later
            }

        }

        private void LinkElectionData(stopgerryContext dbContext)
        {
            //Find the linked State
            LinkedState = dbContext.State.Where(s => s.Abbreviation == Info.StateAbbreviation).FirstOrDefault();
            if (LinkedState == null)
            {
                throw new ArgumentException($"No state with the abbreviation {Info.StateAbbreviation} could be found in the database");
            }

            //Find the linked County
            LinkedCounty = dbContext.County.ToList().Where(c => Convert.ToInt32(c.Id.Slice(0, 2)) == LinkedState.Id && c.Description.ToLower() == county.ToLower()).FirstOrDefault();
            if (LinkedCounty == null)
            {
                throw new ArgumentException($"Could not find an existing match for this county {county}");
            }

            //Loof for a matching ElectionType Record, If non exist then create one.
            LinkedElectionType = dbContext.ElectionType.Where(et => et.Description == Info.ElectionType).FirstOrDefault();
            if (LinkedElectionType == null)
            {
                LinkedElectionType = new ElectionType
                {
                    Description = Info.ElectionType
                };
                SimpleLogger.Info($"ElectionType was not found. Created new record {ObjectDumper.Dump(LinkedElectionType)}");
                dbContext.Add(LinkedElectionType);
                dbContext.SaveChanges();
            }



            //Loof for a matching CountyElection Record, If non exist then create one.
            LinkedCountyElection =
                            dbContext.CountyElection
                                .Where(ce => ce.Countyid == LinkedCounty.Id &&
                                            ce.Electiondate == Info.ElectionDate
                                    )
                                .FirstOrDefault();
            if (LinkedCountyElection == null)
            {
                LinkedCountyElection = new CountyElection()
                {
                    Id = Guid.NewGuid(),
                    Description = Info.FileName,
                    Electiondate = Info.ElectionDate,
                    Electiontypeid = LinkedElectionType.Id,
                    Countyid = LinkedCounty.Id
                };
                SimpleLogger.Info($"CountyElection was not found. Created new record {ObjectDumper.Dump(LinkedCountyElection)}");
                dbContext.Add(LinkedCountyElection);
                dbContext.SaveChanges();
            }

            //Loof for a matching ElectionraceType Record, If non exist then create one.
            LinkedElectionraceType = dbContext.ElectionraceType.Where(rt => rt.Description == office).FirstOrDefault();
            if (LinkedElectionraceType == null)
            {
                LinkedElectionraceType = new ElectionraceType()
                {
                    Description = office,
                    Positionlevelid = 4
                };
                SimpleLogger.Info($"ElectionraceType was not found. Created new record {ObjectDumper.Dump(LinkedElectionraceType)}");
                dbContext.Add(LinkedElectionraceType);
                dbContext.SaveChanges();
            }


            LinkedElectionrace = dbContext.Electionrace.Where(r => r.Countyelectionid == LinkedCountyElection.Id && r.Electionracetypeid == LinkedElectionraceType.Id).FirstOrDefault();
            if (LinkedElectionrace == null)
            {
                LinkedElectionrace = new Electionrace()
                {
                    Id = Guid.NewGuid(),
                    Electionracetypeid = LinkedElectionraceType.Id,
                    Countyelectionid = LinkedCountyElection.Id
                };
                SimpleLogger.Info($"Electionrace was not found. Created new record {ObjectDumper.Dump(LinkedElectionrace)}");
                dbContext.Add(LinkedElectionrace);
                dbContext.SaveChanges();
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
                    LinkedDistrict = dbContext.District.Where(d => d.Districtcode == evaluatedDistrictCode).FirstOrDefault();
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
                if (dbContext.Result.Where(r => r.Candidateid == LinkedCandidate.Id && r.Electionraceid == LinkedElectionrace.Id && r.Source == Info.Url).FirstOrDefault() == null)
                {
                    dbContext.Result.Add(new Result()
                    {
                        Id = Guid.NewGuid(),
                        Candidateid = LinkedCandidate.Id,
                        Numberofvotesrecieved = int.Parse(votes, NumberStyles.AllowThousands),
                        Electionraceid = LinkedElectionrace.Id,
                        Resultresolution = Info.ResultsResolution,
                        Precinct = Info.ResultsResolution == "precinct" ? precinct : null,
                        Districtcode = LinkedDistrict != null ? LinkedDistrict.Districtcode : null,
                        Source = Info.Url,
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