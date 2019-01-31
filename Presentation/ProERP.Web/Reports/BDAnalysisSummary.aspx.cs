using ProERP.Services.Breakdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProERP.Web.Reports
{
    public partial class BDAnalysisSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    int siteId = int.Parse(Request.QueryString["SiteId"]);
                    int plantId =int.Parse(Request.QueryString["PlantId"]);
                    int lineId = 0; //int.Parse(Request.QueryString["LineId"]);
                    int month = 0;
                    string year = "2017";//Request.QueryString["year"];

                    BreakDownReportService _breakDownReportService = new BreakDownReportService();
                    //List<Services.Models.BreakDownMonthlySummaryDataSet> dataSource = _breakDownReportService.GetBreakdownDataGroupByType(1, plantId, 12, startDate, endDate);
                    List<Services.Models.BreakDownMonthlySummaryDataSet> dataSource = _breakDownReportService.GetBreakdownDataGroupByType(siteId, plantId, 0, year, month);
                    //new DateTime(dateTime.Year, dateTime.Month, 1)
                    if (dataSource.Count > 0)
                    {
                        rvSiteMapping.Visible = true;
                        pnlNoData.Visible = false;

                        int maxMonth = ((from d in dataSource select d.Date.Value).Max()).Month;
                        int minMonth = ((from d in dataSource select d.Date.Value).Min()).Month;
                        List<string> BreakDownType = new List<string> { "Electrical", "Mechanical", "Instrumentation", "Utility", "Power", "Process", "PRV", "Idle" };
                        List<string> lines = new List<string>();
                        if (lineId == 0)
                            lines = _breakDownReportService.getLineNamesByPlantId(plantId);
                        else
                            lines.Add(dataSource[0].LineName);
                        string plantName = _breakDownReportService.getPlantNameBylantId(plantId);

                        for (int i = 1; i <= 12; i++)
                        {
                            foreach (string lineName in lines)
                            {
                                foreach (string type in BreakDownType)
                                {
                                    dataSource.Add(new Services.Models.BreakDownMonthlySummaryDataSet
                                    {
                                        Date = new DateTime( int.Parse(year), i, 1),
                                        DaysInMonth = DateTime.DaysInMonth(int.Parse(year), i),
                                        TotalTime = 0,
                                        PlantName = plantName,
                                        LineName = lineName,
                                        BreakDownType = type
                                    });
                                }
                            }
                        }
                        String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();
                        rvSiteMapping.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                        Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                        datasrc.Name = "DataSet1";
                        rvSiteMapping.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));
                        this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("Year", DateTime.Now.ToString()));

                    }
                    else
                    {
                        rvSiteMapping.Visible = false;
                        pnlNoData.Visible = true;
                        Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                        datasrc.Name = "DataSet1";
                        rvSiteMapping.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));
                    }
                    rvSiteMapping.LocalReport.Refresh();
                }
                catch (Exception ex)
                {

                }
            }
        }

    }
}