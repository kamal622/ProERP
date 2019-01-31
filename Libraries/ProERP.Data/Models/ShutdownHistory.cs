using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ShutdownHistory : BaseEntity
    {
        public ShutdownHistory()
        {
            this.PreventiveReviewHistories = new List<PreventiveReviewHistory>();
        }

        public int PlantId { get; set; }
        public int LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public System.DateTime ShutdownDate { get; set; }
        public int ShutdownBy { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<int> StartBy { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<PreventiveReviewHistory> PreventiveReviewHistories { get; set; }
    }
}
