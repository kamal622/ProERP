using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class IndentDetailAttachment : BaseEntity
    {
        public int IndentDetailId { get; set; }
        public string OriginalFileName { get; set; }
        public string SysFileName { get; set; }
        public virtual IndentDetail IndentDetail { get; set; }
    }
}
