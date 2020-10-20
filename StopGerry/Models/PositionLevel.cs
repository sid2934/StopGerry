using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class PositionLevel
    {
        public PositionLevel()
        {
            ElectionraceType = new HashSet<ElectionraceType>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ElectionraceType> ElectionraceType { get; set; }
    }
}
