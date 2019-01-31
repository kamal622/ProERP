using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationViewModel
    {
        public int Id { get; set; }
        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public int QtyToProduce { get; set; }
        public int UOM { get; set; }
        public string LOTSize { get; set; }
        public string ColorSTD { get; set; }
        public string Notes { get; set; }
        public int StatusId { get; set; }
        public int ProductId { get; set; }
        public string WorkOrderNo { get; set; }
        public int LineId { get; set; }
        public int? QAStatusId { get; set; }
        public string UserRole { get; set; }
        public int VerNo { get; set; }
    }
}
