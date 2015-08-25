<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	RFID管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridRfid" title="RFID管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
			<select id="queryType" name="queryType" style="width:120px;">
                <option value=""></option>
                <% List<string> listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.GetList(); %>
                <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                    { %>
                <% foreach (string bindTypeDefine in listBindTypeDefine)
                    { %>
                <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <input id="rfidSearch" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加RFID标签段</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除RFID标签段</a>
            <%} %>
        </div>
		<div id="editRfid" class="easyui-window" title="RFID管理" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editRfidForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>RFID号码段：</span>
					            </td>
					            <td style="width:80%">
                                    <input class="easyui-validatebox" name="rfidStart" required="true" type="text" style="width:120px;"/>--
                                    <input class="easyui-validatebox" name="rfidEnd" required="true" type="text" style="width:120px;"/>
                                    <%-- 
                                    <a href="javascript:void(0);" id="btnCheck" class="easyui-linkbutton" plain="true" onclick="checkCode();"><img alt='' border="0" src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/images/checkvalues.gif' width="18" height="18"  style="vertical-align:middle;"/>检测</a>
                                    --%>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>指定类型：</span>
					            </td>
					            <td style="width:80%">
					                <select id="bindTypeDefine" name="bindTypeDefine" style="width:145px;">
                                        <option value=""></option>
                                        <% listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.GetList(); %>
                                        <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                           { %>
                                        <% foreach (string bindTypeDefine in listBindTypeDefine)
                                           { %>
                                        <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
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
							        <input class="easyui-validatebox" id="memo" name="memo" type="text" style="width:300px;" />
						        </td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">增加</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editRfid').window('close');">取消</a>
				</div>
			</div>
		</div>
		<div id="deleteRfid" class="easyui-window" title="RFID标签段删除" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="deleteRfidForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>RFID号码段：</span>
					            </td>
					            <td style="width:80%">
                                    <input class="easyui-validatebox" name="rfidStart" required="true" type="text" style="width:120px;"/>--
                                    <input class="easyui-validatebox" name="rfidEnd" required="true" type="text" style="width:120px;"/>
                                    <%-- <a href="javascript:void(0);" id="A1" class="easyui-linkbutton" plain="true" onclick="checkCode();"><img alt='' border="0" src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/images/checkvalues.gif' width="18" height="18"  style="vertical-align:middle;"/>检测</a>--%>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width: 20%; text-align:right;" nowrap>
						            <span>指定类型：</span>
					            </td>
					            <td style="width:80%">
					                <select name="bindTypeDefine" style="width:145px;">
                                        <option value=""></option>
                                        <% listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.GetList(); %>
                                        <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                           { %>
                                        <% foreach (string bindTypeDefine in listBindTypeDefine)
                                           { %>
                                        <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
                                        <% } %>
                                        <% } %>
                                    </select>
                                </td>
				            </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="DeleteSubmit()">删除</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#deleteRfid').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridRfid').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/RfidList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 120, align: 'center'},
					{ field: '_bindTypeCn', title: '类型', width: 100, align: 'center' },
					{ field: '_bindDescription', title: '描述', width: 180, align: 'left' },
					{ field: 'status', title: '状态', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
//                    <% if (ynWebRight.rightEdit){ %>
//                    Edit();
//                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
            $('#queryType').change(function(){
                var queryParams = $('#dataGridRfid').datagrid('options').queryParams;
                queryParams.queryType = $('#queryType').val();
    			queryParams.queryWord = $('#rfidSearch').val();
                $('#dataGridRfid').datagrid('reload');
            })
        });
        function Reload() {
            $('#dataGridRfid').datagrid('reload');
        }
		function Query(){
			var queryParams = $('#dataGridRfid').datagrid('options').queryParams;
            queryParams.queryType = $('#queryType').val();
			queryParams.queryWord = $('#rfidSearch').val();
			$('#dataGridRfid').datagrid('reload');
		}
        var currentId = null;
        function Add() {
            $('#editRfid').window('open');
            $("#editRfidForm")[0].reset();

            //$('#rfidStart').focus();
            $('#editRfidForm textarea[name="rfidStart"]').focus();

            currentId = "";
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editRfidForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/RfidSave/',
                        onSubmit: function () {
                            return $('#editRfidForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editRfid').window('close');
                                //currentId = rVal.id;
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
            $('#deleteRfid').window('open');
            $("#deleteRfidForm")[0].reset();

            //$('#rfidStart').focus();
            $('#deleteRfidForm textarea[name="rfidStart"]').focus();
            currentId = "";
        }
        function DeleteSubmit() {
            $.messager.confirm("确认", "确认提交删除?", function (r) {
                if (r) {
                    $('#deleteRfidForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/RfidDelete/',
                        onSubmit: function () {
                            return $('#deleteRfidForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#deleteRfid').window('close');
                                //currentId = rVal.id;
                                Reload();
                            } else {
                                $.messager.alert('确认', '删除信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
    </script>
</asp:Content>
