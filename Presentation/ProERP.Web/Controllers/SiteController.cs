using ProERP.Data.Models;
using ProERP.Services.Site;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProERP.Web.Controllers
{
    [Authorize(Roles = "Admin,Lavel1,Lavel2")]
    public class SiteController : Controller
    {
        // GET: Site
        private readonly SiteService _siteService;
        public SiteController(SiteService siteService)
        {

            this._siteService = siteService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetSiteList(string Name)
        {
            var allData = this._siteService.GetAll(Name);
            var sites = allData.Select(s => new { s.Id, s.Name, s.InCharge, s.Description, s.Address });
            return Json(sites, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSites()
        {
            var allData = this._siteService.GetAll("");
            var sites = allData.Select(s => new { s.Id, s.Name });
            return Json(sites, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSite(Site site)
        {
          
            try
            {
                int siteId = this._siteService.Add(site);
                
            }
            catch (Exception ex)
            {
                
            }
            //return View("List");
            return RedirectToAction("List");
        }

        public ActionResult Update(int id)
        {
            Site data = _siteService.GetSiteById(id);
            SiteViewModel model = new SiteViewModel();
            model.Id = data.Id;
            model.Name = data.Name;
            model.Address = data.Address;
            model.InCharge = data.InCharge;
            model.Description = data.Description;

            return View(model);
        }

        public ActionResult UpdateSite(Site site)
        {
            var result = new { Success = "true", siteId = site.Id };

            try
            {
                if (this._siteService.Update(site) == 0)
                    result = new { Success = "false", siteId = 0 };
            }
            catch (Exception ex)
            {
                result = new { Success = "false", siteId = 0 };
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public JsonResult DeleteSites(int[] Ids)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                int[] ids = this._siteService.Delete(Ids);
                if(ids.Length>0)
                    result = new { Success = "false", Message = "Some dependency found in plants." };

            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Site.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}