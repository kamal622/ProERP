using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ProductMaster : BaseEntity
    {
        public ProductMaster()
        {
            this.DailyPackingDetails = new List<DailyPackingDetail>();
            this.FormulationMasters = new List<FormulationMaster>();
            this.FormulationRequests = new List<FormulationRequest>();
            this.ProcessLogSheet1 = new List<ProcessLogSheet1>();
            this.ProcessLogSheet2 = new List<ProcessLogSheet2>();
            this.RMRequestMasters = new List<RMRequestMaster>();
        }

        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<DailyPackingDetail> DailyPackingDetails { get; set; }
        public virtual ICollection<FormulationMaster> FormulationMasters { get; set; }
        public virtual ICollection<FormulationRequest> FormulationRequests { get; set; }
        public virtual ICollection<ProcessLogSheet1> ProcessLogSheet1 { get; set; }
        public virtual ICollection<ProcessLogSheet2> ProcessLogSheet2 { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<RMRequestMaster> RMRequestMasters { get; set; }
    }
}
