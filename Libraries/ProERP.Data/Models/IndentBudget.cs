using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class IndentBudget : BaseEntity
    {
        public IndentBudget()
        {
            this.Indents = new List<Indent>();
        }

        public string BudgetType { get; set; }
        public string BudgetCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> PlantId { get; set; }
        public Nullable<int> ItemId { get; set; }
        public bool IsActive { get; set; }
        public string FinancialYear { get; set; }
        public virtual Item Item { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<Indent> Indents { get; set; }
    }
}
