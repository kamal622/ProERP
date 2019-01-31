using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Models
{
    public class IndentReportDataSet
    {
        public int Id { get; set; }
        public string IndentNo { get; set; }
        public DateTime? IndentDate { get; set; }
        public int? PlantId { get; set; }
        public int? LineId { get; set; }
        public int? MachineId { get; set; }
        public string MachineName { get; set; }
        public string PlantName { get; set; }
        public decimal? Budget { get; set; }
        public int BudgetId { get; set; }
        public string BudgetCode { get; set; }
        public string BudgetType { get; set; }
        public string LineName { get; set; }
        //public int UnitPrice { get; set; }
        //public int TotalAmount { get; set; }
        public Nullable<int> ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal QtyNeeded { get; set; }
        public string Supplier { get; set; }
        public Nullable<decimal> EstimatePrice { get; set; }
        public Nullable<decimal> FinalPrice { get; set; }
        public Nullable<decimal> IssuedQty { get; set; }
        public int StatusId { get; set; }
        public DateTime? IssuedOn { get; set; }
        public string InitiatedBy { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Make { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public string RequisitionType { get; set; }
        public decimal? ExchangeRate { get; set; }
    }
}
