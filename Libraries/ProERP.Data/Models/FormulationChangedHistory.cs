using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class FormulationChangedHistory : BaseEntity
    {
        public int FormulationRequestId { get; set; }
        public int VerNo { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateOn { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
    }
}
