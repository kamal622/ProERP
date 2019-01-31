using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Purchase : BaseEntity
    {
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> OrderQty { get; set; }
        public Nullable<int> UnitPrice { get; set; }
        public Nullable<int> TotalPrice { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> OrderBy { get; set; }
        public int VendorId { get; set; }
    }
}
