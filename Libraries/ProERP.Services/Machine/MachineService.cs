using ProERP.Core.Data;
using ProERP.Data.Models;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Machine
{
    public class MachineService
    {
        private readonly IRepository<Data.Models.Machine> _machineRepository;
        private readonly IRepository<Data.Models.MachineType> _machineTypeRepository;
        private readonly IRepository<Data.Models.LineMachineActiveHistory> _lineMachineActiveHistoryRepository;

        public MachineService(IRepository<Data.Models.Machine> machineRepository, 
            IRepository<Data.Models.MachineType> machineTypeRepository,
            IRepository<Data.Models.LineMachineActiveHistory> lineMachineActiveHistoryRepository)
        {
            this._machineRepository = machineRepository;
            this._machineTypeRepository = machineTypeRepository;
            this._lineMachineActiveHistoryRepository = lineMachineActiveHistoryRepository;
        }

        public int Add(Data.Models.Machine machine)
        {
            _machineRepository.Insert(machine);
            return machine.Id;
        }

        public void Update(Data.Models.Machine machine,int userId)
        {
            Data.Models.Machine oldMachine = _machineRepository.Table.Where(w => w.Id == machine.Id).FirstOrDefault();
            if (oldMachine != null)
            {
                if (machine.IsActive != oldMachine.IsActive)
                {
                    this._lineMachineActiveHistoryRepository.Insert(new LineMachineActiveHistory
                    {
                        LineId = machine.LineId,
                        MachineId = machine.Id,
                        IsActive = machine.IsActive,
                        UpdateBy = userId,
                        UpdateDate = DateTime.UtcNow,
                    });
                }

                oldMachine.Name = machine.Name;
                oldMachine.Make = machine.Make;
                oldMachine.Model = machine.Model;
                oldMachine.MachineInCharge = machine.MachineInCharge;
                oldMachine.InstallationDate = machine.InstallationDate;
                oldMachine.MachineType = machine.MachineType;
                oldMachine.Description = machine.Description;
                oldMachine.ParentId = machine.ParentId;
                oldMachine.PlantId = machine.PlantId;
                oldMachine.MachineTypeId = machine.MachineTypeId;
                oldMachine.LineId = machine.LineId;
                oldMachine.IsActive = machine.IsActive;
                _machineRepository.Update(oldMachine);
            }
        }
        public void UpdateIsShutdown(int machineId, bool IsShutdown)
        {
            Data.Models.Machine oldMachine = _machineRepository.Table.Where(w => w.Id == machineId).FirstOrDefault();

            if (oldMachine != null)
            {
                oldMachine.IsShutdown = IsShutdown;
                _machineRepository.Update(oldMachine);
            }
        }

        public List<Data.Models.Machine> GetAll(string Name, int SiteId, int PlantId, int LineId)
        {
            var allData = this._machineRepository.Table;

            if (!string.IsNullOrEmpty(Name))
                allData = allData.Where(w => w.Name.ToLower().Contains(Name.Trim().ToLower()));

            if (SiteId != 0)
                allData = allData.Where(w => w.Plant.SiteId == SiteId);
            // allData = allData.Where(w => w.LineMachineMappings.Any(a => a.Line.Plant.SiteId == SiteId));

            if (PlantId != 0)
                allData = allData.Where(w => w.PlantId == PlantId);
            // allData = allData.Where(w => w.LineMachineMappings.Any(a => a.Line.PlantId== PlantId));

            if (LineId != 0)
                allData = allData.Where(w => w.LineId == LineId);
            //allData = allData.Where(w => w.LineMachineMappings.Any(a => a.LineId == LineId));
            return allData.ToList();
        }

        public Data.Models.Machine[] GetAllForLine(int LineId)
        {
            return _machineRepository.Table.Where(w => w.LineId == LineId).ToArray();
        }

        //public Data.Models.Machine[] GetAllForPlant(int PlantId)
        //{
        //    return _machineRepository.Table.Where(w => w.Line.PlantId == PlantId).ToArray();
        //}

        public IQueryable<Data.Models.Machine> GetAllLineMachineforPlant(int plantId)
        {
            return _machineRepository.Table.Where(w => w.PlantId == plantId && w.MachineType.Name == "Machine").OrderBy(o => o.Name);
        }

        public IQueryable<Data.Models.Machine> GetMachinesForLine(int lineId)
        {
            return _machineRepository.Table.Where(w => w.LineId == lineId).OrderBy(w => w.Name);
        }

        public MachineViewModel GetMachineById(int Id)
        {
            return _machineRepository.Table.Where(w => w.Id == Id).Select(s => new MachineViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Make = s.Make,
                Model = s.Model,
                MachineInCharge = s.MachineInCharge,
                InstallationDate = s.InstallationDate,
                Description = s.Description,
                LineId = s.LineId,
                SiteId = 1,
                PlantId = s.PlantId,
                MachineTypeId = s.MachineTypeId,
                IsActive = s.IsActive
            }).FirstOrDefault();
        }

        public IQueryable<Data.Models.Machine> GetSubAssembliesForPlant(int plantId)
        {
            return this._machineRepository.Table.Where(w => w.PlantId == plantId && w.MachineType.Name == "SubAssembly");
        }

        //public int GetPlantIdByLineId(int LineId)
        //{
        //    return _machineRepository.Table.Where(w=>w.LineId==LineId).Select(s => s.Line.PlantId.Value).FirstOrDefault();
        //}

        //public int GetSiteIdByLineId(int LineId)
        //{
        //    return _machineRepository.Table.Where(w => w.LineId == LineId).Select(s => s.Line.Plant.SiteId.Value).FirstOrDefault();
        //}
        public int[] Delete(int[] ids)
        {
            List<int> machineIds = new List<int>();
            foreach (int id in ids)
            {
                //if (_machineRepository.Table.Any(a => a.SubAssemblies.Any(b => b.MachineId == id)))
                //    machineIds.Add(id);
                //else
                //{
                Data.Models.Machine Machine = _machineRepository.Table.FirstOrDefault(w => w.Id == id);
                _machineRepository.Delete(Machine);
                //}

            }
            return machineIds.ToArray();
        }

        public MachineType[] GetAllMachineType()
        {
            return this._machineTypeRepository.Table.ToArray();
        }

        public List<Data.Models.Machine> GetAllMachine()
        {
            return _machineRepository.Table.OrderBy(o => o.Name).ToList();
        }
       
        public Data.Models.Machine GetMachineId(int machineId)
        {
            return _machineRepository.Table.FirstOrDefault(w => w.Id == machineId);
        }

        public int GetMachineIdForAll(int LineId)
        {
            int machineid= _machineRepository.Table.Where(w => w.LineId == LineId && w.Name == "ALL").Select(s=>s.Id).FirstOrDefault();
            return machineid;
        }

        public List<Data.Models.Machine> GetMachineByLineId(int Id)
        {
            return this._machineRepository.Table.Where(W => W.LineId == Id).OrderBy(o => o.Name).ToList();
        }
    }
}
