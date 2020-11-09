using System;
using System.Globalization;
using System.Linq;
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

        private Electionrace LinkedElectionrace { get; set; }


        private bool LinkToDatabase(stopgerryContext dbContext)
        {
            //Find the linked State
            LinkedState = dbContext.State.Where(s => s.Abbreviation == Info.StateAbbreviation).FirstOrDefault();
            if (LinkedState == null)
            {
                SimpleLogger.Error($"No state with the abbreviation {Info.StateAbbreviation} could be found in the database");
                return false;
            }

            //Find the linked County
            LinkedCounty = dbContext.County.ToList().Where(c => Convert.ToInt32(c.Id.Slice(0, 2)) == LinkedState.Id && c.Description.ToLower() == county.ToLower()).FirstOrDefault();
            if (LinkedCounty == null)
            {
                SimpleLogger.Error($"Could not find an existing match for this county {county}");
                return false;
            }

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

            return true;

        }


        public void SaveToDB(OpenElections_Info electionInfo, stopgerryContext dbContext)
        {
            Info = electionInfo;
            if (LinkToDatabase(dbContext))
            {
                //Create the county election id if not exists
                if(dbContext.Result.Where(r => r.Candidateid == LinkedCandidate.Id && r.Electionraceid == LinkedElectionrace.Id && r.Source == Info.Url).FirstOrDefault() == null)
                {
                    dbContext.Result.Add(new Result()
                    {
                        Id = Guid.NewGuid(),
                        Candidateid = LinkedCandidate.Id,
                        Numberofvotesrecieved = int.Parse(votes, NumberStyles.AllowThousands),
                        Electionraceid = LinkedElectionrace.Id,
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
            //Check if this election already exists.

        }
    }
}