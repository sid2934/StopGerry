using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Data_Ingest.Models;

namespace Data_Ingest.Utilities
{
    public class CountyUtilities
    {
        public static void ProcessCountyFile(ResourceEntry resource, stopgerryContext dbContext)
        {
            using (var reader = new StreamReader(resource.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                foreach(var county in csv.GetRecords<dynamic>())
                {
                    string countyFipsAsString = county.FIPS.ToString();
                    string stateId = countyFipsAsString.Substring(0,2);
                    State countyState = dbContext.State.Where(s => s.Id == Convert.ToInt32(stateId)).FirstOrDefault();
                    if(countyState == null)
                    {
                        SimpleLogger.Error($"County record for FIPS = {countyFipsAsString} could not find a corresponding state");
                        break;
                    }

                    dbContext.Add(new County(){
                        Id = county.FIPS,
                        Description = county.NAME,
                        Source = "Census Bureau",
                    });
                }
            }
            dbContext.SaveChanges();
        }
    }
}