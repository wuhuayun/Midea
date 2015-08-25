<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.FromErp.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业领料单查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridCuxWipRelease" title="作业领料单查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            编号：<input id="releaseNumber" type="text" style="width:100px;" />
            计划时间：<input class="easyui-datebox" id="queryStartPlanTime" type="text" style="width:100px;" value="<%=ViewData["startPlanTime"] %>" />-<input class="easyui-datebox" id="queryEndPlanTime" type="text" style="width:100px;" value="<%=ViewData["endPlanTime"] %>" />
            <span>作业状态：</span>
			<select id="queryStatus" name="queryStatus" style="width:150px;">
                <option value=""></option>
                <% List<int> listStatusType = AscmWipDiscreteJobs.StatusTypeDefine.GetList(); %>
                <% if (listStatusType != null && listStatusType.Count > 0)
                    { %>
                <% foreach (int statusType in listStatusType)
                    { %>
                <option value="<%=statusType %>"><%=AscmWipDiscreteJobs.StatusTypeDefine.DisplayText(statusType) %></option>
                <% } %>
                <% } %>
            </select>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridCuxWipRelease').datagrid({
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'releaseHeaderId', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'releaseNumber', title: '编号', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'wipEntityName', title: '作业号', width: 120, align: 'center' },
                    { field: 'scheduledStartDateFt', title: '计划时间', width: 115, align: 'center' },
                    { field: 'wipSupplyTypeCn', title: '供应类型', width: 115, align: 'center' },
                    { field: 'scheduleGroupName', title: '车间', width: 70, align: 'left' },
                    { field: 'netQuantity', title: '计划数量', width: 60, align: 'center' },
                    { field: 'statusTypeCn', title: '作业状态', width: 60, align: 'center' },
                    { field: 'description', title: '作业说明', width: 300, align: 'left' },
                    { field: 'primaryItemDocNumber', title: '装配件', width: 100, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    //WipReleaseLinesView();
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
            //setTimeout(Query, 1000);
        });
        function Query() {
            var options = $('#dataGridCuxWipRelease').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CuxWipReleaseHeadersList';
            options.queryParams.releaseNumber = $("#releaseNumber").val();
            options.queryParams.startPlanTime = $("#queryStartPlanTime").datebox('getText');
            options.queryParams.endPlanTime = $("#queryEndPlanTime").datebox('getText');
            options.queryParams.statusType = $('#queryStatus').val();
            $('#dataGridCuxWipRelease').datagrid('reload');
        }
        var currentId = null;
        function WipReleaseLinesView() {
            var selectRow = $('#dataGridCuxWipRelease').datagrid('getSelected');
            if (selectRow) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CuxWipReleaseLinesView/' + selectRow.releaseHeaderId+"?mi=<%=Request["mi"].ToString() %>";
                parent.openTab('领料单_' + selectRow.releaseNumber, url);
            } else {
                $.messager.alert('提示', '请选择领料单！', 'info');
            }
        }
    </script>
</asp:Content>
