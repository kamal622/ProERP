using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ProERP.Data.Models;
using ProERP.Services.Document;
using ProERP.Services.Plant;
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

namespace ProERP.Web.Controllers
{

    [Authorize]
    public class DocumentsController : BaseController
    {
        // GET: Documents
        private readonly DocumentTypeServices _documentTypeServices;
        private readonly DocumentService _documentService;
        private readonly PlantService _plantService;
        private readonly DocumentHistoryService _documentHistoryService;

        public DocumentsController(DocumentTypeServices documentTypeServices, DocumentService documentService, PlantService plantService, DocumentHistoryService documentHistoryService)
        {
            this._documentTypeServices = documentTypeServices;
            this._documentService = documentService;
            this._plantService = plantService;
            this._documentHistoryService = documentHistoryService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {

            return View();
        }
        public JsonNetResult GetAllCategories()
        {
            var allCategories = this._documentTypeServices.GetAllCategories();
            var allData = allCategories.Select(category => new TreeViewData { Name = category.Desription, Id = category.Id, ParentId = category.ParentCategoryId ?? -1, Value = category.Id }).ToList();
            return JsonNet(allData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SingleFileUpload(HttpPostedFileBase[] SingleFileUpload)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "Documents");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            List<FileUploadModel> files = new List<FileUploadModel>();
            foreach (HttpPostedFileBase File in SingleFileUpload)
            {
                FileInfo fi = new FileInfo(File.FileName);
                string sysFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
                var filePath = Path.Combine(path, sysFileName);
                File.SaveAs(filePath);
                files.Add(new FileUploadModel { OriginalFileName = fi.Name, SysFileName = sysFileName, ZipFileName = "" });
            }

            ViewBag.SingleFileData = JsonConvert.SerializeObject(files.Select(s => new { s.OriginalFileName, s.SysFileName }));
            return PartialView();
        }
        [HttpPost]
        public ActionResult ZipFileUpload(HttpPostedFileBase[] ZipFileUpload)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "Documents");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            List<FileUploadModel> files = new List<FileUploadModel>();
            foreach (HttpPostedFileBase file in ZipFileUpload)
            {
                using (ZipArchive archive = new ZipArchive(file.InputStream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        FileInfo fi = new FileInfo(entry.FullName);//Name
                        if(fi.Extension.Length != 0)
                        {
                            string sysFileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
                            var filePath = Path.Combine(path, sysFileName);
                            entry.ExtractToFile(filePath);
                            files.Add(new FileUploadModel { OriginalFileName = fi.Name, SysFileName = sysFileName, ZipFileName = new FileInfo(file.FileName).Name });
                        }
                    }
                }
            }

            ViewBag.ZipFileData = JsonConvert.SerializeObject(files.Select(s => new { s.OriginalFileName, s.SysFileName, s.ZipFileName }));
            return PartialView();
        }
        [HttpPost]
        public JsonNetResult SaveDocuments(int PlantId, int? LineId, int? MachineId, int[] CategoryIds, FileUploadModel[] UploadFiles)
        {
            string plantName = "", lineName = "", machineName = "";
            var result = new JsonResponse();
            try
            {
                if (PlantId <= 0)
                {
                    result.Status = JsonResponseStatus.Error;
                    result.Message = "Please select plant.";
                    return JsonNet(result, JsonRequestBehavior.DenyGet);
                }

                if (MachineId != null && MachineId > 0)
                    this._documentService.GetPLMNames(MachineId.Value, out plantName, out lineName, out machineName);
                else if (LineId != null && LineId > 0)
                    this._documentService.GetPLNames(LineId.Value, out plantName, out lineName);
                else
                {
                    var plant = this._plantService.GetPlantById(PlantId);
                    plantName = plant.Name;
                }
                string relativePath = Path.Combine(plantName, lineName, machineName);
                string sourcePath = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], "Documents");
                string destinationPath = Path.Combine(sourcePath, relativePath);

                List<Data.Models.Document> docs = new List<Data.Models.Document>();
                foreach (var uploadFile in UploadFiles)
                {
                    var sourceFileName = Path.Combine(sourcePath, uploadFile.SysFileName);
                    var destFileName = Path.Combine(destinationPath, uploadFile.SysFileName);
                    if (!System.IO.Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);

                    System.IO.File.Move(sourceFileName, destFileName);

                    foreach (var categoryId in CategoryIds)
                    {
                        if (categoryId == 1)
                            continue;
                        DateTime dtCreated = DateTime.UtcNow;
                        int createdId = HttpContext.User.Identity.GetUserId<int>();

                        DocumentHistory history = new DocumentHistory
                        {
                            ActionId = 1,
                            ActionDate = dtCreated,
                            ActionBy = createdId,
                        };

                        Document doc = new Document
                        {
                            OriginalFileName = uploadFile.OriginalFileName,
                            SysFileName = uploadFile.SysFileName,
                            ZipFileName = uploadFile.ZipFileName,
                            Type = categoryId,
                            CreatedBy = createdId,
                            CreatedOn = dtCreated,
                            PlantId = PlantId,
                            LineId = LineId,
                            MachineId = MachineId,
                            RelativePath = relativePath
                        };

                        doc.DocumentHistories.Add(history);
                        docs.Add(doc);
                    }
                }

                this._documentService.BuldInsert(docs);
                result.Status = JsonResponseStatus.Success;
                return JsonNet(result, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                return JsonNet(new { Type = "Error", Message = (ex.Message + ex.InnerException ?? ex.InnerException.Message) + ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonNetResult DeleteDocument(int[] Ids)
        {
            JsonResponse response = new Models.JsonResponse();
            try
            {
                int userId = HttpContext.User.Identity.GetUserId<int>();
                this._documentService.DeleteDocuments(Ids, userId);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Record successfully Deleted";
            }
            catch(Exception ex)
            {
                response = new JsonResponse { Status = JsonResponseStatus.Error, Message = ProcessException(ex) };
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonNetResult GetDocuments(int PlantId, int LineId, int MachineId, int CategoryId, string searchKeyword)
        {
            if (searchKeyword == null)
                searchKeyword = "";
            List<Data.Models.Document> documents = this._documentService.GetDocuments(PlantId, LineId, MachineId, CategoryId, searchKeyword.Split(new char[] { ' ', ',', ';', '/' }, StringSplitOptions.RemoveEmptyEntries));
            var finalData = from a in documents
                            select new
                            {
                                Id = a.Id,
                                OriginalFileName = a.OriginalFileName,
                                CategoryName = a.DocumentType.Desription,
                                CreatedBy = a.User.UserName,
                                CreatedOn = a.CreatedOn,
                            };
            return JsonNet(finalData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetHistoryDocuments(int documentId)
        {
            List<Data.Models.DocumentHistory> documenthistory = this._documentHistoryService.GetHistoryDocuments(documentId);
            var finalData = from a in documenthistory
                            select new
                            {
                                Id = a.Id,
                                ActionName = a.DocumentAction.Name,
                                ActionBy = a.User.UserName,
                                ActionDate = a.ActionDate,
                            };
            return JsonNet(finalData, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult SaveTags(int DocumentId, string Tags)
        {
            var result = new JsonResponse();
            try
            {
                this._documentService.UpdateTags(DocumentId, Tags);
                result.Status = JsonResponseStatus.Success;

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                return JsonNet(new { Type = "Error", Message = (ex.Message + ex.InnerException ?? ex.InnerException.Message) + ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }

            return JsonNet(result, JsonRequestBehavior.AllowGet);
        }
        public JsonNetResult GetTagsById(int DocumentId)
        {

            Data.Models.Document existingDocument = this._documentService.GetForId(DocumentId);
            return JsonNet(new { Tags = existingDocument.Tags }, JsonRequestBehavior.AllowGet);
        }
    }
}