using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Data_Ingest.Models
{
    public partial class County
    {
        public County()
        {
            BlockCountyTime = new HashSet<BlockCountyTime>();
            CountyTime = new HashSet<CountyTime>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int CountyTypeId { get; set; }
        public Geometry Border { get; set; }

        public virtual CountyType CountyType { get; set; }
        public virtual ICollection<BlockCountyTime> BlockCountyTime { get; set; }
        public virtual ICollection<CountyTime> CountyTime { get; set; }
    }
}
