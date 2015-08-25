<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择货位--%>
<div id="warelocationSelect" class="easyui-window" title="选择货位" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWarelocation" title="货位信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWarelocationSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  WarelocationSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead>
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="field:'docNumber',width:100,align:'center'">货位编码</th>
                        <th data-options="field:'categoryCode',width:80,align:'center'">物料大类</th>
                        <th data-options="field:'typeCn',width:80,align:'center'">货位形式</th>
                        <th data-options="field: 'description',width:120,align:'center'">描述</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbWarelocationSelect">
                <input id="warelocationSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WarelocationSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WarelocationSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#warelocationSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWarelocation(warehouseId) {
        $('#warelocationSelect').window('open');
        $('#warelocationSelectSearch').val("");
        var options = $('#dgSelectWarelocation').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarelocationListByWarehouse';
        options.queryParams.queryWord = $('#warelocationSelectSearch').val();
        options.queryParams.warehouseId = warehouseId;
        $('#dgSelectWarelocation').datagrid('reload');
    }
    function WarelocationSelectSearch() {
        var queryParams = $('#dgSelectWarelocation').datagrid('options').queryParams;
        queryParams.queryWord = $('#warelocationSelectSearch').val();
        $('#dgSelectWarelocation').datagrid('reload');
    }
    function WarelocationSelectOk() {
        var selectRow = $('#dgSelectWarelocation').datagrid('getSelected');
        if (selectRow) {
            try {
                WarelocationSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择货位！", "info");
            return;
        }
        $('#warelocationSelect').window('close');
    }
</script>
