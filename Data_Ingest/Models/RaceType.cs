using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class RaceType
    {
        public RaceType()
        {
            Race = new HashSet<Race>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int PositionLevelId { get; set; }

        public virtual PositionLevel PositionLevel { get; set; }
        public virtual ICollection<Race> Race { get; set; }
    }
}
