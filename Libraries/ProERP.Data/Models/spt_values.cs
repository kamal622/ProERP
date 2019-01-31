using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class spt_values : BaseEntity
    {
        public string name { get; set; }
        public int number { get; set; }
        public string type { get; set; }
        public Nullable<int> low { get; set; }
        public Nullable<int> high { get; set; }
        public Nullable<int> status { get; set; }
    }
}
