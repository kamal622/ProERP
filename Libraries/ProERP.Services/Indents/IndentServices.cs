using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indents
{
    public class IndentServices
    {
         private readonly IRepository<Data.Models.Indent> _IndentRepository;

        public IndentServices(IRepository<Data.Models.Indent> indentRepository)
        {
            this._IndentRepository = indentRepository;
        }
        public int Add(Data.Models.Indent Indent)
        {
            _IndentRepository.Insert(Indent);
            return Indent.Id;
        }
        public int Update(Data.Models.Indent Indent)
        {
            Data.Models.Indent oldIn = _IndentRepository.Table.FirstOrDefault(w => w.Id == Indent.Id);

            if (oldIn != null)
            {
                //oldIn.PlantId = Indent.PlantId;
                //oldIn.ItemName = Indent.ItemName;
                //oldIn.Description = Indent.Description;
                //oldIn.Specification = Indent.Specification;
                //oldIn.QtyInStock = Indent.QtyInStock;
                //oldIn.QtyNeeded = Indent.QtyNeeded;
                //oldIn.UnitPrice = Indent.UnitPrice;
                //oldIn.TotalAmount = Indent.TotalAmount;
                //oldIn.Priority = Indent.Priority;
                //oldIn.PreferredVendorId = Indent.PreferredVendorId;
               
               
                _IndentRepository.Update(oldIn);
                return oldIn.Id;
            }
            else
                return 0;
        }

        public List<Data.Models.Indent> GetIndentData()
        {
            var allData = from a in this._IndentRepository.Table
                          select a;
            
            return allData.ToList();

        }
        public Data.Models.Indent GetForId(int Id)
        {
            return this._IndentRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
        public int[] Delete(int[] ids)
        {
            List<int> indentIds = new List<int>();
            foreach (int id in ids)
            {
                Data.Models.Indent Indent = _IndentRepository.Table.FirstOrDefault(w => w.Id == id);
                _IndentRepository.Delete(Indent);
            }
            return indentIds.ToArray();
        }
    }
}
