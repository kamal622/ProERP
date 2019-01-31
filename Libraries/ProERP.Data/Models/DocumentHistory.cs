using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class DocumentHistory : BaseEntity
    {
        public Nullable<int> DocumentId { get; set; }
        public Nullable<int> ActionId { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public Nullable<int> ActionBy { get; set; }
        public virtual DocumentAction DocumentAction { get; set; }
        public virtual Document Document { get; set; }
        public virtual User User { get; set; }
    }
}
