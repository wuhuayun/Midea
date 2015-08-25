<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料备料清单
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridPreparation" title="物料备料管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库：</span><select id="queryPreparation" name="queryPreparation" style="width:120px;"><option selected="selected" value=""></option>
            </select>
            需求时间：</span><input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" onclick="Query();"><img alt='' border="0" src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/Kpi_ResetOrder.gif' width="16" height="16"  style="vertical-align:middle;"/>生成备料清单</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印备料清单</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="Ok();">备料完成</a>
            备料清单状态：<input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:120px;" />
            </br>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">备料调整</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
            <%} %>
        </div>
		<div id="editPreparation" class="easyui-window" title="物料备料数量调整" style="padding: 10px;width:640px;height:480px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editPreparationForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>物料：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" required="true" id="Text1" name="name" type="text" style="width:120px;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>	
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>数量：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" required="true" id="Text1" name="name" type="text" style="width:400px;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>					
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>备注：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="description" name="description" type="text" style="width:400px;" />
						        </td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editPreparation').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridPreparation').datagrid({
                url: '',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '编号', width: 80, align: 'center' },
                    { field: 'name', title: '物料', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '作业号', width: 100, align: 'center' },
                    { field: 'name', title: '时间', width: 100, align: 'center' },
                    { field: 'name', title: '仓库(子库)', width: 100, align: 'center' },
                    { field: 'name', title: '货位', width: 100, align: 'center' },
                    { field: 'name', title: '生产线', width: 100, align: 'center' },
                    { field: 'name', title: '数量', width: 60, align: 'center' },
                    { field: 'name', title: '备注', width: 200, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function Reload() {
            $('#dataGridPreparation').datagrid('reload');
        }
        function Search() {
            var queryParams = $('#dataGridPreparation').datagrid('options').queryParams;
            queryParams.queryWord = $('#Search').val();
            $('#dataGridPreparation').datagrid('reload');
        }
        var currentId = null;
        function Edit() {
            //            var selectRow = $('#dataGridPreparation').datagrid('getSelected');
            //            if (selectRow) {
            $('#editPreparation').window('open');
            var sUrl = '';
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    $("#editPreparationForm")[0].reset();

                    $('#name').val(r.name);
                    $('#sortNo').val(r.sortNo);
                    $('#description').val(r.description);

                    $('#name').focus();
                }
            });
            //                currentId = selectRow.id;
            //            } else {
            //                $.messager.alert('提示', '请选择供应商', 'info');
            //            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editPreparationForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/' + currentId,
                        onSubmit: function () {
                            return $('#editPreparationForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editPreparation').window('close');
                                currentId = selectRow.id;
                                Reload();
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
