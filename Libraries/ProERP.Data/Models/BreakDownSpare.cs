using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class BreakDownSpare : BaseEntity
    {
        public int BreakDownId { get; set; }
        public int PartId { get; set; }
        public string Comments { get; set; }
        public Nullable<int> Quantity { get; set; }
        public virtual BreakDown BreakDown { get; set; }
        public virtual Part Part { get; set; }
    }
}
