using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class MachineType : BaseEntity
    {
        public MachineType()
        {
            this.Machines = new List<Machine>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }
    }
}
