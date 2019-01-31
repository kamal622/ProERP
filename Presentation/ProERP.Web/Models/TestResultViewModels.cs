using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class TestResultViewModels
    {
        public int VersionNo { get; set; }
        public TestColorSpecification colorSpec { get; set; }
        public TestQASpecification qaSpec { get; set; }
    }
    public class TestColorSpecification
    {
        public int ColourId { get; set; }
        public int FormulationRequestId { get; set; }
        public string DeltaE { get; set; }
        public string DeltaL { get; set; }
        public string Deltaa { get; set; }
        public string Deltab { get; set; }
    }

    public class TestQASpecification
    {
        public int QAId { get; set; }
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
    }
}