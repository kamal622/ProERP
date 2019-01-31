using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Utility
{
    public class TemplateService
    {
        private readonly IRepository<Data.Models.Template> _templateRepository;
        private readonly IRepository<Data.Models.TemplateMapping> _templateMappingRepository;
        public TemplateService(IRepository<Data.Models.Template> templateRepository, IRepository<Data.Models.TemplateMapping> templateMappingRepository)
        {
            this._templateRepository = templateRepository;
            this._templateMappingRepository = templateMappingRepository;
        }

        public Data.Models.Template GetById(int id)
        {
            return this._templateRepository.GetById(id);
        }

        public Data.Models.Template[] GetAllTemplates()
        {
            return this._templateRepository.Table.Where(w=>w.IsActive == true).OrderBy(o=>o.Name).ToArray();
        }

        public Data.Models.TemplateMapping[] GetTemplateMappings(int templateId)
        {
            return this._templateMappingRepository.Table.Where(w => w.TemplateId == templateId).OrderBy(o=>o.OrderNo).ToArray();
        }

    }
}
