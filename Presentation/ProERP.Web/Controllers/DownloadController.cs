using Microsoft.Reporting.WebForms;
using ProERP.Services.Document;
using ProERP.Services.Indent;
using ProERP.Services.PreventiveMaintenance;
using ProERP.Web.Framework;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ClosedXML.Excel;
using System.Data;
using Newtonsoft.Json;
using ProERP.Services.Models;
using ProERP.Services.Utility;
using ProERP.Services.Breakdown;
using ProERP.Services.Plant;
using ProERP.Services.Line;
using ProERP.Services.MaintenanceRequest;
using ProERP.Services.FormulationRequest;
using ProERP.Services.User;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class DownloadController : BaseController
    {
        #region Private Readonly Members

        private readonly IndentsServices _indentService;
        private readonly IndentDetailServices _indentDetailService;
        private readonly IndentBudgetServices _indentBudgetServices;
        private readonly DocumentService _documentService;
        private readonly PreventiveMaintenanceService _pmServices;
        private readonly TemplateService _templateService;
        private readonly UtilityService _utilityService;
        private readonly BreakdownService _breakDownService;
        private readonly PlantService _plantService;
        private readonly LineService _lineService;
        private readonly MaintenanceRequestServices _mrServices;
        private readonly FormulationRequestService _formulationRequestService;
        private readonly UserService _userService;

        #endregion

        #region .Cotr

        public DownloadController(IndentsServices indentService,
            IndentDetailServices indentDetailService,
            IndentBudgetServices indentBudgetServices,
            DocumentService documentService,
            PreventiveMaintenanceService pmServices,
            TemplateService templateService,
            UtilityService utilityService,
            BreakdownService breakDownService,
            PlantService plantService,
            LineService lineService,
            MaintenanceRequestServices mrServices,
            FormulationRequestService formulationRequestService,
            UserService userService)
        {
            this._indentService = indentService;
            this._indentDetailService = indentDetailService;
            this._indentBudgetServices = indentBudgetServices;
            this._documentService = documentService;
            this._pmServices = pmServices;
            this._templateService = templateService;
            this._utilityService = utilityService;
            this._breakDownService = breakDownService;
            this._plantService = plantService;
            this._lineService = lineService;
            this._mrServices = mrServices;
            this._formulationRequestService = formulationRequestService;
            this._userService = userService;
        }

        #endregion

        // GET: Download
        public ActionResult Index()
        {
            return View();
        }

        #region Documents

        [HttpGet]
        public FileResult Documents(string Ids)
        {
            int[] docIds = Ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim())).ToArray();
            var Documents = this._documentService.GetDocuments(docIds);
            if (Documents.Length == 1)
            {
                string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "Documents", Documents[0].RelativePath);
                //var path = ConfigurationManager.AppSettings["IndentAttachmentPath"];

                var fullFilePath = Path.Combine(path, Documents[0].SysFileName);
                return DownloadFile(System.IO.File.ReadAllBytes(fullFilePath), Documents[0].OriginalFileName);
            }
            else
            {
                using (MemoryStream stream = new MemoryStream())
                {

                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
                    {
                        foreach (var document in Documents)
                        {
                            string docPath = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "Documents", document.RelativePath);
                            //var path = ConfigurationManager.AppSettings["IndentAttachmentPath"];
                            var fullDocPath = Path.Combine(docPath, document.SysFileName);
                            archive.CreateEntryFromFile(fullDocPath, document.OriginalFileName);
                        }
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                    return DownloadFile(ReadFully(stream), "MultipleDocs.zip");
                }

            }
            //return File(fullFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, attachment.OriginalFileName);
        }

        #endregion

        #region Preventive Maintenance

        [HttpGet]
        public FileResult PreventiveHistory(DateTime FromDate, DateTime ToDate, int ScheduleType, int PlantId, int LinetId, int Activity,string WorkDescription)//
        {
            //int PlantId = 0, LinetId = 0;

            //int userId = HttpContext.User.Identity.GetUserId<int>();
            //if (HttpContext.User.IsInRole("Level2") || HttpContext.User.IsInRole("Admin"))
            int userId = 0; // Show all report data to all users
            var allData = this._pmServices.GetReport1Data(userId, FromDate, ToDate, ScheduleType, PlantId, LinetId, Activity, WorkDescription);//

            var jsonData = allData.Select(s => new
            {
                s.LineName,
                s.MachineName,
                s.WorkName,
                s.Notes,
                ScheduleType = s.ScheduleTypeName,
                //s.ScheduleType,
                //AssignedTo = string.Join(",", s.AssignedTo.ToArray()),
                ScheduledReviewDate = s.ScheduledReviewDate,
                ReviewBy = s.UserName,
                s.ReviewDate
                //s.NextReviewDate,
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");
            wb.Worksheets.Add(dt, "PreventiveMaintenance");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "Preventive.xlsx");
            }
        }

        [HttpGet]
        public FileResult SummaryReport(DateTime FromDate, DateTime ToDate, int ScheduleType, int PlantId, int LineId)
        {
            //int userId = HttpContext.User.Identity.GetUserId<int>();
            //if (HttpContext.User.IsInRole("Level2") || HttpContext.User.IsInRole("Admin"))
            int userId = 0; // Show all report data to all users
            var allData = this._pmServices.GetSummaryReportData(userId, FromDate, ToDate, ScheduleType, PlantId, LineId);

            var jsonData = from a in allData
                           select new
                           {
                               a.LineName,
                               //a.TotalActivity,
                               TotalModerateActivity = a.Moderate,
                               TotalCriticalActivity = a.Critical,
                               TotalMinorActivity = a.Minor,
                               //a.ReviewedCount,
                               ReviewedModerateActivity = a.ModerateReviewedCount,
                               ReviewedCriticalActivity = a.CriticalReviewedCount,
                               ReviewedMinorActivity = a.ModerateReviewedCount,
                               //a.LapseCount,
                               LapseModerateActivity = a.ModerateLapseCount,
                               LapseCriticalActivity = a.CriticalLapseCount,
                               LapseMinorActivity = a.ModerateLapseCount,
                               a.HoldCount
                           };

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            wb.Worksheets.Add(dt, "PreventiveMaintenance");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "Preventive.xlsx");
            }
        }

        [HttpGet]
        public FileResult PreventiveMaintenanceList(string Name, int PlantId, int LineId, int ScheduleType)
        {
            var allData = this._pmServices.GetPMData(Name, PlantId, LineId, ScheduleType);
            var jsonData = allData.Select(s => new
            {
                s.Id,
                s.MachineId,
                s.Checkpoints,
                //s.ScheduleType,
                s.ShutdownRequired,
                s.ScheduleStartDate,
                s.ScheduleEndDate,
                s.Description,
                MachineName = s.Machine.Name,
                ScheduleTypeName = s.PreventiveScheduleType.Description,
                WorkName = s.WorkDescription,
                Severity = s.Severity == 1 ? "Minor" : (s.Severity == 2) ? "Modrate" : "Critical"
                // PlantName = s.Plant.Name,
                // SiteId = s.Plant.SiteId,
                //SiteName = s.Plant.Site.Name
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            wb.Worksheets.Add(dt, "PreventiveMaintenance");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "PreventiveList.xlsx");
            }
        }

        [HttpGet]
        public FileResult PreventiveActilityList(int PMType)
        {
            int userId = HttpContext.User.Identity.GetUserId<int>();
            var allData = this._pmServices.GetDashboardData(userId, PMType);
            var jsonData = allData.Select(s => new
            {
                s.PlantName,
                s.LineName,
                s.MachineName,
                s.WorkName,
                ReviewDate = s.NextReviewDate,
                s.Checkpoints,
                s.ScheduleTypeName,
                s.Interval,
                s.Severity,
                IsObservation = s.IsObservation ? "Yes" : "No"
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");

            wb.Worksheets.Add(dt, "PreventiveMaintenance");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "PreventiveActilityList.xlsx");
            }
        }

        public FileResult VerifyPreventiveList(int PlantId, int LineId, int MachineId, int ScheduleType, int Verified, DateTime FromDate, DateTime ToDate)
        {
            var allData = this._pmServices.GetVPData(PlantId, LineId, MachineId, ScheduleType, Verified, FromDate, ToDate);
            var jsonData = allData.Select(s => new
            {
                s.PlantName,
                s.LineName,
                s.MachineName,
                s.WorkName,
                ScheduleType = s.ScheduleTypeName,
                s.Severity,
                ReviewDate = s.ReviewDate == null ? "" : s.ReviewDate.Value.ToString("dd/MM/yyyy"),
                s.ReviewBy,
                s.VerifyBy,
                VerifyDate = s.VerifyDate == null ? "" : s.VerifyDate.Value.ToString("dd/MM/yyyy")
            });
            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");

            wb.Worksheets.Add(dt, "VerifyPreventive");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "VerifyPreventiveList.xlsx");
            }
        }

        #endregion

        #region Breakdown

        [HttpGet]
        public FileResult BreakdownList(int siteId, int plantId, int lineId, DateTime fromDate, DateTime toDate, string sortdatafield, string sortorder)
        {
            var allData = this._breakDownService.GetGridData(siteId, plantId, lineId, fromDate, toDate, sortdatafield, sortorder);
            //int id = 0;
            //for (int i = allData.Count; i < 30; i++)
            //{
            //    id -= 1;
            //    allData.Add(new Core.Models.BreakDownGridModel { Id = id, PlantId = plantId, LineId = lineId });
            //}
            var jsonData = allData.Select(s => new
            {
                History = s.IsHistory == true ? "Yes" : "No",
                Repeated = s.IsRepeated == true ? "Yes" : "No",
                Major = s.IsMajor == true ? "Yes" : "No",
                MachineName = s.MachineName,
                //SubAssemblyName = s.SubAssemblyName,
                Date = s.Date,
                StartTime = s.StartTime == null ? "" : s.StartTime.Value.ToString("HH:mm"),
                StopTime = s.StopTime == null ? "" : s.StopTime.Value.ToString("HH:mm"),
                TotalTime = TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm"),
                FailureDescription = s.FailureDescription,
                ElecticalTime = s.ElecticalTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                MechTime = s.MechTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                InstrTime = s.InstrTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                UtilityTime = s.UtilityTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                PowerTime = s.PowerTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                ProcessTime = s.ProcessTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                PrvTime = s.PrvTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                IdleTime = s.IdleTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                RootCause = s.RootCause,
                Correction = s.Correction,
                CorrectiveAction = s.CorrectiveAction,
                PreventingAction = s.PreventingAction,
                Part = s.PartUsed
            }).OrderBy(o => o.Date);
            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");
            wb.Worksheets.Add(dt, "Breakdownlist");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "Breakdownlist.xlsx");
            }
        }

        [HttpGet]
        public FileResult BreakDownSampleFile()
        {
            byte[] byteArray = System.IO.File.ReadAllBytes(Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "SampleFiles", "BreakdownSampleFile.xlsx"));
            return DownloadFile(byteArray, "BreakdownData.xlsx");
        }

        [HttpGet]
        public ActionResult Backup(int PlantId, int Year, int Month)
        {
            var allData = this._breakDownService.GetAllBreakdownData(PlantId, Year, Month);
            string PlantName = "";
            if (PlantId != 0)
            {
                PlantName = this._plantService.GetPlantName(PlantId);
            }
            string MonthName = "";
            BackupViewModel model = new BackupViewModel();
            model.MonthData = new List<Services.Models.DropDownData>();
            if (Month != 0)
            {
                var month = new DateTime(2000, Month, 1);
                MonthName = month.ToString("MMMM");
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    var dt = new DateTime(2000, i, 1);
                    model.MonthData.Add(new Services.Models.DropDownData { Id = i, Name = dt.ToString("MMMM") });
                }
            }

            string Name = "";
            int SiteId = 1;
            var plantData = this._plantService.GetPlantsForSite(SiteId);
            var plants = plantData.Select(s => new { s.Id, s.Name }).OrderBy(o => o.Name).ToArray();
            var lines = this._lineService.GetAll(Name, SiteId, PlantId);
            var line = lines.Select(s => new { s.Id, s.Name }).OrderBy(o => o.Name).ToArray();
            if (PlantId == 0 && Month == 0) // get all plant data by year wise for all month
            {
                using (var masterStrean = new MemoryStream())
                {
                    using (var masterArchive = new ZipArchive(masterStrean, ZipArchiveMode.Create, true))
                    {
                        foreach (var month in model.MonthData)
                        {
                            plants = plants.OrderBy(o => o.Name).ToArray();
                            var finalData = this._breakDownService.GetAllBreakdownData(PlantId, Year, month.Id);
                            for (int i = 0; i < plants.Length; i++)
                            {
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    var fileInArchive = masterArchive.CreateEntry(month.Name + "/" + plants[i].Name + ".xlsx");
                                    using (var entryStream = fileInArchive.Open())
                                    {
                                        using (var fileToCompressStream = new MemoryStream())
                                        {
                                            var linebyPlant = this._lineService.GetAll(Name, SiteId, plants[i].Id);
                                            var lineData = linebyPlant.Select(s => new { s.Id, s.Name }).OrderBy(o => o.Name).ToArray();
                                            if (lineData.Length > 0)
                                            {
                                                for (int j = 0; j < lineData.Length; j++)
                                                {
                                                    var jsonData = finalData.Where(w => w.LineId == lineData[j].Id).Select(s => new
                                                    {
                                                        History = s.IsHistory == true ? "Yes" : "No",
                                                        Repeated = s.IsRepeated == true ? "Yes" : "No",
                                                        Major = s.IsMajor == true ? "Yes" : "No",
                                                        MachineName = s.MachineName,
                                                        Date = s.Date,
                                                        StartTime = s.StartTime == null ? "" : s.StartTime.Value.ToString("HH:mm"),
                                                        StopTime = s.StopTime == null ? "" : s.StopTime.Value.ToString("HH:mm"),
                                                        TotalTime = TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm"),
                                                        FailureDescription = s.FailureDescription,
                                                        ElecticalTime = s.ElecticalTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        MechTime = s.MechTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        InstrTime = s.InstrTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        UtilityTime = s.UtilityTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        PowerTime = s.PowerTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        ProcessTime = s.ProcessTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        PrvTime = s.PrvTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        IdleTime = s.IdleTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                        RootCause = s.RootCause,
                                                        Correction = s.Correction,
                                                        CorrectiveAction = s.CorrectiveAction,
                                                        PreventingAction = s.PreventingAction,
                                                        Part = s.PartUsed
                                                    }).OrderBy(o => o.Date);
                                                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
                                                    if (dt.Columns.Count <= 0)
                                                        dt.Columns.Add("No data found.");
                                                    wb.Worksheets.Add(dt, lineData[j].Name);
                                                }
                                                wb.SaveAs(fileToCompressStream);
                                                fileToCompressStream.Position = 0;
                                                fileToCompressStream.CopyTo(entryStream);
                                            }
                                            else
                                            {
                                                wb.Worksheets.Add(null, plants[i].Name);
                                                wb.SaveAs(fileToCompressStream);
                                                fileToCompressStream.Position = 0;
                                                fileToCompressStream.CopyTo(entryStream);
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    masterStrean.Seek(0, SeekOrigin.Begin);
                    return DownloadFile(ReadFully(masterStrean), "AllMonths-" + Year + ".zip");
                }
            }
            else if (PlantId != 0 && Month == 0) // get yearly data  for specific plants
            {
                using (var masterStrean = new MemoryStream())
                {
                    using (var masterArchive = new ZipArchive(masterStrean, ZipArchiveMode.Create, true))
                    {
                        foreach (var month in model.MonthData)
                        {
                            plants = plants.OrderBy(o => o.Name).ToArray();
                            var finalData = this._breakDownService.GetAllBreakdownData(PlantId, Year, month.Id);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var fileInArchive = masterArchive.CreateEntry(month.Name + "/" + PlantName + ".xlsx");
                                using (var entryStream = fileInArchive.Open())
                                {
                                    using (var fileToCompressStream = new MemoryStream())
                                    {
                                        var linebyPlant = this._lineService.GetAll(Name, SiteId, PlantId);
                                        var lineData = linebyPlant.Select(s => new { s.Id, s.Name }).OrderBy(o => o.Name).ToArray();
                                        if (lineData.Length > 0)
                                        {
                                            for (int j = 0; j < lineData.Length; j++)
                                            {
                                                var jsonData = finalData.Where(w => w.LineId == lineData[j].Id).Select(s => new
                                                {
                                                    History = s.IsHistory == true ? "Yes" : "No",
                                                    Repeated = s.IsRepeated == true ? "Yes" : "No",
                                                    Major = s.IsMajor == true ? "Yes" : "No",
                                                    MachineName = s.MachineName,
                                                    Date = s.Date,
                                                    StartTime = s.StartTime == null ? "" : s.StartTime.Value.ToString("HH:mm"),
                                                    StopTime = s.StopTime == null ? "" : s.StopTime.Value.ToString("HH:mm"),
                                                    TotalTime = TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm"),
                                                    FailureDescription = s.FailureDescription,
                                                    ElecticalTime = s.ElecticalTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    MechTime = s.MechTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    InstrTime = s.InstrTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    UtilityTime = s.UtilityTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    PowerTime = s.PowerTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    ProcessTime = s.ProcessTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    PrvTime = s.PrvTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    IdleTime = s.IdleTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    RootCause = s.RootCause,
                                                    Correction = s.Correction,
                                                    CorrectiveAction = s.CorrectiveAction,
                                                    PreventingAction = s.PreventingAction,
                                                    Part = s.PartUsed
                                                }).OrderBy(o => o.Date);
                                                DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
                                                if (dt.Columns.Count <= 0)
                                                    dt.Columns.Add("No data found.");
                                                wb.Worksheets.Add(dt, lineData[j].Name);
                                            }
                                            wb.SaveAs(fileToCompressStream);
                                            fileToCompressStream.Position = 0;
                                            fileToCompressStream.CopyTo(entryStream);
                                        }
                                        else
                                        {
                                            wb.Worksheets.Add(null, PlantName);
                                            wb.SaveAs(fileToCompressStream);
                                            fileToCompressStream.Position = 0;
                                            fileToCompressStream.CopyTo(entryStream);
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    masterStrean.Seek(0, SeekOrigin.Begin);
                    return DownloadFile(ReadFully(masterStrean), PlantName + "-AllMonths-" + Year + ".zip");
                }
            }
            else if (PlantId == 0) // get all plant data for specific month
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        plants = plants.OrderBy(o => o.Name).ToArray();
                        for (int i = 0; i < plants.Length; i++)
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var fileInArchive = archive.CreateEntry(plants[i].Name + ".xlsx");
                                using (var entryStream = fileInArchive.Open())
                                {
                                    using (var fileToCompressStream = new MemoryStream())
                                    {
                                        var linebyPlant = this._lineService.GetAll(Name, SiteId, plants[i].Id);
                                        var lineData = linebyPlant.Select(s => new { s.Id, s.Name }).OrderBy(o => o.Name).ToArray();
                                        if (lineData.Length > 0)
                                        {
                                            for (int j = 0; j < lineData.Length; j++)
                                            {
                                                var jsonData = allData.Where(w => w.LineId == lineData[j].Id).Select(s => new
                                                {
                                                    History = s.IsHistory == true ? "Yes" : "No",
                                                    Repeated = s.IsRepeated == true ? "Yes" : "No",
                                                    Major = s.IsMajor == true ? "Yes" : "No",
                                                    MachineName = s.MachineName,
                                                    Date = s.Date,
                                                    StartTime = s.StartTime == null ? "" : s.StartTime.Value.ToString("HH:mm"),
                                                    StopTime = s.StopTime == null ? "" : s.StopTime.Value.ToString("HH:mm"),
                                                    TotalTime = TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm"),
                                                    FailureDescription = s.FailureDescription,
                                                    ElecticalTime = s.ElecticalTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    MechTime = s.MechTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    InstrTime = s.InstrTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    UtilityTime = s.UtilityTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    PowerTime = s.PowerTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    ProcessTime = s.ProcessTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    PrvTime = s.PrvTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    IdleTime = s.IdleTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                                    RootCause = s.RootCause,
                                                    Correction = s.Correction,
                                                    CorrectiveAction = s.CorrectiveAction,
                                                    PreventingAction = s.PreventingAction,
                                                    Part = s.PartUsed
                                                }).OrderBy(o => o.Date);
                                                DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
                                                if (dt.Columns.Count <= 0)
                                                    dt.Columns.Add("No data found.");
                                                wb.Worksheets.Add(dt, lineData[j].Name);
                                            }
                                            wb.SaveAs(fileToCompressStream);
                                            fileToCompressStream.Position = 0;
                                            fileToCompressStream.CopyTo(entryStream);
                                        }
                                        else
                                        {
                                            wb.Worksheets.Add(null, plants[i].Name);
                                            wb.SaveAs(fileToCompressStream);
                                            fileToCompressStream.Position = 0;
                                            fileToCompressStream.CopyTo(entryStream);
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return DownloadFile(ReadFully(memoryStream), "AllPlants-" + MonthName + "-" + Year + ".zip");
                }
            }
            else if (PlantId != 0) // breakdown data using plant 
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    line = lines.Select(s => new { s.Id, s.Name }).OrderBy(o => o.Name).ToArray();
                    if (line.Length > 0)
                    {
                        for (int j = 0; j < line.Length; j++)
                        {
                            var jsonData = allData.Where(w => w.LineId == line[j].Id).Select(s => new
                            {
                                History = s.IsHistory == true ? "Yes" : "No",
                                Repeated = s.IsRepeated == true ? "Yes" : "No",
                                Major = s.IsMajor == true ? "Yes" : "No",
                                MachineName = s.MachineName,
                                Date = s.Date,
                                StartTime = s.StartTime == null ? "" : s.StartTime.Value.ToString("HH:mm"),
                                StopTime = s.StopTime == null ? "" : s.StopTime.Value.ToString("HH:mm"),
                                TotalTime = TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm"),
                                FailureDescription = s.FailureDescription,
                                ElecticalTime = s.ElecticalTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                MechTime = s.MechTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                InstrTime = s.InstrTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                UtilityTime = s.UtilityTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                PowerTime = s.PowerTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                ProcessTime = s.ProcessTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                PrvTime = s.PrvTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                IdleTime = s.IdleTime ? TimeSpan.FromMilliseconds(s.TotalTime == null ? 0 : s.TotalTime.Value).ToString(@"hh\:mm") : "00:00",
                                RootCause = s.RootCause,
                                Correction = s.Correction,
                                CorrectiveAction = s.CorrectiveAction,
                                PreventingAction = s.PreventingAction,
                                Part = s.PartUsed
                            }).OrderBy(o => o.Date);
                            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
                            if (dt.Columns.Count <= 0)
                                dt.Columns.Add("No data found.");
                            wb.Worksheets.Add(dt, line[j].Name);
                        }
                        Stream stream = new MemoryStream();
                        wb.SaveAs(stream);
                        stream.Position = 0;
                        return DownloadFile(ReadFully(stream), PlantName + "-" + MonthName + "-" + Year + ".xlsx");
                    }
                    else
                    {
                        return RedirectToAction("Backup", "System");
                    }
                }
            }
            else
            {
                return RedirectToAction("Backup", "System");
            }
            
        }

        #endregion

        #region Indent

        [HttpGet]
        public FileResult Indent(int Id)
        {
            string indentNo = "";
            Byte[] reportBytes = GetReportBytes(Id, out indentNo);
            return DownloadFile(reportBytes, string.Format("{0}.pdf", indentNo));
        }

        [HttpPost]
        public JsonNetResult SendIndentEmail(int indentId)
        {
            JsonResponse response = new Models.JsonResponse();
            try
            {
                string templatePath = Server.MapPath("~/Extras/EmailTemplates/IndentTemplate.html");
                string body = System.IO.File.ReadAllText(templatePath);
                var indent = this._indentService.GetForId(indentId);
                body = body.Replace("{{INDENT_NOTES}}", indent.Note);

                string toEmail = ConfigurationManager.AppSettings["Lavel2EmailAddress"];
                string subject = indent.Subject; //ConfigurationManager.AppSettings["IndentEmailSubject"];
                if (this.User.IsInRole("Lavel2"))
                {
                    toEmail = ConfigurationManager.AppSettings["ManagerEmailAddress"];
                }
                string indentNo = "";
                byte[] reportBytes = GetReportBytes(indentId, out indentNo);

                SendEmail(new string[] { toEmail }, subject, null, body, new Stream[] { new MemoryStream(reportBytes) }, new string[] { string.Format("{0}.pdf", indentNo) });
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message + Environment.NewLine + ex.StackTrace;
            }

            return JsonNet(response, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public FileResult IndentAttachment(int Id)
        {
            var attachment = this._indentDetailService.GetAttachment(Id);
            string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "IndentAttachments");
            //var path = ConfigurationManager.AppSettings["IndentAttachmentPath"];

            var fullFilePath = Path.Combine(path, attachment.SysFileName);
            return DownloadFile(System.IO.File.ReadAllBytes(fullFilePath), attachment.OriginalFileName);
            //return File(fullFilePath, System.Net.Mime.MediaTypeNames.Application.Octet, attachment.OriginalFileName);
        }

        #endregion

        #region Utility

        [HttpPost]
        public JsonNetResult CreateUtilityDataFile(int templateId, SearchViewModel[] searchViewModel, string reportName)
        {
            var template = this._templateService.GetById(templateId);
            var templateMappings = this._templateService.GetTemplateMappings(templateId);
            var result = this._utilityService.GetGridData(templateId, searchViewModel, templateMappings);

            byte[] bytes = ConvertToExcel(result, templateMappings, reportName);
            string folderPath = Path.Combine(System.IO.Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "Temp"));
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string fileName = string.Format("{0}.xlsx", Guid.NewGuid().ToString());
            System.IO.File.WriteAllBytes(Path.Combine(folderPath, fileName), bytes);
            return JsonNet(new { TempFileName = fileName, DownloadFileName = template.Name.Replace(" ", "_") }, JsonRequestBehavior.DenyGet);
            //return DownloadFile(bytes, string.Format("{0}.xlsx", template.Name.Replace(" ", "_")));
        }

        [HttpGet]
        public FileResult UtilityData(string tempFileName, string downloadFileName)
        {
            string folderPath = Path.Combine(System.IO.Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "Temp"));
            byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(folderPath, tempFileName));
            System.IO.File.Delete(Path.Combine(folderPath, tempFileName));
            return DownloadFile(bytes, string.Format("{0}.xlsx", downloadFileName));
        }

        private byte[] ConvertToExcel(IQueryable data, Data.Models.TemplateMapping[] templateMappings, string reportName)
        {
            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data, Formatting.None), (typeof(DataTable)));
            var ws = wb.Worksheets.Add("Sheet1");

            int lastCellRow = 2;
            int headerRows = 2;
            int startCell = 2;
            int startRow = 2;
            bool isGroupsExists = templateMappings.Any(a => !string.IsNullOrEmpty(a.GroupName));
            ws.Cell(lastCellRow, startCell).Value = reportName;
            if (isGroupsExists)
            {
                headerRows++;
                lastCellRow++;
            }

            string lastGrpName = "";
            IXLCell firstCell = null;
            IXLCell lastCell = null;
            IXLRange range = null;
            for (int i = 0; i < templateMappings.Length; i++)
            {
                var item = templateMappings[i];
                //if (!string.IsNullOrEmpty(item.GroupName))
                //{
                if (item.GroupName != lastGrpName || i == templateMappings.Length - 1)
                {
                    if (firstCell != null)
                    {
                        if (!string.IsNullOrEmpty(item.GroupName) && i == templateMappings.Length - 1)
                            lastCell = ws.Cell(lastCellRow, i + startCell);

                        range = ws.Range(firstCell, lastCell);
                        range.Merge();
                        range.Value = lastGrpName; //item.GroupName;
                        firstCell = null;
                        lastCell = null;
                    }
                    lastGrpName = item.GroupName;
                    firstCell = ws.Cell(lastCellRow, i + startCell);
                    lastCell = ws.Cell(lastCellRow, i + startCell);
                }
                else
                    lastCell = ws.Cell(lastCellRow, i + startCell);
                //ws.Cell(lastCellRow, i + startCell).Value = item.GroupName;
                //}

                ws.Cell(lastCellRow + 1, i + startCell).Value = item.ColumnName;
            }

            headerRows++;
            lastCellRow++;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lastCellRow++;
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < templateMappings.Length; j++)
                {
                    var item = templateMappings[j];
                    ws.Cell(lastCellRow, j + startCell).Value = dr[item.SystemColumnName];
                }
            }

            int lastCellColunn = templateMappings.Length + startCell - 1;
            //int lastCellRow = dt.Rows.Count + 3;

            lastCellRow++;
            for (int j = 0; j < templateMappings.Length; j++)
            {
                var item = templateMappings[j];
                if (item.IsAggregate)
                {
                    String aggregateValue = "";
                    //DataTable data1 = dt.AsEnumerable().Where(w=> w.Field<object>(item.SystemColumnName) != null).CopyToDataTable();

                    switch (item.AggregateFunction)
                    {
                        case "SUM":
                            if (dt.AsEnumerable().Where(w => w.Field<object>(item.SystemColumnName) != null).Any())
                                aggregateValue = string.Format("Total: {0}", decimal.Round(dt.AsEnumerable().Where(w => w.Field<object>(item.SystemColumnName) != null).Sum(s => decimal.Parse(s.Field<string>(item.SystemColumnName))), 2, MidpointRounding.AwayFromZero));//string.Format("\"Total: \" & SUM(A1:B2)");
                            else
                                aggregateValue = "Total: 0";
                            break;
                        case "AVG":
                            if (dt.AsEnumerable().Where(w => w.Field<object>(item.SystemColumnName) != null).Any())
                            {
                                var sum = dt.AsEnumerable().Where(w => w.Field<object>(item.SystemColumnName) != null).Sum(s => decimal.Parse(s.Field<string>(item.SystemColumnName)));
                                aggregateValue = string.Format("Average: {0}", decimal.Round((sum / dt.Rows.Count), 2, MidpointRounding.AwayFromZero));//string.Format("\"Total: \" & SUM(A1:B2)");                            
                            }
                            else
                                aggregateValue = "Average: 0";
                            break;
                        case "MAX":
                            if (dt.AsEnumerable().Where(w => w.Field<object>(item.SystemColumnName) != null).Any())
                                aggregateValue = string.Format("Max: {0}", decimal.Round(dt.AsEnumerable().Where(w => w.Field<object>(item.SystemColumnName) != null).Max(s => decimal.Parse(s.Field<string>(item.SystemColumnName))), 2, MidpointRounding.AwayFromZero));//string.Format("\"Total: \" & SUM(A1:B2)");
                            else
                                aggregateValue = "Max: 0";
                            break;
                    }
                    ws.Cell(lastCellRow, j + startCell).Value = aggregateValue;
                }
            }

            var rngTable = ws.Range(startRow, startCell, lastCellRow, lastCellColunn);
            rngTable.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            var rngHeaders = ws.Range(startRow, startCell, headerRows, lastCellColunn); // The address is relative to rngTable (NOT the worksheet)
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.LightGray;
            rngHeaders.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            rngHeaders.Row(1).Merge();
            rngHeaders.Row(1).Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            //var rngFooter = rngTable.Range(lastCellRow, 1, lastCellRow, lastCellColunn); // The address is relative to rngTable (NOT the worksheet)
            //rngFooter.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //rngFooter.Style.Font.Bold = true;

            //rngTable.Cell(1, 1).Style.Font.Bold = true;
            //rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
            //rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //rngTable.Row(1).Merge();
            //rngTable.Range(2, 1, 2, lastCellColunn).SetAutoFilter();
            ws.Columns().AdjustToContents();

            // wb.Worksheets.Add(dt, "PreventiveMaintenance");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return ReadFully(stream);
            }
        }

        #endregion

        #region Maintenance Request
        [HttpGet]
        public FileResult MaintenanceRequest(int Id)
        {
            Byte[] reportBytes = GetMRReportBytes(Id);
            return DownloadFile(reportBytes, string.Format("MaintenanceRequest.pdf"));
        }
        #endregion

        #region Formulation Request
        [HttpGet]
        public FileResult FormulationRequest(int Id, int? VerNo)
        {
            Byte[] reportByte = FormulationRequestReportBytes(Id, VerNo);
            return DownloadFile(reportByte, string.Format("FormulationRequests.pdf"));
        }

        [HttpGet]
        public FileResult FormulationRequestList(int StatusId)
        {
            var allData = this._formulationRequestService.GetRequestDataForDashboardGrid(StatusId);
            var jsonData = allData.Select(s => new
            {
                s.LotNo,
                s.GradeName,
                s.QtyToProduce,
                s.LOTSize,
                s.ColorSTD,
                s.StatusName,
                s.VerifyUser,
                s.QAStatusName,
                s.VerNo
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");

            wb.Worksheets.Add(dt, "FormulationRequest");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "FormulationRequestList.xlsx");
            }
        }

        [HttpGet]
        public FileResult RMRequestList(int StatusId)
        {
            var allData = this._formulationRequestService.GetRMRequestData(StatusId);
            var jsonData = allData.Select(s => new
            {
                s.LotNo,
                s.GradeName,
                s.LOTSize,
                s.QtyToProduce,
                s.ColorSTD,
                s.RMRequestStatus,
                s.RequestBy
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");

            wb.Worksheets.Add(dt, "RawMaterialRequest");
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "RawMaterialRequestList.xlsx");
            }
        }

        public FileResult BatchYieldReport(int Id)
        {
            Byte[] reportByte = BatchYieldReportBytes(Id);
            return DownloadFile(reportByte, string.Format("BatchYieldReport.pdf"));
        }

        public FileResult FormulationCloseReport(int Id)
        {
            Byte[] reportbyte = FormulationCloseRepostBytes(Id);
            return DownloadFile(reportbyte, string.Format("FormulationCloseReport.pdf"));
        }

        public FileResult ColourQASepcsReport(int Id, int? VerNo)
        {
            Byte[] reportbyte = ColourQASpecReportByte(Id, VerNo);
            return DownloadFile(reportbyte, string.Format("TestResultReport.pdf"));
        }

        public FileResult MaterialIssuedReturnReport(int Id)
        {
            Byte[] reportbyte = MaterialIssuedReturnByte(Id);
            return DownloadFile(reportbyte, string.Format("MaterialIssuedReturn.pdf"));
        }

        public FileResult MaterialIssueSlipReport(int RMRequestId)
        {
            Byte[] reportbyte = MaterialIssuedSlipByte(RMRequestId);
            return DownloadFile(reportbyte, string.Format("MaterialIssuedSlip.pdf"));
        }

        [HttpGet]
        public FileResult GetDailyPackingDetailsExcel(DateTime currentDate)
        {
            var allData = this._formulationRequestService.GetDailyPackingReportData(currentDate);

            var jsonData = allData.Select(s => new
            {
                s.LotNo,
                s.GradeName,
                s.PackingDate,
                s.BagFrom,
                s.BagTo,
                s.TotalBags,
                s.Quantity,
                s.ProductionRemarks
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");
            int lastCellRow = 2;
            int headerRows = 2;
            int startCol = 2;

            IXLCell cell = null;
            IXLRange range = null;
            var ws = wb.Worksheets.Add("DailyPackingDatails");
            //var headerStyle = XLWorkbook.DefaultStyle;
            //headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //headerStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            //headerStyle.Font.Bold = true;
            //headerStyle.Fill.BackgroundColor = XLColor.LightGray;
            //headerStyle.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 2, 3, 2);
            range.Merge();
            range.Value = "Lot No";
            //range.Style = headerStyle;
            range.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 3, 3, 3);
            range.Merge();
            range.Value = "Grade";
            //range.Style = headerStyle;
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(3).Width = 30;

            range = ws.Range(2, 4, 3, 4);
            range.Merge();
            range.Value = "Date";
            //range.Style = headerStyle;
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(4).Width=10;

            range = ws.Range(2, 5, 2, 6);
            range.Merge();
            range.Value = "Total Bags";
            //range.Style = headerStyle;
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 5);
            cell.Value = "To";
            //cell.Style = headerStyle;
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 6);
            cell.Value = "From";
            //cell.Style = headerStyle;
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 7, 3, 7);
            range.Merge();
            range.Value = "Total No Bags";
            //range.Style = headerStyle;
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(7).Width = 15;

            range = ws.Range(2, 8, 3, 8);
            range.Merge();
            range.Value = "Quantity";
            //range.Style = headerStyle;
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(8).Width = 10;

            range = ws.Range(2, 9, 3, 9);
            range.Merge();
            range.Value = "Production Remarks";
            //range.Style = headerStyle;
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(9).Width = 25;

            headerRows++;
            lastCellRow++;

            decimal aggregateToSum = 0;
            decimal aggregateFromSum = 0;
            decimal aggregateTotalSum = 0;
            decimal aggregateQuantitySum = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lastCellRow++;
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var item = dt.Columns[j];
                    ws.Cell(lastCellRow, j + startCol).Value = dr[item];
                    if(item.Caption == "BagFrom")
                    {
                        aggregateFromSum += Convert.ToDecimal(dr[item]);
                    }
                    if (item.Caption == "BagTo")
                    {
                        aggregateToSum += Convert.ToDecimal(dr[item]);
                    }
                    if (item.Caption == "TotalBags")
                    {
                        aggregateTotalSum += Convert.ToDecimal(dr[item]);
                    }
                    if (item.Caption == "Quantity")
                    {
                        aggregateQuantitySum += Convert.ToDecimal(dr[item]);
                    }
                }
            }
            ws.Cell(lastCellRow + 1, 2).Value = "Total";
            ws.Cell(lastCellRow + 1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cell(lastCellRow + 1 ,5).Value = aggregateFromSum;
            ws.Cell(lastCellRow + 1, 6).Value = aggregateToSum;
            ws.Cell(lastCellRow + 1, 7).Value = aggregateTotalSum;
            ws.Cell(lastCellRow + 1, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            ws.Cell(lastCellRow + 1, 8).Value = aggregateQuantitySum;
            ws.Cell(lastCellRow + 1, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
            var lastCellColunn = dt.Columns.Count + 1;
            var rngTable = ws.Range(2, 2, lastCellRow + 1, 9);
            rngTable.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            //rngTable.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            var rngHeaders = ws.Range(2, 2, 3, 9);
            rngHeaders.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            rngHeaders.Style.Fill.SetBackgroundColor(XLColor.LightGray);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var rangeContent = ws.Range(4, 2, lastCellRow + 1, 9);
            rangeContent.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            //ws.Columns().AdjustToContents();
            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "DailyPackingData.xlsx");
            }
        }

        [HttpGet]
        public FileResult GetProcessLogSheet1Excel(int LineId, int BatchId, DateTime currentDate)
        {
            var allData = this._formulationRequestService.GetProcessLogSheet1Data(LineId, BatchId, currentDate);

            var jsonData = allData.Select(s => new
            {
                Time = s.Time.ToString("HH:mm"),
                s.TZ1,
                s.TZ2,
                s.TZ3,
                s.TZ4,
                s.TZ5,
                s.TZ6,
                s.TZ7,
                s.TZ8,
                s.TZ9,
                s.TZ10,
                s.TZ11,
                s.TZ12Die,
                s.TM1,
                s.PM1,
                s.PM11,
                s.Vaccumembar
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");
            var ws = wb.Worksheets.Add( "ProcessLogSheet1");
            int lastCellRow = 2;
            int headerRows = 2;
            int startCol = 2;

            IXLCell cell = null;
            IXLRange range = null;

            range = ws.Range(2, 2, 3, 2);
            range.Merge();
            range.Value = "Time";
            range.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 3, 2, 15);
            range.Merge();
            range.Value = "Barrel Temperature Parameters";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 3);
            cell.Value = "TZ1";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 3);
            cell.Value = "TZ1";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 4);
            cell.Value = "TZ2";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 5);
            cell.Value = "TZ3";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 6);
            cell.Value = "TZ4";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 7);
            cell.Value = "TZ5";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 8);
            cell.Value = "TZ6";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 9);
            cell.Value = "TZ7";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 10);
            cell.Value = "TZ8";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 11);
            cell.Value = "TZ9";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 12);
            cell.Value = "TZ10";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 13);
            cell.Value = "TZ11";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 14);
            cell.Value = "TZ12 Die";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 15);
            cell.Value = "TM1";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 16, 2, 17);
            range.Merge();
            range.Value = "Pressure";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 16);
            cell.Value = "PM1";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 17);
            cell.Value = "PM11";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 18, 2, 18);
            range.Merge();
            range.Value = "Vaccume";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 18);
            cell.Value = "Vaccume Bar";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(18).Width = 15;

            headerRows++;
            lastCellRow++;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lastCellRow++;
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var item = dt.Columns[j];
                    ws.Cell(lastCellRow, j + startCol).Value = dr[item];
                }
            }

            var rngTable = ws.Range(2, 2, lastCellRow, 18);
            rngTable.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            var rngHeaders = ws.Range(2, 2, 3, 18);
            rngHeaders.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            rngHeaders.Style.Fill.SetBackgroundColor(XLColor.LightGray);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var rangeContent = ws.Range(4, 2, lastCellRow, 18);
            rangeContent.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "ProcessLogSheet1Data.xlsx");
            }
        }

        [HttpGet]
        public FileResult GetProcessLogSheet2Excel(int LineId, int BatchId, DateTime currentDate)
        {
            var allData = this._formulationRequestService.GetProcessLogSheet2Data(LineId, BatchId, currentDate);

            var jsonData = allData.Select(s => new
            {
                Time = s.Time.ToString("HH:mm"),
                s.RPM,
                s.TORQ,
                s.AMPS,
                s.RPM1,
                s.RPM2,
                s.RPM3,
                s.F1KGHR,
                s.F1Perc,
                s.F2KGHR,
                s.F2Perc,
                s.F3KGHR,
                s.F3Perc,
                s.F4KGHR,
                s.F4Perc,
                s.F5KGHR,
                s.F5Perc,
                s.F6KGHR,
                s.F6Perc,
                s.Output,
                s.NoofDiesHoles,
                s.Remarks
            });

            XLWorkbook wb = new XLWorkbook();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(jsonData, Formatting.None), (typeof(DataTable)));
            if (dt.Columns.Count <= 0)
                dt.Columns.Add("No data found.");
            var ws = wb.Worksheets.Add("ProcessLogSheet2");
            int lastCellRow = 2;
            int headerRows = 2;
            int startCol = 2;

            IXLCell cell = null;
            IXLRange range = null;

            range = ws.Range(2, 2, 3, 2);
            range.Merge();
            range.Value = "Time";
            range.Style.Border.SetLeftBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 3, 2, 5);
            range.Merge();
            range.Value = "Extrunder Parameter";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 3);
            cell.Value = "RPM";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 4);
            cell.Value = "TORQ";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 5);
            cell.Value = "AMPS";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 6, 2, 7);
            range.Merge();
            range.Value = "Side Feeder";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 6);
            cell.Value = "RPM";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 7);
            cell.Value = "RPM";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 8, 2, 8);
            range.Merge();
            range.Value = "Peletiser";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 8);
            cell.Value = "RPM";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 9, 2, 20);
            range.Merge();
            range.Value = "Feed Rate";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 9);
            cell.Value = "F1 KG/Hr";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 10);
            cell.Value = "F1 %";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 11);
            cell.Value = "F2 KG/Hr";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 12);
            cell.Value = "F2 %";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 13);
            cell.Value = "F3 KG/Hr";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 14);
            cell.Value = "F3 %";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 15);
            cell.Value = "F4 KG/Hr";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 16);
            cell.Value = "F4 %";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 17);
            cell.Value = "F5 KG/Hr";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 18);
            cell.Value = "F5 %";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 19);
            cell.Value = "F6 KG/Hr";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            cell = ws.Cell(3, 20);
            cell.Value = "F6 %";
            cell.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);

            range = ws.Range(2, 21, 3, 21);
            range.Merge();
            range.Value = "Total Output";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(21).Width = 12;

            range = ws.Range(2, 22, 3, 22);
            range.Merge();
            range.Value = "No of Dies Holes";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(22).Width = 15;

            range = ws.Range(2, 23, 3, 23);
            range.Merge();
            range.Value = "Remarks";
            range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            ws.Column(23).Width = 30;

            headerRows++;
            lastCellRow++;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lastCellRow++;
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var item = dt.Columns[j];
                    ws.Cell(lastCellRow, j + startCol).Value = dr[item];
                }
            }

            var rngTable = ws.Range(2, 2, lastCellRow , 23);
            rngTable.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            var rngHeaders = ws.Range(2, 2, 3, 23);
            rngHeaders.Style.Border.SetOutsideBorder(XLBorderStyleValues.Medium);
            rngHeaders.Style.Fill.SetBackgroundColor(XLColor.LightGray);
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            var rangeContent = ws.Range(4, 2, lastCellRow , 23);
            rangeContent.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

            using (Stream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                stream.Position = 0;
                return DownloadFile(ReadFully(stream), "ProcessLogSheet2Data.xlsx");
            }
        }

        #endregion

        #region Common Functions

        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        protected byte[] GetReportBytes(int Id, out string indentNo)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            var indent = this._indentService.GetForId(Id);
            string type = indent.RequisitionType;
            var remainingBudget = this._indentBudgetServices.GetRemainingBudget(indent.BudgetId);
            List<Services.Models.IndentReportDataSet> dataSource = this._indentDetailService.GetIndentDetailById(Id);
            byte[] bytes = null;
            indentNo = "";
            if (dataSource.Count > 0)
            {
                using (ReportViewer ReportViewer1 = new ReportViewer())
                {
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dataSource));

                    if (type == "PR")
                    {
                        ReportViewer1.LocalReport.ReportPath = "Reports/IndentFormat.rdlc";
                        ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("remainingBudget", remainingBudget.ToString()));

                        bytes = ReportViewer1.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);
                    }
                    else
                    {
                        ReportViewer1.LocalReport.ReportPath = "Reports/IndentFormatJR.rdlc";
                        ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("remainingBudget", remainingBudget.ToString()));

                        bytes = ReportViewer1.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);
                    }
                }
                indentNo = dataSource[0].IndentNo.Replace("/", "-");
            }
            return bytes;
        }

        protected byte[] GetMRReportBytes(int Id)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            bool breakdown = this._mrServices.BreakdownStatus(Id);
            List<Services.Models.MaintenanceRequestReportDataSet> dataSource = this._mrServices.GetMaintenanceRequestById(Id);

            foreach (var item in dataSource)
            {
                if (item.WorkEndDate == null)
                    continue;
                var startDateTime = item.RequestDate.Add(item.RequestTime);
                var endDateTime = item.WorkEndDate.Value.Add(item.WorkEndTime.Value);
                var dateDiff = (endDateTime - startDateTime);
                item.TimeSpent = string.Format("{0} Days {1} Hours {2} Mins", (int)dateDiff.TotalDays, dateDiff.Hours, dateDiff.Minutes);
            }

            byte[] bytes = null;
            if (dataSource.Count > 0)
            {
                using (ReportViewer ReportViewer1 = new ReportViewer())
                {
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dataSource));
                    ReportViewer1.LocalReport.ReportPath = "Reports/MaintenanceRequest.rdlc";
                    ReportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("IsBreakdown", breakdown.ToString()));
                    bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            return bytes;
        }

        protected byte[] FormulationRequestReportBytes(int Id, int? VerNo)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            int statusId = this._formulationRequestService.GetFormulationStatusById(Id);
            var formulationData = this._formulationRequestService.GetFormulationReportById(Id);
            if (VerNo == null)
            {
                VerNo = formulationData.VerNo;
            }
            List<Services.Models.FormulationRequestReportViewModel> dataSource = this._formulationRequestService.GetFormulationReportDataById(Id, VerNo);
            List<Services.Models.MachineDetailViewModel> machineSource = this._formulationRequestService.GetMachineForReport(Id, VerNo);
            List<Services.Models.ColourQASpecsReportViewModel> colourqaSource = this._formulationRequestService.GetColourQAReportByFormulationId(Id, VerNo);
            List<Services.Models.FormulationRequestReportDataModel> masterData = this._formulationRequestService.GetFormulationMasterDataById(Id);

            var rmData = this._formulationRequestService.GetRMReportData(Id);
            byte[] bytes = null;
            if (dataSource.Count > 0 && machineSource.Count > 0)
            {
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FormulationDataSet", dataSource));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MachineDataSet", machineSource));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ColourQADataSet", colourqaSource));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FormulationMasterDataSet", masterData));
                    reportViewer1.LocalReport.ReportPath = "Reports/FormulationRequestMachineReport.rdlc";
                    bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            else
            {
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FormulationDataSet", dataSource));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FormulationMasterDataSet", masterData));
                    reportViewer1.LocalReport.ReportPath = "Reports/FormulationRequestReport.rdlc";
                    bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            return bytes;
        }

        protected byte[] BatchYieldReportBytes(int Id)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            int statusId = this._formulationRequestService.GetFormulationStatusById(Id);
            List<Services.Models.BatchYieldReportViewModel> dataSource = this._formulationRequestService.GetBatchYieldReportById(Id);
            List<Services.Models.MachineReadingViewModel> readingSource = this._formulationRequestService.GetMachineReadingDataForReport(Id);
            var formulationData = this._formulationRequestService.GetFormulationBatchReportDataById(Id);
            var batchData = this._formulationRequestService.GetBatchDataById(Id);
            var userData = this._userService.GetAllUserForRemarks();
            var userName = string.Join(", ", userData.Where(w => w.Id == formulationData.VerifyBy).Select(q => q.UserName));
            if (userName == "")
            {
                userName = "N A";
            }
            byte[] bytes = null;
            if (dataSource.Count > 0 && readingSource.Count > 0)
            {
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("BatchYieldDataset", dataSource));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MachineReadingDataset", readingSource));
                    reportViewer1.LocalReport.ReportPath = "Reports/BatchYieldReport.rdlc";
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("GradeName", formulationData.GradeName));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("LotNo", formulationData.LotNo));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("QtyToProduce", formulationData.QtyToProduce.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("BatchStartDate", formulationData.ProgressOn.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("BatchEndDate", formulationData.CloseOn.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("FGPackedQty", batchData.FGPackedQty.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("QCRejected", batchData.QCRejected.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("StartUpTrials", batchData.StartUpTrials.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("MixMaterial", batchData.MixMaterial.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("NSP", batchData.NSP.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Lumps", batchData.Lumps.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("LongsandFines", batchData.LongsandFines.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("LabSample", batchData.LabSample.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Sweepaged", batchData.Sweepaged.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Additives", batchData.Additives.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("PackingBags", batchData.PackingBags.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("VerifiedBy", userName));
                    bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            return bytes;
        }

        protected byte[] FormulationCloseRepostBytes(int Id)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            int VerNo = 0;
            int statusId = this._formulationRequestService.GetFormulationStatusById(Id);
            List<Services.Models.MachineDetailViewModel> machineSource = this._formulationRequestService.GetMachineForReport(Id, VerNo);
            List<Services.Models.FormulationCloseReportModel> dataSource = this._formulationRequestService.GetFormulationCloseReportById(Id);
            byte[] bytes = null;
            if (dataSource.Count > 0)
            {
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("FormulationCloseDataset", dataSource));
                    reportViewer1.LocalReport.ReportPath = "Reports/FormulationCloseReport.rdlc";
                    bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            return bytes;
        }

        protected byte[] ColourQASpecReportByte(int Id, int? VerNo)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            List<Services.Models.ColourQASpecsReportViewModel> dataSource = this._formulationRequestService.GetColourQAReportByFormulationId(Id, VerNo);
            List<Services.Models.MachineDetailViewModel> machineSource = this._formulationRequestService.GetMachineForReport(Id, VerNo);
            byte[] bytes = null;
            if (dataSource.Count > 0)
            {
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ColourQASpecDS", dataSource));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ColourQAMachineDS", machineSource));
                    reportViewer1.LocalReport.ReportPath = "Reports/TestResultReport.rdlc";
                    bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            return bytes;
        }

        protected byte[] MaterialIssuedReturnByte(int Id)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            List<Services.Models.MaterialIssueReturnReportViewModel> dataSource = this._formulationRequestService.GetMaterialReportData(Id);
            var rmData = this._formulationRequestService.GetRMDataForReport(Id);
            var plantname = this._formulationRequestService.GetPlantNameForMaterialIssueReturn(Id);
            if (plantname == null)
            {
                plantname = " ";
            }
            var userData = this._userService.GetAllUserForRemarks();
            var userName = string.Join(", ", userData.Where(w => w.Id == rmData.ReceviedBy).Select(q => q.UserName));
            if (userName == "")
            {
                userName = " ";
            }
            byte[] bytes = null;
            if (dataSource.Count > 0)
            {
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MaterialIssuedReturnDataSet", dataSource));
                    reportViewer1.LocalReport.ReportPath = "Reports/MaterialIssuedReturnReport.rdlc";
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DispatchDate", rmData.DispatchDate.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("RequestDate", rmData.RequestDate.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ReceviedDate", rmData.ReceviedDate.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("PlantName", plantname));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ReceivedBy", userName));
                    
                    bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            return bytes;
        }

        protected byte[] MaterialIssuedSlipByte(int RMRequestId)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            List<Services.Models.MaterialIssueReturnReportViewModel> dataSource = this._formulationRequestService.GetMaterialIssueReportData(RMRequestId);
            string plantName = this._formulationRequestService.GetPlantNameForMaterialSlip(RMRequestId);
            if (plantName == null)
            {
                plantName = " ";
            }
            var rmMasterData = this._formulationRequestService.GetRMDataForMaterialSlip(RMRequestId);
            var userData = this._userService.GetAllUserForRemarks();
            var userName = string.Join(", ", userData.Where(w => w.Id == rmMasterData.ReceviedBy).Select(q => q.UserName));
            if (userName == "")
            {
                userName = " ";
            }
            byte[] bytes = null;
            if (dataSource.Count > 0)
            {
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    reportViewer1.ProcessingMode = ProcessingMode.Local;
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MaterialIssueDataSet", dataSource));
                    reportViewer1.LocalReport.ReportPath = "Reports/MaterialIssueSlip.rdlc";
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("PlantName", plantName));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("SlipNo", " "));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Purpose", " "));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Location", " "));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Date", rmMasterData.RequestDate.ToString()));
                    reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("ReceivedBy", userName));
                    
                    bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }
            }
            return bytes;
        }

        protected FileResult DownloadFile(byte[] bytes, string FileName)
        {
            string fileExtension = Path.GetExtension(FileName);
            var type = System.Net.Mime.MediaTypeNames.Application.Octet;

            // set known types based on file extension  
            if (fileExtension != null)
            {
                switch (fileExtension.ToLower())
                {
                    case ".csv":
                        type = System.Net.Mime.MediaTypeNames.Text.Plain;
                        break;
                    case ".txt":
                        type = System.Net.Mime.MediaTypeNames.Text.Plain;
                        break;
                    case ".pdf":
                        type = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        break;
                    case ".doc":
                    case ".rtf":
                        type = System.Net.Mime.MediaTypeNames.Application.Rtf;
                        break;
                    case ".xml":
                    case ".xsd":
                        type = System.Net.Mime.MediaTypeNames.Text.Xml;
                        break;
                    case ".xls":
                    case ".xlsx":
                        type = System.Net.Mime.MediaTypeNames.Application.Octet;
                        break;
                    default:
                        type = System.Net.Mime.MediaTypeNames.Application.Octet;
                        break;
                }
            }
            return File(bytes, type, FileName);
        }

        #endregion
    }
}