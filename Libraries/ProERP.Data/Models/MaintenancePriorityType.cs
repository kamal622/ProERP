using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class MaintenancePriorityType : BaseEntity
    {
        public MaintenancePriorityType()
        {
            this.Indents = new List<Indent>();
            this.MaintenanceRequests = new List<MaintenanceRequest>();
        }

        public string Description { get; set; }
        public virtual ICollection<Indent> Indents { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}
