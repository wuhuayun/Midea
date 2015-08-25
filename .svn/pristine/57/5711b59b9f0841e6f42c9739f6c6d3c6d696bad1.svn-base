<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择供应商--%>
<div id="supplierSelect" class="easyui-window" title="选择供应商" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectSupplier" title="供应商信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbSupplierSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              columns: [[
                                  { field: 'id', hidden: true },
                                  { field: 'docNumber', title: '编号', width: 100, align: 'center' },
                                  { field: 'name', title: '供应商全称', width: 300, align: 'left' },
                                  { field: 'description', title: '描述', width: 100, align: 'left' },
                                  { field: 'status', title: '状态', width: 50, align: 'left' },
                                  { field: 'enabled', title: '在用', width: 50, align: 'left' }      
                              ]],
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  SupplierSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
		    </table>
            <div id="tbSupplierSelect">
                <input id="supplierSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="SupplierSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="SupplierSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#supplierSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#supplierSelectSearch').keypress(function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode == 13) {
                SupplierSelectSearch();
            }
        })
    });
    function SelectSupplier() {
        $('#supplierSelect').window('open');
        $('#supplierSelectSearch').val("");
        var options = $('#dgSelectSupplier').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierList';
        options.queryParams.queryWord = $('#supplierSelectSearch').val();
        $('#dgSelectSupplier').datagrid('reload');
    }
    function SupplierSelectSearch() {
        var queryParams = $('#dgSelectSupplier').datagrid('options').queryParams;
        queryParams.queryWord = $('#supplierSelectSearch').val();
        $('#dgSelectSupplier').datagrid('reload');
    }
    function SupplierSelectOk() {
        var selectRow = $('#dgSelectSupplier').datagrid('getSelected');
        if (selectRow) {
            try {
                SupplierSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择供应商！", "info");
            return;
        }
        $('#supplierSelect').window('close');
    }
</script>
