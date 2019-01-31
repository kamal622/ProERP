using ProERP.Services.Machine;
using ProERP.Services.MaintenanceRequest;
using ProERP.Services.Plant;
using ProERP.Services.Site;
using ProERP.Services.User;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using ProERP.Web.Framework.Controllers;

[Authorize(Roles = "SysAdmin, Admin,Lavel2,Lavel1")]
public class AuditTaskController : BaseController
{
    private readonly MaintenanceRequestServices _mrServices;
    private readonly StatusServices _statusService;
    private readonly SiteService _siteService;
    private readonly PlantService _plantService;
    private readonly MachineService _machineService;
    private readonly UserService _userService;
    public AuditTaskController(MaintenanceRequestServices mrServices, StatusServices statusService, SiteService siteService, PlantService plantService, MachineService machineService, UserService userService)
    {
        this._mrServices = mrServices;
        this._statusService = statusService;
        this._siteService = siteService;
        this._plantService = plantService;
        this._machineService = machineService;
        this._userService = userService;
    }
    // GET: AuditTask
    public ActionResult Index()
    {
        return View();
    }
    public ActionResult List()
    {
        return View();
    }

    //public JsonResult GetATList(string Name, int PlantId, int PriorityId, int StatusId)
    //{

    //    var allData = this._mrServices.GetATData(Name, PlantId, PriorityId, StatusId);
    //    var AT = allData.Select(s => new
    //    {

    //        s.Id,
    //        s.PlantId,
    //        s.MachineId,
    //        s.AuditDate,
    //        s.AuditBy,
    //        MachineName = s.Machine.Name,
    //        s.TaskDescription,
    //        PrioritytName = s.MaintenancePriorityType.Description,
    //        StatusName = s.Status.Description,
    //        UserName = ((s.AuditBy == null) ? "  " : s.User.UserName)

    //    });
    //    return Json(AT, JsonRequestBehavior.AllowGet);
    //}

    //public JsonResult SaveApproveTask(int MRId)
    //{
    //    var result = new { Success = "true", Message = "Success" };
    //    try
    //    {
    //        int userId = HttpContext.User.Identity.GetUserId<int>();
    //        DateTime auditDate = DateTime.UtcNow;
    //        // StatusId = 3;
    //        this._mrServices.UpdateApproveTask(MRId, auditDate, 3, userId);
    //    }
    //    catch (Exception ex)
    //    {
    //        result = new { Success = "false", Message = "Problem in adding Approve Task.Please contact Admin." };
    //    }

    //    return Json(result, JsonRequestBehavior.AllowGet);
    //}

    //public JsonResult SaveRejectTask(int MRId)
    //{
    //    var result = new { Success = "true", Message = "Success" };
    //    try
    //    {
    //        int userId = HttpContext.User.Identity.GetUserId<int>();
    //        DateTime auditDate = DateTime.UtcNow;

    //        this._mrServices.UpdateApproveTask(MRId, auditDate, 4, userId);
    //    }
    //    catch (Exception ex)
    //    {
    //        result = new { Success = "false", Message = "Problem in adding Reject Task.Please contact Admin." };
    //    }

    //    return Json(result, JsonRequestBehavior.AllowGet);
   // }
    //public JsonResult DeleteAT(int[] Ids)
    //    {
    //        var result = new { Success = "true", Message = "Success" };

    //        try
    //        {
    //            int[] ids = this._mrServices.Delete(Ids);
    //            if (ids.Length > 0)
    //                result = new { Success = "false", Message = "Some dependency found in lines." };
    //        }
    //        catch (Exception ex)
    //        {
    //            result = new { Success = "false", Message = "Problem in deleting Machine.Please contact Admin." };
    //        }
    //        return Json(result, JsonRequestBehavior.AllowGet);
    //    }

}
