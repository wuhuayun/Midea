<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	自动生成分配任务
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgTask" title="自动生成分配任务" style="" border="false" fit="true" singleSelect="false"
			        idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb">
        </table>
        <div id="tb">
            <span>上传日期：</span><input class="easyui-datebox" id="queryDate" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="Query();">查询</a> 
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-save"
                onclick="GenerateTask();">自动生成分配</a>
        </div>
        <div id="DivJobBom" class="easyui-window" title="作业Bom清单" style="padding: 5px;width:640px;height:480px;"
                        data-options="iconCls: 'icon-search',
                                      closed: true,
                                      maximizable: false,
                                      minimizable: false,
                                      resizable: false,
                                      collapsible: false,
                                      modal: true,
                                      shadow: true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
                                <table id="dgJobBom" class="easyui-datagrid" title="作业日志" style="" border="false" 
                                    data-options="rownumbers: true,
                                                  noheader: true,
                                                  fit: true,
                                                  singleSelect: true,
                                                  idField: 'id',
                                                  sortName: '',
                                                  sortOrder: '',
                                                  striped: true,
                                                  pagination: true,
                                                  pageSize: 30,
                                                  toolbar: '#tb2',
                                                  loadMsg: '加载数据...'">
                                    <thead>
                                        <tr>
                                            <th data-options="field:'id',hidden:true">
                                            </th>
                                            <th data-options="field:'ascmMaterialItem_DocNumber',width:100,align:'center'">
                                                组件
                                            </th>
                                            <th data-options="field:'mpsDateRequiredStr',width:80,align:'center'">
                                                需求日期
                                            </th>
                                            <th data-options="field:'ascmMaterialItem_Description',width:250,align:'left'">
                                                组件说明
                                            </th>
                                            <th data-options="field:'quantityPerAssembly',width:60,align:'center'">
                                                每个装
                                            </th>
                                            <th data-options="field:'wipSupplyTypeCn',width:60,align:'center'">
                                                供应类型
                                            </th>
                                            <th data-options="field:'supplySubinventory',width:80,align:'center'">
                                                子库
                                            </th>
                                            <th data-options="field:'requiredQuantity',width:60,align:'center'">
                                                需求数量
                                            </th>
                                        </tr>
                                    </thead>
		                        </table>
                                <div id="tb2">
                                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="jobBomReload();">刷新</a>
                                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#DivJobBom').window('close');">关闭</a> 
                                </div>
                            </div>
                        </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dgTask').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GenerateLogisticsTaskList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px"><table id="discreteJobsDetail_' + index + '"></table></div>';
                },
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { checkbox: true },
                    { field: 'taskIdCn', title: '任务号', width: 50, align: 'center' }
                ]],
                columns: [[
                    { field: 'dateReleasedCn', title: '作业日期', width: 80, align: 'center' },
                    { field: 'productLine', title: '生产线', width: 50, align: 'center' },
                    { field: 'warehouserId', title: '仓库', width: 90, align: 'center' },
                    { field: 'warehousePlace', title: '仓库位置', width: 220, align: 'left' },
                    { field: '_mtlCategoryStatus', title: '物料类别状态', width: 80, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 90, align: 'center' },
                    { field: 'tipCN', title: '作业内容', width: 60, align: 'center' },
                    { field: 'ascmRanker_name', title: '排产员', width: 80, align: 'center' },
                    { field: 'taskTime', title: '上线时间', width: 70, align: 'center' },
                    { field: 'ascmWorker_name', title: '责任人', align: 'center', width: 80 },
                    { field: '_status', title: '状态', width: 50, align: 'center' },
                    { field: 'uploadDate', title: '上传日期', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    //currentId = rowData.id;
                },
                onExpandRow: function (index, row) {
                    currentId = row.id;
                    $('#discreteJobsDetail_' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadWipDiscreteJobsList/' + row.id,
                        fitColumns: true,
                        singleSelect: true,
                        height: 'auto',
                        columns: [[
	                        { field: 'wipEntityId', title: 'ID', width: 60, hidden: true },
                            { field: 'ascmWipEntities_Name', title: '作业号', width: 75, align: 'center' },
                            { field: 'scheduledStartDateCn', title: '作业日期', align: 'center', width: 60 },
                            { field: 'ascmDiscreteJobs_line', title: '产线', align: 'center', width: 60 },
                            { field: 'ascmMaterialItem_DocNumber', title: '装配件', align: 'center', width: 70 },
                            { field: 'ascmMaterialItem_Description', title: '装配件描述', align: 'left', width: 160 },
                            { field: 'netQuantity', title: '计划数量', align: 'center', width: 60 },
                            { field: 'totalRequiredQuantity', title: 'Bom总数量', align: 'center', width: 40 },
                            { field: 'operate', title: '', align: 'center', width: 40, formatter: jobFormat }
					    ]],
                        onResize: function () {
                            $('#dgTask').datagrid('fixDetailRowHeight', index);
                        },
                        onSelect: function (rowIndex, rowData) {
                            tempId = rowData.wipEntityId;
                        },
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                $('#dgTask').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        }
                    });
                    $('#dgTask').datagrid('fixDetailRowHeight', index);
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });

            //初始化默认日期为当天
            var date = new Date();
            var yyyy = date.getFullYear();
            var MM = date.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            var dd = date.getDate();
            dd = dd < 10 ? "0" + dd : dd;
            $("#queryDate").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        });
    </script>
    <script type="text/javascript">
        function Query() {
            var queryParams = $('#dgTask').datagrid('options').queryParams;
            queryParams.queryDate = $('#queryDate').datebox('getText');

            $('#dgTask').datagrid('reload');
        }
        function GenerateTask() {
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GenerateLogisticsTask';
            var generateTaskDate = $('#queryDate').datebox('getText');
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                data: { "generateTaskDate": generateTaskDate },
                beforeSend: function () {
                    $('#dgTask').datagrid('loading');
                },
                success: function (r) {
                    if (r.result) {
                        Query();
                    } else {
                        $.messager.alert('确认', '生成任务失败:' + r.message, 'error');
                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
        function jobFormat(value, row, index) {
            return "<a href='javascript:void(0)' onclick='loadJobBom(\"" + currentId + "\",\"" + row.wipEntityId + "\");'>详情</a>";
        }
        function loadJobBom(taskId, jobId) {
            $('#DivJobBom').window('open');
            var options = $('#dgJobBom').datagrid('options');
            options.queryParams.taskId = taskId;
            options.queryParams.jobId = jobId;
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LoadJobBom';
            $('#dgJobBom').datagrid('reload');
        }
        function jobBomReload() {
            $('#dgJobBom').datagrid('reload');
        }
    </script>
</asp:Content>
