using ProERP.Core.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indent
{
    public class IndentDetailServices
    {
        private readonly IRepository<Data.Models.IndentDetail> _IndentDetailRepository;
        private readonly IRepository<Data.Models.Indent> _IndentsRepository;
        private readonly IRepository<Data.Models.Item> _itemRepository;
        private readonly IRepository<Data.Models.IndentDetailAttachment> _indentDetailAttRepository;
        private readonly IRepository<Data.Models.CurrencyMaster> _currencyRepository;
        public IndentDetailServices(IRepository<Data.Models.IndentDetail> IndentDetailRepository, IRepository<Data.Models.Indent> IndentsRepository
            , IRepository<Data.Models.Item> itemRepository, IRepository<Data.Models.IndentDetailAttachment> indentDetailAttRepository,
            IRepository<Data.Models.CurrencyMaster> currencyRepository)
        {
            this._IndentDetailRepository = IndentDetailRepository;
            this._IndentsRepository = IndentsRepository;
            this._itemRepository = itemRepository;
            this._indentDetailAttRepository = indentDetailAttRepository;
            this._currencyRepository = currencyRepository;
        }

        public int Add(Data.Models.IndentDetail IndentDetail, string itemDescription)
        {
            var existingItem = this._itemRepository.Table.FirstOrDefault(f => f.Id == IndentDetail.ItemId);
            if (existingItem != null)
                existingItem.Description = itemDescription;

            var existingIndent = this._IndentsRepository.Table.FirstOrDefault(f => f.Id == IndentDetail.IndentId);
            if (existingIndent.RequisitionType == "JR")
                IndentDetail.ItemId = null;

            _IndentDetailRepository.Insert(IndentDetail);
            return IndentDetail.Id;
        }

        public int Update(Data.Models.IndentDetail IndentDetail, string itemDescription)
        {
            Data.Models.IndentDetail existingID = _IndentDetailRepository.Table.FirstOrDefault(w => w.Id == IndentDetail.Id);

            if (existingID != null)
            {
                var existingItem = this._itemRepository.Table.FirstOrDefault(f => f.Id == existingID.ItemId);
                if (existingItem != null)
                    existingItem.Description = itemDescription;

                existingID.PreferredVendorId = IndentDetail.PreferredVendorId;
                existingID.Make = IndentDetail.Make;
                existingID.EstimatePrice = IndentDetail.EstimatePrice;
                existingID.Currency = IndentDetail.Currency;
                existingID.ExchangeRate = IndentDetail.ExchangeRate;
                existingID.QtyNeeded = IndentDetail.QtyNeeded;
                existingID.RequiredByDate = IndentDetail.RequiredByDate;
                existingID.StatusId = IndentDetail.StatusId;
                existingID.JobDescription = IndentDetail.JobDescription;
                existingID.UnitOfMeasure = IndentDetail.UnitOfMeasure;

                _IndentDetailRepository.Update(existingID);
                return existingID.Id;
            }
            else
                return 0;
        }
        public List<Data.Models.CurrencyMaster> GetAllCurrencyList()
        {
            return this._currencyRepository.Table.OrderBy(w => w.Currency).ToList();
        }
        public List<Data.Models.IndentDetail> GetIndentDetailData(int IndentId)
        {
            var allData = from a in this._IndentDetailRepository.Table
                          where a.IndentId == IndentId
                          select a;

            return allData.ToList();

        }
        public List<Data.Models.IndentDetail> GetNestedGridData(int IndentId)
        {
            var allData = from a in this._IndentDetailRepository.Table
                          where a.IndentId == IndentId
                          orderby a.RequiredByDate descending
                          select a;

            return allData.ToList();

        }
        public void DeleteIndentDetail(int[] Ids)
        {
            var deletedItems = this._IndentDetailRepository.Table.Where(w => Ids.Contains(w.Id));
            this._indentDetailAttRepository.Delete(this._indentDetailAttRepository.Table.Where(w => Ids.Contains(w.IndentDetail.Id)));
            this._IndentDetailRepository.Delete(deletedItems);
        }
        public void DeleteAllIndent(int Id)
        {
            var deletedIndentDetails = this._IndentDetailRepository.Table.Where(w => w.IndentId == Id);
            this._indentDetailAttRepository.Delete(this._indentDetailAttRepository.Table.Where(w => deletedIndentDetails.Select(s => s.Id).Contains(w.IndentDetailId)));
            this._IndentDetailRepository.Delete(deletedIndentDetails);
            var deleteIndent = _IndentsRepository.Table.Where(w => w.Id == Id);
            this._IndentsRepository.Delete(deleteIndent);
        }
        public IndentDetailViewModel GetForId(int Id)
        {
            var allData = from a in this._IndentDetailRepository.Table
                          where a.Id == Id
                          select new IndentDetailViewModel
                          {
                              Id = a.Id,
                              PreferredVendorId = a.PreferredVendorId,
                              ItemId = a.ItemId,
                              QtyNeeded = a.QtyNeeded,
                              StatusId = a.StatusId,
                              PlantId = a.PlantId,
                              LineId = a.LineId,
                              MachineId = a.MachineId,
                              RequiredByDate = a.RequiredByDate,
                              PoDate = a.PoDate,
                              DeliveryDate = a.DeliveryDate,
                              PoNo = a.PoNo,
                              PoAmount=a.PoAmount,
                              IssuedQty = a.IssuedQty,
                              FinalPrice = a.FinalPrice,
                              UserName = a.User.UserName,
                              ApprovedOn = a.ApprovedOn,
                              UserName2 = a.User2.UserName,
                              Rejectedon = a.Rejectedon,
                              Make = a.Make,
                              EstimatePrice = a.EstimatePrice,
                              JobDescription = a.JobDescription,
                              UnitOfMeasure = a.UnitOfMeasure,
                              Currency = a.Currency ?? 1,
                              ExchangeRate = a.ExchangeRate ?? 1
                          };
            return allData.FirstOrDefault();
            //return this._IndentDetailRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
        public int UpdateApproveTask(Data.Models.IndentDetail IndentDetail, DateTime approvedDate, int userId, DateTime rejectedDate)
        {
            Data.Models.IndentDetail existingID = _IndentDetailRepository.Table.FirstOrDefault(w => w.Id == IndentDetail.Id);
            if (existingID != null)
            {
                existingID.QtyNeeded = IndentDetail.QtyNeeded;
                existingID.RequiredByDate = IndentDetail.RequiredByDate;
                existingID.ApprovedOn = approvedDate;
                existingID.ApprovedBy = userId;
                existingID.StatusId = IndentDetail.StatusId;
                existingID.Rejectedon = null;
                existingID.RejectedBy = null;

                _IndentDetailRepository.Update(existingID);
                return existingID.Id;
            }
            else
                return 0;
        }

        public void UpdateIndentDetailStatus(int indentId, int statusId, int userId,DateTime? PoDate, DateTime? DeliveryDate, string PoNo, decimal? PoAmount)
        {
            var indentDetails = this._IndentDetailRepository.Table.Where(w => w.IndentId == indentId).ToArray();
            
            for (int i = 0; i < indentDetails.Length; i++)
            {
                var indentDetail = indentDetails[i];
                indentDetail.StatusId = statusId;
                if (statusId == 2) // Approved
                {
                    indentDetail.ApprovedBy = userId;
                    indentDetail.ApprovedOn = DateTime.UtcNow;
                    indentDetail.RejectedBy = null;
                    indentDetail.Rejectedon = null;
                }
                else if(statusId == 4)  // PO
                {
                    indentDetail.PoDate= PoDate;
                    indentDetail.DeliveryDate = DeliveryDate;
                    indentDetail.PoNo = PoNo;
                    indentDetail.PoAmount = (PoAmount == null ? 0 : PoAmount.Value);
                }
                else  // Rejected
                {
                    indentDetail.ApprovedBy = null;
                    indentDetail.ApprovedOn = null;
                    indentDetail.RejectedBy = userId;
                    indentDetail.Rejectedon = DateTime.UtcNow;
                }
            }

            this._IndentDetailRepository.Update(indentDetails);

        }

        public bool getIndentStatus(int indentId,int StatusId)
        {
            if(StatusId == 1)//new
                return this._IndentDetailRepository.Table.Any(w => w.IndentId == indentId && w.StatusId == 1);
            else if(StatusId == 2)//approve
                return this._IndentDetailRepository.Table.Any(w => w.IndentId == indentId && w.StatusId == 2);
            else if(StatusId == 3)//reject
                return this._IndentDetailRepository.Table.Any(w => w.IndentId == indentId && w.StatusId == 3);
            else//po
                return this._IndentDetailRepository.Table.Any(w => w.IndentId == indentId && w.StatusId == 4);
        }

        public int UpdateRejectTask(Data.Models.IndentDetail IndentDetail, DateTime rejectedDate, int userId, DateTime approvedDate)
        {
            Data.Models.IndentDetail existingID = _IndentDetailRepository.Table.FirstOrDefault(w => w.Id == IndentDetail.Id);
            if (existingID != null)
            {
                existingID.Rejectedon = rejectedDate;
                existingID.RejectedBy = userId;
                existingID.StatusId = IndentDetail.StatusId;
                existingID.ApprovedBy = null;
                existingID.ApprovedOn = null;
                _IndentDetailRepository.Update(existingID);
                return existingID.Id;
            }
            else
                return 0;
        }
        public int UpdatePoTask(Data.Models.IndentDetail IndentDetail)
        {
            Data.Models.IndentDetail existingID = _IndentDetailRepository.Table.FirstOrDefault(w => w.Id == IndentDetail.Id);
            if (existingID != null)
            {
                existingID.PoNo = IndentDetail.PoNo;
                existingID.PoDate = IndentDetail.PoDate;
                existingID.DeliveryDate = IndentDetail.DeliveryDate;
                existingID.StatusId = IndentDetail.StatusId;
                existingID.PoAmount = IndentDetail.PoAmount;

                _IndentDetailRepository.Update(existingID);
                return existingID.Id;
            }
            else
                return 0;
        }
        public int UpdateIssuedTask(Data.Models.IndentDetail IndentDetail, DateTime issuedDate, int userId)
        {
            Data.Models.IndentDetail existingID = _IndentDetailRepository.Table.FirstOrDefault(w => w.Id == IndentDetail.Id);
            if (existingID != null)
            {
                existingID.IssuedOn = issuedDate;
                existingID.IssuedBy = userId;
                existingID.IssuedQty = IndentDetail.IssuedQty;
                existingID.FinalPrice = IndentDetail.FinalPrice;
                existingID.StatusId = IndentDetail.StatusId;

                _IndentDetailRepository.Update(existingID);
                return existingID.Id;
            }
            else
                return 0;
        }
        public Data.Models.IndentDetailAttachment GetAttachment(int Id)
        {
            return this._indentDetailAttRepository.Table.FirstOrDefault(f => f.Id == Id);
        }

        public List<IndentReportDataSet> GetIndentDetailById(int indentID)
        {
            var allData = this._IndentDetailRepository.Table
                .Where(w => w.IndentId == indentID)
               .Select(s => new IndentReportDataSet
               {
                   Id = s.Id,
                   PlantId = s.PlantId,
                   LineId = s.LineId,
                   LineName = s.Line.Name,
                   MachineId = s.MachineId,
                   MachineName = s.Machine.Name,
                   PlantName = s.Plant.Name,
                   Budget = s.Indent.IndentBudget.Amount,
                   BudgetCode = s.Indent.IndentBudget.BudgetCode,
                   BudgetType = s.Indent.IndentBudget.BudgetType,
                   IndentDate = s.Indent.CreatedOn,
                   IndentNo = s.Indent.IndentNo,
                   //.Where(w => w.EffectiveFrom <= s.IssuedOn).Where(w => w.EffectiveTo >= s.IssuedOn).Select(a => a.MonthlyBudget).FirstOrDefault(),
                   //UnitPrice = s.UnitPrice,
                   //TotalAmount = s.TotalAmount,
                   ItemId = s.ItemId,
                   ItemName = s.Item.Name,
                   ItemCode = s.Item.ItemCode,
                   QtyNeeded = s.QtyNeeded,
                   FinalPrice = s.FinalPrice,
                   IssuedQty = s.IssuedQty,
                   StatusId = s.StatusId,
                   IssuedOn = s.IssuedOn,
                   Description = s.Item.Description,
                   EstimatePrice = s.EstimatePrice,
                   ExchangeRate = (s.ExchangeRate == null ? 1 : s.ExchangeRate),
                   InitiatedBy = s.Indent.User.UserName,
                   Supplier = s.Vendor.Name,
                   Note = s.Indent.Note,
                   RequiredByDate = s.RequiredByDate,
                   UnitOfMeasure = s.Item == null ? s.UnitOfMeasure : s.Item.UnitOfMeasure,
                   Location = (s.Plant == null ? "General" : (s.Plant.Name + (s.Line == null ? "" : "->" + s.Line.Name)
                                                                            + (s.Machine == null ? "" : "->" + s.Machine.Name))),
                   Make = s.Make,
                   JobDescription = s.JobDescription
               });


            return allData.OrderBy(o => o.IssuedOn).OrderBy(o => o.PlantName).ToList();
        }

        
    }
}
