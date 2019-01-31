using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
    public class VendorCategoryService
    {
        private readonly IRepository<Data.Models.VendorCategory> _VCRepository;
        public VendorCategoryService(IRepository<Data.Models.VendorCategory> vcRepository)
        {
            this._VCRepository = vcRepository;
        }

        public List<Data.Models.VendorCategory> GetAll()
        {
            return this._VCRepository.Table.ToList();
        }
    }
}
