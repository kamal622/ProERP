using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class DailyPackingGridModel
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public string LotNo { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public DateTime PackingDate { get; set; }
        public int BagFrom { get; set; }
        public int BagTo { get; set; }
        public int TotalBags { get; set; }
        public int Quantity { get; set; }
        public string ProductionRemarks { get; set; }
    }
}
