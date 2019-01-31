using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class BreakDownMenPower : BaseEntity
    {
        public int BreakDownId { get; set; }
        public string Name { get; set; }
        public int EmployeeTypeId { get; set; }
        public decimal HourlyRate { get; set; }
        public bool IsOverTime { get; set; }
        public string Comments { get; set; }
        public virtual BreakDown BreakDown { get; set; }
        public virtual EmployeeType EmployeeType { get; set; }
    }
}
