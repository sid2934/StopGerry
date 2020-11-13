using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.DataIngest
{

    [Verb("ingest", HelpText = "Targets the data ingest components of StopGerry")]
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

        [Option('p', "process-resource-map", 
            Required = false, 
            Default= false,
            HelpText = "If this flag is included the resource map will be procssed. See -p|--resource-map-path to set path to desired resource map")]
        public bool ProcessResourceMap { get; set; }

        [Option('r', "resource-map-path", 
            Required = false, 
            Default= @"resources/resourceMap.csv",
            HelpText = "The path to the desired resource map. Defaulted to ./resources/resourceMap.csv")]
        public string ResourceMapPath { get; set; }

        [Option('a', "perform-analysis", 
            Required = false, 
            Default= false,
            HelpText = "If -a flag is set, the initial relationship analysis will be ran")]
        public bool PerformAnalysis { get; set; }

        [Option('s', "analysis-states", 
            Required = false, 
            Default= false,
            HelpText = "A csv list of each state to do analysis on given by the state abbreviation")]
        public string AnalysisStates { get; set; }
    }

}