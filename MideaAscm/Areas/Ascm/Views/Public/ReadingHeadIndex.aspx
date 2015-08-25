<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RFID读头管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridReadingHead" title="RFID读头管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
			<%--<select id="queryType" name="queryType" style="width:120px;">
                <option value=""></option>
                <% List<string> listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmReadingHead.BindTypeDefine.GetList(); %>
                <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                    { %>
                <% foreach (string bindTypeDefine in listBindTypeDefine)
                    { %>
                <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.Base.Entities.AscmReadingHead.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
                <% } %>
                <% } %>
            </select>--%>
            <input id="readingHeadSearch" type="text" style="width:100px;" />
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
		<div id="editReadingHead" class="easyui-window" title="RFID读头管理" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editReadingHeadForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <%--<tr style="height:24px">--%>
					            <%--<td style="width: 20%; text-align:right;" nowrap>
						            <span>指定类型：</span>
					            </td>
					            <td style="width:80%">--%>
					                <%--<select id="bindType" name="bindType" style="width:145px;">
                                        <option value=""></option>
                                        <% listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmReadingHead.BindTypeDefine.GetList(); %>
                                        <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                           { %>
                                        <% foreach (string bindTypeDefine in listBindTypeDefine)
                                           { %>
                                        <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.Base.Entities.AscmReadingHead.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
                                        <% } %>
                                        <% } %>
                                    </select>--%>
                                <%--</td>--%>
				            <%--</tr>--%>
				            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>Ip地址：</span>
					            </td>
					            <td>
						            <input class="easyui-validatebox" id="ip" name="ip" type="text" style="width:140px;"/>
					            </td>
				            </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>端口：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="port" name="port" type="text" validType="number" style="width:140px;"/>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>物理地址：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="address" name="address" type="text" style="width:140px;"/>
						        </td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">增加</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editReadingHead').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridReadingHead').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ReadingHeadList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', title: 'ID', width: 100, align: 'center'},
//					{ field: '_bindTypeCn', title: '类型', width: 100, align: 'center' },
					{ field: 'bindId', title: '绑定Id', width: 150, align: 'center' },
					{ field: 'ip', title: 'Ip地址', width: 100, align: 'center' },
					{ field: 'port', title: '端口', width: 80, align: 'center' },
					{ field: 'status', title: '状态', width: 80, align: 'center' },
                    { field: 'address', title: '物理地址', width: 180, align: 'left' }
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
            $('#queryType').change(function(){
                var queryParams = $('#dataGridReadingHead').datagrid('options').queryParams;
                queryParams.queryType = $('#queryType').val();
    			queryParams.queryWord = $('#readingHeadSearch').val();
                $('#dataGridReadingHead').datagrid('reload');
            })
        });
        function Reload() {
            $('#dataGridReadingHead').datagrid('reload');
        }
		function Query(){
			var queryParams = $('#dataGridReadingHead').datagrid('options').queryParams;
            queryParams.queryType = $('#queryType').val();
			queryParams.queryWord = $('#readingHeadSearch').val();
			$('#dataGridReadingHead').datagrid('reload');
		}
        var currentId = null;
        function Add() {
            $('#editReadingHead').window('open');
            $("#editReadingHeadForm")[0].reset();

            //$('#rfidStart').focus();
            $('#editReadingHeadForm textarea[name="ip"]').focus();

            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dataGridReadingHead').datagrid('getSelected');
            if (selectRow) {
                $('#editReadingHead').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ReadingHeadEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editReadingHeadForm")[0].reset();

//				        $("#editReadingHeadForm select[name='bindType']").val(r.bindType);
                        $('#ip').val(r.ip);
				        $('#port').val(r.port);

                        $('#editReadingHeadForm textarea[name="ip"]').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择读头', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editReadingHeadForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ReadingHeadSave/' + currentId,
                        onSubmit: function () {
                            return $('#editReadingHeadForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editReadingHead').window('close');
                                currentId = rVal.id;
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
            var selectRow = $('#dataGridReadingHead').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除读头[<font color="red">' + selectRow.ip + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ReadingHeadDelete/' + selectRow.id;
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
                $.messager.alert('提示', '请选择要删除的读头', 'info');
            }
        }
    </script>
</asp:Content>
