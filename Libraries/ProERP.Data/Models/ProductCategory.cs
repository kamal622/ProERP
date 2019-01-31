using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ProductCategory : BaseEntity
    {
        public ProductCategory()
        {
            this.ProductMasters = new List<ProductMaster>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProductMaster> ProductMasters { get; set; }
    }
}
