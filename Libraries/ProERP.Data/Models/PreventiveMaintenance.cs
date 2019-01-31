using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class PreventiveMaintenance : BaseEntity
    {
        public PreventiveMaintenance()
        {
            this.PreventiveReviewHistories = new List<PreventiveReviewHistory>();
            this.UserAssignments = new List<UserAssignment>();
        }

        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int MachineId { get; set; }
        public string Description { get; set; }
        public string Checkpoints { get; set; }
        public int ScheduleType { get; set; }
        public int Interval { get; set; }
        public Nullable<int> ShutdownRequired { get; set; }
        public Nullable<System.DateTime> ScheduleStartDate { get; set; }
        public Nullable<System.DateTime> ScheduleEndDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> LastReviewDate { get; set; }
        public System.DateTime NextReviewDate { get; set; }
        public Nullable<int> PreferredVendorId { get; set; }
        public string WorkDescription { get; set; }
        public Nullable<System.DateTime> IsDeletedOn { get; set; }
        public Nullable<int> IsDeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Severity { get; set; }
        public bool IsObservation { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual PreventiveScheduleType PreventiveScheduleType { get; set; }
        public virtual ICollection<PreventiveReviewHistory> PreventiveReviewHistories { get; set; }
        public virtual ICollection<UserAssignment> UserAssignments { get; set; }
    }
}
