<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	货位转移查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridLocationTransfer" title="货位转移" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            修改时间：<input class="easyui-datebox" id="queryStartModifyTime" type="text" style="width:100px;" value="<%=ViewData["dtStart"] %>" />-<input class="easyui-datebox" id="queryEndModifyTime" type="text" style="width:100px;" value="<%=ViewData["dtEnd"] %>" />
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Search();"></a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridLocationTransfer').datagrid({
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'materialDocNumber', title: '物料编码', width: 120, align: 'center' },
                    { field: 'materialName', title: '物料名称', width: 300, align: 'left' },
                    { field: 'quantity', title: '物料数量', width: 80, align: 'center' },
                    { field: 'fromLocationDocNumber', title: '来源货位', width: 110, align: 'center' },
                    { field: 'toLocationDocNumber', title: '目标货位', width: 110, align: 'center' },
                    { field: 'operateTypeCn', title: '操作方式', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
//                    if (currentId) {
//                        $(this).datagrid('selectRecord', currentId);
//                    }
                }
            });
        });
        function Search() {
            var options = $('#dataGridLocationTransfer').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationTransferList';
            options.queryParams.startModifyTime = $('#queryStartModifyTime').datebox('getText');
            options.queryParams.endModifyTime = $('#queryEndModifyTime').datebox('getText');
            $('#dataGridLocationTransfer').datagrid('reload');
        }
    </script>
</asp:Content>
