using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
  public  class BreakDownAttachmentGridModel
    {
        public int Id { get; set; }
        public int BreakDownId { get; set; }
        public string OriginalFileName { get; set; }
        public string SysFileName { get; set; }
    }
}
