using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class RawMaterialHistoryViewModel
    {
        public int Id { get; set; }
        public int QtyToProduce { get; set; }
        public int FormulationRequestId { get; set; }
        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestBy { get; set; }
        public string RMRequestStatus { get; set; }
        public string LOTSize { get; set; }
        public string ColorSTD { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string DispatchBy { get; set; }
        public DateTime? ReceviedDate { get; set; }
        public string ReceviedBy { get; set; }
    }
}
