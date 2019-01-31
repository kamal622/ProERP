using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indent
{
   public class ItemsServices
    {
        private readonly IRepository<Data.Models.Item> _ItemRepository;
   
    public ItemsServices (IRepository<Data.Models.Item> ItemRepository)
    {
        this._ItemRepository = ItemRepository;
    }
        public List<Data.Models.Item> GetAllItemNameList()
        {
            return this._ItemRepository.Table.OrderBy(w=>w.Name).ToList();
        }
        public Data.Models.Item GetForId(int Id)
        {
            return this._ItemRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
        public int Add(Data.Models.Item item)
        {
            _ItemRepository.Insert(item);
            return item.Id;
        }
        public void Update(Data.Models.Item item)
        {
            Data.Models.Item existingItem = _ItemRepository.Table.FirstOrDefault(w => w.Id == item.Id);

            if (existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.ItemCode = item.ItemCode;
                existingItem.Price = item.Price;
                existingItem.MOC = item.MOC;
                existingItem.Make = item.Make;
                existingItem.Model = item.Model;
                existingItem.UnitOfMeasure = item.UnitOfMeasure;
                existingItem.AvailableQty = item.AvailableQty;
                existingItem.Description = item.Description;
                existingItem.VendorId = item.VendorId;
                this._ItemRepository.Update(existingItem);
            }
        }
        public List<Data.Models.Item> GetItemGridData(string Name)
        {
            var allData = from a in this._ItemRepository.Table
                          select a;
            if (!string.IsNullOrEmpty(Name))
                allData = allData.Where(w => w.Name.Contains(Name));
            return allData.ToList();
            
        }
        public int[] Delete(int[] ids)
        {
            List<int> itemIds = new List<int>();
            foreach (int id in ids)
            {
                Data.Models.Item Item = _ItemRepository.Table.FirstOrDefault(w => w.Id == id);
                _ItemRepository.Delete(Item);
                
            }
            return itemIds.ToArray();
        }
    }
}
