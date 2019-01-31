using ProERP.Core.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Reports
{
    public class ReportService
    {
        #region Private Member
        private readonly IRepository<Data.Models.IndentDetail> _indentDetailRepository;
        private readonly IRepository<Data.Models.PreventiveReviewHistory> _PRHRepository;
        private readonly IRepository<Data.Models.Line> _lineRepository;
        private readonly IRepository<Data.Models.BreakDown> _breakdownReportRepository;
        private readonly IRepository<Data.Models.Indent> _IndentsRepository;
        private readonly IRepository<Data.Models.ShutdownHistory> _shutdownHistoryRepository;
        private readonly IRepository<Data.Models.User> _userRepository;
        private readonly IRepository<Data.Models.MaintenanceRequest> _maintenanceRequestRepository;
        private readonly IRepository<Data.Models.FormulationRequest> _formulationRequestRepository;
        private readonly IRepository<Data.Models.QOSLine> _qOSLineRepository;
        private readonly IRepository<Data.Models.PreventiveMaintenance> _preventiveMaintenanceRepository;
        #endregion

        public ReportService(IRepository<Data.Models.IndentDetail> indentDetailRepository,
            IRepository<Data.Models.PreventiveReviewHistory> PRHRepository,
            IRepository<Data.Models.Line> lineRepository,
            IRepository<Data.Models.BreakDown> breakdownReportRepository,
            IRepository<Data.Models.ShutdownHistory> shutdownHistoryRepository,
            IRepository<Data.Models.User> userRepository,
            IRepository<Data.Models.Indent> IndentsRepository,
            IRepository<Data.Models.MaintenanceRequest> maintenanceRequestRepository,
            IRepository<Data.Models.FormulationRequest> formulationRequestRepository,
            IRepository<Data.Models.QOSLine> qOSLineRepository,
            IRepository<Data.Models.PreventiveMaintenance> preventiveMaintenanceRepository)
        {
            this._indentDetailRepository = indentDetailRepository;
            this._PRHRepository = PRHRepository;
            this._lineRepository = lineRepository;
            this._breakdownReportRepository = breakdownReportRepository;
            this._shutdownHistoryRepository = shutdownHistoryRepository;
            this._userRepository = userRepository;
            this._IndentsRepository = IndentsRepository;
            this._maintenanceRequestRepository = maintenanceRequestRepository;
            this._formulationRequestRepository = formulationRequestRepository;
            this._qOSLineRepository = qOSLineRepository;
            this._preventiveMaintenanceRepository = preventiveMaintenanceRepository;
        }

        public List<ConsolidateIndentData> GetConsolidatedIndentData(int year)
        {
            var allData = from a in this._indentDetailRepository.Table
                          where a.Indent.CreatedOn.Year == year
                          select new ConsolidateIndentData
                          {
                              PlantId = (a.Plant == null ? 0 : a.PlantId.Value),
                              PlantName = (a.Plant == null ? "NA" : a.Plant.Name),
                              RequisitionType = a.Indent.RequisitionType,
                              BudgetId = a.Indent.BudgetId,
                              BudgetType = a.Indent.IndentBudget.BudgetType,
                              BudgetCode = a.Indent.IndentBudget.BudgetCode + " (" + a.Indent.IndentBudget.FinancialYear + ")",
                              //Expense = (a.FinalPrice == null ? a.EstimatePrice : a.FinalPrice.Value) * (a.IssuedQty == null ? a.QtyNeeded : a.IssuedQty.Value),
                              //Expense = (a.EstimatePrice) * (a.IssuedQty == null ? a.QtyNeeded : a.IssuedQty.Value),
                              Expense = (a.FinalPrice == null ? a.EstimatePrice : a.FinalPrice.Value) * (a.IssuedQty == null ? a.QtyNeeded : a.IssuedQty.Value) * (a.ExchangeRate == null ? 1 : a.ExchangeRate.Value),
                              IndentDate = a.Indent.CreatedOn,
                              TotalBudget = a.Indent.IndentBudget.Amount == null ? 0 : a.Indent.IndentBudget.Amount.Value
                          };

            return allData.ToList();
        }

        public List<IndentReportDataSet> GetIssuedIndentDetail(int plantId, int year)
        {
            var allData = this._indentDetailRepository.Table.Where(w => w.IndentStatu.Description == "Approved" || w.IndentStatu.Description == "PO")
                .Where(w => w.ApprovedOn.Value.Year == year)
                .Where(w => w.PlantId == plantId)
                .Select(s => new IndentReportDataSet
                {
                    Id = s.Id,
                    PlantId = s.PlantId,
                    LineId = s.LineId,
                    LineName = s.Line.Name,
                    MachineId = s.MachineId,
                    MachineName = s.Machine.Name,
                    PlantName = s.Plant.Name,
                    //Budget = totalBudget,
                    BudgetId = s.Indent.BudgetId,
                    Budget = s.Indent.IndentBudget.Amount,
                    BudgetType = s.Indent.IndentBudget.BudgetType,
                    BudgetCode = s.Indent.IndentBudget.BudgetCode,
                    //.Where(w => w.EffectiveFrom <= s.IssuedOn).Where(w => w.EffectiveTo >= s.IssuedOn).Select(a => a.MonthlyBudget).FirstOrDefault(),
                    //UnitPrice = s.UnitPrice,
                    //TotalAmount = s.TotalAmount,
                    ItemId = s.ItemId,
                    ItemName = s.Item != null ? s.Item.Name : s.JobDescription,
                    QtyNeeded = s.QtyNeeded,
                    IssuedQty = s.IssuedQty == null ? s.QtyNeeded : s.IssuedQty,
                    //FinalPrice = s.FinalPrice == null ? s.EstimatePrice : s.FinalPrice,
                    //FinalPrice = s.EstimatePrice ,
                    FinalPrice = (s.FinalPrice == null ? s.EstimatePrice : s.FinalPrice) * (s.ExchangeRate == null ? 1 : s.ExchangeRate.Value),
                    StatusId = s.StatusId,
                    IssuedOn = s.IssuedOn == null ? s.RequiredByDate : s.IssuedOn,
                    RequisitionType = s.Indent.RequisitionType
                });
            return allData.OrderBy(o => o.IssuedOn).OrderBy(o => o.PlantName).ToList();
        }

        public List<PreventiveSummaryReportModel> GetSummaryReportData(int Year, int Month, int PlantId,int ScheduleType)
        {
            var preventiveData = from a in this._PRHRepository.Table
                                 where a.ScheduledReviewDate.Year == Year && a.PreventiveMaintenance.IsDeleted == false
                                 && a.PreventiveMaintenance.Line.IsActive && a.PreventiveMaintenance.Machine.IsActive
                                 && a.ScheduledReviewDate.Month == (Month == 0 ? a.ScheduledReviewDate.Month : Month)
                                 && a.PreventiveMaintenance.PlantId == (PlantId == 0 ? a.PreventiveMaintenance.PlantId : PlantId)
                                 && a.PreventiveMaintenance.ScheduleType == (ScheduleType == 0 ? a.PreventiveMaintenance.ScheduleType : ScheduleType)
                                 && a.IsLineActive == true && a.IsMachineActive == true
                                 select a;

            var allData = from b in this._lineRepository.Table
                          join j in preventiveData on b.Id equals j.PreventiveMaintenance.LineId into join1
                          from a in join1.DefaultIfEmpty()
                          where b.PlantId == (PlantId == 0 ? b.PlantId : PlantId)
                          select new { Preventive = a, Line = b };


            var dtNow = DateTime.Now.Date;
            var finalData = from a in allData
                            orderby a.Preventive.ReviewDate
                            group a by new
                            {
                                ScheduledReviewDate = a.Preventive == null ? dtNow : a.Preventive.ScheduledReviewDate,
                                Id = a.Line.Id,
                                a.Line.PlantId,
                                UserId = ((a.Preventive == null || a.Preventive.User == null) ? 0 : a.Preventive.User.Id),
                                UserName = ((a.Preventive == null || a.Preventive.User == null) ? "NA" : a.Preventive.User.UserName),
                                PlantName = a.Line.Plant.Name,
                                LineName = a.Line.Name
                            } into g
                            select new PreventiveSummaryReportModel
                            {
                                Id = g.Key.Id, // LineId
                                PlantId = g.Key.PlantId,
                                LineId = g.Key.Id,
                                UserId = g.Key.UserId,
                                PlantName = g.Key.PlantName,
                                LineName = g.Key.LineName,
                                UserName = g.Key.UserName,
                                ScheduledReviewDate = g.Key.ScheduledReviewDate,
                                TotalActivity = g.Count(w => w.Preventive != null),
                                ReviewedCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy != null),
                                LapseCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy == null && w.Preventive.HoldId == null && w.Preventive.IsLaps),
                                HoldCount = g.Count(w => w.Preventive != null && w.Preventive.PreventiveHoldHistory != null),
                                Moderate = g.Count(w => w.Preventive != null && w.Preventive.PreventiveMaintenance.Severity == 1),
                                Critical = g.Count(w => w.Preventive != null && w.Preventive.PreventiveMaintenance.Severity == 2),
                                Minor = g.Count(w => w.Preventive != null && w.Preventive.PreventiveMaintenance.Severity == 3),
                                ModerateReviewedCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy != null && w.Preventive.PreventiveMaintenance.Severity == 1),
                                CriticalReviewedCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy != null && w.Preventive.PreventiveMaintenance.Severity == 2),
                                MinorReviewedCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy != null && w.Preventive.PreventiveMaintenance.Severity == 3),
                                ModerateLapseCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy == null && w.Preventive.HoldId == null && w.Preventive.IsLaps && w.Preventive.PreventiveMaintenance.Severity == 1),
                                CriticalLapseCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy == null && w.Preventive.HoldId == null && w.Preventive.IsLaps && w.Preventive.PreventiveMaintenance.Severity == 2),
                                MinorLapseCount = g.Count(w => w.Preventive != null && w.Preventive.ReviewBy == null && w.Preventive.HoldId == null && w.Preventive.IsLaps && w.Preventive.PreventiveMaintenance.Severity == 3)
                            };

            return finalData.ToList();
        }

        public List<BreakDownMonthlySummaryDataSet> GetBDAnalysisSummaryReportData(int PlantId, int Year, int Month)
        {
            var allData = this._breakdownReportRepository.Table.Where(w => w.Date.Year == Year && w.Date.Month == (Month == 0 ? w.Date.Month : Month) && w.IsDeleted == false).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power", BD = s }).ToList();
           
            if (PlantId > 0)
                allData = allData.Where(w => w.BD.PlantId == (PlantId == 0 ? w.BD.PlantId : PlantId)).ToList();
            

            var data = from a in allData
                       select new BreakDownMonthlySummaryDataSet
                       {
                           Date = a.BD.Date,
                           DaysInMonth = DateTime.DaysInMonth(a.BD.Date.Year, a.BD.Date.Month),
                           TotalTime = a.BD.TotalTime,
                           PlantName = a.BD.Plant.Name,
                           LineName = a.BD.Line.Name,
                           FailureDescription = a.BD.FailureDescription,
                           BreakDownType = (a.BD.ElectricalTime ? "Electrical" :
                                            a.BD.MechTime ? "Mechanical" :
                                            a.BD.InstrTime ? "Instrumentation" :
                                            a.BD.UtilityTime ? "Utility" :
                                            a.BD.PowerTime ? "Power" :
                                            a.BD.ProcessTime ? "Process" :
                                            a.BD.PrvTime ? "PRV" :
                                            a.BD.IdleTime ? "Idle" : "")
                       };
            return data.OrderBy(o => o.Date).OrderBy(o => o.LineName).ToList();
        }

        public List<ShutdownSummaryReportModel> GetShutdownSummaryReportData(int plantId, int year, int month)
        {
            var query = from a in this._shutdownHistoryRepository.Table
                        join b in this._PRHRepository.Table on a.Id equals b.ShutdownId
                        join c in this._userRepository.Table on a.ShutdownBy equals c.Id
                        where a.ShutdownDate.Year == year
                        && a.ShutdownDate.Month == (month == 0 ? a.ShutdownDate.Month : month)
                        && a.PlantId == (plantId == 0 ? a.PlantId : plantId)
                        && b.PreventiveMaintenance.ScheduleType == 5
                        && b.IsLineActive == true && b.IsMachineActive == true
                        select new ShutdownSummaryReportModel
                        {
                            Id = a.Id,
                            historyId = b.Id,
                            ShutdownPlantId = a.Plant.Id,
                            ShutdownLineId = a.LineId,
                            ShutdownMachineId = a.MachineId ?? 0,
                            PlantId = b.PreventiveMaintenance.PlantId,
                            LineId = b.PreventiveMaintenance.LineId,
                            MachineId = b.PreventiveMaintenance.MachineId,
                            ShutdownPlantName = a.Plant.Name,
                            ShutdownLineName = a.Line.Name,
                            ShutdownMachineName = (a.Machine == null ? "N/A" : a.Machine.Name),
                            PlantName = b.PreventiveMaintenance.Plant.Name,
                            LineName = b.PreventiveMaintenance.Line.Name,
                            MachineName = b.PreventiveMaintenance.Machine.Name,
                            ShutdownBy = c.UserName,
                            ShutdownDate = a.ShutdownDate,
                            WorkDescription = b.PreventiveMaintenance.WorkDescription,
                            ReviewBy = b.User.UserName,
                            ReviewDate = b.ReviewDate,
                            Note = b.Notes
                        };

            return query.ToList();
        }

        public List<PRReportData> GetPRReportData(int year, int month)
        {
            var query = from a in this._IndentsRepository.Table
                        where a.CreatedOn.Year == year 
                        && a.CreatedOn.Month == (month == 0 ? a.CreatedOn.Month : month)
                        && a.StatusId == 4
                        select new PRReportData
                        {
                            IndentNo = a.IndentNo,
                            PRDate = a.CreatedOn,
                            PODate = a.PoDate,
                            //PRAmount = a.IndentDetails.Sum(s => (s.EstimatePrice * s.QtyNeeded)),
                            PRAmount = a.IndentDetails.Sum(s => (s.EstimatePrice * s.QtyNeeded) * (s.ExchangeRate == null ? 1 : s.ExchangeRate.Value)),
                            POAmount = a.PoAmount
                        };
            return query.ToList();
        }

        public List<RepeatedMajorDataSet> GetRepeatedMajorReportData(int Year,int Month,string Id)
        {
            var allData = this._breakdownReportRepository.Table.Where(w => w.Date.Year == Year && w.Date.Month == (Month == 0 ? w.Date.Month : Month) && w.IsDeleted == false).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power", BD = s }).ToList();
            if(Id == "Repeated")//repeated
                allData = allData.Where(w => w.BD.IsRepeated == true).ToList();
           else//major
                allData = allData.Where(w => w.BD.IsMajor == true).ToList();

            var data = from a in allData
                       select new RepeatedMajorDataSet
                       {
                           Id = a.BD.Id,
                           PlantId = a.BD.PlantId,
                           PlantName=a.BD.Plant.Name,
                           LineId = a.BD.LineId,
                           LineName=a.BD.Line.Name,
                           MachineId = a.BD.MachineId,
                           MachineName = a.MachineName,
                           SubAssemblyId = a.BD.SubAssemblyId ?? 0,
                           SubAssemblyName = a.SubAssemblyName,
                           Date = a.BD.Date,
                           StartTime = DateTime.Now.Date + a.BD.StartTime,
                           StopTime = DateTime.Now.Date + a.BD.EndTime,
                           TotalTime = a.BD.TotalTime,
                           FailureDescription = a.BD.FailureDescription,
                           ElectricalTime = a.BD.ElectricalTime,
                           MechTime = a.BD.MechTime,
                           InstrTime = a.BD.InstrTime,
                           UtilityTime = a.BD.UtilityTime,
                           PowerTime = a.BD.PowerTime,
                           ProcessTime = a.BD.ProcessTime,
                           PrvTime = a.BD.PrvTime,
                           IdleTime = a.BD.IdleTime,
                           ResolveTimeTaken = DateTime.Now.Date + (a.BD.ResolveTimeTaken ?? new TimeSpan(0)),
                           SpareTypeId = a.BD.SpareTypeId ?? 0,
                           SpareTypeName = a.SpareTypeName,
                           SpareDescription = a.BD.SpareDescription,
                           DoneBy = a.BD.DoneBy,
                           RootCause = a.BD.RootCause,
                           Correction = a.BD.Correction,
                           CorrectiveAction = a.BD.CorrectiveAction,
                           PreventingAction = a.BD.PreventingAction
                       };
            return data.ToList(); 
        }

        public List<MaintenanceRequestReportDataSet> GetMaintenanceRequestData(int Year, int Month)
        {
            var allData = from a in this._maintenanceRequestRepository.Table
                          join b in this._userRepository.Table on a.RequestBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          join c in this._userRepository.Table on a.AssignTo equals c.Id into j2
                          from c in j2.DefaultIfEmpty()
                          join d in this._userRepository.Table on a.AssignBy equals d.Id into j3
                          from d in j3.DefaultIfEmpty()
                          join e in this._userRepository.Table on a.ProgressBy equals e.Id into j4
                          from e in j4.DefaultIfEmpty()
                          join f in this._userRepository.Table on a.CompleteBy equals f.Id into j5
                          from f in j5.DefaultIfEmpty()
                          join g in this._userRepository.Table on a.CloseBy equals g.Id into j6
                          from g in j6.DefaultIfEmpty()
                          join i in this._userRepository.Table on a.HoldBy equals i.Id into j7
                          from i in j6.DefaultIfEmpty()
                          where a.CreatedDate.Year == Year && a.CreatedDate.Month == Month
                          select new MaintenanceRequestReportDataSet
                            {
                                Id = a.Id,
                                SerialNo = a.SerialNo,
                                IsBreakdown = a.IsBreakdown,
                                IsCritical = a.IsCritical,
                                BreakdownType = a.BreakdownType,
                                Status = a.MaintanceRequestStatu.StatusName,
                                Priority = a.MaintenancePriorityType.Description,
                                PlantId = a.PlantId,
                                PlantName = (a.Plant == null ? "N/A" : a.Plant.Name),
                                LineId = a.LineId,
                                LineName = (a.Line == null ? "N/A" : a.Line.Name), 
                                MachineId = a.MachineId,
                                MachineName = (a.Machine == null ? "N/A" : a.Machine.Name),
                                RequestDate = a.RequestDate,
                                RequestTime = a.RequestTime,
                                RequestUserName = b.UserProfile == null ? b.UserName : b.UserProfile.FirstName + " " + b.UserProfile.LastName,
                                Remarks = a.Remarks,
                                AssignDate = a.AssignDate,
                                AssignUserName = c.UserProfile == null ? c.UserName : c.UserProfile.FirstName + " " + c.UserProfile.LastName,
                                AssignByUserName = d.UserProfile == null ? d.UserName : d.UserProfile.FirstName + " " + d.UserProfile.LastName,
                                WorkStartDate = a.WorkStartDate,
                                WorkStartTime = a.WorkStartTime,
                                WorkEndDate = a.WorkEndDate,
                                WorkEndTime = a.WorkEndTime,
                                ProgressUserName = e.UserProfile == null ? e.UserName : e.UserProfile.FirstName + " " + e.UserProfile.LastName,
                                CompleteUserName = f.UserProfile == null ? f.UserName : f.UserProfile.FirstName + " " + f.UserProfile.LastName,
                                CloseUserName = g.UserProfile == null ? g.UserName : g.UserProfile.FirstName + " " + g.UserProfile.LastName,
                                Problem = a.Problem,
                                Description = a.Description,
                                StatusId = a.StatusId,
                                PriorityId = a.PriorityId,
                                ProgressDate = a.ProgressDate,
                                ProgressRemarks = a.ProgressRemarks,
                                CompleteDate = a.CompleteDate,
                                CompleteRemarks = a.CompleteRemarks,
                                CloseDate = a.CloseDate,
                                CloseRemarks = a.CloseRemarks
                        };
            return allData.ToList();
        }

        public List<FormulationRequestReportDataModel> GetFormulationRequestData(int Year,int Month,int LineId)
        {
            var allData = from a in this._formulationRequestRepository.Table
                          join b in this._userRepository.Table on a.CreateBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          where a.LineId == (LineId == 0 ? a.LineId : LineId)
                          && a.CreateDate.Year == Year
                          && a.CreateDate.Month == (Month == 0 ? a.CreateDate.Month : Month)
                          select new FormulationRequestReportDataModel
                          {
                              LotNo=a.LotNo,
                              LOTSize=a.LOTSize,
                              GradeName=a.GradeName,
                              QtyToProduce=a.QtyToProduce,
                              ColorSTD=a.ColorSTD,
                              StatusName=a.FormulationRequestsStatu.StatusName,
                              QAStatusName= a.QAStatusId == null ? "N A " : a.QAStatu.Name,
                              CreateDate=a.CreateDate,
                              CreateBy=b.UserName,
                              LineName= a.Line.Name,
                          };
            return allData.ToList();
        }

        public List<QualityObjectiveDataModel> GetQOSReportData(int Year)
        {
            DateTime previousMonth = DateTime.Now.AddMonths(-1);
            int lastMonth;
            int lastMonthDay;
            long idealTotaltime = 0;
            var lineid = this._qOSLineRepository.Table.Select(s => s.LineId).ToList();
            //lastMonth = DateTime.Now.Month;
            //lastMonthDay = DateTime.DaysInMonth(previousMonth.Year, lastMonth);
            var allData = new List<QualityObjectiveDataModel>();
            for (int i = 1; i <= 12; i++)
            {
                var dt = new DateTime(2000, i, 1);
                lastMonthDay = DateTime.DaysInMonth(previousMonth.Year, i);
                if (this._breakdownReportRepository.Table.Any(w => w.IsDeleted == false && w.Date.Month == i && w.Date.Year == Year && (w.PowerTime == true || w.ProcessTime == true || w.PrvTime == true || w.IdleTime == true)))
                {
                    if (this._breakdownReportRepository.Table.Any(w => lineid.Contains(w.LineId) && w.IsDeleted == false && w.Date.Month == i && w.Date.Year == Year && (w.PowerTime == true || w.ProcessTime == true || w.PrvTime == true || w.IdleTime == true)))
                    {
                        idealTotaltime = this._breakdownReportRepository.Table.Where(w => lineid.Contains(w.LineId) && w.IsDeleted == false && w.Date.Month == i && w.Date.Year == Year && (w.PowerTime == true || w.ProcessTime == true || w.PrvTime == true || w.IdleTime == true)).Sum(s => (long)s.TotalTime);
                    }
                    else
                        idealTotaltime = 0;
                }
                double idealTotalHours = TimeSpan.FromMilliseconds(idealTotaltime).TotalHours;
                double totalHoursInMonth = (24 * lastMonthDay * 8);
                double availableTime = (totalHoursInMonth - idealTotalHours);

                //calculate breakdown time
                long breakdownTotaltime = 0;
                if (this._breakdownReportRepository.Table.Any(w => w.IsDeleted == false && w.Date.Month == i && w.Date.Year == Year && (w.ElectricalTime == true || w.MechTime == true || w.InstrTime == true || w.UtilityTime == true)))
                {
                    if (this._breakdownReportRepository.Table.Any(w => lineid.Contains(w.LineId) && w.IsDeleted == false && w.Date.Month == i && w.Date.Year == Year && (w.ElectricalTime == true || w.MechTime == true || w.InstrTime == true || w.UtilityTime == true)))
                    {
                        breakdownTotaltime = this._breakdownReportRepository.Table.Where(w => lineid.Contains(w.LineId) && w.IsDeleted == false && w.Date.Month == i && w.Date.Year == Year && (w.ElectricalTime == true || w.MechTime == true || w.InstrTime == true || w.UtilityTime == true)).Sum(s => (long)s.TotalTime);
                    }
                    else
                        breakdownTotaltime = 0;
                }

                double breakdownTime = TimeSpan.FromMilliseconds(breakdownTotaltime).TotalHours;

                //calculate breakdown percentage
                double breakdownPerc = (breakdownTime * 100) / availableTime;
                var val = breakdownPerc.ToString("F2");
                double Percentage = double.Parse(val, System.Globalization.CultureInfo.InvariantCulture);

                allData.Add(new QualityObjectiveDataModel { Year = Year, MonthName = dt.ToString("MMMM"), QualityObjective = Percentage });
            }
            return allData.ToList();
        }

        public List<PreventiveAuditReportModel> GetPMAuditReportData(int Year, int Month, int Status)
        {
            var allData = from a in this._preventiveMaintenanceRepository.Table
                          select new { preventive = a };

            if (Status == 1) // Add
            {
                var finalData = from a in allData
                                join b in this._userRepository.Table on a.preventive.CreatedBy equals b.Id into j1
                                from b in j1.DefaultIfEmpty()
                                where a.preventive.CreatedOn.Value.Year == Year
                                && a.preventive.CreatedOn.Value.Month == (Month == 0 ? a.preventive.CreatedOn.Value.Month : Month)
                                orderby a.preventive.CreatedOn
                                select new PreventiveAuditReportModel
                                {
                                    PlantName = a.preventive.Plant.Name,
                                    LineName = a.preventive.Line.Name,
                                    MachineName = a.preventive.Machine.Name,
                                    WorkName = a.preventive.WorkDescription,
                                    AuditStatus = a.preventive.CreatedBy != null ? "Added" : " ",
                                    ScheduleTypeName = a.preventive.PreventiveScheduleType.Description,
                                    Interval = a.preventive.Interval,
                                    CreatedOn = a.preventive.CreatedOn,
                                    UpdatedDate = a.preventive.CreatedOn,
                                    UserName = b.UserName
                                };
                return finalData.ToList();
            }
            else if (Status == 2) // Update
            {
                var finalData = from a in allData
                                join b in this._userRepository.Table on a.preventive.UpdatedBy equals b.Id into j2
                                from b in j2.DefaultIfEmpty()
                                where a.preventive.UpdatedOn.Value.Year == Year
                                && a.preventive.UpdatedOn.Value.Month == (Month == 0 ? a.preventive.UpdatedOn.Value.Month : Month)
                                orderby a.preventive.UpdatedOn
                                select new PreventiveAuditReportModel
                                {
                                    PlantName = a.preventive.Plant.Name,
                                    LineName = a.preventive.Line.Name,
                                    MachineName = a.preventive.Machine.Name,
                                    WorkName = a.preventive.WorkDescription,
                                    AuditStatus = a.preventive.CreatedBy != null ? "Updated" : " ",
                                    ScheduleTypeName = a.preventive.PreventiveScheduleType.Description,
                                    Interval = a.preventive.Interval,
                                    CreatedOn = a.preventive.UpdatedOn,
                                    UpdatedDate = a.preventive.UpdatedOn,
                                    UserName = b.UserName
                                };
                return finalData.ToList();
            }
            else if (Status == 3) // Delete
            {
                var finalData = from a in allData
                                join b in this._userRepository.Table on a.preventive.IsDeletedBy equals b.Id into j3
                                from b in j3.DefaultIfEmpty()
                                where a.preventive.IsDeletedOn.Value.Year == Year
                                && a.preventive.IsDeletedOn.Value.Month == (Month == 0 ? a.preventive.IsDeletedOn.Value.Month : Month)
                                orderby a.preventive.IsDeletedOn
                                select new PreventiveAuditReportModel
                                {
                                    PlantName = a.preventive.Plant.Name,
                                    LineName = a.preventive.Line.Name,
                                    MachineName = a.preventive.Machine.Name,
                                    WorkName = a.preventive.WorkDescription,
                                    AuditStatus = a.preventive.CreatedBy != null ? "Deleted" : "N A",
                                    ScheduleTypeName = a.preventive.PreventiveScheduleType.Description,
                                    Interval = a.preventive.Interval,
                                    CreatedOn = a.preventive.IsDeletedOn,
                                    UpdatedDate = a.preventive.IsDeletedOn,
                                    UserName = b.UserName
                                };
                return finalData.ToList();
            }
            else
            {
                var createData = from a in allData
                                 join b in this._userRepository.Table on a.preventive.CreatedBy equals b.Id into j1
                                 from b in j1.DefaultIfEmpty()
                                 where a.preventive.CreatedOn.Value.Year == Year
                                 && a.preventive.CreatedOn.Value.Month == (Month == 0 ? a.preventive.CreatedOn.Value.Month : Month)
                                 orderby a.preventive.CreatedOn
                                 select new PreventiveAuditReportModel
                                 {
                                     PlantName = a.preventive.Plant.Name,
                                     LineName = a.preventive.Line.Name,
                                     MachineName = a.preventive.Machine.Name,
                                     WorkName = a.preventive.WorkDescription,
                                     AuditStatus = a.preventive.CreatedBy != null ? "Added" : "N A",
                                     ScheduleTypeName = a.preventive.PreventiveScheduleType.Description,
                                     Interval = a.preventive.Interval,
                                     CreatedOn = a.preventive.CreatedOn,
                                     UpdatedDate = a.preventive.CreatedOn,
                                     UserName = b.UserName
                                 };
                var updateData = from a in allData
                                 join b in this._userRepository.Table on a.preventive.UpdatedBy equals b.Id into j2
                                 from b in j2.DefaultIfEmpty()
                                 where a.preventive.UpdatedOn.Value.Year == Year
                                 && a.preventive.UpdatedOn.Value.Month == (Month == 0 ? a.preventive.UpdatedOn.Value.Month : Month)
                                 orderby a.preventive.UpdatedOn
                                 select new PreventiveAuditReportModel
                                 {
                                     PlantName = a.preventive.Plant.Name,
                                     LineName = a.preventive.Line.Name,
                                     MachineName = a.preventive.Machine.Name,
                                     WorkName = a.preventive.WorkDescription,
                                     AuditStatus = a.preventive.CreatedBy != null ? "Updated" : "N A",
                                     ScheduleTypeName = a.preventive.PreventiveScheduleType.Description,
                                     Interval = a.preventive.Interval,
                                     CreatedOn = a.preventive.UpdatedOn,
                                     UpdatedDate = a.preventive.UpdatedOn,
                                     UserName = b.UserName
                                 };
                var deletedData = from a in allData
                                  join b in this._userRepository.Table on a.preventive.IsDeletedBy equals b.Id into j3
                                  from b in j3.DefaultIfEmpty()
                                  where a.preventive.IsDeletedOn.Value.Year == Year
                                  && a.preventive.IsDeletedOn.Value.Month == (Month == 0 ? a.preventive.IsDeletedOn.Value.Month : Month)
                                  orderby a.preventive.IsDeletedOn
                                  select new PreventiveAuditReportModel
                                  {
                                      PlantName = a.preventive.Plant.Name,
                                      LineName = a.preventive.Line.Name,
                                      MachineName = a.preventive.Machine.Name,
                                      WorkName = a.preventive.WorkDescription,
                                      AuditStatus = a.preventive.CreatedBy != null ? "Deleted" : "N A",
                                      ScheduleTypeName = a.preventive.PreventiveScheduleType.Description,
                                      Interval = a.preventive.Interval,
                                      CreatedOn = a.preventive.IsDeletedOn,
                                      UpdatedDate = a.preventive.IsDeletedOn,
                                      UserName = b.UserName
                                  };
                var list = new List<PreventiveAuditReportModel>();
                list.AddRange(createData);
                list.AddRange(updateData);
                list.AddRange(deletedData);
                return list;
            }
        }

    }
}
