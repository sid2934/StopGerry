using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Data_Ingest.Models
{
    public partial class District
    {
        public District()
        {
            BlockDistrictTime = new HashSet<BlockDistrictTime>();
            DistrictTime = new HashSet<DistrictTime>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int DistrictTypeId { get; set; }
        public Geometry Border { get; set; }

        public virtual DistrictType DistrictType { get; set; }
        public virtual ICollection<BlockDistrictTime> BlockDistrictTime { get; set; }
        public virtual ICollection<DistrictTime> DistrictTime { get; set; }
    }
}
