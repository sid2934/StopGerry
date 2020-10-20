using System;
using System.IO;
using CommandLine;
using StopGerry.Utilities;

namespace StopGerry.DataIngest
{
    public static class RequestHandler
    {
        public static int HandleRequest(Options options)
        {
            try
            {
                ResourceEntry.ProcessResourceMapFile(options.ResourcePath);
            }
            catch (Exception e)
            {
                SimpleLogger.Error($"Exception thown when trying to process the resource file {options.ResourcePath}\n{e}");
            }


            return 1;
        }
    }
}