<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商车辆基本信息管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridVehicle" title="供应商车辆基本信息管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
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
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">送货日志</a>
        </div>
		<div id="editVehicle" class="easyui-window" title="司机修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editVehicleForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>车牌号：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="Text4" name="startId" type="text" style="width:100px;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>类型：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="Text1" name="startId" type="text" style="width:100px;" />(自有/第三方)
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>单位：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="Text2" name="startId" type="text" style="width:100px;" />
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>车主：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="Text3" name="startId" type="text" style="width:100px;" />
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>规格：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="Text5" name="startId" type="text" style="width:100px;" />
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>描述：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="Text6" name="startId" type="text" style="width:100px;" />
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>备注：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="description" name="description" type="text" style="width:300px;" />
						        </td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editVehicle').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridVehicle').datagrid({
                url: '',
//                pagination: true,
//                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '车牌号', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '类型(自有/第三方)', width: 150, align: 'center' },
                    { field: 'name', title: '单位', width: 60, align: 'center' },
                    { field: 'name', title: '车主', width: 60, align: 'center' },
                    { field: 'name', title: '联系方式', width: 60, align: 'center' },
                    { field: 'name', title: '规格', width: 60, align: 'center' },
                    { field: 'name', title: '描述', width: 60, align: 'center' },
                    { field: 'name', title: '备注', width: 200, align: 'left' }
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
            $('#dataGridVehicle').datagrid('reload');
        }
        var currentId = null;
        function Add() {
            $('#editVehicle').window('open');
            $("#editVehicleForm")[0].reset();

            $('#name').focus();
            currentId = "";
        }
        function Edit() {
//            var selectRow = $('#dataGridVehicle').datagrid('getSelected');
//            if (selectRow) {
                $('#editVehicle').window('open');
                var sUrl = '';
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editVehicleForm")[0].reset();

                        $('#name').val(r.name);
                        $('#sortNo').val(r.sortNo);
                        $('#description').val(r.description);

                        $('#name').focus();
                    }
                });
//                currentId = selectRow.id;
//            } else {
//                $.messager.alert('提示', '请选择供应商', 'info');
//            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editVehicleForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/' + currentId,
                        onSubmit: function () {
                            return $('#editVehicleForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editVehicle').window('close');
                                currentId = selectRow.id;
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
            var selectRow = $('#dataGridRole').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除司机规格[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/' + selectRow.id;
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
                $.messager.alert('提示', '请选择要删除的司机规格', 'info');
            }
        }
    </script>
</asp:Content>
