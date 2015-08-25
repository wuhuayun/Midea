<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择事务原因--%>
<div id="transReasonSelect" class="easyui-window" title="选择原因" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectTransReason" title="事务原因" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'reasonId',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbTransReasonSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  ReasonSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead>
                    <tr>
                        <th data-options="field:'reasonId',hidden:true"></th>
                        <th data-options="field:'reasonName',width:100,align:'left'">名称</th>
                        <th data-options="field:'description',width:200,align:'left'">描述</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbTransReasonSelect">
                <input id="reasonSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="ReasonSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="ReasonSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#transReasonSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#reasonSelectSearch').keypress(function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode == 13) {
                ReasonSelectSearch();
            }
        });
    })
    function SelectReason() {
        $('#transReasonSelect').window('open');
        $('#reasonSelectSearch').val("");
        var options = $('#dgSelectTransReason').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/TransactionReasonAscxList';
        options.queryParams.queryWord = $('#reasonSelectSearch').val();
        $('#dgSelectTransReason').datagrid('reload');
    }
    function ReasonSelectSearch() {
        var queryParams = $('#dgSelectTransReason').datagrid('options').queryParams;
        queryParams.queryWord = $('#reasonSelectSearch').val();
        $('#dgSelectTransReason').datagrid('reload');
    }
    function ReasonSelectOk() {
        var selectRow = $('#dgSelectTransReason').datagrid('getSelected');
        if (selectRow) {
            try {
                ReasonSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择事务原因！", "info");
            return;
        }
        $('#transReasonSelect').window('close');
    }
</script>
