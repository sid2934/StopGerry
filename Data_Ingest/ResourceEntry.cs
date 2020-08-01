using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Data_Ingest.Models;
using Data_Ingest.Utilities;

namespace Data_Ingest
{
    public class ResourceEntry
    {
        public string FilePath { get; set; }

        public string FileType { get; set; }

        public string FileSource { get; set; }

        public string RecordType { get; set; }

        public string RecordDescription { get; set; }

        public DateTime? DateOfNote { get; set; }

        public bool Skip { get; set; } = false;

        /// <summary>
        /// Used to determine if this ResourceFileEntry is a directory
        /// </summary>
        /// <value></value>
        public bool IsDirectory
        {
            get
            {
                return File.GetAttributes(FilePath).HasFlag(FileAttributes.Directory);
            }
        }

        private List<ResourceEntry> GetChildrenResourceEntries()
        {
            var subEntriesList = new List<ResourceEntry>();
            var subEntries = Directory.GetFiles(FilePath);
            foreach (var subEntry in subEntries)
            {
                subEntriesList.Add(new ResourceEntry()
                {
                    FilePath = subEntry,
                    FileType = this.FileType,
                    FileSource = this.FileSource,
                    RecordDescription = this.RecordDescription,
                    RecordType = this.RecordType

                });
            }
            return subEntriesList;
        }

        public static void ProcessResourceMapFile(string resourceMapPath)
        {
            using (var dbContext = new StopGerryPrdContext())
            using (var reader = new StreamReader(resourceMapPath))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                int counter = 0;
                var csvRecords = csvReader.GetRecords<ResourceEntry>().Where(r => r.Skip == false).ToList();
                int numberOfRecords = csvRecords.Count();
                foreach (var entry in csvRecords)
                {
                    SimpleLogger.Info($"Starting processing of {entry.FilePath}. ({++counter}/{numberOfRecords})");
                    entry.ProcessResourceEntry(dbContext);
                    SimpleLogger.Info($"Finished {entry.FilePath}. ({counter}/{numberOfRecords})");
                }
            }
        }

        public void ProcessResourceEntry(StopGerryPrdContext dbContext)
        {
            if (this.IsDirectory)
            {
                SimpleLogger.Info($"Detected {FilePath} as a directory. Will attempt to process each sub file)");
                var subEntries = GetChildrenResourceEntries();
                int counter = 0;
                int numberOfSubEntries = subEntries.Count;
                foreach (var subEntry in GetChildrenResourceEntries())
                {
                    SimpleLogger.Info($"Starting processing of {subEntry.FilePath}. ({++counter}/{numberOfSubEntries})");
                    subEntry.ProcessResourceEntry(dbContext);
                    SimpleLogger.Info($"Finished {subEntry.FilePath}. ({counter}/{numberOfSubEntries})");
                }
            }
            else
            {
                switch (RecordType.ToLower())
                {
                    case "state":
                        if (FileType == "kml" && RecordDescription == "state borders")
                        {
                            StateUtility.ProcessStateKMLData(FilePath, dbContext);
                        }
                        else if (FileType == "csv" && RecordDescription == "basic state data")
                        {
                            StateUtility.ProcessBasicStateData(FilePath, dbContext);
                        }
                        break;

                    case "district":
                        if (FileType == "kml")
                        {
                            DistrictUtilities.ProcessDistrictKMLData(this, dbContext);
                        }
                        break;

                    case "county":
                        break;

                    case "block":
                        if (FileType == "csv" && RecordDescription == "geo header")
                        {
                            BlockUtilities.ProcessStateBlocksCSV(this, dbContext);
                        }
                        break;

                    default:
                        SimpleLogger.Info($"Resource File {FilePath} could not be processesed as the \"record_type\" is invalid");
                        break;
                }
            }
        }


    }
}