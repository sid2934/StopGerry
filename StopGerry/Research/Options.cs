using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.Research
{

    [Verb("research", HelpText = "Targets the research tool components of StopGerry")]
    public class Options
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
            HelpText = "The path to the desired resource map. Defaulted to ./resources/resourceMap.csv")]
        public bool PerformAnalysis { get; set; }

    }

}