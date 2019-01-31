using ProERP.Core.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
    public class VendorService
    {
        private readonly IRepository<Data.Models.Vendor> _VendorRepository;
        public VendorService(IRepository<Data.Models.Vendor> vendorRepository)
        {
            this._VendorRepository = vendorRepository;
        }
        public int Add(Data.Models.Vendor vendor)
        {
            _VendorRepository.Insert(vendor);
            return vendor.Id;
        }
        public void Update(Data.Models.Vendor vendor)
        {
            Data.Models.Vendor existingvendor = _VendorRepository.Table.FirstOrDefault(w => w.Id == vendor.Id);

            if (existingvendor != null)
            {
                existingvendor.Name = vendor.Name;
                existingvendor.Address = vendor.Address;
                existingvendor.PhoneNo = vendor.PhoneNo;
                existingvendor.Email = vendor.Email;
               
                this._VendorRepository.Update(existingvendor);
            }
        }
        public List<Data.Models.Vendor> GetvendorGridData(string Name)
        {
            var allData = from a in this._VendorRepository.Table
                          select a;
            if (!string.IsNullOrEmpty(Name))
                allData = allData.Where(w => w.Name.Contains(Name));
            return allData.ToList();

        }
        public Data.Models.Vendor GetForId(int Id)
        {
            return this._VendorRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
        public int[] Delete(int[] ids)
        {
            List<int> vendorIds = new List<int>();
            foreach (int id in ids)
            {
                Data.Models.Vendor vendor = _VendorRepository.Table.FirstOrDefault(w => w.Id == id);
                _VendorRepository.Delete(vendor);

            }
            return vendorIds.ToArray();
        }
        public List<Data.Models.Vendor> GetAll(int VendorCategoryId)
        {
            return this._VendorRepository.Table.Where(w => w.CategoryId == VendorCategoryId).ToList();
        }

        public List<Data.Models.Vendor> GetAllVendorList()
        {
            return this._VendorRepository.Table.ToList();
        }

        public VendorCategoryModel[] GetVendorCategoryList()
        {
            var allData = from a in this._VendorRepository.Table
                          select new VendorCategoryModel
                          {
                              CategoryId = a.VendorCategory.Id,
                              CategoryName = a.VendorCategory.Name,
                              VendorId = a.Id,
                              VendorName = a.Name
                          };

            return allData.ToArray();
        }
    }
}
