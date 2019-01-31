using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class BreakDownViewModel
    {
    }

    public class VendorCategoryModel
    {
        public int VendorId { get; set; }
        public int CategoryId { get; set; }
        public string VendorName { get; set; }
        public string CategoryName { get; set; }
    }
}
