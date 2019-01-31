using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class RawMateriallDashboadModel
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
    }
}
