using ProERP.Data.Models;
using ProERP.Services.Breakdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProERP.Web.Reports
{
    public partial class MonthlyBreakdownReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    int siteId = int.Parse(Request.QueryString["SiteId"]);
                    int plantId = int.Parse(Request.QueryString["PlantId"]);
                    int lineId = int.Parse(Request.QueryString["LineId"]);
                  //  DateTime startDate = DateTime.Now.AddDays(-40);
                    //DateTime endDate = DateTime.Now;
                    string year = Request.QueryString["year"];
                    int month = int.Parse(Request.QueryString["Month"]);
                    BreakDownReportService _breakDownReportService = new BreakDownReportService();
                    //List<Services.Models.BreakDownMonthlySummaryDataSet> dataSource = _breakDownReportService.GetBreakdownDataGroupByType(1, plantId, 12, startDate, endDate);
                    List<Services.Models.BreakDownMonthlySummaryDataSet> dataSource = _breakDownReportService.GetBreakdownDataGroupByType(1, plantId, lineId, year, month);
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
                        if(month == 0)
                        {
                            for (int i = 1; i <= maxMonth; i++)
                            {
                                foreach (string lineName in lines)
                                {
                                    foreach (string type in BreakDownType)
                                    {
                                        dataSource.Add(new Services.Models.BreakDownMonthlySummaryDataSet
                                        {
                                            Date = new DateTime(int.Parse(year), i, 1),
                                            TotalTime = 0,
                                            PlantName = plantName,
                                            LineName = lineName,
                                            BreakDownType = type
                                        });
                                    }
                                }
                            }
                        }
                       


                        String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();


                        rvSiteMapping.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                        //rvSiteMapping.ServerReport.ReportPath = String.Format(@"\{0}\{1}", reportFolder, Request["ReportName"].ToString());


                        // Customers dsCustomers = GetData("select top 20 * from customers");
                        //ReportDataSource datasource = new ReportDataSource();
                        //datasource.Name = "DataSet1";
                        //rvSiteMapping.LocalReport.DataSources.Clear();
                        //rvSiteMapping.LocalReport.DataSources.Add(datasource);

                        Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                        datasrc.Name = "DataSet1";
                        //datasrc.DataSourceId=
                        rvSiteMapping.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));
                        //this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DateFrom", DateTime.Now.ToString()));
                        //this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DateTo", DateTime.Now.ToString()));
                        //this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("LineName", "P1"));

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