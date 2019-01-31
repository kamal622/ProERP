using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Part
{
   public class PartService
    {

        private readonly IRepository<Data.Models.Part> _partRepository;

        public PartService(IRepository<Data.Models.Part> partRepository)
        {
            this._partRepository = partRepository;
        }
        public int Add(Data.Models.Part part)
        {
            _partRepository.Insert(part);
            return part.Id;
        }

        public int Update(Data.Models.Part part)
        {
            Data.Models.Part oldpart = _partRepository.Table.FirstOrDefault(w => w.Id == part.Id);

            if (oldpart != null)
            {
                //oldpart.PlantId = part.PlantId;
                //oldpart.LineId = part.LineId;
                //oldpart.MachineId = part.MachineId;
                oldpart.Name = part.Name;
                oldpart.Description = part.Description;
                

                _partRepository.Update(oldpart);

                return oldpart.Id;
            }
            else
                return 0;
        }

        public List<Data.Models.Part> GetAll(string Name)
        {
            var allData = this._partRepository.Table;

            if (!string.IsNullOrEmpty(Name))
                allData = allData.Where(w => w.Name.ToLower().Contains(Name.Trim().ToLower()));

            //if (PlantId != 0)
            //    allData = allData.Where(w => w.PlantId == PlantId);
            //if (PlantId != 0)
            //    allData = allData.Where(w => w.LineId == LineId);
            //if (MachineId != 0)
            //    allData = allData.Where(w => w.MachineId == MachineId);
           
            return allData.ToList();
        }

        public Data.Models.Part GetForId(int Id)
        {
            return this._partRepository.Table.FirstOrDefault(f => f.Id == Id);
        }

        public int[] Delete(int[] ids)
        {
            List<int> PartIds = new List<int>();
            foreach (int id in ids)
            {
                
                    Data.Models.Part Part = _partRepository.Table.FirstOrDefault(w => w.Id == id);
                    _partRepository.Delete(Part);
                

            }
            return PartIds.ToArray();
        }

        public List<Data.Models.Part> GetAllPart()
        {
            var allData = this._partRepository.Table;

            return allData.ToList();
        }

        public int partIdForbreakdown(string name)
        {            
            if (!this._partRepository.Table.Any(w => w.Name == name))
            {
                Data.Models.Part part = new Data.Models.Part
                {
                    Name = name,
                    Description = name
                };
                _partRepository.Insert(part);
                return part.Id;
            }
            else
            {
                return this._partRepository.Table.Where(w => w.Name == name).Select(s => s.Id).FirstOrDefault();
            }
        }
    }
}
