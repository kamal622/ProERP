using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class BreakDownService : BaseEntity
    {
        public int BreakDownId { get; set; }
        public string VendorName { get; set; }
        public decimal Cost { get; set; }
        public string Comments { get; set; }
        public virtual BreakDown BreakDown { get; set; }
    }
}
