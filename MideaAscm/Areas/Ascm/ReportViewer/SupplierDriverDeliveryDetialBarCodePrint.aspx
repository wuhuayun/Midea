<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplierDriverDeliveryDetialBarCodePrint.aspx.cs" Inherits="MideaAscm.Areas.Ascm.ReportViewer.SupplierDriverDeliveryDetialBarCodePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css"> 
        html,body{height:100%; margin:0px;} 
        #mainbody{width:100%; height:100%;} 
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%" height="100%" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td valign="top" align="center">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%" SizeToReportContent="True" onprerender="ReportViewer1_PreRender">
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
