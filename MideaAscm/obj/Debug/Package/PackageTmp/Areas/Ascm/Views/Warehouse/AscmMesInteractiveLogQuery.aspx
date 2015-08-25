<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    MES接口日志查询
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dataGridMesInter" title="MES接口日志查询" style="" border="false" fit="true"
            singleselect="true" idfield="id" sortname="sortNo" sortorder="asc" striped="true"
            toolbar="#tb1">
        </table>
        <div id="tb1">
            <div style="margin-bottom:5px;">
                单据号：<input id="docNumber" type="text" style="width: 100px;" />
				更新时间：<input class="easyui-datebox" id="queryStartModifyTime" type="text" style="width: 100px;"
					value="<%=ViewData["dtStart"] %>" />-<input class="easyui-datebox" id="queryEndModifyTime"
						type="text" style="width: 100px;" value="<%=ViewData["dtEnd"] %>" />
				<span>单据类型：</span>
				<select id="queryBillType" name="queryBillType" style="width: 150px;">
					<option value=""></option>
					<% List<string> list = MideaAscm.Dal.Warehouse.Entities.AscmMesInteractiveLog.BillTypeDefine.GetList(); %>
					<% if (list != null && list.Count > 0)
					   { %>
					<% foreach (string sValue in list)
					   { %>
					<option value="<%=sValue %>">
						<%=MideaAscm.Dal.Warehouse.Entities.AscmMesInteractiveLog.BillTypeDefine.DisplayText(sValue)%></option>
					<% } %>
					<% } %>
				</select>
            </div>
			<div style="margin-bottom:5px;">
				<span>是否异常：</span>
			    <select id="queryReturnCode" name="queryReturnCode" style="width:80px;">
                    <option value="-1" selected="selected">异常</option>
                    <option value="0">正常</option>
                </select>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a> 
				<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ExportExcel();">导出</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridMesInter').datagrid({
                rownumbers: true,
                idField: 'id',
                sortName: 'modifyTime',
                sortOrder: 'desc',
                striped: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '单据号', width: 120, align: 'center' }
                ]],
                columns: [[
                    { field: 'billTypeCn', title: '单据类型', width: 115, align: 'center' },
                    { field: 'returnCode', title: '返回代码', width: 200, align: 'center' },
                    { field: 'returnMessage', title: '返回信息', width: 230, align: 'left' },
                    { field: 'modifyTime', title: '更新时间', width: 120, align: 'center' },
                    { field: 'modifyUser', title: '更新人', width: 80, align: 'center' },
                ]],
                onSelect: function (rowIndex, rowData) {
                },
                onDblClickRow: function (rowIndex, rowData) {
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                }
            });
        });
        function Query() {
            var options = $('#dataGridMesInter').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/AscmMesInteractiveLogList';
            options.queryParams.queryWord = $('#docNumber').val();
            options.queryParams.startPlanTime = $('#queryStartModifyTime').datebox('getText');
            options.queryParams.endPlanTime = $('#queryEndModifyTime').datebox('getText');
            options.queryParams.billType = $('#queryBillType').val();
            options.queryParams.returnCode = $('#queryReturnCode').val();
            $('#dataGridMesInter').datagrid('reload');
        }
        function ExportExcel() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/AscmMesInteractiveLogExport/';
            var params = { 
                queryWord: $('#docNumber').val(), 
                startPlanTime: $('#queryStartModifyTime').datebox('getText'),
                endPlanTime: $('#queryEndModifyTime').datebox('getText'),
                billType: $('#queryBillType').val()  
            };
            var iframe = document.createElement("iframe");
            iframe.src = url + "?" + $.param(params);
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>
</asp:Content>
