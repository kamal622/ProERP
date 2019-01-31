using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class UserClaim : BaseEntity
    {
        public int UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public virtual User User { get; set; }
    }
}
