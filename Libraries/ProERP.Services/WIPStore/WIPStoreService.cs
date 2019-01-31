using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.WIPStore
{
    public class WIPStoreService
    {
        private readonly IRepository<Data.Models.WIPStore> _wipStoreRepository;
        public WIPStoreService(IRepository<Data.Models.WIPStore> wipStoreRepository)
        {
            this._wipStoreRepository = wipStoreRepository;
        }

        public List<Data.Models.WIPStore> GetWIPStoreList(string Name)
        {
            var allData = from a in this._wipStoreRepository.Table
                          select a;
            if (!string.IsNullOrEmpty(Name))
            {
                allData = allData.Where(W => W.Name.Contains(Name));
            }
            return allData.ToList();
        }

        public int SaveWIPStore(Data.Models.WIPStore wipData)
        {
            if (wipData != null)
            {
                if (wipData.Id > 0)
                {
                    var existingData = this._wipStoreRepository.Table.FirstOrDefault(w => w.Id == wipData.Id);
                    existingData.Name = wipData.Name;
                    existingData.Location = wipData.Location;
                    existingData.Description = wipData.Description;
                    existingData.UpdateOn = DateTime.Now;
                    existingData.UpdateBy = wipData.UpdateBy;
                    this._wipStoreRepository.Update(existingData);
                }
                else
                {
                    this._wipStoreRepository.Insert(wipData);
                }
                return 1;
            }
            return 0;
        }

        public Data.Models.WIPStore GetWIPById(int Id)
        {
            return this._wipStoreRepository.Table.FirstOrDefault(w => w.Id == Id);
        }

        public int DeleteWIP(int Id)
        {
            var olddata = this._wipStoreRepository.Table.FirstOrDefault(W => W.Id == Id);
            if (olddata != null)
            {
                this._wipStoreRepository.Delete(olddata);
                return 1;
            }
            return 0;
        }

        public List<Data.Models.WIPStore> GetWIpForCloseFormulation()
        {
            return this._wipStoreRepository.Table.OrderBy(w => w.Name).ToList();
        }

    }
}
