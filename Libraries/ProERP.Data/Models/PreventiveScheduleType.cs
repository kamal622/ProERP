using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class PreventiveScheduleType : BaseEntity
    {
        public PreventiveScheduleType()
        {
            this.PreventiveMaintenances = new List<PreventiveMaintenance>();
        }

        public string Description { get; set; }
        public virtual ICollection<PreventiveMaintenance> PreventiveMaintenances { get; set; }
    }
}
