using ProERP.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class MasterViewModel
    {

    }

    public enum JsonResponseStatus
    {
        Success = 1,
        Warning = 2,
        Error = 3
    }
    public class JsonResponse
    {
        public JsonResponseStatus Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class SiteViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string InCharge { get; set; }
        public string Description { get; set; }
    }

    public class PlantViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string PlantInCharge { get; set; }
        public string Location { get; set; }
        public Nullable<int> SiteId { get; set; }
        public Site[] Sites { get; set; }
        public string SiteName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class LineViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string InCharge { get; set; }
        public Site[] Sites { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Plant[] Plants { get; set; }
        public Nullable<int> PlantId { get; set; }
        public string PlantName { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
    }



    public class SubAssemblyViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Site[] Sites { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Plant[] Plants { get; set; }
        public Nullable<int> PlantId { get; set; }
        public string PlantName { get; set; }
        public Line[] Lines { get; set; }
        public Nullable<int> LineId { get; set; }
        public string[] LineName { get; set; }
        public Machine[] Machines { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string[] MachineName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PreventiveMaintenanceViewModel
    {

        public int Id { get; set; }
        public string Description { get; set; }

        public string Checkpoints { get; set; }

        public int ScheduleType { get; set; }
        public int Interval { get; set; }
        public string ScheduleTypeName { get; set; }

        public string ShutdownRequired { get; set; }

        public DateTime? ScheduleStartDate { get; set; }

        public DateTime? ScheduleEndDate { get; set; }

        public DateTime? LastReviewDate { get; set; }

        public DateTime? NextReviewDate { get; set; }

        public DateTime? FromReviewDate { get; set; }

        public DateTime? ToReviewDate { get; set; }

        public DateTime? ReviewDate { get; set; }
        public int ReviewBy { get; set; }

        public string Notes { get; set; }
        public Site[] Sites { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }

        public Plant[] Plants { get; set; }
        public Nullable<int> PlantId { get; set; }
        public string PlantName { get; set; }
        public Data.Models.Line[] Lines { get; set; }
        public Nullable<int> LineId { get; set; }
        public string LineName { get; set; }
        public Machine[] Machines { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string MachineName { get; set; }

        public List<int> UserId { get; set; }
        public string UserName { get; set; }

        public string ErrorMessage { get; set; }

        public int WorkId { get; set; }
        public int CategoryId { get; set; }
        public int PreferredVendorId { get; set; }
        public string WorkDescription { get; set; }
        public int VendorCategoryId { get; set; }
        public int Severity { get; set; }
        public bool IsObservation { get; set; }
    }

    public class PartViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Site[] Sites { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Plant[] Plants { get; set; }
        public Nullable<int> PlantId { get; set; }
        public string PlantName { get; set; }
        public Data.Models.Line[] Lines { get; set; }
        public Nullable<int> LineId { get; set; }
        public string LineName { get; set; }
        public Machine[] Machines { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string MachineName { get; set; }

        public string Description { get; set; }


    }

    public class MaintenanceRequestViewModel
    {
        public int Id { get; set; }
        public string SerialNo { get; set; }
        public int RequestBy { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime RequestTime { get; set; }
        public bool IsBreakdown { get; set; }
        public Site[] Sites { get; set; }
        public int SiteId { get; set; }
        //public Plant[] Plants { get; set; }
        public Nullable<int> PlantId { get; set; }
        public Nullable<int> LineId { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string Problem { get; set; }
        public string Description { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public bool IsCritical { get; set; }
        public Nullable<int> BreakdownType { get; set; }
        public Nullable<int> AssignTo { get; set; }
        public Nullable<int> AssignBy { get; set; }
        public int RemarksBy { get; set; }
        public string Remarks { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public DateTime? WorkStartTime { get; set; }
        public DateTime? WorkEndDate { get; set; }
        public DateTime? WorkEndTime { get; set; }
        public string ProgressRemarks { get; set; }
        public string CompleteRemarks { get; set; }
        public string CloseRemarks { get; set; }
        public string HoldRemarks { get; set; }
    }

    public class IndentsViewModel
    {
        public int Id { get; set; }
        public Site[] Sites { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Plant[] Plants { get; set; }
        public Nullable<int> PlantId { get; set; }
        public string PlantName { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int QtyInStock { get; set; }
        public int QtyNeeded { get; set; }
        public int UnitPrice { get; set; }
        public int TotalAmount { get; set; }
        public string Specification { get; set; }
        public int Priority { get; set; }
        public int IndentNo { get; set; }
        public int PreferredVendorId { get; set; }
        public int VendorCategoryId { get; set; }

    }

    public class TreeViewData
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public int ParentId { get; set; }
    }

    public class ReleaseNoteViewModel
    {
        public List<Data.Models.VersionNote> AllData { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public double TotalPages { get; set; }
    }

    public class MRRemarksViewModel
    {
        public int Id { get; set; }
        public List<GridData> AllRemarks { get; set; }
        
    }
    public class GridData
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
        public string RemarksBy { get; set; }
        public DateTime? RemarksDate { get; set; }
    }

    public class UtilityDetailsViewModel
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string GroupName { get; set; }
        public int OrderNo { get; set; }
        public string Formula { get; set; }
        public bool IsRequired { get; set; }
        public bool IsAggregate { get; set; }
        public string AggregateFunction { get; set; }
        //public int FixColumnId { get; set; }
        public bool IsSearchColumn { get; set; }
        public bool IsEditable { get; set; }
        public string OnChangeFormula { get; set; }
        public int Width { get; set; }
        public bool IsAutoGenerated { get; set; }
        public bool IsOrderBy { get; set; }
        public string DefaultValue { get; set; }
    }
    public class FormulationCreateViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public string QtyToProduce { get; set; }
        public string LOTSize { get; set; }
        public string ColorSTD { get; set; }
        public string LotNo { get; set; }
        public string Notes { get; set; }
        public string WorkOrderNo { get; set; }
        public int LineId { get; set; }
        public int ParentId { get; set; }
        public List<FormulationMasterViewModel> formulation { get; set; }
    }

    public class FormulationMasterViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public decimal PreviousBaseValue { get; set; }
        public decimal BaseValue { get; set; }
        public decimal InGrams { get; set; }
    }

    public class FormulationRemarksViewModel
    {
        public int Id { get; set; }
        public List<GridData> AllRemarks { get; set; }
    }
    public class WIPStoreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }

    public class FormulationRequestsViewModel
    {
        public int Id { get; set; }
        public string LotNo { get; set; }
        public string GradeName { get; set; }
        public int QtyToProduce { get; set; }
        public int UOM { get; set; }
        public string LOTSize { get; set; }
        public string ColorSTD { get; set; }
        public string Notes { get; set; }
        public int StatusId { get; set; }
        public int ProductId { get; set; }
        public string WorkOrderNo { get; set; }
        public string UserRole { get; set; }
        public FormulationRequestsDetailsViewModel FormulationRequestsDetail { get; set; }
        public RMItemViewModel itemdata { get; set; }
    }

    public class FormulationRequestsDetailsViewModel
    {
        public int Id { get; set; }
        public int FormulationRequestId { get; set; }
        public int MachineId { get; set; }
        public int ItemId { get; set; }
        public int ItemQty { get; set; }
        public int UOM { get; set; }
    }

    public class RMItemViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string ItemCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
    }

    public class ProductDropDownModel
    {
        public int ProductId { get; set; }
        public string GradeName { get; set; }
    }

    public class ColourQASpecationViewModel
    {
        public int ColourId { get; set; }
        public int QAId { get; set; }
        public int FormulationRequestId { get; set; }
        public string DeltaE { get; set; }
        public string DeltaL { get; set; }
        public string Deltaa { get; set; }
        public string Deltab { get; set; }
        public string MFI220c10kg { get; set; }
        public string SPGravity { get; set; }
        public string AshContent { get; set; }
        public string NotchImpact { get; set; }
        public string Tensile { get; set; }
        public string FlexuralModule { get; set; }
        public string FlexuralStrength { get; set; }
        public string Elongation { get; set; }
        public string Flammability { get; set; }
        public string GWTAt { get; set; }
    }
}