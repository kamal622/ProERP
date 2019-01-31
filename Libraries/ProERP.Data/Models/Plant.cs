using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Plant : BaseEntity
    {
        public Plant()
        {
            this.BreakDowns = new List<BreakDown>();
            this.Documents = new List<Document>();
            this.EmployeeTypes = new List<EmployeeType>();
            this.IndentBudgets = new List<IndentBudget>();
            this.IndentDetails = new List<IndentDetail>();
            this.Lines = new List<Line>();
            this.Machines = new List<Machine>();
            this.MaintenanceRequests = new List<MaintenanceRequest>();
            this.PlantBudgets = new List<PlantBudget>();
            this.PreventiveMaintenances = new List<PreventiveMaintenance>();
            this.ShutdownHistories = new List<ShutdownHistory>();
        }

        public string Name { get; set; }
        public string PlantInCharge { get; set; }
        public string Location { get; set; }
        public Nullable<int> SiteId { get; set; }
        public virtual ICollection<BreakDown> BreakDowns { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<EmployeeType> EmployeeTypes { get; set; }
        public virtual ICollection<IndentBudget> IndentBudgets { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual ICollection<Line> Lines { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<PlantBudget> PlantBudgets { get; set; }
        public virtual ICollection<PreventiveMaintenance> PreventiveMaintenances { get; set; }
        public virtual ICollection<ShutdownHistory> ShutdownHistories { get; set; }
    }
}
