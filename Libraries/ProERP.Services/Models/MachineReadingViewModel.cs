using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class MachineReadingViewModel
    {
        public int Id { get; set; }
        public int FormulationRequestId { get; set; }
        public int? MachineId { get; set; }
        public string MachineName { get; set; }
        public decimal? Reading { get; set; }
    }
}
