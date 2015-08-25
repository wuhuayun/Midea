<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	卸货点出入日志查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgUnloadingPointLog" title="卸货点使用历史查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库选择：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx"); %>
            <span>时间：</span><input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndTime" type="text" style="width:100px;" />
            <input id="search" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#dgUnloadingPointLog').datagrid({
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointLogList/',
                columns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'unloadingPointName', title: '卸货点名称', width: 160, align: 'center' },
                    { field: 'unloadingPointSn', title: '卸货点编号', width: 120, align: 'center' },
                    { field: 'unloadingPointStatus', title: '状态', width: 100, align: 'center' },
                    { field: 'createTimeShow', title: '时间', width: 160, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
            <%-- 初始化默认日期为当天 --%>
            var date = new Date();
            var yyyy = date.getFullYear();
            var MM = date.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            var dd = date.getDate();
            dd = dd < 10 ? "0" + dd : dd;
            $("#queryStartTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
            $("#queryEndTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        });
        function Query() {
            var options = $('#dgUnloadingPointLog').datagrid('options');
            options.queryParams.warehouseId = $('#warehouseSelect').combogrid('getValue');
            options.queryParams.queryWord = $('#search').val();
            options.queryParams.queryStartTime = $("#queryStartTime").datebox('getText');
            options.queryParams.queryEndTime = $("#queryEndTime").datebox('getText');
            $('#dgUnloadingPointLog').datagrid('reload');
        }
        var currentId = null;
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/UnloadingPointLogPrint.aspx';
            url += "?warehouseId=" + $('#warehouseSelect').combogrid('getValue');
            url += "&queryStartTime=" + $("#queryStartTime").datebox('getText');
            url += "&queryEndTime=" + $("#queryEndTime").datebox('getText');
            url += "&queryWord=" + $('#search').val();
            parent.openTab('卸货点出入日志打印', url);
        }
    </script>
</asp:Content>
