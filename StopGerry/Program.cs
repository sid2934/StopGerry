using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Collections.Concurrent;
using StopGerry.Models;
using NetTopologySuite.Geometries;
using SharpKml.Engine;
using SharpKml.Dom;
using System.Text.RegularExpressions;
using CommandLine;
using StopGerry.DataIngest;
using System.Threading.Tasks;
using StopGerry.Utilities;
using Microsoft.Extensions.Configuration;

namespace StopGerry
{
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {

            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var section = Configuration.GetSection("Sectionofsettings");

            SimpleLogger.Start(true);
            
            CommandLine.Parser.Default.ParseArguments<DataIngest.Options>(args)
                .MapResult(
                    (DataIngest.Options opts) => DataIngest.RequestHandler.HandleRequest(opts),
                    errs => { Console.WriteLine($"Errors {errs}"); return 1; }
                );


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
