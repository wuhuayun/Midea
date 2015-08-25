<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料来料接受_物料
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryMaterial" title="" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Print();">打印</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">货位调整</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="ReturnGoods();">退货</a>
        </div>
		<div id="editDeliveryMaterial" class="easyui-window" title="物料货位设置" style="padding: 10px;width:640px;height:480px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editDeliveryMaterialForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>编号：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" required="true" id="Text1" name="name" type="text" style="width:120px;" />
						        </td>
					        </tr>	
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>物料：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:120px;" />
						        </td>
					        </tr>					
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>数量：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" style="width:120px;"/>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>货位：</span>
						        </td>
						        <td>
							        <select id="Select1" name="queryGys" style="width:120px;"><option selected="selected" value=""></option>
                                    </select>
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
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDeliveryMaterial').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridDeliveryMaterial').datagrid({
                url: '',
//                pagination: true,
//                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '送货单号', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '物料描述', width: 120, align: 'center' },
                    { field: 'name', title: '规格', width: 70, align: 'center' },
                    { field: 'name', title: '到货时间', width: 70, align: 'center' },
                    { field: 'name', title: '数量', width: 70, align: 'center' },
                    { field: 'name', title: '单位', width: 110, align: 'center' },
                    { field: 'name', title: '合计', width: 110, align: 'center' },
                    { field: 'name', title: '备注', width: 70, align: 'left' },
                    { field: 'name', title: '采购订单', width: 50, align: 'center' },
                    { field: 'name', title: '子库', width: 50, align: 'center' },
                    { field: 'name', title: '送货单号', width: 50, align: 'center' },
                    { field: 'name', title: '送货地点', width: 50, align: 'center' },
                    { field: 'name', title: '状态', width: 50, align: 'center' },
                    { field: 'name', title: '备注', width: 70, align: 'left' }
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
        });
        function Reload() {
            $('#dataGridDeliveryMaterial').datagrid('reload');
        }
        var currentId = null;
        function Edit() {
//            var selectRow = $('#dataGridWarehouse').datagrid('getSelected');
//            if (selectRow) {
                $('#editDeliveryMaterial').window('open');
                var sUrl = '';
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editDeliveryMaterialForm")[0].reset();

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
                    $('#editDeliveryMaterialForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/' + currentId,
                        onSubmit: function () {
                            return $('#editDeliveryMaterialForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editDeliveryMaterial').window('close');
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
