<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	员工车辆出入历史查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgEmployeeCarLog" title="员工车辆出入历史查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="" sortOrder="" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>大门：</span><%= Html.DropDownList("queryDoor", (IEnumerable<SelectListItem>)ViewData["listSelectDoor"], new { style = "width:100px;" })%>
            <span>方向：</span><%= Html.DropDownList("queryDirection", (IEnumerable<SelectListItem>)ViewData["listSelectDirection"], new { style = "width:100px;" })%>
            <span>车牌号：</span><input id="queryPlateNumber" name="queryPlateNumber" type="text" style="width:100px;" value=""/>
            <span>姓名：</span><input id="queryEmployeeName" name="queryEmployeeName" type="text" style="width:100px;" />
            <span>时间：</span><input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;"  value="<%= DateTime.Now.ToString("yyyy-MM-dd")%>"/>-<input class="easyui-datebox" id="queryEndTime" type="text" style="width:100px;" value="<%=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") %>"/>
            <input id="search" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dgEmployeeCarLog').datagrid({
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                //url: '',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'rfid', title: '电子标签', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'createTimeShow', title: '时间', width: 120, align: 'center' },
                    { field: 'employeeName', title: '员工姓名', width: 80, align: 'center' },
                    { field: 'plateNumber', title: '车牌号', width: 100, align: 'center' },
                    { field: 'description', title: '描述', width: 200, align: 'left' },
                    { field: 'doorName', title: '大门', width: 100, align: 'left' },
                    { field: 'readingHead', title: '位置', width: 100, align: 'center' }
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
            var options = $('#dgEmployeeCarLog').datagrid('options');
            options.url = "<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/EmployeeCarSwipeLogList/";
            options.queryParams.queryWord = $('#search').val();
            options.queryParams.doorId = $('#queryDoor').val();
            options.queryParams.direction = $('#queryDirection').val();
            options.queryParams.plateNumber = $('#queryPlateNumber').val();
            options.queryParams.employeeName = $('#queryEmployeeName').val();
            options.queryParams.queryStartTime = $("#queryStartTime").datebox('getText');
            options.queryParams.queryEndTime = $("#queryEndTime").datebox('getText');
            $('#dgEmployeeCarLog').datagrid('reload');
        }
        var currentId = null;
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/EmployeeCarSwipeLogPrint.aspx';
            url += "?doorId=" + $('#queryDoor').val();
            url += "&direction=" + $('#queryDirection').val();
            url += "&plateNumber=" + $('#queryPlateNumber').val();
            url += "&employeeName=" + $('#queryEmployeeName').val();
            url += "&queryStartTime=" + $("#queryStartTime").datebox('getText');
            url += "&queryEndTime=" + $("#queryEndTime").datebox('getText');
            url += "&queryWord=" + $('#search').val();
            parent.openTab('员工车辆出入日志打印', url);
        }
    </script>
</asp:Content>
