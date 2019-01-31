using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class QAStatu : BaseEntity
    {
        public QAStatu()
        {
            this.FormulationRequests = new List<FormulationRequest>();
        }

        public string Name { get; set; }
        public virtual ICollection<FormulationRequest> FormulationRequests { get; set; }
    }
}
