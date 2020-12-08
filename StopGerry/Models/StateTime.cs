using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class StateTime
    {
        public Guid Id { get; set; }
        public int StateId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }

        public virtual State State { get; set; }
    }
}
