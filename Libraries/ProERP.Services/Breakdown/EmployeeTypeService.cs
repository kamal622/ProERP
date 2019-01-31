using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Breakdown
{
    public class EmployeeTypeService
    {
        private readonly IRepository<Data.Models.EmployeeType> _ETRepository;

        public EmployeeTypeService(IRepository<Data.Models.EmployeeType> etRepository)
        {
            this._ETRepository = etRepository;
        }

        public List<Data.Models.EmployeeType> GetAllEmployeeType()
        {
            var allData = this._ETRepository.Table;

            return allData.ToList();
        }
    }
}
