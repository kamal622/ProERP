using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class ListUserViewModel
    {
        public int Id { get; set; }
        public int? UserProfile_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
    }
}
