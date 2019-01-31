using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
   public class ScheduleTypeService
    {
        private readonly IRepository<Data.Models.PreventiveScheduleType> _PSTRepository;
        public ScheduleTypeService(IRepository<Data.Models.PreventiveScheduleType> pstRepository)
        {
            this._PSTRepository = pstRepository;
        }

        public List<Data.Models.PreventiveScheduleType> GetAll()
        {
            return this._PSTRepository.Table.ToList();
        }
    }
}
