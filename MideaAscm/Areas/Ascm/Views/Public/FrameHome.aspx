<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>FrameHome</title>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery.portal.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery.chromatable.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jquery/jquery-ui-1.7.2.custom.min.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/jquery.easyui.min.js"></script>
	<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/locale/easyui-lang-zh_CN.js"></script>
	<link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/themes/default/portal.css" rel="stylesheet" type="text/css" />
	<link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath  %>/Areas/YnPublic/Content/css/base.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        $(function () {
            $('#pp').portal({
                border: false,
                fit: true
            });
            $('#divStockAlert').load("/Invoicing/Public/StockAlert");
            $('#divToDoList').load("/Invoicing/Public/ToDoList");
            $('#divDistributionList').load("/Invoicing/Public/DistributionList");
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
    <div region="center" title="" border="false" style="overflow:hidden;">
        <img src="../../../../Content/images/bg_head.jpg" alt="" width="100%" height="100%"/>
    </div>
</body>
</html>
