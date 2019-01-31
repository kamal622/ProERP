using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ItemCategory : BaseEntity
    {
        public ItemCategory()
        {
            this.RMItems = new List<RMItem>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RMItem> RMItems { get; set; }
    }
}
