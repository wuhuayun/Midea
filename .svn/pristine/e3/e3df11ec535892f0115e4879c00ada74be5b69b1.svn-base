<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--岗位-->
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="岗位管理 " fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
                    <table id="dataGridPosition" title="" style="" border="false" fit="true" singleSelect="true"
                        idField="id" sortName="sortNo" sortOrder="asc" toolbar="#tb2">
                        <thead>
				            <tr>
					            <th field="id" width="20" align="center" hidden="true">ID</th>
					            <th field="name" width="120" align="left">岗位名称</th>
                                <th field="description" width="300" align="left">描述</th>
                                <th field="roleName" width="100" align="left">归属角色</th>
                                <th field="sortNo" width="50" align="center" sortable="true">序号</th>
				            </tr>
			            </thead>
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="positionReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="positionAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="positionEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="positionDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editPosition" class="easyui-window" title="岗位" style="padding: 10px;width:500px;height:300px;"
			            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
					        <div region="center" id="editPositionContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                                <form id="editPositionForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>岗位名称：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:200px;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>					
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>序号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" validType="number" style="text-align:right;width:200px;"/>
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
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>归属角色：</span>
						                    </td>
						                    <td style="width:80%">
                                                <input id="roleId" name="roleId" type="text" style="width:0px;display:none" />
                                                <input class="easyui-validatebox" id="roleName" name="roleName" type="text" style="width:200px; background-color:#CCCCCC;" readonly="readonly" />
                                                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" onclick="SelectRole();" style=""><img alt='' title="选择" border="0" src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/ICPROP.gif' width="16" height="13" style=""/></a>
                                                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="$('#roleId').val('');$('#roleName').val('');" title="取消"></a>
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
                                <%-- 选择角色 --%>
                                <%Html.RenderPartial("~/Areas/YnPublic/Views/YnRole/RoleSelect.ascx"); %>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="positionSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editPosition').window('close');">取消</a>
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
    <!--岗位-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridPosition').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/PositionList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                onClickRow: function (row) {
                    currentId=row.id;
                },
                onDblClickRow: function (row) {
                    <% if (ynWebRight.rightEdit){ %>
                    positionEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $('#dataGridPosition').datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function positionReload() {
            $('#dataGridPosition').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/PositionList'
            })
        }
        var currentId = null;
        function positionAdd() {
            $('#editPosition').window('open');
            $("#editPositionForm")[0].reset();

            $('#name').focus();
            currentId = null;
        }
        function positionEdit() {
            var selectRow = $('#dataGridPosition').datagrid('getSelected');
            if (selectRow) {
                $('#editPosition').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/PositionEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editPositionForm")[0].reset();

                        $('#name').val(r.name);
                        $('#sortNo').val(r.sortNo);
                        $('#description').val(r.description);
                        $('#roleId').val(r.roleId);
                        $('#roleName').val(r.roleName);

                        $('#name').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择岗位', 'info');
            }
        }
        function positionSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editPositionForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/PositionSave/' + currentId,
                        onSubmit: function () {
                            return $('#editPositionForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editPosition').window('close');

                                currentId=rVal.id;
                                positionReload()
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});

        }
        function positionDelete() {
            var selectRow = $('#dataGridPosition').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除岗位[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnPosition/PositionDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    currentId=null;
                                    positionReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的岗位', 'info');
            }
        }
        function RoleSelected(selectRow) {
            if (selectRow) {
                $('#roleId').val(selectRow.id);
                $('#roleName').val(selectRow.name);
            }
        }
    </script>
</asp:Content>
