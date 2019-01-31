using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class MaintenanceRequest : BaseEntity
    {
        public string SerialNo { get; set; }
        public int RequestBy { get; set; }
        public System.DateTime RequestDate { get; set; }
        public System.TimeSpan RequestTime { get; set; }
        public bool IsBreakdown { get; set; }
        public Nullable<int> PlantId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string Problem { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public bool IsCritical { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public Nullable<int> AssignBy { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public Nullable<int> ProgressBy { get; set; }
        public Nullable<System.DateTime> ProgressDate { get; set; }
        public Nullable<int> CompleteBy { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<int> CloseBy { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public Nullable<int> HoldBy { get; set; }
        public Nullable<System.DateTime> HoldDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> RemarksBy { get; set; }
        public Nullable<System.DateTime> RemarksDate { get; set; }
        public Nullable<int> BreakdownType { get; set; }
        public Nullable<System.DateTime> WorkStartDate { get; set; }
        public Nullable<System.TimeSpan> WorkStartTime { get; set; }
        public Nullable<System.DateTime> WorkEndDate { get; set; }
        public Nullable<System.TimeSpan> WorkEndTime { get; set; }
        public string ProgressRemarks { get; set; }
        public string CompleteRemarks { get; set; }
        public string CloseRemarks { get; set; }
        public string HoldRemarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual MaintanceRequestStatu MaintanceRequestStatu { get; set; }
        public virtual MaintenancePriorityType MaintenancePriorityType { get; set; }
        public virtual Plant Plant { get; set; }
    }
}
