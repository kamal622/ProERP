using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class ReadingData : BaseEntity
    {
        public int TemplateId { get; set; }
        public int PlantId { get; set; }
        public int LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string C5 { get; set; }
        public string C6 { get; set; }
        public string C7 { get; set; }
        public string C8 { get; set; }
        public string C9 { get; set; }
        public string C10 { get; set; }
        public string C11 { get; set; }
        public string C12 { get; set; }
        public string C13 { get; set; }
        public string C14 { get; set; }
        public string C15 { get; set; }
        public string C16 { get; set; }
        public string C17 { get; set; }
        public string C18 { get; set; }
        public string C19 { get; set; }
        public string C20 { get; set; }
        public string C21 { get; set; }
        public string C22 { get; set; }
        public string C23 { get; set; }
        public string C24 { get; set; }
        public string C25 { get; set; }
        public string C26 { get; set; }
        public string C27 { get; set; }
        public string C28 { get; set; }
        public string C29 { get; set; }
        public string C30 { get; set; }
        public string C31 { get; set; }
        public string C32 { get; set; }
        public string C33 { get; set; }
        public string C34 { get; set; }
        public string C35 { get; set; }
        public string C36 { get; set; }
        public string C37 { get; set; }
        public string C38 { get; set; }
        public string C39 { get; set; }
        public string C40 { get; set; }
        public string C41 { get; set; }
        public string C42 { get; set; }
        public string C43 { get; set; }
        public string C44 { get; set; }
        public string C45 { get; set; }
        public string C46 { get; set; }
        public string C47 { get; set; }
        public string C48 { get; set; }
        public string C49 { get; set; }
        public string C50 { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public virtual Template Template { get; set; }
    }
}
