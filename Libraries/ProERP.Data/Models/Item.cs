using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Item : BaseEntity
    {
        public Item()
        {
            this.IndentBudgets = new List<IndentBudget>();
            this.IndentDetails = new List<IndentDetail>();
        }

        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpecificationFile { get; set; }
        public string IsImported { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public string UnitOfMeasure { get; set; }
        public Nullable<int> AvailableQty { get; set; }
        public string MOC { get; set; }
        public Nullable<int> VendorId { get; set; }
        public virtual ICollection<IndentBudget> IndentBudgets { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
