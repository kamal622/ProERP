<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsolidateReport.aspx.cs" Inherits="ProERP.Web.Reports.ConsolidateReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<!doctype html>
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE11">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="rvSiteMapping" runat="server" Width="99.9%" Height="700px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                <LocalReport ReportPath="Reports\ConsolidateReport.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource Name="DataSet1"></rsweb:ReportDataSource>
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:Panel ID="pnlNoData" runat="server" Visible="false"  HorizontalAlign="Center" Font-Size="20pt">
                No data to display
            </asp:Panel>
        </div>
    </form>
</body>
</html>


