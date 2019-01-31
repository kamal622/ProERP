using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class QASpecification : BaseEntity
    {
        public int FormulationRequestId { get; set; }
        public string MFI220c10kg { get; set; }
        public string SPGravity { get; set; }
        public string AshContent { get; set; }
        public string NotchImpact { get; set; }
        public string Tensile { get; set; }
        public string FlexuralModule { get; set; }
        public string FlexuralStrength { get; set; }
        public string Elongation { get; set; }
        public string Flammability { get; set; }
        public string GWTAt { get; set; }
        public int VerNo { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
    }
}
