using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class CountyTime
    {
        public Guid Id { get; set; }
        public string CountyId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int StateId { get; set; }

        public virtual County County { get; set; }
        public virtual State State { get; set; }
    }
}
