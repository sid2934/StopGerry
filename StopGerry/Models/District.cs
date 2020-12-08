using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace StopGerry.Models
{
    public partial class District
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string DistrictCode { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string DistrictType { get; set; }
        public Geometry Border { get; set; }
    }
}
