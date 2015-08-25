<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商送货合单查询
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
                          <% if (ViewData.Keys.Contains("supplierId")){ %>
                          queryParams: { supplierId: <%=ViewData["supplierId"] %> },
                          <%} %>
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                              ReloadDeliveryOrderBatch();
                              if (rowRec.status == '<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.confirm %>')
                                  $('#btnRecall').show();
                              else
                                  $('#btnRecall').hide();      
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
                    <th data-options="field:'driverSn',width:80,align:'center'">司机编号</th>
                    <th data-options="field:'driverPlateNumber',width:100,align:'center'">车牌号</th>
                </tr>
            </thead>
            <thead>
				<tr>                              
                    <th data-options="field:'statusCn',width:60,align:'center'">状态</th>
                    <th data-options="field:'appointmentStartTimeShow',width:120,align:'center'">最早到货时间</th>
                    <th data-options="field:'appointmentEndTimeShow',width:120,align:'center'">最晚到货时间</th>
                    <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                    <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                    <th data-options="field:'containerNumber',width:50,align:'center'">容器数量</th>
                    <th data-options="field:'palletBindNumber',width:80,align:'center'">托盘绑定数量</th>
                    <th data-options="field:'driverBindNumber',width:80,align:'center'">司机绑定数量</th>
                    <th data-options="field:'confirmor',width:80,align:'center'">确认人</th>
                    <th data-options="field:'_confirmTime',width:100,align:'center'">确认时间</th>
                    <th data-options="field:'_toPlantTime',width:100,align:'center'">到厂时间</th>
                    <th data-options="field:'receiver',width:80,align:'center'">接收人</th>
                    <th data-options="field:'_acceptTime',width:100,align:'center'">接收时间</th>
				</tr>
			</thead>
		</table>
        <div id="tbBatSumMain" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
            <span>司机：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/DriverSelectCombo.ascx"); %>
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
            </div>
            <div>
                <span>生成时间：</span><input class="easyui-datebox" id="queryStartCreateTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndCreateTime" type="text" style="width:100px;" />
                <input id="search" type="text" style="width: 100px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="DeliveryReload();">查询</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-undo" id="btnRecall" onclick="DeliveryRecall();">召回</a>
                <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="$('#printBatSumMain').window('open');">打印送货合单</a>--%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/SupplierDeliveryBatSumPrint.ascx"); %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryOrderOk();">打印合单条码</a>
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
                                      idField: 'id',
                                      sortName: 'id',
                                      sortOrder: 'asc',
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
                                <th data-options="checkbox:true"></th>
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
                                <th data-options="field:'appointmentStartTime',width:100,align:'center'">预约开始时间</th>
                                <th data-options="field:'appointmentEndTime',width:100,align:'center'">预约最后时间</th>
				            </tr>
			            </thead>
                    </table>
                    <div id="tbBatSumDetail" style="padding:5px;height:auto;">
                        <span>物料编码：</span><input id="startInventoryItemId" type="text" style="width:100px;" />-<input id="endInventoryItemId" type="text" style="width:100px;" />
                        <%--<span>送货通知单号：</span><input id="deliveryNotifyNum" type="text" style="width: 100px;" />--%>
                        <span>批单号：</span><input id="batchDocNumber" type="text" style="width: 100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="ReloadDeliveryOrderBatch();">查询</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryOrderBatchOk();">打印物料标签</a>
                    </div>
                </div>
            </div>
		</div>
	</div>
    <div id="editDeliveryOrderBatch" class="easyui-window" title="打印清单确认" style="padding: 10px;width:500px;height:300px;"
			        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				    <div class="easyui-layout" fit="true">
					    <div region="center"  border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">                            
				                <table id="dgEditDeliveryOrderBatch" style="width:100%;" class="easyui-datagrid" border="0" border="false"
					                data-options="fit: true,
                                                  rownumbers: true,
                                                  idField: 'id',
                                                  sortName: 'id',
                                                  sortOrder: 'asc',
                                                  striped: true,
                                                  onClickRow:function(rowIndex,rowRec){
                                                      clickDataGridMaterialRow(rowIndex);
                                                   },
                                                   onLoadSuccess: function(){
                                                      $(this).datagrid('clearSelections');
                                                      $(this).datagrid('clearChecked');                     
                                                  }
                                                  ">
                                      <thead>
				                           <tr>
                                                <th data-options="field:'id',hidden:true"></th>
                                                <th data-options="checkbox:true"></th>
                                                <th data-options="field:'batchDocNumber',width:100,align:'center'">批送货单号</th>
                                                <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                                                <th data-options="field:'containerUnitQuantityId',width:100,align:'center',
                                                    formatter:function(value,row){
                                                        return row.containerUnitQuantity;
                                                    },
                                                    editor:{
                                                        type:'combobox',
                                                        options:
                                                        {
                                                            valueField:'id',
                                                            textField:'containerUnitQuantity',
                                                            multiple:false,
                                                            editable:false,
                                                            mode:'remote'
                                                        }
                                                }">容器单元数</th>
                                           </tr>
                                      </thead>
                                </table>			                
					    </div>
					    <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
						    <a id="unitPrint" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="printDeliveryDetialUnit();">单元数打印</a>
                            <a id="totalPrint" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="printDeliveryDetialTotal();">总数打印</a>
						    <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDeliveryOrderBatch').window('close');">取消</a>
					    </div>
				    </div>
			    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#btnRecall').hide();  
        })
        function DeliveryReload() {
            var options = $('#dgBatSumMain').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DeliveryBatSumQueryList';
            options.queryParams.startCreateTime = $("#queryStartCreateTime").datebox('getText');
            options.queryParams.endCreateTime = $("#queryEndCreateTime").datebox('getText');
            options.queryParams.status = $('#queryStatus').val();
            options.queryParams.driverId = $('#driverSelect').combogrid('getValue');
            $('#dgBatSumMain').datagrid('reload');
            $('#dgDeliveryOrderBatch').datagrid('load', []);
            $('#btnRecall').hide();  
        }
        function ReloadDeliveryOrderBatch() {
            var options = $('#dgDeliveryOrderBatch').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailList1';
            options.queryParams.startInventoryItemId = $('#startInventoryItemId').val();
            options.queryParams.endInventoryItemId = $('#endInventoryItemId').val();
//            options.queryParams.deliveryNotifyNum = $('#deliveryNotifyNum').val();
            options.queryParams.batchDocNumber = $('#batchDocNumber').val();
            options.queryParams.mainId = currentId;
            $('#dgDeliveryOrderBatch').datagrid('reload');
        }
        function DeliveryRecall() {
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow && selectRow.status == '<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.confirm %>') {
                //2014.3.23改为按司机卡召回
                $.messager.confirm("确认", "确认召回司机卡【<font color='red'>" + selectRow.driverId + "</font>】？", function (r) {
                    if (r) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumRecallByDriver/' + selectRow.driverId;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    DeliveryReload();
                                } else {
                                    $.messager.alert('确认', '确认召回失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
                currentId = selectRow.id;
            } else {
                var statusCn = '<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.DisplayText(MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.confirm) %>';
                $.messager.alert('提示', '请选择【<font color="red">' + statusCn + '</font>】的供应商送货合单', 'info');
            }
        }
        //打印清单确认
        function DeliveryOrderOk() {
            acceptProcedureArgument();
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            var options = $('#dgEditDeliveryOrderBatch').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailList';
            options.queryParams.mainId = selectRow.id;
            $('#dgEditDeliveryOrderBatch').datagrid('reload');
            $('#editDeliveryOrderBatch').window('open');
        }

        function DeliveryOrderBatchOk() {
            acceptProcedureArgument();
            var detailIds = "";
            var selectRows = $('#dgDeliveryOrderBatch').datagrid('getSelections');
            if (selectRows) {
                var options = $('#dgEditDeliveryOrderBatch').datagrid('options');
                options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailIdList';
                $.each(selectRows, function (i, row) {
                    if (detailIds != "")
                        detailIds += ",";
                    detailIds += row.id;
                });
                options.queryParams.detailIds = detailIds;
                $('#dgEditDeliveryOrderBatch').datagrid('reload');
                $('#editDeliveryOrderBatch').window('open');
            } else {
                $.messager.alert('提示', '请选择送货批单', 'info');
            }
        }
        var LODOP;
        function print() {
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow) {
                var downloadUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/install_lodop.exe';
                LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'), downloadUrl);
                if (LODOP) {
                    var printParams = { "id": selectRow.id };
                    printPreview(printParams);
                }
            } else {
                $.messager.alert('提示', '请选择供应商送货合单', 'info');
            }
        }
        
        //供应商送货合单总数量打印
        function printDeliveryDetialTotal() {
            var selectRows = $('#dgEditDeliveryOrderBatch').datagrid('getSelections');
            var ids = "";
            $.each(selectRows, function (i, row) {
                if (ids != "")
                    ids += ",";
                ids += row.id;
            });
            if (selectRows) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/SupplierDriverDeliveryBarCodePrint.aspx?ids=' + ids;
                parent.openTab('供应商送货批单总数量打印', url);
            } else {
                $.messager.alert('提示', '请选择要打印的供应商送货合单', 'info');
            }
        }
        //供应商送货批单单元数打印
        function printDeliveryDetialUnit() {
            var selectRows = $('#dgEditDeliveryOrderBatch').datagrid('getSelections');
            var ids = "";
                $.each(selectRows, function (i, row) {
                    if (ids != "")
                        ids += ",";
                    ids += row.id;
                });
                if (selectRows) {
                    var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/SupplierDriverDeliveryDetialBarCodePrint.aspx?ids=' + ids;
                    parent.openTab('供应商送货批单单元数打印', url); 
                } else {
                    $.messager.alert('提示', '请选择要打印的供应商送货批单', 'info');
                }
            }
        
    </script>
    <%-- 单击编辑 --%>
    <script type="text/javascript">
        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgEditDeliveryOrderBatch').datagrid('validateRow', editIndex)) {
                var ed = $('#dgEditDeliveryOrderBatch').datagrid('getEditor', { index: editIndex, field: 'containerUnitQuantityId' });
                var containerUnitQuantity = $(ed.target).combobox('getText');
                $('#dgEditDeliveryOrderBatch').datagrid('getRows')[editIndex]['containerUnitQuantity'] = containerUnitQuantity;
                $('#dgEditDeliveryOrderBatch').datagrid('endEdit', editIndex);
                editIndex = undefined;

                return true;
            } else {
                return false;
            }
        }
        function clickDataGridMaterialRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgEditDeliveryOrderBatch').datagrid('selectRow', index)
							.datagrid('beginEdit', index);

                    var ed = $('#dgEditDeliveryOrderBatch').datagrid('getEditor', { index: index, field: 'containerUnitQuantityId' });
                    var mtlDoc = $('#dgEditDeliveryOrderBatch').datagrid('getRows')[index]['materialDocNumber'];
                    var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerUnitQuantityList/?supplierId=' + '<%=ViewData["supplierId"] %>' + "&materialDocNumber=" + mtlDoc;
                    $(ed.target).combobox('reload', url);

                    editIndex = index;
                } else {
                    $('#dgEditDeliveryOrderBatch').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptProcedureArgument() {
            if (endEditing()) {
                $('#dgEditDeliveryOrderBatch').datagrid('acceptChanges');
            }
        }
    </script>
</asp:Content>
