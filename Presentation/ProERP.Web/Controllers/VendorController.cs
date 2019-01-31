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
    public class VendorController : BaseController
    {
        // GET: Vendor
        private readonly VendorService _vendorServices;

        public VendorController(VendorService vendorServices)
        {
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
            model.VendorViewModel = new VendorViewModel { Id = 0 };
            return View(model);
        }
        public ActionResult Update(int? Id)
        {
            IndentViewModel model = new IndentViewModel();
            model.Id = 0;
            model.VendorViewModel = new VendorViewModel { Id = Id ?? 0 };
            return View(model);
        }
        public ActionResult List()
        {

            return View();
        }
        public JsonNetResult SaveVendor(Vendor vendor)
        {
            var result = new JsonResponse();
            try
            {
                result.Status = JsonResponseStatus.Success;
                if (vendor.Id <= 0)
                {
                    this._vendorServices.Add(vendor);
                }
                else

                    this._vendorServices.Update(vendor);
                var alldata = this._vendorServices.GetAllVendorList();
                var vendors = alldata.Select(m => new { m.Id, m.Name });
                return JsonNet(new { Type = "Success", Message = "", Data = new { Id = vendor.Id, Name = vendor.Name } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                return JsonNet(new { Type = "Error", Message = ex.Message + ex.InnerException ?? ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }
        public JsonResult GetVendorListData(string Name)
        {
            var allData = this._vendorServices.GetvendorGridData(Name);
            var Indent = allData.Select(s => new
            {
                s.Id,
                s.Name,
                s.Address,
                s.PhoneNo,
                s.Email
            });
            return Json(Indent, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVendorById(int Id)
        {
            VendorViewModel model = new VendorViewModel();

            Data.Models.Vendor objVendor = this._vendorServices.GetForId(Id);

            if (objVendor != null)
            {
                model.Id = objVendor.Id;
                model.Name = objVendor.Name;
                model.Address = objVendor.Address;
                model.PhoneNo = objVendor.PhoneNo;
                model.Email = objVendor.Email;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult DeleteVendor(int[] Ids)
        {
            var result = new JsonResponse();

            try
            {
                result.Status = JsonResponseStatus.Success;
                int[] ids = this._vendorServices.Delete(Ids);

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