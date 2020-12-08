using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class DistrictTime
    {
        public Guid Id { get; set; }
        public string DistrictId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }
}
