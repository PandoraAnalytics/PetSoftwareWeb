<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exportbrochure.aspx.cs" Inherits="Breederapp.exportbrochure" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource, ExportSummaryReport %>"></asp:Label></title><%--Export Summary Report--%>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="temp" runat="server"></asp:Label>
        <asp:Label ID="lblPleasewait" Text="<%$Resources:Resource, PleaseWait %>" runat="server"></asp:Label>
    </form>
</body>
</html>

