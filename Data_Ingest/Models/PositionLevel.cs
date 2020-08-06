using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class PositionLevel
    {
        public PositionLevel()
        {
            RaceType = new HashSet<RaceType>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<RaceType> RaceType { get; set; }
    }
}
