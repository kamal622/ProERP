using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Vendor : BaseEntity
    {
        public Vendor()
        {
            this.IndentDetails = new List<IndentDetail>();
            this.Items = new List<Item>();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Note { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> StarRating { get; set; }
        public string Email { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual VendorCategory VendorCategory { get; set; }
    }
}
