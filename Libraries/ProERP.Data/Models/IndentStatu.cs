using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class IndentStatu : BaseEntity
    {
        public IndentStatu()
        {
            this.IndentDetails = new List<IndentDetail>();
            this.Indents = new List<Indent>();
        }

        public string Description { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual ICollection<Indent> Indents { get; set; }
    }
}
