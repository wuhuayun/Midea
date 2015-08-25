<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择用户--%>
<div id="UserInfoSelect" class="easyui-window" title="选择用户" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectUserInfo" title="用户" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbUserInfoSelect',
                              <%--pagination: true,
                              pageSize: 30,--%>
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  UserInfoSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead>
                    <tr>
                        <th data-options="field:'userId',width:100,align:'left'">用户编号</th>
                        <th data-options="field:'userName',width:100,align:'left'">用户名称</th>
                        <th data-options="field:'sex',width:50,align:'left'">性别</th>
                        <th data-options="field:'email',width:200,align:'left'">EMail</th>
                        <th data-options="field:'accountLocked',width:100,align:'left'">账号锁定</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbUserInfoSelect">
                <input id="userInfoSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="UserInfoSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="UserInfoSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#UserInfoSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#userInfoSelectSearch').keypress(function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode == 13) {
                UserInfoSelectSearch();
            }
        });
        $('#userInfoSelectSearch').focus();
    })
    function SelectUserInfo() {
        $('#UserInfoSelect').window('open');
        $('#userInfoSelectSearch').val("");
        var options = $('#dgSelectUserInfo').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseUserInfoList';
        options.queryParams.queryWord = $('#userInfoSelectSearch').val();
        $('#dgSelectUserInfo').datagrid('reload');
    }
    function UserInfoSelectSearch() {
        var queryParams = $('#dgSelectUserInfo').datagrid('options').queryParams;
        queryParams.queryWord = $('#userInfoSelectSearch').val();
        $('#dgSelectUserInfo').datagrid('reload');
    }
    function UserInfoSelectOk() {
        var selectRow = $('#dgSelectUserInfo').datagrid('getSelected');
        if (selectRow) {
            try {
                UserInfoSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择人员！", "info");
            return;
        }
        $('#UserInfoSelect').window('close');
    }
</script>
