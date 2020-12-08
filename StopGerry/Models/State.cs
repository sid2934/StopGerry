using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace StopGerry.Models
{
    public partial class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public string StateType { get; set; }
        public string CountyType { get; set; }
        public Geometry Border { get; set; }
    }
}
