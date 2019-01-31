using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.MaintenanceRequest
{
   public class MaintenancePriorityTypeServices
    {
        private readonly IRepository<Data.Models.MaintenancePriorityType> _MPTRepository;
        public MaintenancePriorityTypeServices(IRepository<Data.Models.MaintenancePriorityType> mptRepository)
        {
            this._MPTRepository = mptRepository;
        }
        public List<Data.Models.MaintenancePriorityType> GetAll()
        {
            return this._MPTRepository.Table.ToList();
        }
    }
}
