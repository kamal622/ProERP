using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.App_Code
{
    public class BreakDownReportDataSet
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public int? SubAssemblyId { get; set; }
        public string SubAssemblyName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public int? TotalTime { get; set; }
        public string FailureDescription { get; set; }
        public bool ElecticalTime { get; set; }
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
    }
}