using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class PlantBudget : BaseEntity
    {
        public Nullable<int> PlantId { get; set; }
        public Nullable<decimal> MonthlyBudget { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public virtual Plant Plant { get; set; }
    }
}
