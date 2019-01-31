using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class Site : BaseEntity
    {
        public Site()
        {
            this.Plants = new List<Plant>();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string InCharge { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Plant> Plants { get; set; }
    }
}
