using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indent
{
    public class IndentDetailAttachmentServices
    {
        private readonly IRepository<Data.Models.IndentDetailAttachment> _IndentDetailAttachmenRepository;

        public IndentDetailAttachmentServices(IRepository<Data.Models.IndentDetailAttachment> IndentDetailAttachmenRepository)
        {
            this._IndentDetailAttachmenRepository = IndentDetailAttachmenRepository;
        }
        public int Add(Data.Models.IndentDetailAttachment IndentDetailAttachments)
        {
            _IndentDetailAttachmenRepository.Insert(IndentDetailAttachments);
            return IndentDetailAttachments.Id;
        }
        public List<Data.Models.IndentDetailAttachment> GetAttachmentData(int IndentDetailId)
        {
            var allData = from a in this._IndentDetailAttachmenRepository.Table
                          where a.IndentDetailId == IndentDetailId
                          select a;

            return allData.ToList();

        }
        public void Delete(int[] Ids)
        {

            var deletedItems = this._IndentDetailAttachmenRepository.Table.Where(w => Ids.Contains(w.Id));
            this._IndentDetailAttachmenRepository.Delete(deletedItems);
        }
    }
}
