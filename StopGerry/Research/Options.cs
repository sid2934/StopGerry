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
            Default= false,
            HelpText = "When this flag is set, the reserch tools will produce a effeciency gap score for all districts being analyzed")]
        public bool EfficiencyGap { get; set; }

    }

}