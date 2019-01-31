using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class DocumentType : BaseEntity
    {
        public DocumentType()
        {
            this.Documents = new List<Document>();
        }

        public Nullable<int> ParentCategoryId { get; set; }
        public string Desription { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
