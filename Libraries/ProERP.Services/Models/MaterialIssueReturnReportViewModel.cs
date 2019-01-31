using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class MaterialIssueReturnReportViewModel
    {
        public int Id { get; set; }
        public int FormulationRequestId { get; set; }
        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public int QtyToProduce { get; set; }
        public int RMRequestId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal? IssuedQty { get; set; }
        public decimal? ReturnQty { get; set; }
        public int? WIPId { get; set; }
        public string WIPStoreName { get; set; }
        public decimal? WIPQty { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string RequestBy { get; set; }
        public DateTime RequestDate { get; set; }
        public string DispatchBy { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string ReceviedBy { get; set; }
        public DateTime? ReceviedDate { get; set; }
        public string PlantName { get; set; }
        public string OldMaterialReturn { get; set; }
        public string Remarks { get; set; }
    }
}
