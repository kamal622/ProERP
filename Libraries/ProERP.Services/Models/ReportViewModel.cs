using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    class ReportViewModel
    {
    }

    public class PreventiveTaskReportModel
    {
        public string UserName { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string Notes { get; set; }
        public DateTime NextReviewDate { get; set; }
        public DateTime ScheduledReviewDate { get; set; }
        public string MachineName { get; set; }
        public string LineName { get; set; }
        public string ScheduleTypeName { get; set; }
        public string WorkName { get; set; }
        public int ScheduleType { get; set; }
        public int Interval { get; set; }
        public IEnumerable<string> AssignedTo { get; set; }
    }

    public class PreventiveSummaryReportModel
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int UserId { get; set; }

        public DateTime ScheduledReviewDate { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string UserName { get; set; }
        public int TotalActivity { get; set; }
        public int Moderate { get; set; }
        public int Critical { get; set; }
        public int Minor { get; set; }
        public int ReviewedCount { get; set; }
        public int ModerateReviewedCount { get; set; }
        public int CriticalReviewedCount { get; set; }
        public int MinorReviewedCount { get; set; }
        public int LapseCount { get; set; }
        public int ModerateLapseCount { get; set; }
        public int CriticalLapseCount { get; set; }
        public int MinorLapseCount { get; set; }
        public int HoldCount { get; set; }
    }

    public class ShutdownSummaryReportModel
    {
        public int Id { get; set; }
        public int historyId { get; set; }
        public int ShutdownPlantId { get; set; }
        public int ShutdownLineId { get; set; }
        public int ShutdownMachineId { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int MachineId { get; set; }
        public string ShutdownPlantName { get; set; }
        public string ShutdownLineName { get; set; }
        public string ShutdownMachineName { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string MachineName { get; set; }
        public string WorkDescription { get; set; }
        public string ReviewBy { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string ShutdownBy { get; set; }
        public DateTime ShutdownDate { get; set; }
        public string Note { get; set; }
    }

    public class RepeatedMajorDataSet
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int LineId { get; set; }
        public string LineName { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public int? SubAssemblyId { get; set; }
        public string SubAssemblyName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public int? TotalTime { get; set; }
        public string FailureDescription { get; set; }
        public bool ElectricalTime { get; set; }
        public bool MechTime { get; set; }
        public bool InstrTime { get; set; }
        public bool UtilityTime { get; set; }
        public bool PowerTime { get; set; }
        public bool ProcessTime { get; set; }
        public bool PrvTime { get; set; }
        public bool IdleTime { get; set; }
        public DateTime? ResolveTimeTaken { get; set; }
        public int? SpareTypeId { get; set; }
        public string SpareTypeName { get; set; }
        public string SpareDescription { get; set; }
        public string DoneBy { get; set; }
        public string RootCause { get; set; }
        public string Correction { get; set; }
        public string CorrectiveAction { get; set; }
        public string PreventingAction { get; set; }
        public string MenPowerUsed { get; set; }
        public string PartsUsed { get; set; }
        public string ServiceUsed { get; set; }
    }

    public class PreventiveAuditReportModel
    {
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string MachineName { get; set; }
        public string ScheduleTypeName { get; set; }
        public int Interval { get; set; }
        public string WorkName { get; set; }
        public string AuditStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UserName { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}
