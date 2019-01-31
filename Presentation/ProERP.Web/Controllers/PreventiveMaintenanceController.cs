using ProERP.Data.Models;
using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.Plant;
using ProERP.Services.PreventiveMaintenance;
using ProERP.Services.Site;
using ProERP.Services.User;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using ProERP.Web.Framework;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class PreventiveMaintenanceController : BaseController
    {
        private readonly SiteService _siteService;
        private readonly PlantService _plantService;
        private readonly LineService _lineService;
        private readonly MachineService _machineService;
        private readonly UserService _userService;
        private readonly PreventiveMaintenanceService _pmServices;
        private readonly UserAssignmentsService _uaServices;
        private readonly PreventiveWorkDescriptionService _pwdServices;
        private readonly PreventiveReviewHistoryService _prhServices;
        private readonly ScheduleTypeService _pstServices;
        private readonly VendorCategoryService _vcServices;
        private readonly VendorService _vendorServices;

        public PreventiveMaintenanceController(SiteService siteService, PlantService plantService, LineService lineService, MachineService machineService, UserService userService, PreventiveMaintenanceService pmServices, UserAssignmentsService uaServices, PreventiveWorkDescriptionService pwdServices, PreventiveReviewHistoryService prhServices, ScheduleTypeService pstServices, VendorCategoryService vcServices, VendorService vendorServices)
        {
            this._siteService = siteService;
            this._plantService = plantService;
            this._lineService = lineService;
            this._machineService = machineService;
            this._userService = userService;
            this._pmServices = pmServices;
            this._uaServices = uaServices;
            this._pwdServices = pwdServices;
            this._prhServices = prhServices;
            this._pstServices = pstServices;
            this._vcServices = vcServices;
            this._vendorServices = vendorServices;
        }

        // GET: PreventiveMaintenance
        #region Preventive Maintenance
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SysAdmin, Admin,Lavel1,Lavel2")]
        public ActionResult List()
        {
            PreventiveMaintenanceViewModel model = new PreventiveMaintenanceViewModel();
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());

            return View(model);
        }

        public JsonResult GetPMList(string Name, int PlantId, int LineId, int ScheduleType)
        {
            var allData = this._pmServices.GetPMData(Name, PlantId, LineId, ScheduleType);
            var PM = allData.Select(s => new
            {
                s.Id,
                s.MachineId,
                s.Checkpoints,
                //s.ScheduleType,
                s.ShutdownRequired,
                s.ScheduleStartDate,
                s.ScheduleEndDate,
                s.Description,
                MachineName = s.Machine.Name,
                ScheduleTypeName = s.PreventiveScheduleType.Description,
                s.Interval,
                WorkName = s.WorkDescription,
                Severity = s.Severity == 1 ? "Moderate" : (s.Severity == 2) ? "Critical" : "Minor",
                s.IsObservation
                // PlantName = s.Plant.Name,
                // SiteId = s.Plant.SiteId,
                //SiteName = s.Plant.Site.Name
            });
            return Json(PM, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "SysAdmin, Admin,Lavel1,Lavel2")]
        public ActionResult Add()
        {
            PreventiveMaintenanceViewModel model = new PreventiveMaintenanceViewModel();
            model.Id = 0;
            model.Sites = _siteService.GetAll("").ToArray();
            model.Plants = this._plantService.GetPlantsForSite(model.Sites.Select(s => s.Id).FirstOrDefault());
            model.UserId = new List<int>();
            return View(model);
        }



        [Authorize(Roles = "SysAdmin, Admin,Lavel1,Lavel2")]
        public ActionResult Update(int? Id)
        {
            PreventiveMaintenanceViewModel model = new PreventiveMaintenanceViewModel();
            model.Id = Id ?? 0;

            return View(model);
        }


        public ActionResult UpdateLastReviewDate(int PMId, DateTime reviewDate)
        {
            PreventiveMaintenanceViewModel model = new PreventiveMaintenanceViewModel();

            Data.Models.PreventiveMaintenance objPM = this._pmServices.GetForId(model.Id);
            if (objPM != null)

                model.LastReviewDate = objPM.LastReviewDate;

            return View(model);
        }


        public JsonResult GetPMById(int Id)
        {
            PreventiveMaintenanceViewModel model = new PreventiveMaintenanceViewModel();

            Data.Models.PreventiveMaintenance objPM = this._pmServices.GetForId(Id);

            if (objPM != null)
            {
                model.SiteId = 1;
                model.MachineId = objPM.MachineId;
                model.PlantId = objPM.PlantId;
                model.LineId = objPM.LineId;
                model.MachineId = objPM.MachineId;
                model.Description = objPM.Description;
                model.Checkpoints = objPM.Checkpoints;
                model.ScheduleType = objPM.ScheduleType;
                model.Interval = objPM.Interval;
                model.ShutdownRequired = Convert.ToString(objPM.ShutdownRequired);
                model.ScheduleStartDate = objPM.ScheduleStartDate;
                model.ScheduleEndDate = objPM.ScheduleEndDate;
                model.UserId = this._userService.GetAllByUserAssignments(objPM.Id);
                model.WorkDescription = objPM.WorkDescription;
                model.Severity = objPM.Severity;
                model.IsObservation = objPM.IsObservation;
                // model.PreferredVendorId = objPM.PreferredVendorId;
                // model.VendorCategoryId = objPM.Vendor.CategoryId;

                //model.WorkName = objPM.PreventiveWorkDescription.Description;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SavePM(PreventiveMaintenance PreventiveMaintenance, List<int> UserIds)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                PreventiveMaintenance PMobj = new Data.Models.PreventiveMaintenance();

                if (PreventiveMaintenance.Id == 0)
                {
                    if (PreventiveMaintenance.ScheduleStartDate == null)
                    {
                        result = new { Success = "false", Message = "Schedule start date is required!" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (PreventiveMaintenance.ScheduleStartDate.Value.Date < DateTime.Now.Date)
                    {
                        result = new { Success = "false", Message = "Schedule start date can not be past date!" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    //DateTime nextReviewDate = DateTime.UtcNow;
                    //if (PreventiveMaintenance.ScheduleType == 1)
                    //{
                    //    nextReviewDate = DateTime.UtcNow.AddDays(PreventiveMaintenance.Interval);
                    //}
                    //else if (PreventiveMaintenance.ScheduleType == 2)
                    //{
                    //    nextReviewDate = DateTime.UtcNow.AddDays(7 * (PreventiveMaintenance.Interval));
                    //}
                    //else if (PreventiveMaintenance.ScheduleType == 3)
                    //{
                    //    nextReviewDate = DateTime.UtcNow.AddMonths(PreventiveMaintenance.Interval);
                    //}
                    //else if (PreventiveMaintenance.ScheduleType == 4)
                    //{
                    //    nextReviewDate = DateTime.UtcNow.AddYears(PreventiveMaintenance.Interval);
                    //}
                    PreventiveMaintenance.CreatedBy = HttpContext.User.Identity.GetUserId<int>();
                    PreventiveMaintenance.CreatedOn = DateTime.UtcNow;
                    //PreventiveMaintenance.UpdatedBy = HttpContext.User.Identity.GetUserId<int>();
                    //PreventiveMaintenance.UpdatedOn= DateTime.UtcNow;
                    PreventiveMaintenance.NextReviewDate = PreventiveMaintenance.ScheduleStartDate.Value.Date; //nextReviewDate.Date;
                    PreventiveMaintenance.IsDeleted = false;
                    PreventiveMaintenance.Id = this._pmServices.Add(PreventiveMaintenance);
                }
                else
                {
                    //PreventiveMaintenance.CreatedBy = HttpContext.User.Identity.GetUserId<int>();
                    //PreventiveMaintenance.CreatedOn = DateTime.Now;
                    PreventiveMaintenance.UpdatedBy = HttpContext.User.Identity.GetUserId<int>();
                    PreventiveMaintenance.UpdatedOn = DateTime.UtcNow;
                    this._pmServices.Update(PreventiveMaintenance);
                }

                this._uaServices.DeleteUA(PreventiveMaintenance.Id, UserIds);

                foreach (int id in UserIds)
                {
                    this._uaServices.Add(new UserAssignment
                    {
                        PreventiveMaintenanceId = PreventiveMaintenance.Id,
                        UserId = id
                    });
                }

            }
            catch (Exception ex)
            {
                string exceptionMsg = base.ProcessException(ex);
                result = new { Success = "false", Message = exceptionMsg };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUserList()
        {
            var allData = this._userService.GetAll();
            var users = allData.Select(s => new { s.Id, s.UserName });
            return Json(users, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetPWDList()
        {

            var allData = this._pwdServices.GetAll();
            var pwd = allData.Select(s => new { s.Id, s.Description });
            return Json(pwd, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetVCList()
        {

            var allData = this._vcServices.GetAll();
            var vc = allData.Select(s => new { s.Id, s.Name });
            return Json(vc, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetVendorList(int VendorCategoryId)
        {

            var allData = this._vendorServices.GetAll(VendorCategoryId);
            var vendor = allData.Select(s => new { s.Id, s.Name });
            return Json(vendor, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetScheduleTypeList()
        {

            var allData = this._pstServices.GetAll();
            var pst = allData.Select(s => new { s.Id, s.Description });
            return Json(pst, JsonRequestBehavior.AllowGet);

        }
        public JsonNetResult UpdateIsObservation(int PMId, bool IsObservation)
        {
            JsonResponse response = new Models.JsonResponse();

            try
            {
                this._pmServices.UpdateIsObservation(PMId, IsObservation);
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response = new JsonResponse { Status = JsonResponseStatus.Error, Message = ProcessException(ex) };
            }

            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePM(int[] Ids)
        {
            var result = new { Success = "true", Message = "Success" };

            try
            {
                DateTime deletedon = DateTime.UtcNow;
                int userId = HttpContext.User.Identity.GetUserId<int>();

                int[] ids = this._pmServices.Delete(Ids, deletedon, userId);

                if (ids.Length > 0)
                    result = new { Success = "false", Message = "Some dependency found in lines." };
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting PM.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Verified PreventiveMaintenance

        public ActionResult VerifyPreventive()
        {
            return View();
        }

        public JsonResult GetVPList(int PlantId, int LineId,int MachineId, int ScheduleType,int Verified, DateTime FromDate, DateTime ToDate)
        {
            var allData = this._pmServices.GetVPData(PlantId, LineId, MachineId, ScheduleType, Verified, FromDate, ToDate);
            var PM = allData.Select(s => new
            {
                s.Id,
                s.PlantName,
                s.LineName,
                s.MachineId,
                s.MachineName,
                s.WorkName,
                s.ScheduleTypeName,
                s.Interval,
                s.Severity,
                s.ReviewDate,
                s.ReviewBy,
                s.VerifyBy,
                s.VerifyDate
            });
            return Json(PM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonNetResult VerifyPreventiveData(int[] Ids)
        {
            DateTime verifyon = DateTime.UtcNow;
            int userId = HttpContext.User.Identity.GetUserId<int>();
            this._pmServices.VerifyPreventiveData(Ids,userId);
            return JsonNet(new { Status = 1, Message = "Data Verify successfully." }, JsonRequestBehavior.AllowGet);
        }

        #endregion 


        #region Report
        // GET: Report 1
        public ActionResult Report1()
        {
            return View();
        }

        public JsonResult GetListForReport1(DateTime FromDate, DateTime ToDate, int ScheduleType, int PlantId, int LinetId, int Activity,string WorkDescription)
        {
            //int userId = HttpContext.User.Identity.GetUserId<int>();
            //if (HttpContext.User.IsInRole("Level2") || HttpContext.User.IsInRole("Admin"))
            int userId = 0; // Show all report data to all users
            var allData = this._pmServices.GetReport1Data(userId, FromDate, ToDate, ScheduleType, PlantId, LinetId, Activity, WorkDescription);
            return Json(allData.Select(s => new { s.UserName, s.ReviewDate, s.Notes, s.NextReviewDate, s.ScheduledReviewDate, s.MachineName, s.LineName, s.ScheduleTypeName, s.WorkName, s.ScheduleType, s.Interval, AssignedTo = string.Join(",", s.AssignedTo.ToArray()) }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetSummaryReportData(DateTime FromDate, DateTime ToDate, int ScheduleType, int PlantId, int LineId)
        {

            //int userId = HttpContext.User.Identity.GetUserId<int>();
            //if (HttpContext.User.IsInRole("Lavel2") || HttpContext.User.IsInRole("Admin"))
            int userId = 0; // Show all report data to all users
            var allData = this._pmServices.GetSummaryReportData(userId, FromDate, ToDate, ScheduleType, PlantId, LineId);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report2()
        {
            return View();
        }

        #endregion 

    }
}