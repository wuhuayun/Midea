<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料领料管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridPreparation" title="物料领料管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>仓库：</span><select id="queryPreparation" name="queryPreparation" style="width:120px;"><option selected="selected" value=""></option>
            </select>
            领料单：<select id="Select1" name="queryPreparation" style="width:120px;"><option selected="selected" value=""></option></select>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="Ok();">领料完成</a>
            领料单状态：<input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:120px;" />
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
    </script>
</asp:Content>
