using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.MaintenanceRequest
{
    public class MaintenanceUserAssignmentsServices
    {
        private readonly IRepository<Data.Models.MaintenanceUserAssignment> _MUARepository;
        public MaintenanceUserAssignmentsServices(IRepository<Data.Models.MaintenanceUserAssignment> muaRepository)
        {
            this._MUARepository = muaRepository;
        }

        public int Add(Data.Models.MaintenanceUserAssignment MUA)
        {
            if (!this._MUARepository.Table.Any(a => a.MaintenanceRequestId == MUA.MaintenanceRequestId && a.UserId == MUA.UserId))
                _MUARepository.Insert(MUA);
            return MUA.Id;
        }

        public void DeleteMUA(int MRId, List<int> userIds)
        {
            var deleteMUAs = this._MUARepository.Table.Where(w => w.MaintenanceRequestId == MRId && !userIds.Contains(w.UserId));
            this._MUARepository.Delete(deleteMUAs);
        }

    }
}
