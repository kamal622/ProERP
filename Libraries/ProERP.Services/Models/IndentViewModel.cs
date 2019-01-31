using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    class IndentViewModel
    {
    }

    public class IndentDetailViewModel
    {
        public int Id { get; set; }
        public int? PreferredVendorId { get; set; }
        public int? ItemId { get; set; }
        public decimal? QtyNeeded { get; set; }
        public int? StatusId { get; set; }
        public int? PlantId { get; set; }
        public int? LineId { get; set; }
        public int? MachineId { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public DateTime? PoDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string PoNo { get; set; }
        public decimal? FinalPrice { get; set; }
        public decimal? IssuedQty { get; set; }
        public decimal? PoAmount { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime? Rejectedon { get; set; }
        public string UserName { get; set; }
        public string UserName2 { get; set; }
        public string Make { get; set; }
        public decimal? EstimatePrice { get; set; }
        public string JobDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public int? Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
    }

    public class ConsolidateIndentData
    {
       public int PlantId { get; set; }
        public string PlantName { get; set; }
        public string RequisitionType { get; set; }
        public int BudgetId { get; set; }
        public string BudgetType { get; set; }
        public string BudgetCode { get; set; }
        public decimal Expense { get; set; }
        public DateTime IndentDate { get; set; }
        public decimal TotalBudget { get; set; }
    }

    public class PRReportData
    {
        public string IndentNo { get; set; }
        public DateTime PRDate { get; set; }
        public DateTime? PODate { get; set; }
        public decimal PRAmount { get; set; }
        public decimal? POAmount { get; set; }
    }
}
