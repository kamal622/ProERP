﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class FormulationRequestReportDataModel
    {
        public int Id { get; set; }
        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public int QtyToProduce { get; set; }
        public int UOM { get; set; }
        public string UOMName { get; set; }
        public string LOTSize { get; set; }
        public string ColorSTD { get; set; }
        public string Notes { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? ProgressOn { get; set; }
        public string ProgressBy { get; set; }
        public string ProgressNotes { get; set; }
        public DateTime? ReadyForTestOn { get; set; }
        public string ReadyForTestBy { get; set; }
        public string ReadyForTestNotes { get; set; }
        public DateTime? TestOn { get; set; }
        public string TestBy { get; set; }
        public string TestNotes { get; set; }
        public DateTime? CloseOn { get; set; }
        public string CloseBy { get; set; }
        public string CloseNotes { get; set; }
        public DateTime? VerifyOn { get; set; }
        public string VerifyBy { get; set; }
        public string RequestBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestRemarks { get; set; }
        public string DispatchBy { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string DispatchRemarks { get; set; }
        public string ReceviedBy { get; set; }
        public DateTime? ReceviedDate { get; set; }
        public string ReceviedRemarks { get; set; }
        public string QAStatusName { get; set; }
        public string LineName { get; set; }
    }
}
