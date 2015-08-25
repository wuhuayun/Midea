<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	领料任务分配管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgTask" title="领料任务分配管理" style="" border="false" fit="true" singleSelect="false"
			    idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb">
        </table>
        <div id="tb" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>起止日期：</span><input class="easyui-datebox" id="queryStartDate" type="text" style="width:130px;" /> ~
                <input class="easyui-datebox" id="queryEndDate" type="text" style="width:130px;" />
                <span>作业日期：</span><input class="easyui-datebox" id="queryStartJobDate" type="text" style="width:130px;" /> ~
                <input class="easyui-datebox" id="queryEndJobDate" type="text" style="width:130px;" />
            </div>

            <div style="margin-bottom:5px;">
                <span>任务类型：</span>
                <select id="queryType" name="queryType" style="width: 100px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                    <option value="">　</option>
                    <% List<int> listTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.IdentificationIdDefine.GetList(); %>
                    <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                        { %>
                    <% foreach (int statusDefine in listTypeDefine)
                        { %>
                    <option value="<%=statusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.IdentificationIdDefine.DisplayText(statusDefine)%></option>
                    <% } %>
                    <% } %>
                </select>
                <span>任务状态：</span>
                <select id="queryStatus" name="queryStatus" style="width: 110px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                    <option value="">　</option>
                    <% List<string> listStatusDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.StatusDefine.GetList(); %>
                    <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                        { %>
                    <% foreach (string statusDefine in listStatusDefine)
                        { %>
                    <option value="<%=statusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.StatusDefine.DisplayText(statusDefine)%></option>
                    <% } %>
                    <% } %>
                </select>
                <span>生产线：</span>
                <select id="queryLine" name="queryLine" style="width: 108px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                    <option value="">　</option>
                    <% List<string> listLineDefine = MideaAscm.Services.GetMaterialManage.AscmDiscreteJobsService.GetInstance().GetLineList(); %>
                    <% if (listLineDefine != null && listLineDefine.Count > 0)
                        { %>
                    <% foreach (string lineDefine in listLineDefine)
                        { %>
                    <option value="<%=lineDefine %>"><%=lineDefine %></option>
                    <% } %>
                    <% } %>
                </select>
                <span>仓库：</span>
                <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewWarehouseIdSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWarehouse", width = "128px" }); %></span>
            </div>
            
            <div style="margin-bottom:5px;">
                <% if (ynWebRight.rightAdd){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
                <%} %>

                <% if (ynWebRight.rightEdit){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
                <%} %>

                <% if (ynWebRight.rightDelete){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete()">删除</a>
                <%} %>

                <% if (ynWebRight.rightEdit){ %>
                <a id="lbtnAllocateTask" name="lbtnAllocateTask" href="javascript:void(0);" class="easyui-linkbutton"
                    plain="true" icon="icon-extract" onclick="AllocateTask();">分配任务</a>
                <%} %>

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
						        <span id="spanWarehouserId"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewWarehouseIdSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "warehouserId" }); %></span>
                                <span style="color: Red;">*</span>
					        </td>
                            <td style="text-align:right;" nowrap>
						        <span>作业内容：</span>
					        </td>
					        <td>
						        <select id="tip" name="tip" style="width:115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <option value="">　</option>
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
						        <select id="mtlCategoryStatus" name="mtlCategoryStatus" style="width:115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <option value="">　</option>
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
						        <select id="taskTime" name="taskTime" style="width:115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <option value="">　</option>
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
                            <%--<td style="text-align:right;" nowrap>
						        <span>类型：</span>
					        </td>
					        <td>
						        <select id="IdentificationId" name="IdentificationId" style="width:115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <option value="">　</option>
                                    <% List<int> listIdentificationIdDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.IdentificationIdDefine.GetList(); %>
                                    <% if (listIdentificationIdDefine != null && listIdentificationIdDefine.Count > 0)
                                       { %>
                                    <% foreach (int identificationIdDefine in listIdentificationIdDefine)
                                        { %>
                                    <option value="<%=identificationIdDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmGetMaterialTask.IdentificationIdDefine.DisplayText(identificationIdDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
					        </td>--%>
                            <td style="width: 20%; text-align:right;" nowrap>
						        <span>物料编码：</span>
					        </td>
					        <td style="width:27%">
						        <span id="spanMaterialDocNumber"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewMaterialDocnumberSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "materialDocNumber", panelWidth = 500 }); %></span>            
					        </td>
				        </tr>
                        <tr style="height:24px">
                            <td style="width: 20%;text-align:right;" nowrap>
						        <span>关联信息：</span>
					        </td>
                            <td>
                                <span id="spanRelatedInfo"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewMarkIdSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "relatedMarkId", panelWidth = 500 }); %></span>
                            </td>
                            <td style="width: 20%;text-align:right;" nowrap>
						        <span></span>
					        </td>
                            <td></td>
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
                                <span id="spanWorker"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "WorkerName", width = "110px", panelWidth = 500, queryParams = "'queryRole':'领料员'" }); %></span>
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <script type="text/javascript">
        var currentId = null;
        var tempId = null;
        var releaseHeaderIds = "";
        $(function () {
            $('#dgTask').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/AllocateLogisticsTaskList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                selectOnCheck: true,
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px; width:98%"><table id="discreteJobsDetail_' + index + '"></table></div>';
                },
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { checkbox: true },
                    { field: 'taskIdCn', title: '任务号', width: 50, align: 'center' }
                ]],
                columns: [[
                    { field: 'dateReleasedCn', title: '作业日期', width: 70, align: 'center' },
                    { field: 'productLine', title: '生产线', width: 50, align: 'center' },
                    { field: 'warehouserId', title: '仓库', width: 70, align: 'center' },
                    { field: 'warehousePlace', title: '仓库位置', width: 100, align: 'center' },
                    { field: '_mtlCategoryStatus', title: '物料类别状态', width: 80, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 90, align: 'center' },
                    { field: 'tipCN', title: '作业内容', width: 60, align: 'center' },
                    { field: 'taskTime', title: '上线时间', width: 70, align: 'center' },
                    { field: 'totalRequiredQuantity', title: '需求数量', align: 'center', width: 90 },
                    { field: 'ascmWorker_name', title: '责任人', align: 'center', width: 80 },
                    { field: '_status', title: '状态', width: 50, align: 'center' },
                    { field: 'uploadDate', title: '上传日期', width: 80, align: 'center' },
                    { field: 'which', title: '第几次', width: 50, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                rowStyler: function (index, row) {
                    if (row._status == "未分配") {
                        return 'color:purple;';
                    }
                },
                onExpandRow: function (index, row) {
                    currentId = row.id;
                    $('#discreteJobsDetail_' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LoadWipDiscreteJobsList/' + row.id,
                        fitColumns: true,
                        rownumbers: true,
                        //singleSelect: true,
                        height: 'auto',
                        columns: [[
                            { field: 'wipEntityId', title: 'ID', width: 60, hidden: true },
                            { field: 'ascmWipEntities_Name', title: '作业号', width: 75, align: 'center' },
                            { field: 'scheduledStartDateCn', title: '作业日期', align: 'center', width: 60 },
                            { field: 'ascmDiscreteJobs_line', title: '产线', align: 'center', width: 60 },
                            { field: 'ascmMaterialItem_DocNumber', title: '装配件', align: 'center', width: 70 },
                            { field: 'ascmMaterialItem_Description', title: '装配件描述', align: 'left', width: 160 },
                            { field: 'netQuantity', title: '计划数量', align: 'center', width: 60 },
                            { field: 'totalRequiredQuantity', title: 'Bom总数量', align: 'center', width: 60 },
                            { field: 'operate', title: '', align: 'center', width: 50, formatter: jobFormat }
					    ]],
                        onResize: function (index, row) {
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
//                    if (currentStatus != null && currentStatus != "") {
//                        $(this).datagrid('clearSelections');
//                    }

                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        //$(this).datagrid('selectRecord', currentId);
                    }
                }
            })
        });
        
    </script>
    <script type="text/javascript">
        function Query() {
            var options = $('#dgTask').datagrid('options').queryParams;
            options.queryStartDate = $('#queryStartDate').datebox('getText');
            options.queryEndDate = $('#queryEndDate').datebox('getText');
            options.queryStartJobDate = $('#queryStartJobDate').datebox('getText');
            options.queryEndJobDate = $('#queryEndJobDate').datebox('getText');
            options.queryStatus = $('#queryStatus').combobox('getValue');
            options.queryLine = $('#queryLine').combobox('getValue');
            options.queryType = $('#queryType').combobox('getValue');
            options.queryWarehouse = $('#queryWarehouse').combogrid('getValue');

            $('#dgTask').datagrid('reload');
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
    <script type="text/javascript">
        function Add() {
            $('#addTask').window('open');
            $("#addTaskForm").form('clear');

            var option = $('#relatedMarkId').combogrid('grid');
            option.datagrid('reload');
        }
    </script>
    <script type="text/javascript">
        function Edit() {
            var selectRow = $('#dgTask').datagrid('getSelected');
            if (selectRow) {
                $('#editTask').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/AllocateLogisticsTaskEdit/' + selectRow.id;
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
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/AllocateLogisticsTaskEditSave/' + currentId,
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
        function AddTaskSave() {
            $('#addTaskForm').form('submit', {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/AllocateLogisticsTaskAddSave/',
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
    </script>
    <script type="text/javascript">
        function Delete() {
            releaseHeaderIds = "";
            var checkRows = $('#dgTask').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选将要删除的任务！", "info");
                return;
            }
            $.each(checkRows, function (i, item) {
                if (releaseHeaderIds != "")
                    releaseHeaderIds += ",";
                releaseHeaderIds += item.id;
            });
            $.messager.confirm('确认', '确认需要删除的任务？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/AllocateLogisticsTaskDelete/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
                        success: function (r) {
                            if (r.result) {
                                $('#dgTask').datagrid('uncheckAll');
                                Query();
                            } else {
                                $.messager.alert('确认', '确认删除任务失败:' + r.message, 'error');
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
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/AllocateLogisticsTask',
                        type: "post",
                        dataType: "json",
                        beforeSend: function () {
                            $('#dgTask').datagrid('loading');
                        },
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
