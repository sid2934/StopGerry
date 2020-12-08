using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace StopGerry.Models
{
    public partial class Block
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public Geometry Coordinates { get; set; }
        public Geometry Border { get; set; }
    }
}
