using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class TemplateFixColumn : BaseEntity
    {
        public TemplateFixColumn()
        {
            this.TemplateMappings = new List<TemplateMapping>();
        }

        public string Name { get; set; }
        public virtual ICollection<TemplateMapping> TemplateMappings { get; set; }
    }
}
