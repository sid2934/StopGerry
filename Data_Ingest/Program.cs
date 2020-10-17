using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Data_Ingest.Models;
using NetTopologySuite.Geometries;
using SharpKml.Engine;
using SharpKml.Dom;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data_Ingest.Utilities;

namespace Data_Ingest
{
    class Program
    {



        static void Main(string[] args)
        {
            SimpleLogger.Start(true);
            //This foreach loop looks at the file_map file in the resources and process each listed file

            //ResourceEntry.ProcessResourceMapFile(@"resources\resourceMap.csv");

            //Generate relatioinships between blocks and districts
            AnalyzeBlocksForDistrictRelationships();

            SimpleLogger.Stop();
        }
        
        //ToDo: This method will soon be replaced with some MPI implementation
        private static void AnalyzeBlocksForDistrictRelationships()
        {
            var dbContext = new stopgerryContext();

            //Get all blocks
            var allBlocks = dbContext.Block.ToList();


            //Get all Districts (this works for now since we only have Kansas (FIPS 20) but we will have to filter it by state later)
            //Get the centroid of the district and max distance from that point.
            //This will allow us to see if a given block's coordinates are potentially within 
            var allDistricts = dbContext.District.ToList();


            var newResults = new ConcurrentBag<BlockDistrictTime>();
            
            Parallel.ForEach(allBlocks, block =>
            {
                Parallel.ForEach(allDistricts, district =>
                {
                    if (district.Border.Contains(block.Coordinates))
                    {
                        //Create new Block_District_Time
                        newResults.Add(new BlockDistrictTime(){
                            Id = Guid.NewGuid(),
                            Blockid = block.Id,
                            Districtid = district.Id,
                            Timestart = Convert.ToDateTime("1/1/1970")
                        });
                    }
                });
            });
            dbContext.AddRange(newResults);
            dbContext.SaveChanges();
        }

    }
}
