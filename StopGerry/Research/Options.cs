using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.Research
{

    [Verb("research", HelpText = "Targets the research tool components of StopGerry")]
    public class Options
    {
        
        [Option('v', "verbosity", 
            Required = false, 
            Default= 0,
            HelpText = "Set output to verbose messages level. usage -v|--verbosity [LOGGING_LEVEL] (valid values 0,1,2)")]
        public short Verbosity { get; set; }


        [Option('c', "log-to-console", 
            Required = false, 
            Default= false,
            HelpText = "If set to true will print all logging messages directly to the console. Can reduce performance")]
        public bool LogToConsole {get; set;}


        [Option('e', "efficiency-gap", 
            Required = false, 
            Default= false,
            HelpText = "When this flag is set, the reserch tools will produce a effeciency gap score for all districts being analyzed")]
        public bool EfficiencyGap { get; set; }

    }

}