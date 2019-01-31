using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indent
{
     public class IndentStatusServices
    {
        private readonly IRepository<Data.Models.IndentStatu> _IndentStatusRepository;
        public IndentStatusServices(IRepository<Data.Models.IndentStatu> IndentStatusRepository)
        {
            this._IndentStatusRepository = IndentStatusRepository;
        }

        public List<Data.Models.IndentStatu> GetAll(IList<string> roles)
        {
            //if (roles.Contains("Lavel1")) // Role name must match role name from database. i.e. Roles.Name
            //{
            //    string[] statusList = new string[] { "New", "PO", "Received" };
            //    return this._IndentStatusRepository.Table.Where(w => statusList.Contains(w.Description) ).ToList();
            //}
            //if (roles.Contains("Lavel2")) // Role name must match role name from database. i.e. Roles.Name
            //{
            //    string[] statusList = new string[] { "New", "Approved", "Rejected" };
            //    return this._IndentStatusRepository.Table.Where(w => w.Description != "Approved" && w.Description != "Rejected").ToList();
            //}
            return this._IndentStatusRepository.Table.ToList();
        }
        public List<Data.Models.IndentStatu> GetAllStatus(IList<string> roles)
        {
            return this._IndentStatusRepository.Table.Where(w => w.Id != 5).ToList();
        }
    }
}
