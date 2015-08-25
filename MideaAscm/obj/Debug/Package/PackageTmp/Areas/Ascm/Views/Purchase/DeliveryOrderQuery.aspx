﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	送货单统计和单据
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryOrder" title="送货单统计" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="signTime" sortOrder="desc" striped="true" toolbar="#tb">
		</table>
        <div id="tb">
            <span>供&nbsp;应&nbsp;商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx"); %>
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
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryOrderPrint();">打印明细表</a>
            <%-- <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryOrderPrint();">打印汇总表</a>--%>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#dataGridDeliveryOrder').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryOrderMainList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px"><table id="deliveryOrderDetail_' + index + '"></table></div>';
                },
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '送货单号', width: 100, align: 'center' },
                ]],
                columns: [[
                    { field: '_deliveryTime', title: '送货日期', width: 110, align: 'center' },
                    { field: 'barCode', title: '条码', width: 70, align: 'center' },
                    { field: 'supplierDocNumber', title: '供应商编号', width: 80, align: 'center' },
                    { field: 'supplierName', title: '供应商', width: 200, align: 'left' },
                    { field: 'supperWarehouse', title: '出货子库', width: 80, align: 'left' },
                    { field: 'warehouseId', title: '收货子库', width: 80, align: 'center' },
                    
                //                    { field: 'name', title: '作业上线时间', width: 110, align: 'center' },
                //                    { field: 'name', title: '送货地点', width: 110, align: 'center' },
                //                    { field: 'name', title: '备注', width: 70, align: 'left' },
                    { field: 'statusCn', title: '状态', width: 50, align: 'center' },
                    { field: 'batchDocNumber', title: '批送货单号', width: 100, align: 'center' },
                    { field: 'batchBarCode', title: '批条码', width: 70, align: 'center' },
                    { field: 'detailCount', title: '记录数', width: 60, align: 'center' },
                    { field: 'totalNumber', title: '总数量', width: 60, align: 'center' },
                    { field: 'comments', title: '注释', width: 150, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onExpandRow: function (index, row) {
                    $('#deliveryOrderDetail_' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryOrderDetailList/' + row.id,
                        fitColumns: true,
                        singleSelect: true,
                        height: 'auto',
                        columns: [[
						    { field: 'id', title: 'ID', width: 20, hidden: true },
                            { field: 'cgid', title: '采购订单', align: 'left', width: 80 },
                            { field: 'materialDocNumber', title: '物料编码', align: 'left', width: 80 },
                            { field: 'materialName', title: '物料描述', align: 'left', width: 250 },
                            { field: 'ascmDeliveryNotifyDetail_needTime', title: '要求到货时间', align: 'center', width: 110 },
                            { field: 'materialUnit', title: '单位', align: 'center', width: 50 },
						    { field: 'deliveryQuantity', title: '送货数量', align: 'center', width: 70 },
                            { field: 'receivedQuantity', title: '接受数量', align: 'center', width: 70 }
					    ]],
                        onResize: function () {
                            $('#dataGridDeliveryOrder').datagrid('fixDetailRowHeight', index);
                        },
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                $('#dataGridDeliveryOrder').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        }
                    });
                    $('#dataGridDeliveryOrder').datagrid('fixDetailRowHeight', index);
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })
            InitWarehouseSelect();
        })
        function Query() {
            var queryParams = $('#dataGridDeliveryOrder').datagrid('options').queryParams;
            //queryParams.queryWord = $('#supplierSearch').val();
            queryParams.supplier = $('#supplierSelect').combogrid('getValue');
            queryParams.warehouse = $('#warehouseSelect').combogrid('getValue');
            queryParams.startDeliveryTime = $("#queryStartDeliveryTime").datebox('getText');
            queryParams.endDeliveryTime = $("#queryEndDeliveryTime").datebox('getText');
            queryParams.status = $('#queryStatus').val();

            $('#dataGridDeliveryOrder').datagrid('reload');
        }
        var currentId = null;
    </script>
</asp:Content>
