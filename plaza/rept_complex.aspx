<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rept_complex.aspx.cs" Inherits="plaza.rept_complex" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Content/mycss.css" rel="stylesheet"/>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <title>Комплексный отчет</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel Width="480" ID="p_header" runat="server" HorizontalAlign="Center">
            <h1><span class="logo1">Pla</span><span class="logo2">Za</span>
        <asp:Label ID="l_header" runat="server" Text=" отчет"></asp:Label></h1>
        <h2><asp:Label ID="l_descript" runat="server" Text=""></asp:Label></h2>
        </asp:Panel>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="rv" runat="server" Height="1200" Width="477px" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="rep_complex.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="dset_complex" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" 
            TypeName="temp_web1.rds_complexTableAdapters.report_complexTableAdapter"></asp:ObjectDataSource>
    </form>
</body>
</html>
