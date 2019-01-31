using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProERP.Services.Models;
using ProERP.Core.Data;
using ProERP.Data.Models;

namespace ProERP.Services.FormulationRequest
{
    public class FormulationRequestService
    {
        private readonly IRepository<Data.Models.FormulationRequest> _formulationRequestRepository;
        private readonly IRepository<Data.Models.FormulationRequestsDetail> _formulationRequestDetailsRepository;
        private readonly IRepository<Data.Models.FormulationRequestsStatu> _statusRepository;
        private readonly IRepository<Data.Models.Machine> _machineRepository;
        private readonly IRepository<Data.Models.RMItem> _ItemRepository;
        private readonly IRepository<Data.Models.FormulationMaster> _formualtionMasterRepository;
        private readonly IRepository<Data.Models.ProductMaster> _productMasterRepository;
        private readonly IRepository<Data.Models.RMRequestMaster> _rmMasterRepository;
        private readonly IRepository<Data.Models.RMRequestDetail> _rmDetailRepository;
        private readonly IRepository<Data.Models.User> _userRepository;
        private readonly IRepository<Data.Models.FormulationRequestClose> _formulationCloseRepository;
        private readonly IRepository<Data.Models.MachineReading> _machineReadingRepository;
        private readonly IRepository<Data.Models.ColourSpecification> _colourSpecRepository;
        private readonly IRepository<Data.Models.QASpecification> _qaSpecRepository;
        private readonly IRepository<Data.Models.WIPStoreItem> _wipStoreItemRepository;
        private readonly IRepository<Data.Models.DailyPackingDetail> _dailyPackingDetailsRepository;
        private readonly IRepository<Data.Models.FormulationChangedHistory> _formulationChangedHistory;
        private readonly IRepository<Data.Models.FormulationRequestsChangeHistory> _formulationRequestsChangeHistory;
        private readonly IRepository<Data.Models.ProcessLogSheet1> _processlogSheet1Repository;
        private readonly IRepository<Data.Models.ProcessLogSheet2> _processlogSheet2Repository;
        private readonly IRepository<Data.Models.Line> _lineRepository;

        public FormulationRequestService(IRepository<Data.Models.FormulationRequest> formulationRequestRepository,
                                         IRepository<Data.Models.FormulationRequestsDetail> formulationRequestDetailsRepository,
                                         IRepository<Data.Models.FormulationRequestsStatu> statusRepository,
                                         IRepository<Data.Models.Machine> machineRepository,
                                         IRepository<Data.Models.RMItem> itemRepository,
                                         IRepository<Data.Models.FormulationMaster> formualtionMasterRepository,
                                         IRepository<Data.Models.ProductMaster> productMasterRepository,
                                         IRepository<Data.Models.RMRequestMaster> rmMasterRepository,
                                         IRepository<Data.Models.RMRequestDetail> rmDetailRepository,
                                         IRepository<Data.Models.User> userRepository,
                                         IRepository<Data.Models.FormulationRequestClose> formulationCloseRepository,
                                         IRepository<Data.Models.MachineReading> machineReadingRepository,
                                         IRepository<Data.Models.ColourSpecification> colourSpecRepository,
                                         IRepository<Data.Models.QASpecification> qaSpecRepository,
                                         IRepository<Data.Models.WIPStoreItem> wipStoreItemRepository,
                                         IRepository<Data.Models.DailyPackingDetail> dailyPackingDetailsRepository,
                                         IRepository<Data.Models.FormulationChangedHistory> formulationChangedHistory,
                                         IRepository<Data.Models.FormulationRequestsChangeHistory> formulationRequestsChangeHistory,
                                         IRepository<Data.Models.ProcessLogSheet1> processlogSheet1Repository,
                                         IRepository<Data.Models.ProcessLogSheet2> processlogSheet2Repository,
                                         IRepository<Data.Models.Line> lineRepository
                                         )
        {
            this._formulationRequestRepository = formulationRequestRepository;
            this._formulationRequestDetailsRepository = formulationRequestDetailsRepository;
            this._statusRepository = statusRepository;
            this._machineRepository = machineRepository;
            this._ItemRepository = itemRepository;
            this._formualtionMasterRepository = formualtionMasterRepository;
            this._productMasterRepository = productMasterRepository;
            this._rmMasterRepository = rmMasterRepository;
            this._rmDetailRepository = rmDetailRepository;
            this._userRepository = userRepository;
            this._formulationCloseRepository = formulationCloseRepository;
            this._machineReadingRepository = machineReadingRepository;
            this._colourSpecRepository = colourSpecRepository;
            this._qaSpecRepository = qaSpecRepository;
            this._wipStoreItemRepository = wipStoreItemRepository;
            this._dailyPackingDetailsRepository = dailyPackingDetailsRepository;
            this._formulationChangedHistory = formulationChangedHistory;
            this._formulationRequestsChangeHistory = formulationRequestsChangeHistory;
            this._processlogSheet1Repository = processlogSheet1Repository;
            this._processlogSheet2Repository = processlogSheet2Repository;
            this._lineRepository = lineRepository;
        }

        public List<Data.Models.FormulationRequestsStatu> GetRequestStatusList()
        {
            return this._statusRepository.Table.OrderBy(o => o.StatusName).ToList();
        }

        public int SaveFormulationData(Data.Models.FormulationRequest formulationData, int[] formulaDetailDeletedIds, int UserId, string Comment)
        {
            try
            {
                int? machineId = 0;
                Data.Models.FormulationRequest lineData = null;
                //if (formulationData.Id > 0)
                //{
                //    lineData = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationData.Id);
                //    if(formulationData.StatusId == 1 && formulationData.VerNo == 1)
                //    {
                //        machineId = null;
                //    }
                //    else
                //    {
                //        machineId = this._machineRepository.Table.FirstOrDefault(w => w.LineId == lineData.LineId).Id;
                //    }
                //}
                if (formulaDetailDeletedIds != null && formulaDetailDeletedIds.Length > 0)
                {
                    var deletedFormulaDetails = this._formulationRequestDetailsRepository.Table.Where(w => formulaDetailDeletedIds.Contains(w.Id));
                    this._formulationRequestDetailsRepository.Delete(deletedFormulaDetails);

                }

                if (formulationData.Id > 0)
                {
                    var exitsingformula = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationData.Id);
                    if (exitsingformula != null)
                    {
                        if (exitsingformula.LotNo != formulationData.LotNo || exitsingformula.ProductId != formulationData.ProductId || exitsingformula.GradeName != formulationData.GradeName || exitsingformula.QtyToProduce != formulationData.QtyToProduce || exitsingformula.LOTSize != formulationData.LOTSize || exitsingformula.ColorSTD != formulationData.ColorSTD || exitsingformula.Notes != formulationData.Notes || exitsingformula.StatusId != formulationData.StatusId || exitsingformula.WorkOrderNo != formulationData.WorkOrderNo)
                        {
                            exitsingformula.LotNo = formulationData.LotNo;
                            exitsingformula.GradeName = formulationData.GradeName;
                            exitsingformula.QtyToProduce = formulationData.QtyToProduce;
                            exitsingformula.LineId = formulationData.LineId;
                            exitsingformula.LOTSize = formulationData.LOTSize;
                            exitsingformula.ColorSTD = formulationData.ColorSTD;
                            exitsingformula.Notes = formulationData.Notes;
                            exitsingformula.WorkOrderNo = formulationData.WorkOrderNo;
                            exitsingformula.StatusId = formulationData.StatusId;
                            exitsingformula.UpdateDate = DateTime.Now;
                            exitsingformula.UpdateBy = formulationData.UpdateBy;
                            this._formulationRequestRepository.Update(exitsingformula);
                        }
                    }
                    foreach (var item in formulationData.FormulationRequestsDetails)
                    {
                        var exitsingDetails = this._formulationRequestDetailsRepository.Table.FirstOrDefault(w => w.Id == item.Id);
                        if (exitsingDetails != null)
                        {
                            if (exitsingDetails.MachineId != item.MachineId || exitsingDetails.ItemId != item.ItemId || exitsingDetails.ItemQtyGram != item.ItemQtyGram || exitsingDetails.ItemQtyPercentage != item.ItemQtyPercentage || exitsingDetails.UOM != item.UOM)
                            {
                                exitsingDetails.MachineId = item.MachineId;
                                exitsingDetails.ItemId = item.ItemId;
                                exitsingDetails.ItemQtyGram = item.ItemQtyGram;
                                exitsingDetails.ItemQtyPercentage = item.ItemQtyPercentage;
                                exitsingDetails.UOM = item.UOM;
                                this._formulationRequestDetailsRepository.Update(exitsingDetails);
                            }
                        }
                        else
                        {
                            var formulationdetails = new FormulationRequestsDetail
                            {
                                FormulationRequestId = formulationData.Id,
                                MachineId = null,
                                ItemId = item.ItemId,
                                ItemQtyGram = item.ItemQtyGram,
                                ItemQtyPercentage = item.ItemQtyPercentage,
                                VerNo = formulationData.VerNo
                            };
                            this._formulationRequestDetailsRepository.Insert(formulationdetails);
                        }
                        //if (item.Id < 0)
                        //{

                        //}
                    }
                    var formulationChange = new FormulationRequestsChangeHistory
                    {
                        FormulationRequestId = formulationData.Id,
                        RequestStatus = formulationData.StatusId,
                        Comment = Comment,
                        UpdateOn = DateTime.Now,
                        UpdateBy = UserId
                    };
                    this._formulationRequestsChangeHistory.Insert(formulationChange);
                }
                else
                {
                    var data = formulationData.FormulationRequestsDetails;
                    var formulation = new Data.Models.FormulationRequest
                    {
                        LotNo = formulationData.LotNo,
                        GradeName = formulationData.GradeName,
                        QtyToProduce = formulationData.QtyToProduce,
                        UOM = formulationData.UOM,
                        LOTSize = formulationData.LOTSize,
                        ColorSTD = formulationData.ColorSTD,
                        Notes = formulationData.Notes,
                        LineId = formulationData.LineId,
                        StatusId = 1,
                        ProductId = formulationData.ProductId,
                        WorkOrderNo = formulationData.WorkOrderNo,
                        //ParentId=formulationData.ParentId,
                        CreateBy = formulationData.CreateBy,
                        CreateDate = DateTime.Now,
                        VerNo = 1,
                        FormulationRequestsDetails = new List<FormulationRequestsDetail>()
                    };
                    foreach (var item in data)
                    {
                        item.MachineId = null;
                        item.VerNo = 1;
                        formulation.FormulationRequestsDetails.Add(item);
                    }
                    this._formulationRequestRepository.Insert(formulation);
                    //if (formulationData.ParentId > 0)
                    //{
                    //    var existingData = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == formulationData.ParentId);
                    //    existingData.StatusId = 8;
                    //    this._formulationRequestRepository.Update(existingData);
                    //}
                    return formulation.Id;
                }
            }
            catch
            {
                throw;
            }
            return 0;
        }

        public void SaveChangedFormulation(Data.Models.FormulationRequest formulationData, int[] formulaDetailDeletedIds, int userId)
        {
            try
            {
                if (formulaDetailDeletedIds != null && formulaDetailDeletedIds.Length > 0)
                {
                    var deletedFormulaDetails = this._formulationRequestDetailsRepository.Table.Where(w => formulaDetailDeletedIds.Contains(w.Id));
                    this._formulationRequestDetailsRepository.Delete(deletedFormulaDetails);
                }

                if (formulationData.Id > 0)
                {
                    var data = formulationData.FormulationRequestsDetails;
                    var exitingData = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationData.Id);
                    if (exitingData != null)
                    {
                        foreach (var item in data)
                        {
                            var detailsData = this._formulationRequestDetailsRepository.Table.FirstOrDefault(W => W.FormulationRequestId == item.FormulationRequestId && W.ItemId == item.ItemId && W.VerNo == 1);
                            if (detailsData != null)
                            {
                                item.MachineId = detailsData.MachineId;
                                item.VerNo = exitingData.VerNo + 1;
                                this._formulationRequestDetailsRepository.Insert(item);
                            }
                        }
                        exitingData.VerNo = exitingData.VerNo + 1;
                        exitingData.StatusId = 8;
                        exitingData.UpdateBy = formulationData.UpdateBy;
                        exitingData.UpdateDate = DateTime.Now;
                        this._formulationRequestRepository.Update(exitingData);

                        var formulationHistory = new FormulationChangedHistory
                        {
                            FormulationRequestId = exitingData.Id,
                            VerNo = exitingData.VerNo + 1,
                            CreateOn = DateTime.Now,
                            CreateBy = userId
                        };
                        this._formulationChangedHistory.Insert(formulationHistory);
                    }
                }
                else
                {

                }
            }
            catch
            {
                throw;
            }
        }

        public int UpdateChangedFormulationRM(int FormulationId, int userId)
        {
            var rmData = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == FormulationId);
            var rmDetailsData = this._formulationRequestDetailsRepository.Table.Where(w => w.FormulationRequestId == FormulationId);
            var rmRequest = new RMRequestMaster
            {
                FormulationRequestId = FormulationId,
                LotNo = rmData.LotNo,
                ProductId = rmData.ProductId,
                RequestBy = userId,
                RequestStatus = 3,
                RequestDate = DateTime.Now,
                RequestRemarks = "",
                RMRequestDetails = new List<RMRequestDetail>()

            };
            foreach (var item in rmDetailsData)
            {
                var rmDetail = new RMRequestDetail
                {
                    FormulationRequestId = item.FormulationRequestId,
                    ItemId = item.ItemId,
                    RequestedQty = item.ItemQtyGram,
                    IssuedQty = item.ItemQtyGram
                };
                rmRequest.RMRequestDetails.Add(rmDetail);
            }
            this._rmMasterRepository.Insert(rmRequest);

            //var existingformulation = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == FormulationId);
            //existingformulation.RMRequestStatus = 3;
            //this._formulationRequestRepository.Update(existingformulation);
            return FormulationId;
        }

        public Data.Models.FormulationRequest GetFormulationRequestById(int Id)
        {
            return this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == Id);
        }

        public List<FormulationDetailViewModel> GetFormulationDetailsById(int RequestId)
        {
            var allData = from a in this._formulationRequestDetailsRepository.Table
                          where a.FormulationRequestId == RequestId && a.VerNo == a.FormulationRequest.VerNo
                          select new FormulationDetailViewModel
                          {
                              Id = a.Id,
                              FormulationRequestId = a.FormulationRequestId,
                              MachineId = a.MachineId,
                              //MachineName = a.Machine.Name,
                              ItemId = a.ItemId,
                              ItemName = a.RMItem.Name,
                              ItemQtyGram = a.ItemQtyGram,
                              ItemQtyPercentage = a.ItemQtyPercentage,
                              //UOM = a.UOM,
                              //DetailUOM = a.UnitOfMeasureMaster.Measurement
                          };
            return allData.ToList();
        }

        public List<Data.Models.FormulationRequest> GetFormulationRequestData(string LotNo, int StatusId)
        {
            var allData = from a in _formulationRequestRepository.Table.Where(W => W.IsDeleted == false)
                          select a;
            if (!string.IsNullOrEmpty(LotNo))
            {
                allData = allData.Where(W => W.LotNo.Contains(LotNo));
            }
            if (StatusId > 0)
            {
                allData = allData.Where(W => W.StatusId == StatusId);
            }
            allData = allData.OrderByDescending(o => o.CreateDate);
            return allData.ToList();
        }

        public int DeleteFormula(int Id, int userid)
        {
            Data.Models.FormulationRequestsDetail[] detailData = this._formulationRequestDetailsRepository.Table.Where(w => w.FormulationRequestId == Id).ToArray();
            this._formulationRequestDetailsRepository.Delete(detailData);
            Data.Models.FormulationRequest objformula = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == Id);
            if (objformula != null)
            {
                objformula.IsDeleted = true;
                objformula.DeletedOn = DateTime.Now;
                objformula.DeletedBy = userid;
                _formulationRequestRepository.Update(objformula);
                return 1;
            }
            return 0;
        }

        public List<FormulationDashboardViewModel> GetRequestDataForDashboardGrid(int StatusId)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join b in this._userRepository.Table on a.VerifyBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          where a.IsDeleted == false
                          orderby a.CreateDate descending
                          select new { formulationData = a, user = b };
            if (StatusId > 0)
            {
                if (StatusId == 1)
                {
                    allData = allData.Where(W => W.formulationData.StatusId == 1);
                }
                else if (StatusId == 2)
                {
                    allData = allData.Where(W => W.formulationData.StatusId == 2 &&( W.formulationData.QAStatusId == null || W.formulationData.QAStatusId == 3));
                }
                else if (StatusId == 3)
                {
                    allData = allData.Where(W => W.formulationData.StatusId == 3);
                }
                else if (StatusId == 4)
                {
                    allData = allData.Where(W => W.formulationData.StatusId == 4 || W.formulationData.StatusId == 5 || W.formulationData.StatusId == 8 || (W.formulationData.StatusId == 2 && W.formulationData.QAStatusId == 2));
                }
                else if (StatusId == 6)
                {
                    allData = allData.Where(W => W.formulationData.StatusId == 6);
                }
                else if (StatusId == 7)
                {
                    allData = allData.Where(W => W.formulationData.StatusId == 7 || W.formulationData.StatusId == 8);
                }
            }
            var finalData = from a in allData
                            select new FormulationDashboardViewModel
                            {
                                Id = a.formulationData.Id,
                                LotNo = a.formulationData.LotNo,
                                GradeName = a.formulationData.GradeName,
                                QtyToProduce = a.formulationData.QtyToProduce,
                                LOTSize = a.formulationData.LOTSize,
                                ColorSTD = a.formulationData.ColorSTD,
                                Notes = a.formulationData.LotNo,
                                StatusId = a.formulationData.StatusId,
                                StatusName = a.formulationData.FormulationRequestsStatu.StatusName,
                                ProductId = a.formulationData.ProductId,
                                LineId = a.formulationData.LineId,
                                VerifyBy = a.formulationData.VerifyBy,
                                //RMStatusId=(a.formulationData!=null)? a.formulationData.RMRequestStatus : 0,
                                VerifyUser = (a.user.UserName != null ) ? a.user.UserName + "/" + a.formulationData.VerifyOn : "N A",
                                QAStatusId = (a.formulationData.QAStatusId != null) ? a.formulationData.QAStatusId : 0,
                                QAStatusName = (a.formulationData.QAStatu.Name != null) ? a.formulationData.QAStatu.Name : "N A",
                                VerNo=a.formulationData.VerNo
                                //ParentId=(a.formulationData.ParentId !=null) ? a.formulationData.ParentId : 0
                            };
            return finalData.ToList();
        }

        public List<FormulationDashboardViewModel> GetRequestDataForCloseDashboardGrid(int StatusId, int Month, int Year)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join b in this._userRepository.Table on a.VerifyBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          where a.IsDeleted == false && a.StatusId == StatusId
                          && a.CloseOn.Value.Month == (Month == 0 ? a.CloseOn.Value.Month : Month)
                          && a.CloseOn.Value.Year == (Year == 0 ? a.CloseOn.Value.Year : Year)
                          orderby a.CreateDate descending
                          select new { formulationData = a, user = b };
            var finalData = from a in allData
                            select new FormulationDashboardViewModel
                            {
                                Id = a.formulationData.Id,
                                LotNo = a.formulationData.LotNo,
                                GradeName = a.formulationData.GradeName,
                                QtyToProduce = a.formulationData.QtyToProduce,
                                LOTSize = a.formulationData.LOTSize,
                                ColorSTD = a.formulationData.ColorSTD,
                                Notes = a.formulationData.LotNo,
                                StatusId = a.formulationData.StatusId,
                                StatusName = a.formulationData.FormulationRequestsStatu.StatusName,
                                ProductId = a.formulationData.ProductId,
                                LineId = a.formulationData.LineId,
                                VerifyBy = a.formulationData.VerifyBy,
                                VerifyUser = a.user.UserName + "/" + a.formulationData.VerifyOn,
                                QAStatusId = (a.formulationData.QAStatusId != null) ? a.formulationData.QAStatusId : 0,
                                QAStatusName = (a.formulationData.QAStatu.Name != null) ? a.formulationData.QAStatu.Name : "N A"
                            };
            return finalData.ToList();
        }

        public int UpdateFormulationRequestNotes(int RequestId, int RequestStatus, int userId, bool TestFail, string Notes)
        {
            Data.Models.FormulationRequest oldFormulation = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == RequestId);
            if (oldFormulation != null)
            {
                if (RequestStatus == 2)
                {
                    oldFormulation.StatusId = 3;
                    oldFormulation.ReadyForTestBy = userId;
                    oldFormulation.ReadyForTestOn = DateTime.Now;
                    oldFormulation.ReadyForTestNotes = Notes;
                    oldFormulation.QAStatusId = 1;
                }
                else if (RequestStatus == 6)
                {
                    if (!TestFail)
                    {
                        oldFormulation.StatusId = 7;
                        oldFormulation.TestBy = userId;
                        oldFormulation.TestOn = DateTime.Now;
                        oldFormulation.TestNotes = Notes;
                    }
                    else
                    {
                        oldFormulation.StatusId = 8;
                        oldFormulation.TestBy = userId;
                        oldFormulation.TestOn = DateTime.Now;
                        oldFormulation.TestNotes = Notes;
                    }

                }
                else if (RequestStatus == 8)
                {
                    oldFormulation.StatusId = 3;
                    oldFormulation.ReadyForTestBy = userId;
                    oldFormulation.ReadyForTestOn = DateTime.Now;
                    oldFormulation.ReadyForTestNotes = Notes;
                    oldFormulation.QAStatusId = 1;
                }
                else if (RequestStatus == 9)
                {
                    oldFormulation.StatusId = 9;
                    oldFormulation.CloseBy = userId;
                    oldFormulation.CloseOn = DateTime.Now;
                    oldFormulation.CloseNotes = Notes;
                }
                this._formulationRequestRepository.Update(oldFormulation);
                return 1;
            }
            else
                return 0;
        }

        public List<FormulationMasterViewModel> GetFormulationByProductId(int productId, bool IsRequest)
        {
            if (!IsRequest)
            {
                var allData = from a in this._formualtionMasterRepository.Table
                              where a.ProductId == productId
                              select new FormulationMasterViewModel
                              {
                                  Id = a.Id,
                                  ProductId = a.ProductId,
                                  ProductName = a.ProductMaster.Name,
                                  ItemId = a.ItemId,
                                  ItemName = a.RMItem.Name,
                                  MachineName = "NA",
                                  PreviousBaseValue = a.PreviousBaseValue,
                                  BaseValue = a.BaseValue,
                                  InGrams = a.BaseValue,
                                  StatusId = 0,
                              };
                return allData.ToList();
            }
            else
            {
                var allData = from a in this._formulationRequestDetailsRepository.Table
                              where a.FormulationRequestId == productId
                              select new FormulationMasterViewModel
                              {
                                  Id = a.Id,
                                  ProductId = a.FormulationRequest.ProductId,
                                  ProductName = a.FormulationRequest.ProductMaster.Name,
                                  ItemId = a.ItemId,
                                  ItemName = a.RMItem.Name,
                                  MachineName = "NA",
                                  //PreviousBaseValue = a.ItemQtyPercentage,
                                  BaseValue = a.ItemQtyPercentage,
                                  InGrams = a.ItemQtyGram,
                                  StatusId = a.FormulationRequest.StatusId
                              };
                return allData.ToList();
            }
        }

        public Data.Models.FormulationRequest GetFormulationById(int Id)
        {
            return this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == Id);
        }
        public List<FormulationMasterViewModel> GetFormulationDetailsbyRequestId(int Id)
        {
            var allData = from a in this._formulationRequestDetailsRepository.Table
                          where a.FormulationRequestId == Id
                          select new FormulationMasterViewModel
                          {
                              Id = a.Id,
                              ItemId = a.ItemId,
                              MachineId = a.MachineId,
                              ItemName = a.RMItem.Name,
                              ProductId = a.FormulationRequest.ProductId,
                              ProductName = a.FormulationRequest.ProductMaster.Name,
                              PreviousBaseValue = a.ItemQtyPercentage,
                              BaseValue = a.ItemQtyPercentage,
                              InGrams = a.ItemQtyGram
                          };
            return allData.ToList();
        }

        public bool IsLotNoExists(string LotNo)
        {
            return this._formulationRequestRepository.Table.Any(w => w.LotNo == LotNo);
        }

        public void UpdateBaseValue(Data.Models.FormulationMaster[] masterData)
        {
            for (int i = 0; i < masterData.Length; i++)
            {
                var item = masterData[i];
                if (item.Id > 0)
                {
                    var existingData = this._formualtionMasterRepository.Table.FirstOrDefault(w => w.Id == item.Id);
                    if (existingData != null && existingData.BaseValue != item.BaseValue)
                    {
                        existingData.PreviousBaseValue = existingData.BaseValue;
                        existingData.BaseValue = item.BaseValue;
                        this._formualtionMasterRepository.Update(existingData);
                    }
                }
            }
        }

        public int SaveRMRequest(int requestId, int userId, int statusId, string remarks, Data.Models.RMRequestDetail[] rmDetailsData)
        {
            var requestData = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == requestId);
            //var detailsData = this._formulationRequestDetailsRepository.Table.Where(w => w.FormulationRequestId == requestId).ToArray();
            if (requestData != null)
            {
                var rmRequest = new RMRequestMaster
                {
                    FormulationRequestId = requestData.Id,
                    LotNo = requestData.LotNo,
                    ProductId = requestData.ProductId,
                    RequestBy = userId,
                    RequestStatus = 1,
                    RequestDate = DateTime.Now,
                    RequestRemarks = remarks,
                    RMRequestDetails = new List<RMRequestDetail>()

                };
                for (int i = 0; i < rmDetailsData.Length; i++)
                {
                    var item = rmDetailsData[i];
                    var rmDetail = new RMRequestDetail
                    {
                        FormulationRequestId = item.FormulationRequestId,
                        ItemId = item.ItemId,
                        RequestedQty = item.RequestedQty,
                    };
                    rmRequest.RMRequestDetails.Add(rmDetail);
                }
                this._rmMasterRepository.Insert(rmRequest);

                //var existingData = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == requestData.Id);
                //existingData.RMRequestStatus = 1;
                //this._formulationRequestRepository.Update(existingData);
                return 1;

            }
            else if (statusId == 3)
            {
                var oldRM = this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == requestId);
                if (oldRM != null)
                {
                    oldRM.RequestStatus = 4;
                    this._rmMasterRepository.Update(oldRM);
                }
                requestData.StatusId = 4;
                this._formulationRequestRepository.Update(requestData);
            }
            return 0;
        }

        public int UpdateRMRequestDetails(Data.Models.RMRequestDetail[] rmDetailsData, int formulationId, int rawMaterialId, string remarks, int userId)
        {
            if (rmDetailsData != null)
            {
                for (int i = 0; i < rmDetailsData.Length; i++)
                {
                    var item = rmDetailsData[i];
                    var existingData = this._rmDetailRepository.Table.FirstOrDefault(w => w.Id == item.Id);
                    if (existingData != null)
                    {
                        existingData.IssuedQty = item.IssuedQty;
                    }
                    this._rmDetailRepository.Update(existingData);
                }
                var existingRM = this._rmMasterRepository.Table.FirstOrDefault(w => w.Id == rawMaterialId);
                existingRM.DispatchBy = userId;
                existingRM.DispatchDate = DateTime.Now;
                existingRM.DispatchRemarks = remarks;
                existingRM.RequestStatus = 2;
                this._rmMasterRepository.Update(existingRM);

                //var existingformulation = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationId);
                //existingformulation.RMRequestStatus = 2;
                //existingformulation.UpdateDate = DateTime.Now;
                //existingformulation.UpdateBy = userId;
                //this._formulationRequestRepository.Update(existingformulation);
                return 1;
            }
            return 0;
        }

        public int UpdateRMRequestStatus(int formulationId, int rawMaterialId, string remarks, int userId)
        {
            //var existingformulation = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationId);
            //if (existingformulation != null)
            //{
            //    existingformulation.RMRequestStatus = 3;
            //    this._formulationRequestRepository.Update(existingformulation);
            //}
            var existingRM = this._rmMasterRepository.Table.FirstOrDefault(w => w.Id == rawMaterialId);
            if (existingRM != null)
            {
                existingRM.RequestStatus = 3;
                existingRM.ReceviedBy = userId;
                existingRM.ReceviedRemarks = remarks;
                existingRM.ReceviedDate = DateTime.Now;
                this._rmMasterRepository.Update(existingRM);
                return 1;
            }
            return 0;
        }

        public List<RawMateriallDashboadModel> GetRMRequestData(int StatusId)
        {
            var allData = from a in this._rmMasterRepository.Table
                          join b in this._userRepository.Table on a.RequestBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          orderby a.RequestDate descending
                          where a.RequestStatus == StatusId
                          select new { a, b };
            var finalData = from c in allData
                            select new RawMateriallDashboadModel
                            {
                                Id = c.a.Id,
                                FormulationRequestId = c.a.FormulationRequestId,
                                GradeName = c.a.FormulationRequest.GradeName,
                                LotNo = c.a.LotNo,
                                RequestDate = c.a.RequestDate,
                                RMRequestStatus = c.a.RMRequestStatu.StatusName,
                                QtyToProduce = c.a.FormulationRequest.QtyToProduce,
                                ColorSTD = c.a.FormulationRequest.ColorSTD,
                                LOTSize = c.a.FormulationRequest.LOTSize,
                                RequestBy = c.b.UserName
                            };
            return finalData.ToList();
        }

        public List<RawMateriallDashboadModel> GetRMCloseRequestData(int StatusId,int Year,int Month,string LotNo)
        {
            var allData = from a in this._rmMasterRepository.Table
                          join b in this._userRepository.Table on a.RequestBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          orderby a.RequestDate descending
                          where a.RequestStatus == StatusId 
                          && a.RequestDate.Year == (Year == 0 ? a.RequestDate.Year : Year)
                          && a.RequestDate.Month == (Month == 0 ? a.RequestDate.Month : Month)
                          select new { a, b };
            if (!string.IsNullOrEmpty(LotNo))
            {
                allData = allData.Where(W => W.a.LotNo.Contains(LotNo));
            }
            var finalData = from c in allData
                            select new RawMateriallDashboadModel
                            {
                                Id = c.a.Id,
                                FormulationRequestId = c.a.FormulationRequestId,
                                GradeName = c.a.FormulationRequest.GradeName,
                                LotNo = c.a.LotNo,
                                RequestDate = c.a.RequestDate,
                                RMRequestStatus = c.a.RMRequestStatu.StatusName,
                                QtyToProduce = c.a.FormulationRequest.QtyToProduce,
                                ColorSTD = c.a.FormulationRequest.ColorSTD,
                                LOTSize = c.a.FormulationRequest.LOTSize,
                                RequestBy = c.b.UserName
                            };
            return finalData.ToList();
        }

        public List<RawMaterialDetailViewModel> GetRMRequestDetailById(int Id, bool IsRecevied)
        {
            if (!IsRecevied)
            {
                var allData = from a in this._rmDetailRepository.Table
                              where a.RMRequestId == Id
                              select a;
                var finalData = from a in allData
                                select new RawMaterialDetailViewModel
                                {
                                    Id = a.Id,
                                    ItemId = a.ItemId,
                                    ItemName = a.RMItem.Name,
                                    RequestedQty = a.RequestedQty,
                                    IssuedQty = a.RequestedQty,
                                    FormulationRequestId = a.FormulationRequestId,
                                    RMRequestId = a.RMRequestId
                                };
                return finalData.ToList();
            }
            else
            {
                var allData = from a in this._rmDetailRepository.Table
                              where a.RMRequestId == Id
                              select a;
                var finalData = from a in allData
                                select new RawMaterialDetailViewModel
                                {
                                    Id = a.Id,
                                    ItemId = a.ItemId,
                                    ItemName = a.RMItem.Name,
                                    RequestedQty = a.RequestedQty,
                                    IssuedQty = (a.IssuedQty != null) ? a.IssuedQty : 0,
                                    FormulationRequestId = a.FormulationRequestId,
                                    RMRequestId = a.RMRequestId
                                };
                return finalData.ToList();
            }

        }

        public List<RawMaterialDetailViewModel> GetRawMaterialsById(int Id)
        {
            var allData = from a in this._formulationRequestDetailsRepository.Table
                          where a.FormulationRequestId == Id && a.VerNo == a.FormulationRequest.VerNo
                          select new RawMaterialDetailViewModel
                          {
                              Id = a.Id,
                              ItemId = a.ItemId,
                              ItemName = a.RMItem.Name,
                              FormulationRequestId = a.FormulationRequestId,
                              RequestedQty = a.ItemQtyGram
                          };
            return allData.ToList();
        }

        public List<FormulationDetailsDashboardModel> GetFormulationDetailsListById(int RequestId, bool IsParent)
        {
            var allData = from a in this._rmDetailRepository.Table
                          where a.FormulationRequestId == RequestId
                          select a;
            var finalData = from a in allData
                            group a by new { a.ItemId , a.RMItem.Name } into g
                            select new FormulationDetailsDashboardModel
                            {
                                //FormulationRequestId = a.FormulationRequestId,
                                ItemId = g.Key.ItemId,
                                ItemName = g.Key.Name,
                                RequestedQty = g.Sum(s=>s.RequestedQty),
                                IssuedQty = g.Sum(s=>s.IssuedQty),
                                MachineId = 0,
                                //VerNo=a.FormulationRequest.VerNo
                            };
            return finalData.ToList();
        }

        public void UpdateFormulationMachineId(int RequestId, Data.Models.FormulationRequestsDetail[] detailsData, int userId, string Remarks)
        {
            if (detailsData != null)
            {
                var existingformulation = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == RequestId);
                for (int i = 0; i < detailsData.Length; i++)
                {
                    var item = detailsData[i];
                    var existingData = this._formulationRequestDetailsRepository.Table.FirstOrDefault(W => W.FormulationRequestId == RequestId && W.ItemId == item.ItemId && W.FormulationRequest.VerNo == existingformulation.VerNo);
                    if (existingData != null)
                    {
                        existingData.MachineId = item.MachineId;
                        this._formulationRequestDetailsRepository.Update(existingData);
                    }
                }
                existingformulation.StatusId = 2;
                existingformulation.ProgressNotes = Remarks;
                existingformulation.ProgressBy = userId;
                existingformulation.ProgressOn = DateTime.Now;
                this._formulationRequestRepository.Update(existingformulation);
            }
        }

        public void SaveFormulationRequestClose(Data.Models.FormulationRequestClose formulationClose, int formulationId, string closeRemarks, Data.Models.RMRequestDetail[] fomulationCloseData, Data.Models.MachineReading[] machineReadingData)
        {
            var existingFormulation = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationId);
            if (formulationClose != null && existingFormulation != null)
            {
                formulationClose.LotNo = existingFormulation.LotNo;
                formulationClose.GradeName = existingFormulation.GradeName;
                this._formulationCloseRepository.Insert(formulationClose);


                existingFormulation.StatusId = 6;
                existingFormulation.CloseOn = DateTime.Now;
                existingFormulation.CloseBy = formulationClose.CreateBy;
                existingFormulation.CloseNotes = closeRemarks;
                this._formulationRequestRepository.Update(existingFormulation);


                if (fomulationCloseData != null)
                {
                    for (int i = 0; i < fomulationCloseData.Length; i++)
                    {
                        var item = fomulationCloseData[i];
                        var exisingData = this._rmDetailRepository.Table.FirstOrDefault(w => w.ItemId == item.ItemId && w.FormulationRequestId == formulationId);
                        if (item.ReturnQty > 0)
                        {
                            exisingData.ReturnQty = item.ReturnQty;
                        }
                        else
                        {
                            exisingData.ReturnQty = null;
                        }
                        exisingData.ReturnQty = item.ReturnQty;
                        if (item.WIPId > 0)
                        {
                            exisingData.WIPId = item.WIPId;
                            exisingData.WIPQty = item.WIPQty;

                            var existingStore = this._wipStoreItemRepository.Table.FirstOrDefault(w => w.StoreId == item.WIPId && w.ItemId == item.ItemId);
                            if (existingStore != null)
                            {
                                existingStore.ItemQty = exisingData.WIPQty + item.WIPQty;
                                this._wipStoreItemRepository.Update(existingStore);
                            }
                            else
                            {
                                var wipstoreItem = new WIPStoreItem
                                {
                                    ItemId = exisingData.ItemId,
                                    StoreId = (int)item.WIPId,
                                    ItemQty = item.WIPQty
                                };
                                this._wipStoreItemRepository.Insert(wipstoreItem);
                            }
                        }
                        else
                        {
                            exisingData.WIPId = null;
                            exisingData.WIPQty = null;
                        }
                        this._rmDetailRepository.Update(exisingData);

                    }
                }
                if (machineReadingData != null)
                {
                    for (int j = 0; j < machineReadingData.Length; j++)
                    {
                        var machineItem = machineReadingData[j];
                        var machineData = new MachineReading
                        {
                            FormulationRequestId = machineItem.FormulationRequestId,
                            MachineId = machineItem.MachineId,
                            Reading = machineItem.Reading,
                        };
                        this._machineReadingRepository.Insert(machineData);
                    }
                }
            }
        }

        public List<FormulationDetailsDashboardModel> GetMaterialListForCloseRequest(int formulationId)
        {
            var rmData = this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == formulationId);
            var allData = from a in this._rmDetailRepository.Table
                          where a.FormulationRequestId == formulationId && (a.FormulationRequest.StatusId == 2 || a.FormulationRequest.StatusId == 4 || a.FormulationRequest.StatusId == 5 || a.FormulationRequest.StatusId == 8)
                          group a by new { a.ItemId, a.RMItem.Name } into g
                          select new FormulationDetailsDashboardModel
                          {
                              ItemId = g.Key.ItemId,
                              ItemName = g.Key.Name,
                              IssuedQty = g.Sum(s => s.IssuedQty),
                              RequestedQty = g.Sum(s => s.RequestedQty),
                              ReturnQty = (g.Sum(s => s.ReturnQty) != null) ? (g.Sum(s => s.ReturnQty)) : 0,
                              WIPId = 0,
                              WIPQty = (g.Sum(s => s.WIPQty) != null) ? (g.Sum(s => s.WIPQty)) : 0,
                          };
            return allData.ToList();
        }

        public List<MachineReadingViewModel> GetMachineReadingData(int LineId)
        {
            var allData = from a in this._machineRepository.Table
                          where a.LineId == LineId
                          select new MachineReadingViewModel
                          {
                              MachineId = a.Id,
                              MachineName = a.Name,
                              Reading = 0
                          };
            return allData.ToList();
        }

        public string VerifyFormulationById(int formulationId, string Notes, int userId)
        {
            var userName = "";
            var existingData = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == formulationId);
            if (existingData != null)
            {
                existingData.VerifyOn = DateTime.Now;
                existingData.VerifyBy = userId;
                existingData.VerifyNotes = Notes;
                this._formulationRequestRepository.Update(existingData);
                var data = this._userRepository.Table.FirstOrDefault(w => w.Id == userId);
                userName = data.UserName;
            }
            var exitingCloseData = this._formulationCloseRepository.Table.FirstOrDefault(W => W.LotNo == existingData.LotNo);
            if (exitingCloseData != null)
            {
                exitingCloseData.VerifiedBy = userId;
                exitingCloseData.VerifiedDate = DateTime.Now;
                this._formulationCloseRepository.Update(exitingCloseData);
            }
            return userName;
        }

        public void ClearVerifyFormulationById(int formulationId)
        {
            var existingData = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == formulationId);
            if (existingData != null)
            {
                existingData.VerifyOn = null;
                existingData.VerifyBy = null;
                existingData.VerifyNotes = null;
                this._formulationRequestRepository.Update(existingData);
            }
        }

        public Data.Models.FormulationRequest GetFormulationRemarks(int FormulationId)
        {
            return this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == FormulationId);
        }

        public Data.Models.RMRequestMaster GetRawMaterialsRemarks(int FormulationId)
        {
            return this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == FormulationId);
        }

        public List<Data.Models.FormulationRequest> GetAllLotNo()
        {
            return this._formulationRequestRepository.Table.Where(w => w.StatusId != 8).ToList();
        }

        public List<Data.Models.FormulationRequest> GetLotNoByLineId(int LineId)
        {
            return this._formulationRequestRepository.Table.Where(w => w.LineId == LineId && w.StatusId != 8).ToList();
        }

        public bool GetRMStatus(int RequestId)
        {
            var rmmasterData = this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == RequestId);
            if (rmmasterData != null)
            {
                return false;
            }
            else
                return true;
        }

        public bool IsRMReceived(int RequestId)
        {
            var rmRequestData = this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == RequestId && w.RequestStatus == 3);
            if (rmRequestData != null)
            {
                return true;
            }
            else
                return false;

        }

        public bool IsChangedMaterialReceived(int RequestId,int VersionNo)
        {
            var rmRequestData = this._rmMasterRepository.Table.Where(w =>w.FormulationRequestId == RequestId  && w.RequestStatus == 3).Count();
            var rmRequest = this._rmMasterRepository.Table.Where(w => w.FormulationRequestId == RequestId && w.RequestStatus == 3).Max(s => s.RequestDate);
            var testDate = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == RequestId).TestOn;
            if (rmRequestData >= VersionNo)
            {
                if(rmRequest > testDate)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;

        }

        public bool IsRMRequestExist(int RequestId)
        {
            var rmRequestData = this._rmMasterRepository.Table.Where(w => w.FormulationRequestId == RequestId && w.RequestStatus == 3).Count();
            var rmRequest = this._rmMasterRepository.Table.Where(w => w.FormulationRequestId == RequestId && (w.RequestStatus == 1 || w.RequestStatus == 2 || w.RequestStatus == 3)).Max(s => s.RequestDate);
            var testDate = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == RequestId).TestOn;
            if (rmRequest > testDate)
            {
                return false;
            }
            else
                return true;
        }

        #region Formulation History Grid Data

        public List<FormulationHistoryViewModel> GetHistoryGridData(int LineId, DateTime fromDate, DateTime toDate)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join b in this._userRepository.Table on a.CreateBy equals b.Id
                          where a.LineId == (LineId == 0 ? a.LineId : LineId)
                          && a.CreateDate.Year >= fromDate.Year
                          && a.CreateDate.Month >= fromDate.Month
                          && a.CreateDate.Day >= fromDate.Day
                          && a.CreateDate.Year <= toDate.Year
                          && a.CreateDate.Month <= toDate.Month
                          && a.CreateDate.Day <= toDate.Day
                                && a.IsDeleted == false
                          orderby a.CreateDate descending
                          //group a by a.LotNo  into g
                          select new FormulationHistoryViewModel
                          {
                              Id = a.Id,
                              LotNo = a.LotNo,
                              GradeName = a.GradeName,
                              StatusName = a.FormulationRequestsStatu.StatusName,
                              QAStatusName = (a.QAStatu.Name != null) ? a.QAStatu.Name : "N A",
                              ColorSTD = a.ColorSTD,
                              CreateDate = a.CreateDate,
                              CreateBy = b.UserName
                          };

            return allData.ToList();
        }

        public List<FormulationHistoryViewModel> GetNestedGridData(int Id)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          where a.Id == Id
                          select new FormulationHistoryViewModel
                          {
                              Id = a.Id,
                              LotNo = a.LotNo,
                              LOTSize = a.LOTSize,
                              GradeName = a.GradeName,
                              QtyToProduce = a.QtyToProduce,
                              StatusName = a.FormulationRequestsStatu.StatusName,
                              QAStatusName = (a.QAStatu.Name != null) ? a.QAStatu.Name : "N A",
                              ColorSTD = a.ColorSTD
                          };
            return allData.ToList();
        }

        public List<RawMaterialHistoryViewModel> GetRMNestedGridData(int FormulationId)
        {
            var allData = from a in this._rmMasterRepository.Table
                          join b in this._userRepository.Table on a.RequestBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          join c in this._userRepository.Table on a.DispatchBy equals c.Id into j2
                          from c in j2.DefaultIfEmpty()
                          join d in this._userRepository.Table on a.ReceviedBy equals d.Id into j3
                          from d in j3.DefaultIfEmpty()
                          where a.FormulationRequestId == FormulationId
                          select new RawMaterialHistoryViewModel
                          {
                              Id = a.Id,
                              LotNo = a.LotNo,
                              RMRequestStatus = a.RMRequestStatu.StatusName,
                              RequestDate = a.RequestDate,
                              RequestBy = b.UserName,
                              DispatchDate = a.DispatchDate,
                              DispatchBy = b.UserName,
                              ReceviedBy = d.UserName,
                              ReceviedDate = a.ReceviedDate,
                              FormulationRequestId=a.FormulationRequestId
                          };
            return allData.ToList();
        }

        public List<FormulationDetailsHistoryModel> GetFormulationDetailsGridData(int FormulationId)
        {
            var allData = from a in this._formulationRequestDetailsRepository.Table
                          where a.FormulationRequestId == FormulationId
                          select new FormulationDetailsHistoryModel
                          {
                              Id=a.Id,
                              ItemName=a.RMItem.Name,
                              ItemQtyPercentage=a.ItemQtyPercentage,
                              ItemQtyGram=a.ItemQtyGram,
                              VerNo=a.VerNo
                          };
            return allData.ToList();
        }

        public List<RMDetailsHistoryViewModel> GetRMDetailsGridData(int RMRequestId)
        {
            var allData = from a in this._rmDetailRepository.Table
                          where a.RMRequestId == RMRequestId
                          select new RMDetailsHistoryViewModel
                          {
                              Id=a.Id,
                              ItemName=a.RMItem.Name,
                              RequestedQty=a.RequestedQty,
                              IssuedQty=a.IssuedQty,
                              ReturnQty=a.ReturnQty,
                              WIPQty=a.WIPQty
                          };
            return allData.ToList();
        }

        #endregion

        #region Dashboad Data
        public int GetFormulationCount(int userId, int StatusId)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join b in this._rmMasterRepository.Table on a.Id equals b.FormulationRequestId into j1
                          from b in j1.DefaultIfEmpty()
                          select new { formulation = a, rm = b };
            if (StatusId == 1)
            {
                allData = allData.Where(w => w.formulation.StatusId == 1 && w.formulation.IsDeleted == false);
            }
            else if (StatusId == 2)//Raw Material request
            {
                allData = allData.Where(w => w.rm.RequestStatus == 1 && w.formulation.IsDeleted == false);
            }
            else if (StatusId == 3)//Raw Material request
            {
                allData = allData.Where(w => w.rm.RequestStatus == 2 && w.formulation.IsDeleted == false);
            }
            else if (StatusId == 4)
            {
                allData = allData.Where(w => w.formulation.StatusId == 3 && w.formulation.IsDeleted == false);
            }
            return allData.Count();
        }

        public int SaveColourandQAData(Data.Models.ColourSpecification colourSpecData, Data.Models.QASpecification qaSpecData, string IsColoursQA, string Notes, int UserId)
        {
            if (colourSpecData != null && qaSpecData != null)
            {
                var verNo = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == colourSpecData.FormulationRequestId).VerNo;
                colourSpecData.VerNo = verNo;
                this._colourSpecRepository.Insert(colourSpecData);
                qaSpecData.VerNo = verNo;
                this._qaSpecRepository.Insert(qaSpecData);

                if (IsColoursQA == "Ok")
                {
                    var fomulationData = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == colourSpecData.FormulationRequestId);
                    fomulationData.QAStatusId = 2;
                    fomulationData.StatusId = 2;
                    fomulationData.TestNotes = Notes;
                    fomulationData.TestOn = DateTime.Now;
                    fomulationData.TestBy = UserId;
                    this._formulationRequestRepository.Update(fomulationData);
                }
                else if (IsColoursQA == "Stop Production")
                {
                    var fomulationData = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == colourSpecData.FormulationRequestId);
                    fomulationData.QAStatusId = 3;
                    fomulationData.StatusId = 4;
                    fomulationData.TestNotes = Notes;
                    fomulationData.TestOn = DateTime.Now;
                    fomulationData.TestBy = UserId;
                    this._formulationRequestRepository.Update(fomulationData);
                }
                //else if (IsColoursQA == "Changed Formulation")
                //{
                //    var fomulationData = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == colourSpecData.FormulationRequestId);
                //    fomulationData.QAStatusId = 3;
                //    fomulationData.StatusId = 5;
                //    fomulationData.TestNotes = Notes;
                //    fomulationData.TestOn = DateTime.Now;
                //    fomulationData.TestBy = UserId;
                //    this._formulationRequestRepository.Update(fomulationData);
                //}
                return 1;
            }
            return 0;
        }

        public int SaveChangedColourandQAData(Data.Models.ColourSpecification colourSpecData, Data.Models.QASpecification qaSpecData, string IsColoursQA, string Notes, int UserId, Data.Models.FormulationRequest formulationData, int[] DeleteFormulationDetailsID)
        {
           
            if (colourSpecData != null && qaSpecData != null && formulationData != null)
            {
                var verNo = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == colourSpecData.FormulationRequestId).VerNo;
                colourSpecData.VerNo = verNo;
                this._colourSpecRepository.Insert(colourSpecData);
                qaSpecData.VerNo = verNo;
                this._qaSpecRepository.Insert(qaSpecData);
               // Data.Models.FormulationRequest lineData = null;
                //if (formulationData.Id > 0)
                //{
                //    lineData = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationData.Id);
                //    machineId = this._machineRepository.Table.FirstOrDefault(w => w.LineId == lineData.LineId).Id;
                //}

                if (IsColoursQA == "Changed Formulation")
                {
                    var fomulationData = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == colourSpecData.FormulationRequestId);
                    fomulationData.QAStatusId = 3;
                    fomulationData.StatusId = 8;
                    fomulationData.TestNotes = Notes;
                    fomulationData.TestOn = DateTime.Now;
                    fomulationData.TestBy = UserId;
                    this._formulationRequestRepository.Update(fomulationData);
                }
                if (formulationData.Id > 0)
                {
                    var data = formulationData.FormulationRequestsDetails;
                    var exitingData = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == formulationData.Id);
                    if (exitingData != null)
                    {
                        foreach (var item in data)
                        {
                            var detailsData = this._formulationRequestDetailsRepository.Table.FirstOrDefault(W => W.FormulationRequestId == item.FormulationRequestId && W.ItemId == item.ItemId && W.VerNo == 1);
                            if (detailsData != null)
                            {
                                detailsData.ItemId= item.ItemId;
                                detailsData.ItemQtyGram = item.ItemQtyGram;
                                detailsData.ItemQtyPercentage = item.ItemQtyPercentage;
                                detailsData.MachineId = item.MachineId;
                                detailsData.VerNo = exitingData.VerNo + 1;
                                this._formulationRequestDetailsRepository.Insert(detailsData);
                            }
                            else
                            {
                                if (item.Id <= 0)
                                {
                                    var formulationDetails = new FormulationRequestsDetail
                                    {
                                        FormulationRequestId = formulationData.Id,
                                        ItemId = item.ItemId,
                                        ItemQtyGram = item.ItemQtyGram,
                                        ItemQtyPercentage = item.ItemQtyPercentage,
                                        MachineId = item.MachineId,
                                        VerNo = exitingData.VerNo + 1
                                    };
                                    this._formulationRequestDetailsRepository.Insert(formulationDetails);
                                }
                            }
                        }
                        exitingData.VerNo = exitingData.VerNo + 1;
                        exitingData.StatusId = 8;
                        exitingData.UpdateBy = formulationData.UpdateBy;
                        exitingData.UpdateDate = DateTime.Now;
                        this._formulationRequestRepository.Update(exitingData);

                        var formulationHistory = new FormulationChangedHistory
                        {
                            FormulationRequestId = exitingData.Id,
                            VerNo = exitingData.VerNo,
                            CreateOn = DateTime.Now,
                            CreateBy = UserId
                        };
                        this._formulationChangedHistory.Insert(formulationHistory);
                    }
                }
                else
                {

                }
                // if (DeleteFormulationDetailsID != null)
                //{
                //    for(int i=0;i< DeleteFormulationDetailsID.Length; i++)
                //    {
                //        var item = DeleteFormulationDetailsID[i];
                //        var selectedId = this._formulationRequestDetailsRepository.Table.FirstOrDefault(w => w.Id == item);

                //    }
                //    //var selectedId = this._formulationRequestDetailsRepository.Table.Where(w => DeleteFormulationDetailsID.Contains(w.Id));
                //    //this._formulationRequestDetailsRepository.Delete(selectedId);
                //}

                return 1;
            }
            return 0;
        }


        public TestColorSpecification[] GetColourSpecByFormulationId(int FormulationId)
        {
            var colorData = from a in this._colourSpecRepository.Table
                            where a.FormulationRequestId == FormulationId
                            select new TestColorSpecification
                            {
                                ColourId = a.Id,
                                FormulationRequestId = a.FormulationRequestId,
                                Deltaa = a.Deltaa,
                                Deltab = a.Deltab,
                                DeltaE = a.DeltaE,
                                DeltaL = a.DeltaL,
                                VerNo = a.VerNo
                            };
            return colorData.ToArray();
        }

        public TestQASpecification[] GetQASpecByFormulationId(int FormulationId)
        {
            var qaData = from a in this._qaSpecRepository.Table
                         where a.FormulationRequestId == FormulationId
                         select new TestQASpecification
                         {
                             QAId = a.Id,
                             FormulationRequestId = a.FormulationRequestId,
                             MFI220c10kg = a.MFI220c10kg,
                             SPGravity = a.SPGravity,
                             AshContent = a.AshContent,
                             NotchImpact = a.NotchImpact,
                             Tensile = a.Tensile,
                             FlexuralModule = a.FlexuralModule,
                             FlexuralStrength = a.FlexuralStrength,
                             Elongation = a.Elongation,
                             Flammability = a.Flammability,
                             GWTAt = a.GWTAt,
                             VerNo = a.VerNo
                         };
            return qaData.ToArray();
        }

        public TestResultViewModel[] GetColorQASpecfication(int FormulationId)
        {

            var q = from a in this._colourSpecRepository.Table
                    join b in this._qaSpecRepository.Table on new { Key1 = a.FormulationRequestId, Key2 = a.VerNo } equals new { Key1 = b.FormulationRequestId, Key2 = b.VerNo }
                    where a.FormulationRequestId == FormulationId
                    group new { a, b } by a.VerNo into g
                    select new
                    {
                        VerNo = g.Key,
                        ColorSpecs = g.FirstOrDefault().a,
                        QASpecs = g.FirstOrDefault().b
                    };

            var allData = q.ToArray();

            var finalData = from a in allData
                            select new TestResultViewModel
                            {
                                VersionNo = a.VerNo,
                                colorSpec = new TestColorSpecification {
                                    ColourId = a.ColorSpecs.Id ,Deltaa =a.ColorSpecs.Deltaa,
                                    Deltab=a.ColorSpecs.Deltab, DeltaE=a.ColorSpecs.DeltaE,
                                    DeltaL=a.ColorSpecs.DeltaL,VerNo=a.ColorSpecs.VerNo,
                                    FormulationRequestId=a.ColorSpecs.VerNo
                                },
                                qaSpec = new TestQASpecification {
                                    QAId = a.QASpecs.Id, FormulationRequestId=a.QASpecs.FormulationRequestId,
                                    MFI220c10kg=a.QASpecs.MFI220c10kg,SPGravity=a.QASpecs.SPGravity,
                                    AshContent=a.QASpecs.AshContent,NotchImpact=a.QASpecs.NotchImpact,
                                    Tensile=a.QASpecs.Tensile,FlexuralModule=a.QASpecs.FlexuralModule,
                                    FlexuralStrength=a.QASpecs.FlexuralStrength,Elongation=a.QASpecs.Elongation,
                                    Flammability=a.QASpecs.Flammability,GWTAt=a.QASpecs.GWTAt,
                                    VerNo=a.QASpecs.VerNo
                                }
                            };

            return finalData.ToArray();
        }

        public Data.Models.FormulationRequest GetFormulationandQAStatus(int FormulationRequestId)
        {
            return this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == FormulationRequestId);
        }

        public int GetColourQASpecVerNo(int FormulationId)
        {
            var colorData = this._colourSpecRepository.Table.Where(W => W.FormulationRequestId == FormulationId);
            var qaData = this._colourSpecRepository.Table.Where(W => W.FormulationRequestId == FormulationId);
            int colorcount = colorData.Count();
            int qacount = qaData.Count();
            if (colorcount == qacount)
            {
                return colorcount;
            }
            else
            {
                return 0;
            }


        }

        public int GetFormulationRequestCount(int StatusId)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          select a;
            if (StatusId == 0)
            {
                allData = allData.Where(w => w.IsDeleted == false);
            }
            else
            {
                allData = allData.Where(w => w.IsDeleted == false && w.StatusId !=6);
            }
            return allData.Count();
        }

        public int GetRMRequestCount(int StatusId)
        {
            var allData = from a in this._rmMasterRepository.Table
                          where a.RequestStatus == (StatusId == 0 ? a.RequestStatus : StatusId)
                          select a;
            return allData.Count();
        }

        public double GetQAStatusPercentage(int QAStatus)
        {
            DateTime previousMonth = DateTime.Now.AddMonths(-1);
            int month = previousMonth.Month;

            var allData = from a in this._formulationRequestRepository.Table
                          where a.IsDeleted == false 
                          && a.CreateDate.Year == DateTime.Now.Year 
                          && a.CreateDate.Month == month
                          select a;
            int totalRequest = allData.Count();
            double Percentage;
            if(QAStatus == 2)
            {
                int testPassCount = allData.Where(w => w.QAStatusId == 2).Count();
                Percentage =((double)(testPassCount * 100)/ totalRequest);
            }
            else
            {
                int testFailCount = allData.Where(w => w.QAStatusId == 3).Count();
                Percentage = ((double)(testFailCount * 100) / totalRequest);
            }
            var val = Percentage.ToString("F2");
            double totalPercentage = double.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
            return totalPercentage;
        }



        #endregion

        #region Report

        public List<FormulationRequestReportViewModel> GetFormulationReportDataById(int Id,int? VerNo)
        {
            var allData = from a in this._formulationRequestDetailsRepository.Table
                          where a.FormulationRequestId == Id && a.VerNo == (VerNo == 0 ? a.VerNo : VerNo)
                          orderby a.Machine.Name ascending
                          select new FormulationRequestReportViewModel
                          {
                              MachineId = a.MachineId,
                              MachineName = a.Machine.Name,
                              ItemId = a.ItemId,
                              ItemName = a.RMItem.Name,
                              ItemQtyGram = a.ItemQtyGram,
                              ItemQtyPercentage = a.ItemQtyPercentage,
                          };
            return allData.ToList();
        }

        public List<FormulationRequestReportDataModel> GetFormulationMasterDataById(int Id)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join d in this._userRepository.Table on a.CreateBy equals d.Id into j1
                          from d in j1.DefaultIfEmpty()
                          join e in this._userRepository.Table on a.ProgressBy equals e.Id into j2
                          from e in j2.DefaultIfEmpty()
                          join f in this._userRepository.Table on a.ReadyForTestBy equals f.Id into j3
                          from f in j3.DefaultIfEmpty()
                          join g in this._userRepository.Table on a.TestBy equals g.Id into j4
                          from g in j4.DefaultIfEmpty()
                          join h in this._userRepository.Table on a.CloseBy equals h.Id into j5
                          from h in j5.DefaultIfEmpty()
                          join i in this._userRepository.Table on a.VerifyBy equals i.Id into j6
                          from i in j6.DefaultIfEmpty()
                          join k in this._rmMasterRepository.Table on a.Id equals k.FormulationRequestId into j7
                          from k in j7.DefaultIfEmpty()
                          join l in this._userRepository.Table on k.RequestBy equals l.Id into j8
                          from l in j8.DefaultIfEmpty()
                          join m in this._userRepository.Table on k.DispatchBy equals m.Id into j9
                          from m in j9.DefaultIfEmpty()
                          join n in this._userRepository.Table on k.ReceviedBy equals n.Id into j10
                          from n in j10.DefaultIfEmpty()
                          where a.Id == Id
                          select new FormulationRequestReportDataModel
                          {
                              LotNo = a.LotNo,
                              GradeName = a.GradeName,
                              QtyToProduce = a.QtyToProduce,
                              LOTSize = a.LOTSize,
                              ColorSTD = a.ColorSTD,
                              Notes = a.Notes,
                              CreateDate = a.CreateDate,
                              CreateBy = d.UserName,
                              ProgressBy = e.UserName,
                              ProgressNotes = a.ProgressNotes,
                              ProgressOn = a.ProgressOn,
                              ReadyForTestBy = f.UserName,
                              ReadyForTestNotes = a.ReadyForTestNotes,
                              ReadyForTestOn = a.ReadyForTestOn,
                              TestBy = g.UserName,
                              TestNotes = a.TestNotes,
                              TestOn = a.TestOn,
                              CloseBy = h.UserName,
                              CloseNotes = a.CloseNotes,
                              CloseOn = a.CloseOn,
                              VerifyBy = i.UserName,
                              VerifyOn = a.VerifyOn,
                              RequestBy = l.UserName,
                              RequestDate = k.RequestDate,
                              RequestRemarks = k.RequestRemarks,
                              DispatchBy = m.UserName,
                              DispatchDate = k.DispatchDate,
                              DispatchRemarks = k.DispatchRemarks,
                              ReceviedBy = n.UserName,
                              ReceviedRemarks = k.ReceviedRemarks,
                              ReceviedDate = k.ReceviedDate
                          };
            return allData.ToList();
        }

        public Data.Models.FormulationRequest GetFormulationReportById(int Id)
        {
            return this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == Id);
        }

        public Data.Models.RMRequestMaster GetRMReportData(int Id)
        {
            return this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == Id);
        }

        public List<MachineDetailViewModel> GetMachineForReport(int Id,int? VerNo)
        {
            var allData = from a in this._machineRepository.Table
                          join b in this._formulationRequestDetailsRepository.Table on new { Key1 = a.Id, Key2 = Id } equals new { Key1 = (int)b.MachineId, Key2 = b.FormulationRequestId } into c
                          from b in c.DefaultIfEmpty()
                          where b.FormulationRequestId == Id && b.VerNo == (VerNo == 0 ? b.VerNo : VerNo)
                          orderby a.Name ascending
                          group b by new { b.Machine.Name, b.ItemQtyPercentage, b.ItemQtyGram } into g
                          select new MachineDetailViewModel
                          {
                              //Id = a.Id,
                              MachineName = g.Key.Name,
                              //ItemQtyPercentage = b == null ? 0 : b.ItemQtyPercentage,
                              //ItemQtyGram = b == null ? 0 : b.ItemQtyGram
                              ItemQtyGram = g.Sum(w => w.ItemQtyGram),
                              ItemQtyPercentage = g.Sum(s => s.ItemQtyPercentage)
                          };
            return allData.ToList();
        }

        public int GetFormulationStatusById(int Id)
        {
            return this._formulationRequestRepository.Table.Where(w => w.Id == Id).Select(s => s.StatusId).FirstOrDefault();
        }

        public List<BatchYieldReportViewModel> GetBatchYieldReportById(int Id)
        {
            var rmdetailsData = this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == Id);
            var allData = from a in this._rmDetailRepository.Table
                          where a.FormulationRequestId == Id
                          group a by a.RMItem.Name into g
                          select new BatchYieldReportViewModel
                          {
                              ItemName = g.Key,
                              IssuedQty = g.Sum(s => s.IssuedQty),
                              ReturnQty = g.Sum(s => s.ReturnQty),
                              DispatchQty = g.Sum(s => s.IssuedQty),
                              WIPQty = g.Sum(s => s.WIPQty),
                              //WIPId = g.Key.WIPId,
                          };
            return allData.ToList();
        }

        public Data.Models.FormulationRequest GetFormulationBatchReportDataById(int Id)
        {
            return this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == Id);
        }



        public Data.Models.FormulationRequestClose GetBatchDataById(int Id)
        {
            var formulation = this._formulationRequestRepository.Table.FirstOrDefault(w => w.Id == Id);
            return this._formulationCloseRepository.Table.FirstOrDefault(w => w.LotNo == formulation.LotNo);
        }

        public List<MachineReadingViewModel> GetMachineReadingDataForReport(int Id)
        {

            var allData = from a in this._machineRepository.Table
                          join b in this._machineReadingRepository.Table on new { Key1 = a.Id, Key2 = Id } equals new { Key1 = (int)b.MachineId, Key2 = b.FormulationRequestId } into c
                          from b in c.DefaultIfEmpty()
                          where b.FormulationRequestId == Id
                          orderby a.Name ascending
                          select new MachineReadingViewModel
                          {
                              Id = b.Id,
                              FormulationRequestId = b.FormulationRequestId,
                              MachineId = a.Id,
                              MachineName = a.Name,
                              Reading = b.Reading
                          };
            return allData.ToList();
        }

        public List<FormulationCloseReportModel> GetFormulationCloseReportById(int Id)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join b in this._formulationCloseRepository.Table on a.LotNo equals b.LotNo into j1
                          from b in j1.DefaultIfEmpty()
                          join c in this._userRepository.Table on b.VerifiedBy equals c.Id into j2
                          from c in j2.DefaultIfEmpty()
                          where a.Id == Id
                          select new FormulationCloseReportModel
                          {
                              Id = b.Id,
                              FormulationRequestId = a.Id,
                              LotNo = a.LotNo,
                              GradeName = a.GradeName,
                              FGPackedQty = b.FGPackedQty,
                              NSP = b.NSP,
                              StartUpTrials = b.StartUpTrials,
                              MixMaterial = b.MixMaterial,
                              Lumps = b.Lumps,
                              LongsandFines = b.LongsandFines,
                              LabSample = b.LabSample,
                              VerifiedDate = b.VerifiedDate,
                              VerifiedBy = c.UserName
                          };
            return allData.ToList();
        }

        public List<ColourQASpecsReportViewModel> GetColourQAReportByFormulationId(int Id,int? VerNo)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join b in this._colourSpecRepository.Table on a.Id equals b.FormulationRequestId into j1
                          from b in j1.DefaultIfEmpty()
                          join c in this._qaSpecRepository.Table on a.Id equals c.FormulationRequestId into j2
                          from c in j2.DefaultIfEmpty()
                          join d in this._userRepository.Table on a.TestBy equals d.Id into j3
                          from d in j3.DefaultIfEmpty()
                          where a.Id == Id
                          && b.VerNo ==(VerNo == 0 ? b.VerNo : VerNo)
                          && c.VerNo == (VerNo == 0 ? b.VerNo : VerNo)
                          select new ColourQASpecsReportViewModel
                          {
                              FormulationRequestId = a.Id,
                              DeltaE = b.DeltaE,
                              DeltaL = b.DeltaL,
                              Deltaa = b.Deltaa,
                              Deltab = b.Deltab,
                              MFI220c10kg = c.MFI220c10kg,
                              SPGravity = c.SPGravity,
                              AshContent = c.AshContent,
                              NotchImpact = c.NotchImpact,
                              Tensile = c.Tensile,
                              FlexuralModule = c.FlexuralModule,
                              FlexuralStrength = c.FlexuralStrength,
                              Elongation = c.Elongation,
                              Flammability = c.Flammability,
                              GWTAt = c.GWTAt,
                              LotNo = a.LotNo,
                              GradeName = a.GradeName,
                              TestOn = a.TestOn,
                              TestNotes = a.TestNotes,
                              TestBy = d.UserName,
                              QtyToProduce = a.QtyToProduce,
                              CreateDate = a.CreateDate
                          };
            return allData.ToList();
        }

        public List<MaterialIssueReturnReportViewModel> GetMaterialReportData(int Id)
        {
            var rmDetailsData = this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == Id);
            var allData = from a in this._rmDetailRepository.Table
                          where a.FormulationRequestId == Id
                          group a by a.RMItem.Name into g
                          select new MaterialIssueReturnReportViewModel
                          {
                              ItemName = g.Key,
                              RequestedQty = g.Sum(s => s.RequestedQty),
                              IssuedQty = g.Sum(s => s.IssuedQty),
                              ReturnQty = g.Sum(s => s.ReturnQty),
                              WIPQty = g.Sum(s => s.WIPQty)
                          };
            return allData.ToList();
        }

        public List<MaterialIssueReturnReportViewModel> GetMaterialIssueReportData(int RMRequestId)
        {
            var allData = from a in this._rmDetailRepository.Table
                          where a.RMRequestId == RMRequestId
                          select new MaterialIssueReturnReportViewModel
                          {
                              ItemName=a.RMItem.Name,
                              RequestedQty=a.RequestedQty,
                              IssuedQty = a.IssuedQty,
                              ReturnQty =a.ReturnQty,
                              //OldMaterialReturn=
                              //Remarks
                          };
            return allData.ToList();
        }

        public Data.Models.RMRequestMaster GetRMDataForReport(int Id)
        {
            var rmData = this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == Id);
            return this._rmMasterRepository.Table.FirstOrDefault(w => w.FormulationRequestId == Id);

        }

        public Data.Models.Machine GetMachineForMaterialReport(int Id)
        {
            var detailsData = this._formulationRequestDetailsRepository.Table.FirstOrDefault(w => w.FormulationRequestId == Id);
            return this._machineRepository.Table.FirstOrDefault(w => w.Id == detailsData.MachineId);
        }

        public string GetPlantNameForMaterialSlip(int RMRequestId)
        {
            int formulationId = this._rmMasterRepository.Table.FirstOrDefault(w => w.Id == RMRequestId).FormulationRequestId;
            var plantName = this._formulationRequestRepository.Table.FirstOrDefault(W=>W.Id == formulationId).Line.Plant.Name;
            if (plantName != null)
            {
                return plantName;
            }
            else
                return null;
        }

        public string GetPlantNameForMaterialIssueReturn(int Id)
        {
            var plantName = this._formulationRequestRepository.Table.FirstOrDefault(W => W.Id == Id).Line.Plant.Name;
            if (plantName != null)
            {
                return plantName;
            }
            else
                return null;
        }

        public Data.Models.RMRequestMaster GetRMDataForMaterialSlip(int RMRequestId)
        {
            return this._rmMasterRepository.Table.FirstOrDefault(w => w.Id == RMRequestId);
        }

        public List<DailyPackingGridModel> GetDailyPackingReportData(DateTime CurrentDate)
        {
            var allData = from a in this._dailyPackingDetailsRepository.Table
                          where a.PackingDate.Year == CurrentDate.Year
                          && a.PackingDate.Month == CurrentDate.Month
                          && a.PackingDate.Day == CurrentDate.Day
                          select new DailyPackingGridModel
                          {
                              Id = a.Id,
                              BatchId = a.BatchId,
                              LotNo = a.FormulationRequest.LotNo,
                              GradeId = a.GradeId,
                              GradeName = a.FormulationRequest.GradeName,
                              PackingDate = a.PackingDate,
                              BagFrom = a.BagFrom,
                              BagTo = a.BagTo,
                              TotalBags = a.TotalBags,
                              Quantity = a.Quantity,
                              ProductionRemarks = a.ProductionRemarks
                          };
            return allData.ToList();
        }

        public void SaveDailyPackingData(Data.Models.DailyPackingDetail[] dailyPackingData, int userId)
        {
            if (dailyPackingData != null)
            {

                for (int i = 0; i < dailyPackingData.Length; i++)
                {
                    var item = dailyPackingData[i];
                    if (item.Id > 0)
                    {
                        var existingData = this._dailyPackingDetailsRepository.Table.FirstOrDefault(w => w.Id == item.Id);
                        if (existingData.BatchId != item.BatchId || existingData.BatchId != item.BatchId ||
                           existingData.BagFrom != item.BagFrom || existingData.BagTo != item.BagTo ||
                           existingData.Quantity != item.Quantity || existingData.TotalBags != item.TotalBags ||
                           existingData.ProductionRemarks != item.ProductionRemarks)
                        {
                            existingData.BatchId = item.BatchId;
                            existingData.GradeId = item.GradeId;
                            existingData.BagFrom = item.BagFrom;
                            existingData.BagTo = item.BagTo;
                            existingData.Quantity = item.Quantity;
                            existingData.TotalBags = item.TotalBags;
                            existingData.ProductionRemarks = item.ProductionRemarks;
                            existingData.UpdateDate = DateTime.Now;
                            existingData.UpdateBy = userId;
                            this._dailyPackingDetailsRepository.Update(existingData);
                        }

                    }
                    else
                    {
                        this._dailyPackingDetailsRepository.Insert(item);
                    }
                }
            }
        }

        public void DeleteDailyPackingData(int[] DeletedIds)
        {
            if (DeletedIds != null || DeletedIds.Length > 0)
            {
                var selectedId = this._dailyPackingDetailsRepository.Table.Where(w => DeletedIds.Contains(w.Id));
                this._dailyPackingDetailsRepository.Delete(selectedId);
            }
        }

        public List<ProcessLogSheet1GridModel> GetProcessLogSheet1Data(int LineId, int BatchId, DateTime currentDate)
        {
            var allData = from a in this._processlogSheet1Repository.Table
                          where a.LineId == LineId
                          && a.Date.Year == currentDate.Year
                          && a.Date.Month == currentDate.Month
                          && a.Date.Day == currentDate.Day
                          && a.BatchId == BatchId
                          select new ProcessLogSheet1GridModel
                          {
                              Id = a.Id,
                              LineId = a.LineId,
                              BatchId = a.BatchId,
                              GradeId = a.GradeId,
                              Date = a.Date,
                              Time = a.Time,
                              TZ1 = a.TZ1,
                              TZ2 = a.TZ2,
                              TZ3 = a.TZ3,
                              TZ4 = a.TZ4,
                              TZ5 = a.TZ5,
                              TZ6 = a.TZ6,
                              TZ7 = a.TZ7,
                              TZ8 = a.TZ8,
                              TZ9 = a.TZ9,
                              TZ10 = a.TZ10,
                              TZ11 = a.TZ11,
                              TZ12Die = a.TZ12Die,
                              TM1 = a.TM1,
                              PM1 = a.PM1,
                              PM11 = a.PM11,
                              Vaccumembar = a.Vaccumembar
                          };
            return allData.ToList();
        }

        public void SaveProcessLogSheet1Data(Data.Models.ProcessLogSheet1[] processLog1Data, int userId)
        {
            if (processLog1Data != null)
            {
                for (int i = 0; i < processLog1Data.Length; i++)
                {
                    var item = processLog1Data[i];
                    if (item.Id > 0)
                    {
                        var existingData = this._processlogSheet1Repository.Table.FirstOrDefault(w => w.Id == item.Id);
                        if (existingData.TZ1 != item.TZ1 || existingData.TZ2 != item.TZ2 || existingData.TZ3 != item.TZ3 ||
                           existingData.TZ4 != item.TZ4 || existingData.TZ5 != item.TZ5 || existingData.TZ6 != item.TZ6 ||
                           existingData.TZ7 != item.TZ7 || existingData.TZ8 != item.TZ8 || existingData.TZ9 != item.TZ9 ||
                           existingData.TZ10 != item.TZ10 || existingData.TZ11 != item.TZ11 || existingData.TM1 != item.TM1 || existingData.TZ11 != item.TZ11 ||
                           existingData.TZ12Die != item.TZ12Die || existingData.PM1 != item.PM1 || existingData.PM11 != existingData.PM11 ||
                           existingData.Vaccumembar != existingData.Vaccumembar)
                        {
                            existingData.TZ1 = item.TZ1;
                            existingData.TZ2 = item.TZ2;
                            existingData.TZ3 = item.TZ3;
                            existingData.TZ4 = item.TZ4;
                            existingData.TZ5 = item.TZ5;
                            existingData.TZ6 = item.TZ6;
                            existingData.TZ7 = item.TZ7;
                            existingData.TZ8 = item.TZ8;
                            existingData.TZ9 = item.TZ9;
                            existingData.TZ10 = item.TZ10;
                            existingData.TZ11 = item.TZ11;
                            existingData.TZ12Die = item.TZ12Die;
                            existingData.TM1 = item.TM1;
                            existingData.PM1 = item.PM1;
                            existingData.PM11 = item.PM11;
                            existingData.Vaccumembar = item.Vaccumembar;
                            existingData.UpdateDate = DateTime.Now;
                            existingData.UpdateBy = userId;
                            this._processlogSheet1Repository.Update(existingData);
                        }
                    }
                    else
                    {
                        this._processlogSheet1Repository.Insert(item);
                    }
                }
            }
        }

        public void DeleteProcessLogSheet1Data(int[] DeletedId)
        {
            if (DeletedId != null || DeletedId.Length > 0)
            {
                var selectedId = this._processlogSheet1Repository.Table.Where(W => DeletedId.Contains(W.Id));
                this._processlogSheet1Repository.Delete(selectedId);
            }
        }

        public List<ProcessLogSheet2GridModel> GetProcessLogSheet2Data(int LineId, int BatchId, DateTime currentDate)
        {
            var allData = from a in this._processlogSheet2Repository.Table
                          where a.LineId == LineId
                          && a.BatchId == a.BatchId
                          && a.Date.Year == currentDate.Year
                          && a.Date.Month == currentDate.Month
                          && a.Date.Day == currentDate.Day
                          select new ProcessLogSheet2GridModel
                          {
                              Id = a.Id,
                              LineId = a.LineId,
                              BatchId = a.BatchId,
                              GradeId = a.GradeId,
                              Date = a.Date,
                              Time = a.Time,
                              TORQ = a.TORQ,
                              AMPS = a.AMPS,
                              RPM = a.RPM,
                              RPM1 = a.RPM1,
                              RPM2 = a.RPM2,
                              RPM3 = a.RPM3,
                              F1KGHR = a.F1KGHR,
                              F1Perc = a.F1Perc,
                              F2KGHR = a.F2KGHR,
                              F2Perc = a.F2Perc,
                              F3KGHR = a.F3KGHR,
                              F3Perc = a.F3Perc,
                              F4KGHR = a.F4KGHR,
                              F4Perc = a.F4Perc,
                              F5KGHR = a.F5KGHR,
                              F5Perc = a.F5Perc,
                              F6KGHR = a.F6KGHR,
                              F6Perc = a.F6Perc,
                              Output = a.Output,
                              NoofDiesHoles = a.NoofDiesHoles,
                              Remarks = a.Remarks
                          };
            return allData.ToList();
        }

        public void SaveProcessLogSheet2Data(Data.Models.ProcessLogSheet2[] processLogSheet2Data, int userId)
        {
            if (processLogSheet2Data != null)
            {
                for (int i = 0; i < processLogSheet2Data.Length; i++)
                {
                    var item = processLogSheet2Data[i];
                    if (item.Id > 0)
                    {
                        var existingData = this._processlogSheet2Repository.Table.FirstOrDefault(W => W.Id == item.Id);
                        if (existingData.RPM != item.RPM || existingData.TORQ != item.TORQ || existingData.AMPS != item.AMPS ||
                            existingData.RPM1 != item.RPM1 || existingData.RPM2 != item.RPM2 || existingData.RPM3 != item.RPM3 ||
                            existingData.F1KGHR != item.F1KGHR || existingData.F1Perc != item.F1Perc || existingData.F2KGHR != item.F2KGHR ||
                            existingData.F2Perc != item.F2Perc || existingData.F3KGHR != item.F3KGHR || existingData.F3Perc != item.F3Perc ||
                            existingData.F4KGHR != item.F4KGHR || existingData.F4Perc != item.F4Perc || existingData.F5KGHR != item.F5KGHR ||
                            existingData.F5Perc != item.F5Perc || existingData.F6KGHR != item.F6KGHR || existingData.F6Perc != item.F6Perc ||
                            existingData.Output != item.Output || existingData.Remarks != item.Remarks)
                        {
                            existingData.RPM = item.RPM;
                            existingData.TORQ = item.TORQ;
                            existingData.AMPS = item.AMPS;
                            existingData.RPM1 = item.RPM1;
                            existingData.RPM2 = item.RPM2;
                            existingData.RPM3 = item.RPM3;
                            existingData.F1KGHR = item.F1KGHR;
                            existingData.F1Perc = item.F1Perc;
                            existingData.F2KGHR = item.F2KGHR;
                            existingData.F2Perc = item.F2Perc;
                            existingData.F3KGHR = item.F3KGHR;
                            existingData.F3Perc = item.F3Perc;
                            existingData.F4KGHR = item.F4KGHR;
                            existingData.F4Perc = item.F4Perc;
                            existingData.F5KGHR = item.F5KGHR;
                            existingData.F5Perc = item.F5Perc;
                            existingData.F6KGHR = item.F6KGHR;
                            existingData.F6Perc = item.F6Perc;
                            existingData.Output = item.Output;
                            existingData.NoofDiesHoles = item.NoofDiesHoles;
                            existingData.Remarks = item.Remarks;
                            existingData.UpdateDate = DateTime.Now;
                            existingData.UpdateBy = userId;
                            this._processlogSheet2Repository.Update(existingData);
                        }
                    }
                    else
                    {
                        this._processlogSheet2Repository.Insert(item);
                    }
                }
            }
        }

        public void DeleteProcessLogSheet2Data(int[] DeletedId)
        {
            if (DeletedId != null || DeletedId.Length > 0)
            {
                var selectedId = this._processlogSheet2Repository.Table.Where(W => DeletedId.Contains(W.Id));
                this._processlogSheet2Repository.Delete(selectedId);
            }
        }

        #endregion

    }
}
