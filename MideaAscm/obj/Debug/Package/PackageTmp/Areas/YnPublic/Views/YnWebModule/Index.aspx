<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnWebModule>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 模块 -->
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
		<table id="dataGridModule" title="模块" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb">
			<thead>
				<tr>
					<th field="id" width="20" align="center" hidden="true">ID</th>
                    <th field="sortNo" width="50" align="center" sortable="true">序号</th>
					<th field="name" width="120" align="left">名称</th>
                    <th field="url" width="240" align="left">Url</th>
                    <th field="isAllowAnonymous" width="80" align="center">允许匿名</th>
                    <th field="isAllowAllUsers" width="80" align="center">允许全部用户</th>
                    <th field="parameter" width="260" align="left">参数</th>
				</tr>
			</thead>
		</table>
        <div id="tb">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="moduleReload();">刷新</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="moduleAdd();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="moduleEdit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="moduleDelete();">删除</a>
            <%} %>
            <input id="moduleSearch" type="text" style="width:100px;" />
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="moduleSearch();"></a> 
        </div>
		<form id="editModuleForm" method="post" style="display:none;">
			<table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				<tr style="height:24px">
					<td style="width:10%; text-align:right;" nowrap>
						<span>名称：</span>
					</td>
					<td style="width:80%">
						<input class="easyui-validatebox" required="true" name="name" type="text" style="width:96%;" /><span style="color:Red;">*</span>
					</td>
				</tr>
				<tr style="height:24px">
					<td style="text-align:right;" nowrap>
						<span>序号：</span>
					</td>
					<td>
						<input class="easyui-validatebox" name="sortNo" type="text" validType="number" style="text-align:right"/>
					</td>
				</tr>
                <tr style="height:24px">
					<td style="width:10%; text-align:right;" nowrap>
						<span>Url：</span>
					</td>
					<td style="width:80%">
						<input class="easyui-validatebox" required="true" name="url" type="text" style="width:96%;" />
					</td>
				</tr>
                <tr style="height:24px">
					<td style="text-align:right;" nowrap>
						<span>允许匿名：</span>
					</td>
					<td>
						<input class="easyui-validatebox" type="checkbox" id="allowAnonymous" name="allowAnonymous" value=""/>
					</td>
				</tr>
				<tr style="height:24px">
					<td style="text-align:right;" nowrap>
						<span>允许全部用户：</span>
					</td>
					<td>
						<input class="easyui-validatebox" type="checkbox" id="allowAllUsers" name="allowAllUsers" value=""/>
					</td>
				</tr>
				<tr style="height:24px">
					<td style="width: 20%; text-align:right;" nowrap>
						<span>参数：</span>
					</td>
					<td style="width:80%">
						<%-- <input class="easyui-validatebox" name="parameter" type="text" style="width:96%;"/>--%>
                        <textarea id="parameter" name="parameter" rows="1" cols="3" style="width:96%;border:1px solid #A4BED4;height:160px;"></textarea>
					</td>
				</tr>
			</table>
		</form>
		<div id="editModule" class="easyui-window" title="模块" style="padding: 10px;width:500px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" id="editModuleContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="moduleSave()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editModule').window('close');">取消</a>
				</div>
			</div>
		</div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $("#dataGridModule").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebModule/ModuleList',
                pagination: true,
                pageSize: 30,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    moduleEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })
            //moduleReload();
        })
        function moduleReload() {
            $('#dataGridModule').datagrid('reload');
        }
        function moduleSearch() {
            var queryParams = $('#dataGridModule').datagrid('options').queryParams;
            queryParams.queryWord = $('#moduleSearch').val();
            $('#dataGridModule').datagrid('reload');
        }
        var currentId = null;
        function moduleAdd() {
            $("#editModule").window("open");
            $("#editModuleForm").show();
            $("#editModuleForm").appendTo("#editModuleContent");
            $("#editModuleForm input[name='name']").focus();
            $("#editModuleForm")[0].reset();
            currentId = null;
        }
        function moduleEdit() {
            var selectRow = $('#dataGridModule').datagrid('getSelected');
            if (selectRow) {
                $("#editModule").window("open");
                $("#editModuleForm").show();
                $("#editModuleForm").appendTo("#editModuleContent");
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebModule/ModuleEdit/' + selectRow.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        $("#editModuleForm")[0].reset();

                        $("#editModuleForm input[name='name']").val(r.name);
                        $("#editModuleForm input[name='sortNo']").val(r.sortNo);
                        $("#editModuleForm input[name=url]").val(r.url);
                        $("#editModuleForm input[name=allowAnonymous]").prop("checked", r.allowAnonymous);
                        $("#editModuleForm input[name=allowAllUsers]").prop("checked", r.allowAllUsers);
                        //$("#editModuleForm input[name=parameter]").val(r.parameter);
                        $('#parameter').val(r.parameter);
                        $("#editModuleForm input[name='name']").focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert("提示", "请先选中要编辑的一行数据！", "info");
            }
        }
        function moduleSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebModule/ModuleSave/' + currentId;
                    $('#editModuleForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            $("#allowAnonymous").val($("#allowAnonymous").prop("checked"));
                            $("#allowAllUsers").val($("#allowAllUsers").prop("checked"));
                            return $('#editModuleForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editModule').window('close');
                                moduleReload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })
                }
            });

        }
        function moduleDelete() {
            var selectRow = $('#dataGridModule').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除链接[<font style='color:red;'>" + selectRow.name + "</font>]", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebModule/ModuleDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    moduleReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + retValue.message, 'error');
                                }
                            }

                        });
                    }
                })
            } else {
                $.messager.alert("提示", "请先选中要删除的一行数据！", "info");
            }
        }
    </script>
</asp:Content>
