<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	未关联物料查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dataGridMaterial" title="" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            物料编号：<input id="docNumber" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
        </div>
    </div>
</asp:Content>

 <asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
 <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridMaterial').datagrid({
                url :'<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MaterialNotRelatedList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '编号', width: 150, align: 'center' },
                    { field: 'name', title: '名称', width: 150, align: 'center' },
                    { field: 'unit', title: '物料计量单位', width: 100, align: 'center' },
                    { field: 'description', title: '描述', width: 500, align: 'left' }
                    
                ]],
                onSelect: function (rowIndex, rowData) {
                },
                onDblClickRow: function (rowIndex, rowData) {
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    $(this).datagrid('clearChecked');
                }
            });
        });
        function Query() {
            var options = $('#dataGridMaterial').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MaterialNotRelatedList';
            options.queryParams.queryWord = $('#docNumber').val();
            $('#dataGridMaterial').datagrid('reload');
        }
    </script>
</asp:Content>
