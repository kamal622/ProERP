using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Document
{
   public class DocumentTypeServices
    {
       
        private readonly IRepository<Data.Models.DocumentType> _DocumentTypeRepository;

        public DocumentTypeServices(IRepository<Data.Models.DocumentType> DocumentTypeRepository)
        {
            this._DocumentTypeRepository = DocumentTypeRepository;
        }
        public List<Data.Models.DocumentType> GetAllCategories()
        {
            return this._DocumentTypeRepository.Table.ToList();
        }
    }
}
