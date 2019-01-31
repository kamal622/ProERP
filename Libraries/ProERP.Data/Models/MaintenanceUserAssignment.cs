using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class MaintenanceUserAssignment : BaseEntity
    {
        public int MaintenanceRequestId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
