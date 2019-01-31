using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Order : BaseEntity
    {
        public Nullable<int> PoNo { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<decimal> OrderQty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
    }
}
