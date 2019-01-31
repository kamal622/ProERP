using Microsoft.Reporting.WebForms;
using ProERP.Services.Indent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProERP.Web.Extras
{
    public partial class Download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string requestCode = Request.QueryString["RequestCode"];
            if (requestCode == "DownloadIndent")
            {
                int indentId = int.Parse(Request.QueryString["IndentId"]);
                string type = Request.QueryString["Type"];
                DownloadIndent(indentId, type);
            }
            else if (requestCode == "IndentAttachment") {
                int Id = int.Parse(Request.QueryString["Id"]);
                DownloadIndentAttachment(Id);
            }
        }


        protected void DownloadIndent(int id, string type)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            //here
            //set report
            //set report datasurce
            IndentReportService _IndentReportService = new IndentReportService();
            List<Services.Models.IndentReportDataSet> dataSource = _IndentReportService.GetIndentDetailById(id);
            if (dataSource.Count > 0)
            {

                //foreach (var ds in dataSource)
                //{
                //    if (ds.PlantId == null)
                //    {
                //        ds.PlantName = "General";
                //    }
                //}
               
                rvSiteMapping.Visible = false;
                pnlNoData.Visible = false;
                byte[] bytes = null;
                if (type == "PR")
                {
                    String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();
                    rvSiteMapping.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                    Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                    datasrc.Name = "DataSet1";
                    //datasrc.DataSourceId=
                    rvSiteMapping.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));

                    bytes = rvSiteMapping.LocalReport.Render(
                    "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);

                }
                else
                {
                    String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();
                    ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                    Microsoft.Reporting.WebForms.ReportDataSource datasrc = new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource);
                    datasrc.Name = "DataSet1";
                    //datasrc.DataSourceId=
                    ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", dataSource));

                    bytes = ReportViewer1.LocalReport.Render(
                    "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);
                }
                DownloadFile(bytes, string.Format("{0}.pdf", dataSource[0].IndentNo.Replace("/", "-")));

            }
        }

        protected void DownloadIndentAttachment(int Id)
        {
            IndentReportService service = new IndentReportService();
            var attachment = service.GetAttachment(Id);
            var path = ConfigurationManager.AppSettings["IndentAttachmentPath"];

            var fullFilePath = Path.Combine(path, attachment.SysFileName);
            DownloadFile(File.ReadAllBytes(fullFilePath), attachment.OriginalFileName);
        }

        protected void DownloadFile(byte[] bytes, string FileName)
        {
            //string path = MapPath(fname);
            //string path = fname;//Server.MapPath(fname);
            //string name = Path.GetFileName(path);
            string fileExtension = Path.GetExtension(FileName);
            string type = "";

            HttpContext.Current.Response.Clear();

            // Clear the content of the response
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            // Buffer response so that page is sent
            // after processing is complete.
            HttpContext.Current.Response.BufferOutput = true;

            // set known types based on file extension  
            if (fileExtension != null)
            {
                switch (fileExtension.ToLower())
                {
                    case ".csv":
                        type = "application/csv";
                        break;
                    case ".txt":
                        type = "text/plain";
                        break;
                    case ".pdf":
                        type = "Application/pdf";
                        break;
                    case ".doc":
                    case ".rtf":
                        type = "Application/msword";
                        break;
                    case ".xml":
                    case ".xsd":
                        type = "text/xml";
                        break;
                    case ".xls":
                    case ".xlsx":
                        type = "application/excel";
                        break;
                }
            }

            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Clear();
            if (type != "")
                HttpContext.Current.Response.ContentType = type; // +";charset=windows-1251";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + FileName + "");
            HttpContext.Current.Response.BinaryWrite(bytes);
            //HttpContext.Current.Response.WriteFile(stream);
            HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.Close();

            //File.Delete(path);
            HttpContext.Current.Response.End();

        }
    }
}