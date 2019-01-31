using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class MaintenanceRequestReportDataSet
    {
        public int Id { get; set; }
        public string SerialNo { get; set; }
        public int RequestBy { get; set; }
        public DateTime RequestDate { get; set; }
        public TimeSpan RequestTime { get; set; }
        public DateTime RequestDateTime { get; set; }
        public bool IsBreakdown { get; set; }
        public int SiteId { get; set; }
        public Nullable<int> PlantId { get; set; }
        public string PlantName { get; set; }
        public Nullable<int> LineId { get; set; }
        public string LineName { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string MachineName { get; set; }
        public string Problem { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public string Priority { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public bool IsCritical { get; set; }
        public Nullable<int> BreakdownType { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public DateTime? AssignDate { get; set; }
        public Nullable<int> AssignBy { get; set; }
        public int RemarksBy { get; set; }
        public string Remarks { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public TimeSpan? WorkStartTime { get; set; }
        public DateTime? WorkStartDateTime { get; set; }
        public DateTime? WorkEndDate { get; set; }
        public TimeSpan? WorkEndTime { get; set; }
        public DateTime? WorkEndDateTime { get; set; }
        public int? TotalTime { get; set; }
        public DateTime? ProgressDate { get; set; }
        public string ProgressRemarks { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string CompleteRemarks { get; set; }
        public DateTime? CloseDate { get; set; }
        public string CloseRemarks { get; set; }
        public DateTime? HoldDate { get; set; }
        public string HoldRemarks { get; set; }
        public string RequestUserName { get; set; }
        public string AssignUserName { get; set; }
        public string AssignByUserName { get; set; }
        public string ProgressUserName { get; set; }
        public string CompleteUserName { get; set; }
        public string CloseUserName { get; set; }
        public string HoldUserName { get; set; }
        public string TimeSpent { get; set; }
    }
}
