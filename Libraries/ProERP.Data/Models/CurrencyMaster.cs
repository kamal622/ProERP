using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class CurrencyMaster : BaseEntity
    {
        public CurrencyMaster()
        {
            this.IndentDetails = new List<IndentDetail>();
        }

        public string Currency { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
    }
}
