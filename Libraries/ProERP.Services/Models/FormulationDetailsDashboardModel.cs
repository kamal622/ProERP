using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationDetailsDashboardModel
    {
        public int Id { get; set; }
        public int FormulationRequestId { get; set; }
        public int ItemId { get; set; }
        public int MachineId { get; set; }
        public string ItemName { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal? IssuedQty { get; set; }
        public decimal? ReturnQty { get; set; }
        public int? WIPId { get; set; }
        public decimal? WIPQty { get; set; }
        public int VerNo { get; set; }
    }
}
