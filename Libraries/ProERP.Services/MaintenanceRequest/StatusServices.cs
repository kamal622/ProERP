using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.MaintenanceRequest
{
   public class StatusServices
    {
        private readonly IRepository<Data.Models.Status> _StatusRepository;
        private readonly IRepository<Data.Models.MaintanceRequestStatu> _MaintanceRequestStatusRepository;
        public StatusServices(IRepository<Data.Models.Status> statusRepository, IRepository<Data.Models.MaintanceRequestStatu> maintanceRequestStatusRepository)
        {
            this._StatusRepository = statusRepository;
            this._MaintanceRequestStatusRepository = maintanceRequestStatusRepository;
        }

        public List<Data.Models.Status> GetAll()
        {
            return this._StatusRepository.Table.ToList();
        }
        public List<Data.Models.Status> GetselectedData()
        {
            var allData = from a in this._StatusRepository.Table
                          where a.Id==5 || a.Id ==6 || a.Id==2
                          select a;

            return allData.ToList();
        }

        public List<Data.Models.MaintanceRequestStatu> GetStatusAll()
        {
            return this._MaintanceRequestStatusRepository.Table.ToList();
        }
    }
}
