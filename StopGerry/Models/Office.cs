using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Office
    {
        public Office()
        {
            Result = new HashSet<Result>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string PositionLevel { get; set; }

        public virtual ICollection<Result> Result { get; set; }
    }
}
