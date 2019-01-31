using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationDashboardViewModel
    {
        public int Id { get; set; }
        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public int QtyToProduce { get; set; }
        public int UOM { get; set; }
        public string UOMName { get; set; }
        public string LOTSize { get; set; }
        public string ColorSTD { get; set; }
        public string Notes { get; set; }
        public int StatusId { get; set; }
        public int ProductId { get; set; }
        public string MachineName { get; set; }
        public string StatusName { get; set; }
        public string ItemName { get; set; }
        public string RMStatus { get; set; }
        public int? RMStatusId { get; set; }
        public int LineId { get; set; }
        public int? VerifyBy { get; set; }
        public string VerifyUser { get; set; }
        public DateTime? VerfyOn { get; set; }
        public int? QAStatusId { get; set; }
        public string QAStatusName { get; set; }
        //public int? ParentId { get; set; }
        public int VerNo { get; set; }
    }
}
