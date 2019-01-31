using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Setting : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
