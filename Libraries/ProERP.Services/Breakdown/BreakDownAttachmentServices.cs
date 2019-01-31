using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Breakdown
{
   public class BreakDownAttachmentServices
    {
        private readonly IRepository<Data.Models.BreakDownAttachment> _BreakdownAttachmenRepository;
        public BreakDownAttachmentServices(IRepository<Data.Models.BreakDownAttachment> BreakdownAttachmenRepository)
        {
            this._BreakdownAttachmenRepository = BreakdownAttachmenRepository;
        }
        public int Add(Data.Models.BreakDownAttachment BreakDownAttachment)
        {
            _BreakdownAttachmenRepository.Insert(BreakDownAttachment);
            return BreakDownAttachment.Id;
        }
        public List<Data.Models.BreakDownAttachment> GetAttachmentData(int BreakDownId)
        {
            var allData = from a in this._BreakdownAttachmenRepository.Table
                          where a.BreakDownId == BreakDownId
                          select a;

            return allData.ToList();

        }
        public void Delete(int[] Ids)
        {
            var deletedItems = this._BreakdownAttachmenRepository.Table.Where(w => Ids.Contains(w.Id));
            this._BreakdownAttachmenRepository.Delete(deletedItems);
        }

        public Data.Models.BreakDownAttachment GetById(int Id)
        {
            return this._BreakdownAttachmenRepository.GetById(Id);
        }
    }
}
