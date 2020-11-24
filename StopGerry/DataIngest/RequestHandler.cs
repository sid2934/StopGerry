using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.DataIngest
{
    public static class RequestHandler
    {
        public static int HandleRequest(Options options)
        {
            
            SimpleLogger.SetLoggingLevel(options.Verbosity, options.LogToConsole);
            SimpleLogger.SetJobId(options.JobId);
            SimpleLogger.Start();
             
            try
            {
                if(options.ProcessResourceMap == true)
                {
                    SimpleLogger.Info("Begin resource map processing");
                    ResourceEntry.ProcessResourceMapFile(options.ResourceMapPath);
                }

                if(options.PerformAnalysis == true)
                {
                    SimpleLogger.Info("Begin block/district analysis");
                    Analysis.AnalyzeBlocksForDistrictRelationships(options.AnalysisStates, options.JobId);
                }
            }
            catch (Exception e)
            {
                SimpleLogger.Error($"Exception was thrown while running in ingest. {ObjectDumper.Dump(options)}\n{e}");
                return -1;
            }

            SimpleLogger.Stop();
            return 0;
        }
    }
}