<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择手工单--%>
<div id="wmsIncManAccSelect" class="easyui-window" title="选择手工单" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWmsIncManAcc" title="手工单信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              <%--checkOnSelect: false,
                              selectOnCheck: true,--%>
                              idField: 'incManAccDetailId',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWmsIncManAccSelect',
                              pagination: true,
                              pageSize: 20,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  WmsIncManAccSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');
                                  $(this).datagrid('clearChecked');                   
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'docNumber',width:120,align:'center'">手工单号</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                        <th data-options="field:'materialName',width:250,align:'left'">物料描述</th>
                        <th data-options="field:'supplierName',width:180,align:'left'">供应商名称</th>
                        <th data-options="field:'supperWarehouse',width:80,align:'center'">出货子库</th>
                        <th data-options="field:'warehouseId',width:80,align:'center'">收货子库</th>
                        <th data-options="field:'receivedQuantity',width:60,align:'center'">总数量</th>
                        <th data-options="field:'supplierAddressVendorSiteCode',width:80,align:'center'">送货地点</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbWmsIncManAccSelect">
                <input id="wmsIncManAccSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WmsIncManAccSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WmsIncManAccSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wmsIncManAccSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWmsIncManAcc(supplierId) {
        $('#wmsIncManAccSelect').window('open');
        $('#wmsIncManAccSelectSearch').val("");
        var options = $('#dgSelectWmsIncManAcc').datagrid('options');
        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsIncManAccSelectList';
        options.url = url;
        options.queryParams.supplierId = supplierId;
        options.queryParams.queryWord = $('#wmsIncManAccSelectSearch').val();
        $('#dgSelectWmsIncManAcc').datagrid('reload');
    }
    function WmsIncManAccSelectSearch() {
        var queryParams = $('#dgSelectWmsIncManAcc').datagrid('options').queryParams;
        queryParams.queryWord = $('#wmsIncManAccSelectSearch').val();
        $('#dgSelectWmsIncManAcc').datagrid('reload');
    }
    function WmsIncManAccSelectOk() {
        var selectRows = $('#dgSelectWmsIncManAcc').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择手工单！", "info");
            return;
        }
        try {
            WmsIncManAccSelected(selectRows);
        } catch (e) { }
        $('#wmsIncManAccSelect').window('close');
    }
</script>
