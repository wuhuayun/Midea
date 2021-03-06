﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商送货确认
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:160px;padding:0px 0px 2px 0px; overflow:hidden;overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%;padding:1px; border-bottom:1px solid #99BBE8;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DeliveryReload();">刷新</a>
                        <% if (ynWebRight.rightEdit){ %>
                        <%-- <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="DeliveryOK();">确认送货</a>--%>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="DriverBindDeliveryOk();">司机卡绑定合单确认送货</a>
                        <%} %>
                        <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="$('#printBatSumMain').window('open');">打印送货合单</a>--%>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="BatSumPrint();">打印司机卡送货时间</a>
                        <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/SupplierDeliveryBatSumPrint.ascx"); %>
	                </div>
                </div>
                <div region="center" border="false" style="padding:0px;overflow:auto;">
                    <table id="dgBatSumMain" title="" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                                    rownumbers: true,
                                    singleSelect : true,
                                    idField: 'id',
                                    sortName: 'id',
                                    sortOrder: 'asc',
                                    striped: true,
                                    <%--pagination: true,
                                    pageSize: 10,--%>
                                    toolbar: '#tbBatSumMain',
                                    loadMsg: '更新数据......',
                                    checkOnSelect:false,
                                    selectOnCheck:false,
                                    <% if (ViewData.Keys.Contains("supplierId")){ %>
                                    queryParams: { supplierId: <%=ViewData["supplierId"] %> },
                                    <%} %>
                                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumMainList',
                                    onSelect: function(rowIndex, rowRec){
                                        currentId = rowRec.id;
                                        ReloadDeliveryOrderBatch();  
                                    },
                                    onCheck: function (rowIndex,rowData) {
                                        SetBatchSumCanSelect();
                                    },
                                    onUncheck: function (rowIndex,rowData) {
                                        SetBatchSumCanSelect();
                                    },
                                    onLoadSuccess: function(){
                                        $(this).datagrid('clearSelections');
                                        $(this).datagrid('clearChecked');
                                        if (currentId) {
                                            $(this).datagrid('selectRecord', currentId);
                                        }  
                                    }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="checkbox:true">aaa</th>
					            <th data-options="field:'docNumber',width:100,align:'center'">合单号</th>
                                <th data-options="field:'driverSn',width:80,align:'center'">司机编号</th>
                                <th data-options="field:'warehouseId',width:80,align:'center'">子库</th>
                                <%--<th data-options="field:'driverPlateNumber',width:70,align:'center'">车牌号</th>--%>
                            </tr>
                        </thead>
                        <thead>
				            <tr>                              
                                <th data-options="field:'statusCn',width:60,align:'center'">状态</th>
                                <th data-options="field:'appointmentStartTimeShow',width:100,align:'center'">预约开始时间</th>
                                <th data-options="field:'appointmentEndTimeShow',width:100,align:'center'">预约最后时间</th>
                                <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                                <th data-options="field:'deliBatNumber',width:60,align:'center'">批单数量</th>
                                <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                                <%--<th data-options="field:'palletBindNumber',width:80,align:'center'">托盘绑定数量</th>--%>
                                <%--<th data-options="field:'driverBindNumber',width:80,align:'center'">司机绑定数量</th>--%>
                                <th data-options="field:'confirmor',width:80,align:'center'">确认人</th>
                                <th data-options="field:'_confirmTime',width:100,align:'center'">确认时间</th>
                                <th data-options="field:'_toPlantTime',width:100,align:'center'">到厂时间</th>
                                <th data-options="field:'receiver',width:80,align:'center'">接收人</th>
                                <th data-options="field:'_acceptTime',width:100,align:'center'">接收时间</th>
				            </tr>
			            </thead>
					</table>
                </div>
                <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/DeliveryOrderBatSumPrint.ascx"); %>
			    <div id="editDriverBindDelivery" class="easyui-window" title="配送合单提交确认" style="padding: 10px;width:500px;height:300px;"
			        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				    <div class="easyui-layout" fit="true">
					    <div region="center" id="editDriverBindDeliverynContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                            <form id="editDriverBindDeliveryForm" method="post" style="">
				                <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                <tr style="height:24px">
						                <td style="width: 20%; text-align:right;" nowrap>
							                <span>司机编号：</span>
						                </td>
						                <td style="width:80%">
							                <input class="easyui-validatebox" id="driverSn" name="driverSn" type="text" style="width:200px;background-color:#CCCCCC;" readonly="readonly"/>
							                <input id="driverId" name="driverId" type="hidden"/>
                                            <% if (ViewData.Keys.Contains("supplierId")){ %>
							                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择司机" onclick="SelectDriver(<%=ViewData["supplierId"] %>);"></a>
							                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-remove" title="移除司机" onclick="RemoveDriver();"></a>
                                            <%} %>
						                </td>
					                </tr>
				                </table>
			                </form>
                            <%-- 选择司机 --%>
                            <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/DriverSelect.ascx"); %>
					    </div>
					    <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                            <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						    <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="DriverBindDeliverySave();">提交</a>
                            <%} %>
						    <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDriverBindDelivery').window('close');">取消</a>
					    </div>
				    </div>
			    </div>
            </div>
        </div>
    </div>
    <div region="center" title="送货清单" border="true" style="padding:0px 0px 0px 0px;">
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
                    <th data-options="field:'batchBarCode',width:80,align:'center'">批条码号</th>
                    <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                    <th data-options="field:'materialDescription',width:250,align:'left'">物料描述</th>
                    <th data-options="field:'batchCreateTime',width:110,align:'center'">生成日期</th>
                    <th data-options="field:'batchSupperWarehouse',width:80,align:'center'">出货子库</th>
                    <th data-options="field:'batchWarehouseId',width:80,align:'center'">收货子库</th>
                    <th data-options="field:'batchStatusCn',width:50,align:'center'">状态</th>
                    <th data-options="field:'batWipLine',width:80,align:'center'">送货地点</th>
                    <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                    <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                    <%--<th data-options="field:'palletBindNumber',width:80,align:'center'">托盘绑定数量</th>--%>
                    <%--<th data-options="field:'driverBindNumber',width:80,align:'center'">司机绑定数量</th>--%>
                    <th data-options="field:'appointmentStartTimeShow',width:100,align:'center'">预约开始时间</th>
                    <th data-options="field:'appointmentEndTimeShow',width:100,align:'center'">预约最后时间</th>
				</tr>
			</thead>
        </table>
        <div id="tbBatSumDetail" style="padding:5px;height:auto;">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliveryOrderBatchOk();">打印物料标签</a>
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
        function DeliveryReload() {
            $('#dgBatSumMain').datagrid('reload');
            $('#dgDeliveryOrderBatch').datagrid('load', []);
        }
        function ReloadDeliveryOrderBatch() {
            var options = $('#dgDeliveryOrderBatch').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailList';
            options.queryParams.mainId = currentId;
            $('#dgDeliveryOrderBatch').datagrid('reload');
        }
        function DeliveryOK() {
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow) {
                var message = "确认合单【<font color='red'>" + selectRow.docNumber + "</font>】送货？";
                var bindNumber = selectRow.containerBindNumber + selectRow.palletBindNumber + selectRow.driverBindNumber;
                if (selectRow.totalNumber != bindNumber) {
                    //message = "合单总数量(<font color='red'>" + selectRow.totalNumber + "</font>)与绑定总数量(<font color='red'>" + bindNumber + "</font>)不相等!" + message;
                    //2014.1.2之前做过一个限制。必须绑定容器才能确认送货，现在是有部分供应商是推预约到货的，不需要绑定容器。就是对这部分供应商不需要做这个限制 刘涛让取消此限制
//                    message = "合单总数量(" + selectRow.totalNumber + ")与绑定总数量(" + bindNumber + ")不相等!";
//                    alert(message);
//                    return;
                }
                $.messager.confirm("确认", message, function (r) {
                    if (r) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumConfirmSubmit/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    DeliveryReload();
                                } else {
                                    $.messager.alert('确认', '确认送货失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
                currentId = selectRow.id;
            }else{
                $.messager.alert('提示', '请选择供应商送货合单', 'info');
            }
        }
        function DriverBindDeliveryOk() {
            $('#editDriverBindDelivery').window('open');
            $("#editDriverBindDeliveryForm").form('clear');

            currentId = "";
        }
        function DriverSelected(r) {
            if (r) {
                $('#driverSn').val(r.sn);
                $('#driverId').val(r.id);
            }
        }
        function RemoveDriver() {
            $('#driverSn').val("");
            $('#driverId').val("");
        }
        function DriverBindDeliverySave() {
            //2014.3.23改为先选择再提交
            var checkRows = $('#dgBatSumMain').datagrid('getChecked')
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请选择批送货合单！", "info");
                return;
            }
            var batchSumIds = "";
            $.each(checkRows, function (index, item) {
                if (batchSumIds)
                    batchSumIds += ",";
                batchSumIds += item.id;
            })
            //var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            //if (selectRow) {
            var message = "确认提交选择的合单送货？";
            $.messager.confirm("确认", message, function (r) {
                if (r) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDriverConfirmSubmit/?driverId=' + $('#driverId').val();
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "batchSumIds": batchSumIds },
                        success: function (r) {
                            if (r.result) {
                                $('#editDriverBindDelivery').window('close');
                                DeliveryReload();
                            } else {
                                $.messager.alert('确认', '司机确认送货失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
            //currentId = selectRow.id;
            //} else {
                //$.messager.alert('提示', '请选择一个司机卡绑定的供应商送货合单', 'info');
            //}
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
                parent.openTab('供应商送货合单总数量打印', url);
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
        function BatSumPrint() {
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert('提示', '请选择供应商送货合单', 'info');
                return;
            }
            if (selectRow.status != '<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.confirm  %>') {
                $.messager.alert('提示', '送货合单状态不正确', 'info');
                return;
            }
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/BatSumPrint.aspx?driverId=' + selectRow.driverId;
            parent.openTab('打印司机卡送货时间', url);
        }
    </script>
    <script type="text/javascript">
        var appointmentIntersectStartTime = null;
        var appointmentIntersectEndTime = null;
        var listSelectBatchSum_Hide = [];
        function SetBatchSumCanSelect() {
            GetSelectBatchSumAppointmentIntersectTime();
            if (appointmentIntersectEndTime) {
                var timeAppointmentEndTime_end = 0;
                var timeAppointmentEndTime_start = 0;
                var currentDate = new Date();
                if (!appointmentIntersectStartTime) {
                    appointmentIntersectStartTime = currentDate;
                }
                timeAppointmentEndTime_end = appointmentIntersectEndTime.getTime();
                timeAppointmentEndTime_start = appointmentIntersectStartTime.getTime();
                var rowsSelect = $('#dgBatSumMain').datagrid('getRows');
                var rowsChecked = $('#dgBatSumMain').datagrid('getChecked');
                for (var iSelect = 0; iSelect < rowsSelect.length; ) {
                    var rowChecked = false;
                    for (var iChecked = 0; iChecked < rowsChecked.length; iChecked++) {
                        if (rowsChecked[iChecked].id == rowsSelect[iSelect].id) {
                            rowChecked = true;
                            break;
                        }
                    }

                    if (!rowChecked) {
                        var appointmentEndTime = rowsSelect[iSelect].appointmentEndTime;
                        var appointmentStartTime = rowsSelect[iSelect].appointmentStartTime;
                        if (appointmentStartTime && appointmentEndTime) {
                            var sAppointmentStartTime = appointmentStartTime.replace(/-/g, "/");
                            var sAppointmentEndTime = appointmentEndTime.replace(/-/g, "/");
                            var timeAppointmentStartTime = (new Date(Date.parse(sAppointmentStartTime))).getTime();
                            var timeAappointmentEndTime = (new Date(Date.parse(sAppointmentEndTime))).getTime();
                            var currentDate = new Date();
                            //1.没有预约时间
                            //3.最晚到货时间早于当前时间的批单,该单总是可以勾选入合单
                            //4.有交集能够加入
                            if (timeAppointmentEndTime_end == 0 || timeAappointmentEndTime < currentDate ||
                                timeAppointmentStartTime <= timeAppointmentEndTime_start && timeAppointmentEndTime_start <= timeAappointmentEndTime ||
                                timeAppointmentStartTime <= timeAppointmentEndTime_end && timeAppointmentEndTime_end <= timeAappointmentEndTime ||
                                timeAppointmentEndTime_start <= timeAppointmentStartTime && timeAppointmentStartTime <= timeAppointmentEndTime_end ||
                                timeAppointmentEndTime_start <= timeAappointmentEndTime && timeAappointmentEndTime <= timeAppointmentEndTime_end) {
                                iSelect++;
                                //符合要求
                            } else {
                                //不符合要求
                                listSelectBatchSum_Hide.push(rowsSelect[iSelect]);
                                $('#dgBatSumMain').datagrid('deleteRow', iSelect);
                            }
                        } else {
                            iSelect++;
                        }
                    } else {
                        iSelect++;
                    }
                }
                /*
                //从删除的列表中重新添加进去
                for (var iBatch = 0; iBatch < listSelectBatchSum_Hide.length; ) {
                    var appointmentEndTime = listSelectBatchSum_Hide[iBatch].appointmentEndTime;
                    var appointmentStartTime = listSelectBatchSum_Hide[iBatch].appointmentStartTime;
                    var add = false;
                    if (appointmentStartTime && appointmentEndTime) {
                        var sAppointmentStartTime = appointmentStartTime.replace(/-/g, "/");
                        var sAppointmentEndTime = appointmentEndTime.replace(/-/g, "/");
                        var timeAppointmentStartTime = (new Date(Date.parse(sAppointmentStartTime))).getTime();
                        var timeAappointmentEndTime = (new Date(Date.parse(sAppointmentEndTime))).getTime();
                        if (timeAppointmentEndTime_end == 0 || timeAppointmentStartTime <= timeAppointmentEndTime_start && timeAppointmentEndTime_end <= timeAappointmentEndTime) {
                            add = true;
                        }
                    } else {
                        add = true;
                    }
                    if (add) {
                        $('#dgBatSumMain').datagrid('appendRow', listSelectBatchSum_Hide[iBatch]);
                        listSelectBatchSum_Hide.splice(iBatch, 1); //
                    } else {
                        iBatch++;
                    }
                }*/
            } else {
                if (listSelectBatchSum_Hide != null && listSelectBatchSum_Hide.length > 0) {
                    //$('#dgSelectDeliveryOrderBatch').datagrid('loadData', listSelectDeliveryOrderBatch_Hide);
                    //listSelectDeliveryOrderBatch_Hide = [];
                    for (var iBatch = 0; iBatch < listSelectBatchSum_Hide.length; ) {
                        $('#dgBatSumMain').datagrid('appendRow', listSelectBatchSum_Hide[iBatch]);
                        listSelectBatchSum_Hide.splice(iBatch, 1);
                    }
                }
            }
        }
        function GetSelectBatchSumAppointmentIntersectTime() {
            var rowsChecked = $('#dgBatSumMain').datagrid('getChecked');
            var currentDate = new Date();
            appointmentIntersectStartTime = null;
            appointmentIntersectEndTime = null;
            //获取最晚时间，并选中已选行
            //弹出的选择窗口
            for (var iChecked = 0; iChecked < rowsChecked.length; iChecked++) {
                var appointmentStartTime = rowsChecked[iChecked].appointmentStartTime;
                var appointmentEndTime = rowsChecked[iChecked].appointmentEndTime;
                if (appointmentStartTime&&appointmentEndTime) {
                    var _appointmentStartTime = appointmentStartTime.replace(/-/g, "/");
                    var _appointmentEndTime = appointmentEndTime.replace(/-/g, "/");
                    var _appointmentStartTime1 = new Date(Date.parse(_appointmentStartTime));
                    var _appointmentEndTime1 = new Date(Date.parse(_appointmentEndTime));
                    //最晚到货时间早于当前时间的批单，该批单总是可以勾选入合单，但不参加合单生成预约到货时间计算
                    if (_appointmentStartTime1 > currentDate) {
                        if (!appointmentIntersectStartTime || appointmentIntersectStartTime < _appointmentStartTime1) {
                            appointmentIntersectStartTime = _appointmentStartTime1;
                        }
                    }
                    if (_appointmentEndTime1 > currentDate) {
                        if (!appointmentIntersectEndTime || appointmentIntersectEndTime > _appointmentEndTime1) {
                            appointmentIntersectEndTime = _appointmentEndTime1;
                        }
                    }
                }
            }
            //return appointmentIntersectStartTime;
            //alert(supplierPassDuration)
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
