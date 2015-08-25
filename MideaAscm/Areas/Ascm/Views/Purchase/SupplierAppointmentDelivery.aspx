<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商预约送货查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryNotify" title="预约送货管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb">
		</table>
        <div id="tb">
            <div style="padding:1px 0px">下单时间：<input class="easyui-datebox" id="queryStartReleasedTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndReleasedTime" type="text" style="width:100px;" />
            需求时间：<input class="easyui-datebox" id="queryStartNeedTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndNeedTime" type="text" style="width:100px;" />
            <span>状态：</span>
			<select id="queryStatus" name="queryStatus" style="width:80px;">
                <option value=""></option>
                <% List<string> listStatusDefine = MideaAscm.Dal.FromErp.Entities.AscmDeliveryNotifyMain.StatusDefine.GetSupplierList(); %>
                <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in listStatusDefine)
                    { %>
                <option value="<%=statusDefine %>"><%=MideaAscm.Dal.FromErp.Entities.AscmDeliveryNotifyMain.StatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select></div>
            <span>采&nbsp;购&nbsp;员：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Public/EmployeeSelectCombo.ascx"); %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
        </div>

	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#dataGridDeliveryNotify').datagrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryNotifyMainList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                //showFooter: true,
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '通知编号', width: 100, align: 'center' },
                ]],
                columns: [[
                    { field: 'wipEntitiesName', title: '任务号', width: 110, align: 'center' },
                    { field: 'wipEntitiesScheduledStartTime', title: '作业开始日期', width: 110, align: 'center' },
                    { field: '_releasedTime', title: '下单日期', width: 110, align: 'center' },
                    { field: '_needTime', title: '需求日期', width: 110, align: 'center' },
                    { field: 'supplierDocNumber', title: '供应商编号', width: 90, align: 'center' },
                    { field: 'warehouseId', title: '子库', width: 80, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' },
                    { field: 'materialName', title: '物料', width: 200, align: 'left' },
                    { field: 'releasedQuantity', title: '下单数量', width: 60, align: 'center' },
                    { field: 'promisedQuantity', title: '承诺数量', width: 60, align: 'center' },
                    { field: 'deliveryQuantity', title: '在途数量', width: 60, align: 'center' },
                    { field: 'detailCount', title: 'PO数', width: 60, align: 'center' },
                    { field: 'materialBuyerName', title: '采购员', width: 60, align: 'center' },
                    { field: 'createUserEmployeeName', title: '下单人', width: 60, align: 'center' },
                    { field: 'ascmHrLocationsAllCode', title: '厂区', width: 60, align: 'center' },
                    { field: 'bomDepartmentsCode', title: '车间', width: 60, align: 'center' },
                    { field: 'ascmFndLookupValuesMeaning', title: '备料类型', width: 60, align: 'center' },
                    { field: '_appointmentStartTime', title: '最早到货时间', width: 110, align: 'center' },
                    { field: '_appointmentEndTime', title: '预约到货时间', width: 110, align: 'center' },
                    { field: 'statusCn', title: '状态', width: 70, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })
        })
        function Query() {
            var queryParams = $('#dataGridDeliveryNotify').datagrid('options').queryParams;
            //queryParams.queryWord = $('#supplierSearch').val();
            queryParams.startReleasedTime = $("#queryStartReleasedTime").datebox('getText');
            queryParams.endReleasedTime = $("#queryEndReleasedTime").datebox('getText');
            queryParams.startNeedTime = $("#queryStartNeedTime").datebox('getText');
            queryParams.endNeedTime = $("#queryEndNeedTime").datebox('getText');
            queryParams.status = $('#queryStatus').val();
            queryParams.employeeBuyer = $('#employeeSelect').combogrid('getValue');
            //$('#dataGridDeliveryNotify').datagrid('reload');
            $('#dataGridDeliveryNotify').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierDeliveryNotifyMainList'
            });
        }
        var currentId = null;
    </script>
</asp:Content>
