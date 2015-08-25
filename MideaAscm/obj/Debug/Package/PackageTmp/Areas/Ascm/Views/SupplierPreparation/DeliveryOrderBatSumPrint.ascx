<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% string datagridId = "dgDeliveryOrderBatch"; %>
<% if (Model != null) datagridId = Model; %>
<div id="printBatSumMain" class="easyui-window" title="供应商合单打印" style="padding: 10px;width:480px;height:320px;"
	iconCls="icon-print" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
	<div class="easyui-layout" fit="true">
		<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
				<table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					<tr style="height:24px">
						<td style="width: 20%; text-align:right;" nowrap>
							<span>收货子库：</span>
						</td>
						<td style="width:80%">
							<select id="warehouseIdSelect" onchange="SelectWarehouseId();" style="width:120px;"></select>
						</td>
					</tr>	
					<tr style="height:24px">
						<td style="text-align:right;" nowrap>
							<span>物料编码：</span>
						</td>
						<td>
							<select id="materialDocNumberSelect" onchange="SelectMaterialDocNumber();" style="width:120px;"></select>(<font color="red">编码前四位</font>)
						</td>
					</tr>
				</table>
		</div>
		<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
			<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="DeliveryPrint();">确认</a>
			<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#printBatSumMain').window('close');">取消</a>
		</div>
	</div>
</div>
<script type="text/javascript">
    function LoadWarehouseIdAndMaterialCode(data) {
        $("#warehouseIdSelect").empty();
        $("#materialDocNumberSelect").empty();
        var warehouseIds = [];
        var materialCodes = [];
        if (data && data.rows.length > 0) {
            $.each(data.rows, function (index, item) {
                if ($.inArray(item.batchWarehouseId, warehouseIds) == -1) {
                    warehouseIds.push(item.batchWarehouseId);
                    $("#warehouseIdSelect").append("<option value='" + item.batchWarehouseId + "'>" + item.batchWarehouseId + "</option>");
                }
                var materialCode = item.materialDocNumber.substr(0, 4);
                if ($.inArray(materialCode, materialCodes) == -1) {
                    materialCodes.push(materialCode);
                    $("#materialDocNumberSelect").append("<option value='" + materialCode + "'>" + materialCode + "</option>");
                }
            })
        }
    }
    function SelectWarehouseId() {
        var warehouseId = $("#warehouseIdSelect").val();
        $("#materialDocNumberSelect").empty();
        var materialCodes = [];
        var data = $('#<%=datagridId %>').datagrid('getData');
        if (data && data.rows.length > 0) {
            $.each(data.rows, function (index, item) {
                if (item.batchWarehouseId == warehouseId) {
                    var materialCode = item.materialDocNumber.substr(0, 4);
                    if ($.inArray(materialCode, materialCodes) == -1) {
                        materialCodes.push(materialCode);
                        $("#materialDocNumberSelect").append("<option value='" + materialCode + "'>" + materialCode + "</option>");
                    }
                }
            });
        }
    }
    function SelectMaterialDocNumber() {
        var materialCode = $("#materialDocNumberSelect").val();
        $("#warehouseIdSelect").empty();
        var warehouseIds = [];
        var data = $('#<%=datagridId %>').datagrid('getData');
        if (data && data.rows.length > 0) {
            $.each(data.rows, function (index, item) {
                var _materialCode = item.materialDocNumber.substr(0, 4);
                if (materialCode == _materialCode) {
                    if ($.inArray(item.batchWarehouseId, warehouseIds) == -1) {
                        warehouseIds.push(item.batchWarehouseId);
                        $("#warehouseIdSelect").append("<option value='" + item.batchWarehouseId + "'>" + item.batchWarehouseId + "</option>");
                    }
                }
            });
        }
    }
    function DeliveryPrint() {
        var selectRow = $('#dgBatSumMain').datagrid('getSelected');
        if (selectRow) {
//            if (selectRow.status == '<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.unConfirm %>') {
//                $.messager.alert('提示', '不能打印状态【<font color="red">' + selectRow.statusCn + '</font>】的供应商送货合单', 'info');
//                return;
//            }
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/SupplierDriverDeliveryPrint.aspx?id=' + selectRow.id;
            url += '&warehouseId=' + $('#warehouseIdSelect').val();
            url += '&materialCode=' + $('#materialDocNumberSelect').val();
            parent.openTab('供应商送货合单打印', url);
        } else {
            $.messager.alert('提示', '请选择要打印的供应商送货合单', 'info');
        }
    }
</script>