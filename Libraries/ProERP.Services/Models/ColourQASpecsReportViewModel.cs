using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class ColourQASpecsReportViewModel
    {
        public int FormulationRequestId { get; set; }
        public string DeltaE { get; set; }
        public string DeltaL { get; set; }
        public string Deltaa { get; set; }
        public string Deltab { get; set; }
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
        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public DateTime? TestOn { get; set; }
        public string TestBy { get; set; }
        public string TestNotes { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string CreatedUser { get; set; }
        public int QtyToProduce { get; set; }
    }
}
