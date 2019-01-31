using Microsoft.AspNet.Identity;
using ProERP.Data.Models;
using ProERP.Services.Indent;
using ProERP.Services.PreventiveMaintenance;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using ProERP.Web.Framework;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using ProERP.Services.User;
using System.Threading.Tasks;
using System.Globalization;

namespace ProERP.Web.Controllers
{
    [Authorize(Roles = "Lavel1,Lavel2")]
    public class IndentController : BaseController
    {
        // GET: Indent
        private readonly ItemsServices _ItemsService;
        private readonly IndentsServices _IndentsServices;
        private readonly IndentDetailServices _IndentDetailServices;
        private readonly VendorService _vendorServices;
        private readonly IndentStatusServices _IndentStatusServices;
        private readonly IndentBudgetServices _IndentBudgetServices;
        private readonly IndentDetailAttachmentServices _IndentDetailAttachmentServices;
        private readonly UserService _userService;
        private ApplicationUserManager _userManager;

        public IndentController(ItemsServices ItemsService, IndentsServices IndentsServices, IndentDetailServices IndentDetailServices, VendorService vendorServices, IndentStatusServices IndentStatusServices, IndentBudgetServices IndentBudgetServices, IndentDetailAttachmentServices IndentDetailAttachmentServices, UserService userService)
        {
            this._ItemsService = ItemsService;
            this._IndentsServices = IndentsServices;
            this._IndentDetailServices = IndentDetailServices;
            this._vendorServices = vendorServices;
            this._IndentStatusServices = IndentStatusServices;
            this._IndentBudgetServices = IndentBudgetServices;
            this._IndentDetailAttachmentServices = IndentDetailAttachmentServices;
            this._userService = userService;
        }

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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {

            return View();
        }
        public ActionResult AddIndentBudget()
        {
            IndentBudgetViewModel model = new IndentBudgetViewModel();
            model.Id = 0;
            return View(model);

        }
        public ActionResult UpdateIndentBudget(int? Id)
        {
            IndentBudgetViewModel model = new IndentBudgetViewModel();
            model.Id = Id ?? 0;
            return View(model);

        }
        public ActionResult ListIndentBudget()
        {

            return View();
        }
        public ActionResult Add()
        {
            IndentViewModel model = new IndentViewModel();
            model.Id = 0;
            model.ItemViewModel = new ItemViewModel { Id = 0 };
            model.VendorViewModel = new VendorViewModel { Id = 0 };
            return View(model);
        }
        public ActionResult Update(int? Id)
        {
            IndentViewModel model = new IndentViewModel();
            model.Id = Id ?? 0;
            model.ItemViewModel = new ItemViewModel { Id = 0 };
            model.VendorViewModel = new VendorViewModel { Id = 0 };
            return View(model);
        }
        public JsonResult GetItemNameList()
        {
            var allData = this._ItemsService.GetAllItemNameList();
            var vc = allData.Select(s => new { s.Id, s.Name });
            return Json(vc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCurrencyList()
        {
            var allData = this._IndentDetailServices.GetAllCurrencyList();
            var currency = allData.Select(s => new { s.Id, s.Currency });
            return Json(currency, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUOMList()
        {
            var allData = this._ItemsService.GetAllItemNameList();
            var vc = allData.Select(s => new { s.Id, s.UnitOfMeasure });
            return Json(vc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVendorList()
        {
            var allData = this._vendorServices.GetAllVendorList();
            var vendor = allData.Select(s => new { s.Id, s.Name });
            return Json(vendor, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUserList()
        {
            var allData = this._userService.GetAll();
            allData.Insert(0, new Data.Models.User { Id = 0, UserName = "ALL" });
            var User = allData.Select(s => new { s.Id, s.UserName });
            return Json(User, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStatusList()
        {
            var roles = this.UserManager.GetRoles(HttpContext.User.Identity.GetUserId<int>());
            var allData = this._IndentStatusServices.GetAll(roles);
            var status = allData.Select(s => new { s.Id, s.Description });
            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetIndentStatusList()
        {
            var roles = this.UserManager.GetRoles(HttpContext.User.Identity.GetUserId<int>());
            var allData = this._IndentStatusServices.GetAllStatus(roles);
            var status = allData.Select(s => new { s.Id, s.Description });
            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBudgetCodeList(string BudgetType)
        {
            var allData = this._IndentBudgetServices.GetAllBudgetCode(BudgetType);
            var BC = allData.Select(s => new { s.Id, BudgetCode = s.BudgetCode + " (" + s.FinancialYear + ")" });
            return Json(BC, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetItemById(int Id)
        {
            ItemViewModel model = new ItemViewModel();

            Data.Models.Item objIn = this._ItemsService.GetForId(Id);

            if (objIn != null)
            {
                model.Id = objIn.Id;
                model.ItemCode = objIn.ItemCode;
                model.Name = objIn.Name;
                // model.SpecificationFile = objIn.SpecificationFile;
                model.Make = objIn.Make;
                model.MOC = objIn.MOC;
                model.Model = objIn.Model;
                model.Price = objIn.Price;
                model.AvailableQty = objIn.AvailableQty;
                model.UnitOfMeasure = objIn.UnitOfMeasure;
                model.Description = objIn.Description;
                model.VendorId = objIn.VendorId;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveGenerateIndent(Indent Indent)
        {
            var result = new { Success = "true", Message = "Success", Data = new { IndentId = 0, IndentNo = "" } };
            try
            {
                int userId = HttpContext.User.Identity.GetUserId<int>();
                Indent.CreatedOn = DateTime.UtcNow;
                Indent.CreatedBy = userId;
                Indent.StatusId = 1;
                int id = this._IndentsServices.Add(Indent);
                //Indent.IndentNo = string.Format("{0}/{1}/{2}/{3}{4}{5}",id, budget, HttpContext.User.Identity.Name, DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                //this._IndentsServices.UpdateIndentNo(id, Indent.IndentNo);
                result = new { Success = "true", Message = "Success", Data = new { IndentId = id, IndentNo = Indent.IndentNo } };
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = (ex.Message + ex.InnerException ?? ex.InnerException.Message) + "<br/>" + ex.StackTrace, Data = new { IndentId = 0, IndentNo = "" } };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult SaveIndentDetail(IndentDetail IndentDetail, string itemDescription)
        {
            var result = new JsonResponse();
            try
            {
                result.Status = JsonResponseStatus.Success;
                IndentDetail IDobj = new Data.Models.IndentDetail();
                if (IndentDetail.Id == 0)
                {
                    if (IndentDetail.StatusId > 1)
                    {
                        result.Status = JsonResponseStatus.Warning;
                        result.Message = "Status should be 'New' for new request. Data not saved.";
                    }
                    else
                    {
                        IndentDetail.Id = this._IndentDetailServices.Add(IndentDetail, itemDescription);
                        result.Data = new { Id = IndentDetail.Id };
                    }
                }
                else
                {
                    result.Data = new { Id = IndentDetail.Id };
                    if (IndentDetail.StatusId == 2) //Approved
                    {
                        if (HttpContext.User.IsInRole("Lavel2"))
                        {
                            int userId = HttpContext.User.Identity.GetUserId<int>();
                            DateTime approvedDate = DateTime.UtcNow;
                            DateTime rejectedDate = DateTime.UtcNow;
                            this._IndentDetailServices.UpdateApproveTask(IndentDetail, approvedDate, userId, rejectedDate);
                        }
                        else
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "You are not authorized to do this action. Data not saved.";
                        }

                    }
                    if (IndentDetail.StatusId == 3) //Rejected
                    {
                        if (HttpContext.User.IsInRole("Lavel2"))
                        {
                            int userId = HttpContext.User.Identity.GetUserId<int>();
                            DateTime rejectedDate = DateTime.UtcNow;
                            DateTime approvedDate = DateTime.UtcNow;
                            this._IndentDetailServices.UpdateRejectTask(IndentDetail, rejectedDate, userId, approvedDate);
                        }
                        else
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "You are not authorized to do this action. Data not saved.";
                        }
                    }
                    if (IndentDetail.StatusId == 4) //PO
                    {

                        var existing = this._IndentDetailServices.GetForId(IndentDetail.Id);
                        int oldStatusId = Convert.ToInt32(existing.StatusId); // Get this from db.

                        if (oldStatusId == 2)
                        {
                            if (IndentDetail.PoDate == null)
                            {
                                result.Status = JsonResponseStatus.Warning;
                                result.Message = "'Please insert PO Date.";
                            }
                            else if (IndentDetail.DeliveryDate == null)
                            {
                                result.Status = JsonResponseStatus.Warning;
                                result.Message = "Please insert delivery Date.";
                            }
                            else if (IndentDetail.PoNo == null)
                            {
                                result.Status = JsonResponseStatus.Warning;
                                result.Message = "Please insert PO no.";
                            }
                            else if(IndentDetail.PoAmount <= 0)
                            {
                                result.Status = JsonResponseStatus.Warning;
                                result.Message = "Please insert PO Amount greater than zero.";
                            }
                            else
                            {
                                this._IndentDetailServices.UpdatePoTask(IndentDetail);
                            }
                                
                        }
                        else if (oldStatusId == 3)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is already rejected. You can not change it's status.";
                        }
                        else if (oldStatusId == 4)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "PO already placed. Data not saved.";
                        }
                        else if (oldStatusId == 5)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is already received. You can not change it's status.";
                        }

                        else
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is not approved yet. Data not saved.";
                        }
                    }
                    if (IndentDetail.StatusId == 5) //Issued
                    {
                        var existing = this._IndentDetailServices.GetForId(IndentDetail.Id);
                        int oldStatusId = Convert.ToInt32(existing.StatusId); // Get this from db.

                        int userId = HttpContext.User.Identity.GetUserId<int>();
                        DateTime issuedDate = DateTime.UtcNow;

                        if (oldStatusId == 2 || oldStatusId == 4)
                        {
                            if (IndentDetail.IssuedQty == null)
                            {
                                result.Status = JsonResponseStatus.Warning;
                                result.Message = "Issued quantity must be greater than zero(0).";
                            }
                            else if (IndentDetail.PoAmount == 0)
                            {
                                result.Status = JsonResponseStatus.Warning;
                                result.Message = "Please insert PO Amount.";
                            }
                            else
                                this._IndentDetailServices.UpdateIssuedTask(IndentDetail, issuedDate, userId);
                        }
                        else if (oldStatusId == 3)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is already rejected. You can not change it's status.";
                        }
                        else if (oldStatusId == 5)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item already received. Can not issue again.";
                        }
                        else
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is not approved yet. Data not saved.";
                        }
                    }
                    if (IndentDetail.StatusId == 1) //New
                    {
                        var existing = this._IndentDetailServices.GetForId(IndentDetail.Id);
                        int oldStatusId = Convert.ToInt32(existing.StatusId); // Get this from db.

                        if (oldStatusId == 5)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is already received. You can not change it's status.";
                        }
                        else if (oldStatusId == 4)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is already PO. You can not change it's status.";
                        }
                        else if (oldStatusId == 3)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is already rejected. You can not change it's status.";
                        }
                        else if (oldStatusId == 2)
                        {
                            result.Status = JsonResponseStatus.Warning;
                            result.Message = "This item is already approved. You can not change it's status.";
                        }
                        else
                        {
                            this._IndentDetailServices.Update(IndentDetail, itemDescription);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIndentGridList(int IndentId)
        {
            var allData = this._IndentDetailServices.GetIndentDetailData(IndentId);
            var IndentDetail = allData.Select(s => new
            {
                s.Id,
                s.IndentId,
                s.QtyNeeded,
                s.RequiredByDate,
                ItemCode = s.Item == null ? "" : s.Item.ItemCode,
                Name = s.Item == null ? "" : s.Item.Name,
                PlantName = s.Plant == null ? "NA" : s.Plant.Name,
                StatusName = s.IndentStatu.Description,
                JobDescription = s.JobDescription == null ? "" : s.JobDescription
            });
            return Json(IndentDetail, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNestedGridData(int IndentId)
        {
            var allData = this._IndentDetailServices.GetNestedGridData(IndentId);
            var IndentDetail = allData.Select(s => new
            {
                s.Id,
                s.IndentId,
                s.QtyNeeded,
                s.RequiredByDate,
                ItemCode = s.Item == null ? "" : s.Item.ItemCode,
                Name = s.Item == null ? "" : s.Item.Name,
                StatusName = s.IndentStatu.Description,
                PlantName = s.Plant == null ? "NA" : s.Plant.Name,
                JobDescription = s.JobDescription == null ? "" : s.JobDescription
            });
            return Json(IndentDetail, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIndentGridData(int User, string Name, string RequisitionType)
        {
            var allData = this._IndentsServices.GetIndentGridData(User, Name, RequisitionType);
            var Indent = allData.Select(s => new
            {
                s.Id,
                s.IndentNo,
                PriorityName = s.MaintenancePriorityType.Description,
                s.CreatedOn,
                s.RequisitionType,
                Status=s.IndentStatu.Description
            });
            return Json(Indent, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIndentById(int IndentId)
        {
            GenerateIndentViewModel model = new GenerateIndentViewModel();

            Data.Models.Indent objIn = this._IndentsServices.GetForId(IndentId);

            if (objIn != null)
            {
                model.Id = objIn.Id;
                model.IndentNo = objIn.IndentNo;
                model.Priority = objIn.Priority;
                model.BudgetType = objIn.IndentBudget.BudgetType;
                model.BudgetId = objIn.BudgetId;
                model.Note = objIn.Note;
                model.Subject = objIn.Subject;
                model.RequisitionType = objIn.RequisitionType;
                model.BudgetHead = objIn.BudgetHead;
                model.StatusId = objIn.StatusId;
                model.PoDate = objIn.PoDate;
                model.DeliveryDate = objIn.DeliveryDate;
                model.PoNo = objIn.PoNo;
                model.PoAmount = objIn.PoAmount;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetRemainingBudgetData(int Id)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                var remainingBudget = this._IndentBudgetServices.GetRemainingBudget(Id);
                CultureInfo hindi = new CultureInfo("hi-IN");
                string text = string.Format(hindi, "{0:c}", remainingBudget);

                response.Data = text;
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message + ex.StackTrace;
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteIndentDetail(int[] Ids)
        {
            this._IndentDetailServices.DeleteIndentDetail(Ids);
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAllIndent(int Id)
        {
            this._IndentDetailServices.DeleteAllIndent(Id);
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIndentDetailsById(int Id)
        {
            ProERP.Services.Models.IndentDetailViewModel model = this._IndentDetailServices.GetForId(Id);

            //if (objID != null)
            //{
            //    model.Id = objID.Id;
            //    model.PreferredVendorId = objID.PreferredVendorId;
            //    model.ItemId = objID.ItemId;
            //    model.QtyNeeded = objID.QtyNeeded;
            //    model.StatusId = objID.StatusId;
            //    model.PlantId = objID.PlantId;
            //    model.LineId = objID.LineId;
            //    model.MachineId = objID.MachineId;
            //    model.RequiredByDate = objID.RequiredByDate;
            //    model.PoDate = objID.PoDate;
            //    model.DeliveryDate = objID.DeliveryDate;
            //    model.PoNo = objID.PoNo;
            //    model.IssuedQty = objID.IssuedQty;
            //    model.FinalPrice = objID.FinalPrice;
            //    //model.UserName = objID.User.UserName;
            //    model.ApprovedOn = objID.ApprovedOn;
            //    //model.RejectedBy = objID.RejectedBy;
            //    //model.UserName2 = objID.User2.UserName;
            //    model.Rejectedon = objID.Rejectedon;

            //}
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult UpdateStatusandNote(int indentId, string note, int StatusId, string Subject,DateTime? PoDate,DateTime? DeliveryDate,string PoNo,decimal? PoAmount)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                int userId = HttpContext.User.Identity.GetUserId<int>();
                DateTime approvedDate = DateTime.UtcNow;
                DateTime rejectedDate = DateTime.UtcNow;
                DateTime UpdatedDate = DateTime.UtcNow;
                response.Status = JsonResponseStatus.Success;

                if (StatusId == 1 ||StatusId == 2 || StatusId == 3)
                {
                    var oldStatus = this._IndentDetailServices.getIndentStatus(indentId, 4);
                    if (oldStatus == true)
                    {
                        response.Status = JsonResponseStatus.Warning;
                        response.Message = "PO already placed. Can not update status.";
                        return JsonNet(response, JsonRequestBehavior.AllowGet);
                    }
                }
                if (StatusId == 4)
                {
                    var oldStatus = this._IndentDetailServices.getIndentStatus(indentId,1);
                    if (oldStatus == true)
                    {
                        response.Status = JsonResponseStatus.Warning;
                        response.Message = "Indent is not approved yet. You can not change status to PO.";
                        return JsonNet(response, JsonRequestBehavior.AllowGet);
                    }
                }
                if (StatusId == 1)
                {
                    var oldStatus = this._IndentDetailServices.getIndentStatus(indentId,2);
                    if (oldStatus == true)
                    {
                        response.Status = JsonResponseStatus.Warning;
                        response.Message = "This item is already approved. You can not change it's status.";
                        return JsonNet(response, JsonRequestBehavior.AllowGet);
                    }
                }
                if (StatusId == 4 || StatusId == 1)
                {
                    var oldStatus = this._IndentDetailServices.getIndentStatus(indentId,3);
                    if (oldStatus == true)
                    {
                        response.Status = JsonResponseStatus.Warning;
                        response.Message = "This item is already rejected. You can not change it's status.";
                        return JsonNet(response, JsonRequestBehavior.AllowGet);
                    }
                }
               
                this._IndentsServices.UpdateIndentNote(indentId, note, Subject, StatusId, userId, approvedDate, rejectedDate, PoDate, DeliveryDate, PoNo, PoAmount, UpdatedDate);
                this._IndentDetailServices.UpdateIndentDetailStatus(indentId, StatusId, userId, PoDate, DeliveryDate, PoNo, PoAmount);
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadIndentAttachments(HttpPostedFileBase[] fileToUpload)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "IndentAttachments");
            List<Data.Models.IndentDetailAttachment> attachments = new List<IndentDetailAttachment>();
            foreach (HttpPostedFileBase File in fileToUpload)
            {
                FileInfo fi = new FileInfo(File.FileName);
                string sysFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
                var filePath = Path.Combine(path, sysFileName);
                File.SaveAs(filePath);
                attachments.Add(new IndentDetailAttachment { OriginalFileName = fi.Name, SysFileName = sysFileName });
            }

            ViewBag.Attachments = JsonConvert.SerializeObject(attachments.Select(s => new { s.OriginalFileName, s.SysFileName }));
            return PartialView();
        }

        //Indent Budget
        public JsonNetResult SaveIndentBudget(IndentBudget IndentBudget)
        {
            var result = new JsonResponse();
            try
            {
                result.Status = JsonResponseStatus.Success;
                IndentBudget IBobj = new Data.Models.IndentBudget();
                if (IndentBudget.Id == 0)

                {
                    if (IndentBudget.BudgetType == null)
                    {
                        result.Status = JsonResponseStatus.Warning;
                        result.Message = "Please insert budget type.";
                    }
                    else if (IndentBudget.BudgetCode == null)
                    {
                        result.Status = JsonResponseStatus.Warning;
                        result.Message = "Please insert budget code.";
                    }
                    else if (IndentBudget.Amount == null)
                    {
                        result.Status = JsonResponseStatus.Warning;
                        result.Message = "Amount must be greater than zero(0).";
                    }
                    else
                        this._IndentBudgetServices.Add(IndentBudget);
                }
                else
                    this._IndentBudgetServices.Update(IndentBudget);
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                return JsonNet(new { Type = "Error", Message = ex.Message + ex.InnerException ?? ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIndentBudgetData(string BudgetType)
        {
            var allData = this._IndentBudgetServices.GetIndentBudgetData(BudgetType);
            var Indent = allData.Select(s => new
            {
                s.Id,
                s.BudgetType,
                s.BudgetCode,
                s.Amount,
                s.FinancialYear,
                s.IsActive
            });
            return Json(Indent, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult DeleteIndentBudget(int[] Ids)
        {
            var result = new JsonResponse();

            try
            {
                result.Status = JsonResponseStatus.Success;
                int[] ids = this._IndentBudgetServices.Delete(Ids);

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIndentBudgetById(int Id)
        {
            IndentBudgetViewModel model = new IndentBudgetViewModel();

            Data.Models.IndentBudget objIB = this._IndentBudgetServices.GetForId(Id);

            if (objIB != null)
            {
                model.Id = objIB.Id;
                model.BudgetType = objIB.BudgetType;
                model.BudgetCode = objIB.BudgetCode;
                model.Amount = objIB.Amount;
                model.IsActive = objIB.IsActive;
                model.FinancialYear = objIB.FinancialYear;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //Indent Attachments
        public JsonNetResult SaveAttachments(int IndentDetailId, string OriginalFileName, string SysFileName)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                this._IndentDetailAttachmentServices.Add(new IndentDetailAttachment { IndentDetailId = IndentDetailId, OriginalFileName = OriginalFileName, SysFileName = SysFileName });
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAttachmentGridList(int IndentDetailId)
        {
            var allData = this._IndentDetailAttachmentServices.GetAttachmentData(IndentDetailId);
            var IDAttachment = allData.Select(s => new
            {
                s.Id,
                s.IndentDetailId,
                s.OriginalFileName,
                s.SysFileName
            });
            return Json(IDAttachment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAttachment(int[] Ids)
        {
            this._IndentDetailAttachmentServices.Delete(Ids);
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}