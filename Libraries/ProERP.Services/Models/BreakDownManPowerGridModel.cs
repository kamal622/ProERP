using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
   public class BreakDownManPowerGridModel
    {
        public int Id { get; set; }
        public int BreakDownId { get; set; }
        public string Name { get; set; }
        public string EmployeeType { get; set; }
        public int EmployeeTypeId { get; set; }
        public bool IsOverTime { get; set; }
        public decimal HourlyRate { get; set; }
        public string Comments { get; set; }
    }
}
