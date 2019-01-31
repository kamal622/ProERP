using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class PreventiveReviewHistory : BaseEntity
    {
        public int PreventiveId { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public Nullable<int> ReviewBy { get; set; }
        public string Notes { get; set; }
        public System.DateTime ScheduledReviewDate { get; set; }
        public Nullable<int> HoldId { get; set; }
        public Nullable<int> ShutdownId { get; set; }
        public bool IsLaps { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsLineActive { get; set; }
        public bool IsMachineActive { get; set; }
        public Nullable<System.DateTime> VerifyDate { get; set; }
        public Nullable<int> VerifyBy { get; set; }
        public virtual PreventiveHoldHistory PreventiveHoldHistory { get; set; }
        public virtual PreventiveMaintenance PreventiveMaintenance { get; set; }
        public virtual ShutdownHistory ShutdownHistory { get; set; }
        public virtual User User { get; set; }
    }
}
