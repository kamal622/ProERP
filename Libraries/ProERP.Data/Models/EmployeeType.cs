using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class EmployeeType : BaseEntity
    {
        public EmployeeType()
        {
            this.BreakDownMenPowers = new List<BreakDownMenPower>();
        }

        public string Type { get; set; }
        public string Description { get; set; }
        public double NormalCharges { get; set; }
        public double OverTimeCharges { get; set; }
        public int PlantId { get; set; }
        public virtual ICollection<BreakDownMenPower> BreakDownMenPowers { get; set; }
        public virtual Plant Plant { get; set; }
    }
}
