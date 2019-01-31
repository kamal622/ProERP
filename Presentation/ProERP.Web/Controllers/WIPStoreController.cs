using Microsoft.AspNet.Identity;
using ProERP.Services.WIPStore;
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
    public class WIPStoreController : BaseController
    {
        private readonly WIPStoreService _wipStoreService;
        public WIPStoreController(WIPStoreService wipStoreService)
        {
            this._wipStoreService = wipStoreService;
        }

        // GET: WIPStore

        [Authorize(Roles = "Admin,Lavel1,Lavel3")]
        public ActionResult List()
        {
            return View();
        }

        public ActionResult WIPStore()
        {
            return View();
        }

        public JsonResult GetWIPStoreList(string Name)
        {
            var allData = this._wipStoreService.GetWIPStoreList(Name);
            var finalData = allData.Select(s => new
            {
                s.Id,
                s.Name,
                s.Location,
                s.Description
            });
            return Json(finalData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveWIPStore(Data.Models.WIPStore wipData)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                wipData.CreateBy = HttpContext.User.Identity.GetUserId<int>();
                wipData.UpdateBy = HttpContext.User.Identity.GetUserId<int>();
                wipData.CreateOn = DateTime.Now;
                this._wipStoreService.SaveWIPStore(wipData);
                response.Status = JsonResponseStatus.Success;
                response.Message = "WIP Store saved";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWIPStoreById(int Id)
        {
            WIPStoreViewModel model = new WIPStoreViewModel();
            Data.Models.WIPStore objData = this._wipStoreService.GetWIPById(Id);
            if (objData != null)
            {
                model.Id = objData.Id;
                model.Name = objData.Name;
                model.Location = objData.Location;
                model.Description = objData.Description;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult DeleteWipById(int Id)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._wipStoreService.DeleteWIP(Id);
                response.Status = JsonResponseStatus.Success;
                response.Message = "WIP Store saved";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetWIPForCloseRequest()
        {
            var allData = this._wipStoreService.GetWIpForCloseFormulation();
            return JsonNet(allData.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}