using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class ProductDashboardViewModel
    {
        public int NewFormulationRequestCount { get; set; }
        public int RMRequestCount { get; set; }
        public int RMDispatchCount { get; set; }
        public int ReadyForTestingCount { get; set; }
    }
}