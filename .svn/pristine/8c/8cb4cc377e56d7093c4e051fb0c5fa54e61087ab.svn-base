<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓管人员管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--用户-->   
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="仓管人员管理 " fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
			        <table id="dataGridWarehouseUserInfo" title="" style="" border="false" fit="true" singleSelect="true"
						idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tb2">
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="warehouseUserReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="warehouseUserAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="warehouseUserEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="warehouseUserPassword();">设置口令</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="warehouseUserDelete();">删除</a>
                        <%} %>
                    </div>
			    </div>
			</div>
		</div>
		<div id="editUser" class="easyui-window" title="用户" style="padding: 10px;width:640px;height:420px;"
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
					                <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="warehouseUserSave()">保存</a>
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
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="setWarehouseUserPassword()">确认</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editPassword').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<script type="text/javascript">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
        $(function () {
            $('#dataGridWarehouseUserInfo').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseUserList',
                frozenColumns: [[
	                { field: 'userId', title: '用户账号', width: 120, sortable: true, align: 'left' }
				]],
                columns: [[
					{ field: 'userName', title: '姓名', width: 180, align: 'left' },
					{ field: 'sex', title: '性别', width: 50, align: 'center' },
                    { field: 'email', title: 'EMail', width: 100, align: 'center' },
					{ field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
					{ field: 'mobileTel', title: '移动电话', width: 100, align: 'center' },
					{ field: 'accountLocked', title: '账号锁定', width: 60, align: 'center' }
				]],
                rownumbers:true,
                pagination: true,
                pageSize: 50,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.userId;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    warehouseUserEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $('#dataGridWarehouseUserInfo').datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function warehouseUserReload() {
            $('#dataGridWarehouseUserInfo').datagrid('reload');
        }
		var currentId = null;
		function warehouseUserAdd(){
			$('#editUser').window('open');
			$("#editUserForm")[0].reset();
			$('#userId').attr('readonly', false);
			$('#userId').focus();

//            $("#divUserExtInfo").hide();
            currentId = "";
		}
		function warehouseUserEdit(){
			var selectRow = $('#dataGridWarehouseUserInfo').datagrid('getSelected');
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
				        $('#userName').focus();

//                        $("#divUserExtInfo").show();
				    }
				});
	            currentId = selectRow.userId;
			}else{
				$.messager.alert('警告', '请选择用户', 'warning');
			}
		}
		function warehouseUserSave(){
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
		            $('#editUserForm').form('submit', {
		                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseUserInfoSave/' + encodeURIComponent(currentId),
		                onSubmit: function () {
		                    $("#isAccountLocked").val($("#isAccountLocked").prop("checked"));
		                    return $('#editUserForm').form('validate');
		                },
		                success: function (r) {
		                    var rVal = $.parseJSON(r);
		                    if (rVal.result) {
		                        $('#editUser').window('close');
		                        $('#dataGridWarehouseUserInfo').datagrid({
		                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseUserList',
		                            onLoadSuccess: function () {
		                                $('#dataGridWarehouseUserInfo').datagrid('clearSelections');
		                                $('#dataGridWarehouseUserInfo').datagrid('selectRecord', rVal.id);
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
        function warehouseUserDelete() {
            var selectRow = $('#dataGridWarehouseUserInfo').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除仓管员[<font color="red">' + selectRow.userName + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnUser/UserDelete/' + encodeURIComponent(selectRow.userId);
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
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
                $.messager.alert('提示', '请选择要删除的仓管员', 'info');
            }
        }
		function warehouseUserPassword(){
			var selectRow = $('#dataGridWarehouseUserInfo').datagrid('getSelected');
			if (selectRow) {
			    $('#editPassword').window('open');
			    $('#editPasswordForm')[0].reset();
			}else{
				$.messager.alert('提示', '请选择要设置口令的仓管员', 'info');
			}
		}
		function setWarehouseUserPassword() {
		    var newPassword1 = $('#newPassword1').val();
		    var newPassword2 = $('#newPassword2').val();
		    if (newPassword1 != newPassword2) {
			    $.messager.alert('错误', '两次输入的新密码不一致，请确认!', 'error');
				return;
            }
            var selectRow = $('#dataGridWarehouseUserInfo').datagrid('getSelected');
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
