<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	美的员工基本信息管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridEmployee" title="美的员工基本信息管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <input id="employeeSearch" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
            <%} %>
        </div>
		<div id="editEmployee" class="easyui-window" title="员工信息修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editEmployeeForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>员工编号：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" required="true" id="docNumber" name="docNumber" type="text" style="width:120px;" /><span style="color:Red;">*</span>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>员工姓名：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:120px;" /><span style="color:Red;">*</span>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>性别：</span>
					            </td>
					            <td>
						            <select id="sex" name="sex" style="width:124px;">
							            <option value="男">男</option>
							            <option value="女">女</option>
						            </select>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>身份证号：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="idNumber" name="idNumber" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>EMail：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="email" name="email" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>办公电话：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="officeTel" name="officeTel" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>移动电话：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="mobileTel" name="mobileTel" type="text" style="width:120px;"/>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>部门：</span>
					            </td>
					            <td style="width:80%">
						            <input class="easyui-validatebox" id="departmentName" name="departmentName" type="text" style="width:120px; background-color:#CCCCCC;" readonly="readonly"/>
							        <input class="easyui-validatebox" id="departmentId" name="departmentId" type="text" style="width:10px;display:none"/>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择部门" onclick="SelectDepartment()"></a>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-remove" title="移除部门" onclick="RemoveDepartment()"></a>
					            </td>
				            </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>备注：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="memo" name="memo" type="text" style="width:300px;" />
						        </td>
					        </tr>
				        </table>
			        </form>
                    <%-- 选择部门 --%>
                    <%Html.RenderPartial("~/Areas/YnPublic/Views/YnDepartment/DepartmentSelect.ascx"); %>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editEmployee').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridEmployee').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '员工编号', width: 80, align: 'center' },
                ]],
                columns: [[
					{ field: 'name', title: '姓名', width: 80, align: 'center' },
					{ field: 'sex', title: '性别', width: 50, align: 'center' },
					{ field: 'idNumber', title: '身份证号', width: 100, align: 'center' },
					{ field: 'email', title: 'EMail', width: 200, align: 'center' },
					{ field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
					{ field: 'mobileTel', title: '移动电话', width: 100, align: 'center' },
                    { field: 'departmentName', title: '部门', width: 100, align: 'left' },
                    { field: '_startTime', title: '开始时间', width: 90, align: 'left' },
                    { field: '_birth', title: '生日', width: 80, align: 'left' },
                    { field: 'memo', title: '备注', width: 200, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    Edit();
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
        function Reload() {
            $('#dataGridEmployee').datagrid('reload');
        }
		function Query(){
			var queryParams = $('#dataGridEmployee').datagrid('options').queryParams;
			queryParams.queryWord = $('#employeeSearch').val();
			$('#dataGridEmployee').datagrid('reload');
		}
        var currentId = null;
        function Add() {
            $('#editEmployee').window('open');
            $("#editEmployeeForm")[0].reset();

            $('#docNumber').focus();
            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dataGridEmployee').datagrid('getSelected');
            if (selectRow) {
                $('#editEmployee').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editEmployeeForm")[0].reset();

				        $('#docNumber').val(r.docNumber);
                        $('#name').val(r.name);
				        $('#sex').val(r.sex);
                        $('#idNumber').val(r.idNumber);
				        $('#email').val(r.email);
				        $('#officeTel').val(r.officeTel);
				        $('#mobileTel').val(r.mobileTel);
                        $('#memo').val(r.memo);
                        $('#departmentId').val(r.departmentId);
                        $('#departmentName').val(r.departmentName);

                        $('#docNumber').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择员工', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editEmployeeForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeSave/' + currentId,
                        onSubmit: function () {
                            return $('#editEmployeeForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editEmployee').window('close');
                                currentId = rVal.id;
                                Reload();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
        function Delete() {
            var selectRow = $('#dataGridEmployee').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除员工[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    Reload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的员工', 'info');
            }
        }
        function DepartmentSelected(selectNode) {
            if (selectNode) {
                $('#departmentId').val(selectNode.id);
                //$('#departmentName').val(selectNode.attributes.name);
                $('#departmentName').val(selectNode.text);
            }
        }
		function RemoveDepartment(){
			$('#departmentId').val("");
			$('#departmentName').val("");
		}
    </script>
</asp:Content>
