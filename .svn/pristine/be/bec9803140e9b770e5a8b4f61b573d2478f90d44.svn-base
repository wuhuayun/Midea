<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料退货单管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridReturnedPurchase" title="物料退货单管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库：</span><select id="queryReturnedPurchase" name="queryReturnedPurchase" style="width:120px;"><option selected="selected" value=""></option>
            </select>
            领料单：<select id="Select1" name="queryReturnedPurchase" style="width:120px;"><option selected="selected" value=""></option></select>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">货位设置</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="">退料完成</a>
            <%} %>
        </div>
		<div id="editReturnedPurchase" class="easyui-window" title="物料退料单编辑" style="padding: 10px;width:640px;height:480px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editReturnedPurchaseForm" method="post" style="">
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
							        <span>货位：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="Text1" name="name" type="text" style="width:120px;" />
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
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editReturnedPurchase').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridReturnedPurchase').datagrid({
                url: '',
                //                pagination: true,
                //                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '编号', width: 80, align: 'center' },
                    { field: 'name', title: '物料', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '时间', width: 100, align: 'center' },
                    { field: 'name', title: '仓库(子库)', width: 100, align: 'center' },
                    { field: 'name', title: '货位', width: 100, align: 'center' },
                    { field: 'name', title: '数量', width: 60, align: 'center' },
                    { field: 'name', title: '备注', width: 200, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
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
            $('#dataGridReturnedPurchase').datagrid('reload');
        }
        function Search() {
            var queryParams = $('#dataGridReturnedPurchase').datagrid('options').queryParams;
            queryParams.queryWord = $('#Search').val();
            $('#dataGridReturnedPurchase').datagrid('reload');
        }
        var currentId = null;
        function Edit() {
            //            var selectRow = $('#dataGridPreparation').datagrid('getSelected');
            //            if (selectRow) {
            $('#editReturnedPurchase').window('open');
            var sUrl = '';
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    $("#editReturnedPurchaseForm")[0].reset();

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
