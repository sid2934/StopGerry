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
                    ResourceEntry.ProcessResourceMapFile(options.ResourceMapPath);
                }

                if(options.PerformAnalysis == true)
                {
                    Analysis.AnalyzeBlocksForDistrictRelationships();
                }
            }
            catch (Exception e)
            {
                SimpleLogger.Error($"Exception thown when trying to process the resource file {options.ResourceMapPath}\n{e}");
                return -1;
            }


            return 0;
        }
    }
}