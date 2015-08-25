<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--选择角色-->
<div id="roleSelect" class="easyui-window" title="选择角色" style="padding: 5px;width:420px;height:480px;"
    iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true" style="background:#fafafa;border:1px solid #99BBE8;">
        <div region="center" border="false" style="background:#fff;">
			<table id="dataGridRole" title="" style="" border="false" fit="true" singleSelect="true"
			    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tbRoleSelect"
                class="easyui-datagrid" data-options="
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleList',
                    onDblClickRow: function (rowIndex, rowData) {
                        RoleSelectOk();
                    }
                ">
                <thead>
				    <tr>
					    <th field="id" width="20" align="center" hidden="true">ID</th>
					    <th field="name" width="120" align="left">角色名称</th>
                        <th field="description" width="140" align="left">描述</th>
                        <th field="sortNo" width="50" align="center" sortable="true">序号</th>
				    </tr>
			    </thead>
			</table>
            <div id="tbRoleSelect">
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="RoleSelectOk()">确认</a>
			    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#roleSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<!--选择角色-->
<script type="text/javascript">
    $(function () {
    })
    function SelectRole() {
        $('#roleSelect').window('open');
    }
    function RoleSelectOk() {
        var selectRow = $('#dataGridRole').datagrid('getSelected');
        if (!selectRow) {
            alert("请选择角色！");
            return;
        }
        try {
            RoleSelected(selectRow);
        } catch (e) { ; }
        $('#roleSelect').window('close');
    }
</script>
