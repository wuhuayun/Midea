<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业需求报表
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataWipDiscreteJobsBom" title="作业需求报表" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            作业名称：<input id="wipEntitiesName" type="text" style="width:100px;" />
            计划时间：<input class="easyui-datebox" id="queryStartPlanTime" type="text" style="width:100px;" value="<%= DateTime.Now.ToString("yyyy-MM-dd")%>" />-<input class="easyui-datebox" id="queryEndPlanTime" type="text" style="width:100px;" value="<%=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") %>" />
            <span>状态：</span>
			<select id="queryStatus" name="queryStatus" style="width:150px;">
                <option value=""></option>
                <% List<MideaAscm.Dal.FromErp.Entities.AscmFndLookupValues> listAscmFndLookupValues = MideaAscm.Services.FromErp.AscmWipDiscreteJobsService.GetInstance().GetStatusList(); %>
                <% if (listAscmFndLookupValues != null && listAscmFndLookupValues.Count > 0)
                    { %>
                <% foreach (MideaAscm.Dal.FromErp.Entities.AscmFndLookupValues ascmFndLookupValues in listAscmFndLookupValues)
                    { %>
                <option value="<%=ascmFndLookupValues.code %>"><%=ascmFndLookupValues.meaning%></option>
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
            $('#dataWipDiscreteJobsBom').datagrid({
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'releaseHeaderId', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'wipEntitiesName', title: '作业名称', width: 120, align: 'left' }
                ]],
                columns: [[
                    { field: 'ascmWipDiscreteJobsDescription', title: '作业描述', width: 300, align: 'left' },
                    { field: 'ascmWipDiscreteJobs_ascmMaterialItem_DocNumber', title: '装配件编码', width: 90, align: 'center' },
                    { field: 'ascmWipDiscreteJobs_ascmMaterialItem_Description', title: '装配件描述', width: 300, align: 'left' },
                    { field: 'ascmWipDiscreteJobsAscmWipScheduleGroupsName', title: '车间', width: 70, align: 'left' },
                    { field: 'ascmWipDiscreteJobsNetQuantity', title: '计划数量', width: 60, align: 'center' },
                    { field: 'ascmWipDiscreteJobsStatusType_Cn', title: '作业状态', width: 60, align: 'center' },
                    { field: 'ascmWipDiscreteJobs_startQuantity', title: '作业开始数量', width: 80, align: 'center' },
                    { field: 'ascmWipDiscreteJobs_quantityCompleted', title: '作业完成数量', width: 80, align: 'center' },
                    { field: 'ascmWipDiscreteJobsScheduledStartDate', title: '作业开始日期', width: 110, align: 'center' },
                    { field: 'ascmWipDiscreteJobs_dateReleased', title: '作业发放日期', width: 110, align: 'center' },
                    { field: 'ascmWipDiscreteJobs_scheduledCompletionDate', title: '作业完成日期', width: 110, align: 'center' },
                    { field: 'ascmWipDiscreteJobs_dateClosed', title: '作业关闭日期', width: 110, align: 'center' },
                    { field: 'ascmMaterialItem_DocNumber', title: '物料编码', width: 100, align: 'center' },
                    { field: 'ascmMaterialItem_Description', title: '物料描述', width: 300, align: 'left' },
                    { field: 'supplySubinventory', title: '供应子库', width: 80, align: 'left' },
                    { field: 'requiredQuantity', title: '需求数量', width: 70, align: 'center' },
                    { field: 'quantityIssued', title: '发料数量', width: 70, align: 'center' },
                    { field: 'quantityDifference', title: '差异数量', width: 70, align: 'center' },
                    { field: 'ascmWipDiscreteJobs_classCode', title: '作业类型', width: 110, align: 'center' },
                    { field: 'wipSupplyTypeCn', title: '供应类型', width: 60, align: 'center' }
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
            setTimeout(Query, 1000);
        });
        function Query() {
            var options = $('#dataWipDiscreteJobsBom').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsQueryList';
            options.queryParams.wipEntitiesName = $("#wipEntitiesName").val();
            options.queryParams.startPlanTime = $("#queryStartPlanTime").datebox('getText');
            options.queryParams.endPlanTime = $("#queryEndPlanTime").datebox('getText');
            options.queryParams.status = $('#queryStatus').val();
            $('#dataWipDiscreteJobsBom').datagrid('reload');
        }
        var currentId = null;
    </script>
</asp:Content>
