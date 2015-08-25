<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择货位物料--%>
<div id="locationMaterialSelect" class="easyui-window" title="选择货位物料" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectLocationMaterial" title="货位物料信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              striped: true,
                              toolbar: '#tbLocationMaterialSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections'); 
                                  $(this).datagrid('clearChecked');                    
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'locationDocNumber',width:100,align:'left'">货位编号</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'materialDocNumber',width:100,align:'left'">物料编码</th>
                        <th data-options="field:'materialName',width:260,align:'left'">物料描述</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbLocationMaterialSelect">
                <input id="locationMaterialSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="LocationMaterialSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="LocationMaterialSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#locationMaterialSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectLocationMaterial(warehouseId) {
        $('#locationMaterialSelect').window('open');
        $('#locationMaterialSelectSearch').val("");
        var options = $('#dgSelectLocationMaterial').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationMaterialAscxList';
        options.queryParams.queryWord = $('#locationMaterialSelectSearch').val();
        options.queryParams.warehouseId = warehouseId;
        $('#dgSelectLocationMaterial').datagrid('reload');
    }
    function LocationMaterialSelectSearch() {
        var queryParams = $('#dgSelectLocationMaterial').datagrid('options').queryParams;
        queryParams.queryWord = $('#locationMaterialSelectSearch').val();
        $('#dgSelectLocationMaterial').datagrid('reload');
    }
    function LocationMaterialSelectOk() {
        var checkRows = $('#dgSelectLocationMaterial').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择货位物料！", "info");
            return;
        }
        try {
            LocationMaterialSelected(checkRows);
        } catch (e) { }
        $('#locationMaterialSelect').window('close');
    }
</script>
