<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	盈亏管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridProfitAndLoss" title="盈亏登记管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库：</span><select id="queryReturnedMaterial" name="queryReturnedMaterial" style="width:120px;"><option selected="selected" value=""></option>
            </select>
            时间：<input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;" />--
            <input class="easyui-datebox" id="queryEndTime" type="text" style="width:100px;" />
            <span>状态：</span><select id="Select1" name="queryReturnedMaterial" style="width:120px;"><option selected="selected" value=""></option>
            </select>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Search();"></a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
            <%} %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印盘点表</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridProfitAndLoss').datagrid({
                url: '',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '编号', width: 80, align: 'center' },
                    { field: 'name', title: '子库名称', width: 80, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '时间', width: 150, align: 'center' },
                    { field: 'name', title: '状态', width: 70, align: 'center' },
                    { field: 'name', title: '仓管员', width: 80, align: 'center' },
                    { field: 'name', title: '批准', width: 80, align: 'center' },
                    { field: 'name', title: '备注', width: 200, align: 'left' }
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
            $('#dataGridProfitAndLoss').datagrid('reload');
        }
        function Search() {
            var queryParams = $('#dataGridProfitAndLoss').datagrid('options').queryParams;
            queryParams.queryWord = $('#Search').val();
            $('#dataGridProfitAndLoss').datagrid('reload');
        }
        var currentId = null;
        function Edit() {
//            var selectRow = $('#dataGridProfitAndLoss').datagrid('getSelected');
//            if (selectRow) {
var id="123";
//id=selectRow.id;
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/ProfitAndLossEdit/' + id+"?mi=<%=Request["mi"].ToString() %>";
                parent.openTab('盈亏单_' + id, url);
//                currentId = selectRow.id;
//            } else {
//                $.messager.alert('提示', '请选择供应商', 'info');
//            }
        }
    </script>
</asp:Content>
