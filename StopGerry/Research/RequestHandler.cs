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

            SimpleLogger.SetLoggingLevel(options.Verbosity, options.LogToConsole);
            SimpleLogger.SetJobId(options.JobId);
            SimpleLogger.Start();

            try
            {
                if(options.EfficiencyGap == true)
                {
                    //EfficiencyGapUtility.GenerateEfficiencyGap("SC", Convert.ToDateTime("2016-11-08"), @"https://raw.githubusercontent.com/openelections/openelections-data-sc/master/2016/20161108__sc__general__precinct.csv");
                    EfficiencyGap.CalculateDistrictEG("45003", "U.S. House", Convert.ToDateTime("2016-11-08"), @"https://raw.githubusercontent.com/openelections/openelections-data-sc/master/2016/20161108__sc__general__precinct.csv");
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