using Microsoft.AspNet.Identity;
using ProERP.Data.Models;
using ProERP.Services.Dashboard;
using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.Plant;
using ProERP.Services.PreventiveMaintenance;
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
    public class LineController : BaseController
    {
        // GET: Line

        private readonly LineService _lineService;
        private readonly SiteService _siteService;
        private readonly PlantService _plantService;
        private readonly MachineService _machineService;
        private readonly DashboardService _dashboardService;
        private readonly PreventiveReviewHistoryService _preventiveReviewHistoryService;
        private readonly PreventiveMaintenanceService _preventiveMaintenanceService;

        public LineController(LineService lineService, SiteService siteService, PlantService plantService, DashboardService dashboardService,
            PreventiveReviewHistoryService preventiveReviewHistoryService, PreventiveMaintenanceService preventiveMaintenanceService,
            MachineService machineService)
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

        public ActionResult Add()
        {
            LineViewModel model = new LineViewModel();
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());

            return View(model);
        }
        public ActionResult Update(int? id)
        {
            LineViewModel model = new LineViewModel();

            return View(model);
        }
        [HttpPost]
        public JsonResult AddLine(Line line)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                this._lineService.Add(line);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in adding Line.Please contact Admin." };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            LineViewModel model = new LineViewModel();
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());
            //_plantService.GetAll("", 0).ToArray();
            return View(model);
        }

        public JsonResult GetPlantsForSite(int SiteId)
        {
            var allData = this._plantService.GetPlantsForSite(SiteId);
            var plants = allData.Select(s => new { s.Id, s.Name });
            return Json(plants, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetLinesForPlant(int PlantId, bool isAll = false)
        {
            var allData = this._lineService.GetLinesForPlant(PlantId);
            if (isAll)
                allData.Insert(0, new Line { Id = 0, Name = "ALL LINES" });
            var lines = allData.Select(s => new { s.Id, s.Name });
            return JsonNet(lines, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLineList(string Name, int SiteId, int PlantId)
        {
            var allData = this._lineService.GetAll(Name, SiteId, PlantId);
            var lines = allData.Select(s => new { s.Id, s.Name, s.Location, s.InCharge, s.Description, s.PlantId, PlantName = s.Plant.Name, SiteId = s.Plant.SiteId, SiteName = s.Plant.Site.Name, s.IsActive, s.IsShutdown });
            return Json(lines, JsonRequestBehavior.AllowGet);            
        }

        public JsonNetResult GetShutdownLineList()
        {
            var allData = this._preventiveReviewHistoryService.GetShutdownLineGridData();
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        } 

        public JsonResult UpdateLine(Line line)
        {
            var result = new { Success = "true", Message = "Success" };
            int userId = HttpContext.User.Identity.GetUserId<int>();
            try
            {
                this._lineService.Update(line, userId);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in adding Line.Please contact Admin." };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult getModelById(int id)
        {
            Line data = _lineService.GetLineById(id);
            LineViewModel model = new LineViewModel();
            model.Id = data.Id;
            model.Name = data.Name;
            model.Location = data.Location;
            model.InCharge = data.InCharge;
            model.SiteId = 1;
            model.PlantId = data.PlantId;
            model.IsActive = data.IsActive;
            //model.SiteName = data.Plant.Site.Name;
            // model.Sites = _siteService.GetAll("").ToArray();
            // model.PlantName = data.Plant.Name;
            // model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());
            //_plantService.GetAll("", 0).ToArray();

            model.ErrorMessage = "";
            return JsonNet(model, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult UpdateIsShutdown(int lineId, bool IsShutdown)
        {
            JsonResponse response = new Models.JsonResponse();

            int userId = HttpContext.User.Identity.GetUserId<int>();
            var line = this._lineService.GetLineById(lineId);
            var plantid = line.PlantId;
            try
            {
                if (IsShutdown)
                {
                    DateTime lastShutDownDate = this._preventiveReviewHistoryService.GetLastShutDownDate(lineId);
                    if ((DateTime.UtcNow.Date - lastShutDownDate).Days <= 5)
                        return JsonNet(new JsonResponse { Status = JsonResponseStatus.Warning, Message = "Last shutdown for this line is whthin last 5 days. Can not shutdown today." }, JsonRequestBehavior.AllowGet);


                    this._lineService.UpdateIsShutdown(lineId, true);
                    var shutdownId = this._dashboardService.Add(new ShutdownHistory
                    {
                        PlantId = plantid,
                        LineId = lineId,
                        MachineId = null,
                        ShutdownDate = DateTime.UtcNow,
                        ShutdownBy = userId,
                        StartDate = null,
                        StartBy = null
                    });

                    this._preventiveMaintenanceService.UpdateShutdownNextReviewDateForLine(lineId);

                    var preventiveIds = this._preventiveMaintenanceService.GetPreventiveIdsForLine(lineId);
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
                            IsLineActive = isLineActive,
                            IsMachineActive= isMachineIsActive
                        });
                    }
                }
                else
                {
                    if (this._preventiveReviewHistoryService.AnyMachineShutdown(lineId))
                        return JsonNet(new JsonResponse { Status = JsonResponseStatus.Warning, Message = "One or more machine for this line is in shutdown mode. Can't update this record." }, JsonRequestBehavior.AllowGet);

                    if (this._preventiveReviewHistoryService.AnyShutdownReviewRemain(lineId))
                        return JsonNet(new JsonResponse { Status = JsonResponseStatus.Warning, Message = "One or more shutdown activity for this line is not done. Can't update this record." }, JsonRequestBehavior.AllowGet);

                    this._lineService.UpdateIsShutdown(lineId, false);
                    int shutdownId = _dashboardService.GetShutdownIdForLine(lineId);
                    this._dashboardService.UpdateShutdownHistoryForLine(shutdownId, userId);
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
        public JsonResult DeleteLine(int[] Ids)
        {
            var result = new { Success = "true", Message = "Success" };
            JsonResponse response = new JsonResponse();

            try
            {
                int[] ids = this._lineService.Delete(Ids);
                if (ids.Length > 0)
                    result = new { Success = "false", Message = "Some dependency found in machines." };

            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Line.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetFormulationLine()
        {
            var allData = this._lineService.GetLineList();
            return JsonNet(allData.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }


    }
}