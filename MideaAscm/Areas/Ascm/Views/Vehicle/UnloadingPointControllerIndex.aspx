<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	卸货点控制器管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgUnloadingPointController" title="卸货点控制器管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <input id="search" type="text" style="width:100px;" />
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
		<div id="editUnloadingPointController" class="easyui-window" title="控制器修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editUnloadingPointControllerForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>控制器名称：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="name" name="name" required="true" type="text" style="width:200px;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>IP地址：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="ip" name="ip" type="text" style="width:200px;" />
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>端口：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="port" name="port" type="text" validType="number" style="width:140px;"/>
						        </td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editUnloadingPointController').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dgUnloadingPointController').datagrid({
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointControllerList',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'name', title: '控制器名称', width: 220, align: 'left' }
                ]],
                columns: [[
                    { field: 'ip', title: 'IP地址', width: 180, align: 'center' },
                    { field: 'port', title: '端口', width: 70, align: 'center' }
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
        function Query() {
            var options = $('#dgUnloadingPointController').datagrid('options');
            options.queryParams.queryWord = $('#search').val();
            $('#dgUnloadingPointController').datagrid('reload');
        }
        var currentId = null;
        function Add() {
            $('#editUnloadingPointController').window('open');
            $("#editUnloadingPointControllerForm")[0].reset();

            $('#name').focus();
            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dgUnloadingPointController').datagrid('getSelected');
            if (selectRow) {
                $('#editUnloadingPointController').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointControllerEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editUnloadingPointControllerForm")[0].reset();
                            $('#name').val(r.name);
                            $('#ip').val(r.ip);
                            $('#port').val(r.port);

                            $('#name').focus();
                        }
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择卸货点', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editUnloadingPointControllerForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointControllerSave/' + currentId,
                        onSubmit: function () {
                            return $('#editUnloadingPointControllerForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editUnloadingPointController').window('close');
                                currentId = rVal.id;
                                Query();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
        function Delete() {
            var selectRow = $('#dgUnloadingPointController').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除卸货点[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointControllerDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    Query();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除卸货点', 'info');
            }
        }
    </script>
</asp:Content>
