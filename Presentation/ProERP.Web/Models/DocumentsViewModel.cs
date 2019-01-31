using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class DocumentsViewModel
    {
    }

    public class FileUploadModel
    {
        public string OriginalFileName { get; set; }
        public string SysFileName { get; set; }
        public string ZipFileName { get; set; }
    }
}