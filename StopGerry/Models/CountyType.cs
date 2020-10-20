using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Countytype
    {
        public Countytype()
        {
            State = new HashSet<State>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<State> State { get; set; }
    }
}
