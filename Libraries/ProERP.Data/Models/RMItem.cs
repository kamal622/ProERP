using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class RMItem : BaseEntity
    {
        public RMItem()
        {
            this.FormulationMasters = new List<FormulationMaster>();
            this.FormulationRequestsDetails = new List<FormulationRequestsDetail>();
            this.RMRequestDetails = new List<RMRequestDetail>();
            this.WIPStoreItems = new List<WIPStoreItem>();
        }

        public int CategoryId { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public virtual ICollection<FormulationMaster> FormulationMasters { get; set; }
        public virtual ICollection<FormulationRequestsDetail> FormulationRequestsDetails { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
        public virtual ICollection<RMRequestDetail> RMRequestDetails { get; set; }
        public virtual ICollection<WIPStoreItem> WIPStoreItems { get; set; }
    }
}
