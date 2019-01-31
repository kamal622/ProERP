using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class FormulationRequestsChangeHistory : BaseEntity
    {
        public int FormulationRequestId { get; set; }
        public int RequestStatus { get; set; }
        public string Comment { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateOn { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual FormulationRequestsStatu FormulationRequestsStatu { get; set; }
    }
}
