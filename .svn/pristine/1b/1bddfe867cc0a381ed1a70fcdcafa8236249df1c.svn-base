    <%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="false">
        <div id="ynportal" style="position: relative">
            <div style="width: 45%;">
                <div title="我的任务" id="divMyTaskRunning" collapsible="true" closable="false" style="height:290px;padding:5px;background: #E6E6FA;">
                    
                </div>
                <div title="公司新闻" collapsible="true" style="height: 290px;padding: 5px; background: #E6E6FA;">
                    <div>公告新闻发布！</div>
                </div>
            </div>
            <div style="width: 55%;">
                <div title="送货信息发布" collapsible="true" style="height: 290px; padding: 5px; background: #E6E6FA;">
                </div>
                <div title="物流完成情况" collapsible="true" style="height: 290px; padding: 5px; background: #E6E6FA;">
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/portal/jquery.portal.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery.chromatable.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery-ui-1.7.2.custom.min.js"></script>
    <link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/portal/portal.css" rel="stylesheet" type="text/css" />
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <style type="text/css">
        table.processTask {
          border: solid 1px #D5D5D5;
          border-collapse: collapse;
	      width:100%; 
        }
        table.processTask tr{
          height:24px;
        }
        table.processTask th {
	        /*background-color:#EEE;
            border-color:#D8D8D8;*/
	        background-color:#ADD8E6;
	        border-right:1px solid #D5D5D5;
	        /*font-size:13.5px;*/
	        line-height:120%;
	        font-weight:bold;
	        /*padding:8px 5px;
	        text-align:left;*/
        }
        table.processTask td {
	        border:1px solid #D5D5D5;
	        /*border-color:#D8D8D8;*/
	        font-size:12px;
	        /*padding:7px 5px;*/
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#ynportal').portal({
                border: false,
                fit: true
            });
        });
    </script>
    <%--加载部件--%>
    <script type="text/javascript">
        $(function () {
            $("#divMyTaskRunning").load("<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnOA/YnTask/MyTaskRunning");
        });
    </script>
</asp:Content>
