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
    
            var state = dbContext.State.Where(s => s.Abbreviation == splitString[1].ToUpper()).FirstOrDefault();
            if (state == null)
            {
                throw new ArgumentException($"No state with the abbreviation {splitString[1].ToUpper()} could be found in the database");
            }

            var countyList = dbContext.County.ToList().Where(c => Convert.ToInt32(c.Id.Slice(0, 2)) == state.Id).ToDictionary(c=>c.Description.ToLower(), c => c.Id);

            Info = new OpenElections_Info()
            {
                Url = url,
                ElectionDate = DateTime.ParseExact(splitString[0], "yyyyMMdd", CultureInfo.InvariantCulture),
                State = state,
                CountyDictionary = countyList,
                ElectionType = splitString[2].ToLower(),
                ResultsResolution = splitString[3].ToLower(),
                FileName = fileNameWithoutExtension
            };


            //Get information from file name
            try
            {
                //This monstrocity will read the contents of the url to memory and then attempt to process it as an OpenElections_Results type
                //If sucessfule this.Results will be set to the values specified
                Results = new CsvReader(new StreamReader(new HttpClient().GetStreamAsync(url).Result), CultureInfo.InvariantCulture).GetRecords<OpenElections_Results>().ToList();

                //Create record for the overall election

                Dictionary<string, int> CandidateLookUp = new Dictionary<string, int>();
                Dictionary<string, string> DistrictLookUp = new Dictionary<string, string>();

                foreach (var record in Results)
                {
                    
                    var result = record.SaveToDB(Info, dbContext);

                    
                    CandidateLookUp[record.candidate] = result.Item1;
                    
                    if(result.Item2 != null)
                    { 
                        DistrictLookUp[record.district] = result.Item2;
                    }
                    else if (result.Item2 != null && !string.IsNullOrWhiteSpace(record.precinct))
                        SimpleLogger.Error($"Record return from OpenElections_Results.SaveToDB() contained null value for DistrictLookUp {ObjectDumper.Dump(record)}");

                }
                
                dbContext.SaveChanges();


            }
            catch (Exception e)
            {
                SimpleLogger.Error($"Failed to read OpenElections data file {url}\n{e}");
                Results = null;
            }

        }
    }
}