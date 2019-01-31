using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
    public class PreventiveReviewHistoryService
    {
        private readonly IRepository<Data.Models.PreventiveReviewHistory> _PRHRepository;
        private readonly IRepository<Data.Models.ShutdownHistory> _shutdownHistoryRepository;
        private readonly IRepository<Data.Models.Line> _lineRepository;
        private readonly IRepository<Data.Models.Machine> _machineRepository;
        private readonly IRepository<Data.Models.User> _userRepository;

        public PreventiveReviewHistoryService(IRepository<Data.Models.PreventiveReviewHistory> prhRepository,
            IRepository<Data.Models.ShutdownHistory> shutdownHistoryRepository,
            IRepository<Data.Models.Line> lineRepository,
            IRepository<Data.Models.Machine> machineRepository,
            IRepository<Data.Models.User> userRepository)
        {
            this._PRHRepository = prhRepository;
            this._shutdownHistoryRepository = shutdownHistoryRepository;
            this._lineRepository = lineRepository;
            this._machineRepository = machineRepository;
            this._userRepository = userRepository;
        }

        public int Add(Data.Models.PreventiveReviewHistory PRH)
        {
            _PRHRepository.Insert(PRH);
            return PRH.Id;
        }

        public DateTime GetLastShutDownDate(int lineId, int? machineId = null)
        {
            if (!this._shutdownHistoryRepository.Table.Any(f => f.LineId == lineId && f.MachineId == machineId))
                return DateTime.MinValue;

            return this._shutdownHistoryRepository.Table.Where(f => f.LineId == lineId && f.MachineId == machineId).Max(m => m.ShutdownDate);
        }

        public bool AnyMachineShutdown(int lineId)
        {
            return this._machineRepository.Table.Any(a => a.LineId == lineId && a.IsActive && a.IsShutdown == true);
        }

        public bool AnyShutdownReviewRemain(int lineId)
        {
            var shutdownId = this._shutdownHistoryRepository.Table.Where(w => w.LineId == lineId && w.MachineId == null && w.StartDate == null).Select(s => s.Id).FirstOrDefault();
            return this._PRHRepository.Table.Any(a => a.ShutdownId == shutdownId && a.ReviewDate == null && a.PreventiveMaintenance.IsDeleted == false);
        }
        public bool AnyShutdownReviewRemainMachine(int machineId)
        {
            var shutdownId = this._shutdownHistoryRepository.Table.Where(w => w.MachineId == machineId && w.StartDate == null).Select(s => s.Id).FirstOrDefault();
            return this._PRHRepository.Table.Any(a => a.ShutdownId == shutdownId && a.ReviewDate == null && a.PreventiveMaintenance.IsDeleted == false);
        }

        public Models.ShutdownGridModel[] GetShutdownLineGridData()
        {
            //int? machineId = null;
            DateTime? shutdownDate = null;
            //var shutDownHistory = from a in this._shutdownHistoryRepository.Table
            //                      join b in this._lineRepository.Table on new { Key1 = a.LineId, Key2 = a.MachineId } equals new { Key1 = b.Id, Key2 = machineId }
            //                      where a.Id == b.ShutdownHistories.Max(m => m.Id)
            //                      select a;

            var query = from a in this._lineRepository.Table
                        join b in this._shutdownHistoryRepository.Table on new { Key1 = a.Id, Key2 = a.ShutdownHistories.Where(w => w.MachineId == null).Max(m => m.Id) } equals new { Key1 = b.LineId, Key2 = b.Id } into j1
                        from b in j1.DefaultIfEmpty()
                        join c in this._userRepository.Table on b.ShutdownBy equals c.Id into j2
                        from c in j2.DefaultIfEmpty()
                        where a.IsActive
                        select new Models.ShutdownGridModel
                        {
                            Id = a.Id,
                            PlantId = a.Plant.Id,
                            LineId = a.Id,
                            MachineId = 0,
                            LineName = a.Name,
                            MachineName = "",
                            PlantName = a.Plant.Name,
                            ShutdownBy = (c != null) ? c.UserName : "",
                            ShutdownDate = (b != null) ? b.ShutdownDate : shutdownDate,
                            IsShutdown = a.IsShutdown
                        };

            return query.ToArray();
        }

        public Models.ShutdownGridModel[] GetShutdownMachineGridData()
        {
            DateTime? shutdownDate = null;

            //var shutDownHistory = from a in this._shutdownHistoryRepository.Table
            //                      join b in this._lineRepository.Table on a.LineId equals b.Id
            //                      join c in this._machineRepository.Table on a.MachineId equals c.Id
            //                      where a.Id == c.ShutdownHistories.Max(m => m.Id)
            //                      select a;

            var query = from a in this._machineRepository.Table
                        join b in this._shutdownHistoryRepository.Table on new { Key1 = a.Id, Key2 = a.ShutdownHistories.Max(m => m.Id) } equals new { Key1 = b.MachineId ?? b.MachineId.Value, Key2 = b.Id } into j1
                        from b in j1.DefaultIfEmpty()
                        join c in this._userRepository.Table on b.ShutdownBy equals c.Id into j2
                        from c in j2.DefaultIfEmpty()
                        where a.IsActive
                        select new Models.ShutdownGridModel
                        {
                            Id = a.Id,
                            PlantId = a.Plant.Id,
                            LineId = a.Line.Id,
                            MachineId = a.Id,
                            LineName = a.Line.Name,
                            MachineName = a.Name,
                            PlantName = a.Plant.Name,
                            ShutdownBy = (c != null) ? c.UserName : "",
                            ShutdownDate = (b != null) ? b.ShutdownDate : shutdownDate,
                            IsShutdown = a.IsShutdown
                        };

            return query.ToArray();
        }
    }
}
