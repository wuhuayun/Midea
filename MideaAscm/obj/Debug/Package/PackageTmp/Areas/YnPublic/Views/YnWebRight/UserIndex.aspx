<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UserIndex
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--用户授权模块功能-->
	<div region="east" split="false" border="false" title="" style="width:500px;padding:0px 0px 0px 2px;overflow:auto;">
        <div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true">
                <div region="center" border="false">
                    <table id="dataGridUserModule" title="用户授权模块功能" style="" border="false" fit="true" 
				    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="tb3">
				        <thead>
					        <tr>
                                <th field="select" checkbox="true"></th>
						        <th field="id" width="50" align="center" hidden="true">ID</th>
						        <th field="moduleName" width="120" align="left">模块名称</th>
						        <th field="IsRightManage" width="55" align="center" editor="{type:'checkbox',options:{on:'√', off:''}}">管理</th>
                                <th field="IsRightAdd" width="55" align="center" editor="{type:'checkbox',options:{on:'√', off:''}}">增加</th>
                                <th field="IsRightDelete" width="55" align="center" editor="{type:'checkbox',options:{on:'√', off:''}}">删除</th>
                                <th field="IsRightEdit" width="55" align="center" editor="{type:'checkbox',options:{on:'√', off:''}}">编辑</th>
                                <th field="IsRightVerify" width="55" align="center" editor="{type:'checkbox',options:{on:'√', off:''}}">审核</th>
					        </tr>
				        </thead>
			        </table>
                    <div id="moduleSelect" class="easyui-window" title="选择模块" style="padding: 5px;width:540px;height:360px;"
				    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">                    
					        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
						        <table id="dataGridSelectModule" title="" style="" border="false" fit="true"
							    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
							        <thead>
								        <tr>
									        <th field="select" checkbox="true"></th>
									        <th field="id" width="50" align="center" hidden="true">ID</th>
									        <th field="name" width="230" align="left">模块名称</th>
								        </tr>
							        </thead>
						        </table>
                                <div id="tb1">
                                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="moduleSelectReload();">刷新</a>
                                    <input id="moduleSelectSearch" type="text" style="width:120px;" />
			                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="moduleSelectSearch();"></a>
                                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                                    <a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="moduleSelectOk()">确认</a>
                                    <%} %>
								    <a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#moduleSelect').window('close');">取消</a> 
                                </div>
					        </div>
				        </div>
			        </div>
                </div>
            </div>
		</div>
	</div>
    <!--用户-->
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="用户" fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
			        <table id="dataGridUser" title="" style="" border="false" fit="true" singleSelect="true"
					idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tb2">
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="userReload();">刷新</a>
			            <input id="userSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="userSearch();"></a>
                    </div>
			    </div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--用户-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridUser').datagrid({
                frozenColumns: [[
	                { field: 'userId', title: '用户账号', width: 120, sortable: true, align: 'left' }
				]],
                columns: [[
					{ field: 'userName', title: '姓名', width: 100, align: 'center' },
					{ field: 'sex', title: '性别', width: 50, align: 'center' },
					{ field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
					{ field: 'mobileTel', title: '移动电话', width: 100, align: 'center' },
					{ field: 'accountLocked', title: '账号锁定', width: 60, align: 'center' }
				]],
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserList',
                pagination: true,
                pageSize: 30,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    userModuleReload();
                },
                onDblClickRow: function (rowIndex, rowData) {
                    
                },
                onLoadSuccess: function () {
                    $('#dataGridUser').datagrid('clearSelections');
                    userModuleReload();
                }
            });
        });
        function userReload() {
            $('#dataGridUser').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserList',
                onLoadSuccess: function () {
                    //$('#dataGridUser').datagrid('clearSelections');
                    //userModuleReload();
                }
            })
        }
        function userSearch() {
            var queryParams = $('#dataGridUser').datagrid('options').queryParams;
            queryParams.queryWord = $('#userSearch').val();
            $('#dataGridUser').datagrid('reload');
        }
    </script>
    <!--用户授权模块功能-->
    <script type="text/javascript">
        var lastIndex;
        $(function () {
            $('#dataGridUserModule').datagrid({
                toolbar: [{
                    text: '刷新',
                    iconCls: 'icon-reload',
                    handler: function () {
                        userModuleReload();
                    }
                }<% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %> , '-', {
                    text: '添加',
                    iconCls: 'icon-add',
                    handler: userAddModule
                }<%} %><% if (ynWebRight.rightDelete){ %> , '-', {
                    text: '移除',
                    iconCls: 'icon-cancel',
                    handler: userRemoveModule
                }<%} %><% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %> , '-', {
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: userModuleRight
                }<%} %>],
                loadMsg: '更新数据......',
                onBeforeLoad: function () {
                    $(this).datagrid('rejectChanges');
                },
                onSelect: function (rowIndex, rowData) {
                    
                },
                onClickRow: function (rowIndex) {
                    if (lastIndex != rowIndex) {
                        $('#dataGridUserModule').datagrid('endEdit', lastIndex);
                        $('#dataGridUserModule').datagrid('beginEdit', rowIndex);
                    } else {
                        $('#dataGridUserModule').datagrid('beginEdit', rowIndex);
                    }
                    lastIndex = rowIndex;
                }
            });
        })
        function userModuleReload() {
            var userId = "";
            var title = '用户授权模块功能';
            var selectRow = $('#dataGridUser').datagrid('getSelected');
            if (selectRow) {
                userId = selectRow.userId;
                title = '用户[<font color="red">' + selectRow.userName + '</font>]授权模块功能';
            }
            $('#dataGridUserModule').datagrid({
                title: title,
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/UserModuleList/' + encodeURIComponent(userId),
                onLoadSuccess: function () {
                    $('#dataGridUserModule').datagrid('clearSelections');
                }
            });
        }
        function moduleSelectSearch() {
            var queryParams = $('#dataGridSelectModule').datagrid('options').queryParams;
            queryParams.queryWord = $('#moduleSelectSearch').val();
            $('#dataGridSelectModule').datagrid('reload');
            //重新载入后清除
            queryParams.queryWord = "";
        }
        function userAddModule() {
            var selectRow = $('#dataGridUser').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请选择用户！", "info");
                return;
            }
            $('#moduleSelectSearch').val("");
            $('#moduleSelect').window('open');
            $('#dataGridSelectModule').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/SelectModuleByUser/' + encodeURIComponent(selectRow.userId),
                pagination: true,
                onLoadSuccess: function () {
                    $('#dataGridSelectModule').datagrid('clearSelections');
                }
            });
        }
        function moduleSelectOk() {
            var selectRows = $('#dataGridSelectModule').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert("提示", "请选择要加入的模块！", "info");
                return;
            }
            $.messager.confirm('确认', '确认添加模块到用户？', function (result) {
                if (result) {
                    var selectRow = $('#dataGridUser').datagrid('getSelected');
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/UserAddModule/' + encodeURIComponent(selectRow.userId);
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { moduleJson: $.toJSON(selectRows) },
                        success: function (r) {
                            if (r.result) {
                                $('#moduleSelect').window('close');
                                userModuleReload();
                            } else {
                                $.messager.alert('确认', '添加失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function userRemoveModule() {
            var selectRows = $('#dataGridUserModule').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要移除的模块！', 'info');
                return;
            }
            $.messager.confirm('确认', '确认从用户中移除模块？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/UserRemoveModule';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { moduleUserLinkJson: $.toJSON(selectRows) },
                        success: function (r) {
                            if (r.result) {
                                userModuleReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function userModuleRight() {
            $('#dataGridUserModule').datagrid('acceptChanges');
            var data = $('#dataGridUserModule').datagrid('getData');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/UserModuleRight';
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                data: { "moduleUserLinkJson": $.toJSON(data.rows) },
                success: function (r) {
                    if (r.result) {
                        userModuleReload();
                    } else {
                        $.messager.alert('确认', '保存失败:' + r.message, 'error');
                    }
                }
            });
        } 
    </script>
</asp:Content>
