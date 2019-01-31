using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class DashboardViewModels
    {
        public int BreakdownYesterDayCount { get; set; }
        public int BreakdownTodayCount { get; set; }
        public int PMOverDueCount { get; set; }
        public int PMPendingCount { get; set; }
        public int PMShutdownCount { get; set; }
        //public int MaintenanceRequestCount { get; set; }
        public int AuditTaskPendingCount { get; set; }
        public int AuditTaskAllCount { get; set; }
        public int MaintenanceRequestOpenCount { get; set; }
        public int MaintenanceRequestInProcessCount { get; set; }
        public double BreakdownMonthPerCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] WhatsNewNotes { get; set; }
        public string ReleaseVersion { get; set; }
        public string UserRole { get; set; }

        public int NewFormulationRequestCount { get; set; }
        public int RMRequestCount { get; set; }
        public int RMDispatchCount { get; set; }
        public int ReadyForTestingCount { get; set; }
        public int ColorQAVerNoCount { get; set; }
    }

   
   
}