using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class DailyPackingDetail : BaseEntity
    {
        public int BatchId { get; set; }
        public int GradeId { get; set; }
        public System.DateTime PackingDate { get; set; }
        public int BagFrom { get; set; }
        public int BagTo { get; set; }
        public int TotalBags { get; set; }
        public int Quantity { get; set; }
        public string ProductionRemarks { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
    }
}
