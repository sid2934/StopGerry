using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using StopGerry.Models;
using NetTopologySuite.Geometries;

namespace StopGerry.Utilities
{
    public class BlockUtilities
    {

        /// <summary>
        /// This method will process a csv containing the geo_headers for a given area.
        /// This will filter for a summary level (SUMLEV) of 101
        /// and will insert all new blocks into the database
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="context"></param>
        internal static void ProcessStateBlocksCSV(ResourceEntry resource, stopgerryContext dbContext)
        {

            var blocks = dbContext.Block;

            //Step 1: Get the block location data read from csv
            using (var reader = new StreamReader(resource.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().Where(r => r.SUMLEV == "101");


                ConcurrentBag<Block> resultCollection = new ConcurrentBag<Block>();

                ConcurrentBag<BlockPopulationTime> censusDataCollection = new ConcurrentBag<BlockPopulationTime>();

                ParallelLoopResult result = Parallel.ForEach(records, record =>
                {
                    //ToDo: Change this blockId to a GUID (requires chagnes to the database) and move each of the current id parts to their own fields
                    var blockId = $"{record.STATE}{record.COUNTY}{record.COUNTYCC}{record.COUNTYSC}{record.COUSUB}{record.COUSUBCC}{record.COUSUBSC}{record.PLACE}{record.PLACECC}{record.PLACESC}{record.TRACT}{record.BLKGRP}{record.BLOCK}";

                    
                    resultCollection.Add(new Block()
                    {
                        Id = blockId,
                        Coordinates = new NetTopologySuite.Geometries.Point(new Coordinate(Convert.ToDouble(record.INTPTLON), Convert.ToDouble(record.INTPTLAT))),
                        Description = $"{record.NAME}|{record.VTD} - LAT:{record.INTPTLAT}, LONG:{record.INTPTLON}",
                        Source = resource.FileSource
                    });
                    

                    var dateOfNote = resource.DateOfNote ?? Convert.ToDateTime("1/1/0001");

                    censusDataCollection.Add(new BlockPopulationTime{
                        Id = Guid.NewGuid(),
                        Blockid = blockId,
                        Reportingdate = dateOfNote,
                        Population = Convert.ToInt32(record.POP100),
                        
                    });
                });

                dbContext.AddRange(resultCollection);
                dbContext.AddRange(censusDataCollection);
                dbContext.SaveChanges();

            }
        }
    }
}