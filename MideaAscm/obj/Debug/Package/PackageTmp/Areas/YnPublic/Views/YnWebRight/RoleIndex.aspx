<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RoleIndex
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--角色授权模块功能-->
	<div region="east" split="false" border="false" title="" style="width:500px;padding:0px 0px 0px 2px;overflow:auto;">
        <div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true">
                <div region="center" border="false">
                    <table id="dataGridRoleModule" title="角色授权模块功能" style="" border="false" fit="true" 
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
    <!--角色-->
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="角色" fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
			        <table id="dataGridRole" title="" style="" border="false" fit="true" singleSelect="true"
					idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb2">
                        <thead>
				            <tr>
					            <th field="id" width="20" align="center" hidden="true">ID</th>
                                <th field="sortNo" width="50" align="center" sortable="true">序号</th>
					            <th field="name" width="120" align="left">角色名称</th>
				            </tr>
			            </thead>
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="roleReload();">刷新</a>
			            <input id="userSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="roleSearch();"></a>
                    </div>
			    </div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--角色-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridRole').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleList',
//                pagination: true,
//                pageSize: 30,
                loadMsg: '更新数据......',
                onSelect: function (rowIndex, rowData) {
                    roleModuleReload();
                },
                onDblClickRow: function (rowIndex, rowData) {
                    
                },
                onLoadSuccess: function () {
                    $('#dataGridRole').datagrid('clearSelections');
                    roleModuleReload();
                }
            });
        });
        function roleReload() {
            $('#dataGridRole').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleList',
                onLoadSuccess: function () {
                    //$('#dataGridRole').datagrid('clearSelections');
                    //roleModuleReload();
                }
            });
        }
        function roleSearch() {
            var queryParams = $('#dataGridRole').datagrid('options').queryParams;
            queryParams.queryWord = $('#roleSearch').val();
            $('#dataGridRole').datagrid('reload');
        }
    </script>
    <!--角色授权模块功能-->
    <script type="text/javascript">
        var lastIndex;
        $(function () {
            $('#dataGridRoleModule').datagrid({
                toolbar: [{
                    text: '刷新',
                    iconCls: 'icon-reload',
                    handler: function () {
                        roleModuleReload();
                    }
                }<% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %> , '-', {
                    text: '添加',
                    iconCls: 'icon-add',
                    handler: roleAddModule
                }<%} %><% if (ynWebRight.rightDelete){ %> , '-', {
                    text: '移除',
                    iconCls: 'icon-cancel',
                    handler: roleRemoveModule
                }<%} %><% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %> , '-', {
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: roleModuleRight
                }<%} %>],
                loadMsg: '更新数据......',
                onBeforeLoad: function () {
                    $(this).datagrid('rejectChanges');
                },
                onSelect: function (rowIndex, rowData) {

                },
                onClickRow: function (rowIndex) {
                    if (lastIndex != rowIndex) {
                        $('#dataGridRoleModule').datagrid('endEdit', lastIndex);
                        $('#dataGridRoleModule').datagrid('beginEdit', rowIndex);
                    } else {
                        $('#dataGridRoleModule').datagrid('beginEdit', rowIndex);
                    }
                    lastIndex = rowIndex;
                }
            });
        })
        function roleModuleReload() {
            var roleId = "";
            var title = '角色授权模块功能';
            var selectRow = $('#dataGridRole').datagrid('getSelected');
            if (selectRow) {
                roleId = selectRow.id;
                title = '角色[<font color="red">' + selectRow.name + '</font>]授权模块功能';
            }
            $('#dataGridRoleModule').datagrid({
                title: title,
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/RoleModuleList/' + roleId,
                onLoadSuccess: function () {
                    $('#dataGridRoleModule').datagrid('clearSelections');
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
        function roleAddModule() {
            var selectRow = $('#dataGridRole').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请选择角色！", "info");
                return;
            }
            $('#moduleSelectSearch').val("");
            $('#moduleSelect').window('open');
            $('#dataGridSelectModule').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/SelectModuleByRole/' + selectRow.id,
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
            $.messager.confirm('确认', '确认添加模块到角色？', function (result) {
                if (result) {
                    var selectRow = $('#dataGridRole').datagrid('getSelected');
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/RoleAddModule/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { moduleJson: $.toJSON(selectRows) },
                        success: function (r) {
                            if (r.result) {
                                $('#moduleSelect').window('close');
                                roleModuleReload();
                            } else {
                                $.messager.alert('确认', '添加失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function roleRemoveModule() {
            var selectRows = $('#dataGridRoleModule').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要移除的模块！', 'info');
                return;
            }
            $.messager.confirm('确认', '确认从角色中移除模块？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/RoleRemoveModule';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { moduleRoleLinkJson: $.toJSON(selectRows) },
                        success: function (r) {
                            if (r.result) {
                                roleModuleReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function roleModuleRight() {
            $('#dataGridRoleModule').datagrid('acceptChanges');
            var data = $('#dataGridRoleModule').datagrid('getData');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/RoleModuleRight';
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                data: { "moduleRoleLinkJson": $.toJSON(data.rows) },
                success: function (r) {
                    if (r.result) {
                        roleModuleReload();
                    } else {
                        $.messager.alert('确认', '保存失败:' + r.message, 'error');
                    }
                }
            });
        } 
    </script>
</asp:Content>
