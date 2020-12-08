using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace StopGerry.Models
{
    public partial class County
    {
        public County()
        {
            Result = new HashSet<Result>();
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public int StateId { get; set; }
        public string Source { get; set; }
        public Geometry Border { get; set; }

        public virtual ICollection<Result> Result { get; set; }
    }
}
