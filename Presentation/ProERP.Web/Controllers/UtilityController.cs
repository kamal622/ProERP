using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProERP.Web.Framework;
using ProERP.Services.Utility;
using ProERP.Web.Framework.Controllers;
using ProERP.Web.Models;
using System.Data;
using ProERP.Services.Models;
using Microsoft.AspNet.Identity;
using ProERP.Data.Models;

namespace ProERP.Web.Controllers
{
    [Authorize]
    public class UtilityController : BaseController
    {
        private readonly TemplateService _templateService;
        private readonly UtilityService _utilityService;

        public UtilityController(TemplateService templateService, UtilityService utilityService)
        {
            this._templateService = templateService;
            this._utilityService = utilityService;
        }

        // GET: Utility
        # region Utility
        public ActionResult Index()
        {
            return View();
        }

        public JsonNetResult GetGridData(int templateId, SearchViewModel[] searchViewModel)
        {
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            var viewModel = searchViewModel.FirstOrDefault(f => f.DataType.ToUpper() == "D");

            if (viewModel != null)
            {
                fromDate = Convert.ToDateTime(((string[])(viewModel.Value["from"]))[0]);
                toDate = Convert.ToDateTime(((string[])(viewModel.Value["to"]))[0]);
            }

            JsonResponse response = new JsonResponse();
            try
            {
                var templateMappings = this._templateService.GetTemplateMappings(templateId);
                var allData = new List<object>();
                dynamic row = new System.Dynamic.ExpandoObject();

                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(int));

                foreach (var item in templateMappings.OrderBy(o => o.OrderNo))
                {
                    switch (item.DataType.ToUpper())
                    {
                        case "I": // Int
                            dt.Columns.Add(item.SystemColumnName, typeof(int));
                            break;
                        case "F": // Numeric
                            dt.Columns.Add(item.SystemColumnName, typeof(decimal));
                            break;
                        case "B": // Bool
                            dt.Columns.Add(item.SystemColumnName, typeof(bool));
                            break;
                        case "D": // Date
                        case "T": // Date
                            dt.Columns.Add(item.SystemColumnName, typeof(DateTime));
                            break;
                        case "S": // String
                        default:
                            dt.Columns.Add(item.SystemColumnName, typeof(string));
                            break;
                    }
                }
                var result = this._utilityService.GetGridData(templateId, searchViewModel, templateMappings);

                #region Comments

                //var finalData = from a in result
                //                select new
                //                {
                //                    a.Id,
                //                    a.TemplateId,
                //                    a.PlantId,
                //                    a.LineId,
                //                    a.MachineId,
                //                    a.C1,
                //                    a.C2,
                //                    a.C3,
                //                    a.C4,
                //                    a.C5,
                //                    a.C6,
                //                    a.C7,
                //                    a.C8,
                //                    a.C9,
                //                    a.C10,
                //                    a.C11,
                //                    a.C12,
                //                    a.C13,
                //                    a.C14,
                //                    a.C15,
                //                    a.C16,
                //                    a.C17,
                //                    a.C18,
                //                    a.C19,
                //                    a.C20,
                //                    a.C21,
                //                    a.C22,
                //                    a.C23,
                //                    a.C24,
                //                    a.C25,
                //                    a.C26,
                //                    a.C27,
                //                    a.C28,
                //                    a.C29,
                //                    a.C30,
                //                    a.C31,
                //                    a.C32,
                //                    a.C33,
                //                    a.C34,
                //                    a.C35,
                //                    a.C36,
                //                    a.C37,
                //                    a.C38,
                //                    a.C39,
                //                    a.C40,
                //                    a.C41,
                //                    a.C42,
                //                    a.C43,
                //                    a.C44,
                //                    a.C45,
                //                    a.C46,
                //                    a.C47,
                //                    a.C48,
                //                    a.C49,
                //                    a.C50,
                //                }; 

                #endregion

                DataTable searchResult = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(Newtonsoft.Json.JsonConvert.SerializeObject(result));

                int id = 0;
                DateTime date = fromDate.Date;
                int gridRowsCount = 31;
                if (fromDate > DateTime.MinValue)
                    gridRowsCount = (toDate - fromDate).Days + 1;

                for (int i = 0; i < gridRowsCount; i++)
                {
                    id -= 1;
                    DataRow dr = dt.NewRow();
                    dr["Id"] = id;
                    List<string> searchConditions = new List<string>();
                    foreach (var item in templateMappings.OrderBy(o => o.OrderNo))
                    {
                        if (item.IsAutoGenerated)
                        {
                            if (item.DataType == "D" && date > DateTime.MinValue)
                                searchConditions.Add(string.Format("{0} = '{1}'", item.SystemColumnName, date.ToString("dd/MM/yyyy")));
                            //else
                            //    searchConditions.Add(string.Format("{0} = {1}", item.SystemColumnName, date.ToString("dd/MM/yyyy")));
                        }

                        if (item.DataType == "D" && item.IsAutoGenerated && date > DateTime.MinValue)
                            dr[item.SystemColumnName] = date;
                        else if (!string.IsNullOrEmpty(item.DefaultValue))
                            dr[item.SystemColumnName] = item.DefaultValue;
                        else
                            dr[item.SystemColumnName] = DBNull.Value;
                    }
                    string whereCondition = string.Join(" AND ", searchConditions);
                    var rows = new DataRow[] { };

                    if (searchResult.Rows.Count > 0 && !string.IsNullOrEmpty(whereCondition))
                        rows = searchResult.Select(whereCondition);

                    if (rows.Length > 0)
                        dt.Rows.Add(rows[0].ItemArray);
                    else
                        dt.Rows.Add(dr);
                    date = date.AddDays(1);
                    if (date > toDate.Date)
                        break;
                }
                response.Data = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(dt));
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetReportTemplates()
        {
            var allTemplates = this._templateService.GetAllTemplates();

            return JsonNet(allTemplates.Select(s => new { s.Id, s.Name }), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetGridModel(int templateId)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                var templateMappings = this._templateService.GetTemplateMappings(templateId);
                List<object> columns = new List<object>();
                List<object> columnGroups = new List<object>();
                List<object> dataFields = new List<object>();
                List<SearchViewModel> searchModel = new List<SearchViewModel>();

                dataFields.Add(new { name = "Id", type = "int" });
                columns.Add(new { name = "Id", datafield = "Id", hidden = true });

                foreach (var item in templateMappings.OrderBy(o => o.OrderNo))
                {
                    if (item.IsSearchColumn)
                    {
                        var searchItem = new SearchViewModel { Id = item.Id, ColumnName = item.ColumnName, SystemColumnName = item.SystemColumnName, DataType = item.DataType, FixColumnId = item.FixColumnId, IsOrderBy = item.IsOrderBy };

                        if (item.DataType.ToUpper() == "D")
                        {
                            searchItem.Value.Add("from", new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).Date);
                            searchItem.Value.Add("to", new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month)).Date);
                        }
                        searchModel.Add(searchItem);
                    }

                    dynamic column = new System.Dynamic.ExpandoObject();
                    column.text = item.ColumnName;
                    column.datafield = item.SystemColumnName;
                    column.OnChangeFormula = item.OnChangeFormula;
                    column.DataType = item.DataType;

                    switch (item.DataType.ToUpper())
                    {
                        case "I": // Int
                            column.columntype = "numberinput";
                            column.cellsalign = "right";
                            column.cellsformat = "n";
                            dataFields.Add(new { name = item.SystemColumnName, type = "int" });
                            break;
                        case "F": // Numeric
                            column.columntype = "numberinput";
                            column.cellsalign = "right";
                            column.cellsformat = "f2";
                            column.decimalDigits = 2;
                            dataFields.Add(new { name = item.SystemColumnName, type = "float" });
                            break;
                        case "B": // Bool
                            column.cellsalign = "center";
                            dataFields.Add(new { name = item.SystemColumnName, type = "bool" });
                            break;
                        case "D": // Date
                            column.cellsalign = "right";
                            column.cellsformat = "dd/MM/yyyy";
                            dataFields.Add(new { name = item.SystemColumnName, type = "date", format = "dd/MM/yyyy" });
                            break;
                        case "T": // Date
                            column.cellsalign = "right";
                            dataFields.Add(new { name = item.SystemColumnName, type = "date", format = "HH:mm" });
                            break;
                        case "S": // String
                        default:
                            dataFields.Add(new { name = item.SystemColumnName, type = "string" });
                            break;
                    }
                    column.IsRequired = item.IsRequired;
                    if (!string.IsNullOrEmpty(item.GroupName))
                    {
                        columnGroups.Add(new { text = item.GroupName, align = "center", name = item.GroupName.Replace(" ", "") });
                        column.columngroup = item.GroupName.Replace(" ", "");
                    }
                    column.editable = item.IsEditable;
                    if (item.IsAggregate)
                    {
                        //column.aggregates = new[] { "sum" };
                        column.aggregates = new[] { item.AggregateFunction.ToLower() };
                    }

                    column.width = item.Width;

                    columns.Add(column);
                }
                response.Data = new { Columns = columns, ColumnGropus = columnGroups, DataFields = dataFields, SearchViewModel = searchModel };
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveData(Data.Models.ReadingData[] readingData, int templateId)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                int userId = HttpContext.User.Identity.GetUserId<int>();
                DateTime dt = DateTime.UtcNow;

                var templateMappings = this._templateService.GetTemplateMappings(templateId);
                var finalData = new List<Data.Models.ReadingData>();

                for (int i = 0; i < readingData.Length; i++)
                {
                    var flag = false;
                    var item = readingData[i];
                    item.TemplateId = templateId;
                    if (item.Id <= 0)
                    {
                        item.CreatedBy = userId;
                        item.CreatedOn = dt;
                    }
                    else
                    {
                        item.UpdatedBy = userId;
                        item.UpdatedOn = dt;
                    }

                    foreach (var map in templateMappings)
                    {
                        var value = Convert.ToString(item.GetType().GetProperty(map.SystemColumnName).GetValue(item, null));
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (!map.IsAutoGenerated && map.FixColumnId == null)
                                flag = true;

                            var date = new DateTime();
                            if (map.DataType == "D" && DateTime.TryParse(value, out date))
                            {
                                value = date.ToString("dd/MM/yyyy");
                                item.GetType().GetProperty(map.SystemColumnName).SetValue(item, value);
                            }
                        }
                        else
                        {
                            if (map.IsRequired)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (flag)
                        finalData.Add(item);
                }

                this._utilityService.SaveData(finalData);
                response.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region UtilityConfiguration
        public ActionResult UtilityConfiguration()
        {
            return View();
        }

        public JsonResult GetReportList(string ReportName)
        {
            var allData = this._utilityService.GetUtilityData(ReportName);
            var data = allData.Select(s => new { s.Id, s.Name, s.Description, s.IsActive });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult SaveReport(int TemplateId,string Name,string Description,Boolean IsActive, Data.Models.TemplateMapping[] templatecolumn,int[] TempMappingIds)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                this._utilityService.SaveReport(TemplateId, Name,Description, IsActive, templatecolumn, TempMappingIds);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Report columns successfully created";
            }
            catch (Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColumnByTemplateId(int TemplateId)
        {
            var allData = this._utilityService.getTemplateId(TemplateId);
            var data = allData.Select(s => new
            {
                s.Id,
                s.TemplateId,
                s.SystemColumnName,
                s.ColumnName,
                DataType = s.DataType == "I" ? "1" : (s.DataType == "D") ? "2" : (s.DataType == "F") ? "3" : (s.DataType == "S") ? "4" : (s.DataType == "B") ? "5" : (s.DataType == "T") ? "6" : "",
                s.GroupName,
                s.OrderNo,
                s.Formula,
                s.IsRequired,
                s.IsAggregate,
                AggregateFunction = s.AggregateFunction == null ? "1" : ( s.AggregateFunction == "SUM") ? "2" : (s.AggregateFunction == "AVG") ? "3" : (s.AggregateFunction == "MAX") ? "4" : (s.AggregateFunction == "MIN") ? "5" : "",
                s.IsSearchColumn ,
                s.IsEditable,
                s.OnChangeFormula,
                s.Width,
                s.IsAutoGenerated,
                s.IsOrderBy,
                s.DefaultValue
            }).OrderBy(o=>o.OrderNo);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult UpdateIsActive(int TempId,Boolean IsActive)
        {
            JsonResponse response = new JsonResponse();
            try
            {
                this._utilityService.UpdateIsActive(TempId, IsActive);
                response.Status = JsonResponseStatus.Success;
                response.Message = "Record updated";
            }
            catch(Exception ex)
            {
                response.Status = JsonResponseStatus.Error;
                response.Message = ProcessException(ex);
            }
            return JsonNet(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}