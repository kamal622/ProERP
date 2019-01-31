using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationCloseReportModel
    {
        public int Id { get; set; }
        public int FormulationRequestId { get; set; }
        public string GradeName { get; set; }
        public string LotNo { get; set; }
        public decimal? FGPackedQty { get; set; }
        public decimal? NSP { get; set; }
        public decimal? StartUpTrials { get; set; }
        public decimal? QCRejected { get; set; }
        public decimal? MixMaterial { get; set; }
        public decimal? Lumps { get; set; }
        public decimal? LongsandFines { get; set; }
        public decimal? LabSample { get; set; }
        public decimal? Sweepaged { get; set; }
        public decimal? PackingBags { get; set; }
        public decimal? Additives { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
