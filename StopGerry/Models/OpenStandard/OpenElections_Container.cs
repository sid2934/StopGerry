/*
    This class is to interface with and provide some utilities for the OpenElections (http://openelections.net)

*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using CsvHelper;
using StopGerry.Utilities;
using System.Linq;

namespace StopGerry.Models.OpenStandard
{
    public class OpenElections_Container
    {
        public OpenElections_Container(OpenElections_Info info) 
        {
            this.Info = info;
               
        }
                public OpenElections_Info Info { get; set; }

        public IEnumerable<OpenElections_Results> Results { get; set; }

        /// <summary>
        /// Creates the container for the OpenElection data file you wish to use
        /// </summary>
        /// <param name="url">The direct GitHub url to the csv file you wish to import</param>
        public OpenElections_Container(string url, stopgerryContext dbContext)
        {


            string fileName = url.Slice(url.LastIndexOf('/') + 1, url.Length);
            string fileNameWithoutExtension = fileName.Slice(0, fileName.LastIndexOf('.'));
            var splitString = fileNameWithoutExtension.Split("__");
            Info = new OpenElections_Info()
            {
                Url = url,
                ElectionDate = DateTime.ParseExact(splitString[0], "yyyyMMdd", CultureInfo.InvariantCulture),
                StateAbbreviation = splitString[1].ToUpper(),
                ElectionType = splitString[2].ToLower(),
                ResultsResolution = splitString[3].ToLower(),
                FileName = fileNameWithoutExtension
            };


            //Get information from file name

            try
            {
                //This monstrocity will read the url to memory and then attempt to process it as an OpenElections_Results type
                //If sucessfule this.Results will be set to the values specified
                Results = new CsvReader(new StreamReader(new HttpClient().GetStreamAsync(url).Result), CultureInfo.InvariantCulture).GetRecords<OpenElections_Results>();

                //Create record for the overall election



                Dictionary<Tuple<string, string>, Electionrace> ElectionRaceLookUp = new Dictionary<Tuple<string, string>, Electionrace>();
                Dictionary<string, Candidate> CandidateLookUp = new Dictionary<string, Candidate>();
                Dictionary<string, District> DistrictLookUp = new Dictionary<string, District>();
                foreach (var record in Results)
                {
                    // ! ToDo Crate a hashtable or dictionary that quickly maps each row to the correct ElectionRace object this is save a tremendous amount of time
                    /*

                        Id = Guid.NewGuid(),
                        Candidateid = LinkedCandidate.Id,
                        Numberofvotesrecieved = int.Parse(votes, NumberStyles.AllowThousands),
                        Electionraceid = LinkedElectionrace.Id,
                        Resultresolution = Info.ResultsResolution,
                        Precinct = Info.ResultsResolution == "precinct" ? precinct : null,
                        Districtcode = LinkedDistrict != null ? LinkedDistrict.Districtcode : null,
                        Source = Info.Url,
                    */

                    var electionLookUpKey =  new Tuple<string,string>(record.county, record.office);
                    Tuple<Electionrace, Candidate, District> shortcut = new Tuple<Electionrace, Candidate, District>(
                        ElectionRaceLookUp.GetValue(electionLookUpKey),
                        CandidateLookUp.GetValue(record.candidate),
                        DistrictLookUp.GetValue(record.district)
                    );
                    


                    var result = record.SaveToDB(Info, dbContext);
                    if(result.Item1 != null)
                    { 
                        ElectionRaceLookUp[electionLookUpKey] = result.Item1;
                    }
                    else
                        SimpleLogger.Error($"Record return from OpenElections_Results.SaveToDB() contained null value for ElectionRaceLookUp {ObjectDumper.Dump(record)}");
                    if(result.Item2 != null)
                    { 
                        CandidateLookUp[record.candidate] = result.Item2;
                    }
                    else
                        SimpleLogger.Error($"Record return from OpenElections_Results.SaveToDB() contained null value for CandidateLookUp {ObjectDumper.Dump(record)}");
                    if(result.Item3 != null)
                    { 
                        DistrictLookUp[record.district] = result.Item3;
                    }
                    else if (result.Item3 != null && !string.IsNullOrWhiteSpace(record.precinct))
                        SimpleLogger.Error($"Record return from OpenElections_Results.SaveToDB() contained null value for DistrictLookUp {ObjectDumper.Dump(record)}");
                }


            }
            catch (Exception e)
            {
                SimpleLogger.Error($"Failed to read OpenElections data file {url}\n{e}");
                Results = null;
            }

        }
    }
}