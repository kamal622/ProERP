using ProERP.Services.Indent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProERP.Web.Reports
{
    public partial class ConsolidateReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                   
                    string year = Request.QueryString["year"];

                    IndentReportService _IndentReportService = new IndentReportService();
                    //List<Services.Models.BreakDownMonthlySummaryDataSet> dataSource = _breakDownReportService.GetBreakdownDataGroupByType(1, plantId, 12, startDate, endDate);
                    List<Services.Models.ConsolidateIndentData> dataSource = _IndentReportService.GetConsolidatedIndentData( year);
                    //new DateTime(dateTime.Year, dateTime.Month, 1)
                    if (dataSource.Count > 0)
                    {
                        rvSiteMapping.Visible = true;
                        pnlNoData.Visible = false;

                        String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();
                        rvSiteMapping.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                        Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                        datasrc.Name = "DataSet1";
                        //datasrc.DataSourceId=
                        rvSiteMapping.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));

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