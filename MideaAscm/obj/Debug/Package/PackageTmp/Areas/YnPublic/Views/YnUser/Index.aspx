<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnUser>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	YnUser
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--用户-->   
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="用户管理 " fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
			        <table id="dataGridUser" title="" style="" border="false" fit="true" singleSelect="true"
						idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tb2">
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="userReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="userAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="userEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="userPassword();">设置口令</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="userDelete();">删除</a>
                        <%} %>
			            <input id="userSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="userSearch();"></a>
                    </div>
			    </div>
			</div>
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
					                <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="userSave()">保存</a>
                                    <%} %>
					                <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editUser').window('close');set_form_oldvalue();">取消</a>
					            </td>
				            </tr>
			            </table>
		            </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:240px;line-height:30px;padding:2px 0px 0px 0px;">
			        <div class="easyui-layout" fit="true" id="divUserExtInfo">
                        <div region="center" split="false" border="false" title="" style="padding:0px 0px 0px 0px;overflow:auto;">
                            <table id="dataGridUserDepartmentPosition" title="用户部门岗位" style="" border="true" fit="true" 
				                 sortName="id" sortOrder="asc" striped="true" toolbar="#tbUserDepartmentPosition">
				                <thead>
					                <tr>
                                        <th field="select" checkbox="true"></th>
						                <th field="id" width="50" align="center" hidden="true">ID</th>
						                <th field="departmentName" width="150" align="left">部门名称</th>
                                        <th field="positionName" width="150" align="left">岗位名称</th>
                                        <th field="roleName" width="150" align="left">岗位归属角色</th>
					                </tr>
				                </thead>
			                </table>
                            <div id="tbUserDepartmentPosition">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                                <a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-add"  onclick="UserAddDepartmentPosition();">添加</a>
                                <%} %>
								<a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="UserRemoveDepartmentPosition();">移除</a> 
                            </div>
                            <%Html.RenderPartial("~/Areas/YnPublic/Views/YnPosition/DepartmentPositionSelect.ascx");%>
                        </div>
                    </div>
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
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--离开页面时,检测表单元素是否被修改-->
    <script type="text/javascript">
        function onBeforeClose() {
            if (is_form_changed()) {
                //return "您的修改内容还没有保存，您确定离开吗？";
                return confirm("您的修改内容还没有保存，您确定离开吗？");
            }
        }
        function is_form_changed() {
            var is_changed = false;
            jQuery("#editUserForm input, #editUserForm textarea, #editUserForm select").each(function () {
                var _v = jQuery(this).attr('_value');
                if (typeof (_v) == 'undefined') _v = '';
                if (_v != jQuery(this).val()) { is_changed = true; }
            });
            return is_changed;
        }
        function set_form_oldvalue() {
            jQuery("#editUserForm input, #editUserForm textarea, #editUserForm select").each(function () {
                jQuery(this).attr('_value', jQuery(this).val());
            });
        }
        $(function () {
            set_form_oldvalue();
        });
    </script>
    <!--用户-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridUser').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserList',
                frozenColumns: [[
	                { field: 'userId', title: '用户账号', width: 120, sortable: true, align: 'left' }
				]],
                columns: [[
					{ field: 'userName', title: '姓名', width: 180, align: 'left' },
					{ field: 'sex', title: '性别', width: 50, align: 'center' },
					{ field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
					{ field: 'mobileTel', title: '移动电话', width: 100, align: 'center' },
					{ field: 'accountLocked', title: '账号锁定', width: 60, align: 'center' },
					{ field: 'departmentPositionList', title: '部门岗位', width: 300, align: 'left' },
                    { field: 'roleList', title: '角色', width: 200, align: 'left' }
				]],
                pagination: true,
                pageSize: 30,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.userId;
                    //userRoleReload();
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    userEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $('#dataGridUser').datagrid('clearSelections');
                    //userRoleReload();
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function userReload() {
            //$('#dataGridUser').datagrid('reload');
            $('#dataGridUser').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserList'
            })
            //$('#dataGridUser').datagrid({
            //    onLoadSuccess: function () {
            //        $('#dataGridUser').datagrid('clearSelections');
            //        userRoleReload();
            //    }
            //})
        }
		function userSearch(){
			var queryParams = $('#dataGridUser').datagrid('options').queryParams;
			queryParams.queryWord = $('#userSearch').val();
			$('#dataGridUser').datagrid('reload');
		}
		var currentId = null;
		function userAdd(){
			$('#editUser').window('open');
			$("#editUserForm")[0].reset();
			$('#userId').attr('readonly', false);
			$('#userId').focus();

            $("#divUserExtInfo").hide();
            currentId = "";
		}
		function userEdit(){
			var selectRow = $('#dataGridUser').datagrid('getSelected');
			if(selectRow){
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
                        //$('#departmentId').val(r.departmentId);

                        set_form_oldvalue();
				        $('#userName').focus();

                        $("#divUserExtInfo").show();
                        UserDepartmentPositionReload();
				    }
				});
	            currentId = selectRow.userId;
			}else{
				$.messager.alert('警告', '请选择用户', 'warning');
			}
		}
		function userSave(){
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
		            $('#editUserForm').form('submit', {
		                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserSave/' + encodeURIComponent(currentId),
		                onSubmit: function () {
		                    $("#isAccountLocked").val($("#isAccountLocked").prop("checked"));
		                    return $('#editUserForm').form('validate');
		                },
		                success: function (r) {
		                    var rVal = $.parseJSON(r);
		                    if (rVal.result) {
		                        $('#editUser').window('close');
		                        $('#dataGridUser').datagrid({
		                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserList',
		                            onLoadSuccess: function () {
		                                $('#dataGridUser').datagrid('clearSelections');
		                                $('#dataGridUser').datagrid('selectRecord', rVal.id);
		                            }
		                        });
		                    } else {
		                        $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
		                    }
		                }
		            });
                }
            })

        }
        function userDelete() {
            var selectRow = $('#dataGridUser').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除用户[<font color="red">' + selectRow.userName + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserDelete/';// + encodeURIComponent(selectRow.userId);
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data:{userIds:selectRow.userId},
                            success: function (r) {
                                if (r.result) {
                                    currentId=null;
                                    userReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的用户', 'info');
            }
        }
		function userPassword(){
			var selectRow = $('#dataGridUser').datagrid('getSelected');
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
            var selectRow = $('#dataGridUser').datagrid('getSelected');
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
    <!--部门岗位-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridUserDepartmentPosition').datagrid({
                loadMsg: '加载数据......'
            });
            $('#dataGridSelectUser').datagrid({
                pagination: true,
                loadMsg: '加载数据......'
            })
        })
        function UserDepartmentPositionReload() {
            $('#dataGridUserDepartmentPosition').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserDepartmentPositionList/' + encodeURIComponent(currentId) + "?t=" + new Date().getTime(),
                onLoadSuccess: function () {
                    $('#dataGridUserDepartmentPosition').datagrid('clearSelections');
                }
            });
        }
        function UserAddDepartmentPosition() {
            SelectDepartmentPosition();
            //$('#departmentSelect').window('open');
            //departmentSelectReload();
        }
        function DepartmentPositionSelected(currentDepartmentId, currentPositionId) {
            if (currentDepartmentId && currentPositionId) {
                $.messager.confirm('确认', '确认添加部门岗位到用户？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserAddDepartmentPosition/' + encodeURIComponent(currentId) + "?t=" + new Date().getTime();
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data: { departmentId: currentDepartmentId, positionId: currentPositionId },
                            success: function (r) {
                                if (r.result) {
                                    UserDepartmentPositionReload();
                                } else {
                                    $.messager.alert('确认', '添加失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function UserRemoveDepartmentPosition() {
            var selectRows = $('#dataGridUserDepartmentPosition').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要移除的部门岗位！', 'info');
                return;
            }
            var departmentPositionIds = "";
            var names = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (names) {
                    names += ",";
                }
                names += selectRows[i].departmentName + ">" + selectRows[i].positionName;
                if (departmentPositionIds) {
                    departmentPositionIds += ",";
                }
                departmentPositionIds += selectRows[i].ynDepartment.id + "_" + selectRows[i].ynPosition.id;
            }
            $.messager.confirm('确认', '确认从用户中移除部门岗位[<font color="red">' + names + '</font>]?', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserRemoveDepartmentPosition/' + encodeURIComponent(currentId);
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { departmentPositionIds: departmentPositionIds },
                        success: function (r) {
                            if (r.result) {
                                UserDepartmentPositionReload();
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
