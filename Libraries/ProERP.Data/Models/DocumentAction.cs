using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class DocumentAction : BaseEntity
    {
        public DocumentAction()
        {
            this.DocumentHistories = new List<DocumentHistory>();
        }

        public string Name { get; set; }
        public virtual ICollection<DocumentHistory> DocumentHistories { get; set; }
    }
}
