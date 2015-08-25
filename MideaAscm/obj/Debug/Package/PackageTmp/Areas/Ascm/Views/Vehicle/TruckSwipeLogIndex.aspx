<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	车辆出入历史查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgTruckSwipeLog" title="车辆出入历史查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="" sortOrder="" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>大门：</span><%= Html.DropDownList("queryDoor", (IEnumerable<SelectListItem>)ViewData["listSelectDoor"], new { style = "width:100px;" })%>
            <span>车牌号：</span><input id="queryPlateNumber" name="queryPlateNumber" type="text" style="width:100px;" />
            <span>时间：</span><input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;"  value="<%= DateTime.Now.ToString("yyyy-MM-dd")%>"/>-<input class="easyui-datebox" id="queryEndTime" type="text" style="width:100px;" value="<%=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") %>"/>
            <input id="search" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#dgTruckSwipeLog').datagrid({
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/TruckSwipeLogList/',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'doorName', title: '大门', width: 100, align: 'left' },
                    { field: 'supplierName', title: '供应商', width: 180, align: 'center' }
                ]],
                columns: [[
                    { field: 'plateNumber', title: '车牌号', width: 100, align: 'center' },
                    { field: 'driverName', title: '司机', width: 60, align: 'center' },
                    { field: 'rfid', title: '司机编号', width: 100, align: 'center' },
                    { field: 'readingHead', title: '读头', width: 100, align: 'center' },
                    { field: 'directionCn', title: '方向', width: 60, align: 'center' },
                    { field: 'pass', title: '放行', width: 60, align: 'center' },
                    { field: 'createTimeShow', title: '时间', width: 120, align: 'left' },
                    { field: 'batSumDocNumber', title: '合单号', width: 80, align: 'center' },
                    { field: 'onTime', title: '是否准时', width: 60, align: 'center' },
                    { field: 'appointmentStartTime', title: '预约开始时间', width: 120, align: 'center' },
                    { field: 'appointmentEndTime', title: '预约最后时间', width: 120, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function Query() {
            var options = $('#dgTruckSwipeLog').datagrid('options');
            options.url = "<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/TruckSwipeLogList/";
            options.queryParams.queryWord = $('#search').val();
            options.queryParams.doorId = $('#queryDoor').val();
            options.queryParams.plateNumber = $('#queryPlateNumber').val();
            options.queryParams.queryStartTime = $("#queryStartTime").datebox('getText');
            options.queryParams.queryEndTime = $("#queryEndTime").datebox('getText');
            $('#dgTruckSwipeLog').datagrid('reload');
        }
        var currentId = null;
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/TruckSwipeLogPrint.aspx';
            url += "?doorId=" + $('#queryDoor').val();
            url += "&queryStartTime=" + $("#queryStartTime").datebox('getText');
            url += "&queryEndTime=" + $("#queryEndTime").datebox('getText');
            url += "&queryWord=" + $('#search').val();
            parent.openTab('供应商车辆出入日志打印', url);
        }
    </script>
</asp:Content>
