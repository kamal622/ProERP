using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class IndentViewModel
    {
        public int Id { get; set; }
        public ItemViewModel ItemViewModel { get; set; }
        public VendorViewModel VendorViewModel { get; set; }
    }
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string SpecificationFile { get; set; }
        public string MOC { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? Price { get; set; }
        public int? AvailableQty { get; set; }
        public string Description { get; set; }
        public int? VendorId { get; set; }
    }
    public class GenerateIndentViewModel
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string IndentNo { get; set; }
        public string BudgetType { get; set; }
        public string BudgetCode { get; set; }
        public int? BudgetId { get; set; }
        public string Note { get; set; }
        public string Subject { get; set; }
        public int? StatusId { get; set; }
        public DateTime? PoDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string PoNo { get; set; }
        public decimal? PoAmount { get; set; }
        public string RequisitionType { get; set; }
        public string BudgetHead { get; set; }
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
        public decimal? PoAmount { get; set; }
        public decimal? IssuedQty { get; set; }
        public decimal? FinalPrice { get; set; }
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

    public class IndentBudgetViewModel
    {
        public int Id { get; set; }
        public string BudgetType { get; set; }
        public string BudgetCode { get; set; }
        public decimal? Amount { get; set; }
        public bool IsActive { get; set; }
        public string FinancialYear { get; set; }
    }
    public class VendorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
    }
}