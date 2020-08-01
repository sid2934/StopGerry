using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class DistrictType
    {
        public DistrictType()
        {
            District = new HashSet<District>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string DistrictTypeCode { get; set; }

        public virtual ICollection<District> District { get; set; }
    }
}
