<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择读头--%>
<div id="readingHeadSelect" class="easyui-window" title="选择读头" style="padding: 5px;width:420px;height:480px;"
    iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true" style="background:#fafafa;border:1px solid #99BBE8;">
        <div region="center" border="false" style="background:#fff;">
			<table id="dataGridSelectReadingHead" title="" style="" border="false" fit="true"
			    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tbReadingHeadSelect"
                class="easyui-datagrid" data-options="
                    onLoadSuccess: function () {
                        $(this).datagrid('clearSelections');
                    },
                    onDblClickRow: function (rowIndex, rowData) {
                        ReadingHeadSelectOk();
                    }
                ">
			</table>
            <div id="tbReadingHeadSelect">
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="ReadingHeadSelectOk()">确认</a>
			    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#readingHeadSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#dataGridSelectReadingHead').datagrid({
            pagination: true,
            pageSize: 50,
            loadMsg: '加载数据......',
            columns: [[
                { field: 'select', title: '',checkbox:true},
                { field: 'id', title: 'ID', width: 100, align: 'center' },
				{ field: 'ip', title: 'Ip地址', width: 100, align: 'center' },
				{ field: 'port', title: '端口', width: 80, align: 'center' },
				{ field: 'status', title: '状态', width: 80, align: 'center' }
            ]]
        });
    })
    function SelectReadingHead(bingType) {
        $('#readingHeadSelect').window('open');
        $('#dataGridSelectReadingHead').datagrid({
            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ReadingHeadList/?bingType=' + bingType
        });
    }
    function ReadingHeadSelectOk() {
//        var selectRow = $('#dataGridReadingHead').datagrid('getSelected');
//        if (!selectRow) {
//            alert("请选择岗位！");
//            return;
        //        }
        var selectRows = $('#dataGridSelectReadingHead').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择岗位！", "info");
            return;
        }
        try {
            ReadingHeadSelected(selectRows);
        } catch (e) { ; }
        $('#readingHeadSelect').window('close');
    }
</script>
