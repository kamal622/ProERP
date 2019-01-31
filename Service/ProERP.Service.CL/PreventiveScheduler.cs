using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProERP.Data;
using ProERP.Core.Data;

namespace ProERP.Service.CL
{
    public class PreventiveScheduler
    {
        public void Start()
        {
            ProERPContext context;
            try
            {
                ProcessHourlySchedules();
                context = new ProERPContext();

                var today = DateTime.Now.Date;

                var schedules = context.PreventiveMaintenances.Where(w => !w.IsDeleted && w.ScheduleType != 5 && w.ScheduleType != 6 && w.NextReviewDate <= today).ToArray();
                for (int i = 0; i < schedules.Length; i++)
                {
                    var schedule = schedules[i];
                    var line = context.Lines.FirstOrDefault(a => a.Id == schedule.LineId);
                    var machine = context.Machines.FirstOrDefault(a => a.Id == schedule.MachineId);
                    if (line == null || machine == null)
                        continue;
                    if (!context.PreventiveReviewHistories.Any(a => a.PreventiveId == schedule.Id && a.ScheduledReviewDate == schedule.NextReviewDate))
                        context.PreventiveReviewHistories.Add(new Data.Models.PreventiveReviewHistory { PreventiveId = schedule.Id, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = schedule.NextReviewDate, IsLaps = false, IsOverdue = false, IsLineActive = line.IsActive, IsMachineActive = machine.IsActive });
                }
                context.SaveChanges();

                // Overdues
                var overdueSchedules = context.PreventiveMaintenances.Where(w => !w.IsDeleted && w.ScheduleType != 5 && w.ScheduleType != 6 && w.NextReviewDate < today).ToArray();
                for (int i = 0; i < overdueSchedules.Length; i++)
                {
                    var schedule = overdueSchedules[i];
                    var newNextReviewDate = DateTime.Now.Date;
                    var existingReview = context.PreventiveReviewHistories.FirstOrDefault(a => a.PreventiveId == schedule.Id && a.ScheduledReviewDate == schedule.NextReviewDate);
                    var isLaps = false;
                    var isOverDue = false;

                    if (schedule.ScheduleType == 1) // Daily
                    {
                        newNextReviewDate = schedule.NextReviewDate.AddDays(1 * (schedule.Interval));
                        if ((today - schedule.NextReviewDate).Days > 0)
                            isLaps = true;
                    }
                    else if (schedule.ScheduleType == 2) // Weekly
                    {
                        newNextReviewDate = schedule.NextReviewDate.AddDays(7 * (schedule.Interval));
                        if ((today - schedule.NextReviewDate).Days >= 7)
                            isOverDue = true;
                        if ((today - schedule.NextReviewDate).Days > 9)
                            isLaps = true;
                    }
                    else if (schedule.ScheduleType == 3 && schedule.Interval < 6) // Monthly
                    {
                        newNextReviewDate = schedule.NextReviewDate.AddMonths(1 * (schedule.Interval));
                        if ((today - schedule.NextReviewDate).Days >= ((newNextReviewDate - schedule.NextReviewDate).Days))
                            isOverDue = true;
                        if ((today - schedule.NextReviewDate).Days > ((newNextReviewDate - schedule.NextReviewDate).Days + 7))
                            isLaps = true;
                    }
                    else if (schedule.ScheduleType == 3 && schedule.Interval >= 6) // Monthly
                    {
                        newNextReviewDate = schedule.NextReviewDate.AddMonths(1 * (schedule.Interval));
                        if ((today - schedule.NextReviewDate).Days >= ((newNextReviewDate - schedule.NextReviewDate).Days))
                            isOverDue = true;
                        if ((today - schedule.NextReviewDate).Days > ((newNextReviewDate - schedule.NextReviewDate).Days + 10))
                            isLaps = true;
                    }
                    else if (schedule.ScheduleType == 4) // Yearly
                    {
                        newNextReviewDate = schedule.NextReviewDate.AddYears(1 * (schedule.Interval));
                        if ((today - schedule.NextReviewDate).Days >= ((newNextReviewDate - schedule.NextReviewDate).Days))
                            isOverDue = true;
                        if ((today - schedule.NextReviewDate).Days > ((newNextReviewDate - schedule.NextReviewDate).Days + 10))
                            isLaps = true;
                    }

                    //if (existingReview == null)
                    //{
                    //    context.PreventiveReviewHistories.Add(new Data.Models.PreventiveReviewHistory { PreventiveId = schedule.Id, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = schedule.NextReviewDate, IsLaps = isLaps, IsOverdue = isOverDue });
                    //}
                    //else
                    if (existingReview != null)
                    {
                        if (existingReview.ReviewBy == null)
                        {
                            existingReview.IsLaps = isLaps;
                            existingReview.IsOverdue = isOverDue;
                        }
                    }

                    if (isLaps)
                    {
                        //existingReview.IsLaps = true;
                        schedule.NextReviewDate = newNextReviewDate;

                        var line = context.Lines.FirstOrDefault(a => a.Id == schedule.LineId);
                        var machine = context.Machines.FirstOrDefault(a => a.Id == schedule.MachineId);
                        if (line != null || machine != null)
                            context.PreventiveReviewHistories.Add(new Data.Models.PreventiveReviewHistory { PreventiveId = schedule.Id, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = schedule.NextReviewDate, IsLaps = false, IsOverdue = false, IsLineActive = line.IsActive, IsMachineActive = machine.IsActive });
                    }

                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context = null;
            }
        }

        private void ProcessHourlySchedules()
        {
            try
            {
                using (ProERPContext context = new ProERPContext())
                {
                    var dateTime = DateTime.Now;
                    //var today = new DateTime(dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond),dateTime.Kind);
                    var today = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
                    var schedules = context.PreventiveMaintenances.Where(w => !w.IsDeleted && w.ScheduleType == 6 && w.NextReviewDate <= today).ToArray();

                    for (int i = 0; i < schedules.Length; i++)
                    {
                        var schedule = schedules[i];
                        var line = context.Lines.FirstOrDefault(a => a.Id == schedule.LineId);
                        var machine = context.Machines.FirstOrDefault(a => a.Id == schedule.MachineId);
                        if (line == null || machine == null)
                            continue;
                        if (!context.PreventiveReviewHistories.Any(a => a.PreventiveId == schedule.Id && a.ScheduledReviewDate == schedule.NextReviewDate))
                            context.PreventiveReviewHistories.Add(new Data.Models.PreventiveReviewHistory { PreventiveId = schedule.Id, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = schedule.NextReviewDate, IsLaps = false, IsOverdue = false, IsLineActive = line.IsActive, IsMachineActive = machine.IsActive });
                    }
                    context.SaveChanges();

                    // Overdues
                    var overdueSchedules = context.PreventiveMaintenances.Where(w => !w.IsDeleted && w.ScheduleType == 6 && w.NextReviewDate < today).ToArray();
                    for (int i = 0; i < overdueSchedules.Length; i++)
                    {
                        var schedule = overdueSchedules[i];
                        var newNextReviewDate = DateTime.Now.Date;
                        var existingReview = context.PreventiveReviewHistories.FirstOrDefault(a => a.PreventiveId == schedule.Id && a.ScheduledReviewDate == schedule.NextReviewDate);
                        var isLaps = false;
                        var isOverDue = false;

                        // Hourly
                        newNextReviewDate = schedule.NextReviewDate.AddHours(1 * (schedule.Interval));
                        if ((today - schedule.NextReviewDate).Hours >= ((newNextReviewDate - schedule.NextReviewDate).TotalHours))
                            isOverDue = true;
                        if ((today - schedule.NextReviewDate).Hours > ((newNextReviewDate - schedule.NextReviewDate).TotalHours + 4))
                            isLaps = true;

                        if (existingReview != null)
                        {
                            if (existingReview.ReviewBy == null)
                            {
                                existingReview.IsLaps = isLaps;
                                existingReview.IsOverdue = isOverDue;
                            }
                        }

                        if (isLaps)
                        {
                            schedule.NextReviewDate = newNextReviewDate;

                            var line = context.Lines.FirstOrDefault(a => a.Id == schedule.LineId);
                            var machine = context.Machines.FirstOrDefault(a => a.Id == schedule.MachineId);
                            if (line != null || machine != null)
                                context.PreventiveReviewHistories.Add(new Data.Models.PreventiveReviewHistory { PreventiveId = schedule.Id, Notes = null, ReviewBy = null, ReviewDate = null, ScheduledReviewDate = schedule.NextReviewDate, IsLaps = false, IsOverdue = false, IsLineActive = line.IsActive, IsMachineActive = machine.IsActive });
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
