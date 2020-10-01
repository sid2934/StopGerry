﻿using System;
using System.Collections.Generic;

namespace Data_Ingest.Models
{
    public partial class VoterTurnout
    {
        public Guid Id { get; set; }
        public int Registeredvoters { get; set; }
        public int? Totalvoters { get; set; }
        public Guid Raceid { get; set; }

        public virtual Race Race { get; set; }
    }
}
