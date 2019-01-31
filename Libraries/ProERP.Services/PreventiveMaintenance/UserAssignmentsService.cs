using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
    public class UserAssignmentsService
    {
        private readonly IRepository<Data.Models.UserAssignment> _UARepository;
        public UserAssignmentsService(IRepository<Data.Models.UserAssignment> _uaRepository)
        {
            this._UARepository = _uaRepository;
        }
        public int Add(Data.Models.UserAssignment UA)
        {
            if (!this._UARepository.Table.Any(a => a.PreventiveMaintenanceId == UA.PreventiveMaintenanceId && a.UserId == UA.UserId))
                _UARepository.Insert(UA);
            return UA.Id;
        }


        public void DeleteUA(int PMId, List<int> userIds)
        {
            var deleteUAs = this._UARepository.Table.Where(w => w.PreventiveMaintenanceId == PMId && !userIds.Contains(w.UserId));
            this._UARepository.Delete(deleteUAs);
        }

      

    }
}

