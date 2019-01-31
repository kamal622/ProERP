using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.RMItem
{
    public class RMItemService
    {
        private readonly IRepository<Data.Models.RMItem> _itemRepository;
        private readonly IRepository<Data.Models.ItemCategory> _itemCategoryRepository;

        public RMItemService(IRepository<Data.Models.RMItem> itemRepository,
                         IRepository<Data.Models.ItemCategory> itemCategoryRepository)
        {
            this._itemRepository = itemRepository;
            this._itemCategoryRepository = itemCategoryRepository;
        }

        public int SaveItem(Data.Models.RMItem itemData)
        {
            this._itemRepository.Insert(itemData);
            return itemData.Id;
        }

        public int UpdateItem(Data.Models.RMItem itemData)
        {
            Data.Models.RMItem oldItem = _itemRepository.Table.FirstOrDefault(W => W.Id == itemData.Id);
            if (oldItem != null)
            {
                oldItem.CategoryId = itemData.CategoryId;
                oldItem.ItemCode = itemData.ItemCode;
                oldItem.Name = itemData.Name;
                oldItem.Description = itemData.Description;
                oldItem.IsActive = itemData.IsActive;
                oldItem.UpdateBy = itemData.UpdateBy;
                oldItem.UpdateDate = itemData.UpdateDate;
                _itemRepository.Update(itemData);
                return oldItem.Id;
            }
            else
                return 0;
        }

        public List<Data.Models.RMItem> GetItemData(string itemName)
        {
            var allData = from a in _itemRepository.Table.Where(W => W.IsDeleted == false)
                          select a;
            if (!string.IsNullOrEmpty(itemName))
            {
                allData = allData.Where(W => W.Name.Contains(itemName));
            }
            return allData.ToList();
        }

        public Data.Models.RMItem GetItemDataById(int Id)
        {
            return this._itemRepository.Table.FirstOrDefault(w => w.Id == Id);
        }

        public int DeleteItem(int Id, int userid)
        {
            Data.Models.RMItem objItem = this._itemRepository.Table.FirstOrDefault(w => w.Id == Id);
            if (objItem != null)
            {
                objItem.IsDeleted = true;
                objItem.DeletedOn = DateTime.Now;
                objItem.DeletedBy = userid;
                _itemRepository.Update(objItem);
                return 1;
            }
            else
                return 0;
        }

        public List<Data.Models.RMItem> GetItemListForFormula()
        {
            return this._itemRepository.Table.OrderBy(o => o.Name).ToList();
        }

        public List<Data.Models.RMItem> GetUOMByItemId(int itemId)
        {
            return this._itemRepository.Table.Where(w => w.Id == itemId).ToList();
        }

        public List<Data.Models.ItemCategory> GetItemCategoryList()
        {
            return this._itemCategoryRepository.Table.OrderBy(o => o.Name).ToList();
        }
    }
}
