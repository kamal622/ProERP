using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Sequence : BaseEntity
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
