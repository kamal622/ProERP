using ProERP.Core.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.MaintenanceRequest
{
    public class MaintenanceRequestServices
    {
        private readonly IRepository<Data.Models.MaintenanceRequest> _MRepository;
        private readonly IRepository<Data.Models.MaintenancePriorityType> _MPTRepository;
        private readonly IRepository<Data.Models.User> _userRepository;
        private readonly IRepository<Data.Models.UserRole> _userRoles;
        private readonly IRepository<Data.Models.Plant> _plantRepository;
        private readonly IRepository<Data.Models.Line> _lineRepository;
        private readonly IRepository<Data.Models.Machine> _machineRepository;
        private readonly IRepository<Data.Models.BreakDown> _breakdownRepository;
        public MaintenanceRequestServices(IRepository<Data.Models.MaintenanceRequest> mrRepository,
            IRepository<Data.Models.MaintenancePriorityType> mptRepository,
            IRepository<Data.Models.User> userRepository, IRepository<Data.Models.UserRole> userRoles,
            IRepository<Data.Models.Plant> plantRepository, IRepository<Data.Models.Line> lineRepository,
            IRepository<Data.Models.Machine> machineRepository, IRepository<Data.Models.BreakDown> breakdownRepository)
        {
            this._MRepository = mrRepository;
            this._MPTRepository = mptRepository;
            this._userRepository = userRepository;
            this._userRoles = userRoles;
            this._plantRepository = plantRepository;
            this._lineRepository = lineRepository;
            this._machineRepository = machineRepository;
            this._breakdownRepository = breakdownRepository;
        }
        public int Add(Data.Models.MaintenanceRequest MR)
        {
            _MRepository.Insert(MR);
            return MR.Id;
        }
        public List<Data.Models.MaintenanceRequest> GetMRData(int PlantId, int LineId, int MachineId, int PriorityId, int StatusId)
        {
            var allData = from a in this._MRepository.Table
                          join b in this._userRepository.Table on a.RequestBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          join c in this._userRepository.Table on a.AssignTo equals c.Id into j2
                          from c in j2.DefaultIfEmpty()
                          join d in this._plantRepository.Table on a.PlantId equals d.Id into j3
                          from d in j3.DefaultIfEmpty()
                          join e in this._lineRepository.Table on a.LineId equals e.Id into j4
                          from e in j4.DefaultIfEmpty()
                          join g in this._machineRepository.Table on a.MachineId equals g.Id into j5
                          from g in j5.DefaultIfEmpty()
                          select a;

            if (PlantId > 0)
                allData = allData.Where(w => w.PlantId == PlantId);
            if (LineId > 0)
                allData = allData.Where(w => w.LineId == LineId);
            if (MachineId > 0)
                allData = allData.Where(w => w.MachineId == MachineId);
            if (PriorityId > 0)
                allData = allData.Where(w => w.PriorityId == PriorityId);
            if (StatusId > 0)
                allData = allData.Where(w => w.StatusId == StatusId);

            allData = allData.OrderByDescending(o => o.CreatedDate);

            return allData.ToList();

        }

        public Data.Models.MaintenanceRequest GetForId(int Id)
        {
            return this._MRepository.Table.FirstOrDefault(f => f.Id == Id);
        }

        public Data.Models.MaintenanceRequest GetRemarksById(int mrId)
        {
            return this._MRepository.Table.Where(W => W.Id == mrId).FirstOrDefault();
        }

        public int Update(Data.Models.MaintenanceRequest MR)
        {
            Data.Models.MaintenanceRequest oldMR = _MRepository.Table.FirstOrDefault(w => w.Id == MR.Id);

            if (oldMR != null)
            {
                oldMR.RequestDate = MR.RequestDate;
                oldMR.RequestTime = MR.RequestTime;
                oldMR.IsBreakdown = MR.IsBreakdown;
                oldMR.PlantId = MR.PlantId;
                oldMR.LineId = MR.LineId;
                oldMR.MachineId = MR.MachineId;
                oldMR.PriorityId = MR.PriorityId;
                oldMR.Problem = MR.Problem;
                oldMR.Description = MR.Description;
                oldMR.IsCritical = MR.IsCritical;
                oldMR.BreakdownType = MR.BreakdownType;
                _MRepository.Update(oldMR);

                return oldMR.Id;
            }
            else
                return 0;
        }

        public int SaveRemarks(MaintenanceGridModel MR)// MR
        {
            Data.Models.MaintenanceRequest oldMR = _MRepository.Table.FirstOrDefault(w => w.Id == MR.Id);

            if (oldMR != null)
            {
                if (MR.StatusId == 1)
                {
                    oldMR.PriorityId = MR.PriorityId;
                    oldMR.StatusId = 2;
                    oldMR.Problem = MR.Problem;
                    oldMR.Description = MR.Description;
                    oldMR.AssignTo = MR.AssignTo;
                    oldMR.AssignBy = MR.AssignBy;
                    oldMR.AssignDate = DateTime.Now;
                    oldMR.Remarks = MR.Remarks;
                    oldMR.RemarksBy = MR.RemarksBy;
                    oldMR.BreakdownType = MR.BreakdownType;
                    oldMR.RemarksDate = DateTime.Now;
                }
                else if (MR.StatusId == 2) //Progress
                {
                    oldMR.StatusId = 3;
                    oldMR.ProgressBy = MR.ProgressBy;
                    oldMR.ProgressDate = DateTime.Now;
                    oldMR.ProgressRemarks = MR.ProgressRemarks;
                    oldMR.WorkStartDate = MR.WorkStartDate;
                    oldMR.WorkStartTime = MR.WorkStartTime.Value.TimeOfDay;
                }
                else if (MR.StatusId == 3)//Complete
                {
                    oldMR.StatusId = 4;
                    oldMR.WorkEndDate = MR.WorkEndDate;
                    oldMR.WorkEndTime = MR.WorkEndTime.Value.TimeOfDay;
                    oldMR.CompleteBy = MR.CompleteBy;
                    oldMR.CompleteDate = DateTime.Now;
                    oldMR.CompleteRemarks = MR.CompleteRemarks;
                }
                else if (MR.StatusId == 4)//close
                {
                    oldMR.StatusId = 5;
                    oldMR.CloseDate = DateTime.Now;
                    oldMR.CloseBy = MR.CloseBy;
                    oldMR.CloseRemarks = MR.CloseRemarks;
                    TimeSpan rtime = oldMR.RequestTime;
                    DateTime requestDate = oldMR.RequestDate + rtime;
                    var totaltime = (oldMR.WorkEndTime - rtime).Value.TotalMilliseconds;
                    DateTime? endDate = oldMR.WorkEndDate + oldMR.WorkEndTime;
                    var day = oldMR.WorkEndDate - oldMR.RequestDate;
                    if (oldMR.IsBreakdown == true)
                    {
                        //if(oldMR.WorkEndDate > oldMR.RequestDate)
                        //{
                        var endTime = new TimeSpan(23, 59, 59);
                        totaltime = (endTime - rtime).TotalMilliseconds;
                        for (int i = 0; i <= day.Value.Days; i++)
                        {
                            this._breakdownRepository.Insert(new Data.Models.BreakDown
                            {
                                PlantId = oldMR.PlantId ?? default(int),
                                LineId = oldMR.LineId ?? default(int),
                                MachineId = oldMR.MachineId ?? default(int),
                                Date = requestDate,
                                StartTime = rtime,
                                //EndTime = oldMR.WorkEndTime ?? default(TimeSpan),
                                EndTime = day.Value.Days == 0 ? oldMR.WorkEndTime ?? default(TimeSpan) : endTime,
                                //TotalTime = unchecked((int)totaltime),
                                TotalTime = day.Value.Days == 0 ? unchecked((int)(oldMR.WorkEndTime - rtime).Value.TotalMilliseconds) : unchecked((int)totaltime),
                                ElectricalTime = oldMR.BreakdownType == 1 ? true : false,
                                MechTime = oldMR.BreakdownType == 2 ? true : false,
                                InstrTime = oldMR.BreakdownType == 3 ? true : false,
                                FailureDescription = oldMR.Description,
                                CreatedBy = oldMR.CreatedBy,
                                CreatedDate = oldMR.CreatedDate,
                            });
                            requestDate = requestDate.AddDays(1);
                            if (i == (day.Value.Days - 1))
                            {
                                rtime = new TimeSpan(00, 00, 00);
                                endTime = oldMR.WorkEndTime ?? default(TimeSpan);
                                totaltime = (endTime - rtime).TotalMilliseconds;
                            }
                            else
                            {
                                rtime = new TimeSpan(00, 00, 00);
                                endTime = new TimeSpan(23, 59, 59);
                                totaltime = (endTime - rtime).TotalMilliseconds;
                            }
                        }
                    }
                }

                _MRepository.Update(oldMR);

                return oldMR.Id;
            }
            else
                return 0;
        }

        //public int StartWork(int Id, DateTime StartDate, int userId,TimeSpan starttime)//DateTime StartTime,
        //{
        //    Data.Models.MaintenanceRequest oldMR = _MRepository.Table.FirstOrDefault(w => w.Id == Id);

        //    if (oldMR != null)
        //    {
        //        oldMR.StatusId = 3;
        //        oldMR.ProgressBy = userId;
        //        oldMR.ProgressDate = DateTime.Now;
        //        oldMR.WorkStartDate = StartDate;
        //        oldMR.WorkStartTime = starttime;
        //        _MRepository.Update(oldMR);

        //        return oldMR.Id;
        //    }
        //    else
        //        return 0;
        //}

        //public int EndWork(int Id, DateTime EndDate, int userId, TimeSpan endtime)
        //{
        //    Data.Models.MaintenanceRequest oldMR = _MRepository.Table.FirstOrDefault(w => w.Id == Id);

        //    if (oldMR != null)
        //    {
        //        oldMR.StatusId = 4;
        //        oldMR.ProgressBy = userId;
        //        oldMR.ProgressDate = DateTime.Now;
        //        oldMR.WorkEndDate = EndDate;
        //        oldMR.WorkEndTime = endtime;
        //        _MRepository.Update(oldMR);

        //        return oldMR.Id;
        //    }
        //    else
        //        return 0;
        //}

        //public int UpdateApproveTask(int MRId, DateTime auditDate, int StatusId, int userId)
        //{
        //    Data.Models.MaintenanceRequest oldMR = _MRepository.Table.FirstOrDefault(w => w.Id == MRId);
        //    if (oldMR != null)
        //    {
        //        oldMR.AuditDate = auditDate;
        //        oldMR.AuditBy = userId;
        //        oldMR.StatusId = StatusId;

        //        _MRepository.Update(oldMR);
        //        return oldMR.Id;
        //    }
        //    else
        //        return 0;
        //}
        //public int UpdateAuditTask(int ATId, string WorkConducted, int StatusId, DateTime? CloseDateTime, DateTime updatedate, int userId)
        //{
        //    Data.Models.MaintenanceRequest oldAT = _MRepository.Table.FirstOrDefault(w => w.Id == ATId);
        //    if (oldAT != null)
        //    {
        //        oldAT.WorkConducted = WorkConducted;
        //        oldAT.StatusId = StatusId;
        //        oldAT.CloseDateTime = CloseDateTime;
        //        oldAT.UpdateBy = userId;
        //        oldAT.UpdateDate = updatedate;
        //        _MRepository.Update(oldAT);
        //        return oldAT.Id;
        //    }
        //    else
        //        return 0;
        //}
        //public int UpdateRejectTask(int MRId, DateTime auditDate, int StatusId, int userId)
        //{
        //    Data.Models.MaintenanceRequest oldMR = _MRepository.Table.FirstOrDefault(w => w.Id == MRId);
        //    if (oldMR != null)
        //    {
        //        oldMR.AuditDate = auditDate;
        //        oldMR.AuditBy = userId;
        //        oldMR.StatusId = StatusId;

        //        _MRepository.Update(oldMR);
        //        return oldMR.Id;
        //    }
        //    else
        //        return 0;
        //}

        // GET AUDIT TASK DATA
        //public List<Data.Models.MaintenanceRequest> GetATData(string Name, int PlantId, int PriorityId, int StatusId)
        //{
        //    var allData = from a in this._MRepository.Table
        //                  select a;

        //    if (!string.IsNullOrEmpty(Name))
        //        allData = allData.Where(w => w.Machine.Name.Contains(Name));
        //    if (PlantId > 0)
        //        allData = allData.Where(w => w.PlantId == PlantId);
        //    if (PriorityId > 0)
        //        allData = allData.Where(w => w.PriorityId == PriorityId);
        //    if (StatusId > 0)
        //        allData = allData.Where(w => w.StatusId == StatusId);

        //    return allData.ToList();

        //}
        public List<MaintenanceGridModel> GetDashboardData(int userId, int StatusId, int pagenum, int pagesize, string sortdatafield, string sortorder, out int allDataCount, System.Collections.Specialized.NameValueCollection query)
        {
            var totalrecord = 0;
            var filtersCount = int.Parse(query.GetValues("filterscount")[0]);
            var allData = from a in this._MRepository.Table
                          join b in this._userRepository.Table on a.RequestBy equals b.Id into j1
                          from b in j1.DefaultIfEmpty()
                          join c in this._userRepository.Table on a.AssignTo equals c.Id into j2
                          from c in j2.DefaultIfEmpty()
                          join d in this._plantRepository.Table on a.PlantId equals d.Id into j3
                          from d in j3.DefaultIfEmpty()
                          join e in this._lineRepository.Table on a.LineId equals e.Id into j4
                          from e in j4.DefaultIfEmpty()
                          join g in this._machineRepository.Table on a.MachineId equals g.Id into j5
                          from g in j5.DefaultIfEmpty()
                          select new { a, b, c, d, e, g };

            if (StatusId == 1) //open
            {
                bool isLavel2 = this._userRoles.Table.Any(w => w.UserId == userId);
                if (isLavel2)
                    allData = allData.Where(w => w.a.StatusId == 1);
            }
            else if (StatusId == 2) //Assign 
            {
                bool isLavel3 = this._userRoles.Table.Any(w => w.UserId == userId);
                if (isLavel3)
                    allData = allData.Where(w => w.a.StatusId == 2 && w.a.AssignTo == userId).OrderByDescending(o => o.a.AssignDate);
            }
            else if (StatusId == 3) //Progress
            {
                bool isLavel3 = this._userRoles.Table.Any(w => w.UserId == userId);
                if (isLavel3)
                    allData = allData.Where(w => w.a.StatusId == 3 && w.a.AssignTo == userId).OrderByDescending(o => o.a.ProgressDate);
            }
            else if (StatusId == 4)  //completed
            {
                bool isLavel3 = this._userRoles.Table.Any(w => w.UserId == userId);
                if (isLavel3)
                    allData = allData.Where(w => w.a.StatusId == 4).OrderByDescending(o => o.a.CompleteDate);
            }
            else  //other
            {
                if (!string.IsNullOrEmpty(sortdatafield) && !string.IsNullOrEmpty(sortorder))
                {
                    if (sortorder == "asc")
                    {
                        switch (sortdatafield)
                        {
                            case "SerialNo":
                                allData = allData.OrderBy(o => o.a.SerialNo);
                                break;
                            case "PlantName":
                                allData = allData.OrderBy(o => o.d.Name);
                                break;
                            case "LineName":
                                allData = allData.OrderBy(o => o.e.Name);
                                break;
                            case "MachineName":
                                allData = allData.OrderBy(o => o.g.Name);
                                break;
                            case "Problem":
                                allData = allData.OrderBy(o => o.a.Problem);
                                break;
                            case "Priority":
                                allData = allData.OrderBy(o => o.a.MaintenancePriorityType.Description);
                                break;
                            case "Status":
                                allData = allData.OrderBy(o => o.a.MaintanceRequestStatu.StatusName);
                                break;
                            case "RequestUserName":
                                allData = allData.OrderBy(o => o.b.UserName);
                                break;
                            case "RequestDate":
                                allData = allData.OrderBy(o => o.a.RequestDate);
                                break;
                            case "AssignUserName":
                                allData = allData.OrderBy(o => o.b.UserName);
                                break;
                            default:
                                allData = allData.OrderBy(o => o.a.CreatedDate);
                                break;
                        }
                    }
                    else
                    {
                        switch (sortdatafield)
                        {
                            case "SerialNo":
                                allData = allData.OrderByDescending(o => o.a.SerialNo);
                                break;
                            case "PlantName":
                                allData = allData.OrderByDescending(o => o.d.Name);
                                break;
                            case "LineName":
                                allData = allData.OrderByDescending(o => o.e.Name);
                                break;
                            case "MachineName":
                                allData = allData.OrderByDescending(o => o.g.Name);
                                break;
                            case "Problem":
                                allData = allData.OrderByDescending(o => o.a.Problem);
                                break;
                            case "Priority":
                                allData = allData.OrderByDescending(o => o.a.MaintenancePriorityType.Description);
                                break;
                            case "Status":
                                allData = allData.OrderByDescending(o => o.a.MaintanceRequestStatu.StatusName);
                                break;
                            case "RequestUserName":
                                allData = allData.OrderByDescending(o => o.b.UserName);
                                break;
                            case "RequestDate":
                                allData = allData.OrderByDescending(o => o.a.RequestDate);
                                break;
                            case "AssignUserName":
                                allData = allData.OrderByDescending(o => o.b.UserName);
                                break;
                            default:
                                allData = allData.OrderByDescending(o => o.a.CreatedDate);
                                break;
                        }
                    }
                }
                else
                    allData = allData.OrderByDescending(o => o.a.CreatedDate);

                if (query != null)
                {
                    for (var i = 0; i < filtersCount; i += 1)
                    {
                        var filterValue = query.GetValues("filtervalue" + i)[0];
                        var filterCondition = query.GetValues("filtercondition" + i)[0];
                        var filterDataField = query.GetValues("filterdatafield" + i)[0];
                        var filterOperator = query.GetValues("filteroperator" + i)[0];
                        if (!string.IsNullOrEmpty(filterDataField))
                        {
                            switch (filterDataField)
                            {
                                case "SerialNo":
                                    allData = allData.Where(w => filterValue.Contains(w.a.SerialNo));
                                    break;
                                case "PlantName":
                                    allData = allData.Where(w => (w.d.Name.Contains(filterValue)));// || (w.d.Name.ToString().EndsWith(filterValue))
                                    break;
                                case "LineName":
                                    allData = allData.Where(w => (w.e.Name.Contains(filterValue)));
                                    break;
                                case "MachineName":
                                    allData = allData.Where(w => (w.g.Name.Contains(filterValue)));
                                    break;
                                case "Problem":
                                    allData = allData.Where(w => (w.a.Problem.Contains(filterValue)));
                                    break;
                                case "Priority":
                                    allData = allData.Where(w => (w.a.MaintenancePriorityType.Description.Contains(filterValue)));
                                    break;
                                case "Status":
                                    allData = allData.Where(w => (w.a.MaintanceRequestStatu.StatusName.Contains(filterValue)));
                                    break;
                                case "RequestUserName":
                                    allData = allData.Where(w => (w.b.UserName.Contains(filterValue)));
                                    break;
                                case "RequestDate":
                                    DateTime dt = DateTime.Parse(filterValue);
                                    allData = allData.Where(w => w.a.RequestDate == dt.Date);
                                    break;
                                case "AssignUserName":
                                    allData = allData.Where(w => filterValue.Contains(w.b.UserName));
                                    break;
                                default:
                                    allData = allData.OrderByDescending(o => o.a.CreatedDate);
                                    break;
                            }
                        }
                    }
                }

                bool isLavel3 = this._userRoles.Table.Any(w => w.UserId == userId && w.Role.Name == "Lavel3");
                if (isLavel3)
                {
                    allData = allData.Where(w => (w.a.AssignTo == userId && w.a.StatusId != 1));
                }
                else
                {
                    allData = allData.Where(w => (w.a.StatusId != 1));
                    totalrecord = allData.Count();
                    allData = allData.Skip(pagenum * pagesize).Take(pagesize);
                }
            }

            var finalData = from f in allData
                                // orderby f.a.CreatedDate descending 
                            select new MaintenanceGridModel
                            {
                                Id = f.a.Id,
                                SerialNo = f.a.SerialNo,
                                StatusId = f.a.StatusId,
                                PlantId = (f.a.Plant == null ? 0 : f.a.PlantId.Value),
                                PlantName = (f.a.Plant == null ? "NA" : f.a.Plant.Name),
                                LineId = (f.a.Plant == null ? 0 : f.a.PlantId.Value),
                                LineName = (f.a.Line == null ? "NA" : f.a.Line.Name),
                                MachineId = (f.a.Machine == null ? 0 : f.a.MachineId.Value),
                                MachineName = (f.a.Machine == null ? "NA" : f.a.Machine.Name),
                                Problem = f.a.Problem,
                                RequestBy = f.a.RequestBy,
                                RequestDate = f.a.RequestDate,
                                IsBreakdown = f.a.IsBreakdown,
                                Priority = f.a.MaintenancePriorityType.Description,
                                Status = f.a.MaintanceRequestStatu.StatusName,
                                AssignTo = f.a.AssignTo,
                                RequestUserName = f.b.UserName,
                                AssignUserName = f.c.UserName,
                                CreatedDate = f.a.CreatedDate
                            };
            allDataCount = totalrecord;
            return finalData.ToList();
        }

        //public int GetAuditTaskPendingCount()
        //{
        //    var allData = from a in this._MRepository.Table
        //                  where a.StatusId == 1
        //                  select a;
        //    return allData.Count();
        //}
        //public int GetAuditTaskAllCount()
        //{
        //    var allData = from a in this._MRepository.Table
        //                  where a.StatusId == 3 || a.StatusId == 5 || a.StatusId == 6
        //                  select a;
        //    return allData.Count();
        //}

        public int GetMaintenanceRequestOpenCount(int userId)
        {
            var allData = from a in this._MRepository.Table
                          where a.StatusId == 2 && a.AssignTo == userId
                          select a;
            return allData.Count();
        }

        public int GetMaintenanceRequestInProcessCount(int userId)
        {
            var allData = from a in this._MRepository.Table
                          where a.StatusId == 3 && a.AssignTo == userId
                          select a;
            return allData.Count();
        }


        public List<MaintenanceRequestReportDataSet> GetMaintenanceRequestById(int Id)
        {
            var username = this._userRepository.Table.Where(w => w.Id > 2);
            var maintenance = this._MRepository.Table.FirstOrDefault(w => w.Id == Id);
            DateTime rDate = maintenance.RequestDate;
            TimeSpan rTime = maintenance.RequestTime;
            DateTime? SDate = maintenance.WorkStartDate;
            TimeSpan? sTime = maintenance.WorkStartTime;
            DateTime? EDate = maintenance.WorkEndDate;
            TimeSpan? ETime = maintenance.WorkEndTime;

            rDate = rDate + rTime;
            SDate = SDate + sTime;
            EDate = EDate + ETime;

            var allData = from a in this._MRepository.Table
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
                          where a.Id == Id
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
                              PlantName = a.Plant.Name,
                              LineId = a.LineId,
                              LineName = a.Line.Name,
                              MachineId = a.MachineId,
                              MachineName = a.Machine.Name,
                              RequestDate = a.RequestDate,
                              RequestTime = a.RequestTime,
                              RequestDateTime = rDate,
                              RequestUserName = b.UserProfile == null ? b.UserName : b.UserProfile.FirstName + " " + b.UserProfile.LastName,
                              //RequestUserName = b.UserName,
                              Remarks = a.Remarks,
                              AssignDate = a.AssignDate,
                              AssignUserName = c.UserProfile == null ? c.UserName : c.UserProfile.FirstName + " " + c.UserProfile.LastName,
                              //AssignUserName = c.UserName,
                              AssignByUserName = d.UserProfile == null ? d.UserName : d.UserProfile.FirstName + " " + d.UserProfile.LastName,
                              //AssignByUserName = d.UserName,
                              WorkStartDate = a.WorkStartDate,
                              WorkStartTime = a.WorkStartTime,
                              WorkStartDateTime = SDate,
                              WorkEndDate = a.WorkEndDate,
                              WorkEndTime = a.WorkEndTime,
                              WorkEndDateTime = EDate,
                              ProgressUserName = e.UserProfile == null ? e.UserName : e.UserProfile.FirstName + " " + e.UserProfile.LastName,
                              //ProgressUserName = e.UserName,
                              CompleteUserName = f.UserProfile == null ? f.UserName : f.UserProfile.FirstName + " " + f.UserProfile.LastName,
                              //CompleteUserName = f.UserName,
                              CloseUserName = g.UserProfile == null ? g.UserName : g.UserProfile.FirstName + " " + g.UserProfile.LastName,
                              //CloseUserName = g.UserName,
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

        public bool BreakdownStatus(int Id)
        {
            return this._MRepository.Table.Where(w => w.Id == Id).Select(s => s.IsBreakdown).FirstOrDefault();
        }

        //public int[] Delete(int[] ids)
        //{
        //    List<int> MRIds = new List<int>();

        //    foreach (int id in ids)
        //    {

        //        Data.Models.MaintenanceRequest MaintenanceRequest = _MRepository.Table.FirstOrDefault(w => w.Id == id);
        //        _MRepository.Delete(MaintenanceRequest);
        //    }
        //    return MRIds.ToArray();
        //}


    }
}