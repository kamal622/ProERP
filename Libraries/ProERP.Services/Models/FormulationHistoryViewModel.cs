using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationHistoryViewModel
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
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public int? QAStatusId { get; set; }
        public string QAStatusName { get; set; }
        public int? ParentId { get; set; }
    }

    public class FormulationDetailsHistoryModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string MachineName { get; set; }
        public int FormulationRequestId { get; set; }
        public decimal ItemQtyPercentage { get; set; }
        public decimal ItemQtyGram { get; set; }
        public int VerNo { get; set; }
    }

    public class RMDetailsHistoryViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal? IssuedQty { get; set; }
        public decimal? ReturnQty { get; set; }
        public decimal? WIPQty { get; set; }
    }

}
