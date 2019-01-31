using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class UserAssignment : BaseEntity
    {
        public int PreventiveMaintenanceId { get; set; }
        public int UserId { get; set; }
        public virtual PreventiveMaintenance PreventiveMaintenance { get; set; }
        public virtual User User { get; set; }
    }
}
