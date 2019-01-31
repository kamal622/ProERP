using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Machine : BaseEntity
    {
        public Machine()
        {
            this.BreakDowns = new List<BreakDown>();
            this.BreakDowns1 = new List<BreakDown>();
            this.Documents = new List<Document>();
            this.FormulationRequestsDetails = new List<FormulationRequestsDetail>();
            this.IndentDetails = new List<IndentDetail>();
            this.LineMachineActiveHistories = new List<LineMachineActiveHistory>();
            this.Machine1 = new List<Machine>();
            this.MachineReadings = new List<MachineReading>();
            this.MaintenanceRequests = new List<MaintenanceRequest>();
            this.PreventiveMaintenances = new List<PreventiveMaintenance>();
            this.ShutdownHistories = new List<ShutdownHistory>();
        }

        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Nullable<System.DateTime> InstallationDate { get; set; }
        public string MachineInCharge { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int MachineTypeId { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public bool IsActive { get; set; }
        public bool IsShutdown { get; set; }
        public virtual ICollection<BreakDown> BreakDowns { get; set; }
        public virtual ICollection<BreakDown> BreakDowns1 { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<FormulationRequestsDetail> FormulationRequestsDetails { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual Line Line { get; set; }
        public virtual ICollection<LineMachineActiveHistory> LineMachineActiveHistories { get; set; }
        public virtual ICollection<Machine> Machine1 { get; set; }
        public virtual Machine Machine2 { get; set; }
        public virtual MachineType MachineType { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<MachineReading> MachineReadings { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
        public virtual ICollection<PreventiveMaintenance> PreventiveMaintenances { get; set; }
        public virtual ICollection<ShutdownHistory> ShutdownHistories { get; set; }
    }
}
