<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--选择用户-->
<div id="userSelect" class="easyui-window" title="选择用户" style="padding: 5px;width:420px;height:480px;"
    iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true" style="background:#fafafa;border:1px solid #99BBE8;">
        <div region="center" border="false" style="background:#fff;">
			<table id="dataGridUser" title="" style="" border="false" fit="true"
			    idField="userId" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tbUserSelect"
                class="easyui-datagrid" data-options="
                    pagination: true,
                    pageSize: 30,
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserList',
                    onLoadSuccess: function () {
                        $(this).datagrid('clearSelections');
                    },
                    onDblClickRow: function (rowIndex, rowData) {
                        UserSelectOk();
                    }
                ">
                <thead>
				    <tr>
                        <th field="select" checkbox="true"></th>
					    <th field="userId" width="100" align="center">用户ID</th>
					    <th field="userName" width="120" align="left">用户名称</th>
				    </tr>
			    </thead>
			</table>
            <div id="tbUserSelect">
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="UserSelectOk()">确认</a>
			    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#userSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<!--选择岗位-->
<script type="text/javascript">
    $(function () {
    })
    function SelectUser() {
        $('#userSelect').window('open');
    }
    function UserSelectOk() {
//        var selectRow = $('#dataGridUser').datagrid('getSelected');
//        if (!selectRow) {
//            alert("请选择岗位！");
//            return;
        //        }
        var selectRows = $('#dataGridUser').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择岗位！", "info");
            return;
        }
        try {
            UserSelected(selectRows);
        } catch (e) { ; }
        $('#userSelect').window('close');
    }
</script>
