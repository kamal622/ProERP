using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class AspNetRole : BaseEntity
    {
        public AspNetRole()
        {
            this.AspNetUsers = new List<AspNetUser>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
