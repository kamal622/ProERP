using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class BreakDownPartGridModel
    {
        public int Id { get; set; }
        public int BreakDownId { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; }
        public string Comments { get; set; }
        public int Quantity { get; set; }
    }
}
