<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择卸货点--%>
<div id="unloadingPointSelect" class="easyui-window" title="选择卸货点" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectUnloadingPoint" title="卸货点信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              <%--checkOnSelect: false,
                              selectOnCheck: true,--%>
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbUnloadingPointSelect',
                              pagination: true,
                              pageSize: 20,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  UnloadingPointSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'name',width:100,align:'center'">卸货点名称</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'warehouseDescription',width:150,align:'left'">仓库描述</th>
                        <th data-options="field:'direction',width:100,align:'center'">方向</th>
                        <th data-options="field:'description',width:150,align:'left'">描述</th>
                        <th data-options="field:'location',width:60,align:'center'">位置</th>
                        <th data-options="field:'statusCn',width:80,align:'center'">状态</th>
                        <th data-options="field:'memo',width:200,align:'left'">备注</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbUnloadingPointSelect">
                <%--<span>仓库选择：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", 
                    new MideaAscm.Code.SelectCombo { queryParams = "'inUnloadingPoint':1", onChange = "warehouseOnChange" }); %>--%>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="UnloadingPointSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#unloadingPointSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectUnloadingPoint(mapId, warehouseId) {
        $('#unloadingPointSelect').window('open');
        var options = $('#dgSelectUnloadingPoint').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointNotInMapList',
        options.queryParams.mapId = mapId;
        options.queryParams.warehouseId = warehouseId;
        $('#dgSelectUnloadingPoint').datagrid('reload');
    }
    /*function warehouseOnChange(newVal, oldVal) {
        var options = $('#dgSelectUnloadingPoint').datagrid('options');
        options.queryParams.warehouseId = newVal;
        $('#dgSelectUnloadingPoint').datagrid('reload');
    }*/
    function UnloadingPointSelectOk() {
        var selectRows = $('#dgSelectUnloadingPoint').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择卸货点！", "info");
            return;
        }
        try {
            UnloadingPointSelected(selectRows);    
        } catch(e) { }
        $('#unloadingPointSelect').window('close');
    }
</script>
