<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	领料任务分配管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: false;">
        <%--<table id="dgTasks" title="领料任务分配管理" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : false,
                          checkOnSelect: true,
                          selectOnCheck: true,
                          idField: 'taskId',
                          sortName: 'taskId',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb1',
                          loadMsg: '更新数据......'"
                          >
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'id',width:40,align:'center'">ID</th>
                    <th data-options="field:'taskId',width:60,align:'center'">任务号</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'IdentificationIdCN',width:60,align:'center'">类型</th>
                    <th data-options="field:'productLine',width:60,align:'center'">生产线</th>
                    <th data-options="field:'warehouserId',width:80,align:'center'">所属仓库</th>
                    <th data-options="field:'warehouserPlace',width:160,align:'left'">仓库位置</th>
                    <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                    <th data-options="field:'materialTypeCN',width:80,align:'center'">物料编码类型</th>
                    <th data-options="field:'categoryStatusCN',width:80,align:'center'">物料类别状态</th>
                    <th data-options="field:'tipCN',width:80,align:'center'">手动任务</th>
                    <th data-options="field:'taskTime',width:70,align:'center'">上线时间</th>
                    <th data-options="field:'ascmRanker_name',width:80,align:'center'">上传人</th>
                    <th data-options="field:'_status',width:80,align:'center'">状态</th>
                    <th data-options="field:'ascmWorker_name',width:80,align:'center'">责任人</th>
                </tr>
            </thead>
        </table>--%>
        <table id="dgTasks" title="领料任务分配管理" style="" border="false" fit="true" singleSelect="false"
			    idField="taskId" sortName="taskId" sortOrder="asc" striped="true" toolbar="#tb1">
        </table>

        <div id="tb1">            
            <span>生成日期：</span><input class="easyui-datebox" id="queryCreateTime" type="text" style="width:100px;" />
            <span>任务状态：</span>
            <select id="queryStatus" name="queryStatus" style="width:80px;">
                <option value=""></option>
                <% List<string> listStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.StatusDefine.GetList(); %>
                <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                   { %>
                <% foreach (string status in listStatusDefine)
                    { %>
                <option value="<%=status %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.StatusDefine.DisplayText(status)%></option>
                <% } %>
                <% } %>
            </select>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">任务查询</a>
            <% if (ynWebRight.rightEdit){ %>
            <a id="lbtnAllocateTask" name="lbtnAllocateTask" href="javascript:void(0);" class="easyui-linkbutton"
                plain="true" icon="icon-extract" onclick="AllocateTask();">任务分配</a>
            <%} %>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete()">删除</a>
            <%} %>
        </div>

        <div id="materialDiv" class="easyui-window" title="作业物料清单" style="padding: 0px;width:640px;height:450px;"
			closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true" iconCls="icon-search">
            <div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:0px;background:#fff;border:1px solid #ccc;">
                    <%--<form id="materialForm" method="post" style="" fit="true" >
                        <ul id="materialList" title="" border="false" singleSelect="true"
			                    idField="id" striped="true">
		                </ul>
			        </form>--%>
                    <table id="materialList" title="作业物料清单" class="easyui-datagrid" style="" border="false" fit="true" singleSelect="true"
			                idField="id" sortName="" striped="true" noheader="true" rownumbers="true">
                    </table>
				</div>
			</div>
		</div>
    </div>

    <div id="addTask" class="easyui-window" title="添加临时任务" style="padding: 10px;width:640px;height:480px;"
		iconCls="icon-add" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
		<div class="easyui-layout" fit="true">
			<div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
		        <form id="addTaskForm" method="post">
			        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				        <tr style="height:24px">
					        <td style="width: 20%; text-align:right;" nowrap>
						        <span>仓库：</span>
					        </td>
					        <td style="width:27%">
						        <span id="spanWarehouserId"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/WarehouserIdSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "warehouserId" }); %></span>
                                <span style="color: Red;">*</span>
					        </td>
                            <td style="text-align:right;" nowrap>
						        <span>作业内容：</span>
					        </td>
					        <td>
						        <select id="tip" name="tip" style="width:115px;">
                                    <option value=""></option>
                                    <% List<string> listTipDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmAllocateRule.OtherDefine.GetList(); %>
                                    <% if (listTipDefine != null && listTipDefine.Count > 0)
                                       { %>
                                    <% foreach (string tip in listTipDefine)
                                        { %>
                                    <option value="<%=tip %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmAllocateRule.OtherDefine.DisplayText(tip)%></option>
                                    <% } %>
                                    <% } %>
                                </select><span style="color: Red;">*</span>
					        </td>
				        </tr>
                        <tr style="height:24px">
					        <td style="width: 20%; text-align:right;" nowrap>
						        <span>生产线：</span>
					        </td>
					        <td style="width:27%">
						        <input class="easyui-validatebox" id="productLine" name="productLine" type="text" style="width:111px;" />
					        </td>
                            <td style="width:18%; text-align:right;" nowrap>
						        <span>备料形式：</span>
					        </td>
					        <td>
						        <select id="mtlCategoryStatus" name="mtlCategoryStatus" style="width:115px;">
                                    <option value=""></option>
                                    <% List<string> listMtlCategoryStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.GetList(); %>
                                    <% if (listMtlCategoryStatusDefine != null && listMtlCategoryStatusDefine.Count > 0)
                                       { %>
                                    <% foreach (string mtlCategoryStatusDefine in listMtlCategoryStatusDefine)
                                        { %>
                                    <option value="<%=mtlCategoryStatusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.MtlCategoryStatusDefine.DisplayText(mtlCategoryStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
					        </td>
				        </tr>
                        <tr style="height:24px">
					        <td style="width: 20%; text-align:right;" nowrap>
						        <span>上线时间：</span>
					        </td>
					        <td style="width:27%">
						        <select id="taskTime" name="taskTime" style="width:115px;">
                                    <option value=""></option>
                                    <% List<string> listTaskTimeDefine = new List<string>(); %>
                                    <% listTaskTimeDefine.Add("上午");%>
                                    <% listTaskTimeDefine.Add("下午"); %>
                                    <% if (listTaskTimeDefine != null && listTaskTimeDefine.Count > 0)
                                       { %>
                                    <% foreach (string taskTime in listTaskTimeDefine)
                                        { %>
                                    <option value="<%=taskTime %>"><%=taskTime%></option>
                                    <% } %>
                                    <% } %>
                                </select>
					        </td>
                            <td style="text-align:right;" nowrap>
						        <span>类型：</span>
					        </td>
					        <td>
						        <select id="IdentificationId" name="IdentificationId" style="width:115px;">
                                    <option value=""></option>
                                    <% List<string> listIdentificationIdDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.IdentificationIdDefine.GetList(); %>
                                    <% if (listIdentificationIdDefine != null && listIdentificationIdDefine.Count > 0)
                                       { %>
                                    <% foreach (string identificationIdDefine in listIdentificationIdDefine)
                                        { %>
                                    <option value="<%=identificationIdDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.IdentificationIdDefine.DisplayText(int.Parse(identificationIdDefine))%></option>
                                    <% } %>
                                    <% } %>
                                </select>
					        </td>
				        </tr>
                        <tr style="height:24px">
					        <td style="width: 20%; text-align:right;" nowrap>
						        <span>物料编码：</span>
					        </td>
					        <td style="width:27%">
						        <span id="spanMaterialDocNumber"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/MaterialDocNumberSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "materialDocNumber" }); %></span>            
					        </td>
                            <td style="text-align:right;" nowrap>
						        <span>关联信息：</span>
					        </td>
                            <td>
                                <span id="span1"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewMarkTaskLogSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "relatedMarkId" }); %></span>
                            </td>
				        </tr>
			        </table>
		        </form>
			</div>
            <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="AddTaskSave()">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#addTask').window('close');">
                        取消</a>
                </div>
		</div>
	</div>
    <div id="editTask" class="easyui-window" title="修改临时领料任务" style="padding: 10px;
            width: 640px; height: 480px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editTaskForm" method="post">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>任务号：</span>
                            </td>
                            <td style="width: 27%">
                                <input type="text" id="sTaskId" name="taskId" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>生产线：</span>
                            </td>
                            <td>
                            <input type="text" id="sProductLine" name="productLine" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>类型：</span>
                            </td>
                            <td>
                                <input type="text" id="sIdentificationIdCN" name="IdentificationIdCN" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>仓库：</span>
                            </td>
                            <td>
                                <input type="text" id="sWarehouserId" name="warehouserId" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>备料形式：</span>
                            </td>
                            <td>
                                <input type="text" id="sCategoryStatusCN" name="categoryStatusCN" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>物料编码：</span>
                            </td>
                            <td>
                                <input type="text" id="sMaterialDocNumber" name="materialDocNumber" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>排产员：</span>
                            </td>
                            <td>
                                <input type="text" id="sAscmRanker_name" name="ascmRanker_name" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>领料员：</span>
                            </td>
                            <td>
                                <span id="spanWorker"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "WorkerName" }); %></span>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>作业内容：</span>
                            </td>
                            <td>
                            <input type="text" id="sTipCN" name="tipCN" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>上线时间：</span>
                            </td>
                            <td>
                            <input type="text" id="sTaskTime" name="taskTime" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>关联任务：</span>
                            </td>
                            <td colspan="3">
                            <textarea class="easyui-validatebox" id="relatedMarkInfo" name="relatedMarkInfo" rows="3"
                                    cols="342" style="width: 360px; background-color: #CCCCCC;" readonly="readonly"></textarea>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="btnEditSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="EditTaskSave()">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editTask').window('close');">
                        取消</a>
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
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/GetAllocateTasksList/';
            var options = $('#dgTasks').datagrid('options');
            options.url = sUrl;
            options.queryParams.queryCreateTime = $("#queryCreateTime").datebox('getText');
            options.queryParams.queryStatus = $("#queryStatus").val();

            $('#dgTasks').datagrid('reload');
        }

        $(function(){
            $('#dgTasks').datagrid({
                loadMsg: '更新数据......',
                pagination: true,
                rownumbers: true,
                pageSize: 50,
                checkOnSelect: true,
                selectOnCheck: true,
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/GetAllocateTasksList/',
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px"><table id="taskDetail_' + index + '"></table></div>';
                },
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { checkbox: true },
                    { field: 'taskIdCn', title: '任务号', width: 50, align: 'center' }
                ]],
                columns: [[
                    { field: 'productLine', title: '生产线', width: 50, align: 'center' },
                    { field: 'warehouserId', title: '仓库', width: 70, align: 'center' },
                    { field: 'warehouserPlace', title: '仓库位置', width: 180, align: 'left' },
                    { field: '_mtlCategoryStatus', title: '物料类别状态', width: 80, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' },
                    { field: 'tipCN', title: '作业内容', width: 60, align: 'center' },
                    { field: 'ascmRanker_name', title: '排产员', width: 80, align: 'center' },
                    { field: 'taskTime', title: '上线时间', width: 70, align: 'center' },
                    { field: 'ascmWorker_name', title: '责任人', align: 'center', width: 80 },
                    { field: 'logisticsClassName', title: '所属车间', align: 'center', width: 80 },
                    { field: '_status', title: '状态', width: 50, align: 'center' },
                    { field: 'uploadDate', title: '上传日期', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    //currentId = rowData.id;
                },
                onExpandRow: function (index, row) {
                    $('#taskDetail_' + index).datagrid({
                        fitColumns: true,
                        singleSelect: true,
                        height: 'auto',
                        columns: [[
                            { field: 'jobId', title: '作业号', width: 90, align: 'center'},
						    { field: 'jobDate', title: '作业日期', width: 80, align: 'center' },
						    { field: 'jobInfoId', title: '装配件', width: 80, align: 'center' },
						    { field: 'jobDesc', title: '装配件描述', width: 80, align: 'left' },
						    { field: 'count', title: '数量', width: 80, align: 'center' },
                            { field: 'productLine', title: '生产线', width: 80, align: 'center' },
                            { field: 'onlineTime', title: '上线时间', width: 80, align: 'center' }, //"<div id=\"ggg\" onclick=\"alert(\'hello\')\">";
                            { field: 'tip2', title: '', width: 70, formatter: function (value, row_job, index) {var e = '<a onclick=' + '"materialQuery(' + "'" + row.taskId + "',"+ "'" + row_job.jobId + "'" + ');"' + '>详情</a>'; return e; } }
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

            $('#materialList').datagrid({
                pagination: false,
                loadMsg: '更新数据......',
                columns: [[                    
                    { field: 'materialDocNumber', title: '组件', width: 100, align: 'center' },
					{ field: 'mpsDateRequiredStr', title: '需求日期', width: 120, align: 'center' },
                    { field: 'materialDescription', title: '组件说明', width: 200, align: 'center' },
                    { field: 'wipSupplyTypeCn', title: '类型', width: 70, align: 'center' },
                    { field: 'supplySubinventory', title: '子库存', width: 90, align: 'center' },
                    { field: 'quantityPerAssembly', title: '每个装', width: 100, align: 'center' },
                    { field: 'requiredQuantity', title: '需求数量', width: 80, align: 'center' }
                ]]
            });

            //-- 初始化默认日期为当天 --
            var date = new Date();
            var yyyy = date.getFullYear();
            var MM = date.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            var dd = date.getDate();
            dd = dd < 10 ? "0" + dd : dd;
            $("#queryCreateTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        });

        function materialQuery(taskId,jobId)
        {
            queryData = $('#dgTasks').datagrid('getRows');
            for(var i in queryData)
            {
                if(queryData[i].taskId == taskId)
                {
                    for(var j in queryData[i].ascmDiscreteJobsList)
                    {
                        if(queryData[i].ascmDiscreteJobsList[j].jobId == jobId)
                        {
                            var materialList = queryData[i].ascmDiscreteJobsList[j].listAscmWipBom;
                            $('#materialList').datagrid('loadData', materialList);
                            $('#materialDiv').window('open');
                        }
                    }
                }
            }
        };

        function Add() {
            $('#addTask').window('open');
            $("#addTaskForm").form('clear');
            var option = $('#relatedMark').combogrid('grid');
            option.datagrid('reload');
        }
        function AddTaskSave() {
            $('#addTaskForm').form('submit', {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/AddTaskSave?warehouserPlace='+$('#warehouserId').combogrid('grid').datagrid('getSelected').description,
                onSubmit: function () {
                    return $('#addTaskForm').form('validate');
                },
                success: function (r) {
                    var rVal = $.parseJSON(r);
                    if (rVal.result) {
                        $('#addTask').window('close');
                        Query();
                    } else {
                        $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                    }
                }
            });
        }
        function Delete() {
            var checkedRows = $('#dgTasks').datagrid('getChecked');
            if (checkedRows) {
                $.messager.confirm('确认', '确认删除？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/TaskDelete/';
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data: { selectRow: JSON.stringify(checkedRows) },
                            success: function (r) {
                                if (r.result) {
                                    $('#dgTasks').datagrid('uncheckAll');
                                    Query();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
            else {
                $.messager.alert('提示', '请勾选要删除的任务', 'info');
            }
        }
    </script>
    <script type="text/javascript">
        function Edit() {
            var selectRow = $('#dgTasks').datagrid('getSelected');
            if (selectRow) {
                $('#editTask').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/TaskEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editTaskForm")[0].reset();
                            $('#WorkerName').combogrid('clear');

                            $('#sTaskId').val(r.taskId);
                            $('#sProductLine').val(r.productLine);
                            $('#sIdentificationIdCN').val(r.IdentificationIdCN);
                            $('#sWarehouserId').val(r.warehouserId);
                            $('#sCategoryStatusCN').val(r.categoryStatusCN);
                            $('#sMaterialDocNumber').val(r.materialDocNumber);
                            $('#sAscmRanker_name').val(r.ascmRanker_name);
                            $('#sTipCN').val(r.tipCN);
                            $('#sTaskTime').val(r.taskTime);
                            $('#WorkerName').combogrid('setValue', r.WorkerName);
                            $('#relatedMarkInfo').val(r.relatedMarkInfo);
                        }
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请填写必填项', 'info');
            }
        }
        function EditTaskSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editTaskForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/EditTaskSave/' + currentId,
                        onSubmit: function () {
                            return $('#editTaskForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editTask').window('close');
                                currentId = rVal.id;
                                Query();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
    <script type="text/javascript">
        function AllocateTask() {
            $.messager.confirm('确认', '请确认是否执行自动分配？', function (result) {
                if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/AllocateTaskList',
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                Query();
                            } else {
                                $.messager.alert('确认', '自动分配任务失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
