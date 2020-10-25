using System;
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
            SimpleLogger.Info("Create DB Context");
            var dbContext = new stopgerryContext();
            
            SimpleLogger.Info($"Connection status with Deja = {(dbContext.Database.CanConnect() ? "true" : "false")}");

            //Get all blocks
            SimpleLogger.Info("Get all blocks");
            var allBlocks = dbContext.Block.ToList();


            //Get all Districts (this works for now since we only have Kansas (FIPS 20) but we will have to filter it by state later)
            //Get the centroid of the district and max distance from that point.
            //This will allow us to see if a given block's coordinates are potentially within 
            SimpleLogger.Info("Get all districts");
            var allDistricts = dbContext.District.ToList();

            PreformanceMetrics.StartTimer();

            var newResults = new ConcurrentBag<BlockDistrictTime>();

            Parallel.ForEach(allBlocks, block =>
            {
                Parallel.ForEach(allDistricts, district =>
                {
                    if (district.Border.Contains(block.Coordinates))
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

            long memoryUsed = PreformanceMetrics.GetMemoryUsage();
            PreformanceMetrics.StopTimer();
            //Create a new preformance record
            dbContext.PerformanceAnalysis.Add(
                new PerformanceAnalysis
                {
                    Numberofblocks = allBlocks.Count(),
                    Numberofdistricts = allDistricts.Count(),
                    Numberofcoresavailable = Environment.ProcessorCount,
                    Memoryused = PreformanceMetrics.GetMemoryUsage(),
                    States = "All",
                    Totalruntime = PreformanceMetrics.ElapseTime
                });
            //dbContext.AddRange(newResults);
            dbContext.SaveChanges();
        }
    }
}