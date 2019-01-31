using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class ProcessLogSheetGridModel
    {
        public List<ProcessLogSheet1GridModel> logSheet1 { get; set; }
        public List<ProcessLogSheet2GridModel> logSheet2 { get; set; }
    }

    public class ProcessLogSheet1GridModel
    {
        public int Id { get; set; }
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
    }

    public class ProcessLogSheet2GridModel
    {
        public int Id { get; set; }
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
    }



}
