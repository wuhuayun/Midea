<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.FromErp.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料来料接收
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:240px;padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="false" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
                    <table id="dgBatSumMain" title="" class="easyui-datagrid" style="" border="false" toolbar="#toolbar_dgBatSumMain"
                        data-options="fit: true,
                                      rownumbers: true,
                                      singleSelect : true,
                                      idField: 'id',
                                      sortName: 'docNumber',
                                      sortOrder: 'desc',
                                      striped: true,
                                      pagination: true,
                                      pageSize: 30,
                                      loadMsg: '更新数据......',
                                      rowStyler: function (index, rowRec) {
                                          if (rowRec.checkContainerQuantity < rowRec.containerQuantity) {
                                                return 'color:red;';
                                          }
                                      },
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
					            <th data-options="field:'docNumber',width:100,align:'center'">合单号</th>
                                <th data-options="field:'supplierDocNumberCn',width:70,align:'center'">供方编码</th>
                            </tr>
                        </thead>
                        <thead>
				            <tr>
                                <th data-options="field:'supplierNameCn',width:70,align:'center'">供方名称</th>
                                <th data-options="field:'warehouseId',width:70,align:'center'">送货仓库</th>                              
                                <th data-options="field:'statusCn',width:60,align:'center'">状态</th>
                                <th data-options="field:'containerQuantity',width:50,align:'center'">容器数</th>
                                <th data-options="field:'checkContainerQuantity',width:70,align:'center'">入库容器数</th>
                                <th data-options="field:'totalQuantity',width:50,align:'center'">总数</th>
                                <th data-options="field:'checkQuantity',width:50,align:'center'">入库数</th>
                                <th data-options="field:'containerBindQuantity',width:70,align:'center'">容器绑定数</th>
                                <th data-options="field:'appointmentStartTimeShow',width:110,align:'center'">最早到货时间</th>
                                <th data-options="field:'appointmentEndTimeShow',width:110,align:'center'">最晚到货时间</th>
                                <th data-options="field:'_toPlantTime',width:110,align:'center'">到厂时间</th>
                                <th data-options="field:'receiver',width:70,align:'center'">接收人</th>
                                <th data-options="field:'_acceptTime',width:110,align:'center'">接收时间</th>
								<th data-options="field:'uploadStatusCn',width:110,align:'center'">上传状态</th>
								<th data-options="field:'uploadTimeShow',width:110,align:'center'">上传日期</th>
				            </tr>
			            </thead>
		            </table>
					<div id="toolbar_dgBatSumMain">
						<div>
							<span>合单条码：</span>
                            <input type="text" id="barcode" name="barcode" style="width:90px;" value=""/>
                            <span>合单号：</span>
                            <input type="text" id="docNumber" name="docNumber" style="width:120px;" value=""/>
                            <span>供应商：</span>
                            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "210px", panelWidth = 600 }); %>
                            
						</div>
						<div>
							<span>更新时间：</span>
							<input class="easyui-datebox" id="queryStartModifyTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndModifyTime" type="text" style="width:100px;" />

							<span> 上传状态： </span>
								<select id="queryReturnCode" name="queryReturnCode" style="width:80px;">
									<option value="" selected="selected">所有</option>
									<option value="0" >正常</option>
									<option value="-1" >异常</option>
								</select>
								<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
                                <a href="javascript:void(0);" class="easyui-linkbutton" id="exportMainToExcel" plain="true" icon="icon-print" onclick="ExportMainExcel();">合单导出</a>
                                <a href="javascript:void(0);" class="easyui-linkbutton" id="exportToExcel" plain="true" icon="icon-print" onclick="ExportExcel();">合单明细导出</a>
						</div>
					</div>
                </div>
            </div>
        </div>
    </div>
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <!--送货合单明细-->
        <table id="dgDeliveryOrderBatch" title="" style="" border="false"></table>
        <div id="tb">
            <span>收货子库：</span>
            <select id="warehouseIdSelect" onchange="loadDeliveryOrderBatch();" style="width:120px;"></select>
            <span>物料大类：</span>
            <select id="materialDocNumberSelect" onchange="loadDeliveryOrderBatch();" style="width:100px;"></select>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%=Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/jquery/howler.min.js"></script>
    <%--<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>--%>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-scrollview.js"></script>
    <style type="text/css">
        .datagrid-header-rownumber,.datagrid-cell-rownumber{ width:40px; }
    </style>
    <%-- 加载送货合单 --%>
    <script type="text/javascript">
        $(function () {
            $('#barcode').keypress(function (e) {
                var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keyCode == 13) {
                    sound.stop().play();
                    Query();
                }
            })
            $('#docNumber').keypress(function (e) {
                var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keyCode == 13) {
                    sound.stop().play();
                    Query();
                }
            })
            $('#barcode').focus();
        });
        var currentId = null;
        var deliveryOrderBatchData = null;
        var isServerReload = false;
        var sound = new Howl({ urls: ['../../Content/welcome.mp3'] });
        function Query() {
            var options = $('#dgBatSumMain').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/IncAccRecDeliBatSumList';
            //options.queryParams.supplierId = $('#supplierSelect').combogrid('getValue');
            options.queryParams.supplierId = getSupplierId();
            options.queryParams.barcode = $('#barcode').val();
            options.queryParams.docNumber = $('#docNumber').val();
            options.queryParams.startModifyTime = $('#queryStartModifyTime').datebox('getText');
            options.queryParams.endModifyTime = $('#queryEndModifyTime').datebox('getText');
            options.queryParams.returnCode = $('#queryReturnCode').val();
            $('#dgBatSumMain').datagrid('reload');
        }
        var currentDetailId = null;
        function ReloadDeliveryOrderBatch() {
            isServerReload = true;
            var options = $('#dgDeliveryOrderBatch').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/IncomingAcceptanceDetailList';
            options.queryParams.mainId = currentId;
            options.queryParams.barcode = $('#barcode').val();
            $('#dgDeliveryOrderBatch').datagrid('reload');
        }
        function LoadWarehouseIdAndMaterialCode() {
            $("#warehouseIdSelect").empty();
            $("#materialDocNumberSelect").empty();
            $("#warehouseIdSelect").append("<option value=''></option>");
            $("#materialDocNumberSelect").append("<option value=''></option>");
            var warehouseIds = [];
            var materialCodes = [];
            if (deliveryOrderBatchData && deliveryOrderBatchData.rows.length > 0) {
                $.each(deliveryOrderBatchData.rows, function (index, item) {
                    if ($.inArray(item.batchWarehouseId, warehouseIds) == -1) {
                        warehouseIds.push(item.batchWarehouseId);
                        $("#warehouseIdSelect").append("<option value='" + item.batchWarehouseId + "'>" + item.batchWarehouseId + "</option>");
                    }
                    var materialCode = item.materialDocNumber.substr(0, 4);
                    if ($.inArray(materialCode, materialCodes) == -1) {
                        materialCodes.push(materialCode);
                        $("#materialDocNumberSelect").append("<option value='" + materialCode + "'>" + materialCode + "</option>");
                    }
                })
            }
        }
        function loadDeliveryOrderBatch() {
            isServerReload = false;
            var warehouseId = $("#warehouseIdSelect").val();
            var materialCode = $("#materialDocNumberSelect").val();
            var newData = {
                "total": 0,
                "rows": []
            };
            if (deliveryOrderBatchData && deliveryOrderBatchData.rows.length > 0) {
                $.each(deliveryOrderBatchData.rows, function (index, item) {
                    var addByWarehouseId = !warehouseId || (warehouseId && warehouseId == item.batchWarehouseId);
                    var addByMaterialCode = !materialCode || (materialCode && materialCode == item.materialDocNumber.substr(0, 4));
                    if (addByWarehouseId && addByMaterialCode) {
                        newData.total += 1;
                        newData.rows.push(item);
                    }
                })
            }
            $('#dgDeliveryOrderBatch').datagrid('loadData', newData);
        }
    </script>
    <%-- 加载送货合单明细 --%>
    <script type="text/javascript">
        $(function () {
            $('#dgDeliveryOrderBatch').datagrid({
                fit: true,
                rownumbers: true,
                singleSelect: true,
                idField: 'id',
                sortName: 'id',
                sortOrder: 'asc',
                striped: true,
                toolbar: '#tb',
                pageSize: 30,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', hidden: true },
                    { field: 'ascmStatusCn', title: '执行结果', width: 60, align: 'center' },
                    { field: 'batchBarCode', title: '批条码号', width: 70, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 90, align: 'center' }
                ]],
                columns: [[
                    { field: 'materialDescription', title: '物料描述', width: 230, align: 'left' },
                    { field: 'totalNumber', title: '送货数', width: 50, align: 'center' },
                    { field: 'checkQuantity', title: '校验数', width: 50, align: 'center',
                        styler: function (value, row, index) {
                            if (value < row.totalNumber)
                                return 'color:red;';
                        }
                    },
                    { field: 'receivedQuantity', title: '接收数', width: 50, align: 'center' },
                    { field: 'batchWarehouseId', title: '收货子库', width: 90, align: 'center' },
                    { field: 'assignWarelocation', title: '货位分配', width: 100 },
                    { field: 'containerBindNumber', title: '容器绑定数', width: 70, align: 'center' },
                    { field: 'batchCreateTime', title: '生成日期', width: 110, align: 'center' },
                    { field: 'batchStatusCn', title: '状态', width: 60, align: 'center' },
                    { field: 'uploadStatusCn', title: '上传状态', width: 110, align: 'center' },
                    { field: 'uploadTimeShow', title: '上传日期', width: 110, align: 'center' }
                ]],
                rowStyler: function (index, rowRec) {
                    if (rowRec.ascmStatusCn == '<%=AscmDeliveryOrderBatch.AscmStatusDefine.DisplayText(AscmDeliveryOrderBatch.AscmStatusDefine.received)%>') {
                        return 'color:green;';
                    } else if (rowRec.ascmStatusCn == '<%=AscmDeliveryOrderBatch.AscmStatusDefine.DisplayText(AscmDeliveryOrderBatch.AscmStatusDefine.receiveFail)%>') {
                        return 'color:red;';
                    } else if (rowRec.ascmStatusCn == '<%=AscmDeliveryOrderBatch.AscmStatusDefine.DisplayText(AscmDeliveryOrderBatch.AscmStatusDefine.enterDoor)%>') {
                        return 'color:blue;';
                    }
                }
            });
        })
    </script>
     <%-- 导出 --%>
    <script type="text/javascript">
        //送货合单明细
        function ExportExcel() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/IncAccExportDetail';
            var rowsDetail = $('#dgDeliveryOrderBatch').datagrid('getRows');
            var rowsMain = $('#dgBatSumMain').datagrid('getSelected');
            if (rowsDetail == null || rowsDetail.length == 0) {
                $.messager.alert('确认', '没有合单明细!', 'error');
                return;
            }
            var params = {
                mainId: rowsMain.id,
                docNumber: rowsMain.docNumber
            };
            var iframe = document.createElement("iframe");
            iframe.src = url + "?" + $.param(params);
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
        //送货合单
        function ExportMainExcel() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/IncAccExportMain';
            var rowsMains = $('#dgBatSumMain').datagrid('getRows');
            if (rowsMains == null || rowsMains.length == 0) {
                $.messager.alert('确认', '没有合单!', 'error');
                return;
            }
            var supplierId = getSupplierId();
            var barcode = $('#barcode').val();
            var docNumber = $('#docNumber').val();
            var startModifyTime = $('#queryStartModifyTime').datebox('getText');
            var endModifyTime = $('#queryEndModifyTime').datebox('getText');
            var returnCode = $('#queryReturnCode').val();
            var params = {
                 'barcode': barcode,
                 'docNumber': docNumber,
                 'supplierId': supplierId,
                 'returnCode': returnCode,
                 'startModifyTime': startModifyTime,
                 'endModifyTime': endModifyTime
            };
            var iframe = document.createElement("iframe");
            iframe.src = url + "?" + $.param(params);
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>
</asp:Content>
