using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ProcessLogSheet2 : BaseEntity
    {
        public int LineId { get; set; }
        public int BatchId { get; set; }
        public int GradeId { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime Time { get; set; }
        public decimal RPM { get; set; }
        public decimal TORQ { get; set; }
        public decimal AMPS { get; set; }
        public decimal RPM1 { get; set; }
        public decimal RPM2 { get; set; }
        public decimal RPM3 { get; set; }
        public decimal F1KGHR { get; set; }
        public decimal F1Perc { get; set; }
        public decimal F2KGHR { get; set; }
        public decimal F2Perc { get; set; }
        public decimal F3KGHR { get; set; }
        public decimal F3Perc { get; set; }
        public decimal F4KGHR { get; set; }
        public decimal F4Perc { get; set; }
        public decimal F5KGHR { get; set; }
        public decimal F5Perc { get; set; }
        public decimal F6KGHR { get; set; }
        public decimal F6Perc { get; set; }
        public decimal Output { get; set; }
        public decimal NoofDiesHoles { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public virtual FormulationRequest FormulationRequest { get; set; }
        public virtual Line Line { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
    }
}
