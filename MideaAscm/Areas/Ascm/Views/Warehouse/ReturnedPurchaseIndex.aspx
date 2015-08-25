<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	退货管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridReturnedPurchase" title="退货管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库：</span><select id="queryReturnedPurchase" name="queryReturnedPurchase" style="width:120px;"><option selected="selected" value=""></option>
            </select>
            时间：<input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;" />--
            <input class="easyui-datebox" id="queryEndTime" type="text" style="width:100px;" />
            <span>状态：</span><select id="Select1" name="queryReturnedPurchase" style="width:120px;"><option selected="selected" value=""></option>
            </select>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">退货处理</a>
            <%} %>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridReturnedPurchase').datagrid({
                url: '',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'name', title: '编号', width: 80, align: 'center' },
                ]],
                columns: [[
                    { field: 'name', title: '供应商', width: 100, align: 'center' },
                    { field: 'name', title: '类型', width: 100, align: 'center' },
                    { field: 'name', title: '日期', width: 100, align: 'center' },
                    { field: 'name', title: '状态', width: 100, align: 'center' },
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
            $('#dataGridReturnedPurchase').datagrid('reload');
        }
        function Search() {
            var queryParams = $('#dataGridReturnedPurchase').datagrid('options').queryParams;
            queryParams.queryWord = $('#Search').val();
            $('#dataGridReturnedPurchase').datagrid('reload');
        }
        var currentId = null;
        function Edit() {
//            var selectRow = $('#dataGridReturnedPurchase').datagrid('getSelected');
//            if (selectRow) {
var id="123";
//id=selectRow.id;
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/ReturnedPurchaseEdit/' + id+"?mi=<%=Request["mi"].ToString() %>";
                parent.openTab('退货单_' + id, url);

//                currentId = selectRow.id;
//            } else {
//                $.messager.alert("提示", "请先选中要编辑的一行数据！", "info");
//            }
        }
    </script>
</asp:Content>
