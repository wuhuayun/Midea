<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择事务类型--%>
<div id="mesTransStyleSelect" class="easyui-window" title="选择事务类型" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectMesTransStyle" title="事务类型" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbMesTransStylelSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  MesTransStyleSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections'); 
                                  <%--$(this).datagrid('clearChecked');--%>                    
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <%--<th data-options="checkbox:true"></th>--%>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="field:'code',width:100,align:'left'">代码</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'name',width:100,align:'left'">名称</th>
                        <th data-options="field:'description',width:260,align:'left'">描述</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbMesTransStylelSelect">
                <input id="mesTransStyleSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="MesTransStyleSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="MesTransStyleSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#mesTransStyleSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectMesTransStyle() {
        $('#mesTransStyleSelect').window('open');
        $('#mesTransStyleSelectSearch').val("");
        var options = $('#dgSelectMesTransStyle').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MesTransStyleList';
        options.queryParams.queryWord = $('#mesTransStyleSelectSearch').val();
        $('#dgSelectMesTransStyle').datagrid('reload');
    }
    function MesTransStyleSelectSearch() {
        var queryParams = $('#dgSelectMesTransStyle').datagrid('options').queryParams;
        queryParams.queryWord = $('#mesTransStyleSelectSearch').val();
        $('#dgSelectMesTransStyle').datagrid('reload');
    }
    function MesTransStyleSelectOk() {
        var selectRow = $('#dgSelectMesTransStyle').datagrid('getSelected');
        if (selectRow) {
            try {
                MesTransStyleSelected(selectRow);
            } catch (e) { }
        }
        else {
            $.messager.alert("提示", "请选择事务类型！", "info");
            return;
        }
        $('#mesTransStyleSelect').window('close');
    }
</script>


