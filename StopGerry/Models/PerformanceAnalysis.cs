using System;
using System.Collections.Generic;

namespace StopGerry.Models
{
    public partial class PerformanceAnalysis
    {
        public Guid Id { get; set; }
        public long TotalRuntime { get; set; }
        public int NumberOfCoresAvailable { get; set; }
        public int NumberOfBlocks { get; set; }
        public int NumberOfDistricts { get; set; }
        public string States { get; set; }
        public long MemoryUsed { get; set; }
        public string Hostname { get; set; }
        public int SystemPageSize { get; set; }
        public string JobId { get; set; }
    }
}
