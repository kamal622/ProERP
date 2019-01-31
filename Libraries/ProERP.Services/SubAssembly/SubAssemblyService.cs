using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.SubAssembly
{
   public class SubAssemblyService
    {
        private readonly IRepository<Data.Models.SubAssembly> _subAssemblyRepository;

        public SubAssemblyService(IRepository<Data.Models.SubAssembly> subAssemblyRepository)
        {
            this._subAssemblyRepository = subAssemblyRepository;
        }
        public int Add(Data.Models.SubAssembly subAssembly)
        {
            _subAssemblyRepository.Insert(subAssembly);
            return subAssembly.Id;
        }

        public object GetSubAssemblyById(int Id)
        {
            return _subAssemblyRepository.Table.Where(w => w.Id == Id).Select(s => new
            {
                s.Id,
                s.Name,
                s.Description,
                s.MachineId,
                LineId=s.Machine.LineId,
                SiteId = s.Machine.Line.Plant.SiteId,
                PlantId = s.Machine.Line.PlantId
            }).FirstOrDefault();
        }

        public void Update(Data.Models.SubAssembly subAssembly)
        {
            //Data.Models.Line oldLine = _userRepository.Table.Where(w => w.Id == line.Id).FirstOrDefault();

            //if (oldLine != null)
            //{

            //}
            //else
            //    return 0;
            _subAssemblyRepository.Update(subAssembly);
        }

        // IMPORTANT!!!
        //TODO: change function name and return type.
       
        public List<Data.Models.SubAssembly> GetAll(string Name, int SiteId, int PlantId, int LineId,int MachineId)
        {
            var allData = this._subAssemblyRepository.Table;

            if (!string.IsNullOrEmpty(Name))
                allData = allData.Where(w => w.Name.ToLower().Contains(Name.Trim().ToLower()));

            if (SiteId != 0)
                allData = allData.Where(w => w.Machine.Line.Plant.SiteId == SiteId);

            if (PlantId != 0)
                allData = allData.Where(w => w.Machine.Line.PlantId == PlantId);

            if (LineId != 0)
                allData = allData.Where(w => w.Machine.LineId == LineId);

            if (MachineId != 0)
                allData = allData.Where(w => w.MachineId == MachineId);

            return allData.ToList();
        }

        public IQueryable<Data.Models.SubAssembly> GetAllForPlant(int PlantId)
        {
            return this._subAssemblyRepository.Table.Where(w => w.Machine.Line.PlantId == PlantId);
        }
    }
}
