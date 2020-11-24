using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using StopGerry.Models;
using Microsoft.EntityFrameworkCore;
using SharpKml.Dom;
using SharpKml.Engine;
using StopGerry.Utilities;

namespace StopGerry.DataIngest.Utilities
{
    public class DistrictUtilities
    {
        /// <summary>
        /// This method provides a utility to define the borders of each state from a given KML file
        /// </summary>
        /// <param name="filePath">The properly formatted KLM file</param>
        internal static void ProcessDistrictKMLData(ResourceEntry resourceFile, stopgerryContext dbContext)
        {
            // This section will populate all of the districts data
            KmlFile file = KmlFile.Load(new StreamReader(resourceFile.FilePath));
            if (file.Root is Kml kml)
            {
                foreach (var placemark in kml.Flatten().OfType<Placemark>())
                {
                    var splitTheFileName = Path.GetFileNameWithoutExtension(resourceFile.FilePath).Split('_');
                    var when = splitTheFileName[1].ToUpper();
                    var where = splitTheFileName[2].ToUpper();
                    var what = splitTheFileName[3].ToUpper();


                    int newDistrictTypeId;
                    if (what.Substring(0, 2) == "CD")
                    {
                        newDistrictTypeId = 1;
                    }
                    else if (what == "SLDL")
                    {
                        newDistrictTypeId = 2;
                    }
                    else if (what == "SLDU")
                    {
                        newDistrictTypeId = 3;
                    }
                    else
                    {
                        SimpleLogger.Error("The KML file being processesed does not have the proper naming convention. The district type was not able to be identified");
                        throw new Exception();
                    }

                    //Create a new Districts record
                    var currentDistrictId = when + what + '|' + Regex.Replace(placemark.Name, @"\<[^\>]*\>", "");

                    var newDistrict = new District()
                    {
                        Districtcode = Regex.Replace(placemark.Name, @"\<[^\>]*\>", ""),
                        Year = Convert.ToInt32(when),
                        Description = placemark.Description.Text,
                        Source = resourceFile.FileSource,
                        Districttypeid = newDistrictTypeId
                    };

                    var districBorder = SharpKMLToNetTopology.GeometryToGeometry(placemark.Geometry);
                    newDistrict.Border = districBorder;
                    dbContext.Add(newDistrict);
                }
                dbContext.SaveChanges();
            }
        }
    }
}