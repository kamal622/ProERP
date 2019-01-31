using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ProERP.Data;
using ProERP.Services;
using ProERP.Web.Framework.Controllers;
using ProERP.Services.PreventiveMaintenance;
using ProERP.Data.Models;
using ProERP.Services.Models;
using ProERP.Services.Breakdown;
using ProERP.Web.Framework;
using ProERP.Services.Plant;
using ProERP.Services.MaintenanceRequest;
using ProERP.Services.Line;
using ProERP.Services.User;
using ProERP.Services.FormulationRequest;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly PreventiveMaintenanceService _pmServices;
        private readonly PreventiveReviewHistoryService _prhServices;
        private readonly PreventiveHoldHistoryService _phhServices;
        private readonly BreakdownService _breakDownService;
        private readonly PlantService _plantService;
        private readonly MaintenanceRequestServices _mrServices;
        private readonly UserService _userService;
        private readonly StatusServices _statusService;
        private readonly LineService _lineService;
        private readonly FormulationRequestService _formulationRequestService;
        public HomeController(PreventiveMaintenanceService pmServices, PreventiveReviewHistoryService prhServices, PreventiveHoldHistoryService phhServices, BreakdownService breakDownService, PlantService plantService, MaintenanceRequestServices mrServices, StatusServices statusService, LineService lineService, UserService userService, FormulationRequestService formulationRequestService)
        {
            this._pmServices = pmServices;
            this._prhServices = prhServices;
            this._phhServices = phhServices;
            this._breakDownService = breakDownService;
            this._plantService = plantService;
            this._mrServices = mrServices;
            this._statusService = statusService;
            this._lineService = lineService;
            this._userService = userService;
            this._formulationRequestService = formulationRequestService;
        }


        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        public ActionResult Index()
        {
            DashboardViewModels model = new DashboardViewModels();
            DateTime dtToday = DateTime.UtcNow.Date;
            DateTime dtYesterday = DateTime.UtcNow.Date.AddDays(-1);
            int userId = HttpContext.User.Identity.GetUserId<int>();
            var userRole = this._userService.GetUserRoleById(userId);

            model.BreakdownTodayCount = _breakDownService.GetBreakdownCounts(dtToday);
            model.BreakdownYesterDayCount = _breakDownService.GetBreakdownCounts(dtYesterday);

            model.PMOverDueCount = _pmServices.GetPMCounts(userId, 1);
            model.PMPendingCount = _pmServices.GetPMCounts(userId, 2);
            model.PMShutdownCount = _pmServices.GetPMCounts(userId, 5);
            model.BreakdownMonthPerCount = _breakDownService.CalcQOS();
            model.WhatsNewNotes = this._userService.GetWhatsNewNotes();
            model.ReleaseVersion = this._userService.GetVersion();

            model.NewFormulationRequestCount = this._formulationRequestService.GetFormulationCount(userId, 1);
            model.RMRequestCount = this._formulationRequestService.GetFormulationCount(userId, 2);
            model.RMDispatchCount = this._formulationRequestService.GetFormulationCount(userId, 3);
            model.ReadyForTestingCount = this._formulationRequestService.GetFormulationCount(userId, 3);
            model.UserRole = userRole;

            return View(model);
        }

        public ActionResult DashBoard()
        {
            DashboardViewModels model = new DashboardViewModels();
            DateTime dtToday = DateTime.UtcNow.Date;
            DateTime dtYesterday = DateTime.UtcNow.Date.AddDays(-1);
            int userId = HttpContext.User.Identity.GetUserId<int>();
            var userRole = this._userService.GetUserRoleById(userId);

            model.BreakdownTodayCount = _breakDownService.GetBreakdownCounts(dtToday);
            model.BreakdownYesterDayCount = _breakDownService.GetBreakdownCounts(dtYesterday);

            model.PMOverDueCount = _pmServices.GetPMCounts(userId, 1);
            model.PMPendingCount = _pmServices.GetPMCounts(userId, 2);
            model.PMShutdownCount = _pmServices.GetPMCounts(userId, 5);
            //model.AuditTaskPendingCount = _mrServices.GetAuditTaskPendingCount();
            //model.AuditTaskAllCount = _mrServices.GetAuditTaskAllCount();
            //model.MaintenanceRequestOpenCount = _mrServices.GetMaintenanceRequestOpenCount();
            //model.MaintenanceRequestInProcessCount = _mrServices.GetMaintenanceRequestInProcessCount();
            model.BreakdownMonthPerCount = _breakDownService.CalcQOS();
            model.WhatsNewNotes = this._userService.GetWhatsNewNotes();
            model.ReleaseVersion = this._userService.GetVersion();

            model.NewFormulationRequestCount = this._formulationRequestService.GetFormulationCount(userId, 1);
            model.RMRequestCount = this._formulationRequestService.GetFormulationCount(userId, 2);
            model.RMDispatchCount = this._formulationRequestService.GetFormulationCount(userId, 3);
            model.ReadyForTestingCount = this._formulationRequestService.GetFormulationCount(userId, 3);
            model.UserRole = userRole;

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult GetPMScheduleListForUser(int PMType)
        {
            int userId = HttpContext.User.Identity.GetUserId<int>();
            var allData = this._pmServices.GetDashboardData(userId, PMType);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetAuditTaskList()
        //{

        //    var allData = this._mrServices.GetDashboardData();
        //    var AT = allData.Select(s => new
        //    {

        //        s.Id,
        //        s.PlantId,
        //        s.MachineId,
        //        s.AuditDate,
        //        s.AuditBy,
        //        s.UpdateBy,
        //        s.UpdateDate,
        //        PlantName = s.Plant.Name,
        //        LineName = (s.Line == null ? "NA" : s.Line.Name),
        //        MachineName = (s.Machine == null ? "NA" : s.Machine.Name),
        //        s.TaskDescription,
        //        PrioritytName = s.MaintenancePriorityType.Description,
        //        StatusName = s.Status.Description,
        //        // AuditUserName = ((s.AuditBy == null) ? " " : s.User.UserName),
        //        UpdateUserName = ((s.UpdateBy == null) ? " " : s.User1.UserName)

        //    });
        //    return Json(AT, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetMRListForDashboard(int StatusId, int pagenum, int pagesize, string sortdatafield, string sortorder)
        {
            var query= Request.QueryString;
            var username = this._userService.GetAll();
            int userId = HttpContext.User.Identity.GetUserId<int>();
            int allDataCount = 0;
            var allData = this._mrServices.GetDashboardData(userId, StatusId, pagenum, pagesize, sortdatafield, sortorder, out allDataCount, query);//,sortdatafield,sortorder
            var data = allData;
            var total = allDataCount;
            var result = new
            {
                TotalRows = total,
                Rows = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatusList()
        {

            var allData = this._statusService.GetselectedData();
            var status = allData.Select(s => new { s.Id, s.Description });
            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SaveReview(int PMId, string Note, int scheduleType, int Interval, DateTime ReviewDate, bool isOverDue)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                int userId = HttpContext.User.Identity.GetUserId<int>();
                this._pmServices.UpdateReviewDate(PMId, Note, userId, ReviewDate, isOverDue);
                //this._prhServices.Add(new PreventiveReviewHistory { PreventiveId = PMId, Notes = Note, ReviewBy = userId, ReviewDate = lastReviewDate });
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = ex.Message + ex.InnerException ?? ex.InnerException.Message };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveHoldDays(int PMId, string Reason, int HoldDays)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                //DateTime nextReviewDate = DateTime.UtcNow;
                //nextReviewDate = DateTime.UtcNow.AddDays(HoldDays);


                DateTime holdon = DateTime.UtcNow;
                int userId = HttpContext.User.Identity.GetUserId<int>();

                int holdId = this._phhServices.Add(new PreventiveHoldHistory { PreventiveId = PMId, Reason = Reason, HoldBy = userId, HoldOn = holdon, HoldDays = HoldDays });
                this._pmServices.UpdateNextReviewDate(PMId, holdId, HoldDays);
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in adding HoldDays.Please contact Admin." };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult SaveAuditTask(int ATId, string WorkConducted, int StatusId, DateTime? CloseDateTime)
        //{
        //    var result = new { Success = "true", Message = "Success" };
        //    try
        //    {
        //        DateTime updatedate = DateTime.UtcNow;
        //        int userId = HttpContext.User.Identity.GetUserId<int>();
        //        this._mrServices.UpdateAuditTask(ATId, WorkConducted, StatusId, CloseDateTime, updatedate, userId);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = new { Success = "false", Message = "Problem in Updating.Please contact Admin." };
        //    }

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetBreakdownCountsByPlant()
        {
            DashboardModel model = new DashboardModel();
            var BD = _breakDownService.GetBreakdownCountsByPlant();

            return Json(BD, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBreakdownCountsByLine(int plantId)
        {
            DashboardModel model = new DashboardModel();
            var BD = _breakDownService.GetBreakdownCountsByLine(plantId);

            return Json(BD, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBreakdownCountsByMachine(int lineId)
        {
            DashboardModel model = new DashboardModel();
            var BD = _breakDownService.GetBreakdownCountsByMachine(lineId);

            return Json(BD, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetAuditTaskData(int Id)
        //{
        //    MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();
        //    Data.Models.MaintenanceRequest objAT = this._mrServices.GetForId(Id);

        //    if (objAT != null)
        //    {

        //        model.StatusId = Convert.ToInt32(objAT.StatusId);
        //        model.WorkConducted = objAT.WorkConducted;
        //        model.CloseDateTime = objAT.CloseDateTime;

        //    }
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}
        public JsonNetResult GetPlantsForChart()
        {
            var allPlants = this._plantService.GetAllPlants();
            return JsonNet(allPlants.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetLinesForPlant(int PlantId)
        {
            Line[] allPlants = this._lineService.GetDashboardForLine(PlantId);
            return JsonNet(allPlants.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonNetResult UpdateVersionStatus()
        {
            JsonResponse response = new Models.JsonResponse();
            try
            {
                int userId = HttpContext.User.Identity.GetUserId<int>();
                this._userService.IsVersionUpdated(userId, false);
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception e)
            {
                string exceptionMsg = base.ProcessException(e);
                response.Status = JsonResponseStatus.Error;
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVersionStatus()
        {
            JsonResponse response = new Models.JsonResponse();
            try
            {
                int userId = HttpContext.User.Identity.GetUserId<int>();
                var data = this._userService.GetVersionStatus(userId);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string exceptionMsg = base.ProcessException(e);
                response.Status = JsonResponseStatus.Error;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllNotes(string Id)
        {
            int pageNo = Convert.ToInt32(Id);
            var Data = this._userService.GetAllNotes();
            ReleaseNoteViewModel model = new ReleaseNoteViewModel();
            model.PageSize = 5;
            model.AllData = Data.OrderByDescending(o => o.PLMMVersion.ReleaseDate).Skip((pageNo - 1) * model.PageSize).Take(model.PageSize).ToList();
            model.CurrentPage = pageNo;
            model.TotalPages = Math.Ceiling((double)Data.Count() / model.PageSize);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ViewNotes()
        {
            return View();
        }

        public JsonResult GetAllVersion()
        {
            var allData = this._userService.GetAllVersion();
            var finalData = allData.Select(s => new
            {
                s.Id,
                ReleaseVersion = s.ReleaseVersion,
                ReleaseDate = s.ReleaseDate,
            });
            return Json(finalData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNestedGridData(int VersionId)
        {
            var allData = this._userService.GetNotes(VersionId);
            var finalData = allData.Select(s => new
            {
                s.Id,
                s.Notes
            });
            return Json(finalData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult SaveNotes(VersionGridModel NotesData, int[] DeletedNotesIds)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._userService.SaveNotesData(NotesData, DeletedNotesIds);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Data saved successfully.";
            }
            catch (Exception e)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(e);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult DeleteVersion(int Id)
        {
            this._userService.DeleteVersion(Id);
            return JsonNet(new { Status = 1, Message = "Data deleted successfully." }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNotesByVersionId(int VersionId)
        {
            var allData = this._userService.getVersionId(VersionId);
            var data = allData.Select(s => new
            {
                s.Id,
                s.VersionId,
                Notes = s.Notes
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Install()
        {

            var roleManager = new RoleManager<ProERP.Web.Models.Role, int>(new RoleStore<ProERP.Web.Models.Role, int, ProERP.Web.Models.UserRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists("SysAdmin"))
            {
                var role = new ProERP.Web.Models.Role();
                role.Name = "SysAdmin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new ProERP.Web.Models.Role();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Lavel1"))
            {
                var role = new ProERP.Web.Models.Role();
                role.Name = "Lavel1";
                roleManager.Create(role);
            }

            if (roleManager.RoleExists("Employee"))
            {
                roleManager.Delete(roleManager.FindByName("Employee"));
            }

            if (UserManager.FindByName("SysAdmin") == null)
            {
                var user = new ProERP.Web.Models.User { UserName = "SysAdmin", Email = "SysAdmin@ProERP.com", EmailConfirmed = true };
                var result = UserManager.Create(user, "Admin@1234");
                UserManager.AddToRole(user.Id, "SysAdmin");
            }

            if (UserManager.FindByName("Admin") == null)
            {
                var user = new ProERP.Web.Models.User { UserName = "Admin", Email = "Admin@ProERP.com", EmailConfirmed = true };
                var result = UserManager.Create(user, "Admin!1234");
                UserManager.AddToRole(user.Id, "Admin");
            }
            return View();

        }

        public JsonNetResult GetCounts()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId<int>();

            var user = System.Web.HttpContext.Current.User.Identity.GetUserName();
            string firstName = "", lastName = "";
            DashboardViewModels model = new DashboardViewModels();

            //model.PMPendingCount = this._pmServices.GetPMCounts(userId, 2);
            //model.PMOverDueCount = this._pmServices.GetPMCounts(userId, 1);
            //model.PMShutdownCount = this._pmServices.GetPMCounts(userId, 5);
            //model.MaintenanceRequestOpenCount = this._mrServices.GetMaintenanceRequestOpenCount(userId);
            //model.MaintenanceRequestInProcessCount = this._mrServices.GetMaintenanceRequestInProcessCount(userId);
            _userService.GetFnameLname(user, out firstName, out lastName);
            model.FirstName = firstName;
            model.LastName = lastName;

            model.NewFormulationRequestCount = this._formulationRequestService.GetFormulationCount(userId, 1);
            model.RMRequestCount = this._formulationRequestService.GetFormulationCount(userId, 2);
            model.RMDispatchCount = this._formulationRequestService.GetFormulationCount(userId, 3);
            model.ReadyForTestingCount = this._formulationRequestService.GetFormulationCount(userId, 4);
            return JsonNet(model, JsonRequestBehavior.AllowGet);
        }


    }
}