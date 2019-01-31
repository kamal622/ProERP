using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
   
    public class HistoryCardDataSet
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
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
        public string AttachmentFile { get; set; }
    }
}
