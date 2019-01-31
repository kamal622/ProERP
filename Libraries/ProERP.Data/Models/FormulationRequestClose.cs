using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class FormulationRequestClose : BaseEntity
    {
        public string GradeName { get; set; }
        public string LotNo { get; set; }
        public Nullable<decimal> FGPackedQty { get; set; }
        public Nullable<decimal> NSP { get; set; }
        public Nullable<decimal> StartUpTrials { get; set; }
        public Nullable<decimal> QCRejected { get; set; }
        public Nullable<decimal> MixMaterial { get; set; }
        public Nullable<decimal> Lumps { get; set; }
        public Nullable<decimal> LongsandFines { get; set; }
        public Nullable<decimal> LabSample { get; set; }
        public Nullable<decimal> Sweepaged { get; set; }
        public Nullable<decimal> Additives { get; set; }
        public Nullable<decimal> PackingBags { get; set; }
        public Nullable<System.DateTime> VerifiedDate { get; set; }
        public Nullable<int> VerifiedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
    }
}
