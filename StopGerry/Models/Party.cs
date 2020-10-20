using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Party
    {
        public Party()
        {
            Candidate = new HashSet<Candidate>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public virtual ICollection<Candidate> Candidate { get; set; }
    }
}
