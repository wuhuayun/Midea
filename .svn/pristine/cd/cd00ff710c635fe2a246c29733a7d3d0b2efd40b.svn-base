<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料送货状态实时监控
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryOrder" title="物料送货状态实时监控" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="signTime" sortOrder="desc" striped="true" toolbar="#tb">
		</table>
        <div id="tb">
            <span>供&nbsp;应&nbsp;商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "203px" }); %>
            <span>仓&nbsp;&nbsp;&nbsp;&nbsp;库：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx"); %><br/>
            <span>生成时间：</span><input class="easyui-datebox" id="queryStartCreateTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndCreateTime" type="text" style="width:100px;" />
            <%--<span>状态：</span>
			<select id="queryStatus" name="queryStatus" style="width:80px;">
                <option value=""></option>
                <% List<string> listStatusDefine = MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderMain.StatusDefine.GetList(); %>
                <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in listStatusDefine)
                    { %>
                <option value="<%=statusDefine %>"><%=MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderMain.StatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>--%>
            <span>采购员：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Public/EmployeeSelectCombo.ascx"); %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印统计表</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#dataGridDeliveryOrder').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/MaterialMonitorList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '批送货单号', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'barCode', title: '批条码', width: 70, align: 'center' },
                    { field: 'createTimeShow', title: '生成日期', width: 110, align: 'center' },
                    { field: 'supplierDocNumber', title: '供应商编号', width: 80, align: 'center' },
                    { field: 'supplierShortName', title: '供应商', width: 60, align: 'left' },
                    { field: 'materialName', title: '物料描述', align: 'left', width: 250 },
                    { field: 'supperWarehouse', title: '出货子库', width: 80, align: 'left' },
                    { field: 'warehouseId', title: '收货子库', width: 80, align: 'center' },
                    { field: 'statusCn', title: '状态', width: 50, align: 'center' },
                    { field: 'deliveryStatus', title: '送货状态', width: 60, align: 'center' },
                    { field: 'detailCount', title: '记录数', width: 60, align: 'center' },
                    { field: 'totalNumber', title: '总数量', width: 60, align: 'center' },
                    { field: 'wipLine', title: '送货地点', width: 80, align: 'center' },
                    { field: 'containerBindNumber', title: '容器绑定数量', width: 80, align: 'center' },
                    { field: 'palletBindNumber', title: '托盘绑定数量', width: 80, align: 'center' },
                    { field: 'driverBindNumber', title: '司机绑定数量', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
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
            var queryParams = $('#dataGridDeliveryOrder').datagrid('options').queryParams;
            queryParams.supplierId = $('#supplierSelect').combogrid('getValue');
            queryParams.warehouse = $('#warehouseSelect').combogrid('getValue');
            queryParams.startCreateTime = $("#queryStartCreateTime").datebox('getText');
            queryParams.endCreateTime = $("#queryEndCreateTime").datebox('getText');
            queryParams.employeeBuyer = $('#employeeSelect').combogrid('getValue');
            $('#dataGridDeliveryOrder').datagrid('reload');
        }
        var currentId = null;
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/MaterialMonitorPrint.aspx?supplier=' + $('#supplierSelect').combogrid('getValue');
            url += "&warehouse=" + $('#warehouseSelect').combogrid('getValue');
            url += "&startCreateTime=" + $("#queryStartCreateTime").datebox('getText');
            url += "&endCreateTime=" + $("#queryEndCreateTime").datebox('getText');
            url += "&employeeBuyer=" + $('#employeeSelect').combogrid('getValue');
            parent.openTab('物料送货监控打印', url);
        }
    </script>
</asp:Content>
