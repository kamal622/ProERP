using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    class PreventiveViewModel
    {
    }

    public class PreventiveDashboardData
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int MachineId { get; set; }
        public string Checkpoints { get; set; }
        public DateTime NextReviewDate { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string MachineName { get; set; }
        public string ScheduleTypeName { get; set; }
        public string WorkName { get; set; }
        public int ScheduleType { get; set; }
        public int Interval { get; set; }
        public string Severity { get; set; }
        public bool IsObservation { get; set; }
    }

    public class VerifiedPreventiveData
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public int MachineId { get; set; }
        public string Checkpoints { get; set; }
        public DateTime NextReviewDate { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string MachineName { get; set; }
        public string ScheduleTypeName { get; set; }
        public string WorkName { get; set; }
        public int ScheduleType { get; set; }
        public int Interval { get; set; }
        public string Severity { get; set; }
        public DateTime ?ReviewDate { get; set; }
        public string ReviewBy { get; set; }
        public DateTime ? VerifyDate { get; set; }
        public string VerifyBy { get; set; }
    }
}
