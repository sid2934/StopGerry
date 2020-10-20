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
    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

    }

    class Program
    {

        static void Main(string[] args)
        {
            //Injects the appsettings.json file into a local Configuration object
            GlobalConfig.InitConfig();



            SimpleLogger.Start(true);

            CommandLine.Parser.Default.ParseArguments<DataIngest.Options>(args)
                .MapResult(
                    (DataIngest.Options opts) => DataIngest.RequestHandler.HandleRequest(opts),
                    errs => { Console.WriteLine($"Errors {errs}"); return 1; }
                );



            SimpleLogger.Stop();
        }

    }
}
