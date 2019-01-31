using ProERP.Data.Models;
using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.Plant;
using ProERP.Services.Site;
using ProERP.Services.SubAssembly;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProERP.Web.Controllers
{
    public class SubAssemblyController : Controller
    {

        private readonly LineService _lineService;
        private readonly SiteService _siteService;
        private readonly PlantService _plantService;
        private readonly MachineService _machineService;
        private readonly SubAssemblyService _SubAssemblyService;


        public SubAssemblyController(LineService lineService, SiteService siteService, PlantService plantService, MachineService machineService, SubAssemblyService SubAssemblyService)
        {
            this._lineService = lineService;
            this._siteService = siteService;
            this._plantService = plantService;
            this._machineService = machineService;
            this._SubAssemblyService = SubAssemblyService;
        }
        // GET: SubAssembly
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }
        //GetSubAssemblyList
        public JsonResult GetSubAssemblyList(string Name, int SiteId, int PlantId, int LineId,int MachineId)
        {
            var allData = this._SubAssemblyService.GetAll(Name, SiteId, PlantId, LineId,MachineId);
            var subAssemblies = allData.Select(s => new { s.Id, s.Name,s.Description,s.MachineId,MachineName=s.Machine.Name , LineId= s.Machine.LineId, LineName = s.Machine.Line.Name });
            // , PlantName = s.LineMachineMappings.Any.Plant.Name, SiteId = s.Plant.SiteId, SiteName = s.Plant.Site.Name });
            return Json(subAssemblies, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add()
        {
            SubAssemblyViewModel model = new SubAssemblyViewModel();

            return View(model);
        }
        [HttpPost]
        public JsonResult AddSubAssembly(SubAssembly SubAssembly)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                int siteId = this._SubAssemblyService.Add(SubAssembly);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in adding SubAssembly.Please contact Admin." };
            }
            //return View("List");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(int? Id)
        {
            ViewBag.SubAssemblyId = Id == null ? 0 : Id.Value;
            return View();
        }
        public JsonResult GetSubAssemblyById(int SubAssemblyId)
        {
            var machine = (_SubAssemblyService.GetSubAssemblyById(SubAssemblyId));

            //  var allData = new { Machine = machine,PlantId= _machineService.GetPlantIdByLineId(machine.LineId.Value),SiteId= _machineService.GetSiteIdByLineId(machine.LineId.Value) };
            // var allData = new { Machine = machine };

            return Json(machine, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSubAssembly(Machine machine)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                this._machineService.Update(machine);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in updating Machine.Please contact Admin." };
            }
            //return View("List");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}