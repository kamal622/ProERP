using ProERP.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indent
{
    public class IndentBudgetServices
    {
        private readonly IRepository<Data.Models.IndentBudget> _IndentBudgetRepository;
        private readonly IRepository<Data.Models.IndentDetail> _IndentDetailRepository;

        public IndentBudgetServices(IRepository<Data.Models.IndentBudget> IndentBudgetRepository,
            IRepository<Data.Models.IndentDetail> indentDetailRepository)
        {
            this._IndentBudgetRepository = IndentBudgetRepository;
            this._IndentDetailRepository = indentDetailRepository;
        }
        public List<Data.Models.IndentBudget> GetAllBudgetCode(string BudgetType)
        {
            return this._IndentBudgetRepository.Table.Where(w => w.BudgetType == BudgetType && w.IsActive).ToList();
        }
        public int Add(Data.Models.IndentBudget IndentBudget)
        {
            _IndentBudgetRepository.Insert(IndentBudget);
            return IndentBudget.Id;
        }
        public void Update(Data.Models.IndentBudget IndentBudget)
        {
            Data.Models.IndentBudget existingIB = _IndentBudgetRepository.Table.FirstOrDefault(w => w.Id == IndentBudget.Id);

            if (existingIB != null)
            {
                existingIB.BudgetType = IndentBudget.BudgetType;
                existingIB.BudgetCode = IndentBudget.BudgetCode;
                existingIB.Amount = IndentBudget.Amount;
                existingIB.IsActive = IndentBudget.IsActive;
                existingIB.FinancialYear = IndentBudget.FinancialYear;

                this._IndentBudgetRepository.Update(existingIB);
            }
        }
        public List<Data.Models.IndentBudget> GetIndentBudgetData(string BudgetType)
        {
            var allData = from a in this._IndentBudgetRepository.Table
                          select a;
            if (!string.IsNullOrEmpty(BudgetType))
                allData = allData.Where(w => w.BudgetType.Contains(BudgetType));
            return allData.ToList();
        }
        public Data.Models.IndentBudget GetForId(int Id)
        {
            return this._IndentBudgetRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
        public int[] Delete(int[] ids)
        {
            List<int> ibIds = new List<int>();
            foreach (int id in ids)
            {
                Data.Models.IndentBudget IndentBudget = _IndentBudgetRepository.Table.FirstOrDefault(w => w.Id == id);
                _IndentBudgetRepository.Delete(IndentBudget);

            }
            return ibIds.ToArray();
        }

        public decimal GetRemainingBudget(int budgetId)
        {
            decimal usedBudget = 0;
            if (this._IndentDetailRepository.Table.Any(w => w.Indent.BudgetId == budgetId && w.IndentStatu.Description == "Approved"))
                usedBudget = this._IndentDetailRepository.Table.Where(w => w.Indent.BudgetId == budgetId && w.IndentStatu.Description == "Approved").Sum(s => (s.FinalPrice == null ? s.EstimatePrice : s.FinalPrice.Value) * s.QtyNeeded);
            var totalBudget = this._IndentBudgetRepository.Table.FirstOrDefault(f => f.Id == budgetId).Amount ?? 0;
            return totalBudget - usedBudget;
        }
    }
}
