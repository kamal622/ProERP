using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class VersionNote : BaseEntity
    {
        public int VersionId { get; set; }
        public string Notes { get; set; }
        public virtual PLMMVersion PLMMVersion { get; set; }
    }
}
