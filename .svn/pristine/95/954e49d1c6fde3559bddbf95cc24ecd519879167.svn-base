<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业退料查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dataGridWmsJobMtlReturn" title="作业退料查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
		<div id="tb1">
			<div style="margin-bottom:5px;">
				单据号：<input id="queryWord" type="text" style="width:130px;" />
				作业号：<input id="wipEntityName" type="text" style="width:130px;" />
				<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
				<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
			</div>
			<div style="margin-bottom:5px;">
				<span>更新时间：</span>
				<input class="easyui-datebox" id="queryStartModifyTime" type="text" style="width:100px;" value="<%=ViewData["dtStart"] %>" />-<input class="easyui-datebox" id="queryEndModifyTime" type="text" style="width:100px;" value="<%=ViewData["dtEnd"] %>" />
				
				<span> 上传状态： </span>
			    <select id="queryReturnCode" name="queryReturnCode" style="width:80px;">
					<option value="" selected="selected">所有</option>
                    <option value="0" >正常</option>
					<option value="-1" >异常</option>
                </select>
			</div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
 <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridWmsJobMtlReturn').datagrid({
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '退料单号', width: 150, align: 'center' },
                    { field: 'wipEntityName', title: '作业号', width: 150, align: 'center' }
                ]],
                columns: [[
                    { field: 'warehouseId', title: '退料子库', width: 100, align: 'center' },
                    { field: 'returnAreaCn', title: '退料区域', width: 100, align: 'center' },
                    { field: 'reasonName', title: '退货原因', width: 150, align: 'center' },
                    { field: 'modifyTime', title: '修改时间', width: 150, align: 'center' },
                    { field: 'meno', title: '备注', width: 300, align: 'center' },
					{ field: 'uploadStatusCn', title: '上传状态', width: 100, align: 'center' },
					{ field: 'uploadTimeShow', title: '上传日期', width: 100, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    WmsJobMtlReturnMainView();
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function Query() {
            var options = $('#dataGridWmsJobMtlReturn').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsJobMtlReturnMainList';
            options.queryParams.queryWord = $('#queryWord').val();
            options.queryParams.wipEntityName = $('#wipEntityName').val();
            options.queryParams.startPlanTime = $('#queryStartModifyTime').datebox('getText');
            options.queryParams.endPlanTime = $('#queryEndModifyTime').datebox('getText');
			options.queryParams.returnCode = $('#queryReturnCode').val();
            $('#dataGridWmsJobMtlReturn').datagrid('reload');
        }
        var currentId = null;
        function WmsJobMtlReturnMainView() {
            var selectRow = $('#dataGridWmsJobMtlReturn').datagrid('getSelected');
            if (selectRow) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsJobMtlReturnMainView/' + selectRow.id+"?mi=<%=Request["mi"].ToString() %>";
                parent.openTab('作业退料单_' + selectRow.docNumber, url);
            } else {
                $.messager.alert('提示', '请选择作业退料单！', 'info');
            }
        }
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/WmsJobMtlReturnPrint.aspx';
            url += "?docNumber=" + $('#queryWord').val();
            url += "&wipEntityName=" + $('#wipEntityName').val();
            url += "&queryStartModifyTime=" + $("#queryStartModifyTime").datebox('getText');
            url += "&queryEndModifyTime=" + $("#queryEndModifyTime").datebox('getText');
            parent.openTab('作业退料查询打印', url);
        }
    </script>
</asp:Content>
