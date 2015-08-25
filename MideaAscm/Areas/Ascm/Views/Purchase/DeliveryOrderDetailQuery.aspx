<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	送货物料统计和单据
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryDetail" title="送货物料统计" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb">
		</table>
        <div id="tb">
            <span>供&nbsp;应&nbsp;商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx"); %>
            <span>物料：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Public/MaterialItemSelectCombo.ascx"); %>
            <span>仓库：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx"); %></br>
            送货时间：</span><input class="easyui-datebox" id="queryStartDeliveryTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndDeliveryTime" type="text" style="width:100px;" />
            <span>状态：</span>
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
            </select>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryDetailPrint();">打印明细表</a>
            <!--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryDetailPrint();">打印供应商汇总表</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryDetailPrint();">打印物料汇总表</a>-->
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dataGridDeliveryDetail').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryOrderDetailList2',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    //{ field: 'mainDocNumber', title: '送货单号', width: 100, align: 'center' },
                    //{ field: 'materialDocNumber', title: '物料编码', width: 90, align: 'center' }
                    {field: 'mainBatchDocNumber', title: '批送货单号', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'mainBatchBarCode', title: '批条码', width: 70, align: 'center' },
                    { field: 'materialName', title: '物料描述', align: 'left', width: 250 },
                    { field: 'ascmDeliveryNotifyDetail_needTime', title: '要求到货时间', align: 'center', width: 110 },
                    { field: 'materialUnit', title: '单位', align: 'center', width: 50 },
					{ field: 'deliveryQuantity', title: '送货数量', align: 'center', width: 70 },
                    { field: 'receivedQuantity', title: '接受数量', align: 'center', width: 70 },
                    { field: 'cgid', title: '采购订单', align: 'left', width: 80 },

                    { field: 'mainDeliveryTime', title: '送货日期', width: 110, align: 'center' },
                    { field: 'mainBarCode', title: '条码', width: 70, align: 'center' },
                    { field: 'supplierDocNumber', title: '供应商编号', width: 80, align: 'center' },
                    { field: 'supplierName', title: '供应商', width: 200, align: 'left' },
                    { field: 'warehouseId', title: '收货子库', width: 80, align: 'center' },
                    { field: 'mainStatusCn', title: '状态', width: 50, align: 'center' }
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
            var queryParams = $('#dataGridDeliveryDetail').datagrid('options').queryParams;
            //queryParams.queryWord = $('#supplierSearch').val();
            queryParams.supplier = $('#supplierSelect').combogrid('getValue');
            queryParams.materialItem = $('#materialItemSelect').combogrid('getValue');
            queryParams.warehouse = $('#warehouseSelect').combogrid('getValue');
            queryParams.startDeliveryTime = $("#queryStartDeliveryTime").datebox('getText');
            queryParams.endDeliveryTime = $("#queryEndDeliveryTime").datebox('getText');
            queryParams.status = $('#queryStatus').val();

            $('#dataGridDeliveryDetail').datagrid('reload');
        }
        function DeliveryDetailPrint() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/DeliveryOrderDetailPrint.aspx?supplier=' + $('#supplierSelect').combogrid('getValue'); //  + "&queryWord=" + $('#saleContractSearch').val();
            url += "&warehouse=" + $('#warehouseSelect').combogrid('getValue');
            url += "&materialItem=" + $('#materialItemSelect').combogrid('getValue');
            url += "&startDeliveryTime=" + $("#queryStartDeliveryTime").datebox('getText');
            url += "&endDeliveryTime=" + $("#queryEndDeliveryTime").datebox('getText');
            url += "&status=" + $('#queryStatus').val();
            parent.openTab('送货单明细统计打印', url);
        }
    </script>
</asp:Content>
