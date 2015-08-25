<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	车门管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--车门读头管理-->
    <div region="east" title="" border="false" style="width:450px;padding:0px 0px 0px 0px;">
        <div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true">
                <div region="center" border="false">
                    <table id="dataGridReadingHead" title="车门读头" style="" border="false" fit="true" 
				        idField="id" sortName="id" sortOrder="asc" striped="true">
			        </table>
                    <%-- 选择读头 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Public/ReadingHeadSelect.ascx"); %>
                </div>
            </div>
		</div>
    </div>
    <!--车门管理-->
    <div region="center" split="false" border="false" title="" style="padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
					<table id="dataGridDoor" title="车门管理" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tbDoor">
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
                        <input id="doorSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="DoorSearch();"></a> 
                    </div>
			        <div id="editDoor" class="easyui-window" title="车门管理" style="padding: 10px;width:500px;height:300px;"
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
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorList',
//                pagination: true,
//                pageSize: 50,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '大门名称', width: 100, align: 'center' },
                    { field: '_directionCn', title: '进出方向', width: 80, align: 'center' },
                    { field: '_vehicleTypeCn', title: '大门类型', width: 80, align: 'center' },
                    { field: 'enabled', title: '是否使用', width: 80, align: 'center' },
                    { field: 'description', title: '描述', width: 120, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentDoorId=rowData.id;
                    DoorReadingHeadReload();
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
                    DoorReadingHeadReload();
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
    <script type="text/javascript">
        $(function () {
            $('#dataGridReadingHead').datagrid({
                loadMsg: '加载数据......',
                columns: [[
                    { field: 'select', title: '',checkbox:true},
                    { field: 'id', title: 'ID', width: 100, align: 'center'},
					{ field: 'ip', title: 'Ip地址', width: 100, align: 'center' },
					{ field: 'port', title: '端口', width: 80, align: 'center' },
					{ field: 'status', title: '状态', width: 80, align: 'center' }
                ]],
                toolbar: [{
                    text: '刷新',
                    iconCls: 'icon-reload',
                    handler: function () {
                        DoorReadingHeadReload();
                    }
                }<% if (ynWebRight.rightEdit){ %> , '-', {
                    text: '添加',
                    iconCls: 'icon-add',
                    handler: DoorAddReadingHead
                }<%} %><% if (ynWebRight.rightDelete){ %> , '-', {
                    text: '移除',
                    iconCls: 'icon-cancel',
                    handler: DoorRemoveReadingHead
                }<%} %>],
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                }
            });
        })
        function DoorReadingHeadReload() {
            var title = '大门读头';
            var selectRow = $('#dataGridDoor').datagrid('getSelected');
            if (selectRow) {
                title = '大门[<font color="red">' + selectRow.name + '</font>]读头';
            }
            $('#dataGridReadingHead').datagrid({
                title: title,
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorReadingHeadList/?doorId=' + currentDoorId
            });
        }
        var currentReadingHeadId = null;
        function DoorAddReadingHead() {
            var selectRow = $('#dataGridDoor').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请选择大门！", "info");
                return;
            }
            //SelectReadingHead('<%=MideaAscm.Dal.Base.Entities.AscmReadingHead.BindTypeDefine.employeeCar %>');
            SelectReadingHead('');
        }
        function ReadingHeadSelected(selectRows) {
            if (selectRows) {
                var ids = "";
                for (var i = 0; i < selectRows.length; i++) {
                    if (ids) {
                        ids += ",";
                    }
                    ids += selectRows[i].id;
                }
                //positionIds += selectRow.id;
                $.messager.confirm('确认', '确认添加选择读头到大门？', function (result) {
                    if (result) {
                        var selectRow = $('#dataGridDoor').datagrid('getSelected');
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorAddReadingHead/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data: { ids: ids },
                            success: function (r) {
                                if (r.result) {
                                    //$('#userSelect').window('close');
                                    DoorReadingHeadReload();
                                } else {
                                    $.messager.alert('确认', '添加失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function DoorRemoveReadingHead() {
            var selectRows = $('#dataGridReadingHead').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要移除的读头！', 'info');
                return;
            }
            var ids = "";
            var names = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (names) {
                    names += ",";
                }
                names += selectRows[i].ip;
                if (ids) {
                    ids += ",";
                }
                ids += selectRows[i].id;
            }
            $.messager.confirm('确认', '确认从大门中移除读头[<font color="red">' + names + '</font>]?', function (result) {
                if (result) {
                    var selectRow = $('#dataGridDoor').datagrid('getSelected');
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/DoorRemoveReadingHead/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { ids: ids },
                        success: function (r) {
                            if (r.result) {
                                DoorReadingHeadReload();
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
