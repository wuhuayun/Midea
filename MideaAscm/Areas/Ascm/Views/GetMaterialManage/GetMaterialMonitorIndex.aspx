<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	领料作业监控管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgMonitorTask" title="领料作业监控列表" style="" border="false" fit="true" singleSelect="false"
			    idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb">
        </table>
        <div id="tb" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>起止日期：</span><input class="easyui-datebox" id="queryStartDate" type="text" style="width:130px;" /> ~
                <input class="easyui-datebox" id="queryEndDate" type="text" style="width:130px;" />
                <span>作业日期：</span><input class="easyui-datebox" id="queryStartJobDate" type="text" style="width:130px;" /> ~
                <input class="easyui-datebox" id="queryEndJobDate" type="text" style="width:130px;" />
                <span>&nbsp;&nbsp;&nbsp;&nbsp;作业号：</span>
                <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewWipEntitiesSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWipEntity", width = "150px", panelWidth = 500 });%></span>
            </div>

            <div style="margin-bottom:5px;">
                <span>任务类型：</span>
                <select id="queryType" name="queryType" style="width: 100px;">
                    <option value=""></option>
                    <% List<int> listTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.GetList(); %>
                    <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                        { %>
                    <% foreach (int statusDefine in listTypeDefine)
                        { %>
                    <option value="<%=statusDefine %>">
                        <%=MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.DisplayText(statusDefine)%></option>
                    <% } %>
                    <% } %>
                </select>
                <span>&nbsp;备料形式：</span>
                <select id="queryFormat" name="queryFormat" style="width: 100px;">
                    <option value=""></option>
                    <option value="SPECWAREHOUSE">特殊子库</option>
                    <option value="TEMPTASK">临时任务</option>
                    <% List<string> listStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                    <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                        { %>
                    <% foreach (string statusDefine in listStatusDefine)
                        { %>
                        <% if (statusDefine != "INSTOCK") %>
                        <% { %>
                    <option value="<%=statusDefine %>">
                        <%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(statusDefine)%></option>
                        <% } %>
                    <% } %>
                    <% } %>
                </select>
                <span>任务状态：</span>
                <select id="queryStatus" name="queryStatus" style="width: 110px;">
                    <option value=""></option>
                    <% listStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.StatusDefine.GetList(); %>
                    <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                        { %>
                    <% foreach (string statusDefine in listStatusDefine)
                        { %>
                        <% if (statusDefine != "NOTALLOCATE") %>
                        <% { %>
                    <option value="<%=statusDefine %>">
                    <%=MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.StatusDefine.DisplayText(statusDefine)%></option>
                        <% } %>
                    <% } %>
                    <% } %>
                </select>
                <span>生产线：</span>
                <select id="queryLine" name="queryLine" style="width: 108px;">
                    <option value=""></option>
                    <% List<string> listLineDefine = MideaAscm.Services.GetMaterialManage.AscmDiscreteJobsService.GetInstance().GetLineList(); %>
                    <% if (listLineDefine != null && listLineDefine.Count > 0)
                        { %>
                    <% foreach (string lineDefine in listLineDefine)
                        { %>
                    <option value="<%=lineDefine %>">
                        <%=lineDefine %></option>
                    <% } %>
                    <% } %>
                </select>

                <span>&nbsp;&nbsp;&nbsp;&nbsp;仓&nbsp;&nbsp;&nbsp;库：</span>
                <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewWarehouseIdSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWarehouse", width = "150px" }); %></span>
                
            </div>
            
            <div style="margin-bottom:5px;">
                <% if (ynWebRight.rightVerify)
                    { %>
                <span>责任人：</span>
                <span><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerName", panelWidth = 500, width = "150px" }); %></span>
                <%} %>

                <% if (ynWebRight.rightEdit)
                   { %>
                <a id="btnStartTask" name="btnStartTask" href="javascript:void(0);"class="easyui-linkbutton" 
                    plain="true" icon="icon-run" onclick="RunTask();">开始任务</a>
                 <%} %>

                <% if (ynWebRight.rightEdit)
                   { %>
                <%--<a id="btnGetMaterial" name="btnGetMaterial" href="javascript:void(0);"class="easyui-linkbutton" 
                    plain="true" icon="icon-save" onclick="GetMaterial();">确认领料</a>--%>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="GetBatchMaterial();">确认领料</a>
                  <%} %>

                <% if (ynWebRight.rightEdit)
                   { %>
                <a id="btnEndTask" name="btnEndTask" href="javascript:void(0);" class="easyui-linkbutton"
                    plain="true" icon="icon-stop" onclick="StopTask();">结束任务</a>
                 <%} %>

                 <% if (ynWebRight.rightEdit)
                   { %>
                <a id="btnMark" name="btnMark" href="javascript:void(0);" class="easyui-linkbutton"
                    plain="true" icon="icon-pin-red" onclick="MarkTask();">标记</a>
                 <%} %>

                 <% if (ynWebRight.rightEdit)
                   { %>
                <a id="btnUnMark" name="btnUnMark" href="javascript:void(0);" class="easyui-linkbutton"
                    plain="true" icon="icon-pin-blue" onclick="UnMarkTask();">取消标记</a>
                 <%} %>

                 <% if (ynWebRight.rightVerify)
                    { %>
                <a id="btnCloseTask" name="btnCloseTask" href="javascript:void(0);" class="easyui-linkbutton"
                    plain="true" icon="icon-lightning" onclick="CloseGetMaterialTask();">关闭</a>
                   <%} %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-message" onclick="MessageInfo();">领料通知</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a> 
            </div>
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
                                            <th data-options="field:'docNumber',width:100,align:'center',sortable:true">
                                                组件
                                            </th>
                                            <th data-options="field:'mpsDateRequiredStr',width:80,align:'center',sortable:true">
                                                需求日期
                                            </th>
                                            <th data-options="field:'description',width:200,align:'left'">
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
                                            <th data-options="field:'transactionQuantity',width:60,align:'center'">
                                                现有数量
                                            </th>
                                            <th data-options="field:'wmsPreparationQuantity',width:60,align:'center'">
                                                备料数量
                                            </th>
                                            <th data-options="field:'getMaterialQuantity',width:60,align:'center'">
                                                领料数量
                                            </th>
                                            <th data-options="field:'quantityGetMaterialDifference',width:70,align:'center'">
                                                领料差异数量
                                            </th>
                                        </tr>
                                    </thead>
		                        </table>
                                <div id="tb2">
                                        <span>仓库：</span>
                                        <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewWarehouseIdSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryBomWarehouse", width = "150px", panelWidth = 500 });%></span>
                                        <span>物料大类：</span>
                                        <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewMaterialCategorySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryBomMtlCategory", width = "150px", panelWidth = 500 });%></span>
                                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="QueryBom();">查询</a> 
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
        var currentStatus = null;
        var tempId = null;
        var expandRowObject = "";
        $(function () {
            $('#dgMonitorTask').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/MonitorTaskList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px; width:80%"><table id="discreteJobsDetail_' + index + '"></table></div>';
                },
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { checkbox: true },
                    { field: 'markTaskOperate', title: '', align: 'center', width: 30, formatter: taskFormat }
                ]],
                columns: [[
                    { field: 'taskIdCn', title: '任务号', width: 50, align: 'center' },
                    { field: 'dateReleasedCn', title: '作业日期', width: 60, align: 'center' },
                    { field: 'productLine', title: '生产线', width: 50, align: 'center' },
                    { field: 'warehouserId', title: '仓库', width: 70, align: 'center' },
                    { field: 'warehousePlace', title: '仓库位置', width: 100, align: 'center' },
                    { field: '_mtlCategoryStatus', title: '物料类别状态', width: 80, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 90, align: 'center' },
                    { field: 'tipCN', title: '作业内容', width: 60, align: 'center' },
                    { field: 'taskTime', title: '上线时间', width: 70, align: 'center' },
                    { field: 'totalRequiredQuantity', title: '需求数量', align: 'center', width: 90 },
                    { field: 'totalWmsPreparationQuantity', title: '备料数量', align: 'center', width: 90 },
                    { field: 'totalGetMaterialQuantity', title: '领料数量', align: 'center', width: 90 },
                    { field: 'ascmWorker_name', title: '责任人', align: 'center', width: 80 },
                    { field: '_status', title: '状态', width: 50, align: 'center' },
                    { field: 'uploadDate', title: '上传日期', width: 80, align: 'center' },
                    { field: '_starTime', title: '开始时间', width: 120, align: 'center' },
                    { field: '_endTime', title: '结束时间', width: 120, align: 'center' },
                    { field: 'which', title: '第几次', width: 50, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                    if (expandRowObject.indexOf(rowData.id) > -1)
                        $('#discreteJobsDetail_' + rowIndex).datagrid("selectAll");
                },
                onUnselect: function (rowIndex, rowData) {
                    if (expandRowObject.indexOf(rowData.id) > -1)
                        $('#discreteJobsDetail_' + rowIndex).datagrid("unselectAll");
                },
                rowStyler: function (index, row) {
                    if (row._status == "已完成") {
                        return 'color:gray;';
                    }
                    else if (row._status == "执行中") {
                        return 'color:blue;';
                    }
                },
                onExpandRow: function (index, row) {
                    currentId = row.id;
                    if (expandRowObject != null && expandRowObject != "")
                        expandRowObject += ",";
                    expandRowObject += index + "[" + row.id + "]";

                    $('#discreteJobsDetail_' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/MonitorJobList/' + row.id,
                        fitColumns: true,
                        rownumbers: true,
                        //singleSelect: true,
                        height: 'auto',
                        columns: [[
                            { checkbox: true },
                            { field: 'markJobOperate', title: '', align: 'center', width: 20, formatter: jobMarkFormat },
                            { field: 'wipEntityId', title: 'ID', width: 60, hidden: true },
                            { field: 'ascmWipEntities_Name', title: '作业号', width: 75, align: 'center' },
                            { field: 'scheduledStartDateCn', title: '作业日期', align: 'center', width: 60 },
                            { field: 'ascmDiscreteJobs_line', title: '产线', align: 'center', width: 60 },
                            { field: 'ascmMaterialItem_DocNumber', title: '装配件', align: 'center', width: 70 },
                            { field: 'ascmMaterialItem_Description', title: '装配件描述', align: 'left', width: 160 },
                            { field: 'netQuantity', title: '计划数量', align: 'center', width: 60 },
                            { field: 'totalRequiredQuantity', title: 'Bom总数量', align: 'center', width: 60 },
                            { field: 'totalPreparationQuantity', title: 'Bom备料数量', align: 'center', width: 60 },
                            { field: 'totalGetMaterialQuantity', title: 'Bom领料数量', align: 'center', width: 60 },
                            { field: 'operate', title: '', align: 'center', width: 50, formatter: jobFormat },
                            { field: 'totalSumQuantity', title: '备料+领料', align: 'center', width: 50, hidden: true }
					    ]],
                        onResize: function (index, row) {
                            $('#dgMonitorTask').datagrid('fixDetailRowHeight', index);
                        },
                        onSelect: function (rowIndex, rowData) {
                            tempId = rowData.wipEntityId;
                        },
                        rowStyler: function (index, row) {
                            if (row.totalRequiredQuantity - row.totalSumQuantity > 0 && row.totalRequiredQuantity - row.totalGetMaterialQuantity > 0) {
                                return 'color:red;';
                            }
                            else if (row.totalRequiredQuantity - row.totalSumQuantity == 0 && row.totalRequiredQuantity - row.totalGetMaterialQuantity > 0) {
                                return 'color:black;';
                            }
                            else if (row.totalRequiredQuantity - row.totalSumQuantity == 0 && row.totalRequiredQuantity - row.totalGetMaterialQuantity == 0) {
                                return 'color:gray;';
                            }
                        },
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                $('#dgMonitorTask').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        }
                    });
                    $('#dgMonitorTask').datagrid('fixDetailRowHeight', index);
                },
                onCollapseRow: function (index, row) {
                    var str = index + "[" + row.id + "]";
                    expandRowObject = expandRowObject.replace(str, '');
                },
                onLoadSuccess: function (data) {
//                    if (currentStatus != null && currentStatus != "") {
//                        $(this).datagrid('clearSelections');
//                    }

                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        //$(this).datagrid('selectRecord', currentId);
                    }
                }
            })

            //MessageInfo();
        });
        function Query() {
            var options = $('#dgMonitorTask').datagrid('options').queryParams;
            options.queryStatus = $('#queryStatus').val();
            options.queryLine = $('#queryLine').val();
            options.queryType = $('#queryType').val();
            options.queryStartDate = $('#queryStartDate').datebox('getText');
            options.queryEndDate = $('#queryEndDate').datebox('getText');
            options.queryStartJobDate = $('#queryStartJobDate').datebox('getText');
            options.queryEndJobDate = $('#queryEndJobDate').datebox('getText');
            options.queryWarehouse = $('#queryWarehouse').combogrid('getValue');
            options.queryFormat = $('#queryFormat').val();
            options.queryWipEntity = $('#queryWipEntity').combogrid('getValue');
            if ($('#queryFormat').val() == 'MIXSTOCK') {
                $('#dgMonitorTask').datagrid('hideColumn', 'tipCN');
                $('#dgMonitorTask').datagrid('hideColumn', 'materialDocNumber');
                $('#dgMonitorTask').datagrid('showColumn', '_mtlCategoryStatus');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalRequiredQuantity');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalWmsPreparationQuantity');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalGetMaterialQuantity');
            }
            else if ($('#queryFormat').val() == 'PRESTOCK') {
                $('#dgMonitorTask').datagrid('hideColumn', 'tipCN');
                $('#dgMonitorTask').datagrid('showColumn', 'materialDocNumber');
                $('#dgMonitorTask').datagrid('showColumn', '_mtlCategoryStatus');
                $('#dgMonitorTask').datagrid('showColumn', 'totalRequiredQuantity');
                $('#dgMonitorTask').datagrid('showColumn', 'totalWmsPreparationQuantity');
                $('#dgMonitorTask').datagrid('showColumn', 'totalGetMaterialQuantity');
            }
            else if ($('#queryFormat').val() == 'SPECWAREHOUSE') {
                $('#dgMonitorTask').datagrid('hideColumn', 'tipCN');
                $('#dgMonitorTask').datagrid('hideColumn', 'materialDocNumber');
                $('#dgMonitorTask').datagrid('hideColumn', '_mtlCategoryStatus');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalRequiredQuantity');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalWmsPreparationQuantity');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalGetMaterialQuantity');
            }
            else if ($('#queryFormat').val() == 'TEMPTASK') {
                $('#dgMonitorTask').datagrid('showColumn', 'tipCN');
                $('#dgMonitorTask').datagrid('showColumn', 'materialDocNumber');
                $('#dgMonitorTask').datagrid('showColumn', '_mtlCategoryStatus');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalRequiredQuantity');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalWmsPreparationQuantity');
                $('#dgMonitorTask').datagrid('hideColumn', 'totalGetMaterialQuantity');
            }
            <% if (ynWebRight.rightVerify)
              { %>
                options.queryPerson = $('#workerName').combogrid('getValue');
            <%} %>
            options.taskString = ""; 
            

            //currentStatus = $('#queryStatus').val();

            $('#dgMonitorTask').datagrid('reload');

            //MessageInfo();
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
            options.queryBomWarehouse = $('#queryBomWarehouse').combogrid('getValue');
            options.queryBomMtlCategory = $('#queryBomMtlCategory').combogrid('getValue');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadRequirementOperationsList';
            $('#dgJobBom').datagrid('reload');
        }
        function jobBomReload() {
            $('#dgJobBom').datagrid('reload');
        }
    </script>
    <script type="text/javascript">
        var releaseHeaderIds = null;
        function RunTask() {
            releaseHeaderIds = "";
            var checkRows = $('#dgMonitorTask').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选将要执行的任务！", "info");
                return;
            }
            $.each(checkRows, function (i, item) {
                if (releaseHeaderIds != "")
                    releaseHeaderIds += ",";
                releaseHeaderIds += item.id;
            });
            $.messager.confirm('确认', '确认勾选任务将要执行？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/StartExcuteTask/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
                        success: function (r) {
                            if (r.result) {
                                Query();
                            } else {
                                $.messager.alert('确认', '开始任务失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function StopTask() {
            releaseHeaderIds = "";
            var checkRows = $('#dgMonitorTask').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选将要结束的任务！", "info");
                return;
            }
            $.each(checkRows, function (i, item) {
                if (releaseHeaderIds != "")
                    releaseHeaderIds += ",";
                releaseHeaderIds += item.id;
            });
            $.messager.confirm('确认', '确认是否完成所有正在执行的任务？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/EndExcuteTask/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
                        success: function (r) {
                            if (r.result) {
                                Query();
                            } else {
                                $.messager.alert('确认', '结束任务失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
    <script type="text/javascript">
        function GetMaterial() {
            releaseHeaderIds = "";
            var checkRows = $('#dgMonitorTask').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选将要领料的任务！", "info");
                return;
            }
            $.each(checkRows, function (i, item) {
                if (releaseHeaderIds != "")
                    releaseHeaderIds += ",";
                releaseHeaderIds += item.id;
            });
            $.messager.confirm('确认', '确认领取勾选任务的物料？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/ConfrimedGetMaterial/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
                        success: function (r) {
                            if (r.result) {
                                Query();
                                if (r.message != "" && r.message != null) {
                                    $.messager.alert('提示', r.message, 'info');
                                }
                            } else {
                                $.messager.alert('确认', '确认领料失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }

        function GetBatchMaterial() {
            releaseHeaderIds = "";
            if (expandRowObject != "") { 
                var myArray = expandRowObject.split(",");
                for (var i = 0; i < myArray.length; i++) {
                    if (myArray[i] != null && myArray[i] != "null" && myArray[i] != "") {
                        var str = myArray[i].substring(0, myArray[i].indexOf("["));
                        var checkRowsJob = $('#discreteJobsDetail_' + str).datagrid('getChecked');
                        if (checkRowsJob.length == 0) {
                            continue;
                        }
                        var newstr = myArray[i].substr(myArray[i].indexOf("[") + 1, myArray[i].indexOf("]") - (myArray[i].indexOf("[") + 1));
                        $.each(checkRowsJob, function (i, item) {
                            if (releaseHeaderIds != "")
                                releaseHeaderIds += ",";
                            releaseHeaderIds += newstr + "[" + item.wipEntityId + "]";
                        });
                    }
                }
            }

            var checkRows = $('#dgMonitorTask').datagrid('getChecked');
            if (checkRows.length > 0) {
                $.each(checkRows, function (i, item) {
                    if (releaseHeaderIds != "")
                        releaseHeaderIds += ",";
                    if (releaseHeaderIds.indexOf(item.id) < 0)
                        releaseHeaderIds += item.id;
                });
            }
            
            if (releaseHeaderIds == "") {
                $.messager.alert("提示", "请勾选将要领料的任务或作业！", "info");
                return;
            }

            $.messager.confirm('确认', '确认领取勾选任务的物料？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/ConfrimedBatchGetMaterial/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
                        success: function (r) {
                            if (r.result) {
                                Query();
                                if (r.message != "" && r.message != null) {
                                    $.messager.alert('提示', r.message, 'info');
                                }
                            } else {
                                $.messager.alert('确认', '确认领料失败:' + r.message, 'error');
                            }

                            expandRowObject = "";
                        }
                    });
                }
            });
        }
    </script>
    <script type="text/javascript">
        function MessageInfo() {
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/GetMaterialNoticeInfo/';
            $.ajax({
                url: sUrl,
                type: "post",
                success: function (r) {
                    var rVal = $.parseJSON(r);
                    if (rVal.result) {
                        if (rVal.message != "" && rVal.message != null) {
                            var rValStr = "ALL";
                            var myArray = rVal.message.split(",");
                            var str = "<div><div style='float:left;color:#000;'><a href='javascript:void(0);' onclick='GetSpecTask(\"" + rVal.message + "\");'>";
                            for (var i = 0; i < myArray.length; i++) {
                                str += "【" + myArray[i] + "】";
                            }
                            str += "</a></div><div style='float:right;color:#000;'><a href='javascript:void(0);' onclick='GetSpecTask(\"" + rValStr + "\");'>查看</a></div></div>";

                            $.messager.show({
                                title: '领料通知',
                                msg: str,
                                timeout: 5000,
                                showType: 'slide'
                            });
                        }
                    }
                }
            });
        }
        function GetSpecTask(taskString) {
            var options = $('#dgMonitorTask').datagrid('options');
            options.queryParams.taskString = taskString;
            options.dataQuery = "";
            options.lineQuery = "";
            options.queryType = "";
            options.queryDate = "";
            options.queryFormat = "";
            options.queryWarehouse = "";
            options.queryWipEntity = "";
            options.queryStartDate = "";
            options.queryEndDate = "";
            options.queryStartJobDate = "";
            options.queryEndJobDate = "";

            options.queryPerson = "";
            $('#dgMonitorTask').datagrid('reload');
        }
    </script>
    <script type="text/javascript">
        function MarkTask() {
            $.messager.confirm('确认', '是否标记该作业？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/MarkTask/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "id": currentId,
                            "wipEntityId": tempId
                        },
                        success: function (r) {
                            if (r.result) {
                                Query();
                                currentId = null;
                                tempId = null;
                            } else {
                                $.messager.alert('确认', '结束任务失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function UnMarkTask() {
            $.messager.confirm('确认', '是否取消该作业标记？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/UnMarkTask/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "id": currentId,
                                "wipEntityId": tempId
                        },
                        success: function (r) {
                            if (r.result) {
                                Query();
                            } else {
                                $.messager.alert('确认', '结束任务失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
    <script type="text/javascript">
        function CloseGetMaterialTask() {
            releaseHeaderIds = "";
            var checkRows = $('#dgMonitorTask').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选将要关闭的领料任务！", "info");
                return;
            }
            $.each(checkRows, function (i, item) {
                if (releaseHeaderIds != "")
                    releaseHeaderIds += ",";
                releaseHeaderIds += item.id;
            });
            $.messager.confirm('确认', '确认需要关闭的领料任务？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/CloseGetMaterialTask/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
                        success: function (r) {
                            if (r.result) {
                                Query();
                            } else {
                                $.messager.alert('确认', '确认关闭任务失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
    <script type="text/javascript">
        function taskFormat(value, row, index) {
            var imgUrl = "";
            if (row.relatedMark != null && row.relatedMark != "") {
                imgUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/flag_blue.png';
            }
            else {
                imgUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/flag_white.png';
            }
            return "<a href='javascript:void(0)'><img src='" + imgUrl + "' alt='' /></a>";
        }
        function jobMarkFormat(value, row, index) {
            var imgUrl = "";
            if (row.ascmMarkTaskLog != null && row.ascmMarkTaskLog != "") {
                imgUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/flag_blue.png';
            }
            else {
                imgUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/flag_white.png';
            }
            return "<a href='javascript:void(0)'><img src='" + imgUrl + "' alt='' /></a>";
        }
    </script>
    <script type="text/javascript">
        function QueryBom() {
            var options = $('#dgJobBom').datagrid('options').queryParams;
            options.taskId = currentId;
            options.jobId = tempId;
            options.queryBomWarehouse = $('#queryBomWarehouse').combogrid('getValue');
            options.queryBomMtlCategory = $('#queryBomMtlCategory').combogrid('getValue');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadRequirementOperationsList';
            $('#dgJobBom').datagrid('reload');
        }
    </script>
</asp:Content>
