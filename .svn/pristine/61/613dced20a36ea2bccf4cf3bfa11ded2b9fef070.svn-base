<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--选择作业-->

<div id="wipDiscreteJobsSelect" class="easyui-window" title="选择作业" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWipDiscreteJobs" title="作业信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              singleSelect: false,
                              rownumbers: true,
                              idField: 'wipEntityId',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWipDiscreteJobsSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');
                                  $(this).datagrid('clearChecked');                     
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'wipEntityId',hidden:'true'"></th>
                        <th data-options="field:'ascmWipEntities_Name',width:120,align:'center'">作业名称</th> 
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'description',width:300,align:'left'">作业描述</th>
                        <th data-options="field:'status_Type',width:70,align:'center'">作业状态</th>
                        <th data-options="field:'scheduledStartDate',width:110,align:'left'">作业计划时间</th>
                        <th data-options="field:'scheduledCompletionDate',width:110,align:'left'">作业上线时间</th>
                        <th data-options="field:'ascmMaterialItem_DocNumber',width:110,align:'left'">装配件编码</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbWipDiscreteJobsSelect" style="padding:5px;height:auto;">
                <div style="margin-bottom:5px;">
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WipDiscreteJobsSelectOk()">确认</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wipDiscreteJobsSelect').window('close');">取消</a> 
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWipDiscreteJobs(startScheduledStartDate, endScheduledStartDate, startWipEntitiesName, endWipEntitiesName, wipSupplyType,
    startSupplySubInventory, endSupplySubInventory, startInventoryItemId, endInventoryItemId, startScheduleGroupId, endScheduleGroupId) {
        $('#wipDiscreteJobsSelect').window('open');
        var options = $('#dgSelectWipDiscreteJobs').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsList';
        options.queryParams.startScheduledStartDate = startScheduledStartDate;
        options.queryParams.endScheduledStartDate = endScheduledStartDate;
        options.queryParams.startWipEntitiesName = startWipEntitiesName;
        options.queryParams.endWipEntitiesName = endWipEntitiesName;
        options.queryParams.wipSupplyType = wipSupplyType;
        options.queryParams.startSupplySubInventory = startSupplySubInventory;
        options.queryParams.endSupplySubInventory = endSupplySubInventory;
        options.queryParams.startInventoryItemId = startInventoryItemId;
        options.queryParams.endInventoryItemId = endInventoryItemId;
        options.queryParams.startScheduleGroupId = startScheduleGroupId;
        options.queryParams.endScheduleGroupId = endScheduleGroupId;
        $('#dgSelectWipDiscreteJobs').datagrid('reload');
    }
//    function WipDiscreteJobsSelectSearch() {
//        var options = $('#dgSelectWipRequireOperation').datagrid('options');
//        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsQueryList';
//        options.queryParams.scheduleGroupId = $('#wipScheduleGroupSelect2').combogrid('getValue');
//        options.queryParams.startPlanTime = $('#wroStartPlanTime').datebox('getText');
//        options.queryParams.endPlanTime = $('#wroEndPlanTime').datebox('getText');
//        $('#dgSelectWipRequireOperation').datagrid('reload');
//    }
    function WipDiscreteJobsSelectOk() {
        var checkRows = $('#dgSelectWipDiscreteJobs').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择作业！", "info");
            return;
        }
        try {
            WipDiscreteJobsSelected(checkRows);
        } catch (e) { }
        $('#wipDiscreteJobsSelect').window('close');
    }
</script>