<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商送货_送货清单
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryOrder" title="" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Print();">打印</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#dataGridDeliveryOrder').datagrid({
                url: '',
//                pagination: true,
//                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '送货单号', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '子库', width: 100, align: 'center' },
                    { field: 'name', title: '作业号', width: 60, align: 'center' },
                    { field: 'name', title: '作业上线时间', width: 100, align: 'center' },
                    { field: 'name', title: '生成日期', width: 110, align: 'center' },
                    { field: 'name', title: '送货地点', width: 150, align: 'center' },
                    { field: 'name', title: '物料品种数', width: 100, align: 'center' },
                    { field: 'name', title: '物料件数', width: 100, align: 'center' },
                    { field: 'name', title: '物料重量', width: 100, align: 'center' },
                    { field: 'name', title: '状态', width: 50, align: 'center' },
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
            $('#dataGridDeliveryOrder').datagrid('reload');
        }
        var currentId = null;
    </script>
</asp:Content>
