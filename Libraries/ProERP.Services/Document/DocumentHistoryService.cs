using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Document
{
   public class DocumentHistoryService
    {
        private readonly IRepository<Data.Models.DocumentHistory> _documenthistoryRepository;
        public DocumentHistoryService(IRepository<Data.Models.DocumentHistory> documenthistoryRepository)
        {
            this._documenthistoryRepository = documenthistoryRepository;
        }
        public List<Data.Models.DocumentHistory> GetHistoryDocuments(int documentId)
        {
            var allData = from a in this._documenthistoryRepository.Table
                          where a.DocumentId == documentId
                          select a;
            return allData.ToList();
        }
    }
}
