using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Document : BaseEntity
    {
        public Document()
        {
            this.DocumentHistories = new List<DocumentHistory>();
        }

        public string OriginalFileName { get; set; }
        public string SysFileName { get; set; }
        public string ZipFileName { get; set; }
        public string RelativePath { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> Type { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public int PlantId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<DocumentHistory> DocumentHistories { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual Line Line { get; set; }
        public virtual Machine Machine { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual User User { get; set; }
    }
}
