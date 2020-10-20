using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.DataIngest
{

    [Verb("ingest", HelpText = "Targets the data ingest components of StopGerry")]
    public class Options
    {

        [Option('p', "resource-map-path", 
            Required = false, 
            Default= @"resources/resourceMap.csv",
            HelpText = "The path to the desired resource map. Defaulted to ./resources/resourceMap.csv")]
        public string ResourcePath { get; set; }

    }

}