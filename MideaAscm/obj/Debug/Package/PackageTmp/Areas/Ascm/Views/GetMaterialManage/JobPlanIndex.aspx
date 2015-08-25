<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	领料作业计划管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: false;">
        <table id="dgTasks" title="领料作业计划管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: false,
                          singleSelect : true,
                          idField: 'taskId',
                          sortName: 'taskId',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: false,
                          loadMsg: '更新数据......'"
                          >
            <thead data-options="frozen:true">
                <tr>
                    <%--<th data-options="checkbox:true"></th>--%>
                    <th data-options="field:'taskId',width:100,align:'center',hidden:true">任务号</th>
                    <th data-options="field:'taskIdCn',width:100,align:'center'">任务号</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'productLine',width:80,align:'center'">生产线</th>
                    <th data-options="field:'warehouserId',width:80,align:'center'">所属仓库</th>
                    <th data-options="field:'warehouserPlace',width:160,align:'center'">仓库位置</th>
                    <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                    <th data-options="field:'materialTypeCN',width:80,align:'center'">物料编码类型</th>
                    <th data-options="field:'categoryStatusCN',width:80,align:'center'">物料类别状态</th>
                    <th data-options="field:'uploadDate',width:80,align:'center'">上传日期</th>
                    <th data-options="field:'taskTime',width:80,align:'center'">上线时间</th>
                    <th data-options="field:'rankManName',width:80,align:'center'">排产员</th>
                </tr>
            </thead>
        </table>
        <div id="tb1">
            <%--<span>作业号：</span>
                <input id="queryJobId" name="queryJobId" type="text" style="width:100px;text-align:right;" />--%>
            
            <span>日期：</span><input class="easyui-datebox" id="queryDate" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">合成作业</a>
        </div>
        <div id="materialDiv" class="easyui-window" title="作业物料清单" style="padding: 0px;width:780px;height:380px;"
			closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			
            <div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:0px;background:#fff;border:1px solid #ccc;">
                    <form id="materialForm" method="post" style="" fit="true" >
                        <ul id="materialList" title="" style="height:320px;" border="false" singleSelect="true"
			                    idField="id" striped="true">
		                </ul>
			        </form>
				</div>
			</div>
		</div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <script type="text/javascript">
        var queryData = null;
        function Query() {
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/GetTasksList/';
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                data: { jobDate: $("#queryDate").datebox('getText') },
                success: function (r) {
                    if (r.rows) {
                        queryData = r.rows;
                        $.messager.alert('确认', '任务合并成功！', 'info');
                        $('#dgTasks').datagrid('loadData', r.rows);
                    }
                }
            });
        };

        $(function(){
            $('#dgTasks').datagrid({
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px"><table id="taskDetail_' + index + '"></table></div>';
                },
                onExpandRow: function (index, row) {
                    $('#taskDetail_' + index).datagrid({
                        fitColumns: true,
                        singleSelect: true,
                        height: 'auto',
                        columns: [[
						    { field: 'jobId', title: '作业号', width: 80},
						    { field: 'jobDate', title: '作业日期', width: 80 },
						    { field: 'jobInfoId', title: '装配件', width: 80 },
						    { field: 'jobDesc', title: '装配件描述', width: 80 },
						    { field: 'count', title: '数量', width: 80 },
                            { field: 'productLine', title: '生产线', width: 80 },
                            { field: 'onlineTime', title: '上线时间', width: 80 }, //"<div id=\"ggg\" onclick=\"alert(\'hello\')\">";
                            { field: 'tip2', title: '', width: 70, formatter: function (value, row_job, index) {var e = '<span onclick=' + '"materialQuery(' + "'" + row.taskId + "',"+ "'" + row_job.jobId + "'" + ');"' + '>详情</span>'; return e; } }
					    ]],
                        onResize: function () {
                            $('#dgTasks').datagrid('fixDetailRowHeight', index);
                        },
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                $('#dgTasks').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        }
                    });
                    $('#taskDetail_' + index).datagrid('loadData',row.ascmDiscreteJobsList);
                    $('#dgTasks').datagrid('fixDetailRowHeight', index);
                }
            });

            $('#materialList').treegrid({
                pagination: false,
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'id', title: 'ID', width: 80, align: 'center' },
					{ field: 'mpsDateRequired', title: '需求日期', width: 120, align: 'center' },
                    { field: 'materialDocNumber', title: '组件', width: 100, align: 'center' },
                    { field: 'materialDescription', title: '组件说明', width: 200, align: 'center' },
                    { field: 'wipSupplyType', title: '类型', width: 100, align: 'center' },
                    { field: 'supplySubinventory', title: '子库存', width: 100, align: 'center' },
                    { field: 'quantityPerAssembly', title: '每个装', width: 100, align: 'center' },
                    { field: 'requiredQuantity', title: '必需', width: 100, align: 'center' }
                ]]
            });

            <%-- 初始化默认日期为当天 --%>
            var date = new Date();
            var yyyy = date.getFullYear();
            var MM = date.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            var dd = date.getDate();
            dd = dd < 10 ? "0" + dd : dd;
            $("#queryDate").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        });

        function materialQuery(taskId,jobId)
        {
            for(var i in queryData)
            {
                if(queryData[i].taskId == taskId)
                {
                    for(var j in queryData[i].ascmDiscreteJobsList)
                    {
                        if(queryData[i].ascmDiscreteJobsList[j].jobId == jobId)
                        {
                            var materialList = queryData[i].ascmDiscreteJobsList[j].listAscmWipBom;
                            $('#materialList').treegrid('loadData',materialList);
                            $('#materialDiv').window('open');
                        }
                    }
                }
            }
        };
    </script>
</asp:Content>
