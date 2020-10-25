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
                    Analysis.AnalyzeBlocksForDistrictRelationships();
                }
            }
            catch (Exception e)
            {
                SimpleLogger.Error($"Exception was thrown while running in ingest. {ObjectDumper.Dump(options)}\n{e}");
                return -1;
            }


            return 0;
        }
    }
}