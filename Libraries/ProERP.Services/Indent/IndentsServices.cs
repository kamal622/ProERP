using ProERP.Core.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indent
{
    public class IndentsServices
    {
        private readonly IRepository<Data.Models.Indent> _IndentsRepository;
        private readonly IRepository<Data.Models.IndentDetail> _IndentDetailRepository;
        private readonly IRepository<Data.Models.Sequence> _sequenceRepository;

        public IndentsServices(IRepository<Data.Models.Indent> IndentsRepository, IRepository<Data.Models.IndentDetail> indentDetailRepository, IRepository<Data.Models.Sequence> sequenceRepository)
        {
            this._IndentsRepository = IndentsRepository;
            this._IndentDetailRepository = indentDetailRepository;
            this._sequenceRepository = sequenceRepository;
        }

        public int Add(Data.Models.Indent Indent)
        {
            string budget = Indent.IndentBudget.BudgetType;
            string seqName = string.Format("Indent_{0}", Indent.RequisitionType);
            int lastIndentNo = this._sequenceRepository.Table.Where(w => w.Name == seqName).Select(s => s.Value).FirstOrDefault();

            var seq = this._sequenceRepository.Table.Where(w => w.Name == seqName).FirstOrDefault();

            if (seq == null)
                this._sequenceRepository.Insert(new Data.Models.Sequence { Name = seqName, Value = 0 });
            else
            {
                seq.Value = lastIndentNo + 1;
                this._sequenceRepository.Update(seq);
            }


            Indent.IndentNo = string.Format("{0}-{1}-{2}-{3}", lastIndentNo + 1, Indent.RequisitionType, budget, DateTime.UtcNow.Year);

            if (this._IndentsRepository.Table.Any(a => a.IndentNo == Indent.IndentNo))
                return Add(Indent);

            Indent.IndentBudget = null;
            _IndentsRepository.Insert(Indent);
            return Indent.Id;
        }

        public void UpdateIndentNo(int Id, string indentNo)
        {
            var existingIndent = this._IndentsRepository.Table.FirstOrDefault(f => f.Id == Id);
            if (existingIndent != null)
            {
                existingIndent.IndentNo = indentNo;
                this._IndentsRepository.Update(existingIndent);
            }
        }
        public List<Data.Models.Indent> GetIndentGridData(int createdByUserId, string itemName, string RequisitionType)
        {
            if (itemName == null)
                itemName = "";
            if (string.IsNullOrEmpty(RequisitionType))
                RequisitionType = "ALL";

            var allData = from a in this._IndentsRepository.Table
                          where a.CreatedBy == (createdByUserId > 0 ? createdByUserId : a.CreatedBy)
                          && a.RequisitionType == (RequisitionType == "ALL" ? a.RequisitionType : RequisitionType)
                          orderby a.CreatedOn descending
                          select a;

            int[] indentIds = this._IndentDetailRepository.Table.Where(w => w.Item.Name.Contains(itemName) || w.JobDescription.Contains(itemName)).Select(s => (int)s.IndentId).ToArray();

            return allData.Where(w => indentIds.Contains(w.Id)).ToList();
        }

        public Data.Models.Indent GetForId(int IndentId)
        {
            return this._IndentsRepository.Table.FirstOrDefault(f => f.Id == IndentId);
        }

        public void UpdateIndentNote(int indentId, string note, string Subject, int StatusId, int userId, DateTime? approvedDate, DateTime? rejectedDate, DateTime? PoDate, DateTime? DeliveryDate, string PoNo, decimal? PoAmount, DateTime UpdatedDate)
        {
            var existingIndent = this._IndentsRepository.Table.FirstOrDefault(f => f.Id == indentId);
            if (existingIndent != null)
            {
                if (StatusId == 2) // Approved
                {
                    existingIndent.ApprovedBy = userId;
                    existingIndent.ApprovedOn = approvedDate;
                    existingIndent.RejectedBy = null;
                    existingIndent.RejectedOn = null;
                }
                else if (StatusId == 3)//reject
                {
                    existingIndent.RejectedBy = userId;
                    existingIndent.RejectedOn = rejectedDate;
                    existingIndent.ApprovedBy = null;
                    existingIndent.ApprovedOn = null;
                }
                else//PO
                {
                    existingIndent.PoDate = PoDate;
                    existingIndent.DeliveryDate = DeliveryDate;
                    existingIndent.PoNo = PoNo;
                    existingIndent.PoAmount = PoAmount;
                }
                existingIndent.StatusId = StatusId;
                existingIndent.Note = note;
                existingIndent.Subject = Subject;
                existingIndent.UpdatedBy = userId;
                existingIndent.UpdatedOn = UpdatedDate;
                this._IndentsRepository.Update(existingIndent);
            }
        }
    }
}
