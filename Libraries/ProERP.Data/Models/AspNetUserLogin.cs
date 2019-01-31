using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class AspNetUserLogin : BaseEntity
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserId { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
