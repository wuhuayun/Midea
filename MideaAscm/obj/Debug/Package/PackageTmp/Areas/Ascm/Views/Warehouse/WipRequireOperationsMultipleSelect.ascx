<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择作业需求--%>
<div id="wipRequireOperationSelect" class="easyui-window" title="选择作业需求" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWipRequireOperation" title="作业需求报表" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWipRequireOperationSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'id',hidden:'true'"></th>
                        <th data-options="field:'wipEntitiesName',width:120,align:'center'">作业名称</th> 
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'ascmWipDiscreteJobsDescription',width:300,align:'left'">作业描述</th>
                        <th data-options="field:'ascmWipRequireOperationWipSupplyType_Cn',width:115,align:'center'">装配件编码</th>
                        <th data-options="field:'ascmWipDiscreteJobs_ascmMaterialItem_Description',width:300,align:'left'">装配件描述</th>
                        <th data-options="field:'ascmWipDiscreteJobsAscmWipScheduleGroupsName',width:70,align:'left'">车间</th>
                        <th data-options="field:'ascmWipDiscreteJobsNetQuantity',width:60,align:'center'">计划数量</th>
                        <th data-options="field:'ascmWipDiscreteJobsStatusType_Cn',width:60,align:'center'">作业状态</th>
                        <th data-options="field:'ascmWipDiscreteJobs_startQuantity',width:80,align:'center'">作业开始数量</th> 
                        <th data-options="field:'ascmWipDiscreteJobs_quantityCompleted',width:80,align:'center'">作业完成数量</th>
                        <th data-options="field:'ascmWipDiscreteJobsScheduledStartDate',width:110,align:'center'">作业开始日期</th>
                        <th data-options="field:'ascmWipDiscreteJobs_dateReleased',width:110,align:'center'">作业发放日期</th>
                        <th data-options="field:'ascmWipDiscreteJobs_scheduledCompletionDate',width:110,align:'center'">作业完成日期</th>
                        <th data-options="field:'ascmWipDiscreteJobs_dateClosed',width:110,align:'center'">作业关闭日期</th>
                        <th data-options="field:'ascmMaterialItem_DocNumber',width:110,align:'center'">物料编码</th>
                        <th data-options="field:'ascmMaterialItem_Description',width:300,align:'left'">物料描述</th>
                        <th data-options="field:'supplySubinventory',width:80,align:'left'">供应子库</th>
                        <th data-options="field:'requiredQuantity',width:70,align:'center'">需求数量</th>
                        <th data-options="field:'quantityIssued',width:70,align:'center'">发料数量</th>
                        <th data-options="field:'quantityDifference',width:70,align:'center'">差异数量</th>
                        <th data-options="field:'ascmWipDiscreteJobs_classCode',width:110,align:'center'">作业类型</th>
                        <th data-options="field:'wipSupplyTypeCn',width:60,align:'center'">供应类型</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbWipRequireOperationSelect" style="padding:5px;height:auto;">
                <div style="margin-bottom:5px;">
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WipRequireOperationSelectOk()">确认</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wipRequireOperationSelect').window('close');">取消</a> 
                </div>
                <div>
                    <span>车间：</span>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipScheduleGroupSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "wipScheduleGroupSelect2" }); %>
                    <span>计划时间：</span>
                    <input id="wroStartPlanTime" name="wroStartPlanTime" class="easyui-datebox" style="width:120px"/>-<input id="wroEndPlanTime" name="wroEndPlanTime" class="easyui-datebox" style="width:120px"/>
                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WipRequireOperationSelectSearch()">查询</a>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWipRequireOperation() {
        $('#wipRequireOperationSelect').window('open');
        $('#dgSelectWipRequireOperation').datagrid('loadData', { "total": 0, "rows": [] });
    }
    function WipRequireOperationSelectSearch() {
        var options = $('#dgSelectWipRequireOperation').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsQueryList';
        options.queryParams.scheduleGroupId = $('#wipScheduleGroupSelect2').combogrid('getValue');
        options.queryParams.startPlanTime = $('#wroStartPlanTime').datebox('getText');
        options.queryParams.endPlanTime = $('#wroEndPlanTime').datebox('getText');
        $('#dgSelectWipRequireOperation').datagrid('reload');
    }
    function WipRequireOperationSelectOk() {
        var checkRows = $('#dgSelectWipRequireOperation').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择作业需求！", "info");
            return;
        }
        try {
            WipRequireOperationSelected(checkRows);
        } catch (e) { }
        $('#wipRequireOperationSelect').window('close');
    }
</script>
