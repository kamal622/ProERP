using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class MaintanceRequestStatu : BaseEntity
    {
        public MaintanceRequestStatu()
        {
            this.MaintenanceRequests = new List<MaintenanceRequest>();
        }

        public string StatusName { get; set; }
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}
