using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
   public class BreakDownServiceGridModel
    {
        public int Id { get; set; }
        public int BreakDownId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal Cost { get; set; }
        public string Comments { get; set; }

        }
}
