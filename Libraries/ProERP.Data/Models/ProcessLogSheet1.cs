using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ProcessLogSheet1 : BaseEntity
    {
        public int LineId { get; set; }
        public int BatchId { get; set; }
        public int GradeId { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime Time { get; set; }
        public decimal TZ1 { get; set; }
        public decimal TZ2 { get; set; }
        public decimal TZ3 { get; set; }
        public decimal TZ4 { get; set; }
        public decimal TZ5 { get; set; }
        public decimal TZ6 { get; set; }
        public decimal TZ7 { get; set; }
        public decimal TZ8 { get; set; }
        public decimal TZ9 { get; set; }
        public decimal TZ10 { get; set; }
        public decimal TZ11 { get; set; }
        public decimal TZ12Die { get; set; }
        public decimal TM1 { get; set; }
        public decimal PM1 { get; set; }
        public decimal PM11 { get; set; }
        public decimal Vaccumembar { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual Line Line { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
    }
}
