using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class DailyPackingGridModel
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int GradeId { get; set; }
        public DateTime PackingDate { get; set; }
        public int BagFrom { get; set; }
        public int BagTo { get; set; }
        public int TotalBags { get; set; }
        public int Quantity { get; set; }
        public string ProductionRemarks { get; set; }
    }
}