using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class FormulationRequest : BaseEntity
    {
        public FormulationRequest()
        {
            this.ColourSpecifications = new List<ColourSpecification>();
            this.DailyPackingDetails = new List<DailyPackingDetail>();
            this.FormulationChangedHistories = new List<FormulationChangedHistory>();
            this.FormulationRequestsChangeHistories = new List<FormulationRequestsChangeHistory>();
            this.FormulationRequestsDetails = new List<FormulationRequestsDetail>();
            this.MachineReadings = new List<MachineReading>();
            this.ProcessLogSheet1 = new List<ProcessLogSheet1>();
            this.ProcessLogSheet2 = new List<ProcessLogSheet2>();
            this.QASpecifications = new List<QASpecification>();
            this.RMRequestDetails = new List<RMRequestDetail>();
            this.RMRequestMasters = new List<RMRequestMaster>();
        }

        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public int QtyToProduce { get; set; }
        public Nullable<int> UOM { get; set; }
        public string LOTSize { get; set; }
        public string ColorSTD { get; set; }
        public string Notes { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> QAStatusId { get; set; }
        public string WorkOrderNo { get; set; }
        public int ProductId { get; set; }
        public int LineId { get; set; }
        public Nullable<System.DateTime> ProgressOn { get; set; }
        public Nullable<int> ProgressBy { get; set; }
        public string ProgressNotes { get; set; }
        public Nullable<System.DateTime> ReadyForTestOn { get; set; }
        public Nullable<int> ReadyForTestBy { get; set; }
        public string ReadyForTestNotes { get; set; }
        public Nullable<System.DateTime> TestOn { get; set; }
        public Nullable<int> TestBy { get; set; }
        public string TestNotes { get; set; }
        public Nullable<System.DateTime> CloseOn { get; set; }
        public Nullable<int> CloseBy { get; set; }
        public string CloseNotes { get; set; }
        public Nullable<System.DateTime> VerifyOn { get; set; }
        public Nullable<int> VerifyBy { get; set; }
        public string VerifyNotes { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public int VerNo { get; set; }
        public virtual ICollection<ColourSpecification> ColourSpecifications { get; set; }
        public virtual ICollection<DailyPackingDetail> DailyPackingDetails { get; set; }
        public virtual ICollection<FormulationChangedHistory> FormulationChangedHistories { get; set; }
        public virtual FormulationRequestsStatu FormulationRequestsStatu { get; set; }
        public virtual Line Line { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
        public virtual QAStatu QAStatu { get; set; }
        public virtual ICollection<FormulationRequestsChangeHistory> FormulationRequestsChangeHistories { get; set; }
        public virtual ICollection<FormulationRequestsDetail> FormulationRequestsDetails { get; set; }
        public virtual ICollection<MachineReading> MachineReadings { get; set; }
        public virtual ICollection<ProcessLogSheet1> ProcessLogSheet1 { get; set; }
        public virtual ICollection<ProcessLogSheet2> ProcessLogSheet2 { get; set; }
        public virtual ICollection<QASpecification> QASpecifications { get; set; }
        public virtual ICollection<RMRequestDetail> RMRequestDetails { get; set; }
        public virtual ICollection<RMRequestMaster> RMRequestMasters { get; set; }
    }
}
