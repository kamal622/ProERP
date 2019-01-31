using Microsoft.AspNet.Identity;
using ProERP.Data.Models;
using ProERP.Services.RMItem;
using ProERP.Web.Framework;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProERP.Web.Controllers
{
    [Authorize(Roles = "Admin,Lavel1,Lavel2,Lavel3,QA,QAManager")]
    public class RMItemController : BaseController
    {
        // GET: RMItem
        private readonly RMItemService _itemService;
        public RMItemController(RMItemService itemService)
        {
            this._itemService = itemService;
        }

        [Authorize(Roles = "Admin,QA,QAManager")]
        public ActionResult List()
        {
            return View();
        }

        public JsonNetResult SaveItems(RMItem itemData)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                if (itemData.Id > 0)
                {
                    itemData.UpdateBy = HttpContext.User.Identity.GetUserId<int>();
                    itemData.UpdateDate = DateTime.Now;
                    _itemService.UpdateItem(itemData);
                }
                else
                {
                    itemData.CreateBy = HttpContext.User.Identity.GetUserId<int>();
                    itemData.CreateDate = DateTime.Now;
                    _itemService.SaveItem(itemData);
                }
                var allData = this._itemService.GetItemListForFormula();
                var items = allData.Select(s => new { s.Id, s.Name });
                return JsonNet(new { Type = "Success", Message = "Item Saved", Data = new { Id = itemData.Id, Name = itemData.Name } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                return JsonNet(new { Type = "Error", Message = ProcessException(ex) ?? ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemList(string itemName)
        {
            var allData = _itemService.GetItemData(itemName);
            var machineData = allData.Select(s => new
            {
                s.Id,
                s.Name,
                s.CategoryId,
                CategoryName = s.ItemCategory.Name,
                s.ItemCode,
                s.Description
            });
            return Json(machineData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemById(int Id)
        {
            RMItemViewModel model = new RMItemViewModel();
            Data.Models.RMItem objItem = this._itemService.GetItemDataById(Id);
            if (objItem != null)
            {
                model.Id = objItem.Id;
                model.CategoryId = objItem.CategoryId;
                model.Name = objItem.Name;
                model.ItemCode = objItem.ItemCode;
                model.Description = objItem.Description;
                model.IsActive = objItem.IsActive;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteItem(int Id)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                int userid = HttpContext.User.Identity.GetUserId<int>();
                int id = this._itemService.DeleteItem(Id, userid);
                if (id == 0)
                    result = new { Success = "false", Message = "Some dependency found in item." };
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Item.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetItemListForFormula()
        {
            var allData = this._itemService.GetItemListForFormula();
            return JsonNet(allData.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetItemCategoryList()
        {
            var allData = this._itemService.GetItemCategoryList();
            return JsonNet(allData.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}