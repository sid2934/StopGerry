using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace StopGerry.Models
{
    public partial class County
    {
        public County()
        {
            BlockCountyTime = new HashSet<BlockCountyTime>();
            CountyElection = new HashSet<CountyElection>();
            CountyTime = new HashSet<CountyTime>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public Geometry Border { get; set; }

        public virtual ICollection<BlockCountyTime> BlockCountyTime { get; set; }
        public virtual ICollection<CountyElection> CountyElection { get; set; }
        public virtual ICollection<CountyTime> CountyTime { get; set; }
    }
}
