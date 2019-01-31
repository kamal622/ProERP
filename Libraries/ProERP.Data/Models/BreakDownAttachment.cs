using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class BreakDownAttachment : BaseEntity
    {
        public int BreakDownId { get; set; }
        public string OriginalFileName { get; set; }
        public string SysFileName { get; set; }
        public virtual BreakDown BreakDown { get; set; }
    }
}
