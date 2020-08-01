using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class CountyType
    {
        public CountyType()
        {
            County = new HashSet<County>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<County> County { get; set; }
    }
}
