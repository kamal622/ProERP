using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Indent : BaseEntity
    {
        public Indent()
        {
            this.IndentDetails = new List<IndentDetail>();
        }

        public string Note { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int Priority { get; set; }
        public string IndentNo { get; set; }
        public Nullable<int> Status { get; set; }
        public int BudgetId { get; set; }
        public string RequisitionType { get; set; }
        public string BudgetHead { get; set; }
        public string Subject { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public Nullable<int> RejectedBy { get; set; }
        public Nullable<System.DateTime> RejectedOn { get; set; }
        public Nullable<System.DateTime> PoDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string PoNo { get; set; }
        public Nullable<decimal> PoAmount { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public virtual IndentBudget IndentBudget { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual IndentStatu IndentStatu { get; set; }
        public virtual MaintenancePriorityType MaintenancePriorityType { get; set; }
        public virtual User User { get; set; }
    }
}
