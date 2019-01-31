using ProERP.Services.Breakdown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProERP.Web.Reports
{
    public partial class HistoryCard : System.Web.UI.Page
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


                    int machineId = 0;
                    int.TryParse(Request.QueryString["MachineId"], out machineId);


                    //  int machineId = (Request.QueryString["MachineId"] != ""? int.Parse(Request.QueryString["MachineId"]):0) ;

                    DateTime startDate = DateTime.Parse(Request.QueryString["StartDate"]);
                    DateTime endDate = DateTime.Parse(Request.QueryString["EndDate"]);
                    //string year = Request.QueryString["year"];

                    BreakDownReportService _breakDownReportService = new BreakDownReportService();
                    var dataSource = _breakDownReportService.GetHistoryReportGridData(siteId, plantId, lineId, machineId, startDate, endDate, true);
                    //var dataSource = _breakDownReportService.GetHistoryReportGridData(siteId, plantId, lineId, year);
                    String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();
                    if (dataSource.Count > 0)
                    {
                        rvSiteMapping.Visible = true;
                        pnlNoData.Visible = false;
                        rvSiteMapping.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                        // rvSiteMapping.ServerReport.ReportPath = String.Format(@"\{0}\{1}", reportFolder, Request["ReportName"].ToString());


                        // Customers dsCustomers = GetData("select top 20 * from customers");
                        //ReportDataSource datasource = new ReportDataSource();
                        //datasource.Name = "DataSet1";
                        //rvSiteMapping.LocalReport.DataSources.Clear();
                        //rvSiteMapping.LocalReport.DataSources.Add(datasource);

                        Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                        datasrc.Name = "DataSet1";
                        //datasrc.DataSourceId=
                        rvSiteMapping.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));
                        this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DateFrom", startDate.ToString()));
                        this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DateTo", endDate.ToString()));
                        this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("LineName", dataSource.Select(f => f.LineName).FirstOrDefault()));
                    }
                    else
                    {
                        rvSiteMapping.Visible = false;
                        pnlNoData.Visible = true;
                        Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                        datasrc.Name = "DataSet1";
                        //datasrc.DataSourceId=
                        rvSiteMapping.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));
                        //this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DateFrom", DateTime.Now.ToString()));
                        //this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("DateTo", DateTime.Now.ToString()));
                        // this.rvSiteMapping.LocalReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter("LineName", "P1"));
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