using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class BreakDownUploadHistory : BaseEntity
    {
        public BreakDownUploadHistory()
        {
            this.BreakDowns = new List<BreakDown>();
        }

        public Nullable<System.DateTime> UploadDate { get; set; }
        public Nullable<int> UploadBy { get; set; }
        public string OriginalFileName { get; set; }
        public string SystemFileName { get; set; }
        public virtual ICollection<BreakDown> BreakDowns { get; set; }
    }
}
