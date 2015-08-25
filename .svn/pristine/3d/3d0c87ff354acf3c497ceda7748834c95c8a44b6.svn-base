<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择领料单--%>
<div id="wipReleaseSelect" class="easyui-window" title="选择领料单" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWipRelease" title="作业领料单信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'releaseHeaderId',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWipReleaseSelect',
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
                        <th data-options="field:'releaseHeaderId',hidden:'true'"></th>
                        <th data-options="field:'releaseNumber',width:80,align:'center'">编号</th>
                        <th data-options="field:'wipEntitiesName',width:120,align:'center'">作业号</th> 
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'ascmWipDiscreteJobsScheduledStartDate',width:115,align:'center'">计划时间</th>
                        <th data-options="field:'ascmWipDiscreteJobsWipSupplyType_Cn',width:115,align:'center'">供应类型</th>
                        <th data-options="field:'ascmWipDiscreteJobsAscmWipScheduleGroupsName',width:70,align:'center'">车间</th>
                        <th data-options="field:'ascmWipDiscreteJobsNetQuantity',width:60,align:'center'">计划数量</th>
                        <th data-options="field:'ascmWipDiscreteJobsStatusType_Cn',width:60,align:'center'">作业状态</th>
                        <th data-options="field:'ascmWipDiscreteJobsDescription',width:300,align:'center'">作业说明</th>
                        <th data-options="field:'ascmWipDiscreteJobs_ascmMaterialItem_DocNumber',width:100,align:'center'">装配件</th> 
                    </tr>
                </thead>
		    </table>
            <div id="tbWipReleaseSelect" style="padding:5px;height:auto;">
                <div style="margin-bottom:5px;">
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WipReleaseSelectOk()">确认</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wipReleaseSelect').window('close');">取消</a> 
                </div>
                <div>
                    <span>车间：</span>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipScheduleGroupSelectCombo.ascx", new MideaAscm.Code.SelectCombo()); %>
                    <span>计划时间：</span>
                    <input id="wrStartPlanTime" name="wrStartPlanTime" class="easyui-datebox" style="width:120px"/>-<input id="wrEndPlanTime" name="wrEndPlanTime" class="easyui-datebox" style="width:120px"/>
                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WipReleaseSelectSearch()">查询</a>
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWipRelease(releaseType) {
        $('#wipReleaseSelect').window('open');
        if (releaseType) {
            var options = $('#dgSelectWipRelease').datagrid('options');
            options.queryParams.releaseType = releaseType;     
        }
        $('#dgSelectWipRelease').datagrid('loadData', { "total": 0, "rows": [] });
    }
    function WipReleaseSelectSearch() {
        var options = $('#dgSelectWipRelease').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CuxWipReleaseHeadersList';
        options.queryParams.scheduleGroupId = $('#wipScheduleGroupSelect').combogrid('getValue');
        options.queryParams.startPlanTime = $('#wrStartPlanTime').datebox('getText');
        options.queryParams.endPlanTime = $('#wrEndPlanTime').datebox('getText');
        $('#dgSelectWipRelease').datagrid('reload');
    }
    function WipReleaseSelectOk() {
        var checkRows = $('#dgSelectWipRelease').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择作业领料单！", "info");
            return;
        }
        try {
            WipReleaseSelected(checkRows);
        } catch (e) { }
        $('#wipReleaseSelect').window('close');
    }
</script>
