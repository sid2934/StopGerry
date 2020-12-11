using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.DataIngest
{

    [Verb("ingest", HelpText = "Targets the data ingest components of StopGerry")]
    public class Options : BaseOptions
    {
        
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
            Default= "All",
            HelpText = "A csv list of each state to do analysis on given by the state abbreviation")]
        public string AnalysisStates { get; set; }

        
    }

}