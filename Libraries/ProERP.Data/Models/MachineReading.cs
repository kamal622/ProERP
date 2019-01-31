using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class MachineReading : BaseEntity
    {
        public int FormulationRequestId { get; set; }
        public int MachineId { get; set; }
        public Nullable<decimal> Reading { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual Machine Machine { get; set; }
    }
}
