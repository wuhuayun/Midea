<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商用户信息管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridUser" title="供应商用户信息管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <input id="userSearch" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="UserQuery();">查询</a>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="UserEdit();">修改</a>
            <%} %>
        </div>
		<div id="editUser" class="easyui-window" title="用户" style="padding: 10px;width:640px;height:540px;"
		    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="north" id="editUserContent" border="true" style="height:70px;padding:10px;background:#fff;">
		            <form id="editUserForm" method="post">
			            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>用户账号：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" readonly="readonly" id="userId" name="userId" type="text" style="width:96%;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>用户姓名：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" readonly="readonly" id="userName" name="userName" type="text" style="width:96%;"/>
					            </td>
				            </tr>
			            </table>
		            </form>
				</div>
				<div region="center" border="false" style="text-align:right;line-height:30px;padding:2px 0px 0px 0px;">
			        <div class="easyui-layout" fit="true" id="divUserExtInfo">
                        <div region="center" split="false" border="false" title="" style="padding:0px 0px 0px 0px;overflow:auto;">
                            <table id="dataGridUserDepartmentPosition" title="用户部门岗位" style="" border="true" fit="true" 
				                idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tbUserDepartmentPosition">
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

	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridUser').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/SupplierUserInfoList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'userId', title: '用户编号', width: 120, align: 'left' },
                ]],
                columns: [[
					{ field: 'userName', title: '用户名称', width: 150, align: 'left' },
					{ field: 'email', title: '电子邮件', width: 200, align: 'left' },
                    { field: 'description', title: '描述', width: 200, align: 'left' },
					{ field: 'departmentPositionList', title: '部门岗位', width: 300, align: 'left' },
                    { field: 'roleList', title: '角色', width: 200, align: 'left' },
                    { field: 'supplierName', title: '供应商名称', width: 200, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    UserEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function UserReload() {
            $('#dataGridUser').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/SupplierUserInfoList'
            })
        }
		function UserQuery(){
			var queryParams = $('#dataGridUser').datagrid('options').queryParams;
			queryParams.queryWord = $('#userSearch').val();
			$('#dataGridUser').datagrid('reload');
		}
		var currentId = null;
		function UserEdit() {
		    var selectRow = $('#dataGridUser').datagrid('getSelected');
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
		                $('#userName').val(r.userName);

		                $("#divUserExtInfo").show();
		                UserDepartmentPositionReload();
		            }
		        });
		        currentId = selectRow.userId;
		    } else {
		        $.messager.alert('警告', '请选择用户', 'warning');
		    }
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
