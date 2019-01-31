using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class LineMachineActiveHistory : BaseEntity
    {
        public int LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public bool IsActive { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual User User { get; set; }
    }
}
