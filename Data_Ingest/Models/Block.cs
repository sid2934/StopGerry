using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Data_Ingest.Models
{
    public partial class Block
    {
        public Block()
        {
            BlockCountyTime = new HashSet<BlockCountyTime>();
            BlockDistrictTime = new HashSet<BlockDistrictTime>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public Geometry Coordinates { get; set; }
        public Geometry Border { get; set; }

        public virtual ICollection<BlockCountyTime> BlockCountyTime { get; set; }
        public virtual ICollection<BlockDistrictTime> BlockDistrictTime { get; set; }
    }
}
