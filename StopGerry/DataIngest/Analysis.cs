using System;
using System.Net;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using StopGerry.Models;
using StopGerry.Utilities;
using System.Collections.Generic;

namespace StopGerry.DataIngest
{
    internal class Analysis
    {
        //This is soon be replaced with a more distributed solution
        //ToDo: If only new blocks were added then only they need to be processed,
        //      if districts were added new relationships need to be found for existing blocks if the timeframe overlaps
        //ToDo: This method needs to check to see if a relationship between a block-district exists before creating a new record
        internal static void AnalyzeBlocksForDistrictRelationships(string states, string jobId = null)
        {
            
            SimpleLogger.Debug("Create DB Context");
            var dbContext = new stopgerryContext();

            SimpleLogger.Debug($"Connection status with Deja = {(dbContext.Database.CanConnect() ? "true" : "false")}");

            int totalNumberOfBlocks = 0;
            int totalNumberOfDistricts = 0;

            List<string> statesToProcess;    


            if(states == null || states == "All")
            {
                SimpleLogger.Debug($"No states were listed using the -s argument so all states will be processed");
                //Generate a List<string with all stats fips codes
                statesToProcess = dbContext.State.Select(x => x.Id.ToString("00")).ToList();
            }
            else
            {
                var stateList = states.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                statesToProcess = dbContext.State.Where(s => stateList.Contains(s.Abbreviation)).Select(x => x.Id.ToString("00")).ToList();
            }


            PreformanceMetrics.StartTimer();
            foreach(var state in statesToProcess)
            {
                
                //This stop will prevent the database interaction from being included in the metrics. This is how we took out initial measurements
                PreformanceMetrics.StopTimer();
                SimpleLogger.Debug($"Get blocks for state: {state} [FIPS]");
                var blocksToProcess = dbContext.Block.Where(b => b.Id.Substring(0,2) == state).ToList();
                SimpleLogger.Debug($"Selected {blocksToProcess.Count} blocks for state: {state} this run");

                SimpleLogger.Debug($"Get districts for state: {state} [FIPS]");

                
                var districtsToProcess = dbContext.District.Where(d => d.Id.Substring(d.Id.IndexOf("|") + 1, 2) == state).ToList();


                SimpleLogger.Debug($"Selected {districtsToProcess.Count} districts for state: {state} this run");

                totalNumberOfBlocks+=blocksToProcess.Count;
                totalNumberOfDistricts+=districtsToProcess.Count;

                PreformanceMetrics.StartTimer();
                //We now have both the districts and the blocks for the current state only


                var newResults = new ConcurrentBag<BlockDistrictTime>();

                Parallel.ForEach(blocksToProcess, block =>
                {
                    Parallel.ForEach(districtsToProcess, district =>
                    {
                        var blockState = block.Id.Substring(0,2);
                        var districtState = district.Id.Substring(district.Id.IndexOf('|') + 1, 2);

                        if (district.Border.Contains(block.Coordinates))
                        {
                            //Create new Block_District_Time
                            //We should check to ensure that there are not matching records already in the database
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

            }
            PreformanceMetrics.StopTimer();
            
            //Create a new preformance record
            var newPerformanceAnalysis = new PerformanceAnalysis
                {
                    Numberofblocks = totalNumberOfBlocks,
                    Numberofdistricts = totalNumberOfDistricts,
                    Numberofcoresavailable = Environment.ProcessorCount,
                    Memoryused = PreformanceMetrics.GetMemoryUsage(),
                    States = states,
                    Totalruntime = PreformanceMetrics.ElapseTime,
                    Hostname = Dns.GetHostName(),
                    Systempagesize = Environment.SystemPageSize,
                    Jobid = jobId,
                };
            SimpleLogger.Info(ObjectDumper.Dump(newPerformanceAnalysis));
            dbContext.PerformanceAnalysis.Add(newPerformanceAnalysis);
            //dbContext.AddRange(newResults);
            dbContext.SaveChanges();
        }
    }
}