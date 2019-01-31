using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class SpareType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
