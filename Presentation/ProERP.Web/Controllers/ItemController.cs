using ProERP.Data.Models;
using ProERP.Services.Indent;
using ProERP.Services.PreventiveMaintenance;
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
    [Authorize]
    public class ItemController : BaseController
    {
        // GET: Item
        private readonly ItemsServices _ItemsService;
        private readonly VendorService _vendorServices;

        public ItemController(ItemsServices ItemsService, VendorService vendorServices)
        {
            this._ItemsService = ItemsService;
            this._vendorServices = vendorServices;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            IndentViewModel model = new IndentViewModel();
            model.Id = 0;
            model.ItemViewModel = new ItemViewModel { Id = 0 };
            return View(model);
        }
        public ActionResult Update(int? Id)
        {
            IndentViewModel model = new IndentViewModel();
            model.Id = 0;
            model.ItemViewModel = new ItemViewModel { Id = Id ?? 0 };
            return View(model);
        }
        public JsonNetResult SaveNewItem(Item item)
        {
            var result = new JsonResponse();
            try
            {
                result.Status = JsonResponseStatus.Success;
                if (item.Id <= 0)
                    this._ItemsService.Add(item);
                else
                {
                    this._ItemsService.Update(item);
                }
                var alldata = this._ItemsService.GetAllItemNameList();
                var items = alldata.Select(m => new { m.Id, m.Name });
                return JsonNet(new { Type = "Success", Message = "", Data = new { Id = item.Id, Name = item.Name } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                return JsonNet(new { Type = "Error", Message = ex.Message + ex.InnerException ?? ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult List()
        {

            return View();
        }
        public JsonResult GetVendorList()
        {
            var allData = this._vendorServices.GetAllVendorList();
            var vendor = allData.Select(s => new { s.Id, s.Name });
            return Json(vendor, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemById(int Id)
        {
            ItemViewModel model = new ItemViewModel();

            Data.Models.Item objIn = this._ItemsService.GetForId(Id);

            if (objIn != null)
            {
                model.Id = objIn.Id;
                model.ItemCode = objIn.ItemCode;
                model.Name = objIn.Name;
                //model.SpecificationFile = objIn.SpecificationFile;
                model.Make = objIn.Make;
                model.Model = objIn.Model;
                model.Price = objIn.Price;
                model.MOC = objIn.MOC;
                model.AvailableQty = objIn.AvailableQty;
                model.UnitOfMeasure = objIn.UnitOfMeasure;
                model.Description = objIn.Description;
                model.VendorId = objIn.VendorId;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetItemGridData(string Name)
        {
            var allData = this._ItemsService.GetItemGridData(Name);
            var itemData = allData.Select(s => new
            {
                s.Id,
                s.ItemCode,
                s.Name,
                s.Price,
                s.Make,
                s.MOC,
                s.Model,
                s.UnitOfMeasure,
                s.AvailableQty
            });
            return JsonNet(itemData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult DeleteItem(int[] Ids)
        {
            var result = new JsonResponse();

            try
            {
                result.Status = JsonResponseStatus.Success;
                int[] ids = this._ItemsService.Delete(Ids);

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }

    }
}
