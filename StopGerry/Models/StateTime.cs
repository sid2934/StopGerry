using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class StateTime
    {
        public Guid Id { get; set; }
        public int Stateid { get; set; }
        public DateTime Timestart { get; set; }
        public DateTime? Timeend { get; set; }

        public virtual State State { get; set; }
    }
}
