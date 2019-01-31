using ProERP.Core.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.PreventiveMaintenance
{
    public class PreventiveMaintenanceService
    {

        private readonly IRepository<Data.Models.PreventiveMaintenance> _PMRepository;
        private readonly IRepository<Data.Models.UserAssignment> _UARepository;
        private readonly IRepository<Data.Models.PreventiveReviewHistory> _PRHRepository;
        private readonly IRepository<Data.Models.User> _userRepository;
        private readonly IRepository<Data.Models.UserRole> _userRoles;
        private readonly IRepository<Data.Models.Line> _lineRepository;
        private readonly IRepository<Data.Models.PreventiveHoldHistory> _PHHRepository;
        public PreventiveMaintenanceService(IRepository<Data.Models.PreventiveMaintenance> pmRepository,
            IRepository<Data.Models.UserAssignment> uaRepository,
            IRepository<Data.Models.PreventiveReviewHistory> prhRepository,
            IRepository<Data.Models.User> userRepository,
            IRepository<Data.Models.UserRole> userRoles,
            IRepository<Data.Models.Line> lineRepository,
            IRepository<Data.Models.PreventiveHoldHistory> phhRepository)
        {
            this._PMRepository = pmRepository;
            this._UARepository = uaRepository;
            this._PRHRepository = prhRepository;
            this._userRepository = userRepository;
            this._userRoles = userRoles;
            this._lineRepository = lineRepository;
            this._PHHRepository = phhRepository;
        }
        public int Add(Data.Models.PreventiveMaintenance PM)
        {
            _PMRepository.Insert(PM);
            return PM.Id;
        }
        public Data.Models.PreventiveMaintenance GetForId(int Id)
        {
            return this._PMRepository.Table.FirstOrDefault(f => f.Id == Id);
        }

        public int[] GetPreventiveIdsForLine(int lineId)
        {
            return this._PMRepository.Table.Where(f => f.LineId == lineId && f.ScheduleType == 5 && f.Line.IsActive && f.Machine.IsActive && f.IsDeleted == false).Select(s => s.Id).ToArray();
        }

        public void UpdateShutdownNextReviewDateForLine(int lineId)
        {
            var allData = this._PMRepository.Table.Where(w => w.LineId == lineId && w.ScheduleType == 5 && w.Line.IsActive && w.Machine.IsActive).ToArray();

            for (int i = 0; i < allData.Length; i++)
                allData[i].NextReviewDate = DateTime.UtcNow.Date;

            this._PMRepository.Update(allData);
        }

        public int[] GetPreventiveIdsForMachine(int machineId)
        {
            return this._PMRepository.Table.Where(f => f.MachineId == machineId && f.ScheduleType == 5 && f.Machine.IsActive && f.Line.IsActive && f.IsDeleted == false).Select(s => s.Id).ToArray();
        }

        public void UpdateShutdownNextReviewDateForMachine(int machineId)
        {
            var allData = this._PMRepository.Table.Where(w => w.MachineId == machineId && w.Machine.IsActive && w.Line.IsActive && w.ScheduleType == 5).ToArray();

            for (int i = 0; i < allData.Length; i++)
                allData[i].NextReviewDate = DateTime.UtcNow.Date;

            this._PMRepository.Update(allData);
        }

        public List<Data.Models.PreventiveMaintenance> GetPMData(string Name, int PlantId, int LineId, int ScheduleType)
        {
            var allData = from a in this._PMRepository.Table
                          where a.IsDeleted == false
                          select a;

            if (!string.IsNullOrEmpty(Name))
                allData = allData.Where(w => w.Machine.Name.Contains(Name));
            if (PlantId > 0)
                allData = allData.Where(w => w.PlantId == PlantId);
            if (LineId > 0)
                allData = allData.Where(w => w.LineId == LineId);
            if (ScheduleType > 0)
                allData = allData.Where(w => w.ScheduleType == ScheduleType);


            return allData.ToList();

        }

        public VerifiedPreventiveData[] GetVPData(int PlantId, int LineId,int MachineId, int ScheduleType,int Verified, DateTime FromDate, DateTime ToDate)
        {
            var allData = from a in this._PMRepository.Table
                          join b in this._PRHRepository.Table on a.Id equals b.PreventiveId
                          join c in this._userRepository.Table on b.VerifyBy equals c.Id into j2
                          from c in j2.DefaultIfEmpty()
                          join d in this._userRepository.Table on b.ReviewBy equals d.Id into j3
                          from d in j3.DefaultIfEmpty()
                          where a.IsDeleted == false && b.ReviewBy != null
                          select new { Preventive = a , ReviewHistory = b, verify = c,Review = d };

            if (PlantId > 0)
                allData = allData.Where(w => w.Preventive.PlantId ==  PlantId);
            if (LineId > 0)
                allData = allData.Where(w => w.Preventive.LineId == LineId);
            if(MachineId > 0)
                allData = allData.Where(w => w.Preventive.MachineId == MachineId);
            if (ScheduleType > 0)
                allData = allData.Where(w => w.Preventive.ScheduleType == ScheduleType);

            if (Verified == 1) //yes
                allData = allData.Where(w => w.ReviewHistory.VerifyBy != null && w.ReviewHistory.VerifyDate != null);
            if (Verified == 2) //no
                allData = allData.Where(w => w.ReviewHistory.VerifyBy == null && w.ReviewHistory.VerifyDate == null);
           
            allData = allData.Where(w => w.ReviewHistory.ReviewDate >= FromDate && w.ReviewHistory.ReviewDate <= ToDate);

            var finalData = from a in allData
                           select new VerifiedPreventiveData
                           {
                               Id= a.ReviewHistory.Id,
                               PlantName = a.Preventive.Plant.Name,
                               LineName=a.Preventive.Line.Name,
                               MachineName=a.Preventive.Machine.Name,
                               WorkName=a.Preventive.WorkDescription,
                               Severity = a.Preventive.Severity == 1 ? "Moderate" : (a.Preventive.Severity == 2) ? "Critical" : "Minor",
                               Interval=a.Preventive.Interval,
                               ScheduleTypeName=a.Preventive.PreventiveScheduleType.Description,
                               ReviewDate =a.ReviewHistory.ReviewDate,
                               ReviewBy= (a.Review.UserName != null) ? a.Review.UserName : "",
                               VerifyDate =a.ReviewHistory.VerifyDate,
                               VerifyBy= (a.verify.UserName != null) ? a.verify.UserName : ""
                           };

            return finalData.ToArray();
        }

        public void VerifyPreventiveData(int[] ids, int userId)
        {
            Data.Models.PreventiveReviewHistory[] reviewHistoryRecords = this._PRHRepository.Table.Where(w => ids.Contains(w.Id)).ToArray();

            for (int i = 0; i < reviewHistoryRecords.Length; i++)
            {
                var existingPRH = reviewHistoryRecords[i];
                if (existingPRH.VerifyBy != null)
                    continue;

                existingPRH.VerifyBy = userId;
                existingPRH.VerifyDate = DateTime.UtcNow;
            }

            _PRHRepository.Update(reviewHistoryRecords);
        }


        public int Update(Data.Models.PreventiveMaintenance PM)
        {
            Data.Models.PreventiveMaintenance oldPM = _PMRepository.Table.FirstOrDefault(w => w.Id == PM.Id);

            if (oldPM != null)
            {

                oldPM.Description = PM.Description;
                //oldPM.LineId = PM.LineId;
                //oldPM.MachineId = PM.MachineId;
                oldPM.Checkpoints = PM.Checkpoints;
                oldPM.ShutdownRequired = PM.ShutdownRequired;
                //oldPM.ScheduleType = PM.ScheduleType;
                //oldPM.ShutdownRequired = PM.ShutdownRequired;
                //oldPM.ScheduleStartDate = PM.ScheduleStartDate;
                oldPM.ScheduleEndDate = PM.ScheduleEndDate;
                //oldPM.NextReviewDate = PM.NextReviewDate;
                //oldPM.WorkId = PM.WorkId;
                oldPM.UpdatedOn = PM.UpdatedOn;
                oldPM.UpdatedBy = PM.UpdatedBy;
                oldPM.WorkDescription = PM.WorkDescription;
                oldPM.PreferredVendorId = PM.PreferredVendorId;
                //oldPM.Interval = PM.Interval;
                oldPM.Severity = PM.Severity;
                oldPM.IsObservation = PM.IsObservation;
                _PMRepository.Update(oldPM);
                return oldPM.Id;
            }
            else
                return 0;
        }
        public void UpdateIsObservation(int PMId, bool IsObservation)
        {
            Data.Models.PreventiveMaintenance oldPM = _PMRepository.Table.Where(w => w.Id == PMId).FirstOrDefault();
            if (oldPM != null)
            {
                oldPM.IsObservation = IsObservation;
                _PMRepository.Update(oldPM);
            }
        }
        public void VerifyOverdueReviews()
        {
            DateTime dtToday = DateTime.UtcNow.Date;
            var allData = from a in this._PMRepository.Table
                          where (a.ScheduleEndDate >= dtToday || a.ScheduleEndDate == null)
                          && a.NextReviewDate < dtToday && a.ScheduleType != 5
                          select a;

            var preventiveData = allData.ToArray();
            foreach (var preventiveMaintenance in preventiveData)
                VerifyNextReviewDate(preventiveMaintenance.Id, preventiveMaintenance.ScheduleType, preventiveMaintenance.Interval, preventiveMaintenance.NextReviewDate);

        }

        public void VerifyNextReviewDate(int PMId, int scheduleType, int interval, DateTime reviewDate)
        {
            DateTime nextReviewDate = DateTime.UtcNow.Date;
            DateTime dateCompare = DateTime.UtcNow.Date;
            switch (scheduleType)
            {
                case 1: // Daily
                    dateCompare = DateTime.UtcNow.Date.AddDays(-1 * (interval - 1));
                    nextReviewDate = reviewDate.AddDays(1 * interval);
                    break;
                case 2: // Weekly
                    int delta = DayOfWeek.Monday - DateTime.UtcNow.Date.DayOfWeek;
                    DateTime monday = DateTime.UtcNow.Date.AddDays(delta);
                    dateCompare = monday.AddDays(-7 * (interval - 1));
                    nextReviewDate = reviewDate.AddDays(7 * interval);
                    break;
                case 3: // Monthly
                    DateTime firstDayofMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                    dateCompare = firstDayofMonth.AddMonths(-1 * (interval - 1));
                    nextReviewDate = reviewDate.AddMonths(1 * interval);
                    break;
                case 4: // Yearly
                    DateTime firstDayofYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
                    dateCompare = firstDayofYear.AddYears(-1 * (interval - 1));
                    nextReviewDate = reviewDate.AddYears(1 * interval);
                    break;
                case 5://Shutdown
                    break;
            }

            if (scheduleType != 5)
            {
                if (!this._PRHRepository.Table.Any(f => f.PreventiveId == PMId && f.ScheduledReviewDate == reviewDate))
                    this._PRHRepository.Insert(new Data.Models.PreventiveReviewHistory { PreventiveId = PMId, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = reviewDate });

                if (!this._PRHRepository.Table.Any(f => f.PreventiveId == PMId && f.ScheduledReviewDate == nextReviewDate))
                    this._PRHRepository.Insert(new Data.Models.PreventiveReviewHistory { PreventiveId = PMId, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = nextReviewDate });

                if (nextReviewDate < dateCompare)
                    VerifyNextReviewDate(PMId, scheduleType, interval, nextReviewDate);
                else
                {
                    Data.Models.PreventiveMaintenance existingPM = _PMRepository.Table.FirstOrDefault(w => w.Id == PMId);
                    existingPM.NextReviewDate = dateCompare;
                    _PMRepository.Update(existingPM);
                }
            }
        }

        public void UpdateReviewDate(int PMId, string Note, int userId, DateTime ReviewDate, bool isOverDue)
        {
            Data.Models.PreventiveMaintenance existingPM = _PMRepository.Table.FirstOrDefault(w => w.Id == PMId);
            
            if (existingPM != null)
            {
                DateTime lastReviewDate = DateTime.UtcNow;
                DateTime nextReviewDate = existingPM.NextReviewDate;

                if (isOverDue)
                    nextReviewDate = DateTime.UtcNow.Date;

                if (existingPM.ScheduleType == 1) // Daily
                {
                    nextReviewDate = nextReviewDate.AddDays(1 * (existingPM.Interval));
                }
                else if (existingPM.ScheduleType == 2) // Weekly
                {
                    nextReviewDate = nextReviewDate.AddDays(7 * (existingPM.Interval));
                }
                else if (existingPM.ScheduleType == 3) //Monthly
                {
                    nextReviewDate = nextReviewDate.AddMonths(1 * (existingPM.Interval));
                }
                else if (existingPM.ScheduleType == 4) // Yearly
                {
                    nextReviewDate = nextReviewDate.AddYears(1 * (existingPM.Interval));
                }
                else if (existingPM.ScheduleType == 6)//Hourly
                {
                    nextReviewDate = nextReviewDate.AddHours(1 * (existingPM.Interval));
                }
                else // Shutdown
                {
                    nextReviewDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                }

                var existingHistrory = this._PRHRepository.Table.FirstOrDefault(f => f.PreventiveId == PMId && f.ScheduledReviewDate == ReviewDate && (f.ShutdownId == null || f.ShutdownId == f.PreventiveMaintenance.PreventiveReviewHistories.Max(m => m.ShutdownId)));


                if (existingHistrory == null)
                    this._PRHRepository.Insert(new Data.Models.PreventiveReviewHistory { PreventiveId = PMId, Notes = Note, ReviewBy = userId, ReviewDate = lastReviewDate, ScheduledReviewDate = ReviewDate, IsLaps = false, IsOverdue = false });
                else
                {
                    existingHistrory.Notes = Note;
                    existingHistrory.ReviewBy = userId;
                    existingHistrory.ReviewDate = lastReviewDate;
                    //existingHistrory.ScheduledReviewDate = existingPM.NextReviewDate;
                    this._PRHRepository.Update(existingHistrory);
                }

                existingPM.LastReviewDate = lastReviewDate;
                existingPM.NextReviewDate = nextReviewDate;
                _PMRepository.Update(existingPM);

                if (isOverDue)
                {
                    //DateTime dtToday = DateTime.UtcNow.Date;
                    //var allData = from a in this._PMRepository.Table
                    //              where a.Id == PMId
                    //              select a;

                    //var preventiveData = this._PMRepository.Table.FirstOrDefault(w => w.Id == PMId);
                    var preventiveHistory = this._PRHRepository.Table.Where(w => w.PreventiveId == PMId && w.ScheduledReviewDate > ReviewDate).OrderBy(o => o.ScheduledReviewDate).ToArray();

                    for (int i = 0; i < preventiveHistory.Length; i++)
                    {
                        var preventiveReviewHistory = preventiveHistory[i];

                        preventiveReviewHistory.ScheduledReviewDate = nextReviewDate;
                        this._PRHRepository.Update(preventiveReviewHistory);
                        //if (existingPM.ScheduleType == 1) // Daily
                        //{
                        //    nextReviewDate = nextReviewDate.AddDays(1 * (existingPM.Interval));
                        //}
                        /*else*/
                        if (existingPM.ScheduleType == 2) // Weekly
                        {
                            nextReviewDate = nextReviewDate.AddDays(7 * (existingPM.Interval));
                        }
                        else if (existingPM.ScheduleType == 3) //Monthly
                        {
                            nextReviewDate = nextReviewDate.AddMonths(1 * (existingPM.Interval));
                        }
                        else if (existingPM.ScheduleType == 4) // Yearly
                        {
                            nextReviewDate = nextReviewDate.AddYears(1 * (existingPM.Interval));
                        }
                        else if (existingPM.ScheduleType == 6)//Hourly
                        {
                            nextReviewDate = nextReviewDate.AddHours(1 * (existingPM.Interval));
                        }
                        else // Shutdown
                        {
                            nextReviewDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                        }
                    }


                }

                //if (existingPM.ScheduleType != 5) // Shutdown
                //    this._PRHRepository.Insert(new Data.Models.PreventiveReviewHistory { PreventiveId = PMId, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = nextReviewDate });
            }
        }

        public int UpdateNextReviewDate(int PMId, int holdId, int holdValue)
        {
            Data.Models.PreventiveMaintenance existingPM = _PMRepository.Table.FirstOrDefault(w => w.Id == PMId);
            if (existingPM != null)
            {
                DateTime nextReviewDate = DateTime.UtcNow.Date;

                switch (existingPM.ScheduleType)
                {
                    case 1:
                        nextReviewDate = existingPM.NextReviewDate.AddDays(holdValue); // Days
                        break;
                    case 2:
                        nextReviewDate = existingPM.NextReviewDate.AddDays(holdValue * 7); // Weeks
                        break;
                    case 3:
                        nextReviewDate = existingPM.NextReviewDate.AddMonths(holdValue); // Months
                        break;
                    case 4:
                        nextReviewDate = existingPM.NextReviewDate.AddYears(holdValue); // Years
                        break;
                }

                var existingHistrory = this._PRHRepository.Table.FirstOrDefault(f => f.PreventiveId == PMId && f.ScheduledReviewDate == existingPM.NextReviewDate);
                existingHistrory.HoldId = holdId;

                existingPM.NextReviewDate = nextReviewDate;
                _PMRepository.Update(existingPM);
                return existingPM.Id;
            }
            else
                return 0;
        }
        public int[] Delete(int[] ids, DateTime deletedon, int userId)
        {
            List<int> PMIds = new List<int>();

            foreach (int id in ids)
            {
                //var existingHistrory = this._PRHRepository.Table.FirstOrDefault(f => f.PreventiveId == id && (f.ReviewDate != null || f.HoldId != null));
                //if (existingHistrory == null)
                //{
                //    var HoldHistory = _PHHRepository.Table.Where(w => w.PreventiveId == id);
                //    _PHHRepository.Delete(HoldHistory);
                //    var ReviewHistory = _PRHRepository.Table.Where(w => w.PreventiveId == id);
                //    _PRHRepository.Delete(ReviewHistory);
                //    var UserAssignment = _UARepository.Table.Where(w => w.PreventiveMaintenanceId == id);
                //    _UARepository.Delete(UserAssignment);
                //    Data.Models.PreventiveMaintenance PreventiveMaintenance = _PMRepository.Table.FirstOrDefault(w => w.Id == id);
                //    _PMRepository.Delete(PreventiveMaintenance);
                //}
                //else
                //{
                Data.Models.PreventiveMaintenance existingPM = _PMRepository.Table.FirstOrDefault(w => w.Id == id);
                existingPM.IsDeleted = true;
                existingPM.IsDeletedOn = deletedon;
                existingPM.IsDeletedBy = userId;
                _PMRepository.Update(existingPM);
                //}
            }
            return PMIds.ToArray();
        }

        public bool GetLineIsActive(int preventiveIds)
        {
            return this._PMRepository.Table.Where(w => w.Id == preventiveIds).Select(s => s.Line.IsActive).FirstOrDefault();
        }
        public bool GetMachineIsActive(int preventiveIds)
        {
            return this._PMRepository.Table.Where(f => f.Id == preventiveIds).Select(s => s.Machine.IsActive).FirstOrDefault();
        }

        //public PreventiveDashboardData[] GetDashboardData(int userId, int PMType)
        //{

        //    bool isLavel3 = this._userRoles.Table.Any(w => w.UserId == userId && w.Role.Name == "Lavel3");


        //    //DateTime dtToday = DateTime.UtcNow.AddDays(1).Date;
        //    DateTime dtToday = DateTime.UtcNow.Date;
        //    DateTime dtNext = DateTime.UtcNow.Date.AddDays(7);
        //    var allData = from a in this._PMRepository.Table
        //                  where (a.ScheduleEndDate >= dtToday || a.ScheduleEndDate == null) && a.IsDeleted == false
        //                  //&& (a.UserAssignments.Any(b => b.UserId == userId) || isLavel3)
        //                  && a.Line.IsActive && a.Machine.IsActive
        //                  select a;

        //    var overDueData = from a in allData
        //                      join b in this._PRHRepository.Table on a.Id equals b.PreventiveId
        //                      where b.ReviewBy == null && a.ScheduleType != 1 && a.ScheduleType != 5
        //                      && ((a.ScheduleType == 2 && dtToday < DbFunctions.AddDays(a.NextReviewDate, 2)) && b.ScheduledReviewDate < a.NextReviewDate && b.ScheduledReviewDate == this._PRHRepository.Table.Where(w => w.PreventiveId == a.Id && w.ScheduledReviewDate < a.NextReviewDate).Max(m => m.ScheduledReviewDate))
        //                      orderby a.NextReviewDate
        //                      select new { PreventiveMaintenance = a, ReviewHistory = b };


        //    if (PMType == 1) // Due Date
        //    {
        //        if (isLavel3)
        //            allData = allData.Where(w => w.UserAssignments.Any(a => a.UserId == userId));
        //        //allData = allData.Where(w => w.NextReviewDate < dtToday && (w.ScheduleEndDate >= dtToday || w.ScheduleEndDate == null) && w.ScheduleType != 5 && w.Line.IsActive && w.Machine.IsActive);                
        //        var finalData = from a in overDueData
        //                        select new PreventiveDashboardData
        //                        {
        //                            Id = a.PreventiveMaintenance.Id,
        //                            PlantId = a.PreventiveMaintenance.PlantId,
        //                            LineId = a.PreventiveMaintenance.LineId,
        //                            MachineId = a.PreventiveMaintenance.MachineId,
        //                            Checkpoints = a.PreventiveMaintenance.Checkpoints,
        //                            NextReviewDate = a.ReviewHistory.ScheduledReviewDate,
        //                            PlantName = a.PreventiveMaintenance.Plant.Name,
        //                            LineName = a.PreventiveMaintenance.Line.Name,
        //                            MachineName = a.PreventiveMaintenance.Machine.Name,
        //                            ScheduleTypeName = a.PreventiveMaintenance.PreventiveScheduleType.Description,
        //                            WorkName = a.PreventiveMaintenance.WorkDescription,
        //                            ScheduleType = a.PreventiveMaintenance.ScheduleType,
        //                            Interval = a.PreventiveMaintenance.Interval,
        //                            Severity = a.PreventiveMaintenance.Severity == 1 ? "Moderate" : (a.PreventiveMaintenance.Severity == 2) ? "Critical" : "Minor",
        //                            IsObservation = a.PreventiveMaintenance.IsObservation
        //                        };
        //        return finalData.ToArray();
        //    }
        //    else if (PMType == 2) // Today
        //    {
        //        if (isLavel3)
        //            allData = allData.Where(w => w.UserAssignments.Any(a => a.UserId == userId));
        //        allData = allData.Where(w => w.NextReviewDate <= dtToday && ((w.ScheduleType == 1 && w.NextReviewDate == dtToday) || (w.ScheduleType == 2 && w.NextReviewDate < DbFunctions.AddDays(dtToday, 7))) && w.ScheduleStartDate <= dtToday && w.ScheduleType != 5 && !overDueData.Select(s => s.PreventiveMaintenance.Id).Contains(w.Id));
        //    }
        //    else if (PMType == 3)// Up Comming
        //    {
        //        if (isLavel3)
        //            allData = allData.Where(w => w.UserAssignments.Any(a => a.UserId == userId));
        //        allData = allData.Where(w => w.ScheduleStartDate <= dtNext && w.NextReviewDate > dtToday && w.NextReviewDate <= dtNext && w.ScheduleType != 5);
        //    }
        //    else // Shutdown
        //    {
        //        allData = allData.Where(w => w.ScheduleType == 5 && w.NextReviewDate != System.Data.SqlTypes.SqlDateTime.MinValue.Value && (w.Line.IsShutdown || w.Machine.IsShutdown));
        //    }
        //    //return allData.OrderByDescending(o => o.NextReviewDate).ToList();            
        //    var finalData2 = from a in allData
        //                     orderby a.NextReviewDate
        //                     select new PreventiveDashboardData
        //                     {
        //                         Id = a.Id,
        //                         PlantId = a.PlantId,
        //                         LineId = a.LineId,
        //                         MachineId = a.MachineId,
        //                         Checkpoints = a.Checkpoints,
        //                         NextReviewDate = a.NextReviewDate,
        //                         PlantName = a.Plant.Name,
        //                         LineName = a.Line.Name,
        //                         MachineName = a.Machine.Name,
        //                         ScheduleTypeName = a.PreventiveScheduleType.Description,
        //                         WorkName = a.WorkDescription,
        //                         ScheduleType = a.ScheduleType,
        //                         Interval = a.Interval,
        //                         Severity = a.Severity == 1 ? "Moderate" : (a.Severity == 2) ? "Critical" : "Minor",
        //                         IsObservation = a.IsObservation
        //                     };

        //    return finalData2.ToArray();
        //}

        public PreventiveDashboardData[] GetDashboardData(int userId, int PMType)
        {

            bool isLavel3 = this._userRoles.Table.Any(w => w.UserId == userId && w.Role.Name == "Lavel3");


            //DateTime dtToday = DateTime.UtcNow.AddDays(1).Date;
            DateTime dtToday = DateTime.UtcNow.Date;

            var allData = from a in this._PMRepository.Table
                          join b in this._PRHRepository.Table on a.Id equals b.PreventiveId
                          where (a.ScheduleEndDate >= dtToday || a.ScheduleEndDate == null) && a.IsDeleted == false
                          &&  b.IsLaps == false && b.ReviewDate == null  /*&& a.Line.IsActive && a.Machine.IsActive*/
                          && b.IsLineActive == true && b.IsMachineActive == true
                          select new { PreventiveMaintenance = a, ReviewHistory = b };

            if (PMType == 1) // Over Dues
            {
                if (isLavel3)
                    allData = allData.Where(w => w.PreventiveMaintenance.UserAssignments.Any(a => a.UserId == userId));

                allData = from a in allData
                          where a.ReviewHistory.IsOverdue == true && a.ReviewHistory.IsLaps == false && a.PreventiveMaintenance.ScheduleType != 5
                          select a;
            }
            else if (PMType == 2) // Current
            {
                if (isLavel3)
                    allData = allData.Where(w => w.PreventiveMaintenance.UserAssignments.Any(a => a.UserId == userId));
                allData = from a in allData
                          where a.ReviewHistory.IsOverdue == false && a.ReviewHistory.IsLaps == false && a.PreventiveMaintenance.ScheduleType != 5
                          select a;
            }
            //else if (PMType == 3)// Up Comming
            //{
            //    if (isLavel3)
            //        allData = allData.Where(w => w.UserAssignments.Any(a => a.UserId == userId));
            //    allData = allData.Where(w => w.ScheduleStartDate <= dtNext && w.NextReviewDate > dtToday && w.NextReviewDate <= dtNext && w.ScheduleType != 5);
            //}
            else // Shutdown
            {
                allData = allData.Where(w => w.PreventiveMaintenance.ScheduleType == 5
                                            && w.PreventiveMaintenance.NextReviewDate != System.Data.SqlTypes.SqlDateTime.MinValue.Value
                                            && (w.PreventiveMaintenance.Line.IsShutdown || w.PreventiveMaintenance.Machine.IsShutdown)
                                            && w.ReviewHistory.ShutdownId == w.PreventiveMaintenance.PreventiveReviewHistories.Max(m => m.ShutdownId));
            }
            var finalData = from a in allData
                            orderby a.PreventiveMaintenance.NextReviewDate
                            select new PreventiveDashboardData
                            {
                                Id = a.PreventiveMaintenance.Id,
                                PlantId = a.PreventiveMaintenance.PlantId,
                                LineId = a.PreventiveMaintenance.LineId,
                                MachineId = a.PreventiveMaintenance.MachineId,
                                Checkpoints = a.PreventiveMaintenance.Checkpoints,
                                NextReviewDate = a.PreventiveMaintenance.NextReviewDate,
                                PlantName = a.PreventiveMaintenance.Plant.Name,
                                LineName = a.PreventiveMaintenance.Line.Name,
                                MachineName = a.PreventiveMaintenance.Machine.Name,
                                ScheduleTypeName = a.PreventiveMaintenance.PreventiveScheduleType.Description,
                                WorkName = a.PreventiveMaintenance.WorkDescription,
                                ScheduleType = a.PreventiveMaintenance.ScheduleType,
                                Interval = a.PreventiveMaintenance.Interval,
                                Severity = a.PreventiveMaintenance.Severity == 1 ? "Moderate" : (a.PreventiveMaintenance.Severity == 2) ? "Critical" : "Minor",
                                IsObservation = a.PreventiveMaintenance.IsObservation
                            };

            return finalData.ToArray();
        }

        // DashBoard Preventive Maintenance Count
        //public int GetPMCounts(int userId, int PMType)
        //{
        //    DateTime dtToday = DateTime.UtcNow.Date;
        //    var allData = from a in this._PMRepository.Table
        //                  where (a.ScheduleEndDate >= dtToday || a.ScheduleEndDate == null) && a.IsDeleted == false
        //                  && a.UserAssignments.Any(b => b.UserId == userId)
        //                  select a;
        //    if (PMType == 1) // Due Date
        //    {
        //        allData = allData.Where(w => w.NextReviewDate < dtToday && (w.ScheduleEndDate >= dtToday || w.ScheduleEndDate == null));
        //    }
        //    else if (PMType == 2) // Today
        //    {
        //        allData = allData.Where(w => w.ScheduleStartDate <= dtToday && w.NextReviewDate == dtToday);
        //    }
        //    return allData.Count();
        //}

        public int GetPMCounts(int userId, int PMType)
        {
            bool isLavel3 = this._userRoles.Table.Any(w => w.UserId == userId && w.Role.Name == "Lavel3");

            DateTime dtToday = DateTime.UtcNow.Date;

            var allData = from a in this._PMRepository.Table
                          join b in this._PRHRepository.Table on a.Id equals b.PreventiveId
                          where (a.ScheduleEndDate >= dtToday || a.ScheduleEndDate == null) && a.IsDeleted == false 
                         /* && a.Line.IsActive && a.Machine.IsActive*/ && b.IsLaps == false && b.ReviewDate == null
                         && b.IsLineActive == true && b.IsMachineActive == true
                          select new { PreventiveMaintenance = a, ReviewHistory = b };

            if (PMType == 1) // Over Dues
            {
                if (isLavel3)
                    allData = allData.Where(w => w.PreventiveMaintenance.UserAssignments.Any(a => a.UserId == userId));

                allData = from a in allData
                          where a.ReviewHistory.IsOverdue == true && a.ReviewHistory.IsLaps == false  && a.PreventiveMaintenance.ScheduleType != 5
                          select a;
            }
            else if (PMType == 2) // Current
            {
                if (isLavel3)
                    allData = allData.Where(w => w.PreventiveMaintenance.UserAssignments.Any(a => a.UserId == userId));
                allData = from a in allData
                          where a.ReviewHistory.IsOverdue == false && a.ReviewHistory.IsLaps == false && a.PreventiveMaintenance.ScheduleType != 5
                          select a;
            }
            else // Shutdown
            {
                allData = allData.Where(w => w.PreventiveMaintenance.ScheduleType == 5
                                            && w.PreventiveMaintenance.NextReviewDate != System.Data.SqlTypes.SqlDateTime.MinValue.Value
                                            && (w.PreventiveMaintenance.Line.IsShutdown || w.PreventiveMaintenance.Machine.IsShutdown)
                                            && w.ReviewHistory.ShutdownId == w.PreventiveMaintenance.PreventiveReviewHistories.Max(m => m.ShutdownId));

            }
            return allData.Count();
        }

        public List<PreventiveTaskReportModel> GetReport1Data(int userId, DateTime FromDate, DateTime ToDate, int ScheduleType, int PlantId, int LinetId, int Activity,string WorkDescription)//
        {
            DateTime dtToday = DateTime.UtcNow.Date;
            var allData = from a in this._PRHRepository.Table
                          where a.ScheduledReviewDate <= dtToday
                          && a.ScheduledReviewDate >= FromDate && a.ScheduledReviewDate <= ToDate
                          && a.PreventiveMaintenance.IsDeleted == false
                          && a.PreventiveMaintenance.PlantId == (PlantId == 0 ? a.PreventiveMaintenance.PlantId : PlantId)
                          && a.PreventiveMaintenance.LineId == (LinetId == 0 ? a.PreventiveMaintenance.LineId : LinetId)
                          && a.IsLineActive == true && a.IsMachineActive == true
                          //&& a.PreventiveMaintenance.UserAssignments.Any(an => an.UserId == userId)
                          //orderby a.ReviewDate descending
                          select a;
            if (!string.IsNullOrEmpty(WorkDescription))
            {
                allData = allData.Where(w => w.PreventiveMaintenance.WorkDescription.Contains(WorkDescription));
            }
            if (Activity == 1)
                allData = allData.Where(w => w.PreventiveMaintenance.ScheduleType != 5);
            else if (Activity == 2)
                allData = allData.Where(w => w.PreventiveMaintenance.ScheduleType != 5 && w.IsOverdue == false && w.IsLaps == false);
            else if (Activity == 3)
                allData = allData.Where(w => w.PreventiveMaintenance.ScheduleType != 5 && w.IsOverdue == true && w.IsLaps == false);
            else
                allData = allData.Where(w => w.PreventiveMaintenance.ScheduleType != 5 && w.IsLaps == true);

            if (userId > 0)
                allData = allData.Where(w => w.PreventiveMaintenance.UserAssignments.Any(a => a.UserId == userId)).Select(s => s);
            if (ScheduleType > 0)
                allData = allData.Where(w => w.PreventiveMaintenance.ScheduleType == ScheduleType).Select(s => s);
            if (PlantId > 0)
                allData = allData.Where(w => w.PreventiveMaintenance.PlantId == PlantId).Select(s => s);
            if (LinetId > 0)
                allData = allData.Where(w => w.PreventiveMaintenance.LineId == LinetId).Select(s => s);

            return allData.OrderBy(o => o.ReviewBy).Select(a => new PreventiveTaskReportModel
            {
                //AssignedUserName =  a.PreventiveMaintenance.UserAssignments.Aggregate("", (acc, t) => (acc == "" ? "" : acc + ",") + t.User.UserName),
                UserName = a.User.UserName,
                ReviewDate = a.ReviewDate,
                Notes = a.Notes,
                NextReviewDate = a.PreventiveMaintenance.NextReviewDate,
                ScheduledReviewDate = a.ScheduledReviewDate,
                MachineName = a.PreventiveMaintenance.Machine.Name,
                LineName = a.PreventiveMaintenance.Line.Name,
                ScheduleTypeName = a.PreventiveMaintenance.PreventiveScheduleType.Description,
                WorkName = a.PreventiveMaintenance.WorkDescription,
                ScheduleType = a.PreventiveMaintenance.ScheduleType,
                Interval = a.PreventiveMaintenance.Interval,
                AssignedTo = a.PreventiveMaintenance.UserAssignments.Select(s => s.User.UserName)
            }).ToList();

        }

        public List<PreventiveSummaryReportModel> GetSummaryReportData(int userId, DateTime FromDate, DateTime ToDate, int ScheduleType, int PlantId, int LinetId)
        {
            var preventiveData = from a in this._PRHRepository.Table
                                 where a.ScheduledReviewDate >= FromDate && a.ScheduledReviewDate <= ToDate && a.PreventiveMaintenance.IsDeleted == false
                                 && a.PreventiveMaintenance.Line.IsActive && a.PreventiveMaintenance.Machine.IsActive && a.PreventiveMaintenance.ScheduleType != 5
                                 && a.IsLineActive == true && a.IsMachineActive == true
                                 select a;

            if (userId > 0)
                preventiveData = preventiveData.Where(w => w.PreventiveMaintenance.UserAssignments.Any(a => a.UserId == userId)).Select(s => s);
            if (ScheduleType > 0)
                preventiveData = preventiveData.Where(w => w.PreventiveMaintenance.ScheduleType == ScheduleType).Select(s => s);
            if (PlantId > 0)
                preventiveData = preventiveData.Where(w => w.PreventiveMaintenance.PlantId == PlantId).Select(s => s);
            if (LinetId > 0)
                preventiveData = preventiveData.Where(w => w.PreventiveMaintenance.LineId == LinetId).Select(s => s);

            var allData = from b in this._lineRepository.Table
                          join j in preventiveData on b.Id equals j.PreventiveMaintenance.LineId into join1
                          from a in join1.DefaultIfEmpty()
                          where b.PlantId == PlantId
                          select new { Preventive = a, Line = b };



            var finalData = from a in allData
                            orderby a.Preventive.ReviewDate
                            group a by new { Id = a.Line.Id, LineName = a.Line.Name } into g
                            select new PreventiveSummaryReportModel
                            {
                                Id = g.Key.Id,
                                LineId = g.Key.Id,
                                LineName = g.Key.LineName,
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

    }
}
