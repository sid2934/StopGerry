using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class Candidate
    {
        public Candidate()
        {
            Result = new HashSet<Result>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int PartyId { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual Party Party { get; set; }
        public virtual ICollection<Result> Result { get; set; }
    }
}
