<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnDepartment>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	批送货合单管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--供应商送货合单明细-->
	<div region="center" split="false" border="false" title="" style="padding:2px 0px 0px 0px;overflow:auto;">
        <div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true">
                <div region="center" border="false">
                    <table id="dgBatSumDetail" title="送货合单明细" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                                      idField: 'id',
                                      sortName: 'id',
                                      sortOrder: 'asc',
                                      striped: 'true',
                                      toolbar: '#tbBatSumDetail',
                                      loadMsg: '加载数据......',
                                      onLoadSuccess: function () {
                                          $(this).datagrid('clearSelections');
                                      }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="field:'batchDocNumber',width:100,align:'center'">批送货单号</th>
                            </tr>
                        </thead>
				        <thead>
					        <tr>
                                <th data-options="field:'batchBarCode',width:100,align:'center'">批条码号</th>
                                <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                                <th data-options="field:'materialDescription',width:250,align:'left'">物料描述</th>
                                <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                                <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                                <%--<th data-options="field:'palletBindNumber',width:80,align:'center'">托盘绑定数量</th>--%>
                                <%--<th data-options="field:'driverBindNumber',width:80,align:'center'">司机绑定数量</th>--%>
                                <th data-options="field:'appointmentStartTimeShow',width:100,align:'center'">预约开始时间</th>
                                <th data-options="field:'appointmentEndTimeShow',width:100,align:'center'">预约最后时间</th>
					        </tr>
				        </thead>
			        </table>
                    <div id="tbBatSumDetail">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DetailReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" id="btnDetailAdd" onclick="DetailAdd();">添加</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" id="btnDetailRemove" onclick="DetailRemove();">移除</a>
                        <%} %>
                    </div>
                    <%-- 选择批送货单 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierDeliveryOrderBatchSelect.ascx"); %>
                </div>
            </div>
		</div>
	</div>
    <!--供应商送货合单-->
	<div region="north" title="" border="true" style="height:200px;padding:0px;overflow:auto;">
		<div class="easyui-panel" title="供应商送货合单管理 " fit="true" border="false">
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
                                      <%--pagination: true,
                                      pageSize: 50,--%>
                                      toolbar: '#tbBatSumMain',
                                      loadMsg: '更新数据......',
                                      <% if (ViewData.Keys.Contains("supplierId")){ %>
                                      queryParams: { supplierId: <%=ViewData["supplierId"] %> },
                                      <%} %>
                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumMainList',
                                      onSelect: function(rowIndex, rowRec){
                                          currentId = rowRec.id;
                                          DetailReload();
                                          SetButton();  
                                      },
                                      onDblClickRow: function(rowIndex, rowRec){
                                          <% if (ynWebRight.rightEdit){ %>
                                          MainEdit();
                                          <%} %>  
                                      },
                                      onLoadSuccess: function(data){
                                          $(this).datagrid('clearSelections');
                                          if (data.rows.length == 0){
                                              currentId = null;    
                                          }
                                          if (currentId) {
                                              $(this).datagrid('selectRecord', currentId);
                                          }
                                      }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="field:'id',hidden:true"></th>
					            <th data-options="field:'docNumber',width:90,align:'center'">合单号</th>
					            <%--<th data-options="field:'supplierName',width:180,align:'left'">供应商名称</th>--%>
                                <th data-options="field:'driverSn',width:80,align:'center'">司机编号</th>
                                <th data-options="field:'driverPlateNumber',width:70,align:'center'">车牌号</th>
                            </tr>
                        </thead>
                        <thead>
				            <tr>                              
                                <th data-options="field:'statusCn',width:60,align:'center'">状态</th>
                                <th data-options="field:'warehouseId',width:80,align:'center'">仓库</th>
                                <th data-options="field:'appointmentStartTimeShow',width:100,align:'center'">最早到货时间</th>
                                <th data-options="field:'appointmentEndTimeShow',width:100,align:'center'">最晚到货时间</th>
                                <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                                <th data-options="field:'containerBindNumber',width:80,align:'center'">容器绑定数量</th>
                                <th data-options="field:'containerNumber',width:50,align:'center'">容器数量</th>
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
                    <div id="tbBatSumMain">
                        <input id="MainSearch" type="text" style="width:100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="MainQuery();">查询</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="MainAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" id="btnMainEdit" onclick="MainEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" id="btnMainDelete" onclick="MainDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editBatSumMain" class="easyui-window" title="批送货合单" style="padding: 10px;width:500px;height:300px;"
			            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
					        <div region="center" id="editBatSumMainContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                                <form id="editBatSumMainForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                        <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>仓库：</span>
						                    </td>
						                    <td style="width:80%">
                                                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "warehouseId", width = "205px" });%>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>合单号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input id="docNumber" name="docNumber" type="text" style="width:200px;background-color:#CCCCCC;" readonly="readonly"/>
						                    </td>
					                    </tr>
                                        
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <%--<span>司机编号：</span>--%>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="driverSn" name="driverSn" type="text" style="width:200px;background-color:#CCCCCC;visibility:hidden;" readonly="readonly" />
							                    <input id="driverId" name="driverId" type="hidden"/>
                                                <%-- <% if (ViewData.Keys.Contains("supplierId")){ %>
							                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择司机" onclick="SelectDriver(<%=ViewData["supplierId"] %>);"></a>
							                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-remove" title="移除司机" onclick="RemoveDriver();"></a>
                                                <%} %>--%>
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
                                <%-- 选择司机 --%>
                                <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/DriverSelect.ascx"); %>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="MainSave();">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editBatSumMain').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
			    </div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <!--批送货合单-->
    <script type="text/javascript">
        var currentId = null;
        function SetButton() {
            $('#btnMainEdit').show();
            $('#btnMainDelete').show();
            $('#btnDetailAdd').show();
            $('#btnDetailRemove').show();
            $('#MainSave').show();
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow) {
                if (selectRow.status == '<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.confirm %>') {
                    $('#btnMainEdit').hide();
                    $('#btnMainDelete').hide();
                    $('#btnDetailAdd').hide();
                    $('#btnDetailRemove').hide();
                    $('#MainSave').hide();
                }
            }
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
        function MainQuery() {
            var queryParams = $('#dgBatSumMain').datagrid('options').queryParams;
            queryParams.queryWord = $('#MainSearch').val();
            $('#dgBatSumMain').datagrid('reload');
            $('#dgBatSumDetail').datagrid('load', []);
        }
        function MainAdd() {
            SetButton();
            $('#editBatSumMain').window('open');
            $("#editBatSumMainForm").form('clear');
            $('#warehouseId').combogrid('readonly', false); //easyui版本为1.3.3及以上

            currentId = "";
        }
        function MainEdit() {
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow) {
                $('#editBatSumMain').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumMainEdit/' + currentId;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editBatSumMainForm").form('clear');
                            $('#docNumber').val(r.docNumber);
                            $('#driverSn').val(r.driverSn);
                            $('#driverId').val(r.driverId);
                            $('#warehouseId').combogrid('setValue', r.warehouseId);
                            $('#warehouseId').combogrid('readonly', true); //easyui版本为1.3.3及以上
                        }
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择供应商送货合单', 'info');
            }
        }
        function MainSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editBatSumMainForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumMainSave/' + currentId,
                        onSubmit: function () {
                            return $('#editBatSumMainForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editBatSumMain').window('close');
                                if (rVal.id)
                                    currentId = rVal.id;
                                MainQuery();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function MainDelete() {
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除供应商送货合单[<font color="red">' + selectRow.docNumber + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumMainDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    MainQuery();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的供应商送货合单', 'info');
            }
        }
    </script>
    <!--批送货合单明细-->
    <script type="text/javascript">
        function DetailReload() {
            var title = '送货合单明细';
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow) {
                title = '送货合单[<font color="red">' + selectRow.docNumber + '</font>]明细';
            }
            var options = $('#dgBatSumDetail').datagrid('options');
            options.title = title;
            options.queryParams.mainId = currentId;
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailList';
            $('#dgBatSumDetail').datagrid('reload');
        }
        function DetailAdd() {
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请先选择供应商送货合单！", "info");
                return;
            }
            SelectDeliveryOrderBatch(currentId,selectRow.supplierId, true, selectRow.warehouseId);
        }
        function DeliveryOrderBatchSelected(selectRows) {
            if (selectRows) {
                $.messager.confirm('确认', '确认添加选择的批送货单到合单？', function (result) {
                    if (result) {
                        var selectRow = $('#dgBatSumMain').datagrid('getSelected');
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailAdd/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data: { "appointmentTimeFilter":$("#appointmentTimeFilter").prop("checked"),"deliveryOrderBatchJson": $.toJSON(selectRows) },
                            success: function (r) {
                                if (r.result) {
                                    MainQuery();
                                } else {
                                    $.messager.alert('确认', '添加失败:' + r.message, 'error');
                                }
                            }

                        });
                    }
                });
            }
        }
        function DetailRemove() {
            var selectRows = $('#dgBatSumDetail').datagrid('getSelections');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请选择要移除的批送货单！', 'info');
                return;
            }
            var detailIds = "";
            for (var i = 0; i < selectRows.length; i++) {
                if (detailIds) {
                    detailIds += ",";
                }
                detailIds += selectRows[i].id;
            }
            $.messager.confirm('确认', '确认从合单中移除选择的批送货单?', function (result) {
                if (result) {
                    var selectRow = $('#dgBatSumMain').datagrid('getSelected');
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/SupplierDeliveryBatSumDetailRemove/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { detailIds: detailIds },
                        success: function (r) {
                            if (r.result) {
                                MainQuery();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
    <!--批送货单选择处理-->
    <script type="text/javascript">
        var CanTriggerCheck = false;
        var timeAppointmentEndTime_end = 0;
        var timeAppointmentEndTime_start = 0;
        function SelectDeliveryOrderBatchLoaded() {
            CanTriggerCheck = false;
            var rowsDetail = $('#dgBatSumDetail').datagrid('getRows');
            var rowsSelect = $('#dgSelectDeliveryOrderBatch').datagrid('getRows');
            //获取最晚时间，并选中已选行
            for (var iDetail = 0; iDetail < rowsDetail.length; iDetail++) {
                for (var iSelect = 0; iSelect < rowsSelect.length; iSelect++) {
                    if (rowsDetail[iDetail]["batchBarCode"] == rowsSelect[iSelect]["barCode"]) {
                        $('#dgSelectDeliveryOrderBatch').datagrid("checkRow", iSelect);
                    }
                }
            }
            setTimeout(function () { CanTriggerCheck = true; SetDeliveryOrderBatchCanSelect(); }, 100);
        }
        function SetDeliveryOrderBatchCanSelect(unCheckAll) {
            var appointmentTimeFilter_Check = document.getElementById("appointmentTimeFilter");
            if (!appointmentTimeFilter_Check.checked)
                return;
            if (!CanTriggerCheck)
                return;
            if (!supplierPassDuration)
                return;
            var appointmentEndTime_Last = null;
            if (!unCheckAll)
                appointmentEndTime_Last = GetSelectDeliveryOrderBatchAppointmentEndTimeLast();
            //alert(appointmentEndTime_Last);
            if (appointmentEndTime_Last) {
                timeAppointmentEndTime_end = appointmentEndTime_Last.getTime();
                timeAppointmentEndTime_start = timeAppointmentEndTime_end - supplierPassDuration * 60 * 1000;
                var rowsSelect = $('#dgSelectDeliveryOrderBatch').datagrid('getRows');
                var rowsChecked = $('#dgSelectDeliveryOrderBatch').datagrid('getChecked');
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
                            var currentDate=new Date();
                            //1.没有预约时间
                            //2.预约起止时间相等
                            //3.最晚到货时间早于当前时间的批单，该批单总是可以勾选入合单
                            //4.包含T（LM）- Tc至 T（LM）时间段的批单能够加入至合单
                            if (timeAppointmentEndTime_end == 0 || appointmentStartTime == appointmentEndTime || timeAappointmentEndTime < currentDate ||
                                timeAppointmentStartTime <= timeAppointmentEndTime_start && timeAppointmentEndTime_start <= timeAappointmentEndTime ||
                                timeAppointmentStartTime <= timeAppointmentEndTime_end && timeAppointmentEndTime_end <= timeAappointmentEndTime||
                                timeAppointmentEndTime_start <= timeAppointmentStartTime && timeAppointmentStartTime <= timeAppointmentEndTime_end ||
                                timeAppointmentEndTime_start <= timeAappointmentEndTime && timeAappointmentEndTime <= timeAppointmentEndTime_end) {
                                iSelect++;
                                //符合要求
                            } else {
                                //不符合要求
                                listSelectDeliveryOrderBatch_Hide.push(rowsSelect[iSelect]);
                                $('#dgSelectDeliveryOrderBatch').datagrid('deleteRow', iSelect);
                            }
                        }
                    } else {
                        iSelect++;
                    }
                }
                //从删除的列表中重新添加进去
                for (var iBatch = 0; iBatch < listSelectDeliveryOrderBatch_Hide.length; ) {
                    var appointmentEndTime = listSelectDeliveryOrderBatch_Hide[iBatch].appointmentEndTime;
                    var appointmentStartTime = listSelectDeliveryOrderBatch_Hide[iBatch].appointmentStartTime;
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
                        $('#dgSelectDeliveryOrderBatch').datagrid('appendRow', listSelectDeliveryOrderBatch_Hide[iBatch]);
                        listSelectDeliveryOrderBatch_Hide.splice(iBatch, 1); //
                    } else {
                        iBatch++;
                    }
                }
            } else {
                if (listSelectDeliveryOrderBatch_Hide != null && listSelectDeliveryOrderBatch_Hide.length > 0) {
                    //$('#dgSelectDeliveryOrderBatch').datagrid('loadData', listSelectDeliveryOrderBatch_Hide);
                    //listSelectDeliveryOrderBatch_Hide = [];
                    for (var iBatch = 0; iBatch < listSelectDeliveryOrderBatch_Hide.length; ) {
                        $('#dgSelectDeliveryOrderBatch').datagrid('appendRow', listSelectDeliveryOrderBatch_Hide[iBatch]);
                        listSelectDeliveryOrderBatch_Hide.splice(iBatch, 1); 
                    }
                }
            }
        }
        function GetSelectDeliveryOrderBatchAppointmentEndTimeLast() {
            var rowsDetail = $('#dgBatSumDetail').datagrid('getRows');
            var rowsChecked = $('#dgSelectDeliveryOrderBatch').datagrid('getChecked');
            var appointmentEndTime_Last = null;
            var currentDate = new Date();
            var selectRow = $('#dgBatSumMain').datagrid('getSelected');
            if (selectRow && selectRow.appointmentEndTime) {
                var _appointmentEndTime = selectRow.appointmentEndTime.replace(/-/g, "/");
                appointmentEndTime_Last = new Date(Date.parse(_appointmentEndTime));
            }
            /*不能这样取明显，改为直接去合单的时间
            //合单中已添加批单
            for (var iDetail = 0; iDetail < rowsDetail.length; iDetail++) {
                var appointmentEndTime = rowsDetail[iDetail].appointmentEndTime;
                if (appointmentEndTime) {
                    var _appointmentEndTime = appointmentEndTime.replace(/-/g, "/");
                    var _appointmentEndTime1 = new Date(Date.parse(_appointmentEndTime));
                    //最晚到货时间早于当前时间的批单，该批单总是可以勾选入合单，但不参加合单生成预约到货时间计算
                    if (_appointmentEndTime1 < currentDate)
                        continue;
                    if (!appointmentEndTime_Last || appointmentEndTime_Last < _appointmentEndTime1) {
                        appointmentEndTime_Last = _appointmentEndTime1;
                    }
                }
            }
            */
            //获取最晚时间，并选中已选行
            //弹出的选择窗口
            for (var iChecked = 0; iChecked < rowsChecked.length; iChecked++) {
                var appointmentEndTime = rowsChecked[iChecked].appointmentEndTime;
                if (appointmentEndTime) {
                    var _appointmentEndTime = appointmentEndTime.replace(/-/g, "/");
                    var _appointmentEndTime1 = new Date(Date.parse(_appointmentEndTime));
                    //最晚到货时间早于当前时间的批单，该批单总是可以勾选入合单，但不参加合单生成预约到货时间计算
                    if (_appointmentEndTime1 > currentDate)
                        if (!appointmentEndTime_Last || appointmentEndTime_Last < _appointmentEndTime1) {
                            appointmentEndTime_Last = _appointmentEndTime1;
                    }
                }
            }
            return appointmentEndTime_Last;
            //alert(supplierPassDuration)
        }
    </script>
</asp:Content>
