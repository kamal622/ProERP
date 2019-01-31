using ProERP.Data;
using ProERP.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services.Breakdown
{
    public class BreakDownReportService
    {

        public List<BreakDownReportDataSet> GetRDLCData(int siteId, int plantId, int lineId, DateTime fromDate, DateTime toDate)
        {
            ProERPContext db = new ProERPContext();
            var allData = db.BreakDowns.Where(w => w.PlantId == plantId && w.LineId == lineId && w.Date >= fromDate && w.Date <= toDate && w.IsDeleted == false).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power", BD = s }).ToArray();

            var data = from a in allData
                       select new BreakDownReportDataSet
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

        public List<HistoryCardDataSet> GetHistoryReportGridData(int siteId, int plantId, int lineId, int? machineId, DateTime fromDate, DateTime toDate, bool? isHistory)
        {
            ProERPContext db = new ProERPContext();
            var allData = db.BreakDowns.Where(w => w.PlantId == plantId && w.LineId == lineId && w.Date >= fromDate && w.Date <= toDate && w.IsDeleted == false && w.IsHistory == ((isHistory == null) ? w.IsHistory : isHistory)).Select(s => new
            {
                MachineName = s.Machine1.Name,
                SubAssemblyName = s.Machine == null ? "" : s.Machine.Name,
                SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power",
                BD = s
            }).ToArray();
            if (machineId != null && machineId > 0)
                allData = allData.Where(w => w.BD.MachineId == machineId).ToArray();

            int[] breakDownIds = allData.Select(s => s.BD.Id).Distinct().ToArray();
            var allMenPower = db.BreakDownMenPowers.Where(w => breakDownIds.Contains(w.BreakDownId)).ToArray();
            var allServices = db.BreakDownServices.Where(w => breakDownIds.Contains(w.BreakDownId)).Select(s => new { s.VendorName, s.BreakDownId }).ToArray();
            var allParts = db.BreakDownSpares.Where(w => breakDownIds.Contains(w.BreakDownId)).Select(s => new { s.Part.Name,s.Quantity, s.BreakDownId }).ToArray();
            var allAttachments = db.BreakDownAttachments.Where(w => breakDownIds.Contains(w.BreakDownId)).Select(s => new { s.OriginalFileName, s.BreakDownId }).ToArray();

            var gridData = from a in allData
                           orderby a.BD.Date
                           select new HistoryCardDataSet
                           {
                               Id = a.BD.Id,
                               PlantId = a.BD.PlantId,
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
                               PreventingAction = a.BD.PreventingAction,
                               MenPowerUsed = string.Join(", ", allMenPower.Where(w => w.BreakDownId == a.BD.Id).Select(s => s.Name)),
                               ServiceUsed = string.Join(", ", allServices.Where(w => w.BreakDownId == a.BD.Id).Select(s => s.VendorName)),
                               PartsUsed = string.Join(", ", allParts.Where(w => w.BreakDownId == a.BD.Id).Select(s =>  s.Name  + " (" + s.Quantity + ") ")),
                               AttachmentFile =string.Join(", ",allAttachments.Where(w=>w.BreakDownId == a.BD.Id).Select(s=>s.OriginalFileName ))
                           };
            return gridData.ToList();
        }

        //public List<BreakDownMonthlySummaryDataSet> GetBreakdownDataGroupByType(int? siteId, int? plantId, int? lineId, DateTime fromDate, DateTime toDate)
        public List<BreakDownMonthlySummaryDataSet> GetBreakdownDataGroupByType(int siteId, int? plantId, int? lineId, string year,int month)
        {
            ProERPContext db = new ProERPContext();
            // var allData = db.BreakDowns.Where(w => w.Date >= fromDate && w.Date <= toDate).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power", BD = s }).ToList();
            var allData = db.BreakDowns.Where(w => w.Date.Year.ToString() == year && w.Date.Month ==(month == 0 ? w.Date.Month : month) && w.IsDeleted == false).Select(s => new { MachineName = s.Machine1.Name, SubAssemblyName = s.Machine == null ? "" : s.Machine.Name, SpareTypeName = s.SpareTypeId == null ? "" : s.SpareTypeId == 1 ? "Service" : "Man Power", BD = s }).ToList();

            if (lineId != null && lineId > 0)
                allData = allData.Where(w => w.BD.LineId == lineId).ToList();
            else if (plantId != null && plantId > 0)
                allData = allData.Where(w => w.BD.PlantId == plantId).ToList();


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

        public List<string> getLineNamesByPlantId(int plantId)
        {
            ProERPContext db = new ProERPContext();
            return db.Lines.Where(w => w.PlantId == plantId).Select(s => s.Name).ToList();
        }

        public string getPlantNameBylantId(int plantId)
        {
            ProERPContext db = new ProERPContext();
            return db.Plants.Where(w => w.Id == plantId).Select(s => s.Name).FirstOrDefault();

        }

    }
}
