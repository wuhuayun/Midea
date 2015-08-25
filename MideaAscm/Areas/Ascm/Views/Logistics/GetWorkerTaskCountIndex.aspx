<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	GetWorkerTaskCountIndex
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: false;">
        <table id="dgWorkloadStatistics" title="员工任务统计查询" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'workerId',
                          striped: true,
                          toolbar: '#tb',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GetWorkerTaskCountList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){

                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                     
                          }">
                        <thead>
                            <tr>
                                <th data-options="field:'workerId',width:150,align:'center'">
                                    用户ID
                                </th>
                                <th data-options="field:'ascmWorker_name',width:150,align:'left'">
                                    姓名
                                </th>
                                <th data-options="field:'avgTime',width:250,align:'center'">
                                    平均时长(分钟)
                                </th>
                                <th data-options="field:'taskCount',width:250,align:'center'">
                                    总任务数
                                </th>
                                <th data-options="field:'operate',width:50,align:'center',formatter:taskFormat">
                                    详情
                                </th>
                            </tr>
                        </thead>
                    </table>
        <div id="tb">
            <span>起止日期：</span>
            <input id="queryStartDate" class="easyui-datebox" type="text" style="width: 100px" />-
            <input id="queryEndDate" class="easyui-datebox" type="text" style="width: 100px" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="Search();">查询</a>
        </div>
        <div id="DivTaskList" class="easyui-window" title="任务清单" style="padding: 5px;width:640px;height:480px;"
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
                    <table id="dgTaskList" class="easyui-datagrid" title="任务信息" style="" border="false" 
                        data-options="rownumbers: true,
                                        noheader: true,
                                        fit: true,
                                        singleSelect: true,
                                        idField: 'id',
                                        sortName: 'ascmMaterialItem_DocNumber',
                                        sortOrder: 'asc',
                                        striped: true,
                                        pagination: true,
                                        pageSize: 30,
                                        toolbar: '#tb2',
                                        loadMsg: '加载数据...'">
                        <thead>
                            <tr>
                                <th data-options="field:'id',hidden:true">
                                </th>
                                <th data-options="field:'taskIdCn',width:80,align:'center',sortable:true">
                                    任务号
                                </th>
                                <th data-options="field:'uploadDate',width:70,align:'center'">
                                    生成日期
                                </th>
                                <th data-options="field:'productLine',width:60,align:'center'">
                                    生产线
                                </th>
                                <th data-options="field:'warehouserId',width:60,align:'center'">
                                    所属仓库
                                </th>
                                <th data-options="field:'_mtlCategoryStatus',width:80,align:'center'">
                                    物料类别状态
                                </th>
                                <th data-options="field:'materialDocNumber',width:100,align:'center'">
                                    物料编码
                                </th>
                                <th data-options="field:'tipCn',width:60,align:'center'">
                                    作业内容
                                </th>
                                <th data-options="field:'_status',width:60,align:'center'">
                                    状态
                                </th>
                                <th data-options="field:'starTime',width:150,align:'center'">
                                    开始时间
                                </th>
                                <th data-options="field:'endTime',width:150,align:'center'">
                                    结束时间
                                </th>
                                <th data-options="field:'errorTime',width:200,align:'center'">
                                    异常时间
                                </th>
                            </tr>
                        </thead>
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="taskListReload();">刷新</a>
                        <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#DivTaskList').window('close');">关闭</a> 
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
<script type="text/javascript">
    var currentId = null;
    var currentStartDate = "";
    var currentEndDate = "";

    function taskFormat(value, row, index) {
        return "<a href='javascript:void(0)' onclick='loadTaskDetail(\"" + row.workerId + "\");'>详情</a>";
    }
    function loadTaskDetail(id) {
        $('#DivTaskList').window('open');
        var options = $('#dgTaskList').datagrid('options');
        options.queryParams.workerId = id;
        options.queryParams.queryStartTime = currentStartDate;
        options.queryParams.queryEndTime = currentEndDate;
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadWorkerTaskDetailList';

        $('#dgTaskList').datagrid('reload');
    }
    function taskListReload() {
        $('#dgTaskList').datagrid('reload');
    }
</script>
<script type="text/javascript">
    function Search() {
        var options = $('#dgWorkloadStatistics').datagrid('options').queryParams;
        options.queryStartTime = $('#queryStartDate').datebox('getText');
        options.queryEndTime = $('#queryEndDate').datebox('getText');

        currentStartDate = $('#queryStartDate').datebox('getText');
        currentEndDate = $('#queryEndDate').datebox('getText');

        $('#dgWorkloadStatistics').datagrid('reload');
    }
</script>
</asp:Content>
