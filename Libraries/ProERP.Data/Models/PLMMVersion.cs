using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class PLMMVersion : BaseEntity
    {
        public PLMMVersion()
        {
            this.VersionNotes = new List<VersionNote>();
        }

        public System.DateTime ReleaseDate { get; set; }
        public string ReleaseVersion { get; set; }
        public virtual ICollection<VersionNote> VersionNotes { get; set; }
    }
}
