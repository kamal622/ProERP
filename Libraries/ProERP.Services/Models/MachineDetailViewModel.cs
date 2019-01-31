using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class MachineDetailViewModel
    {
        public int Id { get; set; }
        public string MachineName { get; set; }
        public decimal ItemQtyPercentage { get; set; }
        public decimal? ItemQtyGram { get; set; }
    }
}
