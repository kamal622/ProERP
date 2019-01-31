using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationDetailViewModel
    {
        public int Id { get; set; }
        public int FormulationRequestId { get; set; }
        public int? MachineId { get; set; }
        public string MachineName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemQtyGram { get; set; }
        public decimal ItemQtyPercentage { get; set; }
    }
}
