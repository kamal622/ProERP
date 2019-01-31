using ProERP.Data.Models;
using ProERP.Services.Plant;
using ProERP.Services.Site;
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
    public class PlantController : BaseController
    {
        private readonly PlantService _plantService;
        private readonly SiteService _siteService;

        public PlantController(PlantService plantService, SiteService siteService)
        {
            this._plantService = plantService;
            this._siteService = siteService;
        }

        // GET: Plant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            List<Site> sites = _siteService.GetAll("");

            PlantViewModel model = new PlantViewModel();
            model.Sites = sites.ToArray();
            return View(model);
        }
        public JsonResult GetPlantList(string Name, int SiteId)
        {
            var allData = this._plantService.GetAll(Name, SiteId);
            var plants = allData.Select(s => new { s.Id, s.Name, s.PlantInCharge, s.Location, s.SiteId, SiteName = s.Site.Name });
            return Json(plants, JsonRequestBehavior.AllowGet);

        }

        public JsonNetResult GetPlantsForSite(int SiteId)
        {
            Plant[] allPlants = this._plantService.GetPlantsForSite(SiteId);
            return JsonNet(allPlants.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {
            List<Site> sites = _siteService.GetAll("");
            PlantViewModel model = new PlantViewModel();
            model.Sites = sites.ToArray();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddPlant(Plant plant)
        {
            try
            {
                int siteId = this._plantService.Add(plant);
            }
            catch (Exception ex)
            {

            }
            //return View("List");
            return RedirectToAction("List");
        }

        public PlantViewModel getModelById(int id)
        {
            Plant data = _plantService.GetPlantById(id);
            PlantViewModel model = new PlantViewModel();
            model.Id = data.Id;
            model.Name = data.Name;
            model.Location = data.Location;
            model.PlantInCharge = data.PlantInCharge;
            model.SiteId = data.SiteId;
            model.SiteName = _siteService.getSiteNameById(data.SiteId.Value);
            model.Sites = _siteService.GetAll("").ToArray();
            model.ErrorMessage = "";
            return model;
        }
        public ActionResult Update(int id)
        {
            PlantViewModel model = getModelById(id);
            return View(model);
        }

        public ActionResult UpdatePlant(Plant plant)
        {
            try
            {
                this._plantService.Update(plant);
            }
            catch (Exception ex)
            {
                PlantViewModel model = getModelById(plant.Id);
                model.ErrorMessage = "There is a problem updating Plant.Please try again later.";
                return View();
            }
            return RedirectToAction("List");
        }



        [HttpPost]
        public JsonResult DeletePlant(int[] Ids)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                int[] ids = this._plantService.Delete(Ids);
                if (ids.Length > 0)
                    result = new { Success = "false", Message = "Some dependency found in lines." };

            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Site.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}