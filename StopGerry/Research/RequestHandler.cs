using System;
using System.IO;
using System.Linq;
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
                    if(options.ElectionDate == null)
                    {
                        SimpleLogger.Fatal($"To calculate the Efficiency Gap you must provide a election date.\nSee the -a|--election-date flag for more info.");
                        return -1;
                    }
                    if(options.Source == null)
                    {
                        SimpleLogger.Fatal($"To calculate the Efficiency Gap you must provide a source for the data.\nSee the -u|--source flag for more info.");
                        return -1;
                    }
                    if(!(string.IsNullOrWhiteSpace(options.States) ^ string.IsNullOrWhiteSpace(options.Districts)))
                    {
                        SimpleLogger.Fatal($"To calculate the Efficiency Gap you must provide either states or districts but not both to process.\nSee the -s|--states and -d|--districts flags for more info.");
                        return -1;
                    }
                    if(!string.IsNullOrWhiteSpace(options.States))
                    {
                        //Process each state in the list
                        //Get the state appreviations
                        var districtTypes = options.DistrictTypes.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToArray();
                        var whitelistedDistrictTypes = new[] { "U.S. House", "State House of Representatives", "State Senate" };
                        foreach(var type in districtTypes)
                        {
                            if(!whitelistedDistrictTypes.Contains(type))
                            {
                                SimpleLogger.Fatal($"District type \"{type}\" is not a vaild option. \nSee the -t|--district-types flag for more info.");
                                return -1;
                            }
                        }

                        var result = EfficiencyGap.StateUtilities.ProcessStates(options.States, Convert.ToDateTime(options.ElectionDate), options.Source, districtTypes);
                        SimpleLogger.Info($"EfficiencyGap results =\n{ObjectDumper.Dump(result)}");

                        if(!string.IsNullOrWhiteSpace(options.OutputToFile))
                        {
                            //Write result to file
                        }
                        if(options.GenerateGraphics == true)
                        {
                            //Generate Graphics
                        }
                    }

                    





                    //EfficiencyGapUtility.GenerateEfficiencyGap("SC", Convert.ToDateTime("2016-11-08"), @"https://raw.githubusercontent.com/openelections/openelections-data-sc/master/2016/20161108__sc__general__precinct.csv");
                    EfficiencyGap.StateUtilities.CalculateStateEG("SC", Convert.ToDateTime("2016-11-08"), @"https://raw.githubusercontent.com/openelections/openelections-data-sc/master/2016/20161108__sc__general__precinct.csv");
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