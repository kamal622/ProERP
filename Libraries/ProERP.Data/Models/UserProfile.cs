using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class UserProfile : BaseEntity
    {
        public UserProfile()
        {
            this.Users = new List<User>();
        }

        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVersionUpdated { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
