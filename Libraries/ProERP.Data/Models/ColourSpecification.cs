using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ColourSpecification : BaseEntity
    {
        public int FormulationRequestId { get; set; }
        public string DeltaE { get; set; }
        public string DeltaL { get; set; }
        public string Deltaa { get; set; }
        public string Deltab { get; set; }
        public int VerNo { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
    }
}
