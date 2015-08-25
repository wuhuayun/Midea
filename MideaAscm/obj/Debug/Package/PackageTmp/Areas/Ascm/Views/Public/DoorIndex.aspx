<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	事业部大门管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--车门管理-->
    <div region="center" split="false" border="false" title="" style="padding:0px 0px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
					<table id="dataGridDoor" title="事业部大门管理" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="" sortOrder="" striped="true" toolbar="#tbDoor">
		            </table>
                    <div id="tbDoor">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DoorReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DoorAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="DoorEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DoorDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editDoor" class="easyui-window" title="事业部大门管理" style="padding: 10px;width:500px;height:300px;"
				        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" id="editDoorContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
			                    <form id="editDoorForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>名称：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" name="name" type="text" style="width:140px;" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>方向：</span>
						                    </td>
						                    <td style="width:80%">
					                            <select name="direction" style="width:145px;">
                                                    <option value=""></option>
                                                    <% List<string> listDirectionDefine = MideaAscm.Dal.Base.Entities.AscmDoor.DirectionDefine.GetList(); %>
                                                    <% if (listDirectionDefine != null && listDirectionDefine.Count > 0)
                                                       { %>
                                                    <% foreach (string directionDefine in listDirectionDefine)
                                                       { %>
                                                    <option value="<%=directionDefine %>"><%=MideaAscm.Dal.Base.Entities.AscmDoor.DirectionDefine.DisplayText(directionDefine)%></option>
                                                    <% } %>
                                                    <% } %>
                                                </select>
                                            </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>车辆类型：</span>
						                    </td>
						                    <td style="width:80%">
					                            <select name="vehicleType" style="width:145px;">
                                                    <option value=""></option>
                                                    <% List<string> listVehicleTypeDefine = MideaAscm.Dal.Base.Entities.AscmDoor.VehicleTypeDefine.GetList(); %>
                                                    <% if (listVehicleTypeDefine != null && listVehicleTypeDefine.Count > 0)
                                                       { %>
                                                    <% foreach (string vehicleTypeDefine in listVehicleTypeDefine)
                                                       { %>
                                                    <option value="<%=vehicleTypeDefine %>"><%=MideaAscm.Dal.Base.Entities.AscmDoor.VehicleTypeDefine.DisplayText(vehicleTypeDefine)%></option>
                                                    <% } %>
                                                    <% } %>
                                                </select>
                                            </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="text-align:right;" nowrap>
							                    <span>备注：</span>
						                    </td>
						                    <td>
							                    <input class="easyui-validatebox" name="description" type="text" style="width:300px;" />
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="DoorSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDoor').window('close');">取消</a>
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
            $('#dataGridDoor').datagrid({
                rownumbers: true,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorList',
                columns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'name', title: '大门名称', width: 200, align: 'left' },
                    { field: '_directionCn', title: '进出方向', width: 80, align: 'center' },
                    { field: '_vehicleTypeCn', title: '大门类型', width: 80, align: 'center' },
                    { field: 'enabled', title: '是否使用', width: 80, align: 'center' },
                    { field: 'description', title: '描述', width: 220, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentDoorId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    DoorEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentDoorId) {
                        $(this).datagrid('selectRecord', currentDoorId);
                    }
                }
            });
        });
        function DoorReload() {
            $('#dataGridDoor').datagrid('reload');
        }
        var currentDoorId = null;
        function DoorAdd() {
            $("#editDoor").window("open");
            $("#editDoorForm")[0].reset();

            $("#editDoorForm input[name='name']").focus();
            currentDoorId = null;
        }
        function DoorEdit() {
            var selectRow = $('#dataGridDoor').datagrid('getSelected');
            if (selectRow) {
                $('#editDoor').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editDoorForm")[0].reset();

				        $("#editDoorForm input[name='name']").val(r.name);
				        $("#editDoorForm select[name='direction']").val(r.direction);
				        $("#editDoorForm select[name='vehicleType']").val(r.vehicleType);
				        $("#editDoorForm input[name='description']").val(r.description);

                        $("#editDoorForm input[name='name']").focus();
                    }
                });
                currentDoorId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择大门', 'info');
            }
        }
        function DoorSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editDoorForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorSave/' + currentDoorId,
                        onSubmit: function () {
                            return $('#editDoorForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editDoor').window('close');
                                currentDoorId = rVal.id;
                                DoorReload();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
        function DoorDelete() {
            var selectRow = $('#dataGridDoor').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除大门[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    DoorReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的大门', 'info');
            }
        }
    </script>
</asp:Content>
