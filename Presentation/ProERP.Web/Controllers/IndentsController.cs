using ProERP.Data.Models;
using ProERP.Services.Indents;
using ProERP.Services.MaintenanceRequest;
using ProERP.Services.Plant;
using ProERP.Services.PreventiveMaintenance;
using ProERP.Services.Site;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProERP.Web.Controllers
{
    public class IndentsController : BaseController
    {
        private readonly SiteService _siteService;
        private readonly PlantService _plantService;
        private readonly IndentServices _indentService;
        private readonly MaintenancePriorityTypeServices _mtpService;
        private readonly VendorCategoryService _vcServices;
        private readonly VendorService _vendorServices;
        public IndentsController(IndentServices indentService, SiteService siteService, PlantService plantService, VendorCategoryService vcServices, VendorService vendorServices, MaintenancePriorityTypeServices mtpService)
   
        {
            this._indentService = indentService;
            this._siteService = siteService;
            this._plantService = plantService;
            this._vcServices = vcServices;
            this._vendorServices = vendorServices;
            this._mtpService = mtpService;
        }
        // GET: Indents
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult List()
        {
            IndentsViewModel model = new IndentsViewModel();
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());
            return View(model);
        }

        public ActionResult Add()
        {
            IndentsViewModel model = new IndentsViewModel();
            model.Id = 0;
            return View(model);
        }
        public ActionResult Update(int? Id)
        {
            IndentsViewModel model = new IndentsViewModel();
            model.Id = Id ?? 0;
            return View(model);
        }
        public JsonResult GetIndentById(int Id)
        {
            IndentsViewModel model = new IndentsViewModel();

            Data.Models.Indent objIn = this._indentService.GetForId(Id);

            if (objIn != null)
            {
                model.SiteId = 1;
               // model.PlantId = objIn.PlantId;
                //model.ItemName = objIn.ItemName;
                //model.Specification = objIn.Specification;
                //model.Description = objIn.Description;
                //model.QtyInStock = objIn.QtyInStock;
                //model.QtyNeeded = objIn.QtyNeeded;
                //model.UnitPrice = objIn.UnitPrice;
                //model.TotalAmount = objIn.TotalAmount;
                //model.Priority = objIn.Priority;
                //model.PreferredVendorId = objIn.PreferredVendorId;
                //model.VendorCategoryId = objIn.Vendor.CategoryId;

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveIndent(Indent Indent)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                Indent Indentobj = new Data.Models.Indent();

                if (Indent.Id == 0)
                { 
                    this._indentService.Add(Indent);
                }
                else
                {
                    this._indentService.Update(Indent);
                }
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in adding Indent.Please contact Admin." };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIndentsList()
        {
            var allData = this._indentService.GetIndentData();
            var Indent = allData.Select(s => new
            {
                s.Id,
                s.IndentNo,
                //s.ItemName,
                //s.QtyInStock,
                //s.QtyNeeded,
                //s.UnitPrice,
                //s.TotalAmount,
                //s.Priority,
                //s.IsApprove,
                //PrioritytName=s.MaintenancePriorityType.Description
            });
            return Json(Indent, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteIndent(int[] Ids)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                int[] ids = this._indentService.Delete(Ids);
                if (ids.Length > 0)
                    result = new { Success = "false", Message = "Some dependency found in lines." };
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Indent.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}