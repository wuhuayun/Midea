<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	车间任务物料需求报表
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataWipDiscreteJobsBom" title="车间任务物料需求报表" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            编号：<input id="materialItem_DocNumber" type="text" style="width:100px;" />
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
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CuxWipReleaseHeadersList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'releaseHeaderId', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'ascmMaterialItem_DocNumber', title: '物料编码', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'ascmMaterialItem_Description', title: '物料描述', width: 300, align: 'left' },
                    { field: 'ascmMaterialItem_unit', title: '单位', width: 60, align: 'center' },
                    { field: 'wipSupplyTypeCn', title: '供应类型', width: 60, align: 'center' },
                    { field: 'requiredQuantity', title: '需求数', width: 80, align: 'center' },
                    { field: 'quantityIssued', title: '已发数', width: 80, align: 'center' },
                    { field: 'transactionQuantity', title: '现有数', width: 80, align: 'center' },
                    { field: 'orderCount', title: '现有订单', width: 80, align: 'center' },
                    { field: 'toOrgPrimaryQuantity', title: '未入库', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
//                onDblClickRow: function (rowIndex, rowData) {
//                    WipReleaseLinesView();
//                },
                onLoadSuccess: function (data) {
                    if (!data.result) {
                        $.messager.alert('错误', data.message, 'error');
                    } else {
                        $(this).datagrid('clearSelections');
                        if (currentId) {
                            $(this).datagrid('selectRecord', currentId);
                        }
                    }
                }
            });
            setTimeout(Query, 1000);
        });
        function Query() {
            var options = $('#dataWipDiscreteJobsBom').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsQuerySumList';
            options.queryParams.materialItem_DocNumber = $("#materialItem_DocNumber").val();
            options.queryParams.startPlanTime = $("#queryStartPlanTime").datebox('getText');
            options.queryParams.endPlanTime = $("#queryEndPlanTime").datebox('getText');
            options.queryParams.status = $('#queryStatus').val();
//            $('#dataGridCuxWipRelease').datagrid({
//                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CuxWipReleaseHeadersList'
//            });
            $('#dataWipDiscreteJobsBom').datagrid('reload');
        }
        var currentId = null;
    </script>
</asp:Content>
