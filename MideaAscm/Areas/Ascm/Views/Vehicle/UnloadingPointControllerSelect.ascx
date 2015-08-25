<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择控制器--%>
<div id="unloadingPointControllerSelect" class="easyui-window" title="选择控制器" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectController" title="控制器信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbControllerSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  UnloadingPointControllerSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead>
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="field:'name',width:220,align:'center'">控制器名称</th>
                        <th data-options="field:'ip',width:100,align:'left'">IP地址</th>
                        <th data-options="field:'port',width:70,align:'left'">端口</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbControllerSelect">
                <input id="controllerSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="UnloadingPointControllerSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="UnloadingPointControllerSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#unloadingPointControllerSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectUnloadingPointController() {
        $('#unloadingPointControllerSelect').window('open');
        $('#controllerSelectSearch').val("");
        var options = $('#dgSelectController').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointControllerList';
        options.queryParams.queryWord = $('#controllerSelectSearch').val();
        $('#dgSelectController').datagrid('reload');
    }
    function UnloadingPointControllerSelectSearch() {
        var queryParams = $('#dgSelectController').datagrid('options').queryParams;
        queryParams.queryWord = $('#controllerSelectSearch').val();
        $('#dgSelectController').datagrid('reload');
    }
    function UnloadingPointControllerSelectOk() {
        var selectRow = $('#dgSelectController').datagrid('getSelected');
        if (selectRow) {
            try {
                UnloadingPointControllerSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择控制器！", "info");
            return;
        }
        $('#unloadingPointControllerSelect').window('close');
    }
</script>
