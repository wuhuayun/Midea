<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业退料生成
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
   <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <table id="dgWmsMtlReturnDetail" title="作业退料查找" class="easyui-datagrid" style="" border="false"
             data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          checkOnSelect: false,
                          selectOnCheck: false,
                          idField: 'id',
                          sortName: 'docNumber',
                          sortOrder: 'desc',
                          striped: true,
                          toolbar: '#tb',
                          pagination: true,
                          pageSize: 100,
                          pageList: [50, 100, 150, 200],
                          loadMsg: '数据加载中，请稍候...',
                          onSelect: function (rowIndex, rowData) {
                              currentId=rowData.id;
                          },
                          onCheck: function (rowIndex, rowRec) {
                              if (rowRec.locked) {
                                  $(this).datagrid('uncheckRow', rowIndex);
                              }
                          },
                          onCheckAll: function (rows) {
                              $.each(rows, function (index, row) {
                                  if (row.locked) {
                                      var rowIndex = $('#dgWmsMtlReturnDetail').datagrid('getRowIndex', row);
                                      $('#dgWmsMtlReturnDetail').datagrid('uncheckRow', rowIndex);
                                  }
                              });
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                   
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'wipEntityId',hidden:true"></th>
                    <th data-options="field:'_scheduledStartDate',width:90,align:'center'">作业日期</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'productionLine',width:110,align:'left'">产线</th>
                    <th data-options="field:'isScheduled',width:85,align:'left'">是否停产</th>
                    <th data-options="field:'ascmWipEntities_Name',width:110,align:'left'">作业号</th>
                    <th data-options="field:'requireScheduledDateSegment',width:220,align:'left',hidden:true">作业时间</th>
                    <th data-options="field:'ascmMaterialItem_DocNumber',width:90,align:'left'">装配件编码</th>
                    <th data-options="field:'ascmMaterialItem_Description',width:230,align:'left'">装配件描述</th>
                    <th data-options="field:'ascmWipScheduleGroupsName',width:60,align:'center'">计划组</th>
                    <th data-options="field:'netQuantity',width:60,align:'center'">作业数量</th>
                    <th data-options="field:'jobCompoundedPerson',width:60,align:'center'">已配料人员</th>
                    <th data-options="field:'containerQuantity',width:120,align:'center'">容器数</th>
                    <th data-options="field:'checkQuantity',width:70,align:'center'">校验数</th>
                    <th data-options="field:'subStatusCn',width:60,align:'left'">备料状态</th>
                </tr>
            </thead>
        </table>
        <div id="tb" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>作业日期：</span>
                <input class="easyui-datebox" id="startScheduledStartDate" type="text" style="width:150px;" value="" data-options="validType:'checkDate'"/>-<input class="easyui-datebox" id="endScheduledStartDate" type="text" style="width:150px;" value="" data-options="validType:'checkDate'"/>

                <span> &nbsp;作&nbsp;业&nbsp;号：</span>
                 <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startWipEntityName", width = "120px", panelWidth = 500, queryParams = "'filterWipEntityStatus':false" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endWipEntityName", width = "120px", panelWidth = 500, queryParams = "'filterWipEntityStatus':false" }); %>
                <span>供应类型：</span>
                <select id="wipSupplyType">
                    <option value="pushType">推式</option>
                    <option value="pullType">拉式</option>
                </select>
            </div>
            <div style="margin-bottom:5px;">
                <span>供应子库：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startSupplySubInventory", width = "150px" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endSupplySubInventory", width = "150px" });%>
            </div>
            <div style="margin-bottom:5px;">
                <span>物料编码：</span>
                <input class="easyui-validatebox" id="queryStartMaterialDocNumber" type="text" style="width:145px;" />-<input class="easyui-validatebox" id="queryEndMaterialDocNumber" type="text" style="width:145px;" />
                <span>&nbsp;计&nbsp;划&nbsp;组：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipScheduleGroupSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startScheduleGroup", width = "150px", queryParams = "fromMonitor:true" });%> 
                
                <a class="easyui-linkbutton" iconCls="icon-search" href="javascript:void(0)" onclick="Query()">查询</a>
                <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="CreateWmsPreparation()">退料管理</a>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
//        var queryWipEntityId = null;
//        var queryWipEntityName = null;
//        var queryWarehouseId = null;
//        var queryWipSupplyType = null;
//        var queryStartMaterialDocNumber = null;
//        var queryEndMaterialDocNumber = null;
//        var startScheduledStartDate = "", endScheduledStartDate = "";
//        var scheduleGroupName = "";

        function Query() {
            var options = $('#dgWmsMtlReturnDetail').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationMonitorList';
            //options.queryParams.pattern = "wipJob";
            options.queryParams.startScheduledStartDate = $('#startScheduledStartDate').datebox('getValue');
            options.queryParams.endScheduledStartDate = $('#endScheduledStartDate').datebox('getValue');
            options.queryParams.startWipEntityName = getWipEntityName("startWipEntityName");
            options.queryParams.endWipEntityName = getWipEntityName("endWipEntityName");
            options.queryParams.wipSupplyType = $('#wipSupplyType').val();
            options.queryParams.startSupplySubInventory = getSubInventoryId("startSupplySubInventory");
            options.queryParams.endSupplySubInventory = getSubInventoryId("endSupplySubInventory");
            options.queryParams.startMaterialDocNumber = $('#queryStartMaterialDocNumber').val();
            options.queryParams.endMaterialDocNumber = $('#queryEndMaterialDocNumber').val();
            options.queryParams.startScheduleGroupName = getScheduleGroupName("startScheduleGroup");
            options.loadMsg = '数据加载中，请稍候...';
            $('#dgWmsMtlReturnDetail').datagrid('reload');
            $('#dgWmsMtlReturnDetail').datagrid('clearChecked');
        }
        function CreateWmsPreparation() {
           //var row = $('#dgWmsMtlReturnDetail').datagrid('getSelected');
            var row = $('#dgWmsMtlReturnDetail').datagrid('getChecked');
            var wipEntityName = row.jobWipEntityName;
            var wipSupplyType = $('#wipSupplyType').val();
            if (row!="") {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsJobMtlReturnIndex/?MainId=' + row.id + '&WipEntityName=' + wipEntityName + "&WarehouseId=" + row.jobWarehouseSegment
                + "&WipSupplyType=" + wipSupplyType +'&mi=<%=Request["mi"].ToString() %>';
                parent.openTab('作业退料管理_' + wipEntityName, url);
            } else{
                //$.messager.alert('提示', '请选择物料进行退料操作！', 'info');
                $.messager.alert("提示", "请选择物料进行退料操作！", "info"); 
            return;
            }
        }
    </script>
     <%--大小写通用--%>
    <script type="text/javascript">
        //获取子库ID，大小写通用
        function getSubInventoryId(cboId) {
            var subInventoryId = $('#' + cboId).combobox('getValue');
            var g = $('#' + cboId).combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].id.toUpperCase() == subInventoryId.toUpperCase()) {
                    subInventoryId = rows[i].id;
                    break;
                }
            }
            return subInventoryId;
        }
        //获取作业名称，大小写通用
        function getWipEntityName(cboId) {
            var wipEntityName = $('#' + cboId).combobox('getValue');
            var g = $('#' + cboId).combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].wipEntityId == wipEntityName || rows[i].name.toUpperCase() == wipEntityName.toUpperCase()) {
                    wipEntityName = rows[i].name;
                    break;
                }
            }
            return wipEntityName;
        }
        //获取计划组名称，大小写通用
        function getScheduleGroupName(cboId) {
            var scheduleGroupName = $('#' + cboId).combobox('getValue');
            var g = $('#' + cboId).combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].scheduleGroupId == scheduleGroupName || rows[i].scheduleGroupName.toUpperCase() == scheduleGroupName.toUpperCase()) {
                    scheduleGroupName = rows[i].scheduleGroupName;
                    break;
                }
            }
            return scheduleGroupName;
        }
    </script>
</asp:Content>