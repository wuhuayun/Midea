<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商司机送货_物料
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryMaterial" title="" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Print();">打印</a>
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
    </script>
</asp:Content>
