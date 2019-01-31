using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Document
{
    public class DocumentService
    {
        private readonly IRepository<Data.Models.Document> _documentRepository;
        private readonly IRepository<Data.Models.Machine> _machineRepository;
        private readonly IRepository<Data.Models.Line> _lineRepository;

        public DocumentService(IRepository<Data.Models.Document> documentRepository,
            IRepository<Data.Models.Machine> machineRepository,
            IRepository<Data.Models.Line> lineRepository)
        {
            this._documentRepository = documentRepository;
            this._machineRepository = machineRepository;
            this._lineRepository = lineRepository;
        }
        public void BuldInsert(List<Data.Models.Document> documents)
        {
            this._documentRepository.Insert(documents);
        }
        public void GetPLMNames(int machineId, out string plantName, out string lineName, out string machineName)
        {
            var allData = this._machineRepository.Table.Where(a => a.Id == machineId)
                          .Select(a => new { PlantName = a.Plant.Name, LineName = a.Line.Name, MachineName = a.Name })
                          .FirstOrDefault();

            plantName = allData.PlantName;
            lineName = allData.LineName;
            machineName = allData.MachineName;
        }
        public void GetPLNames(int lineId, out string plantName, out string lineName)
        {
            var allData = this._lineRepository.Table.Where(w => w.Id == lineId).Select(s => new { PlantName = s.Plant.Name, LineName = s.Name }).FirstOrDefault();

            plantName = allData.PlantName;
            lineName = allData.LineName;
        }
        public List<Data.Models.Document> GetDocuments(int PlantId, int? LineId, int? MachineId, int CategoryId, string[] searchKeyword)
        {
            var allData = from a in this._documentRepository.Table
                          where a.PlantId == PlantId
                          && a.IsDeleted == false 
                          && a.LineId == ((LineId == null || LineId == 0) ? a.LineId : LineId.Value)
                          && a.MachineId == ((MachineId == null || MachineId == 0) ? a.MachineId : MachineId.Value)
                          && a.DocumentType.Id == ((CategoryId == 1) ? a.DocumentType.Id : CategoryId)
                          select a;

            if (searchKeyword.Length > 0)
                allData = allData.Where(w => searchKeyword.Any(a => w.Tags.Contains(a)));
            return allData.ToList();
        }
        public void UpdateTags(int DocumentId, string Tags)
        {
            Data.Models.Document existingDocument = _documentRepository.Table.FirstOrDefault(w => w.Id == DocumentId);
            if (existingDocument != null)
            {
                existingDocument.Tags = Tags;
                _documentRepository.Update(existingDocument);
            }

        }
        public Data.Models.Document GetForId(int DocumentId)
        {
            return this._documentRepository.Table.FirstOrDefault(f => f.Id == DocumentId);
        }
        public Data.Models.Document[] GetDocuments(int[] Ids)
        {
            return this._documentRepository.Table.Where(f => Ids.Contains(f.Id)).ToArray();
        }

        public void DeleteDocuments(int[] ids, int userId)
        {
            Data.Models.Document[] existingdoc = _documentRepository.Table.Where(w => ids.Contains(w.Id)).ToArray();
            for(int i=0;i<existingdoc.Length;i++)
            {
                var documentid = existingdoc[i];
                documentid.IsDeleted = true;
                documentid.DeletedDate = DateTime.UtcNow;
                documentid.DeletedBy = userId;
            }
            _documentRepository.Update(existingdoc);
            
        }
    }
}
