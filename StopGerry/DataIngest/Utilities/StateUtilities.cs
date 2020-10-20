using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using StopGerry.Models;
using Microsoft.EntityFrameworkCore;
using SharpKml.Dom;
using SharpKml.Engine;
using StopGerry.Utilities;
using CsvHelper;
using System.Globalization;
using StopGerry.Utilities;

namespace StopGerry.DataIngest.Utilities
{
    internal class StateUtility
    {
        
        /// <summary>
        /// This method provides a utility to define the borders of each state from a given KML file
        /// </summary>
        /// <param name="filePath">The properly formatted KLM file</param>
        internal static void ProcessStateKMLData(string filePath, stopgerryContext dbContext)
        {

            var states = dbContext.State;

            // This section will populate all of the states data
            KmlFile file = KmlFile.Load(new StreamReader(filePath));
            if (file.Root is Kml kml)
            {
                foreach (var placemark in kml.Flatten().OfType<Placemark>())
                {


                    //Create a new State record
                    var currentStateName = Regex.Replace(placemark.Name, @"\<[^\>]*\>", "");
                    var currentState = states.Where(s => s.Name == currentStateName).FirstOrDefault();
                    if (currentState == null)
                    {
                        Console.WriteLine(currentStateName);
                        //throw new Exception();
                    }
                    else
                    {
                        try
                        {
                            
                            var stateBorder = SharpKMLToNetTopology.GeometryToGeometry(placemark.Geometry);
                            currentState.Border = stateBorder;
                            dbContext.State.Attach(currentState);
                            dbContext.Entry(currentState).Property(x => x.Border).IsModified = true;
                            dbContext.SaveChanges();
                        }
                        catch
                        {
                            Console.WriteLine(currentState.Name);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Will process the basic state data file stateDate.csv (found in the resources).
        /// This method is used to repopulate data into the database after a full wipe
        /// </summary>
        /// <param name="filePath">Path to the stateData.csv file</param>
        /// <param name="dbContext">Database context</param>
        internal static void ProcessBasicStateData(string filePath, stopgerryContext dbContext)
        {
            using(StreamReader reader = new StreamReader(filePath))
            using(CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                while(csvReader.Read())
                {
                    var dynamicRecord = csvReader.GetRecord<dynamic>();
                    dbContext.Add(new State(){
                        Id = Convert.ToInt32(dynamicRecord.Id),
                        Abbreviation = dynamicRecord.Abbreviation,
                        Border = null, //This prevents errors about the Geomerty encoding that SQL Server uses. We will just reprocess the border kml
                        Name = dynamicRecord.Name,
                        Source = dynamicRecord.Source,
                        Countytypeid = Convert.ToInt32(dynamicRecord.CountyTypeId),
                        Statetypeid = Convert.ToInt32(dynamicRecord.StateTypeId),
                        
                    });
                }
                dbContext.SaveChanges();
            }
        }
    }
}
