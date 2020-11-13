using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.Research
{
    public static class RequestHandler
    {
        public static int HandleRequest(Options options)
        {
            try
            {
                if(options.EfficiencyGap == true)
                {
                    //var dbContext = new dbContext();
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