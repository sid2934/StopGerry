using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.Research
{

    [Verb("research", HelpText = "Targets the research tool components of StopGerry")]
    public class Options : BaseOptions
    {

        [Option('e', "efficiency-gap",
            Required = false,
            Default = false,
            HelpText = "When this flag is set, the reserch tools will produce a effeciency gap for the given set. Requires the -a and -u flags to be set and either the -s or -d flag.")]
        public bool EfficiencyGap { get; set; }

        [Option('s', "states",
            Required = false,
            Default = null,
            HelpText = "Provide a csv list of the states that are to be processed. see -t|--district-types for info on filtering the districts processed")]
        public string States { get; set; }

        [Option('t', "district-types",
            Required = false,
            Default = "U.S. House, State House of Representatives, State Senate",
            HelpText = "Provide a csv list of the district types that are to be processed. Valid Options \"U.S. House\", \"State House of Representatives\", \"State Senate\"")]
        public string DistrictTypes { get; set; }

        [Option('d', "districts",
            Required = false,
            HelpText = "Provide a csv list of the districts that are to be processed. Format - \"{StateFips}{DistrictNumber}|{Office}\" e.g. \"45003|U.S. House\",")]
        public string Districts { get; set; }

        [Option('a', "election-date",
            Required = false,
            HelpText = "Sets the election date that will be used to select data. ISO 8601 style")]
        public string ElectionDate { get; set; }

        [Option('u', "source",
            Required = false,
            HelpText = "Sets the source (Source column on stopgerry.public.Result) that data will be selected from")]
        public string Source { get; set; }


        [Option('o', "output-to-file",
            Required = false,
            Default = null,
            HelpText = "Provides a file path to print the results to")]
        public string OutputPath { get; set; }

        [Option('g', "generate-graphics",
            Required = false,
            Default = null,
            HelpText = "When -g|--generate-graphics is set the results will create graphics to represent the data")]
        public bool GenerateGraphics { get; set; }
    }

}