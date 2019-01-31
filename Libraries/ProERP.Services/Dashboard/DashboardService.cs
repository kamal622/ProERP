using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProERP.Core.Data;
using ProERP.Data.Models;
using ProERP.Services.Models;

namespace ProERP.Services.Dashboard
{
    public class DashboardService
    {
        private readonly IRepository<Data.Models.ShutdownHistory> _shutdownHistoryRepository;
        public DashboardService(IRepository<Data.Models.ShutdownHistory> shutdownHistoryRepository)
        {
            this._shutdownHistoryRepository = shutdownHistoryRepository;
        }
        public int Add(Data.Models.ShutdownHistory shutdownhistory)
        {
            _shutdownHistoryRepository.Insert(shutdownhistory);
            return shutdownhistory.Id;
        }
        public int GetShutdownIdForLine(int lineId)
        {
            return  this._shutdownHistoryRepository.Table.Where(w => w.LineId == lineId && w.StartBy == null && w.StartDate == null).Select(s => s.Id).FirstOrDefault();
        }
        public void UpdateShutdownHistoryForLine(int shutdownId, int userId)
        {
            Data.Models.ShutdownHistory oldShutdownHistory = _shutdownHistoryRepository.Table.Where(w => w.Id == shutdownId).FirstOrDefault();
            if (oldShutdownHistory != null)
            {
                oldShutdownHistory.StartDate = DateTime.UtcNow.Date;
                oldShutdownHistory.StartBy = userId;
                _shutdownHistoryRepository.Update(oldShutdownHistory);
            }
        }
        public int UpdateShutdownHistoryForMachine(int machineId)
        {
            return this._shutdownHistoryRepository.Table.Where(w => w.MachineId== machineId && w.StartBy == null && w.StartDate == null).Select(s => s.Id).FirstOrDefault();
        }
        public void UpdateShutdownHistoryFormachine(int shutdownId, int userId)
        {
            Data.Models.ShutdownHistory oldShutdownHistory = _shutdownHistoryRepository.Table.Where(w => w.Id == shutdownId).FirstOrDefault();
            if (oldShutdownHistory != null)
            {
                oldShutdownHistory.StartDate = DateTime.UtcNow.Date;
                oldShutdownHistory.StartBy = userId;
                _shutdownHistoryRepository.Update(oldShutdownHistory);
            }
        }

    }
}
