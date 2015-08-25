<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--批单物料打印-->
<div id="wDeliBatMaterial" class="easyui-window" title="物料标签打印" style="padding: 5px;width:660px;height:400px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgDeliBatMaterial" title="批单信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true, 
                              noheader: true,
                              singleSelect: false,
                              checkOnSelect: true,
                              selectOnCheck: true,
                              rownumbers: true,
                              idField: 'batchId',
                              striped: true,
                              toolbar: '#tbDeliBatMaterial',
                              pagination: true,
                              pageSize: 60,
                              pageList: [20, 40, 60, 100],
                              <%--loadMsg: '数据加载中......',--%>
                              onLoadSuccess: function(){
                                    $(this).datagrid('clearSelections');
                                    $(this).datagrid('clearChecked');                    
                              }">
                <thead>
                    <tr>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'batchId',hidden:true"></th>
                        <th data-options="field:'batchBarCode',width:70,align:'center'">批条码号</th>
                        <th data-options="field:'materialDocNumber',width:90,align:'center'">物料编码</th>
                        <th data-options="field:'materialDescription',width:190,align:'center'">物料描述</th>
                        <th data-options="field:'assignQuantity',width:60,align:'center'">接收数量</th>
                        <th data-options="field:'warehouseId',width:70,align:'center'">子库</th>
                        <th data-options="field:'locationDocNumber',width:70,align:'center'">分配货位</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbDeliBatMaterial" style="padding:5px;height:auto">
                <div style="margin-bottom:5px">
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" icon="icon-print"  onclick="deliBatIncAccMtlPrint();">打印</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" icon="icon-cancel"  onclick="$('#wDeliBatMaterial').window('close');">取消</a>
                    <input type="checkbox" id="batchPrint" name="printType" value="<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsMaterialLabel.PrintType.batchPrint %>"/><%=MideaAscm.Dal.Warehouse.Entities.AscmWmsMaterialLabel.PrintType.DisplayText(MideaAscm.Dal.Warehouse.Entities.AscmWmsMaterialLabel.PrintType.batchPrint) %>
                    <input type="checkbox" id="wipEntityPrint" name="printType" value="<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsMaterialLabel.PrintType.wipEntityPrint %>"/><%=MideaAscm.Dal.Warehouse.Entities.AscmWmsMaterialLabel.PrintType.DisplayText(MideaAscm.Dal.Warehouse.Entities.AscmWmsMaterialLabel.PrintType.wipEntityPrint) %>
                </div>
                <div>
                    <span>时间：</span>
                    <input id="startReceivedTime" class="easyui-datetimebox" data-options="showSeconds:false" style="width:125px"/>-<input id="endReceivedTime" class="easyui-datetimebox" data-options="showSeconds:false" style="width:125px"/>
                    <span>接收人：</span>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "100px" });%>
                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="deliBatMtlQuery();">查询</a>
                </div> 
            </div>
        </div>
    </div>
</div>
<object  id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0"> 
    <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
</object>
<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/CustomMaterialPrint2.js"></script>
<script type="text/javascript">
    var checkedPrintType = $('#batchPrint').attr("checked", "true");
    $(function () {
        $("[name='printType']").each(function () {
            $(this).click(function () {
                if ($(this).is(':checked')) {
                    if (checkedPrintType) {
                        checkedPrintType.removeAttr("checked");
                    }
                    checkedPrintType = $(this);
                    checkedPrintType.attr("checked", "true");
                } else if (checkedPrintType.attr("id") == $(this).attr("id")) {
                    $(this).attr("checked", "true");
                }
            })
        })
    })
    function deliBatMtlQuery() {
        var options = $('#dgDeliBatMaterial').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatMaterialAscxList';
        options.queryParams.startReceivedTime = $('#startReceivedTime').datetimebox('getText');
        options.queryParams.endReceivedTime = $('#endReceivedTime').datetimebox('getText');
        options.queryParams.receivedUserId = $('#warehouseUserSelect').combogrid('getValue');
        options.loadMsg = '数据加载中...';
        $('#dgDeliBatMaterial').datagrid('reload');
    }
    function getDeliBatMaterial() {
        $('#startReceivedTime').datetimebox('clear');
        $('#endReceivedTime').datetimebox('clear');
        $('#warehouseUserSelect').combogrid('clear');
        if (receivedBatchIds != "") {
            var options = $('#dgDeliBatMaterial').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatMaterialAscxList';
            options.queryParams.receivedBatchIds = receivedBatchIds;
            $('#dgDeliBatMaterial').datagrid('reload');
        } else {
            $('#dgDeliBatMaterial').datagrid('loadData', { "total": 0, "rows": [] });
        }
        $('#wDeliBatMaterial').window('open');
    }
    function deliBatIncAccMtlPrint() {
        var rows = $('#dgDeliBatMaterial').datagrid('getChecked');
        if (rows.length > 0) {
            printPreview(rows);
        } else {
            $.messager.alert('提示', '请勾选需要打印的数据！', 'info');
        }
    }
    function printPreview(rows) {
        $.ajax({
            type: "POST",
            url: '<%=Url.Action("WmsMaterialLabelPrint", "Warehouse", new { Area = "Ascm" })%>',
            cache: false,
            dataType: 'json',
            data: { "printType": checkedPrintType.val(), "dataJson": $.toJSON(rows) },
            beforeSend: function () {
                var options = $('#dgDeliBatMaterial').datagrid('options');
                options.loadMsg = '加载打印数据...';
                $('#dgDeliBatMaterial').datagrid('loading');
                return true;
            },
            success: function (result) {
                $('#dgDeliBatMaterial').datagrid('loaded');
                if (result.success) {
                    materialPrintPreview(LODOP, result.data);
                } else if (result.errorMsg) {
                    $.messager.alert('错误', result.errorMsg, 'error');
                }
            }
        });
    }
</script>

