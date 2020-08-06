using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Data_Ingest.Models
{
    public partial class State
    {
        public State()
        {
            CountyTime = new HashSet<CountyTime>();
            StateTime = new HashSet<StateTime>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public int StateTypeId { get; set; }
        public int CountyTypeId { get; set; }
        public Geometry Border { get; set; }

        public virtual CountyType CountyType { get; set; }
        public virtual StateType StateType { get; set; }
        public virtual ICollection<CountyTime> CountyTime { get; set; }
        public virtual ICollection<StateTime> StateTime { get; set; }
    }
}
