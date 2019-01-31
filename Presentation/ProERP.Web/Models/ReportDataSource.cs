using ProERP.Core.Models;
using ProERP.Services.Breakdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProERP.Web.Models
{
    public class ReportDataSource
    {
        private readonly BreakdownService _breakDownService;
        private List<BreakDownReportDataSet> newDataSet;
        //public ReportDataSource()
        //{
        //    newDataSource = new List<BreakDownReportDataSet>();
        //    BreakDownReportDataSet data = new BreakDownReportDataSet();
        //    //data.Id = 1000;
        //    //data.PlantId = 2;
        //    //data.LineId = 1;
        //    //data.MachineName = "Machine1";
        //    //newDataSource.Add(data);

        //    BreakdownService _breakDownService = new BreakdownService();
        //   // var dataSource = _breakDownService.GetRDLCData(1, 1, 1, DateTime.Now.AddDays(-30), DateTime.Now);

        //    List<BreakDownGridModel> dataSource = _breakDownService.GetRDLCData(1, 1, 12, DateTime.Now.AddDays(-30), DateTime.Now);
        //    foreach(var newData in dataSource)
        //    {
        //        data = new BreakDownReportDataSet();
        //        data.Id = newData.Id;
        //        data.PlantId = newData.PlantId;
        //        data.LineId = newData.LineId;
        //        data.MachineName = newData.MachineName;
        //        newDataSource.Add(data);
        //    }

        //}
        public ReportDataSource()
        {
            //BreakdownService _breakDownService = new BreakdownService();
            //var dataSource = _breakDownService.GetRDLCData(1, 1, 12, DateTime.Now.AddDays(-30), DateTime.Now);
            newDataSet = new List<BreakDownReportDataSet>();

            //foreach (var dataOld in dataSource)
            //{
            //    BreakDownReportDataSet datanew = new BreakDownReportDataSet();
            //    datanew.Id = dataOld.Id;
            //    datanew.PlantId = dataOld.PlantId;
            //    datanew.LineId = dataOld.LineId;
            //    datanew.MachineId = dataOld.MachineId;
            //    datanew.MachineName = dataOld.MachineName;
            //    datanew.SubAssemblyId = dataOld.SubAssemblyId;
            //    datanew.SubAssemblyName = dataOld.SubAssemblyName;
            //    datanew.Date = dataOld.Date;
            //    datanew.StartTime = dataOld.StartTime;
            //    datanew.StopTime = dataOld.StopTime;
            //    datanew.TotalTime = dataOld.TotalTime;
            //    datanew.FailureDescription = dataOld.FailureDescription;
            //    datanew.ElecticalTime = dataOld.ElecticalTime;
            //    datanew.MechTime = dataOld.MechTime;
            //    datanew.InstrTime = dataOld.InstrTime;
            //    datanew.UtilityTime = dataOld.UtilityTime;
            //    datanew.PowerTime = dataOld.PowerTime;
            //    datanew.ProcessTime = dataOld.ProcessTime;
            //    datanew.PrvTime = dataOld.PrvTime;
            //    datanew.IdleTime = dataOld.IdleTime;
            //    datanew.ResolveTimeTaken = dataOld.ResolveTimeTaken;
            //    datanew.SpareTypeId = dataOld.SpareTypeId;
            //    datanew.SpareTypeName = dataOld.SpareTypeName;
            //    datanew.SpareDescription = dataOld.SpareDescription;
            //    datanew.DoneBy = dataOld.DoneBy;
            //    datanew.RootCause = dataOld.RootCause;
            //    datanew.Correction = dataOld.Correction;
            //    datanew.CorrectiveAction = dataOld.CorrectiveAction;
            //    datanew.PreventingAction = dataOld.PreventingAction;
            //    //datanew.MenPowerUsed = string.Join("; "; allMenPower.Where(w => w.BreakDownId == dataOld.Id).Select(s => s.Name));
            //    //datanew.ServiceUsed = string.Join("; "; allServices.Where(w => w.BreakDownId == dataOld.Id).Select(s => s.Name));
            //    //datanew.PartsUsed = string.Join("; "; allParts.Where(w => w.BreakDownId == dataOld.Id).Select(s => s.Name))

            //    newDataSet.Add(datanew);
            //}

        }
        //public List<BreakDownReportDataSet> BreakDownDataSet()
        //{


        //    return newDataSource;
        //}

        public List<BreakDownReportDataSet> BreakDownDataSet()
        {

            return newDataSet;
        }
        public List<BreakDownReportDataSet> BreakDownAnalysisByPlant(int plantId)
        {

            return newDataSet;
        }
    }
}