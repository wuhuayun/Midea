<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    工作量统计
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgTaskStatistics" url="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/TaskStatisticsList"
            idfield="workerId" sortorder="desc" title="工作量统计" singleselect="true" toolbar="#tb1" autoRowHeight="true"
            fitcolumns="true"  striped="true">
            <thead>
                <tr>
                    <th field="workerId" width="80">
                        用户ID
                    </th>
                    <th field="userName" width="100">
                        姓名
                    </th>
                    <th field="avgTime" align="right" width="80">
                        平均任务时长
                    </th>
                    <th field="taskNumber" align="right" width="80">
                        总任务数
                    </th>
                </tr>
            </thead>
        </table>
        <div id="tb1">
            <span>统计方式：</span>
            <select id="sltWay" style="width: 100px;">
                <option value="wk">星期</option>
                <option value="mm">月</option>
            </select>
            <span>统计时间段：</span>
            <input id="txtSatTime" class="easyui-datebox" data-options="formatter:myformatter,parser:myparser"
                style="width: 150px" />
            <span>~</span>
            <input id="txtEndTime" class="easyui-datebox" data-options="formatter:myformatter,parser:myparser"
                style="width: 150px" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="Search();">查询</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <script type="text/javascript">
        function myformatter(date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
        }
        function myparser(s) {
            if (!s) return new Date();
            var ss = (s.split('-'));
            var y = parseInt(ss[0], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[2], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d);
            } else {
                return new Date();
            }

        }  
    </script>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dgTaskStatistics').datagrid({
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px"><table id="StatisticsDetail_' + index + '"></table></div>';
                },
                onExpandRow: function (index, row) {
                    $('#StatisticsDetail_' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/TaskDetailStatisticsList',
                        fitColumns: true,
                        queryParams: {
                            workerId: row.workerId.toString(),
                            userName: row.userName.toString()
                        },
                        singleSelect: true,
                        height: 'auto',
                        columns: [[
						    { field: 'id', title: 'id', width: 20, hidden: true },
                            { field: 'strUserName', title: '责任人', align: 'left', width: 80 },
                            { field: 'taskId', title: '任务号', align: 'left', width: 80 },
                            { field: 'strUsedTime', title: '任务时长', align: 'left', width: 80 },
                            { field: 'warehouserId', title: '所属仓库', align: 'left', width: 250 }
					    ]],
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                $('#dgTaskStatistics').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        },
                        onResize: function () {
                            $('#dgTaskStatistics').datagrid('fixDetailRowHeight', index);
                        }
                    });
                    $('#dgTaskStatistics').datagrid('fixDetailRowHeight', index);
                }
            });
        });
        function Search() {
            var options = $('#dgTaskStatistics').datagrid('options');
            options.queryParams.queryWord = $('#sltWay').val();
            options.queryParams.strSatTime = $('#txtSatTime').datebox('getValue');
            options.queryParams.strEndTime = $('#txtEndTime').datebox('getValue');
            $('#dgTaskStatistics').datagrid('reload');
        }
    </script>
</asp:Content>
