<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择司机--%>
<div id="driverSelect" class="easyui-window" title="选择司机" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectDriver" title="司机信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: 'sn',
                              sortOrder: 'asc',
                              striped: true,
                              toolbar: '#tbDriverSelect',
                              pagination: true,
                              pageSize: 20,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  DriverSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="field:'sn',width:100,align:'center'">司机编号</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'name',width:80,align:'center'">姓名</th>
                        <th data-options="field:'sex',width:60,align:'center'">性别</th>
                        <th data-options="field:'mobileTel',width:100,align:'center'">移动电话</th>
                        <th data-options="field:'rfid',width:100,align:'center'">RFID</th>
                        <th data-options="field:'supplierName',width:180,align:'left'">供应商名称</th>
                        <th data-options="field:'typeCn',width:80,align:'center'">类型</th>
                        <th data-options="field:'plateNumber',width:80,align:'center'">车牌号</th>
                        <th data-options="field:'load',width:60,align:'center'">载重</th>
                        <th data-options="field:'description',width:160,align:'left'">描述</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbDriverSelect">
                <input id="driverSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="DriverSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="DriverSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#driverSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectDriver(supplierId) {
        $('#driverSelect').window('open');
        $('#driverSelectSearch').val("");
        var options = $('#dgSelectDriver').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DriverList';
        options.queryParams.supplierId = supplierId;
        options.queryParams.queryWord = $('#driverSelectSearch').val();
        $('#dgSelectDriver').datagrid('reload');
    }
    function DriverSelectSearch() {
        var queryParams = $('#dgSelectDriver').datagrid('options').queryParams;
        queryParams.queryWord = $('#driverSelectSearch').val();
        $('#dgSelectDriver').datagrid('reload');
    }
    function DriverSelectOk() {
        var selectRow = $('#dgSelectDriver').datagrid('getSelected');
        if (selectRow) {
            try {
                DriverSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择司机！", "info");
            return;
        }
        $('#driverSelect').window('close');
    }
</script>
