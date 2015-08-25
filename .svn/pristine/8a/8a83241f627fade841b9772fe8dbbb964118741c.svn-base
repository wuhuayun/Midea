<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库(子库)管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridWarehouse" title="仓库(子库)管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="Reload();">刷新</a>
            <% if (ynWebRight.rightEdit){ %>
			<%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>--%>
            <%} %>
			<input id="warehouseSearch" type="text" style="width:100px;" />
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Search();"></a>
        </div>
		<div id="editWarehouse" class="easyui-window" title="仓库(子库)" style="padding: 10px;width:640px;height:480px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editWarehouseForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>名称：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" required="true" id="id" name="id" type="text" style="width:120px; background-color:#CCCCCC;" readonly="readonly"/>
						        </td>
					        </tr>	
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>是否启用：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" type="checkbox" id="enabled" name="enabled" value=""/>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>描述：</span>
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
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editWarehouse').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridWarehouse').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: '子库名称', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'description', title: '描述', width: 300, align: 'left' },
                    //{ field: 'name', title: '联系电话', width: 100, align: 'center' },
                    { field: '_createTime', title: '创建时间', width: 110, align: 'center' },
                    { field: '_modifyTime', title: '最后更新时间', width: 110, align: 'center' },
                    { field: 'status', title: '状态', width: 50, align: 'left' },
                    { field: 'enabled', title: '在用', width: 50, align: 'left' }
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
            $('#dataGridWarehouse').datagrid('reload');
        }
        function Search() {
            var queryParams = $('#dataGridWarehouse').datagrid('options').queryParams;
            queryParams.queryWord = $('#warehouseSearch').val();
            $('#dataGridWarehouse').datagrid('reload');
        }
        var currentId = null;
        function Edit() {
            var selectRow = $('#dataGridWarehouse').datagrid('getSelected');
            if (selectRow) {
                $('#editWarehouse').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseEdit/'+encodeURIComponent(selectRow.id);
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editWarehouseForm")[0].reset();

                        $('#id').val(r.id);
                        $("#enabled").prop("checked", r.enabled);
                        $('#description').val(r.description);

                        $('#name').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择供应商', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editWarehouseForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Warehouse/WarehouseSave/' + encodeURIComponent(currentId),
                        onSubmit: function () {
                            $("#enabled").val($("#enabled").prop("checked"));
                            return $('#editWarehouseForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editWarehouse').window('close');
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
    </script>
</asp:Content>
