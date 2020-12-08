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
            CountyTime = new HashSet<CountyTime>();
            Result = new HashSet<Result>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public int StateId { get; set; }
        public string Source { get; set; }
        public Geometry Border { get; set; }

        public virtual ICollection<BlockCountyTime> BlockCountyTime { get; set; }
        public virtual ICollection<CountyTime> CountyTime { get; set; }
        public virtual ICollection<Result> Result { get; set; }
    }
}
