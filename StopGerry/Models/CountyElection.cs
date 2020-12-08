using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class CountyElection
    {
        public Guid Id { get; set; }
        public string CountyId { get; set; }
        public string Description { get; set; }
        public DateTime ElectionDate { get; set; }
        public string ElectionType { get; set; }

        public virtual County County { get; set; }
    }
}
