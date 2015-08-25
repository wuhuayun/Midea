﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 部门岗位用户设置 -->
	<div region="west" split="false" border="false" title="" style="width:360px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="部门岗位" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" title="" border="false">
	            <div region="north" border="false" style="height:30px;overflow:hidden;">
                    <div style="float:left; height:26px; width:100%;background:#F4F4F4;border-bottom:1px solid #DDDDDD; padding:1px;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DepPositionReload();">刷新</a>
	                </div>
	            </div>
                <div region="center" border="false" style="background:#fff;">
                    <div class="easyui-panel" title="" data-options="fit:true,border:false">
	                    <ul id="treeDepPosition" class="easyui-tree" data-options="
			                    animate: true,
				                onBeforeLoad: function(node,param){
					                if (!node) {
                                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentTree?withPosition=1';
                                        $(this).tree('options').url = url;
                                        param.id = 0;
                                    }else{
                                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentTree?withPosition=1';
                                        $(this).tree('options').url = url;
                                    }
				                },
                                onSelect: function (node) {
                                    currentDepartmentId=null;
                                    currentPositionId=null;
                                    currentDepartmentName='';
                                    currentPositionName='';
                                    if (node.id > 1000) {
                                        currentPositionId = node.id-1000000;
                                        currentDepartmentId=null;
                                        var parentNode = $(this).tree('getParent',node.target);
                                        if(parentNode){
                                            currentDepartmentId=parentNode.id;
                                            currentDepartmentName=parentNode.text;
                                        }
                                        currentPositionName=node.attributes.name;
                                    }
                                    DepPositionUserReload();
                                }
		                    "></ul>
                    </div>
                </div>
			</div>
		</div>
	</div>
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <table id="dataGridDepPositionUser" title="部门岗位用户" style="" border="false" fit="true" checkOnSelect="false" selectOnCheck="false"  singleSelect="true"
			idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tbUser">
			<thead>
				<tr>
                    <th field="select" checkbox="true"></th>
					<th field="userId" width="150" align="left">用户账号</th>
					<th field="userName" width="250" align="left">用户名称</th>
					<th field="sex" width="50" align="center">性别</th>
					<th field="officeTel" width="100" align="left">办公电话</th>
					<th field="mobileTel" width="100" align="left">移动电话</th>
					<th field="accountLocked" width="80" align="left">账号锁定</th>
				</tr>
			</thead>
		</table>
        <div id="tbUser">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DepPositionUserReload();">刷新</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DepPositionAddUser();">添加关联用户</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DepPositionNewUser();">增加部门用户</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="DepPositionEditUser();">修改</a>
            <%} %>
            <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="userPassword();">设置口令</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="DepPositionRemoveUser();">移除关联用户</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DepPositionDeleteUser();">删除部门用户</a>
            <%} %>
        </div>
		<div id="editUser" class="easyui-window" title="用户" style="padding: 10px;width:640px;height:540px;"
		    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" id="editUserContent" border="true" style="padding:10px;background:#fff;">
		            <form id="editUserForm" method="post">
			            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>用户账号：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" required="true" id="userId" name="userId" type="text" style="width:96%;" /><span style="color:Red;">*</span>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>用户姓名：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" required="true" id="userName" name="userName" type="text" style="width:96%;" /><span style="color:Red;">*</span>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>性别：</span>
					            </td>
					            <td>
						            <select id="sex" name="sex" style="width:100px;">
							            <option value="男">男</option>
							            <option value="女">女</option>
						            </select>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>EMail：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="email" name="email" type="text" style="width:96%;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>办公电话：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="officeTel" name="officeTel" type="text" style="width:96%;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>移动电话：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="mobileTel" name="mobileTel" type="text" style="width:96%;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>账号锁定：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" type="checkbox" id="isAccountLocked" name="isAccountLocked" value=""/>
					            </td>
				            </tr>
				            <tr style="height:40px;border-top:1px solid #ccc;">
					            <td style="text-align:right;" nowrap>
					            </td>
					            <td style="text-align:right;">
                                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					                <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="DepPositionUserSave()">保存</a>
                                    <%} %>
					                <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editUser').window('close');">取消</a>
					            </td>
				            </tr>
			            </table>
		            </form>
				</div>
			</div>
		</div>
		<div id="editPassword" class="easyui-window" title="设置用户口令" style="padding: 10px;width:500px;height:300px;"
		    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" id="editPasswordContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
		            <form id="editPasswordForm" method="post" style="">
			            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>设置新的密码：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" required="true" id="newPassword1" name="newPassword1" type="password" style="width:96%;" />
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>重复新的密码：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" required="true" id="newPassword2" name="newPassword2" type="password" style="width:96%;" />
					            </td>
				            </tr>
			            </table>
		            </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="setUserPassword()">确认</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editPassword').window('close');">取消</a>
				</div>
			</div>
		</div>
        <div id="userSelect" class="easyui-window" title="选择用户" style="padding: 5px;width:540px;height:360px;"
			iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">                    
				<div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
					<table id="dataGridSelectUser" title="" style="" border="false" fit="true"
					idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tb1">
						<thead>
							<tr>
								<th field="select" checkbox="true"></th>
								<th field="userId" width="150" align="left">用户账号</th>
								<th field="userName" width="280" align="left">用户名称</th>
							</tr>
						</thead>
					</table>
                    <div id="tb1">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="UserSelectReload();">刷新</a>
                        <input id="userSelectSearch" type="text" style="width:120px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="UserSelectSearch();"></a>
                        <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                        <a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="UserSelectOk()">确认</a>
                        <%} %>
						<a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#userSelect').window('close');">取消</a> 
                    </div>
				</div>
			</div>
		</div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 表单类型 --%>
    <script type="text/javascript">
        var currentDepartmentId = null, currentPositionId = null;
        var currentDepartmentName = "", currentPositionName = "";
        function DepPositionReload() {
            $('#treeDepPosition').tree('reload');
        }
    </script>
    <script type="text/javascript">
        var currentUserId = null;
        $(function () {
            $('#dataGridDepPositionUser').datagrid({
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentUserId=rowData.userId;
                    //userRoleReload();
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearChecked');    
                    $(this).datagrid('clearSelections');
                    if (currentUserId) {
                        $(this).datagrid('selectRecord', currentUserId);
                    }
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    DepPositionEditUser();
                    <%} %>
                }
            });
            $('#dataGridSelectUser').datagrid({
                pagination: true,
                pageSize: 50,
                loadMsg: '加载数据......'
            })
        })
        function DepPositionUserReload() {
            var title = '部门岗位用户';
            if (currentDepartmentName != "" && currentPositionName!="") {
                title = '部门岗位[<font color="red">' + currentDepartmentName + ">" + currentPositionName + '</font>]用户';
            }
            $("#dataGridDepPositionUser").datagrid({
                title: title,
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/DepartmentPositionUserList/?departmentId=' + currentDepartmentId + '&positionId=' + currentPositionId
            })
        }
        function DepPositionAddUser() {
            if (!currentDepartmentId||!currentPositionId) {
                $.messager.alert("提示", "请选择部门岗位！", "info");
                return;
            }
            $('#userSelectSearch').val("");
            $('#userSelect').window('open');
            UserSelectReload();
        }
        function DepPositionNewUser() {
            if (!currentDepartmentId||!currentPositionId) {
                $.messager.alert("提示", "请选择部门岗位！", "info");
                return;
            }
            $('#editUser').window('open');
            $("#editUserForm")[0].reset();
            $('#userId').attr('readonly', false);
            $('#userId').focus();

            currentUserId = "";
        }
        function DepPositionEditUser() {
            var selectRow = $('#dataGridDepPositionUser').datagrid('getSelected');
            if (selectRow) {
                $('#editUser').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserEdit/' + encodeURIComponent(selectRow.userId);
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editUserForm")[0].reset();

                        $('#userId').val(r.userId);
                        $('#userId').attr('readonly', true);
                        $('#userName').val(r.userName);
                        $('#sex').val(r.sex);
                        $('#email').val(r.email);
                        $('#officeTel').val(r.officeTel);
                        $('#mobileTel').val(r.mobileTel);
                        $("#isAccountLocked").prop("checked", r.isAccountLocked);

                        $('#userName').focus();

                    }
                });
                currentUserId = selectRow.userId;
            } else {
                $.messager.alert('警告', '请选择用户', 'warning');
            }
        }
        function DepPositionUserSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editUserForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserSave/' + encodeURIComponent(currentUserId)+'?departmentId=' + currentDepartmentId + '&positionId=' + currentPositionId,
                        onSubmit: function () {
                            $("#isAccountLocked").val($("#isAccountLocked").prop("checked"));
                            return $('#editUserForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editUser').window('close');
                                currentUserId = rVal.id;
                                DepPositionUserReload();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
                }
            })

        }
        function DepPositionDeleteUser() {
            var selectRows = $('#dataGridDepPositionUser').datagrid('getChecked');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要删除的用户', 'info');
                return;
            }
            var userIds = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (userIds) {
                    userIds += ",";
                }
                userIds +=encodeURIComponent(selectRows[i].userId);
            }
            $.messager.confirm('确认', '确认删除用户[<font color="red">' + userIds + '</font>]？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserDelete/';// + encodeURIComponent(selectRow.userId);
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { userIds: userIds },
                        success: function (r) {
                            if (r.result) {
                                currentUserId="";
                                DepPositionUserReload();
                            } else {
                                $.messager.alert('确认', '删除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function UserSelectReload() {
            if (!currentDepartmentId||!currentPositionId) {
				return;
            }
            $('#dataGridSelectUser').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/SelectUserByDepartmentPosition/?departmentId=' + currentDepartmentId + '&positionId=' + currentPositionId,
                onLoadSuccess: function () {
                    $('#dataGridSelectUser').datagrid('clearSelections');
                }
            });
        }
        function UserSelectSearch() {
            var queryParams = $('#dataGridSelectUser').datagrid('options').queryParams;
            queryParams.queryWord = $('#userSelectSearch').val();
            $('#dataGridSelectUser').datagrid('reload');
            //重新载入后清除
            queryParams.queryWord = "";
        }
        function UserSelectOk() {
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
            $.messager.confirm('确认', '确认添加选择用户到部门岗位？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/DepartmentPositionAddUser?departmentId=' + currentDepartmentId + '&positionId=' + currentPositionId;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { userIds: userIds },
                        success: function (r) {
                            if (r.result) {
                                $('#userSelect').window('close');
                                DepPositionUserReload();
                            } else {
                                $.messager.alert('确认', '添加失败:' + r.message, 'error');
                            }
                        }

                    });
                }
            });
        }
        function DepPositionRemoveUser() {
            var selectRows = $('#dataGridDepPositionUser').datagrid('getChecked');
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
            $.messager.confirm('确认', '确认从角色中移除用户[<font color="red">' + userNames + '</font>]?', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/DepartmentPositionRemoveUser?departmentId=' + currentDepartmentId + '&positionId=' + currentPositionId;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { userIds: userIds },
                        success: function (r) {
                            if (r.result) {
                                DepPositionUserReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }

                    });
                }
            });
        }
		function userPassword(){
			var selectRow = $('#dataGridDepPositionUser').datagrid('getSelected');
			if (selectRow) {
			    $('#editPassword').window('open');
			    $('#editPasswordForm')[0].reset();
			}else{
				$.messager.alert('提示', '请选择要设置口令的用户', 'info');
			}
		}
		function setUserPassword() {
		    var newPassword1 = $('#newPassword1').val();
		    var newPassword2 = $('#newPassword2').val();
		    if (newPassword1 != newPassword2) {
			    $.messager.alert('错误', '两次输入的新密码不一致，请确认!', 'error');
				return;
            }
            var selectRow = $('#dataGridDepPositionUser').datagrid('getSelected');
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/SetUserPassword/' + encodeURIComponent(selectRow.userId),
                type: 'post',
                dataType: "json",
                data: { password: newPassword1 },
                beforeSend: function () {
                    return $("#editPasswordForm").form('validate');
                },
                success: function (r) {
                    if (r.result) {
                        $('#editPassword').window('close');
                    }else{
                        $.messager.alert('确认', '设置口令失败:' + r.message, 'error');
                    }
                }
            })
		}
    </script>
</asp:Content>
