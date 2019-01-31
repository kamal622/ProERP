using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Template : BaseEntity
    {
        public Template()
        {
            this.ReadingDatas = new List<ReadingData>();
            this.TemplateMappings = new List<TemplateMapping>();
        }

        public Nullable<int> PlantId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public virtual ICollection<ReadingData> ReadingDatas { get; set; }
        public virtual ICollection<TemplateMapping> TemplateMappings { get; set; }
    }
}
