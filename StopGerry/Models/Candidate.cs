﻿using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class Candidate
    {
        public Candidate()
        {
            Result = new HashSet<Result>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Party { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Result> Result { get; set; }
    }
}
