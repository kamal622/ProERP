using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class FormulationMaster : BaseEntity
    {
        public int ProductId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public int ItemId { get; set; }
        public decimal BaseValue { get; set; }
        public decimal PreviousBaseValue { get; set; }
        public virtual RMItem RMItem { get; set; }
        public virtual ProductMaster ProductMaster { get; set; }
    }
}
