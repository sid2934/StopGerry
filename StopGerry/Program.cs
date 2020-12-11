using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Collections.Concurrent;
using StopGerry.Models;
using NetTopologySuite.Geometries;
using SharpKml.Engine;
using SharpKml.Dom;
using System.Text.RegularExpressions;
using CommandLine;
using StopGerry.DataIngest;
using System.Threading.Tasks;
using StopGerry.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace StopGerry
{
    public class BaseOptions
    {
        [Option('v', "verbosity",
            Required = false,
            Default = 0,
            HelpText = "Set output to verbose messages level. usage -v|--verbosity [LOGGING_LEVEL] (valid values 0,1,2)")]
        public short Verbosity { get; set; }


        [Option('c', "log-to-console",
            Required = false,
            Default = false,
            HelpText = "If set to true will print all logging messages directly to the console. Can reduce performance")]
        public bool LogToConsole { get; set; }

        [Option('j', "job-id",
            Required = false,
            Default = null,
            HelpText = "The job id for this run, will be used in certian outputs")]
        public string JobId { get; set; }

    }

    class Program
    {

        static void Main(string[] args)
        {
            //Injects the appsettings.json file into a local Configuration object
            GlobalConfig.InitConfig();





            CommandLine.Parser.Default.ParseArguments<DataIngest.Options, Research.Options>(args)
                .MapResult(
                    (DataIngest.Options opts) => DataIngest.RequestHandler.HandleRequest(opts),
                    (Research.Options opts) => Research.RequestHandler.HandleRequest(opts),
                    errs => { Console.WriteLine($"Errors {errs}"); return 1; }
                );

        }

    }
}
