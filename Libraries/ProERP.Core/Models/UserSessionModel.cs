using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Core.Models
{
    public class UserSessionModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PMPendingCount { get; set; }
        public int PMOverDueCount { get; set; }
        public int PMShutDownCount { get; set; }
    }
}
