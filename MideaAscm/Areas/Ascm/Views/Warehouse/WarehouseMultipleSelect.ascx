<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择仓库--%>
<div id="warehouseSelect" class="easyui-window" title="选择仓库" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWarehouse" title="仓库信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWarehouseSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  WarehouseSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead>
                    <tr>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'id',width:60,align:'center'">ID</th>
                        <th data-options="field:'description',width:200,align:'left'">描述</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbWarehouseSelect">
                <input id="warehouseSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WarehouseSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WarehouseSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#warehouseSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWarehouse() {
        $('#warehouseSelect').window('open');
        $('#warehouseSelectSearch').val("");
        var options = $('#dgSelectWarehouse').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseList';
        options.queryParams.queryWord = $('#warehouseSelectSearch').val();
        $('#dgSelectWarehouse').datagrid('reload');
    }
    function WarehouseSelectSearch() {
        var queryParams = $('#dgSelectWarehouse').datagrid('options').queryParams;
        queryParams.queryWord = $('#warehouseSelectSearch').val();
        $('#dgSelectWarehouse').datagrid('reload');
    }
    function WarehouseSelectOk() {
        var selectRows = $('#dgSelectWarehouse').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择仓库！", "info");
            return;
        }
        try {
            WarehouseSelected(selectRows);
        } catch (e) { }
        $('#warehouseSelect').window('close');
    }
</script>
