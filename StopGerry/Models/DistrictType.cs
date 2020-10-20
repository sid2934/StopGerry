using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Districttype
    {
        public Districttype()
        {
            District = new HashSet<District>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string DistrictTypeCode { get; set; }

        public virtual ICollection<District> District { get; set; }
    }
}
