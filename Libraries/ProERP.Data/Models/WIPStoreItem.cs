using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class WIPStoreItem : BaseEntity
    {
        public int StoreId { get; set; }
        public int ItemId { get; set; }
        public Nullable<decimal> ItemQty { get; set; }
        public virtual RMItem RMItem { get; set; }
        public virtual WIPStore WIPStore { get; set; }
    }
}
