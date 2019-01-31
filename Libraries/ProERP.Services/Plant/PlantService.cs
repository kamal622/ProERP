using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Plant
{
    public class PlantService
    {

        private readonly IRepository<Data.Models.Plant> _plantRepository;
        public PlantService(IRepository<Data.Models.Plant> plantRepository)
        {
            this._plantRepository = plantRepository;
        }

        public int Add(Data.Models.Plant plant)
        {
            _plantRepository.Insert(plant);
            return plant.Id;
        }

        public int Update(Data.Models.Plant plant)
        {
            Data.Models.Plant oldPlant = _plantRepository.Table.FirstOrDefault(w => w.Id == plant.Id);

            if (oldPlant != null)
            {
                oldPlant.Name = plant.Name;
                oldPlant.Location = plant.Location;
                oldPlant.PlantInCharge = plant.PlantInCharge;
                oldPlant.SiteId = plant.SiteId;
                _plantRepository.Update(oldPlant);
                return plant.Id;
            }
            else
                return 0;
        }

        public int[] Delete(int[] ids)
        {
            List<int> siteIds = new List<int>();
            foreach (int id in ids)
            {
                if (_plantRepository.Table.Any(a => a.Machines.Any(b => b.PlantId == id)))
                    siteIds.Add(id);
                else
                {
                    Data.Models.Plant Plant = _plantRepository.Table.FirstOrDefault(w => w.Id == id);
                    _plantRepository.Delete(Plant);
                }

            }
            return siteIds.ToArray();
        }

        public List<Data.Models.Plant> GetAll(string name,int SiteId)
        {
            var allData = this._plantRepository.Table;

            if (!string.IsNullOrEmpty(name))
                allData = allData.Where(w => w.Name.ToLower().Contains(name.Trim().ToLower()));

            if(SiteId!=0)
                allData = allData.Where(w => w.SiteId==SiteId);

            return allData.ToList();
        }
       

        public Data.Models.Plant GetPlantById(int Id)
        {
            return _plantRepository.Table.FirstOrDefault(w => w.Id == Id);
        }

        public Data.Models.Plant[] GetPlantsForSite(int siteId)
        {
            return this._plantRepository.Table.Where(w => w.SiteId == siteId).OrderBy(w =>w.Name).ToArray();
        }

        public List<Data.Models.Plant> GetAllPlants()
        {
            return this._plantRepository.Table.OrderBy(w =>w.Name).ToList();
        }
        public string GetPlantName(int PlantId)
        {
            return _plantRepository.Table.Where(w => w.Id == (PlantId == 0 ? w.Id : PlantId)).Select(s=>s.Name).FirstOrDefault();
        }
    }
}
