<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoryCard.aspx.cs" Inherits="ProERP.Web.Reports.HistoryCard" %>

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
            <rsweb:reportviewer id="rvSiteMapping" runat="server" width="99.9%" height="1000px" font-names="Verdana" font-size="8pt" waitmessagefont-names="Verdana" waitmessagefont-size="14pt">
                <LocalReport ReportPath="Reports\HistoryCard.rdlc">

                    <DataSources>
                        <rsweb:ReportDataSource Name="DataSet1"  ></rsweb:ReportDataSource>
                    </DataSources>
                </LocalReport>
            </rsweb:reportviewer>
          <asp:Panel ID="pnlNoData" runat="server" Visible="false"  HorizontalAlign="Center" Font-Size="20pt">
                No data to display
            </asp:Panel>
    </div>
    </form>
</body>
</html>
