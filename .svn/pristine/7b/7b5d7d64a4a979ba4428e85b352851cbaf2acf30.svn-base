<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	读头日志查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgReadingHeadLog" title="读头日志查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="" sortOrder="" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            <span>时间：</span><input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;"  value="<%= DateTime.Now.ToString("yyyy-MM-dd")%>"/>-<input class="easyui-datebox" id="queryEndTime" type="text" style="width:100px;" value="<%=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") %>"/>
            <input id="search" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
			<%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>--%>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#dgReadingHeadLog').datagrid({
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' }
                ]],
                columns: [[
                    { field: 'readingHeadIp', title: '读头IP', width: 100, align: 'center' },
                    { field: 'readingHeadPort', title: '读头端口', width: 70, align: 'center' },
                    { field: 'rfid', title: 'RFID', width: 180, align: 'center' },
                    { field: 'sn', title: 'RFID标签号', width: 100, align: 'center' },
                    { field: 'createTimeShow', title: '时间', width: 120, align: 'left' },
                    { field: 'processedName', title: '是否处理', width: 120, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
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
            var options = $('#dgReadingHeadLog').datagrid('options');
            options.url = "<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/ReadingHeadLogList/";
            options.queryParams.queryWord = $('#search').val();
            options.queryParams.queryStartTime = $("#queryStartTime").datebox('getText');
            options.queryParams.queryEndTime = $("#queryEndTime").datebox('getText');
            $('#dgReadingHeadLog').datagrid('reload');
        }
        var currentId = null;
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/ReadingHeadLogPrint.aspx';
            url += "?queryStartTime=" + $("#queryStartTime").datebox('getText');
            url += "&queryEndTime=" + $("#queryEndTime").datebox('getText');
            url += "&queryWord=" + $('#search').val();
            parent.openTab('读头日志打印', url);
        }
    </script>
</asp:Content>
