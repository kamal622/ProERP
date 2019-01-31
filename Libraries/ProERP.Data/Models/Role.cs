using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            this.UserRoles = new List<UserRole>();
        }

        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
