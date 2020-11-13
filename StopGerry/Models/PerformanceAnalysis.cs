using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class PerformanceAnalysis
    {
        public Guid Id { get; set; }
        public long Totalruntime { get; set; }
        public int Numberofcoresavailable { get; set; }
        public int Numberofblocks { get; set; }
        public int Numberofdistricts { get; set; }
        public string States { get; set; }
        public long Memoryused { get; set; }
        public string Hostname { get; set; }
        public int Systempagesize { get; set; }
        public ulong Numberofskippedcomparisons {get;set;}
    }
}
