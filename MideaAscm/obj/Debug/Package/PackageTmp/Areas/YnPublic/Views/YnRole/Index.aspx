<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnRole>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--角色-->
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="角色管理 " fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
			        <table id="dataGridRole" title="" style="" border="false" fit="true" singleSelect="true"
					idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb2">
                        <thead>
				            <tr>
					            <th field="id" width="50" align="center">ID</th>
					            <th field="name" width="120" align="left">角色名称</th>
                                <th field="description" width="300" align="left">描述</th>
                                <th field="sortNo" width="50" align="center" sortable="true">序号</th>
				            </tr>
			            </thead>
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="roleReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="roleAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="roleEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="roleDelete();">删除</a>
                        <%} %>
			            <input id="userSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="roleSearch();"></a>
                    </div>
			        <div id="editRole" class="easyui-window" title="角色" style="padding: 10px;width:500px;height:300px;"
			            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
					        <div region="center" id="editRoleContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                                <form id="editRoleForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>角色名称：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:96%;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>					
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>序号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" validType="number" style="text-align:right"/>
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
						        <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="roleSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editRole').window('close');">取消</a>
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
    <!--角色-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridRole').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleList',
                //pagination: true,
                //pageSize: 30,
                loadMsg: '更新数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    roleEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $('#dataGridRole').datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function roleReload() {
            $('#dataGridRole').datagrid('reload');
        }
        function roleSearch() {
            var queryParams = $('#dataGridRole').datagrid('options').queryParams;
            queryParams.queryWord = $('#roleSearch').val();
            $('#dataGridRole').datagrid('reload');
        }
        var currentId = null;
        function roleAdd() {
            $('#editRole').window('open');
            $("#editRoleForm")[0].reset();

            $('#name').focus();
            currentId = "";
        }
        function roleEdit() {
            var selectRow = $('#dataGridRole').datagrid('getSelected');
            if (selectRow) {
                $('#editRole').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editRoleForm")[0].reset();

                        $('#name').val(r.name);
                        $('#sortNo').val(r.sortNo);
                        $('#description').val(r.description);

                        $('#name').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择角色', 'info');
            }
        }
        function roleSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editRoleForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleSave/' + currentId,
                        onSubmit: function () {
                            return $('#editRoleForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editRole').window('close');
                                $('#dataGridRole').datagrid({
                                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleList',
                                    onLoadSuccess: function () {
                                        $('#dataGridRole').datagrid('clearSelections');
                                        $('#dataGridRole').datagrid('selectRecord', rVal.id);
                                    }
                                });
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
        function roleDelete() {
            var selectRow = $('#dataGridRole').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除角色[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnRole/RoleDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    roleReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的角色', 'info');
            }
        }
    </script>
</asp:Content>
