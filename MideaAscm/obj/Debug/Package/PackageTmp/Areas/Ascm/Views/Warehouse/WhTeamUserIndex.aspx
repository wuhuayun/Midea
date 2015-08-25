<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 组别用户设置 -->
	<div region="west" split="false" border="true" title="" style="width:390px;padding:0px 2px 0px 0px;overflow:auto;">
		<table id="dataGridTeam" title="组别" style="" border="false" fit="true" singleSelect="true"
				idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#toolbar_team">
            <thead>
				<tr>
					<th field="name" width="120" align="left">组别名称</th>
                    <th field="description" width="260" align="left">描述</th>
				</tr>
			</thead>
		</table>
        <div id="toolbar_team">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="teamReload();">刷新</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="teamAdd();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="teamEdit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="teamDelete();">删除</a>
            <%} %>
			<input id="txtTeamName" type="text" style="width:100px;" />
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="teamSearch();"></a>
        </div>
        <div id="editTeam" class="easyui-window" title="组别" style="padding: 10px;width:500px;height:300px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" id="editTeamContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editTeamForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>组别名称：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:96%;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>					
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>序号：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" validType="number" style="text-align:right"/>
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
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="teamSave()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editTeam').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>

    <div region="center" title="" border="true" style="padding:0px;">
        <table id="dataGridUser" title="组别用户" style="" border="false" fit="true" checkOnSelect="false" selectOnCheck="false"  singleSelect="true"
			idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#toolbar_User">
			<thead>
				<tr>
                    <th field="select" checkbox="true"></th>
					<th field="userId" width="150" align="left">用户账号</th>
					<th field="userName" width="250" align="left">用户名称</th>
					<th field="isLeader" width="120" align="left" formatter="formatIsLeader">是否负责人</th>
				</tr>
			</thead>
		</table>
        <div id="toolbar_User">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="teamUserReload();">刷新</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="teamAddUser();">添加关联用户</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="teamRemoveUser();">移除关联用户</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="setTeamLeader();">设为负责人</a>
            <%} %>
        </div>
        <div id="userSelect" class="easyui-window" title="选择用户" style="padding: 5px;width:540px;height:360px;"
			iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">                    
				<div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
					<table id="dataGridSelectUser" title="" style="" border="false" fit="true"
					idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tb1">
						<thead>
							<tr>
								<th field="select" checkbox="true"></th>
								<th field="userId" width="150" align="left">用户账号</th>
								<th field="userName" width="280" align="left">用户名称</th>
							</tr>
						</thead>
					</table>
                    <div id="tb1">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="UserSelectReload();">刷新</a>
                        <input id="userSelectSearch" type="text" style="width:120px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="UserSelectSearch();"></a>
                        <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                        <a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="UserSelectOk()">确认</a>
                        <%} %>
						<a href="javascript:void(0);"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#userSelect').window('close');">取消</a> 
                    </div>
				</div>
			</div>
		</div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 表单类型 --%>
    <script type="text/javascript">
        function formatIsLeader(value,rowData,rowIndex) 
        {
            if(value == true)
            {
                return "√";
            }

            return  "";
        }

        var currentTeamId = null;
        var currentTeamName = "";
        var currentUserId = null;

        $(function () {
            $('#dataGridTeam').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WhTeamList',
                loadMsg: '更新数据......',
                onSelect: function (rowIndex, rowData) {
                    currentTeamId = rowData.id;
                    teamUserReload();
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    teamEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $('#dataGridTeam').datagrid('clearSelections');
                    if (currentTeamId) {
                        $(this).datagrid('selectRecord', currentTeamId);
                    }
                }
            });

            $('#dataGridUser').datagrid({
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentUserId = rowData.userId;
                    //userteamReload();
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearChecked');
                    $(this).datagrid('clearSelections');
                    if (currentUserId) {
                        $(this).datagrid('selectRecord', currentUserId);
                    }
                }
            });

            $('#dataGridSelectUser').datagrid({
                pagination: true,
                pageSize: 50,
                loadMsg: '加载数据......'
            });

        });

        function teamReload() {
            $('#dataGridTeam').datagrid('reload');
        }

        function teamSearch() {
            var queryParams = $('#dataGridTeam').datagrid('options').queryParams;
            queryParams.queryWord = $('#txtTeamName').val();
            $('#dataGridTeam').datagrid('reload');
        }

        function teamAdd() {
            $('#editTeam').window('open');
            $("#editTeamForm")[0].reset();

            $('#name').focus();
            currentTeamId = null;
        }

        function teamEdit() {
            var selectRow = $('#dataGridTeam').datagrid('getSelected');
            if (selectRow) {
                $('#editTeam').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WhTeamEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editTeamForm")[0].reset();

                        $('#name').val(r.name);
                        $('#sortNo').val(r.sortNo);
                        $('#description').val(r.description);
                        $('#name').focus();
                    }
                });
                currentTeamId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择组别', 'info');
            }
        }

        function teamSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editTeamForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WhTeamSave/' + currentTeamId,
                        onSubmit: function () {
                            return $('#editTeamForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editTeam').window('close');
                                $('#dataGridTeam').datagrid({
                                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WhTeamList',
                                    onLoadSuccess: function () {
                                        $('#dataGridTeam').datagrid('clearSelections');
                                        $('#dataGridTeam').datagrid('selectRecord', rVal.id);
                                    }
                                });
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }

        function teamDelete() {
            var selectRow = $('#dataGridTeam').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除组别[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WhTeamDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    currentTeamId = null;
                                    teamReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的组别', 'info');
            }
        }

        function teamUserReload() {
            $("#dataGridUser").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/TeamUserList?teamId=' + currentTeamId
            });
        }

        function teamAddUser() {
            if (!currentTeamId) {
                $.messager.alert("提示", "请选择组别！", "info");
                return;
            }
            $('#userSelectSearch').val("");
            $('#userSelect').window('open');
            UserSelectReload();
        }

        function teamRemoveUser() {
            var selectRows = $('#dataGridUser').datagrid('getChecked');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要移除的用户！', 'info');
                return;
            }
            var userIds = "";
            var userNames = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (userNames) {
                    userNames += ",";
                }
                userNames += selectRows[i].userName;
                if (userIds) {
                    userIds += ",";
                }
                userIds += selectRows[i].userId;
            }
            $.messager.confirm('确认', '确认从组别中移除用户[<font color="red">' + userNames + '</font>]?', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WhTeamRemoveUser?teamId=' + currentTeamId + '&userIds=' + userIds;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                teamUserReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }

                    });
                }
            });
        }

        function setTeamLeader() {
            if (!currentTeamId || currentTeamId == 0) {
                $.messager.alert('提示', '请选择组别！', 'info');
                return;
            }

            var selectRows = $('#dataGridUser').datagrid('getChecked');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要用户！', 'info');
                return;
            }
            var userId = selectRows[0].userId;
            var userName = selectRows[0].userName;

            $.messager.confirm('确认', '确认将用户[<font color="red">' + userName + '</font>]设为负责人？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/SetWhTeamLeader?teamId=' + currentTeamId + '&userId=' + userId;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                teamUserReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        
        function UserSelectReload() {
//            if (!currentDepartmentId||!currentPositionId) {
//				return;
//            }

            var queryParams = $('#dataGridSelectUser').datagrid('options').queryParams;
            queryParams.queryWord = "";
            $('#dataGridSelectUser').datagrid('reload');
        }

        function UserSelectSearch() {
            var queryParams = $('#dataGridSelectUser').datagrid('options').queryParams;
            queryParams.queryWord = $('#userSelectSearch').val();
            $('#dataGridSelectUser').datagrid('reload');
        }

        function UserSelectOk() {
            var selectRows = $('#dataGridSelectUser').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert("提示", "请选择要加入的用户！", "info");
                return;
            }
            var userIds = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (userIds) {
                    userIds += ",";
                }
                userIds += selectRows[i].userId;
            }
            $.messager.confirm('确认', '确认添加用户到组别？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WhTeamAddUser?teamId=' + currentTeamId + '&userIds=' + userIds;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                $('#userSelect').window('close');
                                teamUserReload();
                            } else {
                                $.messager.alert('确认', '添加失败:' + r.message, 'error');
                            }
                        }

                    });
                }
            });
        }
    </script>
</asp:Content>
