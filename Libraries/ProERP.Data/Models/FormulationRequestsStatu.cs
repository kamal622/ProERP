using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class FormulationRequestsStatu : BaseEntity
    {
        public FormulationRequestsStatu()
        {
            this.FormulationRequests = new List<FormulationRequest>();
            this.FormulationRequestsChangeHistories = new List<FormulationRequestsChangeHistory>();
        }

        public string StatusName { get; set; }
        public virtual ICollection<FormulationRequest> FormulationRequests { get; set; }
        public virtual ICollection<FormulationRequestsChangeHistory> FormulationRequestsChangeHistories { get; set; }
    }
}
