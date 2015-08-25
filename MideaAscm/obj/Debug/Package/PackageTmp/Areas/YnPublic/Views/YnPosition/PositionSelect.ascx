<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--选择岗位-->
<div id="positionSelect" class="easyui-window" title="选择岗位" style="padding: 5px;width:420px;height:480px;"
    iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true" style="background:#fafafa;border:1px solid #99BBE8;">
        <div region="center" border="false" style="background:#fff;">
			<table id="dataGridPosition" title="" style="" border="false" fit="true"
			    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tbPositionSelect"
                class="easyui-datagrid" data-options="
                    pagination: true,
                    pageSize: 30,
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/PositionList',
                    onLoadSuccess: function () {
                        $(this).datagrid('clearSelections');
                    },
                    onDblClickRow: function (rowIndex, rowData) {
                        PositionSelectOk();
                    }
                ">
                <thead>
				    <tr>
                        <th field="select" checkbox="true"></th>
					    <th field="id" width="20" align="center" hidden="true">ID</th>
					    <th field="name" width="120" align="left">岗位名称</th>
                        <th field="description" width="140" align="left">描述</th>
                        <th field="sortNo" width="50" align="center" sortable="true">序号</th>
				    </tr>
			    </thead>
			</table>
            <div id="tbPositionSelect">
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="PositionSelectOk()">确认</a>
			    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#positionSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<!--选择岗位-->
<script type="text/javascript">
    $(function () {
    })
    function SelectPosition() {
        $('#positionSelect').window('open');
    }
    function PositionSelectOk() {
//        var selectRow = $('#dataGridPosition').datagrid('getSelected');
//        if (!selectRow) {
//            alert("请选择岗位！");
//            return;
        //        }
        var selectRows = $('#dataGridPosition').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择岗位！", "info");
            return;
        }
        try {
            PositionSelected(selectRows);
        } catch (e) { ; }
        $('#positionSelect').window('close');
    }
</script>
