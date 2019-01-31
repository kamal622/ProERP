using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class DashboardModel
    {
    }

    public class BreakdownsByPlantModel
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public int Count { get; set; }
    }
    public class BreakdownsByLineModel
    {
        public int Id { get; set; }
        public string LineName { get; set; }
        public int Count { get; set; }
    }
    public class BreakdownsByMachineModel
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public int Count { get; set; }
        public int? TotalTime { get; set; }
    }

    public class ShutdownGridModel
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int MachineId { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string MachineName { get; set; }
        public string ShutdownBy { get; set; }
        public DateTime? ShutdownDate { get; set; }
        public bool IsShutdown { get; set; }
    }

    public class VersionGridModel
    {
        public int Id { get; set; }
        public string ReleaseVersion { get; set; }
        public DateTime ReleaseDate { get; set; }
        public VersionNotesModel[] Notes { get; set; }
    }

    public class VersionNotesModel
    {
        public int Id { get; set; }
        public string Notes { get; set; }
    }

    public class MaintenanceGridModel
    {
        public int Id { get; set; }
        public string SerialNo { get; set; }
        public int RequestBy { get; set; }
        public string RequestUserName { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestTime { get; set; }
        public bool IsBreakdown { get; set; }
        public int SiteId { get; set; }
        public Nullable<int> PlantId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string MachineName { get; set; }
        public string Problem { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public string Priority { get; set; }
        public int StatusId { get; set; }
        public int RemarksBy { get; set; }
        public string Status { get; set; }
        public bool IsCritical { get; set; }
        public Nullable<int> BreakdownType { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public string AssignUserName { get; set; }
        public Nullable<int> AssignBy { get; set; }
        public Nullable<int>  ProgressBy { get; set; }
        public Nullable<int>  CompleteBy { get; set; }
        public Nullable<int> CloseBy { get; set; }
        public string Remarks { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public DateTime? WorkStartTime { get; set; }
        public DateTime? WorkEndDate { get; set; }
        public DateTime? WorkEndTime { get; set; }
        public string ProgressRemarks { get; set; }
        public string CompleteRemarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CloseRemarks { get; set; }
        public string HoldRemarks { get; set; }
    }
}
