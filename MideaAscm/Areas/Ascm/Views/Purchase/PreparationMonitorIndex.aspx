<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商备料状态实时监控
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgDeliveryOrderBatch" title="供应商备料状态实时监控" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          idField: 'id',
                          sortName: 'id',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb',
                          loadMsg: '更新数据......',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="field:'id',hidden:true"></th>
                    <th data-options="field:'batchDocNumber',width:100,align:'center'">批送货单号</th>
                </tr>
            </thead>
			<thead>
				<tr>
                    <th data-options="field:'batchBarCode',width:100,align:'center'">批条码号</th>
                    <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                    <th data-options="field:'materialDescription',width:250,align:'left'">物料描述</th>
                    <th data-options="field:'batchCreateTime',width:110,align:'center'">生成日期</th>
                    <th data-options="field:'batchSupperWarehouse',width:80,align:'center'">出货子库</th>
                    <th data-options="field:'batchWarehouseId',width:80,align:'center'">收货子库</th>
                    <th data-options="field:'batchStatusCn',width:50,align:'center'">状态</th>
                    <th data-options="field:'batWipLine',width:80,align:'center'">送货地点</th>
                    <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                    <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                    <th data-options="field:'palletBindNumber',width:80,align:'center'">托盘绑定数量</th>
                    <th data-options="field:'driverBindNumber',width:80,align:'center'">司机绑定数量</th>
                    <th data-options="field:'appointmentStartTimeShow',width:120,align:'center'">预约开始时间</th>
                    <th data-options="field:'appointmentEndTimeShow',width:120,align:'center'">预约最后时间</th>
				</tr>
			</thead>
        </table>
        <div id="tb">
            <span>供应商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx"); %>
            <span>要求送货时间：</span><input class="easyui-datebox" id="queryStartTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndTime" type="text" style="width:100px;" />
            <span>状态：</span>
            <select id="queryStatus" name="queryStatus" style="width:80px;">
                <option value=""></option>
                <% List<string> listStatusDefine = MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.GetList(); %>
                <% if (listStatusDefine != null && listStatusDefine.Count > 0){ %>
                <% foreach (string statusDefine in listStatusDefine){ %>
                <option value="<%=statusDefine %>"><%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" id="logQuery" plain="true" onclick="PreparationMonitorLogQuery();"><img alt='' border="0" src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/images/LOG16.GIF' width="16" height="16"  style="vertical-align:middle;"/>备料跟踪日志查看</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="PreparationPrint();">打印</a>
        </div>
        <%--日志查询--%>
        <div id="logQueryWindow" class="easyui-window" title="备料跟踪日志查看" style="padding: 5px;width:640px;height:480px;"
            iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
                    <table id="dataGridPreparationMonitorLogQuery" title="备料跟踪日志" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#toolBarLogQuery">
		            </table>
                    <div id="toolBarLogQuery">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="LogQueryReload();">刷新</a>
                        <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#logQueryWindow').window('close');">关闭</a> 
                    </div>
                </div>
            </div>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        var currentId = null;
        
    </script>
    <%--日志查询--%>
    <script type="text/javascript">
        $(function () {
            $('#dataGridPreparationMonitorLogQuery').datagrid({
                noheader: true,
                rownumbers: true,
                loadMsg: '加载数据...',
                columns: [[
                    { field: 'id', title: 'ID', width: 20, hidden: 'true' },
                    { field: 'triggerTime', title: '时间', width: 100, align: 'center' },
                    { field: 'eventType', title: '类型', width: 80, align: 'center' },
                    { field: 'userId', title: '经办', width: 50, align: 'center' },
                    { field: 'description', title: '描述', width: 200, align: 'left' }
                ]],
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                }
            });
        })
        function LogQueryReload() {
            $('#dataGridPreparationMonitorLogQuery').datagrid('reload');
        }
    </script>
</asp:Content>
