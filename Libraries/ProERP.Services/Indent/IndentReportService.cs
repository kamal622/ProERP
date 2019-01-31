using ProERP.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Indent
{
    public class IndentReportService
    {
        public List<IndentReportDataSet> GetIssuedIndentDetail(int plantId, string year)
        {
            ProERPContext db = new ProERPContext();
            int approvedYear = DateTime.UtcNow.Year;
            int.TryParse(year, out approvedYear);

            //int[] budgetIds = db.IndentDetails.Where(w => w.IndentStatu.Description == "Approved")
            //    .Where(w => w.ApprovedOn.Value.Year == approvedYear)
            //    .Where(w => w.PlantId == plantId)
            //    .Select(s => s.Indent.BudgetId).ToArray();

            //  var totalBudget = db.IndentBudgets.Where(w => budgetIds.Contains(w.Id)).Sum(s => s.Amount);

            var allData = db.IndentDetails.Where(w => w.IndentStatu.Description == "Approved")
                .Where(w => w.ApprovedOn.Value.Year == approvedYear)
                .Where(w => w.PlantId == plantId)
                .Select(s => new IndentReportDataSet
                {
                    Id = s.Id,
                    PlantId = s.PlantId,
                    LineId = s.LineId,
                    LineName = s.Line.Name,
                    MachineId = s.MachineId,
                    MachineName = s.Machine.Name,
                    PlantName = s.Plant.Name,
                    //Budget = totalBudget,
                    BudgetId = s.Indent.BudgetId,
                    Budget = s.Indent.IndentBudget.Amount,
                    BudgetType = s.Indent.IndentBudget.BudgetType,
                    BudgetCode = s.Indent.IndentBudget.BudgetCode,
                    //.Where(w => w.EffectiveFrom <= s.IssuedOn).Where(w => w.EffectiveTo >= s.IssuedOn).Select(a => a.MonthlyBudget).FirstOrDefault(),
                    //UnitPrice = s.UnitPrice,
                    //TotalAmount = s.TotalAmount,
                    ItemId = s.ItemId,
                    ItemName = s.Item != null ? s.Item.Name : s.JobDescription,
                    QtyNeeded = s.QtyNeeded,
                    IssuedQty = s.IssuedQty == null ? s.QtyNeeded : s.IssuedQty,
                    FinalPrice = s.FinalPrice == null ? s.EstimatePrice : s.FinalPrice,
                    StatusId = s.StatusId,
                    IssuedOn = s.IssuedOn == null ? s.RequiredByDate : s.IssuedOn,
                    RequisitionType = s.Indent.RequisitionType
                });


            return allData.OrderBy(o => o.IssuedOn).OrderBy(o => o.PlantName).ToList();
        }

        public List<IndentReportDataSet> GetIndentDetailById(int indentID)
        {
            ProERPContext db = new ProERPContext();

            var allData = db.IndentDetails
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

        public Data.Models.IndentDetailAttachment GetAttachment(int Id)
        {
            using (var entity = new ProERP.Data.ProERPContext())
                return entity.IndentDetailAttachments.FirstOrDefault(f => f.Id == Id);
        }

        public List<ConsolidateIndentData> GetConsolidatedIndentData(string year)
        {
            int approvedYear = DateTime.UtcNow.Year;
            int.TryParse(year, out approvedYear);

            using (var entity = new ProERPContext())
            {
                var allData = from a in entity.IndentDetails
                              where a.Indent.CreatedOn.Year == approvedYear
                              select new ConsolidateIndentData
                              {
                                  PlantId = (a.Plant == null ? 0 : a.PlantId.Value),
                                  PlantName = (a.Plant == null ? "NA" : a.Plant.Name),
                                  RequisitionType = a.Indent.RequisitionType,
                                  BudgetId = a.Indent.BudgetId,
                                  BudgetType = a.Indent.IndentBudget.BudgetType,
                                  BudgetCode = a.Indent.IndentBudget.BudgetCode,
                                  //Expense = (a.PoAmount == null ? a.EstimatePrice : a.FinalPrice.Value) * (a.IssuedQty == null ? a.QtyNeeded : a.IssuedQty.Value),
                                  Expense = (a.EstimatePrice) * (a.IssuedQty == null ? a.QtyNeeded : a.IssuedQty.Value),
                                  IndentDate = a.Indent.CreatedOn,
                                  TotalBudget = a.Indent.IndentBudget.Amount == null ? 0 : a.Indent.IndentBudget.Amount.Value
                              };

                return allData.ToList();
            }
        }
    }
}
