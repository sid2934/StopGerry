using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using StopGerry.Models;
using StopGerry.Utilities;

namespace StopGerry.DataIngest.Utilities
{
    public class OhioElectionData
    {
        public static void ProcessOhioElectionDate(ResourceEntry resource, stopgerryContext dbContext)
        {

            string electiontypeString = resource.RecordDescription.Split('-')[3];


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
                            if (csv.TryGetField<string>(i, out string candidateString))
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

                                Candidate candidate = dbContext.Candidate.Where(c => c.Name == candidateName && c.Party == candidatePartyString).FirstOrDefault();
                                if (candidate == null)
                                {
                                    candidate = new Candidate()
                                    {
                                        Name = candidateName,
                                        Party = candidatePartyString,
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

                        //ToDo: Make this not do a database request everytime. Read all counties to a dictionary or hashtable and look up that way
                        County county = dbContext.County.Where(c => c.Description == countyName).FirstOrDefault();
                        if (county == null)
                        {
                            SimpleLogger.Error($"The county {countyName} could not be found in the list of counties in the database.");
                            continue;
                        }


                        foreach (var candidate in candidates)
                        {
                            try
                            {
                                dbContext.Result.Add(new Result()
                                {
                                    //! Todo: Actually add the result
                                });
                            }
                            catch (Exception e)
                            {
                                SimpleLogger.Error($"Could not process result record for County:{countyName} RaceId:{("NEEDS TO BE SET")} Candidate:{candidate.Value.Name}.\n{e}");
                            }
                        }
                        dbContext.SaveChanges();

                    }
                }
            }
        }
    }
}
