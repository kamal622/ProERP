using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class VendorCategory : BaseEntity
    {
        public VendorCategory()
        {
            this.Vendors = new List<Vendor>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
