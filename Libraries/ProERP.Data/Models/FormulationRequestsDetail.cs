using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class FormulationRequestsDetail : BaseEntity
    {
        public int FormulationRequestId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public int ItemId { get; set; }
        public decimal ItemQtyPercentage { get; set; }
        public decimal ItemQtyGram { get; set; }
        public Nullable<int> UOM { get; set; }
        public int VerNo { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual RMItem RMItem { get; set; }
        public virtual Machine Machine { get; set; }
    }
}
