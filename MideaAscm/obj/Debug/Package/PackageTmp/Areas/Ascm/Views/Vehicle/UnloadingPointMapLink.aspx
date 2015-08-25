<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	卸货地图关联卸货点
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--卸货位地图-->
    <div region="west" split="false" border="false" title="" style="width:450px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
					<table id="dgUnloadingPointMap" title="卸货位地图关联卸货点" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="" sortOrder="" striped="true" toolbar="#tb">
		            </table>
                    <div id="tb">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="MapReload();">刷新</a>
                    </div>
			    </div>
			</div>
		</div>
	</div>
    <!--关联卸货点-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
					<table id="dgUnloadingPointMapLink" title="关联卸货点" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="" sortOrder="" striped="true" toolbar="#tb2">
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="LinkReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="LinkAdd();">添加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="LinkEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="LinkRemove();">移除</a>
                        <%} %>
                    </div>
                    <%-- 选择卸货点 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Vehicle/UnloadingPointSelect.ascx"); %>
			        <div id="editLink" class="easyui-window" title="关联卸货点" style="padding: 10px;width:500px;height:300px;"
				        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
			                    <form id="editLinkForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>卸货点名称：</span>
						                    </td>
						                    <td style="width:80%">
                                                <input id="pointName" name="pointName" type="text" style="width:120px;background-color:#CCCCCC;" readonly="readonly"/>
                                                <input id="pointId" name="pointId" type="hidden" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>X：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-numberbox" id="x" name="x" type="text" style="width:120px;text-align:right;" data-options="min:0" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>Y：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-numberbox" id="y" name="y" type="text" style="width:120px;text-align:right;" data-options="min:0" />
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
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="LinkSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editLink').window('close');">取消</a>
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
    <script type="text/javascript">
        $(function () {
            $('#dgUnloadingPointMap').datagrid({
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapList',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'name', title: '地图名称', width: 160, align: 'center' }
                ]],
                columns: [[
                    { field: 'warehouseDescription', title: '仓库名称', width: 150, align: 'left' },
                    { field: 'width', title: '卸货点宽', width: 80, align: 'center' },
                    { field: 'height', title: '卸货点高', width: 80, align: 'center' },
                    { field: 'imgWidth', title: '地图宽', width: 80, align: 'center' },
                    { field: 'imgHeight', title: '地图高', width: 80, align: 'center' },
                    { field: 'description', title: '描述', width: 180, align: 'left' },
                    { field: 'memo', title: '备注', width: 200, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentMapId = rowData.id;
                    LinkReload();
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentMapId) {
                        $(this).datagrid('selectRecord', currentMapId);
                    } else {
                        currentMapId = null;
                        LinkReload();
                    }
                }
            });
        });
        function MapReload() {
            $('#dgUnloadingPointMap').datagrid('reload');
        }
        var currentMapId = null;
    </script>
    <script type="text/javascript">
        $(function () {
            $('#dgUnloadingPointMapLink').datagrid({
                idField: 'id',
                checkOnSelect: false,
                selectOnCheck: false,
                loadMsg: '更新数据......',
                columns: [[
                    { checkbox: true },
                    { field: 'id', hidden: 'true' },
                    { field: 'pointName', title: '卸货点名称', width: 120, align: 'center' },
                    { field: 'x', title: 'X', width: 60, align: 'center' },
                    { field: 'y', title: 'Y', width: 60, align: 'center' },
                    { field: 'memo', title: '备注', width: 150, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentLinkId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    LinkEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    $(this).datagrid('clearChecked');
                    if (currentLinkId) {
                        $(this).datagrid('selectRecord', currentLinkId);
                    }
                }
            });
        });
        function LinkReload() {
            var options = $('#dgUnloadingPointMapLink').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapLinkList/';
            options.queryParams.mapId = currentMapId;    
            $('#dgUnloadingPointMapLink').datagrid('reload');
        }
        var currentLinkId = null;
        function LinkAdd() {
            var selectRow = $('#dgUnloadingPointMap').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请先选择卸货位地图！", "info");
                return;
            }
            SelectUnloadingPoint(selectRow.id, selectRow.warehouseId);
        }
        function UnloadingPointSelected(selectRows) {
            if (selectRows) {
                $.messager.confirm('确认', '确认添加选择的卸货点到卸货位地图？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapLinkAdd/',
                            type: "post",
                            dataType: "json",
                            data: { "mapId":currentMapId, "unloadingPointJson": $.toJSON(selectRows) },
                            success: function (r) {
                                if (r.result) {
                                    LinkReload();
                                } else {
                                    $.messager.alert('确认', '添加失败:' + r.message, 'error');
                                }
                            }

                        });
                    }
                });
            }
        }
        function LinkRemove() {
            var selectRows = $('#dgUnloadingPointMapLink').datagrid('getChecked');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请勾选要移除的卸货点！', 'info');
                return;
            }
            var linkIds = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (linkIds) {
                    linkIds += ",";
                }
                linkIds += selectRows[i].id;
            }
            $.messager.confirm('确认', '确认从卸货位地图中移除选择的卸货点?', function (result) {
                if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapLinkRemove/',
                        type: "post",
                        dataType: "json",
                        data: { "linkIds":linkIds },
                        success: function (r) {
                            if (r.result) {
                                LinkReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }

                    });
                }
            });
        }
        function LinkEdit() {
            var selectRow = $('#dgUnloadingPointMapLink').datagrid('getSelected');
            if (selectRow) {
                $('#editLink').window('open');
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapLinkEdit/' + currentLinkId,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editLinkForm")[0].reset();
                            $('#pointName').val(r.pointName);
                            $('#pointId').val(r.pointId);
                            $('#x').numberbox('setValue', r.x);
                            $('#y').numberbox('setValue', r.y);
                            $('#memo').val(r.memo);
                        }
                    }
                });
                currentLinkId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择关联的卸货点', 'info');
            }
        }
        function LinkSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editLinkForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Vehicle/UnloadingPointMapLinkSave/' + currentLinkId,
                        onSubmit: function () {
                            return $('#editLinkForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editLink').window('close');
                                if (rVal.id)
                                    currentLinkId = rVal.id;
                                LinkReload();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
