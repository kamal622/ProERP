using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Part : BaseEntity
    {
        public Part()
        {
            this.BreakDownSpares = new List<BreakDownSpare>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<BreakDownSpare> BreakDownSpares { get; set; }
    }
}
