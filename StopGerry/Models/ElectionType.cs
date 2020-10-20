﻿using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class ElectionType
    {
        public ElectionType()
        {
            CountyElection = new HashSet<CountyElection>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CountyElection> CountyElection { get; set; }
    }
}
