using ProERP.Services.Line;
using ProERP.Services.Plant;
using ProERP.Services.Reports;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        #region Priavate Member
        private readonly ReportService _reportService;
        private readonly PlantService _plantService;
        private readonly LineService _lineService;
        #endregion

        #region .Contr
        public ReportController(ReportService reportService, PlantService plantService,
                                LineService lineService)
        {
            this._reportService = reportService;
            this._plantService = plantService;
            this._lineService = lineService;
        }
        #endregion

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportTemplate(string ReportName/*, string ReportDescription, int Width, int Height*/)
        {
            var rptInfo = new ReportInfo();
            rptInfo.ReportURL = String.Format("/Reports/ReportTemplate.aspx?ReportName={0}", ReportName);
            //{
            //    ReportName = ReportName,
            //    //ReportDescription = ReportDescription,
            //    ReportURL = Server.MapPath(String.Format("~/Reports/ReportTemplate.aspx?ReportName={0}", ReportName)),
            //    //Width = Width,
            //    //Height = Height
            //};

            return View(rptInfo);
        }

        #region Breakdown
        public ActionResult HistoryCard()
        {
            return View();
        }

        public ActionResult BreakdownMonthlySummary()
        {
            return View();
        }

        public ActionResult BDAnalysisSummary()
        {
            BDAnalysisSummaryReportModel model = new BDAnalysisSummaryReportModel();
            model.YearData = new List<int>();
            model.MonthData = new List<DropDownData>();
            for (int i = 0; i < 15; i++)
            {
                model.YearData.Add(DateTime.Now.Year - i);
            }
            model.MonthData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.PlantData = this._plantService.GetAllPlants();
            model.PlantData.Insert(0, new Data.Models.Plant { Id = 0, Name = "-- ALL --" });
            model.Year = DateTime.Now.Year;
            model.PlantId = 0;
            model.Month = DateTime.Now.Month;
            model.ReportDataSource = this._reportService.GetBDAnalysisSummaryReportData(model.PlantId, model.Year, model.Month);
            return View(model);
        }

        [HttpPost]
        public ActionResult BDAnalysisSummary(int PlantId, int Year, int Month)
        {
            var ReportDataSource = this._reportService.GetBDAnalysisSummaryReportData(PlantId, Year, Month);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\BDAnalysisSummary.rdlc"
            };
            return PartialView("_RdlcReportViewer", model);
        }

      

        public ActionResult RepeatedMajorReport(string Id)
        {
            RepeatedMajorReportModel model = new Models.RepeatedMajorReportModel();
            model.YearData = new List<int>();
            for (int i = 0; i < 15; i++)
                model.YearData.Add(DateTime.Now.Year - i);

            model.MonthData = new List<DropDownData>();
            model.MonthData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            model.ReportDataSource = this._reportService.GetRepeatedMajorReportData(model.Year, model.Month, Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult RepeatedMajorReport(int Year, int Month, string Id)
        {
            var ReportDataSource = this._reportService.GetRepeatedMajorReportData(Year, Month, Id);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\RepeatedMajorReport.rdlc"
            };
            return PartialView("_RdlcReportViewer", model);
        }

        public ActionResult QualityObjectiveYearlySummary()
        {
            QualityObjectiveReportModel model = new QualityObjectiveReportModel();
            model.YearData = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                model.YearData.Add(DateTime.Now.Year - i);
            }
            model.Year = DateTime.Now.Year;
            model.ReportDataSource = this._reportService.GetQOSReportData(model.Year);
            return View(model);
        }

        [HttpPost]
        public ActionResult QualityObjectiveYearlySummary(int Year)
        {
            var ReportDataSource = this._reportService.GetQOSReportData(Year);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "QualityObjectiveDataSet",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\QualityObjectiveYearlySummary.rdlc"
            };
            return PartialView("_RdlcReportViewer", model);
        }
        #endregion

        #region Indent
        public ActionResult IndentBudget()
        {
            IndentBudgetReportModel model = new Models.IndentBudgetReportModel();
            model.YearData = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                model.YearData.Add(DateTime.Now.Year - i);
            }
            model.Year = DateTime.Now.Year;
            model.PlantData = this._plantService.GetAllPlants().ToArray();
            model.PlantId = model.PlantData.FirstOrDefault().Id;
            model.ReportDataSource = this._reportService.GetIssuedIndentDetail(model.PlantId, model.Year);
            return View(model);
        }

        [HttpPost]
        public ActionResult IndentBudget(int PlantId, int Year)
        {
            var ReportDataSource = this._reportService.GetIssuedIndentDetail(PlantId, Year);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\IndentBudgetReport.rdlc"
            };

            return PartialView("_RdlcReportViewer", model);
        }

        public ActionResult ConsolidateReport()
        {
            IndentConsolidateModel model = new IndentConsolidateModel();
            model.YearData = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                model.YearData.Add(DateTime.Now.Year - i);
            }
            model.Year = DateTime.Now.Year;
            model.ReportDataSource = this._reportService.GetConsolidatedIndentData(model.Year);
            return View(model);
        }

        [HttpPost]
        public ActionResult ConsolidateReport(int Year)
        {
            var ReportDataSource = this._reportService.GetConsolidatedIndentData(Year);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ConsolidateReport.rdlc"
            };

            return PartialView("_RdlcReportViewer", model);
        }

        public ActionResult PRSummaryReport()
        {
            PRSummaryReportModel model = new Models.PRSummaryReportModel();
            model.YearData = new List<int>();
            for (int i = 0; i < 15; i++)
                model.YearData.Add(DateTime.Now.Year - i);

            model.MonthData = new List<DropDownData>();
            model.MonthData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            model.ReportDataSource = this._reportService.GetPRReportData(model.Year, model.Month);
            return View(model);
        }
        [HttpPost]
        public ActionResult PRSummaryReport(int year, int month)
        {
            var ReportDataSource = this._reportService.GetPRReportData(year, month);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\PRSummaryReport.rdlc"
            };
            return PartialView("_RdlcReportViewer", model);
        }

        #endregion

        #region Preventive
        public ActionResult PreventiveSummary()
        {
            PreventiveMonthlySummaryReportModel model = new Models.PreventiveMonthlySummaryReportModel();
            model.YearData = new List<int>();
            model.MonthData = new List<Models.DropDownData>();
            model.PlantData = this._plantService.GetAllPlants();

            model.PlantData.Insert(0, new Data.Models.Plant { Id = 0, Name = "-- ALL --" });

            for (int i = 0; i < 15; i++)
                model.YearData.Add(DateTime.Now.Year - i);

            model.MonthData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMMM") });
            }
            model.ScheduleData = new List<Models.DropDownData>();
            model.ScheduleData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            model.ScheduleData.Add(new DropDownData { Id = 1, Name = "Daily" });
            model.ScheduleData.Add(new DropDownData { Id = 2, Name = "Weekly" });
            model.ScheduleData.Add(new DropDownData { Id = 3, Name = "Monthly" });
            model.ScheduleData.Add(new DropDownData { Id = 4, Name = "Yearly" });
            model.ScheduleData.Add(new DropDownData { Id = 5, Name = "Shutdown" });
            model.ScheduleData.Add(new DropDownData { Id = 6, Name = "Hourly" });

            model.PlantId = 0;
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            model.ScheduleType = 0;
            model.ReportDataSource = this._reportService.GetSummaryReportData(model.Year, model.Month, model.PlantId, model.ScheduleType);
            return View(model);
        }

        [HttpPost]
        public ActionResult PreventiveSummary(int Year, int Month, int PlantId, int ScheduleType)
        {
            var ReportDataSource = this._reportService.GetSummaryReportData(Year, Month, PlantId, ScheduleType);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\PreventiveSummaryReport.rdlc"
            };

            return PartialView("_RdlcReportViewer", model);
        }

        public ActionResult ShutdownMonthlySummary()
        {
            ShutdownMonthlySummaryReportModel model = new Models.ShutdownMonthlySummaryReportModel();
            model.YearData = new List<int>();
            model.MonthData = new List<DropDownData>();
            model.PlantData = this._plantService.GetAllPlants();
            model.PlantData.Insert(0, new Data.Models.Plant { Id = 0, Name = "--ALL--" });
            for (int i = 0; i < 15; i++)
                model.YearData.Add(DateTime.Now.Year - i);

            model.MonthData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.PlantId = 0;
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            model.ReportDataSource = this._reportService.GetShutdownSummaryReportData(model.PlantId, model.Year, model.Month);
            return View(model);
        }

        [HttpPost]
        public ActionResult ShutdownMonthlySummary(int plantId, int year, int month)
        {
            var ReportDataSource = this._reportService.GetShutdownSummaryReportData(plantId, year, month);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ShutdownMonthlySummary.rdlc"
            };

            return PartialView("_RdlcReportViewer", model);
        }

        public ActionResult PMAuditReport()
        {
            PreventiveAuditModel model = new PreventiveAuditModel();
            model.YearData = new List<int>();
            model.MonthData = new List<DropDownData>();
            model.StatusData = new List<DropDownData>();
            for (int i = 0; i < 10; i++)
            {
                model.YearData.Add(DateTime.Now.Year - i);
            }
            model.MonthData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.StatusData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            model.StatusData.Add(new DropDownData { Id = 1, Name = "Add" });
            model.StatusData.Add(new DropDownData { Id = 2, Name = "Update" });
            model.StatusData.Add(new DropDownData { Id = 3, Name = "Delete" });
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            model.Status = 0;
            model.ReportDataSource = this._reportService.GetPMAuditReportData(model.Year, model.Month, model.Status);
            return View(model);
        }

        [HttpPost]
        public ActionResult PMAuditReport(int Year, int Month, int Status)
        {
            var ReportDataSource = this._reportService.GetPMAuditReportData(Year, Month, Status);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "PMAuditDataSet",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\PMAuditSummary.rdlc"
            };

            return PartialView("_RdlcReportViewer", model);
        }

        #endregion

        #region MaintenanceRequest

        public ActionResult MaintenanceRequestReport()
        {
            MaintenanceRequestReportModel model = new Models.MaintenanceRequestReportModel();
            model.YearData = new List<int>();
            for (int i = 0; i < 15; i++)
                model.YearData.Add(DateTime.Now.Year - i);

            model.MonthData = new List<DropDownData>();
            model.MonthData.Add(new DropDownData { Id = 0, Name = "-- ALL --" });
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            model.ReportDataSource = this._reportService.GetMaintenanceRequestData(model.Year, model.Month);
            foreach (var item in model.ReportDataSource)
            {
                if (item.WorkEndDate == null)
                    continue;
                var startDateTime = item.RequestDate.Add(item.RequestTime);
                var endDateTime = item.WorkEndDate.Value.Add(item.WorkEndTime.Value);
                var dateDiff = (endDateTime - startDateTime);
                item.TimeSpent = string.Format("{0} Days {1} Hours {2} Mins",(int)dateDiff.TotalDays, dateDiff.Hours, dateDiff.Minutes);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MaintenanceRequestReport(int Year,int Month)
        {
            var ReportDataSource = this._reportService.GetMaintenanceRequestData(Year, Month);
            foreach (var item in ReportDataSource)
            {
                if (item.WorkEndDate == null)
                    continue;
                var startDateTime = item.RequestDate.Add(item.RequestTime);
                var endDateTime = item.WorkEndDate.Value.Add(item.WorkEndTime.Value);
                var dateDiff = (endDateTime - startDateTime);
                item.TimeSpent = string.Format("{0} Days {1} Hours {2} Mins", (int)dateDiff.TotalDays, dateDiff.Hours, dateDiff.Minutes);
            }
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "DataSet1",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\MaintenanceRequestReport.rdlc"
            };
            return PartialView("_RdlcReportViewer", model);
        }

        #endregion

        #region Formulation Request

        public ActionResult FormulationRequestMonthlySummary()
        {
            FormulationMonthlySummaryReportModel model = new FormulationMonthlySummaryReportModel();
            model.YearData = new List<int>();
            model.MonthData = new List<DropDownData>();
            for(int i = 0; i < 15; i++)
            {
                model.YearData.Add(DateTime.Now.Year - i);
            }
            model.MonthData.Add(new DropDownData{ Id = 0 , Name = "-- ALL --" });
            for(int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                model.MonthData.Add(new DropDownData { Id = i, Name = dt.ToString("MMM") });
            }
            model.LineData = this._lineService.GetLineList();
            model.LineData.Insert(0, new Data.Models.Line { Id = 0 ,Name = "-- ALL --" });
            model.Year = DateTime.Now.Year;
            model.Month = DateTime.Now.Month;
            model.LineId = 0;
            model.ReportDataSource = this._reportService.GetFormulationRequestData(model.Year, model.Month, model.LineId);
            return View(model);
        }

        [HttpPost]
        public ActionResult FormulationRequestMonthlySummary(int Year,int Month,int LineId)
        {
            var ReportDataSource = this._reportService.GetFormulationRequestData(Year, Month, LineId);
            ReportViewModels model = new ReportViewModels
            {
                DataSetName = "FormulationMasterDataSet",
                ReportDataSource = ReportDataSource,
                ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\FormulationMonthlySummary.rdlc"
            };
            return PartialView("_RdlcReportViewer", model);
        }

        #endregion

    }
}