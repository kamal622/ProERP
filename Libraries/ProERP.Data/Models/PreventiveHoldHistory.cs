using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class PreventiveHoldHistory : BaseEntity
    {
        public PreventiveHoldHistory()
        {
            this.PreventiveReviewHistories = new List<PreventiveReviewHistory>();
        }

        public int PreventiveId { get; set; }
        public int HoldBy { get; set; }
        public Nullable<System.DateTime> HoldOn { get; set; }
        public string Reason { get; set; }
        public int HoldDays { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<PreventiveReviewHistory> PreventiveReviewHistories { get; set; }
    }
}
