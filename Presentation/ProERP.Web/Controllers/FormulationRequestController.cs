using ProERP.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProERP.Services.FormulationRequest;
using ProERP.Data.Models;
using ProERP.Web.Models;
using ProERP.Services.Models;
using ProERP.Services.User;
using ProERP.Web.Framework;
using Microsoft.AspNet.Identity;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class FormulationRequestController : BaseController
    {
        // GET: FormulationRequest
        private readonly FormulationRequestService _formulationRequestService;
        private readonly UserService _userService;

        public FormulationRequestController(FormulationRequestService formulationRequestService,
                                         UserService userService)
        {
            this._formulationRequestService = formulationRequestService;
            this._userService = userService;
        }
        [Authorize(Roles = "Admin,QA,QAManager")]
        public ActionResult List()
        {
            FormulationRequestsViewModel model = new FormulationRequestsViewModel();
            int userId = HttpContext.User.Identity.GetUserId<int>();
            var userRole = this._userService.GetUserRoleById(userId);
            model.UserRole = userRole;
            return View(model);
        }

        [Authorize(Roles = "Admin,QA,QAManager")]
        public ActionResult Create(int? Id)
        {
            FormulationCreateViewModel model = new FormulationCreateViewModel();
            model.Id = Id ?? 0;
            return View(model);
        }

        public JsonNetResult SaveFormulationData(FormulationRequest formulationData, int[] formulaDetailDeletedIds,string Comment)
        {
            JsonResponse response = null;
            var Id = 0;
            try
            {
                response = new JsonResponse();
                formulationData.CreateBy = HttpContext.User.Identity.GetUserId<int>();
                formulationData.UpdateBy = HttpContext.User.Identity.GetUserId<int>();
                var createdby = HttpContext.User.Identity.GetUserId<int>();
                Id = this._formulationRequestService.SaveFormulationData(formulationData, formulaDetailDeletedIds, createdby, Comment);
                response.Status = JsonResponseStatus.Success;
                response.Message = "FormulationRequest saved";
                response.Data = Id;
                //Create(Id);
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveChangedFormulationData(FormulationRequest formulationData, int[] formulaDetailDeletedIds)
        {
            JsonResponse response = null;
            var Id = 0;
            try
            {
                response = new JsonResponse();
                formulationData.CreateBy = HttpContext.User.Identity.GetUserId<int>();
                formulationData.UpdateBy = HttpContext.User.Identity.GetUserId<int>();
                var createdby = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.SaveChangedFormulation(formulationData, formulaDetailDeletedIds, createdby);
                response.Status = JsonResponseStatus.Success;
                response.Message = "FormulationRequest saved";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult UpdateChangeRMData(int FormulationId)
        {
            JsonResponse resonse = null;
            try
            {
                resonse = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.UpdateChangedFormulationRM(FormulationId, userId);
                resonse.Status = JsonResponseStatus.Success;
            }
            catch(Exception ex)
            {
                resonse.Status = JsonResponseStatus.Error;
                resonse.Message = ProcessException(ex);
            }
            return JsonNet(resonse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFormulationGridData(string LotNo, int StatusId)
        {
            var allData = _formulationRequestService.GetFormulationRequestData(LotNo, StatusId);
            var FormulationData = allData.Select(s => new
            {
                s.Id,
                s.LOTSize,
                s.GradeName,
                s.LotNo,
                s.ColorSTD,
                s.QtyToProduce,
                //UOM=s.UnitOfMeasureMaster.Measurement,
                s.CreateDate,
                s.Notes,
                Status = s.FormulationRequestsStatu.StatusName,
                s.StatusId
            });
            return Json(FormulationData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetFormulationById(int Id)
        {
            var formuladata = this._formulationRequestService.GetFormulationRequestById(Id);
            int userId = HttpContext.User.Identity.GetUserId<int>();
            var userRole = this._userService.GetUserRoleById(userId);
            FormulationViewModel formulation = new FormulationViewModel
            {
                Id = formuladata.Id,
                LotNo = formuladata.LotNo,
                GradeName = formuladata.GradeName,
                QtyToProduce = formuladata.QtyToProduce,
                //UOM = formuladata.UOM,
                LineId = formuladata.LineId,
                LOTSize = formuladata.LOTSize,
                ColorSTD = formuladata.ColorSTD,
                Notes = formuladata.Notes,
                WorkOrderNo = formuladata.WorkOrderNo,
                StatusId = formuladata.StatusId,
                QAStatusId = formuladata.QAStatusId,
                ProductId=formuladata.ProductId,
                UserRole= userRole,
                VerNo=formuladata.VerNo
            };
            var formulationDetail = this._formulationRequestService.GetFormulationDetailsById(Id);
            return JsonNet(new { formulation = formulation, formulationDetail = formulationDetail }, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetRequestStatusList()
        {
            var allData = this._formulationRequestService.GetRequestStatusList();
            return JsonNet(allData.Select(s => new { s.Id, s.StatusName }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFormula(int Id)
        {
            var result = new { Success = "true", Message = "Success" };
            try
            {
                int userid = HttpContext.User.Identity.GetUserId<int>();
                int id = this._formulationRequestService.DeleteFormula(Id, userid);
                if (id == 0)
                    result = new { Success = "false", Message = "Some dependency found in Formulation request." };
            }
            catch (Exception ex)
            {
                result = new { Success = "false", Message = "Problem in deleting Formulation request.Please contact Admin." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataForDashboardGrid(int StatusId)
        {
            var allData = _formulationRequestService.GetRequestDataForDashboardGrid(StatusId);
            var FormulationData = allData.Select(s => new
            {
                s.Id,
                s.LOTSize,
                s.GradeName,
                s.LotNo,
                s.ColorSTD,
                s.QtyToProduce,
                s.StatusId,
                s.StatusName,
                RMStatus=s.RMStatus,
                RMStatusId=s.RMStatusId,
                UOM = s.UOMName,
                s.ProductId,
                s.LineId,
                s.VerifyUser,
                s.VerifyBy,
                s.QAStatusId,
                s.QAStatusName,
                s.VerNo
                //s.ParentId
            });
            return Json(FormulationData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataForCloseDashboardGrid(int StatusId, int Month, int Year)
        {
            var allData = _formulationRequestService.GetRequestDataForCloseDashboardGrid(StatusId, Month, Year);
            var FormulationData = allData.Select(s => new
            {
                s.Id,
                s.LOTSize,
                s.GradeName,
                s.LotNo,
                s.ColorSTD,
                s.QtyToProduce,
                s.StatusId,
                s.StatusName,
                s.RMStatus,
                s.RMStatusId,
                UOM = s.UOMName,
                s.ProductId,
                s.LineId,
                s.VerifyUser,
                s.VerifyBy,
                s.QAStatusName
            });
            return Json(FormulationData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRMRequestDataGird(int StatusId)
        {
            var allData = this._formulationRequestService.GetRMRequestData(StatusId);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRMCloseRequestDataGird(int StatusId, int Year, int Month, string LotNo)
        {
            var allData = this._formulationRequestService.GetRMCloseRequestData(StatusId,Year,Month,LotNo);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetRMRequestStatus(int RequestId)
        {
            JsonResponse resonse = null;
            try
            {
                resonse = new JsonResponse();
                var status = this._formulationRequestService.GetRMStatus(RequestId);
                resonse.Data = status;
                resonse.Status = JsonResponseStatus.Success;
            }
            catch(Exception ex)
            {
                resonse.Status = JsonResponseStatus.Error;
                resonse.Message = ProcessException(ex);
            }
            return JsonNet(resonse, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult IsRMReceived(int RequestId)
        {
            JsonResponse resonse = null;
            try
            {
                resonse = new JsonResponse();
                var status = this._formulationRequestService.IsRMReceived(RequestId);
                resonse.Data = status;
                resonse.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                resonse.Status = JsonResponseStatus.Error;
                resonse.Message = ProcessException(ex);
            }
            return JsonNet(resonse, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult IsChangedRMReceived(int RequestId,int VersionNo)
        {
            JsonResponse resonse = null;
            try
            {
                resonse = new JsonResponse();
                var status = this._formulationRequestService.IsChangedMaterialReceived(RequestId, VersionNo);
                resonse.Data = status;
                resonse.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                resonse.Status = JsonResponseStatus.Error;
                resonse.Message = ProcessException(ex);
            }
            return JsonNet(resonse, JsonRequestBehavior.AllowGet);
        }

        //public JsonNetResult CheckParentId(int ParentId)
        //{
        //    JsonResponse resonse = null;
        //    try
        //    {
        //        resonse = new JsonResponse();
        //        //var status = this._formulationRequestService.IsParentId(ParentId);
        //        resonse.Data = status;
        //        resonse.Status = JsonResponseStatus.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        resonse.Status = JsonResponseStatus.Error;
        //        resonse.Message = ProcessException(ex);
        //    }
        //    return JsonNet(resonse, JsonRequestBehavior.AllowGet);
        //}

        public JsonNetResult IsChangeRMExists(int RequestId)
        {
            JsonResponse resonse = null;
            try
            {
                resonse = new JsonResponse();
                var status = this._formulationRequestService.IsRMRequestExist(RequestId);
                resonse.Data = status;
                resonse.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                resonse.Status = JsonResponseStatus.Error;
                resonse.Message = ProcessException(ex);
            }
            return JsonNet(resonse, JsonRequestBehavior.AllowGet);
        }
        
        public JsonNetResult UpdateFormulationNotes(int RequestId, int RequestStatus, bool TestFail, string Notes)
        {
            var result = new JsonResponse();
            try
            {
                var userId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.UpdateFormulationRequestNotes(RequestId, RequestStatus, userId, TestFail, Notes);
                result.Status = JsonResponseStatus.Success;
                result.Message = "Notes saved";
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ProcessException(ex);
            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult UpdateFormulationMachine(int RequestId, Data.Models.FormulationRequestsDetail[] detailsData, string Remarks)
        {
            var result = new JsonResponse();
            try
            {
                var userId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.UpdateFormulationMachineId(RequestId, detailsData, userId, Remarks);
                result.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ProcessException(ex);
            }
            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetFormulationByProductId(int productId, bool IsRequest)
        {
            var allData = this._formulationRequestService.GetFormulationByProductId(productId, IsRequest);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetFormulationRequestById(int Id)
        {
            var formulation = this._formulationRequestService.GetFormulationById(Id);
            if (formulation != null)
            {
                FormulationCreateViewModel model = new FormulationCreateViewModel
                {
                    Id = formulation.Id,
                    CategoryId = formulation.ProductMaster.CategoryId,
                    ProductId = formulation.ProductId,
                    QtyToProduce = formulation.QtyToProduce.ToString(),
                    LotNo = formulation.LotNo,
                    ColorSTD = formulation.ColorSTD,
                    LOTSize = formulation.LOTSize,
                    Notes = formulation.Notes,
                    WorkOrderNo = formulation.WorkOrderNo,
                    LineId = formulation.LineId
                };
                return JsonNet(model, JsonRequestBehavior.AllowGet);
            }
            else
                return JsonNet(null, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult IsLotNoExists(string LotNo)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                response.Data = this._formulationRequestService.IsLotNoExists(LotNo);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Lotno alerdy exists";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult UpdateBaseValue(Data.Models.FormulationMaster[] masterData)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._formulationRequestService.UpdateBaseValue(masterData);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Formulation Base value updated";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveRMRequest(int requestId, int statusId, string remarks, Data.Models.RMRequestDetail[] rmDetailsData)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.SaveRMRequest(requestId, userId, statusId, remarks, rmDetailsData);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Raw material request geneated";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetRMRequestDetailsById(int Id, bool IsRecevied)
        {
            var allData = this._formulationRequestService.GetRMRequestDetailById(Id, IsRecevied);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetRawMaterialsRequestById(int Id)
        {
            var allData = this._formulationRequestService.GetRawMaterialsById(Id);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult UpdateRMRequest(Data.Models.RMRequestDetail[] rmDetailsData, int formulationId,int rawMaterialId, string remarks)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.UpdateRMRequestDetails(rmDetailsData, formulationId, rawMaterialId, remarks, userId);
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult UpdateRMStatus(int formulationId,int rawMaterialId, string remarks)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.UpdateRMRequestStatus(formulationId, rawMaterialId, remarks, userId);
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetFormulationDetailsListById(int RequestId, bool IsParent)
        {
            var allData = this._formulationRequestService.GetFormulationDetailsListById(RequestId, IsParent);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveFormulationClose(Data.Models.FormulationRequestClose formulationClose,int formulationId, string closeRemarks, Data.Models.RMRequestDetail[] fomulationCloseData, Data.Models.MachineReading[] machineReadingData)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                //formulationClose.VerifiedDate = DateTime.Now;
                //formulationClose.VerifiedBy = userId;
                formulationClose.CreateDate = DateTime.Now;
                formulationClose.CreateBy = userId;
                this._formulationRequestService.SaveFormulationRequestClose(formulationClose, formulationId, closeRemarks, fomulationCloseData, machineReadingData);
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Message = ProcessException(ex);
                response.Status = JsonResponseStatus.Error;
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetRawMaterialListForCloseRequest(int formulationId)
        {
            var allData = this._formulationRequestService.GetMaterialListForCloseRequest(formulationId);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetMachineReadingDataById(int LineId)
        {
            var allData = this._formulationRequestService.GetMachineReadingData(LineId);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult VerifyFomulationById(int formulationId, string Notes)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                var userName = this._formulationRequestService.VerifyFormulationById(formulationId, Notes, userId);
                response.Status = JsonResponseStatus.Success;
                response.Data = userName;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult ClearVerifyFomulationById(int formulationId)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._formulationRequestService.ClearVerifyFormulationById(formulationId);
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetFormulationRemarksById(int FormulationId)
        {
            if (FormulationId != 0)
            {
                var allData = this._formulationRequestService.GetFormulationRemarks(FormulationId);
                var rmData = this._formulationRequestService.GetRawMaterialsRemarks(FormulationId);
                var userData = this._userService.GetAllUserForRemarks();
                FormulationRemarksViewModel data = new FormulationRemarksViewModel();
                data.AllRemarks = new List<GridData>();
                data.AllRemarks.Add(new GridData { Id = 1, StatusName = "New", Remarks = allData.Notes, RemarksDate = allData.CreateDate, RemarksBy = string.Join(", ", userData.Where(w => w.Id == allData.CreateBy).Select(q => q.UserName)) });
                if (rmData != null)
                {
                    data.AllRemarks.Add(new GridData { Id = 2, StatusName = "RM Requested", Remarks = rmData.RequestRemarks, RemarksDate = (rmData.RequestDate != null) ? rmData.ReceviedDate : null, RemarksBy = string.Join(", ", userData.Where(w => w.Id == rmData.RequestBy).Select(q => q.UserName)) });
                    data.AllRemarks.Add(new GridData { Id = 3, StatusName = "RM Dispatch", Remarks = rmData.DispatchRemarks, RemarksDate = rmData.DispatchDate, RemarksBy = string.Join(", ", userData.Where(w => w.Id == rmData.DispatchBy).Select(q => q.UserName)) });
                    data.AllRemarks.Add(new GridData { Id = 4, StatusName = "RM Received", Remarks = rmData.ReceviedRemarks, RemarksDate = rmData.ReceviedDate, RemarksBy = string.Join(", ", userData.Where(w => w.Id == rmData.ReceviedBy).Select(q => q.UserName)) });
                    data.AllRemarks.Add(new GridData { Id = 5, StatusName = "In Progress", Remarks = allData.ProgressNotes, RemarksDate = allData.ProgressOn, RemarksBy = string.Join(", ", userData.Where(w => w.Id == allData.ProgressBy).Select(q => q.UserName)) });
                    data.AllRemarks.Add(new GridData { Id = 6, StatusName = "Testing Notes", Remarks = allData.ReadyForTestNotes, RemarksDate = allData.ReadyForTestOn, RemarksBy = string.Join(", ", userData.Where(w => w.Id == allData.ReadyForTestBy).Select(q => q.UserName)) });
                    data.AllRemarks.Add(new GridData { Id = 7, StatusName = "Test Pass", Remarks = allData.TestNotes, RemarksDate = allData.TestOn, RemarksBy = string.Join(", ", userData.Where(w => w.Id == allData.TestBy).Select(q => q.UserName)) });
                    data.AllRemarks.Add(new GridData { Id = 8, StatusName = "Close", Remarks = allData.CloseNotes, RemarksDate = allData.CloseOn, RemarksBy = string.Join(", ", userData.Where(w => w.Id == allData.CloseBy).Select(q => q.UserName)) });
                }
                return JsonNet(data, JsonRequestBehavior.AllowGet);
            }
            return JsonNet(null, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveColourQAData(Data.Models.ColourSpecification colourSpecData,Data.Models.QASpecification qaSpecData,string IsColoursQA,string Notes)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int UserId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.SaveColourandQAData(colourSpecData, qaSpecData, IsColoursQA, Notes, UserId);
                var formulationData = this._formulationRequestService.GetFormulationandQAStatus(colourSpecData.FormulationRequestId);
                response.Status = JsonResponseStatus.Success;
                response.Data = new { FormulationRequestId= formulationData.Id, StatusId = formulationData.StatusId, QAStatusId = formulationData.QAStatusId };
            }
            catch(Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveChangedColourQAData(Data.Models.ColourSpecification colourSpecData, Data.Models.QASpecification qaSpecData, string IsColoursQA, string Notes, FormulationRequest formulationData,int[] DeleteFormulationDetailsID)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int UserId = HttpContext.User.Identity.GetUserId<int>();
                this._formulationRequestService.SaveChangedColourandQAData(colourSpecData, qaSpecData, IsColoursQA, Notes, UserId, formulationData, DeleteFormulationDetailsID);
                //var formulationData = this._formulationRequestService.GetFormulationandQAStatus(colourSpecData.FormulationRequestId);
                response.Status = JsonResponseStatus.Success;
                //response.Data = new { FormulationRequestId = formulationData.Id, StatusId = formulationData.StatusId, QAStatusId = formulationData.QAStatusId };
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColourQaSpecByFormulationId(int FormulationId)
        {
            var allData = this._formulationRequestService.GetColorQASpecfication(FormulationId);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetColorQAVerNo(int FormulationId)
        {
            var count = this._formulationRequestService.GetColourQASpecVerNo(FormulationId);
            return JsonNet(count,JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetAllLotNo()
        {
            var allData=this._formulationRequestService.GetAllLotNo();
            return JsonNet(allData.Select(s => new { BatchId = s.Id, s.LotNo , s.ProductId , s.GradeName  }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetBatchByLineId(int LineId)
        {
            var allData = this._formulationRequestService.GetLotNoByLineId(LineId);
            return JsonNet(allData.Select(s => new { BatchId = s.Id, s.LotNo, s.ProductId, s.GradeName }), JsonRequestBehavior.AllowGet);
        }

        #region DailyPackingDetails
        public ActionResult DailyPackingDetails()
        {
            return View();
        }

        public JsonNetResult GetDailyPackingGridData(DateTime currentDate)
        {
            int id = 0;
            //DateTime currentDate = DateTime.Now;
            var allData = this._formulationRequestService.GetDailyPackingReportData(currentDate);
            for (int i = allData.Count; i < 30; i++)
            {
                id = -1;
                allData.Add(new Services.Models.DailyPackingGridModel { Id = id });
            }
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveDailyPackingData(Data.Models.DailyPackingDetail[] dailyPackingData,DateTime dailypackingDate)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                foreach (var item in dailyPackingData)
                {
                    item.PackingDate = dailypackingDate;
                    item.CreateDate = DateTime.Now;
                    item.CreateBy = userId;
                }
                this._formulationRequestService.SaveDailyPackingData(dailyPackingData, userId);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Daily packing data saved!";
            }
            catch(Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult DeleteDailyPackingData(int[] DeletedId)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._formulationRequestService.DeleteDailyPackingData(DeletedId);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Record deleted successfully!";
            }
            catch(Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Process LogSheet

        public  ActionResult ProcessLogSheet()
        {
            return View();
        }

        public JsonNetResult GetProcessLogSheet1GridData(int LineId, int BatchId, DateTime currentDate)
        {
            int id = 0;
            var logSheet1Data = this._formulationRequestService.GetProcessLogSheet1Data(LineId, BatchId, currentDate);
            for (int i = logSheet1Data.Count; i < 8; i++)
            {
                id = -1;
                logSheet1Data.Add(new Services.Models.ProcessLogSheet1GridModel { Id = id });
            }
            return JsonNet( logSheet1Data, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetProcessLogSheet2GridData(int LineId, int BatchId, DateTime currentDate)
        {
            int id = 0;
            var logSheet2Data = this._formulationRequestService.GetProcessLogSheet2Data(LineId, BatchId, currentDate);
            for (int i = logSheet2Data.Count; i < 8; i++)
            {
                id = -1;
                logSheet2Data.Add(new Services.Models.ProcessLogSheet2GridModel { Id = id });
            }
            return JsonNet( logSheet2Data , JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult SaveProcessLogSheet1GridData(ProcessLogSheet1[] processLog1Data)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                foreach (var item in processLog1Data)
                {
                    item.CreateDate = DateTime.Now;
                    item.CreateBy = userId;
                }
                this._formulationRequestService.SaveProcessLogSheet1Data(processLog1Data,userId);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Report data saved";
            }
            catch(Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message=ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }



        public JsonNetResult DeleteProcessLogSheet1GridData(int[] DeletedId)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._formulationRequestService.DeleteProcessLogSheet1Data(DeletedId);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Record deleted successfully!";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveProcessLogSheet2GridData(ProcessLogSheet2[] processLogSheet2Data)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                int userId = HttpContext.User.Identity.GetUserId<int>();
                foreach (var Sheet2item in processLogSheet2Data)
                {
                    Sheet2item.CreateDate = DateTime.Now;
                    Sheet2item.CreateBy = userId;
                }
                this._formulationRequestService.SaveProcessLogSheet2Data(processLogSheet2Data, userId);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Report data saved";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult DeleteProcessLogSheet2GridData(int[] DeletedId)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._formulationRequestService.DeleteProcessLogSheet2Data(DeletedId);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Record deleted successfully!";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Nested Grid
        [Authorize(Roles = "Admin,QA,QAManager,Lavel1,Lavel2,Lavel3")]
        public ActionResult FormulationRequestHistory()
        {
            return View();
        }

        public JsonResult GetFormulationHistoryData(int LineId, DateTime fromDate,DateTime toDate)
        {
            var allData = this._formulationRequestService.GetHistoryGridData(LineId, fromDate, toDate);
            var lotData = allData.Select(s => new {
                s.Id,
                s.LotNo,
                s.QAStatusName,
                s.StatusName,
                s.GradeName,
                s.ColorSTD,
                s.CreateDate,
                s.CreateBy
            });
            return Json(lotData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFormulationNestedGridData(int Id)
        {
            var allData = this._formulationRequestService.GetNestedGridData(Id);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRawmaterialNestedGridData(int FormulationId)
        {
            var allData = this._formulationRequestService.GetRMNestedGridData(FormulationId);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFormulationDetailsHistory(int FormulationId)
        {
            var allData = this._formulationRequestService.GetFormulationDetailsGridData(FormulationId);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRMDetailsHistoryData(int RMRequestId)
        {
            var allData = this._formulationRequestService.GetRMDetailsGridData(RMRequestId);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        #endregion



    }
}