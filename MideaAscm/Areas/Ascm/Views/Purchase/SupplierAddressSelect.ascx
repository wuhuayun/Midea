<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择供应商--%>
<div id="supplierAddressSelect" class="easyui-window" title="选择供应商地址" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectSupplierAddress" title="供应商地址信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbSupplierAddressSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              columns: [[
                                  { field: 'id', title: 'Id', width: 100, align: 'center', hidden: 'true' },
                                  { field: 'vendorSiteCode', title: '名称', width: 150, align: 'center' },
                                  { field: 'vendorSiteCodeAlt', title: '简称', width: 200, align: 'left' }   
                              ]],
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  SupplierAddressSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
		    </table>
            <div id="tbSupplierAddressSelect">
                <input id="supplierAddressSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="SupplierAddressSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="SupplierAddressSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#supplierAddressSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectSupplierAddress(supplierId) {
        $('#supplierAddressSelect').window('open');
        $('#supplierAddressSelectSearch').val("");
        var options = $('#dgSelectSupplierAddress').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierAddressList/' + supplierId;
        options.queryParams.queryWord = $('#supplierAddressSelectSearch').val();
        $('#dgSelectSupplierAddress').datagrid('reload');
    }
    function SupplierAddressSelectSearch() {
        var queryParams = $('#dgSelectSupplierAddress').datagrid('options').queryParams;
        queryParams.queryWord = $('#supplierAddressSelectSearch').val();
        $('#dgSelectSupplierAddress').datagrid('reload');
    }
    function SupplierAddressSelectOk() {
        var selectRow = $('#dgSelectSupplierAddress').datagrid('getSelected');
        if (selectRow) {
            try {
                SupplierAddressSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择供应商！", "info");
            return;
        }
        $('#supplierAddressSelect').window('close');
    }
</script>
