﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择批送货单--%>
<div id="deliveryOrderBatchSelect" class="easyui-window" title="选择批送货单" style="padding: 5px;width:840px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectDeliveryOrderBatch" title="批送货单信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              <%--checkOnSelect: false,
                              selectOnCheck: true,--%>
                              idField: 'id',
                              sortName: 'id',
                              sortOrder: 'asc',
                              striped: true,
                              toolbar: '#tbDeliveryOrderBatchSelect',
                              pagination: true,
                              pageSize: 10,
                              pageList:[10,30,50,100],
                              loadMsg: '更新数据......',
                              checkOnSelect:false,
                              selectOnCheck:false,
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  <%--DeliveryOrderBatchSelectOk();--%>
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');    
                                    try {
                                        SelectDeliveryOrderBatchLoaded();    
                                    } catch(e) { }               
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="checkbox:true"></th>
                        <%--<th data-options="field:'docNumber',width:100,align:'center'">批送货单号</th>--%>
                        <th data-options="field:'barCode',width:70,align:'center'">批条码</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <%--<th data-options="field:'barCode',width:70,align:'center'">批条码</th>--%>
                        <th data-options="field:'createTimeShow',width:110,align:'center'">生成日期</th>
                        <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                        <th data-options="field:'materialName',width:250,align:'left'">物料描述</th>
                        <th data-options="field:'supplierName',width:180,align:'left'">供应商名称</th>
                        <th data-options="field:'supperWarehouse',width:80,align:'center'">出货子库</th>
                        <th data-options="field:'warehouseId',width:80,align:'center'">收货子库</th>
                        <th data-options="field:'statusCn',width:50,align:'center'">状态</th>
                        <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                        <th data-options="field:'wipLine',width:80,align:'center'">送货地点</th>
                        <th data-options="field:'appointmentStartTime',width:100,align:'center'">预约开始时间</th>
                        <th data-options="field:'appointmentEndTime',width:100,align:'center'">预约最后时间</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbDeliveryOrderBatchSelect">
                <div style="margin-bottom:5px;">
                    <span>确认日期：</span>
                    <input class="easyui-datebox" id="startConfirmDate" type="text" style="width:120px;" value="" />-<input class="easyui-datebox" id="endConfirmDate" type="text" style="width:120px;" value="" />
                    <span>通知日期：</span>
                    <input class="easyui-datebox" id="startNotifyDate" type="text" style="width:120px;" value="" />-<input class="easyui-datebox" id="endNotifyDate" type="text" style="width:120px;" value="" />
                </div>
                <div style="margin-bottom:5px;">
                    <span>生成日期：</span>
                    <input class="easyui-datebox" id="startCreateTime" type="text" style="width:120px;" value="" />-<input class="easyui-datebox" id="endCreateTime" type="text" style="width:120px;" value="" />
                    <span>预约时间：</span>
                    <input class="easyui-datebox" id="appointmentStartTime" type="text" style="width:120px;" value="<%=(ViewData.ContainsKey("appointmentStartTime")?ViewData["appointmentStartTime"]:"") %>" />-<input class="easyui-datebox" id="appointmentEndTime" type="text" style="width:120px;" value="<%=(ViewData.ContainsKey("appointmentStartTime")?ViewData["appointmentStartTime"]:"") %>" />
                </div>
                 <div style="margin-bottom:5px;">
                    <span>物料编码：</span>
                    <input class="easyui-validatebox" id="startMaterialDocNumber" type="text" style="width:145px;" />-<input class="easyui-validatebox" id="endMaterialDocNumber" type="text" style="width:145px;" />
                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="DeliveryOrderBatchSelectSearch();"></a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="DeliveryOrderBatchSelectOk()">确认</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#deliveryOrderBatchSelect').window('close');">取消</a> 
                </div>
                <%--<input id="deliveryOrderBatchSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="DeliveryOrderBatchSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="DeliveryOrderBatchSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#deliveryOrderBatchSelect').window('close');">取消</a> --%>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    var v_supplierId = null;
    //var v_ascmStatus = null;
    function SelectDeliveryOrderBatch(supplierId, isSupplier) {
        if (isSupplier) {
            $('#dgSelectDeliveryOrderBatch').datagrid("hideColumn", "supplierName");
            var options = $('#dgSelectDeliveryOrderBatch').datagrid('options');
            options.sortName = 'appointmentEndTime';
            options.queryParams.isSupplierSelect = true
        } else {
            $('#dgSelectDeliveryOrderBatch').datagrid("showColumn", "supplierName");
        }
        $('#deliveryOrderBatchSelect').window('open');
        v_supplierId = supplierId;
        //v_ascmStatus = ascmStatus;

//        var options = $('#dgSelectDeliveryOrderBatch').datagrid('options');

//        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryOrderBatchAscxList1';
//        options.url = url;
//        options.queryParams.supplier = v_supplierId;

        //options.queryParams.ascmStatus = v_ascmStatus;
//        options.queryParams.status = '<%=MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderBatch.StatusDefine.open %>';
//        options.queryParams.startConfirmDate = $('#startConfirmDate').datebox('getText');
//        options.queryParams.endConfirmDate = $('#endConfirmDate').datebox('getText');
//        options.queryParams.startNotifyDate = $('#startNotifyDate').datebox('getText');
//        options.queryParams.endNotifyDate = $('#endNotifyDate').datebox('getText');
//        options.queryParams.startMaterialDocNumber = $('#startMaterialDocNumber').val();
//        options.queryParams.endMaterialDocNumber = $('#endMaterialDocNumber').val();
////        options.queryParams.queryWord = $('#deliveryOrderBatchSelectSearch').val();
        //$('#dgSelectDeliveryOrderBatch').datagrid('reload');
        DeliveryOrderBatchSelectSearch();
    }
    function DeliveryOrderBatchSelectSearch() {
        var options = $('#dgSelectDeliveryOrderBatch').datagrid('options');
        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryOrderBatchAscxList1';
        options.url = url;
        options.queryParams.supplier = v_supplierId;
        //options.queryParams.ascmStatus = v_ascmStatus;
        options.queryParams.status = '<%=MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderBatch.StatusDefine.open %>';
        options.queryParams.startCreateTime = $('#startCreateTime').datebox('getText');
        options.queryParams.endCreateTime = $('#endCreateTime').datebox('getText');
        options.queryParams.startConfirmDate = $('#startConfirmDate').datebox('getText');
        options.queryParams.endConfirmDate = $('#endConfirmDate').datebox('getText');
        options.queryParams.startNotifyDate = $('#startNotifyDate').datebox('getText');
        options.queryParams.endNotifyDate = $('#endNotifyDate').datebox('getText');
        options.queryParams.startMaterialDocNumber = $('#startMaterialDocNumber').val();
        options.queryParams.endMaterialDocNumber = $('#endMaterialDocNumber').val();
        options.queryParams.appointmentStartTime = $('#appointmentStartTime').datebox('getText');
        options.queryParams.appointmentEndTime = $('#appointmentEndTime').datebox('getText');
        
        $('#dgSelectDeliveryOrderBatch').datagrid('reload');
    }
    function DeliveryOrderBatchSelectOk() {
        var selectRows = $('#dgSelectDeliveryOrderBatch').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择批送货单！", "info");
            return;
        }
        try {
            DeliveryOrderBatchSelected(selectRows);    
        } catch(e) { }
        $('#deliveryOrderBatchSelect').window('close');
    }
</script>
