<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择作业--%>
<div id="wipDiscreteJobSelect" class="easyui-window" title="选择作业" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWipDiscreteJob" title="作业信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'wipEntityId',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWipDiscreteJobSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  WipDiscreteJobSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="field:'wipEntityId',hidden:'true'"></th>
                        <th data-options="field:'ascmWipEntities_Name',width:120,align:'left'">作业名称</th> 
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'description',width:300,align:'left'">作业描述</th>
                        <th data-options="field:'statusType_Cn',width:120,align:'center'">作业状态</th>
                        <th data-options="field:'classCode',width:120,align:'center'">作业类型</th>
                        <th data-options="field:'ascmMaterialItem_DocNumber',width:90,align:'center'">装配件编码</th>
                        <th data-options="field:'ascmMaterialItem_Description',width:300,align:'left'">装配件描述</th>
                        <th data-options="field:'ascmWipScheduleGroupsName',width:70,align:'left'">车间</th>
                        <th data-options="field:'productionLine',width:110,align:'center'">生产线</th>
                        <th data-options="field:'netQuantity',width:60,align:'center'">计划数量</th>
                        <th data-options="field:'scheduledStartDate',width:110,align:'center'">计划开始日期</th>
                        <th data-options="field:'scheduledCompletionDate',width:110,align:'center'">计划完成日期</th>
                        <th data-options="field:'startQuantity',width:80,align:'center'">作业开始数量</th> 
                        <th data-options="field:'quantityCompleted',width:80,align:'center'">作业完成数量</th>
                        <th data-options="field:'dateReleased',width:110,align:'center'">作业发放日期</th>
                        <th data-options="field:'dateClosed',width:110,align:'center'">作业关闭日期</th>
                        <th data-options="field:'wipSupplyType_Cn',width:60,align:'center'">供应类型</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbWipDiscreteJobSelect" style="padding:5px;height:auto;">
                <div style="margin-bottom:5px;">
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WipDiscreteJobSelectOk()">确认</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wipDiscreteJobSelect').window('close');">取消</a> 
                </div>
                <div>
                    <span>车间：</span>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipScheduleGroupSelectCombo.ascx", new MideaAscm.Code.SelectCombo { }); %>
                    <span>计划时间：</span>
                    <input id="wdjStartPlanTime" name="wdjStartPlanTime" class="easyui-datebox" style="width:120px"/>-<input id="wdjEndPlanTime" name="wdjEndPlanTime" class="easyui-datebox" style="width:120px"/>
                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WipDiscreteJobSelectSearch()">查询</a>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWipDiscreteJob() {
        $('#wipDiscreteJobSelect').window('open');
        $('#dgSelectWipDiscreteJob').datagrid('loadData', { "total": 0, "rows": [] });
    }
    function WipDiscreteJobSelectSearch() {
        var options = $('#dgSelectWipDiscreteJob').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsAscx';
        options.queryParams.scheduleGroupId = $('#wipScheduleGroupSelect').combogrid('getValue');
        options.queryParams.startPlanTime = $('#wdjStartPlanTime').datebox('getText');
        options.queryParams.endPlanTime = $('#wdjEndPlanTime').datebox('getText');
        $('#dgSelectWipDiscreteJob').datagrid('reload');
    }
    function WipDiscreteJobSelectOk() {
        var selectRow = $('#dgSelectWipDiscreteJob').datagrid('getSelected');
        if (selectRow) {
            try {
                WipDiscreteJobSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "请选择作业需求！", "info");
            return;
        }
        $('#wipDiscreteJobSelect').window('close');
    }
</script>
