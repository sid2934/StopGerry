using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Data_Ingest.Models;

namespace Data_Ingest.Utilities
{
    public class OhioElectionData
    {
        public static void ProcessOhioElectionDate(ResourceEntry resource, StopGerryPrdContext dbContext)
        {

            string electiontypeString = resource.RecordDescription.Split('-')[3];

            ElectionType electionType = dbContext.ElectionType.Where(et => et.Description == electiontypeString).FirstOrDefault();
            if (electionType == null)
            {
                electionType = new ElectionType()
                {
                    Description = electiontypeString,
                };
                dbContext.Add(electionType);
                dbContext.SaveChanges();
            }

            string raceTypeString = resource.RecordDescription.Split('-')[2];
            RaceType raceType = dbContext.RaceType.Where(rt => rt.Description == raceTypeString).FirstOrDefault();
            if (raceType == null)
            {
                raceType = new RaceType()
                {
                    Description = raceTypeString,
                    PositionLevelId = 4
                };
                dbContext.Add(electionType);
                dbContext.SaveChanges();
            }

            using (var reader = new StreamReader(resource.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                Dictionary<int, Candidate> candidates = new Dictionary<int, Candidate>();
                while (csv.Read())
                {
                    string countyName = csv.GetField(0);

                    if (countyName == "County Name")
                    {
                        //This loop will grab all of the candidates names
                        //it will add new parties and candidates to the db
                        for (int i = 6; ; i++)
                        {
                            string candidateString;
                            if (csv.TryGetField<string>(i, out candidateString))
                            {

                                //See if party exists if not add it to the db
                                int partyStartIndex;
                                int partyLength = candidateString.Length - candidateString.LastIndexOf(')');
                                string candidatePartyString;

                                if ((partyStartIndex = candidateString.IndexOf('(') + 1) == 0)
                                {
                                    candidatePartyString = "Unknown";
                                }
                                else
                                {
                                    candidatePartyString = candidateString.Substring(partyStartIndex, partyLength).Trim();
                                }


                                Party candidateParty = dbContext.Party.Where(p => p.Name == candidatePartyString || p.Abbreviation == candidatePartyString).FirstOrDefault();
                                if (candidateParty == null)
                                {
                                    candidateParty = new Party()
                                    {
                                        Name = candidatePartyString,
                                        Abbreviation = candidatePartyString.Length <= 5 ? candidatePartyString : candidatePartyString.Substring(0, 5),
                                    };
                                    dbContext.Add(candidateParty);
                                    dbContext.SaveChanges();
                                }

                                //See if the candidate already exists if not create them
                                int nameEndIndex = candidateString.IndexOf('(') - 1;
                                string candidateName;
                                if (nameEndIndex <= 0)
                                {
                                    candidateName = candidateString;
                                }
                                else
                                {
                                    candidateName = candidateString.Substring(0, candidateString.IndexOf('(') - 1).Trim();
                                }

                                Candidate candidate = dbContext.Candidate.Where(c => c.Name == candidateName && c.PartyId == candidateParty.Id).FirstOrDefault();

                                if (candidate == null)
                                {
                                    candidate = new Candidate()
                                    {
                                        Name = candidateName,
                                        PartyId = candidateParty.Id,
                                    };
                                    dbContext.Add(candidate);
                                }

                                candidates.Add(i, candidate);

                            }
                            else
                            {
                                break;
                            }
                        }
                        //At this point all candidates should have a record in database and their party should exist
                        dbContext.SaveChanges();
                    }
                    else if (countyName == "Total")
                    {
                        //create a voter turnout record
                    }
                    else if (countyName == "Percentage")
                    {
                        continue; //skip
                    }
                    else
                    {
                        County county = dbContext.County.Where(c => c.Description == countyName).FirstOrDefault();
                        if (county == null)
                        {
                            SimpleLogger.Error($"The county {countyName} could not be found in the list of counties in the database.");
                            continue;
                        }
                        //START HERE. create a valid CountyElection, Race, then generate a result for each candidate
                        CountyElection countyElection =
                            dbContext.CountyElection
                                .Where(ce => ce.Description == resource.RecordDescription)
                                .FirstOrDefault();
                        if (countyElection == null)
                        {
                            countyElection = new CountyElection()
                            {
                                Id = Guid.NewGuid(),
                                Description = resource.RecordDescription,
                                ElectionDate = Convert.ToDateTime(resource.DateOfNote),
                                ElectionTypeId = electionType.Id,
                                CountyId = county.Id
                            };
                            dbContext.Add(countyElection);
                            dbContext.SaveChanges();
                        }


                        Race race = dbContext.Race.Where(r => r.CountyElectionId == countyElection.Id && r.RaceTypeId == raceType.Id).FirstOrDefault();
                        if (race == null)
                        {
                            race = new Race()
                            {
                                Id = Guid.NewGuid(),
                                RaceTypeId = raceType.Id,
                                CountyElectionId = countyElection.Id
                            };
                            dbContext.Add(race);
                            dbContext.SaveChanges();
                        }




                        foreach (var candidate in candidates)
                        {
                            try
                            {
                                dbContext.Result.Add(new Result()
                                {
                                    Id = Guid.NewGuid(),
                                    CandidateId = candidate.Value.Id,
                                    NumberOfVotesRecieved = int.Parse(csv.GetField(candidate.Key), NumberStyles.AllowThousands),
                                    RaceId = race.Id,
                                    Source = resource.FileSource,
                                });
                            }
                            catch (Exception e)
                            {
                                SimpleLogger.Error($"Could not process result record for County:{countyName} RaceId:{race.Id} Candidate:{candidate.Value.Name}.\n{e}");
                            }
                        }
                        dbContext.SaveChanges();

                    }
                }
            }
        }
    }
}
