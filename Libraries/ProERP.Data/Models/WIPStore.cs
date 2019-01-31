using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class WIPStore : BaseEntity
    {
        public WIPStore()
        {
            this.RMRequestDetails = new List<RMRequestDetail>();
            this.WIPStoreItems = new List<WIPStoreItem>();
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateOn { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public virtual ICollection<RMRequestDetail> RMRequestDetails { get; set; }
        public virtual ICollection<WIPStoreItem> WIPStoreItems { get; set; }
    }
}
