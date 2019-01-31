using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class RMRequestDetail : BaseEntity
    {
        public int RMRequestId { get; set; }
        public int FormulationRequestId { get; set; }
        public int ItemId { get; set; }
        public decimal RequestedQty { get; set; }
        public Nullable<decimal> IssuedQty { get; set; }
        public Nullable<decimal> ReturnQty { get; set; }
        public Nullable<int> WIPId { get; set; }
        public Nullable<decimal> WIPQty { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual RMItem RMItem { get; set; }
        public virtual RMRequestMaster RMRequestMaster { get; set; }
        public virtual WIPStore WIPStore { get; set; }
    }
}
