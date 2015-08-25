<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    领料任务监控
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgTask" title="领料作业监控列表" style="" border="false" fit="true" singleselect="false"
           idField="wipEntityIdentification"  striped="true" toolbar="#tb">
        </table>
        <div id="tb" style="padding: 5px; height: auto;">
            <div style="margin-bottom: 5px;">
                <span>起止日期：</span><input class="easyui-datebox" id="queryStartDate" type="text" style="width: 130px;" />
                ~
                <input class="easyui-datebox" id="queryEndDate" type="text" style="width: 130px;" />
                <span>&nbsp;作业日期：</span><input class="easyui-datebox" id="queryStartJobDate" type="text"
                    style="width: 130px;" />
                ~
                <input class="easyui-datebox" id="queryEndJobDate" type="text" style="width: 130px;" />
                <span>&nbsp;&nbsp;&nbsp;&nbsp;作业号：</span> <span>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewWipEntitiesSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWipEntity", width = "150px", panelWidth = 500 });%></span>
            </div>
            <div style="margin-bottom: 5px;">
                <span>任务类型：</span><span><select id="queryType" name="queryType" style="width: 100px;"
                    class="easyui-combobox" data-options="panelHeight:'auto'">
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
                </select></span> <span>&nbsp;&nbsp;备料形式：</span><span><select id="queryFormat" name="queryFormat"
                    style="width: 100px;" class="easyui-combobox" data-options="panelHeight:'auto'">
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
                </select></span> <span>&nbsp;&nbsp;任务状态：</span><span><select id="queryStatus" name="queryStatus"
                    style="width: 110px;" class="easyui-combobox" data-options="panelHeight:'auto'">
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
                </select></span> <span>&nbsp;&nbsp;生产线：</span><span><select id="queryLine" name="queryLine"
                    style="width: 108px;" class="easyui-combobox" data-options="panelHeight:'auto'">
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
                </select></span> <span>&nbsp;&nbsp;&nbsp;&nbsp;仓&nbsp;&nbsp;&nbsp;库：</span> <span>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewWarehouseIdSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWarehouse", width = "150px" }); %></span>
            </div>
            <div style="margin-bottom: 5px;">
                <% if (ynWebRight.rightVerify)
                   { %>
                <span>&nbsp;&nbsp;责任人：</span> <span>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerName", panelWidth = 500, width = "130px", queryParams = "'queryRole':'领料员'" }); %></span>
                <%} %>
                &nbsp;
         
                <% if (ynWebRight.rightEdit)
                   { %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-save"
                    onclick="GetBatchMaterial();">确认领料</a>
                <%} %>
             
                <% if (ynWebRight.rightVerify)
                   { %>
                <a id="btnCloseTask" name="btnCloseTask" href="javascript:void(0);" class="easyui-linkbutton"
                    plain="true" icon="icon-lightning" onclick="CloseGetMaterialTask();">关闭</a>
                <%} %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-message"
                    onclick="MessageInfo();">领料通知</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                        plain="true" icon="icon-search" onclick="Query();">查询</a>
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
            $('#dgTask').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MonitorLogisticsTaskList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px; width:90%"><table id="discreteJobsDetail_' + index + '"></table></div>';
                },
                loadMsg: "加载中...",
                columns: [[
                    { checkbox: true },
                    //{ field: 'wipEntityIdentification', title: '作业号', width: 120, align: 'center' },
                    { field: 'ascmWipEntities_Name', title: '作业号', width: 120, align: 'center' },
                    { field: 'scheduledStartDateCn', title: '作业日期', width: 70, align: 'center' },
                    { field: 'ascmDiscreteJobs_line', title: '生产线', width: 50, align: 'center' },
                    { field: '_mtlCategoryStatus', title: '物料类别状态', width: 80, align: 'center' },
                    { field: 'tipCN', title: '作业内容', width: 60, align: 'center' },
                    { field: 'onlineTime', title: '上线时间', width: 70, align: 'center' },
                    { field: 'ascmWorker_name', title: '责任人', width: 80, align: 'center' },
                    { field: 'taskWipState', title: '状态', width: 70, align: 'center' },
                    { field: 'uploadDate', title: '上传日期', width: 110, align: 'center' },
                    { field: 'taskStarTime', title: '开始时间', width: 120, align: 'center' },
                    { field: 'taskEndTime', title: '结束时间', width: 120, align: 'center' },
                    { field: 'ascmMaterialItem_DocNumber', title: '装配件', width: 70, align: 'center' },
                    { field: 'ascmMaterialItem_Description', title: '装配件描述', width: 160, align: 'left' },
                    { field: 'netQuantity', title: '计划数量', width: 60, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    if (rowData.mtlCategoryStatus == "") {
                        currentId = rowData.wipEntityId;
                    }
                    else {
                        currentId = rowData.wipEntityId + ":" + rowData.mtlCategoryStatus;
                    }
                    if (expandRowObject.indexOf(currentId) > -1)
                        $('#discreteJobsDetail_' + rowIndex).datagrid("selectAll");
                },
                onUnselect: function (rowIndex, rowData) {
                    if (rowData.mtlCategoryStatus == "") {
                        currentId = rowData.wipEntityId;
                    }
                    else {
                        currentId = rowData.wipEntityId + ":" + rowData.mtlCategoryStatus;
                    }
                    if (expandRowObject.indexOf(currentId) > -1)
                        $('#discreteJobsDetail_' + rowIndex).datagrid("unselectAll");
                },
                rowStyler: function (index, row) {
                    if (row.taskWipState == "已完成") {
                        return 'color:gray;';
                    }
                    else  {
                        return 'color:blue;';
                    }
                },
                onExpandRow: function (index, row) {
                    currentId = row.wipEntityId;
                    if (expandRowObject != null && expandRowObject != "") {
                        expandRowObject += ",";
                    }
                    if (row.mtlCategoryStatus == "") {
                        expandRowObject += index + "[" + row.wipEntityId + "]";
                    } else {
                        expandRowObject += index + "[" + row.wipEntityId + ":" + row.mtlCategoryStatus + "]";
                    }
                    $('#discreteJobsDetail_' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadRequirementOperationsList/?jobId=' + row.wipEntityId + '&queryBomMtlCategory=' + row.mtlCategoryStatus,
                        fitColumns: true,
                        rownumbers: true,
                        idField:'id',
                        height: 'auto',
                        columns: [[
                            { checkbox: true },
                            {field: 'docNumber', title: '组件', width: 100, align: 'center' },
                            { field: 'mpsDateRequiredStr', title: '需求日期', width: 75, align: 'center' },
                            { field: 'description', title: '组件说明', align: 'center', width: 60 },
                            { field: 'quantityPerAssembly', title: '每个装', align: 'center', width: 60 },
                            { field: 'wipSupplyTypeCn', title: '供应类型', align: 'center', width: 70 },
                            { field: 'supplySubinventory', title: '子库', align: 'left', width: 160 },
                            { field: 'requiredQuantity', title: ' 需求数量', align: 'center', width: 60 },
                            { field: 'transactionQuantity', title: '现有数量', align: 'center', width: 60 },
                            { field: 'wmsPreparationQuantity', title: '备料数量', align: 'center', width: 60 },
                            { field: 'getMaterialQuantity', title: '领料数量', align: 'center', width: 60 },
                            { field: 'quantityGetMaterialDifference', title: ' 领料差异数量', align: 'center', width: 50 }
					    ]],
                        onCheck: function (detailindex, row) {
                            if (row.wmsPreparationQuantity <= 0) {
                                $('#discreteJobsDetail_' + index).datagrid('uncheckRow', detailindex);
                            }
                        },
                        onCheckAll: function (rows) {
                            $.each(rows, function (i, item) {
                                if (item.wmsPreparationQuantity <= 0) {
                                    $('#discreteJobsDetail_' + index).datagrid('uncheckRow', $('#discreteJobsDetail_' + index).datagrid('getRowIndex', item));
                                }
                            });
                        },
                        onResize: function (index, row) {
                            $('#dgTask').datagrid('fixDetailRowHeight', index);
                        },
                        onSelect: function (rowIndex, rowData) {
                            tempId = rowData.id;
                        },
                        onLoadSuccess: function (data) {                       
                            setTimeout(function () {
                                $('#dgTask').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        }
                    });
                },
                onCollapseRow: function (index, row) {
                    var str = "";
                    if (row.mtlCategoryStatus == "") {
                        str = index + "[" + row.wipEntityId + "]";
                    } else {
                        str = index + "[" + row.wipEntityId + ":" + row.mtlCategoryStatus + "]";
                    }
                    expandRowObject = expandRowObject.replace(str, '');
                },
                onLoadSuccess: function (data) {

                }
            })
        });       
    </script>
    <script type="text/javascript">
    function Query() {
            var options = $('#dgTask').datagrid('options').queryParams;
            options.queryStatus = $('#queryStatus').combobox('getValue');
            options.queryLine = $('#queryLine').combobox('getValue');
            options.queryType = $('#queryType').combobox('getValue');
            options.queryStartDate = $('#queryStartDate').datebox('getText');
            options.queryEndDate = $('#queryEndDate').datebox('getText');
            options.queryStartJobDate = $('#queryStartJobDate').datebox('getText');
            options.queryEndJobDate = $('#queryEndJobDate').datebox('getText');
            options.queryWarehouse = $('#queryWarehouse').combogrid('getValue');
            options.queryFormat = $('#queryFormat').combobox('getValue');
            options.queryWipEntity = $('#queryWipEntity').combogrid('getValue');
            <% if (ynWebRight.rightVerify)
              { %>
                options.queryPerson = $('#workerName').combogrid('getValue');
            <%} %>
            options.taskString = "";             
            //currentStatus = $('#queryStatus').val();
            $('#dgTask').datagrid('reload');
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
            var checkRows = $('#dgTask').datagrid('getChecked');
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
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/StartExcuteTask/';
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
            var checkRows = $('#dgTask').datagrid('getChecked');
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
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/EndExcuteTask/';
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
                            releaseHeaderIds += newstr + "[" + item.id + "]";
                        });
                    }
                }
            }

            var checkRows = $('#dgTask').datagrid('getChecked');
            if (checkRows.length > 0) {
                $.each(checkRows, function (i, item) {
                    if (releaseHeaderIds != "")
                        releaseHeaderIds += ",";
                   if (item.mtlCategoryStatus == "") {
                        currentId = item.wipEntityId;
                    }
                    else {
                        currentId = item.wipEntityId + ":" + item.mtlCategoryStatus;
                    }
                    if (releaseHeaderIds.indexOf(item) < 0)
                        releaseHeaderIds += currentId;
                });
            }

            if (releaseHeaderIds == "") {
                $.messager.alert("提示", "请勾选将要领料的任务或作业！", "info");
                return;
            }

            $.messager.confirm('确认', '确认领取勾选任务的物料？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/ConfrimedBatchGetMaterial/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
                        success: function (r) {
                            if (r.result) {
                                Query();
                                if (r.message != "" && r.message != null) {
                                    $('#dgTask').datagrid('reload');
                                  
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
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GetMaterialNoticeInfo/';
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
            var options = $('#dgTask').datagrid('options').queryParams;
            options.taskString = taskString;
            options.queryStatus = "";
            options.queryLine = "";
            options.queryType = "";
            options.queryFormat = "";
            options.queryWarehouse = "";
            options.queryWipEntity = "";
            options.queryStartDate = "";
            options.queryEndDate = "";
            options.queryStartJobDate = "";
            options.queryEndJobDate = "";

            <% if (ynWebRight.rightVerify)
              { %>
                options.queryPerson = $('#workerName').combogrid('getValue');
            <%} %>

            $('#dgTask').datagrid('reload');
        }
    </script>
    <script type="text/javascript">
        function MarkTask() {
            $.messager.confirm('确认', '是否标记该作业？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/MarkTask/';
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
                                $.messager.alert('确认', '标记失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function UnMarkTask() {
            $.messager.confirm('确认', '是否取消该作业标记？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/UnMarkTask/';
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
                                $.messager.alert('确认', '取消标记失败:' + r.message, 'error');
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
            var checkRows = $('#dgTask').datagrid('getChecked');
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
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/CloseGetMaterialTask/';
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
