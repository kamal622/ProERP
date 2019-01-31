using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
    public class PreventiveHoldHistoryService
    {
        private readonly IRepository<Data.Models.PreventiveHoldHistory> _PHHRepository;
        public PreventiveHoldHistoryService(IRepository<Data.Models.PreventiveHoldHistory> phhRepository)
        {
            this._PHHRepository = phhRepository;
        }
        public int Add(Data.Models.PreventiveHoldHistory PHH)
        {
            _PHHRepository.Insert(PHH);
            return PHH.Id;
        }
    }
}
