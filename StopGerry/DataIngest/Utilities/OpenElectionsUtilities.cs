
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using StopGerry.Models;
using StopGerry.Models.OpenStandard;
namespace StopGerry.DataIngest.Utilities
{
    public class OpenElectionsUtilities
    {
        public static void ProcessOpenElections(ResourceEntry resource, stopgerryContext dbContext)
        {
            using (var reader = new StreamReader(resource.FilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<OpenElections_Container> list = new List<OpenElections_Container>();
                foreach (var record in csv.GetRecords<dynamic>())
                {
                    list.Add(new OpenElections_Container(record.url, dbContext));
                }
            }
        }
    }
}