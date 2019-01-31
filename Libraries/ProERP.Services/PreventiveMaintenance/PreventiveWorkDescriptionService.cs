using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
   public class PreventiveWorkDescriptionService
    {
        private readonly IRepository<Data.Models.PreventiveWorkDescription> _PWDRepository;
        public PreventiveWorkDescriptionService(IRepository<Data.Models.PreventiveWorkDescription> pwdRepository)
        {
            this._PWDRepository = pwdRepository;
        }

        public List<Data.Models.PreventiveWorkDescription> GetAll()
        {
            return this._PWDRepository.Table.OrderBy(o=> o.Description).ToList();
        }
    }
}
