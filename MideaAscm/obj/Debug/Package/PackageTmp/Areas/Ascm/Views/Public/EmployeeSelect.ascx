<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择员工--%>
<div id="employeeSelect" class="easyui-window" title="选择员工" style="padding: 5px;width:640px;height:480px;"
    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dataGridSelectEmployee" title="员工信息" style="" border="false" fit="true" singleSelect="true"
			    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tbEmployeeSelect">
		    </table>
            <div id="tbEmployeeSelect">
                <input id="employeeSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="EmployeeSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="EmployeeSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#employeeSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#dataGridSelectEmployee').datagrid({
            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeList',
            pageSize: 30,
            pagination: true,
            noheader: true,
            rownumbers: true,
            loadMsg: '加载数据...',
            columns: [[
                { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                { field: 'docNumber', title: '员工编号', width: 80, align: 'center' },
				{ field: 'name', title: '姓名', width: 80, align: 'center' },
				{ field: 'sex', title: '性别', width: 50, align: 'center' },
				{ field: 'email', title: 'EMail', width: 120, align: 'center' },
				{ field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
				{ field: 'mobileTel', title: '移动电话', width: 100, align: 'center' }
            ]],
            onSelect: function (rowIndex, rowData) {

            },
            onDblClickRow: function (rowIndex, rowData) {
                EmployeeSelectOk();
            },
            onLoadSuccess: function () {
                $(this).datagrid('clearSelections');
            }
        });
    })
    function SelectEmployee() {
        $('#employeeSelect').window('open');
        $('#employeeSelectSearch').val("")
        var queryParams = $('#dataGridSelectEmployee').datagrid('options').queryParams;
        queryParams.queryWord = $('#employeeSelectSearch').val();
        $('#dataGridSelectEmployee').datagrid('reload');
    }
    function EmployeeSelectSearch() {
        var queryParams = $('#dataGridSelectEmployee').datagrid('options').queryParams;
        queryParams.queryWord = $('#employeeSelectSearch').val();
        $('#dataGridSelectEmployee').datagrid('reload');
    }
    function EmployeeSelectOk() {
        var selectRow = $('#dataGridSelectEmployee').datagrid('getSelected');
        if (selectRow) {
            EmployeeSelected(selectRow);
        } else {
            $.messager.alert("提示", "没有选择礼包！", "info");
            return;
        }
        $('#employeeSelect').window('close');
    }
</script>
