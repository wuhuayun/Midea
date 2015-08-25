<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnDepartment>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--关联用户-->
	<div region="east" split="false" border="false" title="" style="width:300px;padding:0px 0px 0px 2px;overflow:auto;">
        <div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true">
                <div region="center" border="false">
                    <table id="dataGridDepartmentUser" title="部门用户" style="" border="false" fit="true" 
				    idField="userId" sortName="userId" sortOrder="asc" striped="true">
				        <thead>
					        <tr>
                                <th field="select" checkbox="true"></th>
						        <th field="userId" width="100" align="center">用户账号</th>
						        <th field="userName" width="120" align="left">用户名称</th>
					        </tr>
				        </thead>
			        </table>
                    <div id="userSelect" class="easyui-window" title="选择用户" style="padding: 5px;width:540px;height:360px;"
				    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">                    
					        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
						        <table id="dataGridSelectUser" title="" style="" border="false" fit="true"
							    idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tb1">
							        <thead>
								        <tr>
									        <th field="select" checkbox="true"></th>
									        <th field="userId" width="100" align="center">用户账号</th>
									        <th field="userName" width="120" align="left">用户名称</th>
								        </tr>
							        </thead>
						        </table>
                                <div id="tb1">
                                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="userSelectReload();">刷新</a>
                                    <input id="userSelectSearch" type="text" style="width:120px;" />
			                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="userSelectSearch();"></a>
                                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                                    <a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="userSelectOk()">确认</a>
                                    <%} %>
								    <a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#userSelect').window('close');">取消</a> 
                                </div>
					        </div>
				        </div>
			        </div>
                </div>
            </div>
		</div>
	</div>
    <!--部门-->
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="部门管理 " fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
                    <table id="dataGridDepartment" title="" style="" border="false" fit="true"
                        idField="id" treeField="name" sortName="sortNo" sortOrder="asc" toolbar="#tb2">
                        <thead>
				            <tr>
					            <th field="id" width="20" align="center" hidden="true">ID</th>
					            <th field="name" width="220" align="left">部门名称</th>
                                <th field="description" width="300" align="left">描述</th>
                                <th field="sortNo" width="50" align="center" sortable="true">序号</th>
				            </tr>
			            </thead>
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="departmentReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="departmentAddRoot();">增加一级部门</a>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="departmentAddChild();">增加子部门</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="departmentEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="departmentDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editDepartment" class="easyui-window" title="部门" style="padding: 10px;width:500px;height:300px;"
			            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
					        <div region="center" id="editDepartmentContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                                <form id="editDepartmentForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>部门名称：</span>
						                    </td>
						                    <td style="width:80%">
                                                <input id="parentId" name="parentId" type="text" style="width:0px;display:none" />
							                    <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:200px;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>					
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>序号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" validType="number" style="text-align:right;width:200px;"/>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>描述：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="description" name="description" type="text" style="width:96%;" />
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="departmentSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDepartment').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
			    </div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--单位-->
    <script type="text/javascript">
        function OrganizationSelected(selectNode) {
            if (selectNode) {
                $('#organizationId').val(selectNode.id);
                $('#organizationName').val(selectNode.text);
                parentId = null;
                currentId = null;
                departmentReload();
            }
        }
    </script>
    <!--部门-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridDepartment').treegrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentList',
                //pagination: true,
                //pageSize: 30,
                loadMsg: '更新数据......',
                onClickRow: function (row) {
                    currentId=row.id;
                    parentId=row.parentId;
                    departmentUserReload();
                },
                onDblClickRow: function (row) {
                    <% if (ynWebRight.rightEdit){ %>
                    departmentEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $('#dataGridDepartment').treegrid('unselectAll');
                    if (currentId) {
                        $(this).treegrid('select', currentId);
                    }
                    departmentUserReload();
                },
				onBeforeLoad: function(row,param){
					if (!row) {	// load top level rows
                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentList?root=true';
                        $(this).treegrid("options").url = url;
						param.id = 0;	// set id=0, indicate to load new page rows
					}else{
                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentList?root=false';
                        $(this).treegrid("options").url = url;
                    }
				}
            });
        });
        function departmentReload() {
            if(parentId){
                $('#dataGridDepartment').treegrid('reload', parentId);
            }else{
                $('#dataGridDepartment').treegrid('reload');
            }
        }
        var currentId = null,parentId=null;
        function departmentAddRoot() {
            $('#editDepartment').window('open');
            $("#editDepartmentForm")[0].reset();

            $('#parentId').val(0);
            $('#name').focus();
            currentId = null;
            parentId=0;
        }
        function departmentAddChild() {
            var selectRow = $('#dataGridDepartment').treegrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请选中父部门！", "info");
                return;
            }
            $('#editDepartment').window('open');
            $("#editDepartmentForm")[0].reset();
            $('#parentId').val(selectRow.id);
            $('#name').focus();
            parentId=selectRow.id;
            currentId = null;
        }
        function departmentEdit() {
            var selectRow = $('#dataGridDepartment').treegrid('getSelected');
            if (selectRow) {
                $('#editDepartment').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $('#name').val(r.name);
                        $('#sortNo').val(r.sortNo);
                        $('#parentId').val(r.parentId);
                        $('#description').val(r.description);

                        $('#name').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择部门', 'info');
            }
        }
        function departmentSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editDepartmentForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentSave/' + currentId,
                        onSubmit: function () {
                            return $('#editDepartmentForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editDepartment').window('close');
                                if(rVal.entity.parentId)
                                {
                                    $('#dataGridDepartment').treegrid('update', {
                                        id: rVal.entity.parentId,
                                        row: {
                                            state: "closed"
                                        }
                                    });
                                }
                                currentId=rVal.id;
                                departmentReload()
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});

        }
        function departmentDelete() {
            var selectRow = $('#dataGridDepartment').treegrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除部门[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    currentId=null;
                                    departmentReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的部门', 'info');
            }
        }
    </script>
    <!--选择用户-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridDepartmentUser').datagrid({
                toolbar: [{
                    text: '刷新',
                    iconCls: 'icon-reload',
                    handler: function () {
                        departmentUserReload();
                    }
                }<% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %> , '-', {
                    text: '添加',
                    iconCls: 'icon-add',
                    handler: departmentAddUser
                }<%} %><% if (ynWebRight.rightDelete){ %> , '-', {
                    text: '移除',
                    iconCls: 'icon-cancel',
                    handler: departmentRemoveUser
                }<%} %>],
                loadMsg: '加载数据......'
            });
            $('#dataGridSelectUser').datagrid({
                pagination: true,
                loadMsg: '加载数据......'
            })
        })
        function departmentUserReload() {
            var departmentId = "";
            var title = '部门用户';
            var selectRow = $('#dataGridDepartment').treegrid('getSelected');
            if (selectRow) {
                departmentId = selectRow.id;
                title = '部门[<font color="red">' + selectRow.name + '</font>]用户';
            }
            $('#dataGridDepartmentUser').datagrid({
                title: title,
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentUserList/' + departmentId,
                onLoadSuccess: function () {
                    $('#dataGridDepartmentUser').datagrid('clearSelections');
                }
            });
        }
        function userSelectReload() {
            var selectRow = $('#dataGridDepartment').treegrid('getSelected');
			if(!selectRow) {
				return;
            }
            $('#dataGridSelectUser').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/SelectUserByDepartment/' + selectRow.id,
                onLoadSuccess: function () {
                    $('#dataGridSelectUser').datagrid('clearSelections');
                }
            });
        }
        function userSelectSearch() {
            var queryParams = $('#dataGridSelectUser').datagrid('options').queryParams;
            queryParams.queryWord = $('#userSelectSearch').val();
            $('#dataGridSelectUser').datagrid('reload');
            //重新载入后清除
            queryParams.queryWord = "";
        }
        function departmentAddUser() {
            var selectRow = $('#dataGridDepartment').treegrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请选择部门！", "info");
                return;
            }
            $('#userSelectSearch').val("");
            $('#userSelect').window('open');
            userSelectReload();
        }
        function userSelectOk() {
            var selectRows = $('#dataGridSelectUser').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert("提示", "请选择要加入的用户！", "info");
                return;
            }
            var userIds = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (userIds) {
                    userIds += ",";
                }
                userIds += selectRows[i].userId;
            }
            $.messager.confirm('确认', '确认添加选择用户到部门？', function (result) {
                if (result) {
                    var selectRow = $('#dataGridDepartment').treegrid('getSelected');
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentAddUser/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { userIds: userIds },
                        success: function (r) {
                            if (r.result) {
                                $('#userSelect').window('close');
                                departmentUserReload();
                            } else {
                                $.messager.alert('确认', '添加失败:' + r.message, 'error');
                            }
                        }

                    });
                }
            });
        }
        function departmentRemoveUser() {
            var selectRows = $('#dataGridDepartmentUser').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要移除的用户！', 'info');
                return;
            }
            var userIds = "";
            var userNames = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (userNames) {
                    userNames += ",";
                }
                userNames += selectRows[i].userName;
                if (userIds) {
                    userIds += ",";
                }
                userIds += selectRows[i].userId;
            }
            $.messager.confirm('确认', '确认从部门中移除用户[<font color="red">' + userNames + '</font>]?', function (result) {
                if (result) {
                    var selectRow = $('#dataGridDepartment').treegrid('getSelected');
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentRemoveUser/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { userIds: userIds },
                        success: function (r) {
                            if (r.result) {
                                departmentUserReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }

                    });
                }
            });
        }
    </script>
</asp:Content>
