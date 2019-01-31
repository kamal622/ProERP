using ProERP.Core.Data;
using ProERP.Data.Models;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace ProERP.Services.Utility
{
    public class UtilityService
    {
        private readonly IRepository<Data.Models.ReadingData> _readingDataRepository;
        private readonly IRepository<Data.Models.Template> _templateRepository;
        private readonly IRepository<Data.Models.TemplateMapping> _templateMappingRepository;

        public UtilityService(IRepository<Data.Models.ReadingData> readingDataRepository, IRepository<Data.Models.Template> templateRepository,
            IRepository<Data.Models.TemplateMapping> templateMappingRepository)
        {
            this._readingDataRepository = readingDataRepository;
            this._templateRepository = templateRepository;
            this._templateMappingRepository = templateMappingRepository;
        }

        #region Utility

        public void SaveData(List<Data.Models.ReadingData> readingData)
        {
            try
            {
                // Update
                List<Data.Models.ReadingData> readingDataToUpdate = new List<Data.Models.ReadingData>();
                var dataToUpdate = readingData.Where(w => w.Id > 0).ToArray();
                foreach (var item in dataToUpdate)
                {
                    var existing = this._readingDataRepository.Table.FirstOrDefault(f => f.Id == item.Id);
                    existing.PlantId = item.PlantId;
                    existing.LineId = item.LineId;
                    existing.C1 = item.C1;
                    existing.C2 = item.C2;
                    existing.C3 = item.C3;
                    existing.C4 = item.C4;
                    existing.C5 = item.C5;
                    existing.C6 = item.C6;
                    existing.C7 = item.C7;
                    existing.C8 = item.C8;
                    existing.C9 = item.C9;
                    existing.C10 = item.C10;
                    existing.C11 = item.C11;
                    existing.C12 = item.C12;
                    existing.C13 = item.C13;
                    existing.C14 = item.C14;
                    existing.C15 = item.C15;
                    existing.C16 = item.C16;
                    existing.C17 = item.C17;
                    existing.C18 = item.C18;
                    existing.C19 = item.C19;
                    existing.C20 = item.C20;
                    existing.C21 = item.C21;
                    existing.C22 = item.C22;
                    existing.C23 = item.C23;
                    existing.C24 = item.C24;
                    existing.C25 = item.C25;
                    existing.C26 = item.C26;
                    existing.C27 = item.C27;
                    existing.C28 = item.C28;
                    existing.C29 = item.C29;
                    existing.C30 = item.C30;
                    existing.C31 = item.C31;
                    existing.C32 = item.C32;
                    existing.C33 = item.C33;
                    existing.C34 = item.C34;
                    existing.C35 = item.C35;
                    existing.C36 = item.C36;
                    existing.C37 = item.C37;
                    existing.C38 = item.C38;
                    existing.C39 = item.C39;
                    existing.C40 = item.C40;
                    existing.C41 = item.C41;
                    existing.C42 = item.C42;
                    existing.C43 = item.C43;
                    existing.C44 = item.C44;
                    existing.C45 = item.C45;
                    existing.C46 = item.C46;
                    existing.C47 = item.C47;
                    existing.C48 = item.C48;
                    existing.C49 = item.C49;
                    existing.C50 = item.C50;
                    readingDataToUpdate.Add(existing);
                }
                this._readingDataRepository.Update(readingDataToUpdate);

                // Insert
                var dataToInsert = readingData.Where(w => w.Id <= 0).ToArray();
                this._readingDataRepository.Insert(dataToInsert);
            }
            catch
            {
                throw;
            }
        }

        public IQueryable GetGridData(int templateId, SearchViewModel[] searchViewModel, Data.Models.TemplateMapping[] templateMappings)
        {
            try
            {
                var viewModel = searchViewModel.FirstOrDefault(f => f.DataType.ToUpper() == "D");
                List<string> orderByColumns = new List<string>();
                string whereCondition = "";
                string dateRange = "";
                string orderBy = "";
                List<string> selectColumns = new List<string>();
                string select = "";
                List<string> dates = new List<string>();
                List<string> whereConditions = new List<string>();

                if (viewModel != null)
                {
                    DateTime fromDate = Convert.ToDateTime(((string[])(viewModel.Value["from"]))[0]);
                    DateTime toDate = Convert.ToDateTime(((string[])(viewModel.Value["to"]))[0]);
                    for (int i = 0; i < (toDate - fromDate).Days; i++)
                        dates.Add(string.Format("{0} = \"{1}\"", viewModel.SystemColumnName, fromDate.AddDays(i).ToString("dd/MM/yyyy")));
                    dateRange = string.Join(" OR ", dates);
                    //dateRange = string.Format("@0.Contains(it.{0})", viewModel.SystemColumnName);
                    //whereCondition = string.Format("{0} >= \"{1}\" AND {0} <= \"{2}\"", viewModel.SystemColumnName, fromDate.ToString("dd/MM/yyyy"), toDate.ToString("dd/MM/yyyy"));
                }

                foreach (var search in searchViewModel)
                {
                    if (search.IsOrderBy)
                        orderByColumns.Add(search.SystemColumnName);
                    if (search.DataType != "D")
                        whereConditions.Add(string.Format(" {0} = {1}", search.SystemColumnName, ((string[])(viewModel.Value["val"]))[0]));
                }

                whereCondition = string.Join(" AND ", whereConditions);
                orderBy = string.Join(",", orderByColumns);
                if (string.IsNullOrEmpty(orderBy))
                    orderBy = "Id";

                selectColumns = templateMappings.OrderBy(o => o.OrderNo).Select(s => s.SystemColumnName).ToList();
                selectColumns.Insert(0, "Id");

                var fixcolumn = templateMappings.Where(w => w.TemplateId == templateId).Select(s => new { s.FixColumnId ,s.SystemColumnName }).ToArray();
                for (int i = 0; i < fixcolumn.Length; i++)
                {
                    var column = fixcolumn[i];
                    if (column.FixColumnId != 0)
                    {
                        if (column.FixColumnId == 1)
                        {
                            selectColumns.Add(@"""x"" as PlantName");
                        }
                        else if (column.FixColumnId == 2)
                        {
                            selectColumns.Add("'' as LineName");
                        }
                    }
                }

                select = string.Format("new ( {0} )", string.Join(",", selectColumns));
                //if (viewModel != null)
                //{
                //    DateTime fromDate = Convert.ToDateTime(((string[])(viewModel.Value["from"]))[0]);
                //    DateTime toDate = Convert.ToDateTime(((string[])(viewModel.Value["to"]))[0]);
                //    return this._readingDataRepository.Table.Where(whereCondition, fromDate, toDate).OrderBy(orderBy).ToArray();
                //}
                //else

                var query = this._readingDataRepository.Table.Where(w => w.TemplateId == templateId);

                if (dates.Count > 0)
                    query = query.Where(dateRange);

                if (!string.IsNullOrEmpty(whereCondition))
                    query = query.Where(whereCondition);

                return query.OrderBy(orderBy).Select(select);

                //return this._readingDataRepository.Table.Where(w => w.TemplateId == templateId).Where(whereCondition).OrderBy(orderBy).Select(select);
            }
            catch
            {
                throw;
            }
        }

        public int getPlantId(int id)
        {
            return this._readingDataRepository.Table.Where(w => w.TemplateId == id).Select(s=>s.PlantId).FirstOrDefault();
        }

        #endregion

        #region UtilityConfiguration
        public List<Data.Models.Template> GetUtilityData(string ReportName)
        {
            var allData = this._templateRepository.Table.ToList();
            if (!string.IsNullOrEmpty(ReportName))
                allData = allData.Where(w => w.Name.ToLower().Contains(ReportName.Trim().ToLower())).ToList();
            return allData.ToList();
        }

        public void SaveReport(int TemplateId, string Name, string Description,Boolean IsActive, Data.Models.TemplateMapping[] templatecolumn, int[] TempMappingIds)
        {
            if (TempMappingIds != null && TempMappingIds.Length > 0)
            {

                var Deltecolumns = this._templateMappingRepository.Table.Where(w => TempMappingIds.Contains(w.Id));
                this._templateMappingRepository.Delete(Deltecolumns);
            }

            if (templatecolumn.Any(a => a.TemplateId == 0))
            {
                var reportName = new Template
                {
                    Name = Name,
                    Description = Description,
                    IsActive = IsActive,
                    TemplateMappings = new List<TemplateMapping>()
                };

                var order = 1;
                for (int i = 0; i < templatecolumn.Length; i++)
                {
                    var data = templatecolumn[i];

                    if (data.Id < 0)//Insert
                    {
                        //DataType
                        if (data.DataType == "1")
                            data.DataType = "I";
                        else if (data.DataType == "2")
                            data.DataType = "D";
                        else if (data.DataType == "3")
                            data.DataType = "F";
                        else if (data.DataType == "4")
                            data.DataType = "S";
                        else if (data.DataType == "5")
                            data.DataType = "B";
                        else if (data.DataType == "6")
                            data.DataType = "T";
                        else
                            data.DataType = "S";

                        data.FixColumnId = null;

                        if (data.AggregateFunction == "1")
                            data.AggregateFunction = null;
                        else if(data.AggregateFunction == "2")
                            data.AggregateFunction = "SUM";
                        else if (data.AggregateFunction == "3")
                            data.AggregateFunction = "AVG";
                        else if (data.AggregateFunction == "4")
                            data.AggregateFunction = "MAX";
                        else if (data.AggregateFunction == "5")
                            data.AggregateFunction = "MIN";
                        else
                            data.AggregateFunction = null;
                        
                        if(data.OrderNo == 0)
                        {
                            data.OrderNo = order++;
                            data.SystemColumnName = "C" + data.OrderNo;
                        }
                        else
                        {
                            data.SystemColumnName = "C" + data.OrderNo;
                            data.OrderNo = data.OrderNo;
                        }
                        reportName.TemplateMappings.Add(data);
                    }
                }
                this._templateRepository.Insert(reportName);
            }
            else//Update
            {
                var exisitingTemplate = this._templateRepository.Table.FirstOrDefault(f => f.Id == TemplateId);
                if (exisitingTemplate != null)
                {
                    if(exisitingTemplate.Name != Name || exisitingTemplate.Description != Description || exisitingTemplate.IsActive != IsActive)
                    {
                        exisitingTemplate.Name = Name;
                        exisitingTemplate.Description = Description;
                        exisitingTemplate.IsActive = IsActive;
                        this._templateRepository.Update(exisitingTemplate);
                    }
                }

                for (int i = 0; i < templatecolumn.Length; i++)
                {
                    var data = templatecolumn[i];
                    Data.Models.TemplateMapping existingData = this._templateMappingRepository.Table.FirstOrDefault(w => w.Id == data.Id);
                    if (existingData != null)
                    {
                        existingData.ColumnName = data.ColumnName;
                        existingData.OrderNo = data.OrderNo;
                        existingData.GroupName = data.GroupName;
                        existingData.Formula = data.Formula;
                        existingData.IsRequired = data.IsRequired;
                        existingData.IsAggregate = data.IsAggregate;
                        existingData.IsSearchColumn = data.IsSearchColumn;
                        existingData.IsEditable = data.IsEditable;
                        existingData.OnChangeFormula = data.OnChangeFormula;
                        existingData.Width = data.Width;
                        existingData.IsAutoGenerated = data.IsAutoGenerated;
                        existingData.IsOrderBy = data.IsOrderBy;
                        existingData.DefaultValue = data.DefaultValue;

                        if (data.DataType == "1")
                            existingData.DataType = "I";
                        else if (data.DataType == "2")
                            existingData.DataType = "D";
                        else if (data.DataType == "3")
                            existingData.DataType = "F";
                        else if (data.DataType == "4")
                            existingData.DataType = "S";
                        else if (data.DataType == "5")
                            existingData.DataType = "B";
                        else if (data.DataType == "6")
                            existingData.DataType = "T";
                        else
                            existingData.DataType = "S";

                        if (data.AggregateFunction == "1")
                            existingData.AggregateFunction = null;
                        else if (data.AggregateFunction == "2")
                            existingData.AggregateFunction = "SUM";
                        else if (data.AggregateFunction == "3")
                            existingData.AggregateFunction = "AVG";
                        else if (data.AggregateFunction == "4")
                            existingData.AggregateFunction = "MAX";
                        else if (data.AggregateFunction == "5")
                            existingData.AggregateFunction = "MIN";
                        else
                            existingData.AggregateFunction = null;

                        this._templateMappingRepository.Update(existingData);
                    }
                    else
                    {
                        var orderNo = this._templateMappingRepository.Table.Where(w => w.TemplateId == TemplateId).Max(s => s.OrderNo);
                        var no = orderNo + 1;
                        if (data.DataType == "1")
                            data.DataType = "I";
                        else if (data.DataType == "2")
                            data.DataType = "D";
                        else if (data.DataType == "3")
                            data.DataType = "F";
                        else if (data.DataType == "4")
                            data.DataType = "S";
                        else if (data.DataType == "5")
                            data.DataType = "B";
                        else if (data.DataType == "6")
                            data.DataType = "T";
                        else
                            data.DataType = "S";

                        data.FixColumnId = null;

                        if (data.AggregateFunction == "1")
                            data.AggregateFunction = null;
                        else if (data.AggregateFunction == "2")
                            data.AggregateFunction = "SUM";
                        else if (data.AggregateFunction == "3")
                            data.AggregateFunction = "AVG";
                        else if (data.AggregateFunction == "4")
                            data.AggregateFunction = "MAX";
                        else if (data.AggregateFunction == "5")
                            data.AggregateFunction = "MIN";
                        else
                            data.AggregateFunction = null;

                        data.SystemColumnName = "C" + no;
                        data.OrderNo = no;
                        this._templateMappingRepository.Insert(data);
                    }
                }


            }
           
        }

        public void UpdateIsActive(int TempId,Boolean IsActive)
        {
            Data.Models.Template oldTemplate = this._templateRepository.Table.Where(w => w.Id == TempId).FirstOrDefault();
            if (oldTemplate != null)
            {
                oldTemplate.IsActive = IsActive;
                this._templateRepository.Update(oldTemplate);
            }
        }

        public List<Data.Models.TemplateMapping> getTemplateId(int TemplateId)
        {
            return this._templateMappingRepository.Table.Where(w => w.TemplateId == TemplateId).ToList();
        }


        #endregion
    }
}
