using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ProERP.Core.Models;
using ProERP.Data.Models;
using ProERP.Services.Breakdown;
using ProERP.Services.Line;
using ProERP.Services.Machine;
using ProERP.Services.Models;
using ProERP.Services.Part;
using ProERP.Services.Plant;
using ProERP.Services.PreventiveMaintenance;
using ProERP.Services.Site;
//using ProERP.Services.SubAssembly;
using ProERP.Web.Framework;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ClosedXML.Excel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Text;
using ProERP.Core;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class BreakdownController : BaseController
    {
        //private readonly ApplicationUserManager _userManager;
        private readonly SiteService _siteService;
        private readonly PlantService _plantService;
        private readonly LineService _lineService;
        private readonly MachineService _machineService;
        //private readonly SubAssemblyService _subAssemblyService;
        private readonly BreakdownService _breakDownService;
        private readonly VendorService _vendorServices;
        private readonly PartService _partServices;
        private readonly EmployeeTypeService _etServices;
        private readonly BreakDownAttachmentServices _breakdownattachmentServices;

        public BreakdownController(/*ApplicationUserManager userManager,*/ SiteService siteService, PlantService plantService, LineService lineService, MachineService machineService,
            BreakdownService breakDownService, VendorService vendorServices, PartService partServices,
            EmployeeTypeService etServices, BreakDownAttachmentServices breakdownattachmentServices)
        {
            this._siteService = siteService;
            this._plantService = plantService;
            this._lineService = lineService;
            this._machineService = machineService;
            //this._subAssemblyService = subAssemblyService;
            this._breakDownService = breakDownService;
            this._vendorServices = vendorServices;
            //this._userManager = userManager;
            this._partServices = partServices;
            this._etServices = etServices;
            this._breakdownattachmentServices = breakdownattachmentServices;
        }

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        // GET: Breakdown
        public ActionResult List()
        {
            BreakdownViewModel model = new BreakdownViewModel();
            List<Site> allSites = this._siteService.GetAll("");
            Plant[] allPlants = this._plantService.GetPlantsForSite(allSites.Select(s => s.Id).FirstOrDefault());
            model.SiteList = allSites; // allSites.Select(s=>new Site{ Id=s.Id, Name=s.Name }).ToArray();
            model.PlantList = allPlants; // allPlants.Select(s => new PlantModel { Id = s.Id, Name = s.Name }).ToArray();
            return View(model);
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonNetResult GetPlants(int siteId)
        {
            Plant[] allPlants = this._plantService.GetPlantsForSite(siteId);
            return JsonNet(allPlants.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Update()
        {
            return View();
        }

        public JsonNetResult GetGridData(int siteId, int plantId, int lineId, DateTime fromDate, DateTime toDate, string sortdatafield, string sortorder)
        {
            var allData = this._breakDownService.GetGridData(siteId, plantId, lineId, fromDate, toDate, sortdatafield, sortorder);
            int id = 0;
            for (int i = allData.Count; i < 30; i++)
            {
                id -= 1;
                allData.Add(new BreakDownGridModel { Id = id, PlantId = plantId, LineId = lineId, IdleTime = true });
            }
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetLineListForPlant(int plantId, DateTime fromDate, DateTime toDate)
        {
            var allLines = this._lineService.GetLinesForPlant(plantId);
            var allMachines = this._machineService.GetAllLineMachineforPlant(plantId);
            var allSubAssemblies = this._machineService.GetSubAssembliesForPlant(plantId);
            return JsonNet(new
            {
                LineData = allLines.Select(s => new { s.Id, s.Name }),
                MachineData = allMachines.Select(s => new { LineId = s.LineId, MachineId = s.Id, MachineName = s.Name }),
                SubAssemblyData = allSubAssemblies.Select(s => new { LineId = s.LineId, SubAssemblyId = s.Id, SubAssemblyName = s.Name })
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetMachineListForLine(int lineId)
        {
            var allMachines = this._machineService.GetMachinesForLine(lineId);
            return JsonNet(new
            {
                MachineData = allMachines.Select(s => new { LineId = s.LineId, MachineId = s.Id, MachineName = s.Name }),
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetMachinesForLine(int lineId)
        {
            var allMachines = this._machineService.GetAllForLine(lineId);
            return JsonNet(allMachines.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveBreakdownData(BreakDownGridModel[] breakDownData, BreakDownServiceGridModel[] ServiceData, BreakDownManPowerGridModel[] MenPowerData
            , BreakDownPartGridModel[] PartData, BreakDownAttachmentGridModel[] AttachmentData, int[] DeletedServiceIds, int[] DeletedMenPowerIds, int[] DeletedPartIds, int[] DeletedAttIds)
        {
            JsonResponse response = null;
            try
            {
                response = new JsonResponse();
                this._breakDownService.SaveBreakdownData(breakDownData, ServiceData, MenPowerData, PartData, AttachmentData, DeletedServiceIds, DeletedMenPowerIds, DeletedPartIds, DeletedAttIds, HttpContext.User.Identity.GetUserId<int>());
                response.Status = JsonResponseStatus.Success;
                response.Message = "Data saved successfully.";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Lavel1,Lavel2")]
        public JsonNetResult DeleteBreakdownData(int[] Ids)
        {
            DateTime deletedon = DateTime.UtcNow;
            int userId = HttpContext.User.Identity.GetUserId<int>();
            this._breakDownService.DeleteBreakdownData(Ids, deletedon, userId);
            return JsonNet(new { Status = 1, Message = "Data deleted successfully." }, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetVendorCategoryList()
        {
            var allData = this._vendorServices.GetVendorCategoryList();
            var v = allData.Select(s => new
            {
                s.VendorId,
                s.CategoryName,
                s.VendorName

            });
            return JsonNet(v, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetPartList()
        {
            var alldata = this._partServices.GetAllPart();
            var Part = alldata.Select(m => new { m.Id, m.Name });
            return JsonNet(Part, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetEmployeeTypeList()
        {
            var alldata = this._etServices.GetAllEmployeeType();
            var ET = alldata.Select(m => new { m.Id, m.Type });
            return JsonNet(ET, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetSparesData(int breakDownId)
        {
            Data.Models.BreakDownService[] objService = this._breakDownService.GetForId(breakDownId);
            var ServiceData = objService.Select(s => new BreakDownService { Id = s.Id, BreakDownId = s.BreakDownId, Cost = s.Cost, VendorName = s.VendorName, Comments = s.Comments });
            var objMenPower = this._breakDownService.GetForMenPowerId(breakDownId);
            var MenPowerData = objMenPower.Select(s => new { Id = s.Id, BreakDownId = s.BreakDownId, Name = s.Name, EmployeeTypeId = s.EmployeeTypeId, EmployeeType = s.EmployeeType.Type, IsOverTime = s.IsOverTime, HourlyRate = s.HourlyRate, Comments = s.Comments });
            var objPart = this._breakDownService.GetForPartId(breakDownId);
            var PartData = objPart.Select(s => new { Id = s.Id, BreakDownId = s.BreakDownId, PartId = s.PartId, PartName = s.Part.Name, Comments = s.Comments , Quantity = s.Quantity });
            var objAttachment = this._breakDownService.GetForAttachmentId(breakDownId);
            var AttachmentData = objAttachment.Select(s => new { Id = s.Id, BreakDownId = s.BreakDownId, OriginalFileName = s.OriginalFileName });

            return JsonNet(new { Data = new { ServiceData = ServiceData, MenPowerData = MenPowerData, PartData = PartData, AttachmentData = AttachmentData } }, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult SavePartData(string Name, string Description)
        {
            try
            {
                int Id = this._partServices.Add(new Part { Name = Name, Description = Description });
                var alldata = this._partServices.GetAllPart();
                var Part = alldata.Select(m => new { m.Id, m.Name });
                return JsonNet(new { Type = "Success", Message = "", Data = new { Id = Id, Name = Name } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonNet(new { Type = "Error", Message = ex.Message + ex.InnerException ?? ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //BreakDown Attachments
        public ActionResult UploadBreakdownAttachments(HttpPostedFileBase[] fileToUpload)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "BreakdownAttachments");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            List<Data.Models.BreakDownAttachment> attachments = new List<BreakDownAttachment>();
            foreach (HttpPostedFileBase File in fileToUpload)
            {
                FileInfo fi = new FileInfo(File.FileName);
                string sysFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
                var filePath = Path.Combine(path, sysFileName);
                File.SaveAs(filePath);
                attachments.Add(new BreakDownAttachment { OriginalFileName = fi.Name, SysFileName = sysFileName });
            }

            ViewBag.Attachments = JsonConvert.SerializeObject(attachments.Select(s => new { s.OriginalFileName, s.SysFileName }));
            return PartialView();
        }

        public ActionResult GetImageData(int Id)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                var attachment = this._breakdownattachmentServices.GetById(Id);
                var path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "BreakdownAttachments");
                var filePath = Path.Combine(path, attachment.SysFileName);
                string fileExtension = Path.GetExtension(attachment.SysFileName);
                if(fileExtension != null && fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".jpeg" || fileExtension.ToLower() == ".png")
                {
                    using (Image image = ScaleImage(Image.FromFile(filePath), 700, 600))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            image.Save(stream, ImageFormat.Jpeg);
                            byte[] imageBytes = stream.ToArray();

                            // Convert byte[] to Base64 String
                            string base64String = Convert.ToBase64String(imageBytes);
                            response.Data = base64String;
                        }
                    }
                }
                if (fileExtension != null && (fileExtension.ToLower() == ".txt" || fileExtension.ToLower() == ".pdf" ||
                                              fileExtension.ToLower() == ".xls" || fileExtension.ToLower() == ".xlsx"))
                {
                    var fullFilePath = Path.Combine(path, attachment.SysFileName);
                    return DownloadFile(System.IO.File.ReadAllBytes(fullFilePath), attachment.OriginalFileName);
                }
                //byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
                //string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                //response.Data = base64ImageRepresentation;
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        protected Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = 1.0;
            double ratioY = 1.0;

            if (image.Width > maxWidth)
                ratioX = (double)maxWidth / image.Width;
            if (image.Height > maxHeight)
                ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        [HttpGet]
        public FileResult DownloadBreakDownAttachmentFile(int Id)
        {
            var attachment = this._breakdownattachmentServices.GetById(Id);
            var path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "BreakdownAttachments");
            var filePath = Path.Combine(path, attachment.SysFileName);
            string fileExtension = Path.GetExtension(attachment.SysFileName);
            var fullFilePath = Path.Combine(path, attachment.SysFileName);
            return DownloadFile(System.IO.File.ReadAllBytes(fullFilePath), attachment.OriginalFileName);
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

        public JsonNetResult SaveAttachments(int BreakDownId, string OriginalFileName, string SysFileName)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                this._breakdownattachmentServices.Add(new BreakDownAttachment { BreakDownId = BreakDownId, OriginalFileName = OriginalFileName, SysFileName = SysFileName });
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ex.Message + ex.InnerException ?? ex.InnerException.Message;
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetAttachmentGridList(int BreakDownId)
        {
            var allData = this._breakdownattachmentServices.GetAttachmentData(BreakDownId);
            var IDAttachment = allData.Select(s => new
            {
                s.Id,
                s.BreakDownId,
                s.OriginalFileName,
                s.SysFileName
            });
            return JsonNet(IDAttachment, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteAttachment(int[] Ids)
        {
            this._breakdownattachmentServices.Delete(Ids);
            return Json(JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SingleFileUpload(HttpPostedFileBase[] SingleFileUpload)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "BreakdownExcel");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            List<BreakDownUploadModel> files = new List<BreakDownUploadModel>();
            foreach (HttpPostedFileBase File in SingleFileUpload)
            {
                FileInfo fi = new FileInfo(File.FileName);
                string sysFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
                var filePath = Path.Combine(path, sysFileName);
                File.SaveAs(filePath);
                files.Add(new BreakDownUploadModel { OriginalFileName = fi.Name, SystemFileName = sysFileName, });
            }

            ViewBag.SingleFileData = JsonConvert.SerializeObject(files.Select(s => new { s.OriginalFileName, s.SystemFileName }));
            return PartialView();
        }

        [HttpPost]
        public JsonNetResult SaveExcelFile(BreakDownUploadModel[] UploadFiles, int PlantId, int LineId)
        {
            var result = new JsonResponse();
            try
            {
                string relativePath = Path.Combine("");
                string sourcePath = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "BreakdownExcel");
                string destinationPath = Path.Combine(sourcePath, relativePath);
                StringBuilder sb = new StringBuilder();

                //foreach (var uploadFile in UploadFiles)
                var uploadFile = UploadFiles[0];
                {
                    var sourceFileName = Path.Combine(sourcePath, uploadFile.SystemFileName);
                    var destFileName = Path.Combine(destinationPath, uploadFile.SystemFileName);
                    System.IO.File.Move(sourceFileName, destFileName);

                    DateTime uploaddate = DateTime.UtcNow;
                    int userId = HttpContext.User.Identity.GetUserId<int>();

                    var breakDownUploadHistory = new BreakDownUploadHistory
                    {
                        UploadDate = uploaddate,
                        UploadBy = userId,
                        OriginalFileName = uploadFile.OriginalFileName,
                        SystemFileName = uploadFile.SystemFileName,
                        BreakDowns = new List<BreakDown>()
                    };
                    int machineid = this._machineService.GetMachineIdForAll(LineId);

                    ClosedXML.Excel.XLWorkbook wb = new XLWorkbook(destFileName);

                    using (XLWorkbook wbook = new XLWorkbook(destFileName))
                    {
                        IXLWorksheet worksheet = wbook.Worksheet(1);

                        var rows = worksheet.Rows().ToArray();
                        for (int i = 1; i < rows.Length; i++)
                        {
                            try
                            {
                                var row = rows[i];
                                //foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                //{ }
                                //var cells = row.Cells().ToArray();

                                //var cells = row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber).ToArray();
                                var cells = row.Cells(1, 21).ToArray();
                                if (cells.Count() <= 20)
                                    continue;

                                var isHistory = Convert.ToString(cells[0].Value).Equals("Yes", StringComparison.OrdinalIgnoreCase);
                                var isRepeated = Convert.ToString(cells[1].Value).Equals("Yes", StringComparison.OrdinalIgnoreCase);
                                var isMajor = Convert.ToString(cells[2].Value).Equals("Yes", StringComparison.OrdinalIgnoreCase);
                                var Date = DateTime.MinValue;
                                var startDateTime = DateTime.MinValue; // cells[2].Value.CastTo<DateTime>().TimeOfDay;
                                var endDateTime = DateTime.MinValue; //cells[3].Value.CastTo<DateTime>().TimeOfDay;

                                var isDate = DateTime.TryParse(Convert.ToString(cells[3].Value), out Date);
                                var isStartTime = DateTime.TryParse(Convert.ToString(cells[4].Value), out startDateTime);
                                var isEndTime = DateTime.TryParse(Convert.ToString(cells[5].Value), out endDateTime);
                                if (!isDate || !isStartTime || !isEndTime)
                                {
                                    if (!isDate)
                                        sb.AppendLine(string.Format("Row {0} Not saved. Message: Date is empty or format is not correct.", i));
                                    if (!isStartTime)
                                        sb.AppendLine(string.Format("Row {0} Not saved. Message: Start time is empty or format is not correct.", i));
                                    if (!isEndTime)
                                        sb.AppendLine(string.Format("Row {0} Not saved. Message: End time is empty or format is not correct.", i));

                                    continue;
                                }
                                var startTime = startDateTime.TimeOfDay;
                                var endTime = endDateTime.TimeOfDay;

                                var totalTime = (endTime - startTime).TotalMilliseconds;
                                var zeroTime = new TimeSpan(0, 0, 0);

                                var isElec = !cells[8].Value.Equals(string.Empty) && !cells[8].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);
                                var isMech = !cells[9].Value.Equals(string.Empty) && !cells[9].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);
                                var isInst = !cells[10].Value.Equals(string.Empty) && !cells[10].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);
                                var isUtility = !cells[11].Value.Equals(string.Empty) && !cells[11].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);
                                var isPower = !cells[12].Value.Equals(string.Empty) && !cells[12].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);
                                var isProcess = !cells[13].Value.Equals(string.Empty) && !cells[13].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);
                                var isPrv = !cells[14].Value.Equals(string.Empty) && !cells[14].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);
                                var isIdle = !cells[15].Value.Equals(string.Empty) && !cells[15].Value.CastTo<DateTime>().TimeOfDay.Equals(zeroTime);

                                var partName = Convert.ToString(cells[20].Value); // string.IsNullOrEmpty(Convert.ToString(cells[20].Value)) ? null : Convert.ToString(cells[20].Value);
                                List<BreakDownSpare> spares = new List<BreakDownSpare>();
                                if (!string.IsNullOrEmpty(partName))
                                {
                                    string[] partList = partName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var item in partList)
                                    {
                                            var partId = this._partServices.partIdForbreakdown(item.Trim());  // Get this from db. If not exists add new and get new id
                                            spares.Add(new BreakDownSpare { PartId = partId });
                                    }
                                }

                                if (isElec == false && isMech == false && isInst == false && isUtility == false && isPower == false && isProcess == false && isPrv == false && isIdle == false)
                                {
                                    sb.AppendLine(string.Format("Row {0} Not saved. Message: please enter atleast one time (Elec./Mech./Instr./Util/Power/Proc./Prv/Idel).", i));
                                    continue;
                                }
                                else
                                {
                                    if (((isElec == true) && (isMech == true || isInst == true || isUtility == true || isPower == true || isProcess == true || isPrv == true || isIdle == true)) ||
                                        (((isMech == true) && (isElec == true || isInst == true || isUtility == true || isPower == true || isProcess == true || isPrv == true || isIdle == true))) ||
                                        (((isInst == true) && (isElec == true || isMech == true || isUtility == true || isPower == true || isProcess == true || isPrv == true || isIdle == true))) ||
                                        (((isUtility == true) && (isElec == true || isMech == true || isInst == true || isPower == true || isProcess == true || isPrv == true || isIdle == true))) ||
                                        (((isPower == true) && (isElec == true || isMech == true || isInst == true || isUtility == true || isProcess == true || isPrv == true || isIdle == true))) ||
                                        (((isProcess == true) && (isElec == true || isMech == true || isInst == true || isUtility == true || isPower == true || isPrv == true || isIdle == true))) ||
                                        (((isPrv == true) && (isElec == true || isMech == true || isInst == true || isUtility == true || isPower == true || isProcess == true || isIdle == true))) ||
                                        (((isIdle == true) && (isElec == true || isMech == true || isInst == true || isUtility == true || isPower == true || isProcess == true || isPrv == true))))
                                    {
                                        sb.AppendLine(string.Format("Row {0} Not saved. Message: Multiple entry for time found. Please enter only one time (Elec./Mech./Instr./Util/Power/Proc./Prv/Idel).", i));
                                        continue;
                                    }
                                }

                                breakDownUploadHistory.BreakDowns.Add(new Data.Models.BreakDown
                                {
                                    Id = 0,
                                    IsHistory = isHistory,
                                    IsRepeated = isRepeated,
                                    IsMajor = isMajor,
                                    PlantId = PlantId,
                                    LineId = LineId,
                                    MachineId = machineid,
                                    Date = Date,
                                    SubAssemblyId = null,
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    TotalTime = unchecked((int)totalTime),
                                    FailureDescription = string.IsNullOrEmpty(Convert.ToString(cells[7].Value)) ? null : Convert.ToString(cells[7].Value),
                                    ElectricalTime = isElec,
                                    MechTime = isMech,
                                    InstrTime = isInst,
                                    UtilityTime = isUtility,
                                    PowerTime = isPower,
                                    ProcessTime = isProcess,
                                    PrvTime = isPrv,
                                    IdleTime = isIdle,
                                    ResolveTimeTaken = null,
                                    SpareTypeId = null,
                                    SpareDescription = null,
                                    DoneBy = null,
                                    RootCause = string.IsNullOrEmpty(Convert.ToString(cells[16].Value)) ? null : Convert.ToString(cells[16].Value),
                                    Correction = string.IsNullOrEmpty(Convert.ToString(cells[17].Value)) ? null : Convert.ToString(cells[17].Value),
                                    CorrectiveAction = string.IsNullOrEmpty(Convert.ToString(cells[18].Value)) ? null : Convert.ToString(cells[18].Value),
                                    PreventingAction = string.IsNullOrEmpty(Convert.ToString(cells[19].Value)) ? null : Convert.ToString(cells[19].Value),
                                    CreatedBy = userId,
                                    CreatedDate = DateTime.UtcNow,
                                    UpdatedBy = null,
                                    UpdatedDate = null,
                                    DeletedDate = null,
                                    DeletedBy = null,
                                    IsDeleted = false,
                                    BreakDownSpares = spares
                                });
                            }
                            catch (Exception ex)
                            {
                                sb.AppendLine(string.Format("Row {0} Not saved. Message: {1}", i, ex.GetAllMessages()));
                            }
                        }
                    }
                    if (breakDownUploadHistory.BreakDowns.Count > 0)
                        this._breakDownService.InsertExcelFile(breakDownUploadHistory);
                }
                result.Status = JsonResponseStatus.Success;
                if (string.IsNullOrEmpty(sb.ToString()))
                    result.Message = "Excel file uploaded successfully.";
                else
                    result.Message = string.Format("<b>Message:</b><br />Excel file uploaded with some error. <br /><b>Error:</b><br />{0}", sb.ToString().Replace(Environment.NewLine, "<br />"));
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Message = ProcessException(ex);
            }
            return JsonNet(result, JsonRequestBehavior.DenyGet);
        }

        #region Reports 

        // GET: History Report 
        public ActionResult HistoryReport()
        {
            return View();
        }
        public JsonNetResult GetListForHistroyReport(int siteId, int plantId, int lineId, DateTime fromDate, DateTime toDate)
        {
            var allData = this._breakDownService.GetHistoryReportGridData(siteId, plantId, lineId, fromDate, toDate);
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HistoryReportMonthly()
        {
            return View();
        }

        #endregion
    }
}