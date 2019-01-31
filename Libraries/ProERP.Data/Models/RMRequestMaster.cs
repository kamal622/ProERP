using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class RMRequestMaster : BaseEntity
    {
        public RMRequestMaster()
        {
            this.RMRequestDetails = new List<RMRequestDetail>();
        }

        public string LotNo { get; set; }
        public int ProductId { get; set; }
        public int FormulationRequestId { get; set; }
        public int RequestStatus { get; set; }
        public System.DateTime RequestDate { get; set; }
        public int RequestBy { get; set; }
        public string RequestRemarks { get; set; }
        public Nullable<System.DateTime> DispatchDate { get; set; }
        public Nullable<int> DispatchBy { get; set; }
        public string DispatchRemarks { get; set; }
        public Nullable<System.DateTime> ReceviedDate { get; set; }
        public Nullable<int> ReceviedBy { get; set; }
        public string ReceviedRemarks { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        public virtual ICollection<RMRequestDetail> RMRequestDetails { get; set; }
        public virtual RMRequestStatu RMRequestStatu { get; set; }
    }
}
