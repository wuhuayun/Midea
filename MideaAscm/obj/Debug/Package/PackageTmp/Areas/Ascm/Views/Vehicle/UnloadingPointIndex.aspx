<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	卸货点管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgUnloadingPoint" title="卸货点管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="warehouseId,id" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库选择：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWarehouseSelect" }); %>
            <span>状态：</span>
            <select id="queryStatus" name="queryStatus" style="width:80px;">
                <option value=""></option>
                <% List<string> listStatusDefine = MideaAscm.Dal.Vehicle.Entities.AscmUnloadingPoint.StatusDefine.GetList(); %>
                <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in listStatusDefine)
                    { %>
                <option value="<%=statusDefine %>"><%=MideaAscm.Dal.Vehicle.Entities.AscmUnloadingPoint.StatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>
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
		<div id="editUnloadingPoint" class="easyui-window" title="卸货点修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editUnloadingPointForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>卸货点名称：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="name" name="name" required="true" type="text" style="width:200px;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>仓库名称：</span>
						        </td>
						        <td style="width:80%">
                                    <input class="easyui-validatebox" id="warehouseDescription" name="warehouseDescription" required="true" type="text" style="width:200px;background-color:#CCCCCC;" readonly="readonly"/><span style="color:Red;">*</span>
							        <input id="warehouseId" name="warehouseId" type="hidden"/>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择仓库" onclick="SelectWarehouse();"></a>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-remove" title="移除仓库" onclick="RemoveWarehouse();"></a>
						        </td>
					        </tr>
					        <%--<tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>方向：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="direction" name="direction" type="text" style="width:160px;" />
						        </td>
					        </tr>--%>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>状态：</span>
						        </td>
						        <td style="width:80%">
							        <select id="status" name="status" style="width:205px;">
                                        <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                                            { %>
                                        <% foreach (string statusDefine in listStatusDefine)
                                            { %>
                                        <option value="<%=statusDefine %>"><%=MideaAscm.Dal.Vehicle.Entities.AscmUnloadingPoint.StatusDefine.DisplayText(statusDefine)%></option>
                                        <% } %>
                                        <% } %>
                                    </select>
						        </td>
					        </tr>
                            <%--<tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>IP地址：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="ip" name="ip" type="text" style="width:200px;" />
						        </td>
					        </tr>--%>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>启用：</span>
						        </td>
						        <td style="width:80%">
							        <input type="checkbox" id="enabledYes" name="enabled" value="是"/>是
                                    <input type="checkbox" id="enabledNo" name="enabled" value="否"/>否
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>位置：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="location" name="location" type="text" style="width:200px;" />
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>控制器：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="controllerName" name="controllerName" required="true" type="text" style="width:200px;background-color:#CCCCCC;" readonly="readonly"/>
							        <input id="controllerId" name="controllerId" type="hidden"/>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择控制器" onclick="SelectUnloadingPointController();"></a>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-remove" title="移除控制器" onclick="RemoveUnloadingPointController();"></a>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>控制器地址：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-numberbox" id="controllerAddress" name="controllerAddress" type="text" data-options="min:0,max:7" style="width:200px;" />
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>描述：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="description" name="description" type="text" style="width:300px;" />
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
                    <%-- 选择仓库 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelect.ascx"); %>
                    <%-- 选择控制器 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Vehicle/UnloadingPointControllerSelect.ascx"); %>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editUnloadingPoint').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var preEnabled = null;
        $(function () {
            $('#dgUnloadingPoint').datagrid({
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointList',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'name', title: '卸货点名称', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'warehouseDescription', title: '仓库名称', width: 150, align: 'left' },
                    { field: 'controllerName', title: '控制器名称', width: 150, align: 'left' },
                    { field: 'controllerAddress', title: '控制器地址', width: 150, align: 'left' },
                    { field: 'statusCn', title: '状态', width: 80, align: 'center' },
                    //{ field: 'ip', title: 'IP地址', width: 120, align: 'center' },
                    { field: 'enabledCn', title: '启用', width: 70, align: 'center' },
                    //{ field: 'direction', title: '方向', width: 80, align: 'center' },
                    { field: 'location', title: '位置', width: 100, align: 'center' },
                    { field: 'description', title: '描述', width: 150, align: 'left' },
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
            $("[name='enabled']").each(function () {
                $(this).click(function () {
                    if ($(this).is(':checked')) {
                        if (preEnabled != null) {
                            preEnabled.removeAttr("checked"); 
                        }
                        preEnabled = $(this);
                        preEnabled.attr("checked", "true");
                    }
                })
            })
        });
        function Query() {
            var options = $('#dgUnloadingPoint').datagrid('options');
            options.queryParams.status = $('#queryStatus').val();
            options.queryParams.warehouseId = $('#queryWarehouseSelect').combogrid('getValue');
            options.queryParams.queryWord = $('#search').val();
            $('#dgUnloadingPoint').datagrid('reload');
        }
        function WarehouseSelected(r) {
            if (r) {
                $('#warehouseDescription').val(r.description);
                $('#warehouseId').val(r.id);
            }
        }
        function RemoveWarehouse(){
            $('#warehouseDescription').val("");
            $('#warehouseId').val("");
        }
        function UnloadingPointControllerSelected(r) {
            if (r) {
                $('#controllerName').val(r.name);
                $('#controllerId').val(r.id);
            }
        }
        function RemoveUnloadingPointController(){
            $('#controllerName').val("");
            $('#controllerId').val("");
        }
        var currentId = null;
        function Add() {
            $('#editUnloadingPoint').window('open');
            $("#editUnloadingPointForm")[0].reset();

            $('#name').focus();
            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dgUnloadingPoint').datagrid('getSelected');
            if (selectRow) {
                $('#editUnloadingPoint').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editUnloadingPointForm")[0].reset();
                            $('#name').val(r.name);
                            $('#warehouseId').val(r.warehouseId);
                            $('#warehouseDescription').val(r.warehouseDescription);
                            $('#controllerId').val(r.controllerId);
                            $('#controllerName').val(r.controllerName);
                            $('#controllerAddress').numberbox('setValue', r.controllerAddress);
                            $('#ip').val(r.ip);
                            //$('#direction').val(r.direction);
                            $('#description').val(r.description);
                            $('#location').val(r.location);
                            $('#status').val(r.status);
                            $('#memo').val(r.memo);
                            $("[name='enabled']").each(function () {
                                if ($(this).val() == r.enabledCn) {
                                    if (preEnabled != null) {
                                        preEnabled.removeAttr("checked");
                                    }
                                    $(this).attr('checked', 'true');
                                    preEnabled = $(this);
                                    preEnabled.attr("checked", "true");
                                }
                            })

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
                    $('#editUnloadingPointForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointSave/' + currentId,
                        onSubmit: function () {
                            return $('#editUnloadingPointForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editUnloadingPoint').window('close');
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
            var selectRow = $('#dgUnloadingPoint').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除卸货点[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointDelete/' + selectRow.id;
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
