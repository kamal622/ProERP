using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class LineMachineMapping : BaseEntity
    {
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
    }
}
