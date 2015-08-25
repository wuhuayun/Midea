<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>FrameHome</title>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery.portal.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery.chromatable.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery-ui-1.7.2.custom.min.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/easyui/jquery.easyui.min.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/easyui/locale/easyui-lang-zh_CN.js"></script>
	<link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/easyui/themes/default/portal.css" rel="stylesheet" type="text/css" />
	<link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath  %>/Areas/YnPublic/Content/css/base.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        $(function () {
            $('#pp').portal({
                border: false,
                fit: true
            });
        });
//        function remove() {
//            $('#pp').portal('remove', $('#pgrid'));
//            $('#pp').portal('resize');
//        }
    </script>
    <style type="text/css">
        .title
        {
            font-size: 16px;
            font-weight: bold;
            padding: 20px 10px;
            background: #eee;
            overflow: hidden;
            border-bottom: 1px solid #ccc;
        }
        .t-list
        {
            padding: 1px;
        }
    </style>
</head>
<body class="easyui-layout">
    <div region="center" title="" border="false">
        <div id="pp" style="position: relative">
            <div style="width: 45%;">
                <div title="新闻动态" collapsible="true" style="text-align: center; background: #E6E6FA; min-height: 250px;padding: 5px;">
                    <div>公告新闻发布！</div>
                </div>
                <div title="库存预警" collapsible="true" style="min-height: 250px;padding: 5px; background: #E6E6FA;">
                    <%--<div class="t-list">
                        <a href="http://hbzhyp.com" target="_blank">湖北中合誉品公司</a></div>--%>
                    <%Html.RenderAction("StockAlert", "Public", new { area = "Invoicing" });%>
                </div>
            </div>
            <div style="width: 55%;">
                <div title="待办事项" collapsible="true" style="min-height: 250px; padding: 5px; background: #E6E6FA;">
                    <%Html.RenderAction("ToDoList", "Public", new { area = "Invoicing" });%>
                </div>
                <div title="物流配送" collapsible="true" style="min-height: 250px; padding: 5px; background: #E6E6FA;">
                    <%Html.RenderAction("DistributionList", "Public", new { area = "Invoicing" });%>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
