<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	事业部卸货位地图管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgUnloadingPointMap" title="事业部卸货位地图管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="" sortOrder="" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库选择：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWarehouseSelect" }); %>
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
		<div id="editUnloadingPointMap" class="easyui-window" title="事业部卸货位地图管理" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editUnloadingPointMapForm" method="post" enctype="multipart/form-data" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>卸货地图名称：</span>
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
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>描述：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="description" name="description" type="text" style="width:300px;" />
						        </td>
					        </tr>
					        <%--<tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>方向：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="direction" name="direction" type="text" style="width:120px;" />
						        </td>
					        </tr>--%>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>卸货点宽：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-numberbox" id="width" name="width" type="text" style="width:120px;text-align:right;" data-options="min:0" />
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>卸货点高：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-numberbox" id="height" name="height" type="text" style="width:120px;text-align:right;" data-options="min:0" />
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
                            <tr style="height:24px">
					            <td style="text-align:right; vertical-align:top">
						            <span>地图：</span>
					            </td>
                                <td style="" colspan="6">
                                    <input type="file" id="imgUpload" name="imgUpload" size="23" style="width:300px;"/><br/>
                                    <img src='' alt="" id="img" style="max-height:140px;max-width:300px;"/>
					            </td>
				            </tr>
				        </table>
			        </form>
                    <%-- 选择仓库 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelect.ascx"); %>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editUnloadingPointMap').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dgUnloadingPointMap').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'name', title: '地图名称', width: 160, align: 'center' }
                ]],
                columns: [[
                    { field: 'warehouseDescription', title: '仓库名称', width: 150, align: 'left' },
                    { field: 'description', title: '描述', width: 200, align: 'left' },
                    //{ field: 'direction', title: '方向', width: 120, align: 'center' },
                    { field: 'imgWidth', title: '地图宽', width: 80, align: 'center' },
                    { field: 'imgHeight', title: '地图高', width: 80, align: 'center' },
                    { field: 'width', title: '卸货点宽', width: 80, align: 'center' },
                    { field: 'height', title: '卸货点高', width: 80, align: 'center' },
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
        function Query() {
            var options = $('#dgUnloadingPointMap').datagrid('options');
            options.queryParams.queryWord = $('#search').val();
            options.queryParams.warehouseId = $('#queryWarehouseSelect').combogrid('getValue');
            $('#dgUnloadingPointMap').datagrid('reload');
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
        var currentId = null;
        function Add() {
            $('#editUnloadingPointMap').window('open');
            $("#editUnloadingPointMapForm")[0].reset();
            $('#width').numberbox('setValue', 0);
            $('#height').numberbox('setValue', 0);
            $('#img').hide();
            $('#name').focus();
            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dgUnloadingPointMap').datagrid('getSelected');
            if (selectRow) {
                $('#editUnloadingPointMap').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editUnloadingPointMapForm")[0].reset();
                            $('#name').val(r.name);
                            $('#warehouseId').val(r.warehouseId);
                            $('#warehouseDescription').val(r.warehouseDescription);
                            $('#width').numberbox('setValue', r.width);
                            $('#height').numberbox('setValue', r.height);
                            //$('#direction').val(r.direction);
                            $('#description').val(r.description);
                            $('#memo').val(r.memo);
                            $('#img').hide();
                            if (r.imgUrl){
                                $('#img').show();
                                $('#img').attr('src', r.imgUrl);
                            }

                            $('#name').focus();
                        }
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择卸货位地图', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editUnloadingPointMapForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapSave/' + currentId,
                        onSubmit: function () {
                            return $('#editUnloadingPointMapForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editUnloadingPointMap').window('close');
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
            var selectRow = $('#dgUnloadingPointMap').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除卸货位地图[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapDelete/' + selectRow.id;
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
                $.messager.alert('提示', '请选择要删除的卸货位地图', 'info');
            }
        }
    </script>
</asp:Content>
