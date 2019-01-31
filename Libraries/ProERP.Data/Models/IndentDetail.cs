using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class IndentDetail : BaseEntity
    {
        public IndentDetail()
        {
            this.IndentDetailAttachments = new List<IndentDetailAttachment>();
        }

        public int PreferredVendorId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public decimal QtyNeeded { get; set; }
        public string JobDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Make { get; set; }
        public int StatusId { get; set; }
        public Nullable<bool> IsApprove { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public Nullable<int> RejectedBy { get; set; }
        public Nullable<System.DateTime> Rejectedon { get; set; }
        public Nullable<int> IssuedBy { get; set; }
        public Nullable<System.DateTime> IssuedOn { get; set; }
        public Nullable<int> PlantId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public int IndentId { get; set; }
        public decimal EstimatePrice { get; set; }
        public Nullable<decimal> FinalPrice { get; set; }
        public Nullable<System.DateTime> RequiredByDate { get; set; }
        public Nullable<System.DateTime> PoDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string PoNo { get; set; }
        public Nullable<decimal> IssuedQty { get; set; }
        public decimal PoAmount { get; set; }
        public Nullable<int> Currency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public virtual CurrencyMaster CurrencyMaster { get; set; }
        public virtual Indent Indent { get; set; }
        public virtual IndentStatu IndentStatu { get; set; }
        public virtual Item Item { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<IndentDetailAttachment> IndentDetailAttachments { get; set; }
    }
}
