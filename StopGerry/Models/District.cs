using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace StopGerry.Models
{
    public partial class District
    {
        public District()
        {
            BlockDistrictTime = new HashSet<BlockDistrictTime>();
        }

        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Districtcode { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int Districttypeid { get; set; }
        public Geometry Border { get; set; }

        public virtual Districttype Districttype { get; set; }
        public virtual ICollection<BlockDistrictTime> BlockDistrictTime { get; set; }
    }
}
