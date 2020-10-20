using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Demographic
    {
        public Guid Id { get; set; }
        public Guid Populationtimeid { get; set; }

        public virtual BlockPopulationTime Populationtime { get; set; }
    }
}
