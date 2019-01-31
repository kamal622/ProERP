using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Site
{
    public class SiteService
    {
        private readonly IRepository<Data.Models.Site> _siteRepository;
        //  private readonly IRepository<Data.Models.Plant> _
        public SiteService(IRepository<Data.Models.Site> siteRepository)
        {
            this._siteRepository = siteRepository;
        }

        public int Add(Data.Models.Site site)
        {
            _siteRepository.Insert(site);
            return site.Id;
        }

        public int Update(Data.Models.Site site)
        {
            Data.Models.Site oldSite = _siteRepository.Table.FirstOrDefault(w => w.Id == site.Id);

            if (oldSite != null)
            {
                oldSite.Name = site.Name;
                oldSite.Address = site.Address;
                oldSite.InCharge = site.InCharge;
                oldSite.Description = site.Description;
                _siteRepository.Update(oldSite);
                return site.Id;
            }
            else
                return 0;
        }

        public int[] Delete(int[] ids)
        {
            List<int> siteIds = new List<int>();
            foreach (int id in ids)
            {
                if (_siteRepository.Table.Any(a => a.Plants.Any(b => b.SiteId == id)))
                    siteIds.Add(id);
                else
                {
                    Data.Models.Site Site = _siteRepository.Table.FirstOrDefault(w => w.Id == id);
                    _siteRepository.Delete(Site);
                }

            }
            return siteIds.ToArray();
        }

        public List<Data.Models.Site> GetAll(string name)
        {
            var allData = this._siteRepository.Table;

            if (!string.IsNullOrEmpty(name))
                allData = allData.Where(w => w.Name.ToLower().Contains(name.Trim().ToLower()));

            return allData.ToList();
        }

        public Data.Models.Site GetSiteById(int Id)
        {
            return _siteRepository.Table.FirstOrDefault(w => w.Id == Id);
        }

        public string getSiteNameById(int Id)
        {
            return _siteRepository.Table.FirstOrDefault(w => w.Id == Id).Name;

        }


    }
}
