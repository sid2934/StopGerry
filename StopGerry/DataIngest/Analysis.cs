using System;
using System.Net;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using StopGerry.Models;
using StopGerry.Utilities;

namespace StopGerry.DataIngest
{
    internal class Analysis
    {
        //This is soon be replaced with a more distributed solution
        //ToDo: If only new blocks were added then only they need to be processed,
        //      if districts were added new relationships need to be found for existing blocks if the timeframe overlaps
        //ToDo: This method needs to check to see if a relationship between a block-district exists before creating a new record
        internal static void AnalyzeBlocksForDistrictRelationships()
        {
            SimpleLogger.Debug("Create DB Context");
            var dbContext = new stopgerryContext();

            SimpleLogger.Debug($"Connection status with Deja = {(dbContext.Database.CanConnect() ? "true" : "false")}");

            //Get all blocks
            SimpleLogger.Debug("Get all blocks");
            var allBlocks = dbContext.Block.ToList();


            //Get all Districts (this works for now since we only have Kansas (FIPS 20) but we will have to filter it by state later)
            //Get the centroid of the district and max distance from that point.
            //This will allow us to see if a given block's coordinates are potentially within 
            SimpleLogger.Debug("Get all districts");
            var allDistricts = dbContext.District.ToList();

            PreformanceMetrics.StartTimer();

            var newResults = new ConcurrentBag<BlockDistrictTime>();
            ulong skippedComparisons = 0;
            const bool ENABLE_STATE_CHECK = true;

            Parallel.ForEach(allBlocks, block =>
            {
                Parallel.ForEach(allDistricts, district =>
                {
                    var blockState = block.Id.Substring(0,2);
                    var districtState = district.Id.Substring(district.Id.IndexOf('|') + 1, 2);

                    if (ENABLE_STATE_CHECK && !blockState.Equals(districtState))
                    {
                        ++skippedComparisons;
                    }
                    else if (district.Border.Contains(block.Coordinates))
                    {
                        //Create new Block_District_Time
                        newResults.Add(new BlockDistrictTime()
                        {
                            Id = Guid.NewGuid(),
                            Blockid = block.Id,
                            Districtid = district.Id,
                            Timestart = Convert.ToDateTime("1/1/1970")
                        });
                    }
                });
            });
            
            PreformanceMetrics.StopTimer();
            //Create a new preformance record
            var newPerformanceAnalysis = new PerformanceAnalysis
                {
                    Numberofblocks = allBlocks.Count(),
                    Numberofdistricts = allDistricts.Count(),
                    Numberofskippedcomparisons = skippedComparisons,
                    Numberofcoresavailable = Environment.ProcessorCount,
                    Memoryused = PreformanceMetrics.GetMemoryUsage(),
                    States = "All",
                    Totalruntime = PreformanceMetrics.ElapseTime,
                    Hostname = Dns.GetHostName(),
                    Systempagesize = Environment.SystemPageSize

                };
            SimpleLogger.Info(ObjectDumper.Dump(newPerformanceAnalysis));
            dbContext.PerformanceAnalysis.Add(newPerformanceAnalysis);
            //dbContext.AddRange(newResults);
            dbContext.SaveChanges();
        }
    }
}