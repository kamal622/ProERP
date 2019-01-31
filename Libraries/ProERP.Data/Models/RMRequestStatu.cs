using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class RMRequestStatu : BaseEntity
    {
        public RMRequestStatu()
        {
            this.RMRequestMasters = new List<RMRequestMaster>();
        }

        public string StatusName { get; set; }
        public virtual ICollection<RMRequestMaster> RMRequestMasters { get; set; }
    }
}
