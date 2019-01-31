using ProERP.Core;
using ProERP.Core.Data;
using ProERP.Core.Models;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Breakdown
{
    public class BreakdownService
    {
        private readonly IRepository<Data.Models.BreakDown> _breakDownRepository;
        private readonly IRepository<Data.Models.BreakDownMenPower> _breakDownMenPowerRepository;
        private readonly IRepository<Data.Models.BreakDownService> _breakDownServiceRepository;
        private readonly IRepository<Data.Models.BreakDownSpare> _breakDownSpareRepository;
        private readonly IRepository<Data.Models.Line> _lineRepository;
        private readonly IRepository<Data.Models.BreakDownAttachment> _breakdownAttachmentRepository;
        private readonly IRepository<Data.Models.BreakDownUploadHistory> _breakDownUploadHistory;
        private readonly IRepository<Data.Models.QOSLine> _qOSLineRepository;


        public BreakdownService(IRepository<Data.Models.BreakDown> breakDownRepository, IRepository<Data.Models.BreakDownMenPower> breakDownMenPowerRepository
            , IRepository<Data.Models.BreakDownService> breakDownServiceRepository, IRepository<Data.Models.BreakDownSpare> breakDownSpareRepository,
            IRepository<Data.Models.Line> lineRepository, IRepository<Data.Models.BreakDownAttachment> breakdownAttachmentRepository,
            IRepository<Data.Models.BreakDownUploadHistory> breakDownUploadHistory,
            IRepository<Data.Models.QOSLine> qOSLineRepository)
        {
            this._breakDownRepository = breakDownRepository;
            this._breakDownMenPowerRepository = breakDownMenPowerRepository;
            this._breakDownServiceRepository = breakDownServiceRepository;
            this._breakDownSpareRepository = breakDownSpareRepository;
            this._lineRepository = lineRepository;
            this._breakdownAttachmentRepository = breakdownAttachmentRepository;
            this._breakDownUploadHistory = breakDownUploadHistory;
            this._qOSLineRepository = qOSLineRepository;
        }

        public Data.Models.BreakDown[] GetAll(int siteId, int plantId, int lineId)
        {
            return this._breakDownRepository.Table.Where(w => w.PlantId == plantId && w.LineId == lineId).ToArray();
        }

        public List<Core.Models.BreakDownGridModel> GetGridData(int siteId, int plantId, int lineId, DateTime fromDate, DateTime toDate, string sortdatafield, string sortorder)
        {
            var allData = this._breakDownRepository.Table.Where(w => w.PlantId == plantId && w.LineId == lineId && w.Date >= fromDate && w.Date <= toDate && w.IsDeleted == false).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power", BD = s });
            int[] breakDownIds = allData.Select(s => s.BD.Id).Distinct().ToArray();
            var allParts = this._breakDownSpareRepository.Table.Where(w => breakDownIds.Contains(w.BreakDownId)).Select(s => new { s.Part.Name, s.BreakDownId }).ToArray();

            if (!string.IsNullOrEmpty(sortdatafield) && !string.IsNullOrEmpty(sortorder))
            {
                if (sortorder == "ascending")
                {
                    switch (sortdatafield)
                    {
                        case "StartTime":
                            allData = allData.OrderBy(o => o.BD.Date).ThenBy(o => o.BD.StartTime);
                            break;
                        default:
                            allData = allData.OrderBy(o => o.BD.Date);
                            break;
                    }
                }
                else
                {
                    switch (sortdatafield)
                    {
                        case "StartTime":
                            allData = allData.OrderBy(o => o.BD.Date).ThenByDescending(o => o.BD.StartTime);
                            break;
                        default:
                            allData = allData.OrderByDescending(o => o.BD.Date);
                            break;
                    }
                }
            }
            else
               allData=allData.OrderBy(o => o.BD.Date).ThenBy(o=>o.BD.StartTime);


            var gridData = from a in allData.ToArray()
                           select new Core.Models.BreakDownGridModel
                           {
                               Id = a.BD.Id,
                               IsHistory = a.BD.IsHistory,
                               IsRepeated = a.BD.IsRepeated,
                               IsMajor = a.BD.IsMajor,
                               PlantId = a.BD.PlantId,
                               LineId = a.BD.LineId,
                               MachineId = a.BD.MachineId,
                               MachineName = a.MachineName,
                               SubAssemblyId = a.BD.SubAssemblyId ?? 0,
                               SubAssemblyName = a.SubAssemblyName,
                               Date = a.BD.Date,
                               StartTime = DateTime.Now.Date + a.BD.StartTime,
                               StopTime = DateTime.Now.Date + a.BD.EndTime,
                               TotalTime = a.BD.TotalTime,
                               FailureDescription = a.BD.FailureDescription,
                               ElecticalTime = a.BD.ElectricalTime,
                               MechTime = a.BD.MechTime,
                               InstrTime = a.BD.InstrTime,
                               UtilityTime = a.BD.UtilityTime,
                               PowerTime = a.BD.PowerTime,
                               ProcessTime = a.BD.ProcessTime,
                               PrvTime = a.BD.PrvTime,
                               IdleTime = a.BD.IdleTime,
                               ResolveTimeTaken = DateTime.Now.Date + (a.BD.ResolveTimeTaken ?? new TimeSpan(0)),
                               SpareTypeId = a.BD.SpareTypeId, // ?? 0,
                               SpareTypeName = a.SpareTypeName,
                               SpareDescription = a.BD.SpareDescription,
                               DoneBy = a.BD.DoneBy,
                               RootCause = a.BD.RootCause,
                               Correction = a.BD.Correction,
                               CorrectiveAction = a.BD.CorrectiveAction,
                               PreventingAction = a.BD.PreventingAction,
                               PartUsed = string.Join(", ", allParts.Where(w => w.BreakDownId == a.BD.Id).Select(s => s.Name))
                           };
            return gridData.ToList();
        }

        public List<Core.Models.BreakDownGridModel> GetAllBreakdownData(int PlantId, int Year, int Month)
        {
            var allData = this._breakDownRepository.Table.Where(w => w.PlantId == (PlantId == 0 ? w.PlantId : PlantId) && w.Date.Year == (Year == 0 ? w.Date.Year : Year) && w.Date.Month == (Month == 0 ? w.Date.Month : Month) && w.IsDeleted == false).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power" , BD = s });
            int[] breakDownIds = allData.Select(s => s.BD.Id).Distinct().ToArray();
            var allParts = this._breakDownSpareRepository.Table.Where(w => breakDownIds.Contains(w.BreakDownId)).Select(s => new { s.Part.Name, s.BreakDownId, s.Quantity }).ToArray();

            var gridData = from a in allData.ToArray() 
                           where a.BD.PlantId == (PlantId == 0 ? a.BD.PlantId : PlantId)
                           select new Core.Models.BreakDownGridModel
                           {
                               Id = a.BD.Id,
                               IsHistory = a.BD.IsHistory,
                               PlantId = a.BD.PlantId,
                               PlantName = a.BD.Plant.Name,
                               LineId = a.BD.LineId,
                               LineName = a.BD.Line.Name,
                               MachineId = a.BD.MachineId,
                               MachineName = a.MachineName,
                               SubAssemblyId = a.BD.SubAssemblyId ?? 0,
                               SubAssemblyName = a.SubAssemblyName,
                               Date = a.BD.Date,
                               StartTime = DateTime.Now.Date + a.BD.StartTime,
                               StopTime = DateTime.Now.Date + a.BD.EndTime,
                               TotalTime = a.BD.TotalTime,
                               FailureDescription = a.BD.FailureDescription,
                               ElecticalTime = a.BD.ElectricalTime,
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
                               PreventingAction = a.BD.PreventingAction,
                               PartUsed = string.Join(", ", allParts.Where(w => w.BreakDownId == a.BD.Id).Select(s => s.Name + " (" + s.Quantity + ") "))
                           };
            return gridData.ToList();
        }

        public List<Core.Models.HistoryReportGridModel> GetHistoryReportGridData(int siteId, int plantId, int lineId, DateTime fromDate, DateTime toDate)
        {
            var allData = this._breakDownRepository.Table.Where(w => w.PlantId == plantId && w.LineId == lineId && w.Date >= fromDate && w.Date <= toDate && w.IsDeleted == false).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power", BD = s }).ToArray();

            int[] breakDownIds = allData.Select(s => s.BD.Id).Distinct().ToArray();
            var allMenPower = this._breakDownMenPowerRepository.Table.Where(w => breakDownIds.Contains(w.BreakDownId)).ToArray();
            var allServices = this._breakDownServiceRepository.Table.Where(w => breakDownIds.Contains(w.BreakDownId)).Select(s => new { s.VendorName, s.BreakDownId }).ToArray();
            var allParts = this._breakDownSpareRepository.Table.Where(w => breakDownIds.Contains(w.BreakDownId)).Select(s => new { s.Part.Name, s.BreakDownId }).ToArray();


            var gridData = from a in allData
                           select new Core.Models.HistoryReportGridModel
                           {
                               Id = a.BD.Id,
                               PlantId = a.BD.PlantId,
                               LineId = a.BD.LineId,
                               MachineId = a.BD.MachineId,
                               MachineName = a.MachineName,
                               SubAssemblyId = a.BD.SubAssemblyId ?? 0,
                               SubAssemblyName = a.SubAssemblyName,
                               Date = a.BD.Date,
                               StartTime = DateTime.Now.Date + a.BD.StartTime,
                               StopTime = DateTime.Now.Date + a.BD.EndTime,
                               TotalTime = a.BD.TotalTime,
                               FailureDescription = a.BD.FailureDescription,
                               ElecticalTime = a.BD.ElectricalTime,
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
                               PreventingAction = a.BD.PreventingAction,
                               MenPowerUsed = string.Join(", ", allMenPower.Where(w => w.BreakDownId == a.BD.Id).Select(s => s.Name)),
                               ServiceUsed = string.Join(", ", allServices.Where(w => w.BreakDownId == a.BD.Id).Select(s => s.VendorName)),
                               PartsUsed = string.Join(", ", allParts.Where(w => w.BreakDownId == a.BD.Id).Select(s => s.Name))
                           };

            return gridData.ToList();
        }

        public void SaveBreakdownData(BreakDownGridModel[] breakDownData, BreakDownServiceGridModel[] ServiceData, BreakDownManPowerGridModel[] MenPowerData
            , BreakDownPartGridModel[] PartData, BreakDownAttachmentGridModel[] AttachmentData,
            int[] DeletedServiceIds, int[] DeletedMenPowerIds, int[] DeletedPartIds, int[] DeletedAttIds, int userId)
        {
            if (DeletedServiceIds != null && DeletedServiceIds.Length > 0)
            {
                // Delete
                //int[] serviceItemIds = DeletedServiceIds; //ServiceData.Select(s => s.Id).ToArray();
                var deleteServiceItems = this._breakDownServiceRepository.Table.Where(w => DeletedServiceIds.Contains(w.Id));
                this._breakDownServiceRepository.Delete(deleteServiceItems);
            }

            if (DeletedMenPowerIds != null && DeletedMenPowerIds.Length > 0)
            {
                // Delete
                //int[] manpowerItemIds = DeletedMenPowerIds; //ManPowerData.Select(s => s.Id).ToArray();
                var deleteMenPowerItems = this._breakDownMenPowerRepository.Table.Where(w => DeletedMenPowerIds.Contains(w.Id));
                this._breakDownMenPowerRepository.Delete(deleteMenPowerItems);
            }

            if (DeletedPartIds != null && DeletedPartIds.Length > 0)
            {
                //Delete
                //int[] partItemIds = DeletedPartIds; //PartData.Select(s => s.Id).ToArray();
                var deletePartItems = this._breakDownSpareRepository.Table.Where(w => DeletedPartIds.Contains(w.Id));
                this._breakDownSpareRepository.Delete(deletePartItems);
            }
            if (DeletedAttIds != null && DeletedAttIds.Length > 0)
            {
                //Delete
                //int[] partItemIds = DeletedPartIds; //PartData.Select(s => s.Id).ToArray();
                var deleteAttachmentItems = this._breakdownAttachmentRepository.Table.Where(w => DeletedAttIds.Contains(w.Id));
                this._breakdownAttachmentRepository.Delete(deleteAttachmentItems);
            }

            for (int i = 0; i < breakDownData.Length; i++)
            {
                BreakDownGridModel item = breakDownData[i];

                Data.Models.BreakDown existingBreakDown = null;
                //int breakDownId = item.Id;

                if (item.Id > 0)
                    existingBreakDown = this._breakDownRepository.Table.FirstOrDefault(f => f.Id == item.Id);

                if (existingBreakDown == null)
                {
                    // Insert
                    Data.Models.BreakDown NewBreakDown = new Data.Models.BreakDown
                    {
                        PlantId = item.PlantId,
                        LineId = item.LineId,
                        MachineId = item.MachineId,
                        SubAssemblyId = item.SubAssemblyId,
                        Date = item.Date.Value,
                        IsHistory = item.IsHistory,
                        IsRepeated = item.IsRepeated,
                        IsMajor = item.IsMajor,
                        StartTime = item.StartTime.Value.TimeOfDay,
                        EndTime = item.StopTime.Value.TimeOfDay,
                        TotalTime = item.TotalTime.Value,
                        FailureDescription = string.IsNullOrEmpty(item.FailureDescription) ? item.FailureDescription : item.FailureDescription.Replace("&nbsp;", " "),
                        ElectricalTime = item.ElecticalTime,
                        MechTime = item.MechTime,
                        InstrTime = item.InstrTime,
                        UtilityTime = item.UtilityTime,
                        PowerTime = item.PowerTime,
                        ProcessTime = item.ProcessTime,
                        PrvTime = item.PrvTime,
                        IdleTime = item.IdleTime,
                        SpareTypeId = item.SpareTypeId,
                        SpareDescription = item.SpareDescription,
                        DoneBy = item.DoneBy,
                        RootCause = string.IsNullOrEmpty(item.RootCause) ? item.RootCause : item.RootCause.Replace("&nbsp;", " "),
                        Correction = string.IsNullOrEmpty(item.Correction) ? item.Correction : item.Correction.Replace("&nbsp;", " "),
                        CorrectiveAction = string.IsNullOrEmpty(item.CorrectiveAction) ? item.CorrectiveAction : item.CorrectiveAction.Replace("&nbsp;", " "),
                        PreventingAction = string.IsNullOrEmpty(item.PreventingAction) ? item.PreventingAction : item.PreventingAction.Replace("&nbsp;", " "),
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow
                    };

                    //if (item.ElecticalTime != null)
                    //    NewBreakDown.ElectricalTime = item.ElecticalTime.Value.TimeOfDay;
                    //if (item.MechTime != null)
                    //    NewBreakDown.MechTime = item.MechTime.Value.TimeOfDay;
                    //if (item.InstrTime != null)
                    //    NewBreakDown.InstrTime = item.InstrTime.Value.TimeOfDay;
                    //if (item.UtilityTime != null)
                    //    NewBreakDown.UtilityTime = item.UtilityTime.Value.TimeOfDay;
                    //if (item.PowerTime != null)
                    //    NewBreakDown.PowerTime = item.PowerTime.Value.TimeOfDay;
                    //if (item.ProcessTime != null)
                    //    NewBreakDown.ProcessTime = item.ProcessTime.Value.TimeOfDay;
                    //if (item.PrvTime != null)
                    //    NewBreakDown.PrvTime = item.PrvTime.Value.TimeOfDay;
                    //if (item.IdleTime != null)
                    //    NewBreakDown.IdleTime = item.IdleTime.Value.TimeOfDay;
                    if (item.ResolveTimeTaken != null)
                        NewBreakDown.ResolveTimeTaken = item.ResolveTimeTaken.Value.TimeOfDay;

                    this._breakDownRepository.Insert(NewBreakDown);
                    existingBreakDown = NewBreakDown;
                }
                else
                {
                    // Update
                    //existingBreakDown.PlantId = item.PlantId;
                    //existingBreakDown.LineId = item.LineId;

                    if (existingBreakDown.MachineId != item.MachineId || existingBreakDown.SubAssemblyId != item.SubAssemblyId || existingBreakDown.IsHistory != item.IsHistory
                       || existingBreakDown.IsRepeated != item.IsRepeated || existingBreakDown.IsMajor != item.IsMajor
                       || existingBreakDown.Date != item.Date.Value || existingBreakDown.StartTime != item.StartTime.Value.TimeOfDay || existingBreakDown.EndTime != item.StopTime.Value.TimeOfDay
                       || existingBreakDown.TotalTime != item.TotalTime.Value || existingBreakDown.FailureDescription != item.FailureDescription || existingBreakDown.ElectricalTime != item.ElecticalTime
                       || existingBreakDown.MechTime != item.MechTime || existingBreakDown.InstrTime != item.InstrTime || existingBreakDown.UtilityTime != item.UtilityTime
                       || existingBreakDown.PowerTime != item.PowerTime || existingBreakDown.ProcessTime != item.ProcessTime || existingBreakDown.PrvTime != item.PrvTime
                       || existingBreakDown.IdleTime != item.IdleTime || existingBreakDown.SpareTypeId != item.SpareTypeId || existingBreakDown.SpareDescription != item.SpareDescription
                       || existingBreakDown.DoneBy != item.DoneBy || existingBreakDown.RootCause != item.RootCause || existingBreakDown.Correction != item.Correction
                       || existingBreakDown.PreventingAction != item.PreventingAction || existingBreakDown.CorrectiveAction != item.CorrectiveAction)
                    {
                        existingBreakDown.MachineId = item.MachineId;
                        existingBreakDown.SubAssemblyId = item.SubAssemblyId;
                        existingBreakDown.IsHistory = item.IsHistory;
                        existingBreakDown.IsRepeated = item.IsRepeated;
                        existingBreakDown.IsMajor = item.IsMajor;
                        existingBreakDown.Date = item.Date.Value;
                        existingBreakDown.StartTime = item.StartTime.Value.TimeOfDay;
                        existingBreakDown.EndTime = item.StopTime.Value.TimeOfDay;
                        existingBreakDown.TotalTime = item.TotalTime.Value;
                        existingBreakDown.FailureDescription = string.IsNullOrEmpty(item.FailureDescription) ? item.FailureDescription : item.FailureDescription.Replace("&nbsp;", " ");
                        existingBreakDown.ElectricalTime = item.ElecticalTime;
                        existingBreakDown.MechTime = item.MechTime;
                        existingBreakDown.InstrTime = item.InstrTime;
                        existingBreakDown.UtilityTime = item.UtilityTime;
                        existingBreakDown.PowerTime = item.PowerTime;
                        existingBreakDown.ProcessTime = item.ProcessTime;
                        existingBreakDown.PrvTime = item.PrvTime;
                        existingBreakDown.IdleTime = item.IdleTime;
                        existingBreakDown.SpareTypeId = item.SpareTypeId;
                        existingBreakDown.SpareDescription = item.SpareDescription;
                        existingBreakDown.DoneBy = item.DoneBy;
                        existingBreakDown.RootCause = string.IsNullOrEmpty(item.RootCause) ? item.RootCause : item.RootCause.Replace("&nbsp;", " ");
                        existingBreakDown.Correction = string.IsNullOrEmpty(item.Correction) ? item.Correction : item.Correction.Replace("&nbsp;", " ");
                        existingBreakDown.CorrectiveAction = string.IsNullOrEmpty(item.CorrectiveAction) ? item.CorrectiveAction : item.CorrectiveAction.Replace("&nbsp;", " ");
                        existingBreakDown.PreventingAction = string.IsNullOrEmpty(item.PreventingAction) ? item.PreventingAction : item.PreventingAction.Replace("&nbsp;", " ");
                        existingBreakDown.UpdatedBy = userId;
                        existingBreakDown.UpdatedDate = DateTime.UtcNow;
                        if (item.ResolveTimeTaken != null)
                            existingBreakDown.ResolveTimeTaken = item.ResolveTimeTaken.Value.TimeOfDay;

                        this._breakDownRepository.Update(existingBreakDown);
                    }



                    //if (item.ElecticalTime != null)
                    //    existingBreakDown.ElectricalTime = item.ElecticalTime.Value.TimeOfDay;
                    //if (item.MechTime != null)
                    //    existingBreakDown.MechTime = item.MechTime.Value.TimeOfDay;
                    //if (item.InstrTime != null)
                    //    existingBreakDown.InstrTime = item.InstrTime.Value.TimeOfDay;
                    //if (item.UtilityTime != null)
                    //    existingBreakDown.UtilityTime = item.UtilityTime.Value.TimeOfDay;
                    //if (item.PowerTime != null)
                    //    existingBreakDown.PowerTime = item.PowerTime.Value.TimeOfDay;
                    //if (item.ProcessTime != null)
                    //    existingBreakDown.ProcessTime = item.ProcessTime.Value.TimeOfDay;
                    //if (item.PrvTime != null)
                    //    existingBreakDown.PrvTime = item.PrvTime.Value.TimeOfDay;
                    //if (item.IdleTime != null)
                    //    existingBreakDown.IdleTime = item.IdleTime.Value.TimeOfDay;

                }

                if (ServiceData != null)
                {
                    var ServiceDataForBreakDown = ServiceData.Where(w => w.BreakDownId == item.Id).ToArray();
                    for (int j = 0; j < ServiceDataForBreakDown.Length; j++)
                    {
                        BreakDownServiceGridModel serviceItem = ServiceDataForBreakDown[j];
                        ProERP.Data.Models.BreakDownService existingServiceItem = this._breakDownServiceRepository.Table.FirstOrDefault(w => w.Id == serviceItem.Id);

                        if (existingServiceItem == null)
                        {
                            // Insert
                            Data.Models.BreakDownService NewService = new Data.Models.BreakDownService
                            {
                                BreakDownId = existingBreakDown.Id,
                                VendorName = serviceItem.VendorName,
                                Cost = serviceItem.Cost,
                                Comments = serviceItem.Comments
                            };

                            this._breakDownServiceRepository.Insert(NewService);
                        }
                        else
                        {
                            // Update
                        }
                    }
                }

                if (MenPowerData != null)
                {
                    var MenPowerDataForBreakDown = MenPowerData.Where(w => w.BreakDownId == item.Id).ToArray();
                    for (int k = 0; k < MenPowerDataForBreakDown.Length; k++)
                    {
                        BreakDownManPowerGridModel menpowerItem = MenPowerDataForBreakDown[k];
                        ProERP.Data.Models.BreakDownMenPower existingMenPowerItem = this._breakDownMenPowerRepository.Table.FirstOrDefault(w => w.Id == menpowerItem.Id);
                        if (existingMenPowerItem == null)
                        {
                            if (!menpowerItem.IsOverTime)
                                menpowerItem.HourlyRate = 0;
                            // Insert
                            Data.Models.BreakDownMenPower NewMenPower = new Data.Models.BreakDownMenPower
                            {
                                BreakDownId = existingBreakDown.Id,
                                Name = menpowerItem.Name,
                                EmployeeTypeId = menpowerItem.EmployeeTypeId,
                                Comments = menpowerItem.Comments,
                                HourlyRate = menpowerItem.HourlyRate,
                            };

                            this._breakDownMenPowerRepository.Insert(NewMenPower);
                        }
                        else
                        {
                            // Update
                        }
                    }


                }

                if (PartData != null)
                {


                    var PartDataForBreakDown = PartData.Where(w => w.BreakDownId == item.Id).ToArray();
                    for (int l = 0; l < PartDataForBreakDown.Length; l++)
                    {
                        BreakDownPartGridModel partItem = PartDataForBreakDown[l];
                        ProERP.Data.Models.BreakDownSpare existingPartItem = this._breakDownSpareRepository.Table.FirstOrDefault(w => w.Id == partItem.Id);
                        if (existingPartItem == null)
                        {
                            // Insert
                            Data.Models.BreakDownSpare NewPart = new Data.Models.BreakDownSpare
                            {
                                BreakDownId = existingBreakDown.Id,
                                PartId = partItem.PartId,
                                Quantity = partItem.Quantity,
                                Comments = partItem.Comments

                            };

                            this._breakDownSpareRepository.Insert(NewPart);
                        }
                        else
                        {
                            // Update
                        }
                    }

                }
                if (AttachmentData != null)
                {
                    var AttachmentDataForBreakDown = AttachmentData.Where(w => w.BreakDownId == item.Id).ToArray();
                    for (int m = 0; m < AttachmentDataForBreakDown.Length; m++)
                    {
                        BreakDownAttachmentGridModel AttachmentItem = AttachmentDataForBreakDown[m];
                        ProERP.Data.Models.BreakDownAttachment existingAttachmentItem = this._breakdownAttachmentRepository.Table.FirstOrDefault(w => w.Id == AttachmentItem.Id);

                        if (existingAttachmentItem == null)
                        {
                            // Insert
                            Data.Models.BreakDownAttachment NewService = new Data.Models.BreakDownAttachment
                            {
                                BreakDownId = existingBreakDown.Id,
                                OriginalFileName = AttachmentItem.OriginalFileName,
                                SysFileName = AttachmentItem.SysFileName
                            };

                            this._breakdownAttachmentRepository.Insert(NewService);
                        }
                        else
                        {
                            // Update
                        }
                    }
                }
            }
        }

        public int InsertExcelFile(Data.Models.BreakDownUploadHistory breakDownUploadHistory)
        {
            _breakDownUploadHistory.Insert(breakDownUploadHistory);
            return breakDownUploadHistory.Id;
        }

        public void SaveBreakdownExcelData(Data.Models.BreakDown breakdownlist, int userId, int uploadId)
        {
            breakdownlist.CreatedBy = userId;
            breakdownlist.UploadId = uploadId;
            this._breakDownRepository.Insert(breakdownlist);
        }

        public Data.Models.BreakDownService[] GetForId(int breakDownId)
        {
            return this._breakDownServiceRepository.Table.Where(f => f.BreakDownId == breakDownId).ToArray();
        }
        public IQueryable<Data.Models.BreakDownMenPower> GetForMenPowerId(int breakDownId)
        {
            return this._breakDownMenPowerRepository.Table.Where(f => f.BreakDownId == breakDownId);
        }
        public IQueryable<Data.Models.BreakDownSpare> GetForPartId(int breakDownId)
        {
            return this._breakDownSpareRepository.Table.Where(f => f.BreakDownId == breakDownId);
        }
        public IQueryable<Data.Models.BreakDownAttachment> GetForAttachmentId(int breakDownId)
        {
            return this._breakdownAttachmentRepository.Table.Where(f => f.BreakDownId == breakDownId);
        }
        public List<Data.Models.BreakDown> GetHistoryReportData()
        {
            return this._breakDownRepository.Table.ToList();
        }
        public void DeleteBreakdownData(int[] ids, DateTime deletedon, int userId)
        {
            //var deletedServiceItems = this._breakDownServiceRepository.Table.Where(w => Ids.Contains(w.BreakDownId));
            //this._breakDownServiceRepository.Delete(deletedServiceItems);

            //var deletedMenPowerItems = this._breakDownMenPowerRepository.Table.Where(w => Ids.Contains(w.BreakDownId));
            //this._breakDownMenPowerRepository.Delete(deletedMenPowerItems);

            //var deletedPartItems = this._breakDownSpareRepository.Table.Where(w => Ids.Contains(w.BreakDownId));
            //this._breakDownSpareRepository.Delete(deletedPartItems);

            //var deletedItems = this._breakDownRepository.Table.Where(w => Ids.Contains(w.Id));
            //this._breakDownRepository.Delete(deletedItems);
            foreach (int id in ids)
            {
                Data.Models.BreakDown existingBD = _breakDownRepository.Table.FirstOrDefault(w => w.Id == id);
                existingBD.IsDeleted = true;
                existingBD.DeletedDate = deletedon;
                existingBD.DeletedBy = userId;
                _breakDownRepository.Update(existingBD);
            }
        }
        public BreakdownsByPlantModel[] GetBreakdownCountsByPlant()
        {
            var allData = from a in this._breakDownRepository.Table
                          where a.IsDeleted == false
                          group a by new { a.PlantId, a.Plant.Name } into grp
                          select new BreakdownsByPlantModel
                          {
                              Id = grp.Key.PlantId,
                              PlantName = grp.Key.Name,
                              Count = grp.Count()
                          };

            return allData.ToArray();
        }
        public BreakdownsByLineModel[] GetBreakdownCountsByLine(int plantId)
        {
            if (plantId <= 0)
                return new BreakdownsByLineModel[] { };

            var allData = from a in this._lineRepository.Table
                          join b in this._breakDownRepository.Table on a.Id equals b.LineId into j1
                          from c in j1.DefaultIfEmpty()
                          where a.PlantId == plantId && c.IsDeleted == false
                          group c by new { a.Id, a.Name } into grp
                          select new BreakdownsByLineModel
                          {
                              Id = grp.Key.Id,
                              LineName = grp.Key.Name,
                              Count = grp.Where(w => w != null).Count()
                          };
            return allData.ToArray();
        }
        // DashBoard BreakDown Count
        public int GetBreakdownCounts(DateTime date)
        {
            var allData = from a in this._breakDownRepository.Table
                          where a.Date == date && a.IsDeleted == false
                          select a;
            return allData.Count();
        }
        public BreakdownsByMachineModel[] GetBreakdownCountsByMachine(int lineId)
        {

            var allData = from a in this._breakDownRepository.Table
                          where a.LineId == lineId && a.IsDeleted == false
                          group a by new { a.MachineId, a.Machine1.Name, a.TotalTime } into grp
                          select new BreakdownsByMachineModel
                          {
                              Id = grp.Key.MachineId,
                              MachineName = grp.Key.Name,
                              TotalTime = grp.Key.TotalTime,
                              Count = grp.Count()

                          };
            return allData.ToArray();
        }

        public double CalcQOS()
        {
            DateTime previousMonth = DateTime.Now.AddMonths(-1);
            int lastMonth = previousMonth.Month;
            int lastMonthDay = DateTime.DaysInMonth(previousMonth.Year, lastMonth);

            //calculate avialable time
            long idealTotaltime = 0;
            var lineid = this._qOSLineRepository.Table.Select(s => s.LineId).ToList();
            if (this._breakDownRepository.Table.Any(w => w.IsDeleted == false && w.Date.Month == lastMonth && w.Date.Year == previousMonth.Year && (w.PowerTime == true || w.ProcessTime == true || w.PrvTime == true || w.IdleTime == true)))
                idealTotaltime = this._breakDownRepository.Table.Where(w => lineid.Contains(w.LineId) && w.IsDeleted == false && w.Date.Month == lastMonth && w.Date.Year == previousMonth.Year && (w.PowerTime == true || w.ProcessTime == true || w.PrvTime == true || w.IdleTime == true)).Sum(s => (long)s.TotalTime);

            double idealTotalHours = TimeSpan.FromMilliseconds(idealTotaltime).TotalHours;
            double totalHoursInMonth = (24 * lastMonthDay * 8);
            double availableTime = (totalHoursInMonth - idealTotalHours);

            //calculate breakdown time
            long breakdownTotaltime = 0;
            if (this._breakDownRepository.Table.Any(w => w.IsDeleted == false && w.Date.Month == lastMonth && w.Date.Year == previousMonth.Year && (w.ElectricalTime == true || w.MechTime == true || w.InstrTime == true || w.UtilityTime == true)))
                breakdownTotaltime = this._breakDownRepository.Table.Where(w => lineid.Contains(w.LineId) && w.IsDeleted == false && w.Date.Month == lastMonth && w.Date.Year == previousMonth.Year && (w.ElectricalTime == true || w.MechTime == true || w.InstrTime == true || w.UtilityTime == true)).Sum(s => (long)s.TotalTime);
            double breakdownTime = TimeSpan.FromMilliseconds(breakdownTotaltime).TotalHours;

            //calculate breakdown percentage
            double breakdownPerc = (breakdownTime  * 100) / availableTime;

            var val = breakdownPerc.ToString("F2");
            double Percentage = double.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
            return Percentage;
        }
    }
}
