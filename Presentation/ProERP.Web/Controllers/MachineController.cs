using ProERP.Data.Models;
//using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.Plant;
using ProERP.Services.Site;
using ProERP.Web.Framework;
using ProERP.Web.Framework.Controllers;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProERP.Services.Line;
using ProERP.Services.Dashboard;
using ProERP.Services.PreventiveMaintenance;
using Microsoft.AspNet.Identity;
using ProERP.Web.Models;

namespace ProERP.Web.Controllers
{
    [Authorize(Roles = "Admin,Lavel1,Lavel2,Lavel3,QA,QAManager")]
    public class MachineController : BaseController
    {
        // GET: Machine
        private readonly LineService _lineService;
        private readonly SiteService _siteService;
        private readonly PlantService _plantService;
        private readonly MachineService _machineService;
        private readonly DashboardService _dashboardService;
        private readonly PreventiveReviewHistoryService _preventiveReviewHistoryService;
        private readonly PreventiveMaintenanceService _preventiveMaintenanceService;

        public MachineController(SiteService siteService, PlantService plantService, MachineService machineService, LineService lineService,
            DashboardService dashboardService,PreventiveReviewHistoryService preventiveReviewHistoryService, PreventiveMaintenanceService preventiveMaintenanceService)
        {
            this._lineService = lineService;
            this._siteService = siteService;
            this._plantService = plantService;
            this._machineService = machineService;
            this._dashboardService = dashboardService;
            this._preventiveReviewHistoryService = preventiveReviewHistoryService;
            this._preventiveMaintenanceService = preventiveMaintenanceService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            MachineViewModel model = new MachineViewModel();
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());
            // model.MachineType = _machineService.
            //   model.Lines = this._lineService.GetLinesForPlant(model.Plants.Select(s => s.Id).FirstOrDefault());

            return View(model);
        }



        public JsonResult GetMachineList(string Name, int SiteId, int PlantId, int LineId)
        {
            var allData = this._machineService.GetAll(Name, SiteId, PlantId, LineId);
            var machines = allData.Select(s => new
            {
                s.Id,
                s.Name,
                s.Make,
                s.Model,
                s.Description,
                s.InstallationDate,
                s.MachineInCharge,
                s.MachineTypeId,
                MachineTypeName = s.MachineType.Name,
                PlantName = s.Plant.Name,
                SiteId = s.Plant.SiteId,
                PlantId =s.PlantId,
                LineId=s.LineId,
                LineName = s.Line.Name,
                s.IsActive,
                s.IsShutdown
            });
            return Json(machines, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetShutdownMachineList()
        {
            var allData = this._preventiveReviewHistoryService.GetShutdownMachineGridData();
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        //GetMachinesForLine
        //public JsonResult GetMachinesForLine(int LineId)
        //{
        //    var allData = this._machineService.GetAllForLine(LineId);
        //    var machines = allData.Select(s => new { s.Id, s.Name });
        //    return Json(machines, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Add()
        {
            MachineViewModel model = new MachineViewModel();
           
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());
            //model.PlantId = model.Plants.Select(s => s.Id).FirstOrDefault();
            // model.MachineType = _machineService.
            //   model.Lines = this._lineService.GetLinesForPlant(model.Plants.Select(s => s.Id).FirstOrDefault());

            return View(model);
        }

        [HttpPost]
        public JsonResult AddMachine(Data.Models.Machine machine)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                this._machineService.Add(machine);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in adding Machine.Please contact Admin." };
            }
            //return View("List");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetMachineType()
        {
            var alldata = this._machineService.GetAllMachineType();
            var MachineType = alldata.Select(m => new { m.Id, m.Name });
            return JsonNet(MachineType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int? Id)
        {
            ViewBag.MachineId = Id == null ? 0 : Id.Value;
            var model = _machineService.GetMachineById(Id == null ? 0 : Id.Value);
            //MachineViewModel model = new MachineViewModel();
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.SiteId);
            return View(model);
        }

        public JsonResult GetMachineById(int MachineId)
        {
            MachineViewModel model = new MachineViewModel();

            var machine = (_machineService.GetMachineById(MachineId));

            //  var allData = new { Machine = machine,PlantId= _machineService.GetPlantIdByLineId(machine.LineId.Value),SiteId= _machineService.GetSiteIdByLineId(machine.LineId.Value) };
            // var allData = new { Machine = machine };

            return Json(machine, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMachine(int PlantId)
        {
            var alldata = this._machineService.GetAllLineMachineforPlant(PlantId);
            var Machine = alldata.Select(m => new { m.Id, m.Name });
            return Json(Machine, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMachinesForLine(int lineId)
        {
            var alldata = this._machineService.GetMachinesForLine(lineId);
            var Machine = alldata.Select(m => new { m.Id, m.Name });
            return Json(Machine, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateMachine(Data.Models.Machine machine)
        {
            var result = new { Success = "true", Message = "Success" };
            int userId = HttpContext.User.Identity.GetUserId<int>();
            try
            {
                this._machineService.Update(machine, userId);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in updating Machine.Please contact Admin." };
            }
            //return View("List");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult UpdateIsShutdown(int machineId, bool IsShutdown)
        {
            JsonResponse response = new Models.JsonResponse();
            var result = new { Success = "true", Message = "Success" };
            int userId = HttpContext.User.Identity.GetUserId<int>();
            var machine = this._machineService.GetMachineId(machineId);
            var lineid = machine.LineId;
            var plantid = machine.PlantId;

            try
            {
                if(IsShutdown)
                {

                    DateTime lastShutDownDate = this._preventiveReviewHistoryService.GetLastShutDownDate(lineid, machineId);
                    if ((DateTime.UtcNow.Date - lastShutDownDate).Days <= 5)
                        return JsonNet(new JsonResponse { Status = JsonResponseStatus.Warning, Message = "Last shutdown for this machine is whthin last 5 days. Can not shutdown today." }, JsonRequestBehavior.AllowGet);

                    this._machineService.UpdateIsShutdown(machineId, true);
                    var shutdownId = this._dashboardService.Add(new ShutdownHistory
                    {
                        PlantId = plantid,
                        LineId = lineid,
                        MachineId = machineId,
                        ShutdownDate = DateTime.UtcNow,
                        ShutdownBy = userId,
                        StartDate = null,
                        StartBy = null
                    });
                    this._preventiveMaintenanceService.UpdateShutdownNextReviewDateForMachine(machineId);
                    var preventiveIds = this._preventiveMaintenanceService.GetPreventiveIdsForMachine(machineId);
                    for (int i = 0; i < preventiveIds.Length; i++)
                    {
                        var isLineActive = this._preventiveMaintenanceService.GetLineIsActive(preventiveIds[i]);
                        var isMachineIsActive = this._preventiveMaintenanceService.GetMachineIsActive(preventiveIds[i]);
                        this._preventiveReviewHistoryService.Add(new PreventiveReviewHistory
                        {
                            PreventiveId = preventiveIds[i],
                            ReviewDate = null,
                            ReviewBy = null,
                            Notes = null,
                            ScheduledReviewDate = DateTime.UtcNow.Date,
                            HoldId = null,
                            ShutdownId = shutdownId,
                            IsLaps = false,
                            IsOverdue = false,
                            IsLineActive= isLineActive,
                            IsMachineActive = isMachineIsActive
                        });
                    }
                }
                else
                {
                    if (this._preventiveReviewHistoryService.AnyShutdownReviewRemainMachine(machineId))
                        return JsonNet(new JsonResponse { Status = JsonResponseStatus.Warning, Message = "One or more shutdown activity for this machine is not done. Can't update this record." }, JsonRequestBehavior.AllowGet);
                    
                    this._machineService.UpdateIsShutdown(machineId, false);
                    int shutdownId = _dashboardService.UpdateShutdownHistoryForMachine(machineId);
                    this._dashboardService.UpdateShutdownHistoryFormachine(shutdownId, userId);
                }

                response.Status = JsonResponseStatus.Success;

            }
            catch (Exception ex)
            {
                response = new JsonResponse { Status = JsonResponseStatus.Error, Message = ProcessException(ex) };
            }

            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMachine(int[] Ids)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                int[] ids = this._machineService.Delete(Ids);
                if (ids.Length > 0)
                    result = new { Success = "false", Message = "Some dependency found in lines." };
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Machine.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetMachinebyLineId(int Id)
        {
            var allData = this._machineService.GetMachineByLineId(Id);
            return JsonNet(allData.Select(s => new { MachineId = s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }
    }
}