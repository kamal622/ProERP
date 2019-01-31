using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Line : BaseEntity
    {
        public Line()
        {
            this.BreakDowns = new List<BreakDown>();
            this.Documents = new List<Document>();
            this.FormulationRequests = new List<FormulationRequest>();
            this.IndentDetails = new List<IndentDetail>();
            this.LineMachineActiveHistories = new List<LineMachineActiveHistory>();
            this.Machines = new List<Machine>();
            this.MaintenanceRequests = new List<MaintenanceRequest>();
            this.PreventiveMaintenances = new List<PreventiveMaintenance>();
            this.ProcessLogSheet1 = new List<ProcessLogSheet1>();
            this.ProcessLogSheet2 = new List<ProcessLogSheet2>();
            this.ShutdownHistories = new List<ShutdownHistory>();
        }

        public int PlantId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string InCharge { get; set; }
        public bool IsActive { get; set; }
        public bool IsShutdown { get; set; }
        public virtual ICollection<BreakDown> BreakDowns { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<FormulationRequest> FormulationRequests { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<LineMachineActiveHistory> LineMachineActiveHistories { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
        public virtual ICollection<PreventiveMaintenance> PreventiveMaintenances { get; set; }
        public virtual ICollection<ProcessLogSheet1> ProcessLogSheet1 { get; set; }
        public virtual ICollection<ProcessLogSheet2> ProcessLogSheet2 { get; set; }
        public virtual ICollection<ShutdownHistory> ShutdownHistories { get; set; }
    }
}
