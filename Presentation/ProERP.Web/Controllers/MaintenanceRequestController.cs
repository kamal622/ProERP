using ProERP.Data.Models;
using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.MaintenanceRequest;
using ProERP.Services.Plant;
using ProERP.Services.Site;
using ProERP.Services.User;
using ProERP.Services.Models;
using ProERP.Web.Framework;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class MaintenanceRequestController : BaseController
    {
        private readonly MaintenanceRequestServices _mrServices;
        private readonly SiteService _siteService;
        private readonly PlantService _plantService;
        private readonly LineService _lineService;
        private readonly MachineService _machineService;
        private readonly MaintenancePriorityTypeServices _mtpService;
        private readonly StatusServices _statusService;
        private readonly UserService _userService;
        public MaintenanceRequestController(MaintenanceRequestServices mrServices, SiteService siteService, PlantService plantService, LineService lineService, MachineService machineService, MaintenancePriorityTypeServices mtpService, StatusServices statusService, UserService userService)
        {
            this._mrServices = mrServices;
            this._siteService = siteService;
            this._plantService = plantService;
            this._lineService = lineService;
            this._machineService = machineService;
            this._mtpService = mtpService;
            this._statusService = statusService;
            this._userService = userService;
        }
        // GET: MaintenanceRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();
            model.Id = 0;
            return View(model);
        }
        public ActionResult List()
        {
            MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();
            model.Sites = _siteService.GetAll("").ToArray();
            return View(model);
        }

        public ActionResult Update(int? Id)
        {
            MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();

            model.Id = Id ?? 0;

            return View(model);
        }

        //public ActionResult UpdateApproveTask(int MRId, DateTime auditDate, int StatusId)
        //{
        //    MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();


        //    Data.Models.MaintenanceRequest objMR = this._mrServices.GetForId(model.Id);
        //    if (objMR != null)

        //        model.AuditDate = objMR.AuditDate;
        //    model.StatusId = 3;
        //    return View(model);
        //}

        //public ActionResult UpdateRejectTask(int MRId, DateTime auditDate, int StatusId)
        //{
        //    MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();


        //    Data.Models.MaintenanceRequest objMR = this._mrServices.GetForId(model.Id);
        //    if (objMR != null)

        //        model.AuditDate = objMR.AuditDate;
        //    model.StatusId = 4;
        //    return View(model);
        //}


        public JsonResult GetMRById(int Id)
        {
            MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();
            Data.Models.MaintenanceRequest objMR = this._mrServices.GetForId(Id);
            if (objMR != null)
            {
                model.Id = objMR.Id;
                model.SerialNo = objMR.SerialNo;
                model.RequestDate = objMR.RequestDate;
                TimeSpan rtime = objMR.RequestTime;
                model.RequestTime = new DateTime() + rtime;
                model.Problem = objMR.Problem;
                model.Description = objMR.Description;
                model.IsBreakdown = objMR.IsBreakdown;
                model.IsCritical = objMR.IsCritical;
                model.PlantId = objMR.PlantId;
                model.LineId = objMR.LineId;
                model.MachineId = objMR.MachineId;
                model.StatusId = objMR.StatusId;
                model.PriorityId = objMR.PriorityId;
                model.BreakdownType = objMR.BreakdownType;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMRForDashboard(int Id)
        {
            MaintenanceRequestViewModel model = new MaintenanceRequestViewModel();
            Data.Models.MaintenanceRequest objMR = this._mrServices.GetForId(Id);
            if (objMR != null)
            {
                model.Id = objMR.Id;
                model.SerialNo = objMR.SerialNo;
                model.RequestDate = objMR.RequestDate;
                TimeSpan rtime = objMR.RequestTime;
                model.RequestTime = new DateTime() + rtime;
                model.Problem = objMR.Problem;
                model.Description = objMR.Description;
                model.IsBreakdown = objMR.IsBreakdown;
                model.IsCritical = objMR.IsCritical;
                model.PlantId = objMR.PlantId;
                model.LineId = objMR.LineId;
                model.MachineId = objMR.MachineId;
                model.StatusId = objMR.StatusId;
                model.PriorityId = objMR.PriorityId;
                model.BreakdownType = objMR.BreakdownType;
                model.Remarks = objMR.Remarks;
                model.Remarks = objMR.Remarks;
                model.WorkStartDate = objMR.WorkStartDate;
                TimeSpan? starttime = objMR.WorkStartTime;
                model.WorkStartTime = new DateTime() + starttime;
                model.WorkEndDate = objMR.WorkEndDate;
                TimeSpan? endtime = objMR.WorkEndTime;
                model.WorkEndTime = new DateTime() + endtime;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserList()
        {
            var allData = this._userService.GetAll();
            var users = allData.Select(s => new { s.Id, s.UserName }).OrderBy(o=>o.UserName);
            return Json(users, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetMRList(int PlantId, int LineId, int MachineId, int PriorityId, int StatusId)
        {
            var username = this._userService.GetAll();
            var allData = this._mrServices.GetMRData(PlantId, LineId, MachineId, PriorityId, StatusId);
            var MR = allData.Select(s => new
            {
                s.Id,
                s.SerialNo,
                PlantId = (s.Plant == null ? 0 : s.PlantId.Value),
                PlantName = (s.Plant == null ? "NA" : s.Plant.Name),
                LineId = (s.Machine == null ? 0 : s.LineId.Value),
                LineName = (s.Line == null ? "NA" : s.Line.Name),
                MachineId = (s.Machine == null ? 0 : s.MachineId.Value),
                MachineName = (s.Machine == null ? "NA" : s.Machine.Name),
                s.Problem,
                s.RequestBy,
                s.RequestDate,
                Priority = s.MaintenancePriorityType.Description,
                Status = s.MaintanceRequestStatu.StatusName,
                IsBreakdown = s.IsBreakdown,
                RequestUserName = string.Join(", ", username.Where(w => w.Id == s.RequestBy).Select(q => q.UserName)),
                AssignUserName = string.Join(", ", username.Where(w => w.Id == s.AssignTo).Select(q => q.UserName))
            });
            return Json(MR, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMRRemarks(int mrId)
        {
            if( mrId != 0)
            {
                var allData = this._mrServices.GetRemarksById(mrId);
                var username = this._userService.GetAll();
                MRRemarksViewModel data = new MRRemarksViewModel();
                data.AllRemarks = new List<GridData>();
                data.AllRemarks.Add(new GridData { Id = 1, StatusName = "Open", Remarks = allData.Problem, RemarksDate = allData.RequestDate, RemarksBy = string.Join(", ", username.Where(w => w.Id == allData.RequestBy).Select(q => q.UserName)) });
                data.AllRemarks.Add(new GridData { Id = 2, StatusName = "Assigned", Remarks = allData.Remarks, RemarksDate = allData.AssignDate, RemarksBy = string.Join(", ", username.Where(w => w.Id == allData.AssignBy).Select(q => q.UserName)) });
                data.AllRemarks.Add(new GridData { Id = 3, StatusName = "InProgress", Remarks = allData.ProgressRemarks, RemarksDate = allData.ProgressDate, RemarksBy = string.Join(", ", username.Where(w => w.Id == allData.ProgressBy).Select(q => q.UserName)) });
                data.AllRemarks.Add(new GridData { Id = 4, StatusName = "Complete", Remarks = allData.CompleteRemarks, RemarksDate = allData.CompleteDate, RemarksBy = string.Join(", ", username.Where(w => w.Id == allData.CompleteBy).Select(q => q.UserName)) });
                data.AllRemarks.Add(new GridData { Id = 5, StatusName = "Close", Remarks = allData.CloseRemarks, RemarksDate = allData.CloseDate, RemarksBy = string.Join(", ", username.Where(w => w.Id == allData.CloseBy).Select(q => q.UserName)) });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPriorityList()
        {

            var allData = this._mtpService.GetAll();
            var priority = allData.Select(s => new { s.Id, s.Description });
            return Json(priority, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetStatusList()
        {

            var allData = this._statusService.GetStatusAll();
            var status = allData.Select(s => new { s.Id, s.StatusName });
            return Json(status, JsonRequestBehavior.AllowGet);

        }

        public JsonNetResult SaveMR(MaintenanceRequest MaintenanceRequest,DateTime requestTime)
        {
            var result = new JsonResponse();
            TimeSpan rtime = requestTime.TimeOfDay;
            try
            {
                if (MaintenanceRequest.Id == 0)
                {
                    
                    if (MaintenanceRequest.PlantId == 0)
                    {
                        MaintenanceRequest.PlantId = null;
                    }
                    if (MaintenanceRequest.LineId == 0)
                    {
                        MaintenanceRequest.LineId = null;
                    }
                    if (MaintenanceRequest.MachineId == 0)
                    {
                        MaintenanceRequest.MachineId = null;
                    }
                    MaintenanceRequest.StatusId = 1;
                    MaintenanceRequest.RequestTime = rtime;
                    if(MaintenanceRequest.PlantId != null)
                    {
                        MaintenanceRequest.BreakdownType = 1;
                    }
                    MaintenanceRequest.RequestBy = HttpContext.User.Identity.GetUserId<int>(); 
                    MaintenanceRequest.CreatedBy = HttpContext.User.Identity.GetUserId<int>();
                    MaintenanceRequest.CreatedDate = DateTime.Now;
                    MaintenanceRequest.Id = this._mrServices.Add(MaintenanceRequest);
                }
                else
                {
                    MaintenanceRequest.RequestTime = rtime;
                    this._mrServices.Update(MaintenanceRequest);
                }
                result.Status = JsonResponseStatus.Success;
                result.Message = "Maintenance request created";
                return JsonNet(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ProcessException(ex);
                return JsonNet(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonNetResult SaveRemarks(MaintenanceGridModel MaintenanceRequest)//MaintenanceRequest MaintenanceRequest
        {
            var result = new JsonResponse();
            try
            {
                if (MaintenanceRequest.Id != 0)
                {
                    var existing = this._mrServices.GetForId(MaintenanceRequest.Id);
                    TimeSpan? starttime = existing.WorkStartTime;
                    TimeSpan requesttime = existing.RequestTime;
                    DateTime? sDate = existing.WorkStartDate;//+ starttime;
                    DateTime? sTime = existing.WorkStartDate+ starttime;
                    DateTime rDate = existing.RequestDate;
                    DateTime rTime = existing.RequestDate + requesttime;
                    if (MaintenanceRequest.StatusId == 1)
                    {
                        MaintenanceRequest.AssignBy = HttpContext.User.Identity.GetUserId<int>();
                        MaintenanceRequest.RemarksBy = HttpContext.User.Identity.GetUserId<int>();
                        this._mrServices.SaveRemarks(MaintenanceRequest);
                        result.Status = JsonResponseStatus.Success;
                        result.Message = "Maintenance request updated";
                    }
                    else if(MaintenanceRequest.StatusId == 2)
                    {
                        MaintenanceRequest.ProgressBy = HttpContext.User.Identity.GetUserId<int>();
                        if (MaintenanceRequest.WorkStartDate <= rDate)//end date is less
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "Start date is less then working request date";
                        }
                        else if (MaintenanceRequest.WorkStartDate.Value.Date > rDate) //next day start time
                        {
                            this._mrServices.SaveRemarks(MaintenanceRequest);
                            result.Status = JsonResponseStatus.Success;
                            result.Message = "Maintenance request updated";
                        }
                        else if (MaintenanceRequest.WorkStartTime.Value.TimeOfDay > requesttime) //start time is greater
                        {
                            this._mrServices.SaveRemarks(MaintenanceRequest);
                            result.Status = JsonResponseStatus.Success;
                            result.Message = "Maintenance request updated";
                        }
                        else
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "Start time is less then working request time";
                        }
                    }
                    else if (MaintenanceRequest.StatusId == 3)
                    {
                        MaintenanceRequest.CompleteBy = HttpContext.User.Identity.GetUserId<int>();
                        if ( MaintenanceRequest.WorkEndDate <= sDate)//end date is less
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "Working end date is less then working start date";
                        }
                        else if (MaintenanceRequest.WorkEndDate.Value.Date > sDate) //next day start time
                        {
                            this._mrServices.SaveRemarks(MaintenanceRequest);
                            result.Status = JsonResponseStatus.Success;
                            result.Message = "Maintenance request updated";
                        }
                        else if (MaintenanceRequest.WorkEndTime.Value.TimeOfDay > starttime) //end time is greater
                        {
                            this._mrServices.SaveRemarks(MaintenanceRequest);
                            result.Status = JsonResponseStatus.Success;
                            result.Message = "Maintenance request updated";
                        }
                        else
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "Working end date is less then working start date";
                        }
                    }
                    else if (MaintenanceRequest.StatusId == 4)
                    {
                        MaintenanceRequest.CloseBy = HttpContext.User.Identity.GetUserId<int>();
                        this._mrServices.SaveRemarks(MaintenanceRequest);
                        result.Status = JsonResponseStatus.Success;
                        result.Message = "Maintenance request updated";
                    }
                    else
                    {
                        this._mrServices.SaveRemarks(MaintenanceRequest);
                        result.Status = JsonResponseStatus.Success;
                        result.Message = "Maintenance request updated";
                    }
                }
                return JsonNet(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ProcessException(ex);
                return JsonNet(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}