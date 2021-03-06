﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商送货确认
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="true" style="height:250px;padding:0px;overflow:auto;">
		<div class="easyui-panel" title="供应商送货合单" fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
        <table id="dgBatSumMain" title="" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          idField: 'id',
                          sortName: 'id',
                          sortOrder: 'asc',
                          striped: true,
                          pagination: true,
                          pageSize: 10,
                          toolbar: '#tbBatSumMain',
                          loadMsg: '更新数据......',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                              ReloadDeliveryOrderBatch();  
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="field:'id',hidden:true"></th>
                    <th data-options="field:'barcode',width:80,align:'center'">条码号</th>
					<th data-options="field:'docNumber',width:80,align:'center'">合单号</th>
                    <th data-options="field:'driverSn',width:70,align:'center'">司机编号</th>
                    <th data-options="field:'supplierName',width:160,align:'left'">供应商名称</th>
                    <th data-options="field:'driverPlateNumber',width:90,align:'center'">车牌号</th>
                </tr>
            </thead>
            <thead>
				<tr>                              
                    <th data-options="field:'statusCn',width:60,align:'center'">状态</th>
                    <th data-options="field:'appointmentStartTimeShow',width:120,align:'center'">预约开始时间</th>
                    <th data-options="field:'appointmentEndTimeShow',width:120,align:'center'">预约最后时间</th>
                    <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                    <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                    <th data-options="field:'palletBindNumber',width:80,align:'center'">托盘绑定数量</th>
                    <th data-options="field:'driverBindNumber',width:80,align:'center'">司机绑定数量</th>
                    <th data-options="field:'confirmor',width:80,align:'center'">确认人</th>
                    <th data-options="field:'_confirmTime',width:100,align:'center'">确认时间</th>
                    <th data-options="field:'_toPlantTime',width:100,align:'center'">到厂时间</th>
                    <th data-options="field:'receiver',width:80,align:'center'">接受人</th>
                    <th data-options="field:'_acceptTime',width:100,align:'center'">接受时间</th>
				</tr>
			</thead>
		</table>
        <div id="tbBatSumMain" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
            <span>供应商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx"); %>
            <span>司机：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/DriverSelectCombo.ascx"); %>
            </div>
            <div>
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
            <span>生成时间：</span><input class="easyui-datebox" id="queryStartCreateTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndCreateTime" type="text" style="width:100px;" />
            <input id="search" type="text" style="width: 100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="DeliveryReload();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="$('#printBatSumMain').window('open');">打印送货合单</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DeliveryBatSumMainExport();">导出查询报表</a>
            </div> 
        </div>
        <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/DeliveryOrderBatSumPrint.ascx"); %>
                </div>
            </div>
		</div>
    </div>
    <div region="center" split="false" border="false" title="" style="padding:2px 0px 0px 0px;overflow:auto;">
        <div class="easyui-panel" title="送货清单" fit="true" border="true">
			<div class="easyui-layout" fit="true">
                <div region="center" border="false">
        <table id="dgDeliveryOrderBatch" title="" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          idField: 'id',
                          sortName: 'id',
                          sortOrder: 'desc',
                          striped: true,
                          toolbar: '#tbBatSumDetail',
                          loadMsg: '更新数据......',
                          onLoadSuccess: function(data){
                              $(this).datagrid('clearSelections');
                              LoadWarehouseIdAndMaterialCode(data);
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
                    <th data-options="field:'batchWipLine',width:80,align:'center'">送货地点</th>
                    <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                    <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                    <th data-options="field:'palletBindNumber',width:80,align:'center'">托盘绑定数量</th>
                    <th data-options="field:'driverBindNumber',width:80,align:'center'">司机绑定数量</th>
				</tr>
			</thead>
        </table>
                <div id="tbBatSumDetail">
                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DeliveryBatSumDetailExport();">导出查询报表</a>
                </div>
                </div>
            </div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <%--<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-scrollview.js"></script>--%>
    <style type="text/css">
        .datagrid-header-rownumber,.datagrid-cell-rownumber{ width:40px; }
    </style>
    <script type="text/javascript">
        var currentId = null;
        function DeliveryReload() {
            var options = $('#dgBatSumMain').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DeliveryBatSumQueryList';
            options.queryParams.startCreateTime = $("#queryStartCreateTime").datebox('getText');
            options.queryParams.endCreateTime = $("#queryEndCreateTime").datebox('getText');
            options.queryParams.status = $('#queryStatus').val();
            //options.queryParams.supplierId = $('#supplierSelect').combogrid('getValue');
            options.queryParams.supplierId = getSupplierId();
            //options.queryParams.driverId = $('#driverSelect').combogrid('getValue');
            options.queryParams.driverId = getDriverId();
            options.queryParams.queryWord = $('#search').val();
            $('#dgBatSumMain').datagrid('reload');
            $('#dgDeliveryOrderBatch').datagrid('loadData', []);
        }
        function ReloadDeliveryOrderBatch() {
            var options = $('#dgDeliveryOrderBatch').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailList';
            options.queryParams.mainId = currentId;
            $('#dgDeliveryOrderBatch').datagrid('reload');

            /*$.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailList/',
                type: "post",
                dataType: "json",
                data: {
                    "mainId": currentId
                },
                beforeSend: function () {
                    var options = $('#dgDeliveryOrderBatch').datagrid('options');
                    options.loadMsg = '数据加载中，请稍候...';
                    $('#dgDeliveryOrderBatch').datagrid('loading');
                },
                success: function (r) {
                    $('#dgDeliveryOrderBatch').datagrid('loaded');
                    if (r.result) {
                        $('#dgDeliveryOrderBatch').datagrid('loadData', r.rows);
                    } else {
                        $.messager.alert('错误', '加载批送货单失败', 'error');
                    }
                }
            });*/
        }
        function DeliveryBatSumMainExport() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DeliveryBatSumExport/';
            var params = {
                startCreateTime: $("#queryStartCreateTime").datebox('getText'),
                endCreateTime: $("#queryEndCreateTime").datebox('getText'),
                status: $('#queryStatus').val(),
                supplierId: getSupplierId(),
                driverId: getDriverId(),
                queryWord: $('#search').val()
            };
            var iframe = document.createElement("iframe");
            iframe.src = url + "?" + $.param(params);
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
        function DeliveryBatSumDetailExport() {
            var row = $('#dgBatSumMain').datagrid('getSelected');
            if (row) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DeliveryBatDetailExport/';
                var params = {
                    mainId: currentId
                };
                var iframe = document.createElement("iframe");
                iframe.src = url + "?" + $.param(params);
                iframe.style.display = "none";
                document.body.appendChild(iframe);
            } else {
                $.messager.alert('提示', '请选择供应商送货合单', 'info');
            }
        }
    </script>
</asp:Content>
