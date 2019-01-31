using ProERP.Data.Models;
using System;
using System.Collections.Generic;

namespace ProERP.Web.Models
{
    public class ReportViewModels
    {
        public string DataSetName { get; set; }
        public string ReportPath { get; set; }
        public object ReportDataSource { get; set; }
    }

    public class IndentConsolidateModel
    {
        public int Year { get; set; }
        public List<int> YearData { get; set; }
        public List<Services.Models.ConsolidateIndentData> ReportDataSource { get; set; }
    }

    public class IndentBudgetReportModel
    {
        public int Year { get; set; }
        public List<int> YearData { get; set; }
        public int PlantId { get; set; }
        public Plant[] PlantData { get; set; }
        public List<Services.Models.IndentReportDataSet> ReportDataSource { get; set; }
    }

    public class PreventiveMonthlySummaryReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int PlantId { get; set; }
        public int ScheduleType { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<Plant> PlantData { get; set; }
        public List<DropDownData> ScheduleData { get; set; }
        public List<ProERP.Services.Models.PreventiveSummaryReportModel> ReportDataSource { get; set; }
    }

    public class DropDownData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BDAnalysisSummaryReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int PlantId { get; set; }
        public List<int> YearData { get; set; }
        public List<Plant> PlantData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<ProERP.Services.Models.BreakDownMonthlySummaryDataSet> ReportDataSource { get; set; }
    }

    public class ShutdownMonthlySummaryReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int PlantId { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<Plant> PlantData { get; set; }
        public List<ProERP.Services.Models.ShutdownSummaryReportModel> ReportDataSource { get; set; }
    }

    public class PRSummaryReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<ProERP.Services.Models.PRReportData> ReportDataSource { get; set; }
    }

    public class RepeatedMajorReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<ProERP.Services.Models.RepeatedMajorDataSet> ReportDataSource { get; set; }
    }

    public class MaintenanceRequestReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<ProERP.Services.Models.MaintenanceRequestReportDataSet> ReportDataSource { get; set; }
    }

    public class FormulationMonthlySummaryReportModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int LineId { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<Line> LineData { get; set; }
        public List<ProERP.Services.Models.FormulationRequestReportDataModel> ReportDataSource { get; set; }
    }

    public class QualityObjectiveReportModel
    {
        public int Year { get; set; }
        public List<int> YearData { get; set; }
        public List<ProERP.Services.Models.QualityObjectiveDataModel> ReportDataSource { get; set; }
    }

    public class PreventiveAuditModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Status { get; set; }
        public List<int> YearData { get; set; }
        public List<DropDownData> MonthData { get; set; }
        public List<DropDownData> StatusData { get; set; }
        public List<ProERP.Services.Models.PreventiveAuditReportModel> ReportDataSource { get; set; }
    }

}