<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物流组用户信息查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <div class="easyui-panel" title="用户管理 " fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
			        <table id="dataGridUser" title="" style="" border="false" fit="true" singleSelect="true"
						idField="userId" sortName="userId" sortOrder="asc" striped="true" toolbar="#tb2">
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="userReload();">刷新</a>

                        <% if (ynWebRight.rightVerify){ %>
                            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="userLogisticsClass();">设置物流班组</a>
                            <span>所属班组：</span>
                            <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewLogisticsClassSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "logisticsName", panelWidth = 500 }); %></span>
                        <%} %>

                        <input id="userSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="userSearch();"></a>
                    </div>
			    </div>
                <div id="editLogisticsClass" class="easyui-window" title="设置用户物流班组" style="padding: 10px;width:500px;height:300px;"
		            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			        <div class="easyui-layout" fit="true">
				        <div region="center" id="editLogisticsClassContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
		                    <form id="editLogisticsClassForm" method="post" style="">
			                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				                    <tr style="height:24px">
					                    <td style="width: 20%; text-align:right;" nowrap>
						                    <span>所属物流班组：</span>
					                    </td>
					                    <td style="width:80%">
						                    <input class="easyui-validatebox" id="oldLogisticsName" name="logisticsClassName" style="width:150px;" readOnly ="readOnly" />
					                    </td>
				                    </tr>
				                    <tr style="height:24px">
					                    <td style="width: 20%; text-align:right;" nowrap>
						                    <span>重置物流班组：</span>
					                    </td>
					                    <td style="width:80%">
						                    <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewLogisticsClassSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "logisticsClass", width = "155px", panelWidth = 500 }); %></span>
					                    </td>
				                    </tr>
			                    </table>
		                    </form>
				        </div>
				        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    
					        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="setUserLogisticsClass()">确认</a>
                    
					        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editLogisticsClass').window('close');">取消</a>
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
    var currentId = "";
    $(function () {
        $('#dataGridUser').datagrid({
            url: '/Ascm/Logistics/LogisticsYnUserList',
            frozenColumns: [[
	                { field: 'userId', title: '用户账号', width: 90, sortable: true, align: 'left' }
				]],
            columns: [[
					{ field: 'userName', title: '姓名', width: 90, align: 'left' },
					{ field: 'sex', title: '性别', width: 50, align: 'center' },
					{ field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
					{ field: 'mobileTel', title: '移动电话', width: 100, align: 'center' },
					{ field: 'accountLocked', title: '账号锁定', width: 60, align: 'center' },
					{ field: 'departmentPositionList', title: '部门岗位', width: 120, align: 'left' },
                    { field: 'roleList', title: '角色', width: 120, align: 'left' },
                    { field: 'logisticsClassName', title: '所属物流组', width: 120, align: 'left' }
				]],
            //            pagination: true,
            //            pageSize: 30,
            loadMsg: '加载数据......',
            onSelect: function (rowIndex, rowData) {
                currentId = rowData.userId;
                //userRoleReload();
            },
            onDblClickRow: function (rowIndex, rowData) {
                <% if (ynWebRight.rightVerify){ %>
                    userLogisticsClass();
                <%} %>
            },
            onLoadSuccess: function (data) {
                $('#dataGridUser').datagrid('clearSelections');
                //userRoleReload();
                if (currentId) {
                    $(this).datagrid('selectRecord', currentId);
                }
            }
        });
    });
</script>

<script type="text/javascript">
    function userSearch() {
        var options = $('#dataGridUser').datagrid('options').queryParams;
        options.queryWord = $('#userSearch').val();

        <% if (ynWebRight.rightVerify){ %>
               options.queryLogisticsClass = $('#logisticsName').combogrid('getValue');
        <%} %>
        
        $('#dataGridUser').datagrid('reload');
    }

    function userReload() {
        $('#dataGridUser').datagrid({
            url: '/Ascm/Logistics/LogisticsYnUserList'
        });
    }
</script>
<script type="text/javascript">
    function userLogisticsClass() {
        var selectRow = $('#dataGridUser').datagrid('getSelected');
        if (selectRow) {
            $('#editLogisticsClass').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GetLogisticsYnUserClass/' + selectRow.userId;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#editLogisticsClassForm")[0].reset();
                        $('#logisticsClass').combogrid('clear');

                        $('#oldLogisticsName').val(r.logisticsClassName);
                    }
                }
            });
            currentId = selectRow.userId;
        } else {
            $.messager.alert('提示', '请选择要设置物流班组的用户', 'info');
        }
    }

    function setUserLogisticsClass() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editLogisticsClassForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/SetLogisticsYnUserClass/' + currentId,
                    onSubmit: function () {
                        return $('#editLogisticsClassForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editLogisticsClass').window('close');
                            currentId = rVal.userId;
                            userSearch();
                        } else {
                            $.messager.alert('确认', '设置物流班组信息失败:' + rVal.message, 'error');
                        }
                    }
                });
            }
        });
    }
</script>
</asp:Content>
