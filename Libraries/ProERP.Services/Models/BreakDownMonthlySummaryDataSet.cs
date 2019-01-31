using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class BreakDownMonthlySummaryDataSet
    {
        public DateTime? Date { get; set; }
        public int DaysInMonth { get; set; }
        public int? TotalTime { get; set; }
        public string PlantName { get; set; }
        public string LineName { get; set; }
        public string BreakDownType { get; set; }
        public string FailureDescription { get; set; }
    }
}
