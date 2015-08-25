<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsBackInvoiceMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商退货
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:180px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" id="mainSave" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="Save();">退货确认</a>
                        <%} %>
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
                    <form id="editWmsBackInvoiceMainForm" method="post" style="">
                        <table style="width:740px;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>退货单号：</span>
						        </td>
						        <td style="width:160px">
                                    <input type="text" name="docNumber" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.docNumber%>"/>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>手工退货单号：</span>
						        </td>
						        <td style="width:160px">
							        <input class="easyui-validatebox" type="text" name="manualDocNumber" style="width:120px;" required="true" value="<%=Model.manualDocNumber%>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
                                    <span style="display:none;">状态：</span>
                                </td>
						        <td style="width:180px">
                                    <input id="statusCn" name="statusCn" type="text" style="width:120px;background-color:#F0F0F0;display:none;" readonly="readonly" value="<%=Model.statusCn%>"/>
                                    <input id="status" name="status" type="hidden" value="<%=Model.status %>"/>
                                </td>
					        </tr>
                            <tr>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>供方编码：</span>
						        </td>
                                <td style="width:160px">
                                    <input type="text" id="supplierDocNumber" name="supplierDocNumber" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.supplierDocNumber%>"/>
                                    <input type="hidden" id="supplierId" name="supplierId" value="<%=Model.supplierId %>"/>
                                    <a href="javascript:void(0);" id="selectSupplier" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择供应商" onclick="SelectSupplier();"></a>
                                </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
                                    <span>供方名称：</span>
                                </td>
                                <td>
                                    <input type="text" id="supplierName" name="supplierName" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.supplierName%>"/>
                                </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
                                    <span>状态：</span>
                                </td>
                                <td>
                                    <input type="checkbox" id="lm_deliveried" name="accountStatus" value="lm_deliveried"/>已交货
                                    <input type="checkbox" id="lm_received" name="accountStatus" value="lm_received"/>已接收
                                </td>
                            </tr>
                            <tr style="height:24px">
						       <%-- <td style="text-align:right;" nowrap="nowrap">
							        <span>默认仓库：</span>
						        </td>
						        <td>
                                    <input id="warehouseId" name="warehouseId" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.warehouseId%>"/>
							        <a href="javascript:void(0);" id="selectWarehouse" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择仓库" onclick="SelectWarehouse();"></a>
						        </td>--%>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>退货原因：</span>
						        </td>
						        <td>
                                    <input id="reasonName" name="reasonName" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.reasonName%>"/>
                                    <input id="reasonId" name="reasonId" type="hidden" value="<%=Model.reasonId %>"/>
							        <a href="javascript:void(0);" id="selectReason" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择原因" onclick="SelectReason();"></a>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
                                    <span>责任人：</span>
                                </td>
						        <td style="width:160px">
                                    <input id="responsiblePerson" name="responsiblePerson" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.responsiblePerson%>"/>
                                </td>
					        </tr>
                            <tr>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>备注：</span>
						        </td>
                                <td colspan="5">
                                    <input type="text" id="memo" name="memo" style="width:480px;" value="<%=Model.memo %>"/>
                                </td>
                            </tr>
							 <tr>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>上传状态：</span>
						        </td>
                                <td colspan="5">
                                    <input type="text" id="uploadStatusCn" name="uploadStatusCn" style="width:120px;" readonly="readonly" value="<%=Model.uploadStatusCn%>" />
                                </td>
                            </tr>
                        </table>
                    </form>
                    <%-- 选择仓库 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelect.ascx"); %>
                    <%-- 选择退货原因 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/TransactionReasonSelect.ascx"); %>
                    <%-- 选择供应商 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelect.ascx"); %>
                </div>
            </div>
        </div>
    </div>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dgWmsBackInvoiceDetail" title="退货单明细" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          checkOnSelect: false,
                          selectOnCheck: false,
                          idField: 'id',
                          sortName: 'id',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tbDetail',
                          loadMsg: '更新数据......',
                          onClickRow: clickWmsBackInvoiceDetailRow,
                          onLoadSuccess: function(){
                              $(this).datagrid('clearChecked');  
                              editIndex = undefined;                     
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="field:'id',hidden:true"></th>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'materialDescription',width:280,align:'left'">物料描述</th>
                    <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                    <th data-options="field:'deliveryQuantity',width:70,align:'center'">送货数量</th>
                    <th data-options="field:'returnQuantityTotal',width:70,align:'center'">已退数量</th>
                    <th data-options="field:'returnQuantity',width:70,align:'center',editor:{ type: 'numberbox', options: { min: 0, validType: 'checkQuantity' } }">退货数量</th>
                    <th data-options="field:'warehouseId',width:100,align:'center',
                        editor: {
                            type: 'combobox',
                            options: {
                                valueField: 'id',
                                textField: 'id',
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseComboboxList',
                                multiple: false,
                                required: true,
                                mode: 'local',
                                validType: 'checkInput',
                                filter: function (q, row) {
                                    var opts = $(this).combobox('options');
                                    return row[opts.textField].indexOf(q) == 0;
                                },
                                onChange: function (newValue, oldValue) {
                                    warehouseIsChanged = true;
                                }
                            }
                        }">仓库</th>
                    <th data-options="field:'warelocationId',width:100,align:'center',
                        formatter:function(value,row){
                            return row.warelocationDoc;
                        },
                        editor:{
                            type:'combobox',
                            options:
                            {
                                valueField: 'id',
                                textField: 'docNumber',
                                multiple: false,
                                editable: false,
                                mode: 'local'
                            }
                        }">货位</th>
                    <th data-options="field:'docNumber',width:100,align:'center'">批条码号</th>
                </tr>
            </thead>
        </table>
        <div id="tbDetail">
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" id="detailAdd" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DetailAdd();">添加批单</a>
            <%} %>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" id="detailAddManAcc" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DetailAddManAcc();">添加手工单</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" id="detailRemove" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DetailRemove();">移除</a>
            <%} %>
        </div>
        <%-- 选择批送货单 --%>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/DeliveryOrderBatchSelect.ascx"); %>
        <%-- 选择手工单 --%>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WmsIncManAccSelect.ascx"); %>
        <!--进度条-->           
        <div id="wProgressbar" class="easyui-window" title="完成退货......" style="padding: 10px;width:440px;height:80px;"
            data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
            <div id="p" class="easyui-progressbar" style="width:400px;"></div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 供应商退货主表 --%>
    <script type="text/javascript">
        $(function(){
            var preEnabled=$('#lm_deliveried').attr("checked", "true");
            $("[name='accountStatus']").each(function () {
                $(this).click(function () {
                    if ($(this).is(':checked')) {
                        if (preEnabled != null) {
                            preEnabled.removeAttr("checked"); 
                        }
                        preEnabled = $(this);
                        preEnabled.attr("checked", "true");
                    }
                    else
                    {
                        if (preEnabled.val() == $(this).val()) {
                                preEnabled.attr("checked", "true");
                            }
                    }
                })
            })
        })
        function WarehouseSelected(r) {
            if (r) {
                $('#warehouseId').val(r.id);
            }
        }
        function SupplierSelected(r)
        {
            if(r)
            {
                var detailRows = $('#dgWmsBackInvoiceDetail').datagrid('getRows');
                if(detailRows.length>0)
                {
                    $.messager.confirm('确认', '更改供应商会清空退货单明细，是否确认更改？', function (result) {
                         if (result) {
                            $('#dgWmsBackInvoiceDetail').datagrid('loadData',[]);
                            $('#supplierDocNumber').val(r.docNumber);
                            $('#supplierId').val(r.id);
                            $('#supplierName').val(r.name);
                        }
                    });
                }
                else
                {
                    $('#supplierDocNumber').val(r.docNumber);
                    $('#supplierId').val(r.id);
                    $('#supplierName').val(r.name);
                }
            }
        }
        function ReasonSelected(r) {
            if (r) {
                $('#reasonId').val(r.reasonId);
                $('#reasonName').val(r.reasonName);
            }
        }
        function SetButtons() {
            var status = $('#status').val();
            $('#mainSave').hide();
            $('#detailAdd').hide();
            $('#detailAddManAcc').hide();
            $('#detailRemove').hide();
            $('#selectWarehouse').hide();
            $('#selectReason').hide();
            $('#selectSupplier').hide();
            <% if (ynWebRight.rightEdit){ %>
            if (status=="") {
                $('#mainSave').show();
                $('#detailAdd').show();
                $('#detailAddManAcc').show();
                $('#detailRemove').show();
                $('#selectWarehouse').show();
                $('#selectReason').show();
                $('#selectSupplier').show();
            }
            <%} %>
        }
        function Save() {
            if (endEditing()){
                var detailRows = $('#dgWmsBackInvoiceDetail').datagrid('getRows');
                if (detailRows.length == 0) {
                    $.messager.alert('错误', '没有添加供应商退货明细!', 'error');
                    return;
                }
                $.messager.confirm("确认", "确认提交保存？", function (r) {
                    if (r) {
                        //接受退货明细中退货数量的更改
                        //acceptWmsBackInvoiceDetail();
                        //验证退货明细数量
                        var message = "";
                        $.each(detailRows, function (index, item) {
                            if(item.warelocationId==null||item.warelocationId==""||item.warelocationId=="0")
                            {
                                message += "<li>退料单明细有未选择货位编码项，请重新填写后保存！</li>";
                                $('#dgWmsBackInvoiceDetail').datagrid('selectRecord', item.id);
                                return;
                            }
                            if (item.returnQuantity > item.deliveryQuantity) {
                                message += "<li>物料<font color='red'>" + item.materialDocNumber + "</font>退货数量[" + item.returnQuantity + "]大于送货数量[" + item.deliveryQuantity + "]</li>";
                                $('#dgWmsBackInvoiceDetail').datagrid('selectRecord', item.id);
                                return;
                            }
                        })
                        if (message != "") {
                            $.messager.alert('错误', "<ul>" + message + "</ul>", 'error');
                            return;
                        }
                        //提交后台保存
                        var data = {
                            "detailJson": $.toJSON(detailRows)
                        };
                        var options = {
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsBackInvoiceSave/',
                            type: 'POST',
                            dataType: 'json',
                            data: data,
                            beforeSubmit: function () {
                                var flag = $('#editWmsBackInvoiceMainForm').form('validate');
                                if (flag) {
                                    $('#wProgressbar').window('open');
                                    $('#p').progressbar('setValue', 0);
                                    setInterval(updateProgress, 200);
                                }
                                return flag;
                            },
                            success: function (r) {
                                if (r.result) {
                                    $('#p').progressbar('setValue', 100);
                                    $('#wProgressbar').window('close');

                                    $("#editWmsBackInvoiceMainForm input[name='docNumber']").val(r.entity.docNumber);
                                    $('#status').val(r.entity.status);
                                    $('#statusCn').val(r.entity.statusCn);
                                    $('#responsiblePerson').val(r.entity.responsiblePerson);
                                    SetButtons();
                                    $.messager.alert('提示', '提交完成!', 'info');

									//上传状态
									setMainUploadStatus(r.entity.returnCode);
                                } else {
                                    $('#p').progressbar('setValue', 100);
                                    $('#wProgressbar').window('close');

                                    $.messager.alert('错误', '提交失败:' + r.message, 'error');
                                }
                            }
                        };
                        $('#editWmsBackInvoiceMainForm').ajaxForm(options);
                        $('#editWmsBackInvoiceMainForm').submit();
                    }
                });
            }
        }
        function updateProgress() {
            var value = $('#p').progressbar('getValue');
            if (value < 99) {
                $('#p').progressbar('setValue', value + 1);
            }
        }
    </script>
    <%-- 供应商退货明细 --%>
    <script type="text/javascript">
        function DetailAdd() {
            SelectDeliveryOrderBatch($('#supplierId').val());
        }
        function DetailAddManAcc() {
            SelectWmsIncManAcc($('#supplierId').val());
        }
        var maxId = 0;
        function DeliveryOrderBatchSelected(selectRows) {
            if (selectRows) {
                $.messager.confirm('确认', '确认添加选择的送货批单到供应商退货单？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsBackInvoiceDetailAdd/',
                            type: "post",
                            dataType: "json",
                            data: { "deliveryOrderBatchJson": $.toJSON(selectRows) },
                            success: function (r) {
                                if (r.result) {
                                    var rows = $('#dgWmsBackInvoiceDetail').datagrid('getRows');
                                    $.each(r.entity, function (i, item) {
                                        if (item.warelocationId == 0) {
                                            item.warelocationId = "";
                                        }
                                        var isAdd = true;
                                        if (item.id == 0) {
                                            maxId = maxId - 1;
                                            item.id = maxId;
                                        }
                                        $.each(rows, function (_i, _item) {
                                            if (_item.docNumber == item.docNumber) {
                                                isAdd = false;
                                                return;
                                            }
                                        });
                                        if (isAdd)
                                            $('#dgWmsBackInvoiceDetail').datagrid('appendRow', item);
                                    });
                                } else {
                                    $.messager.alert('确认', '添加失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function WmsIncManAccSelected(selectRows) {
            if (selectRows) {
                $.messager.confirm('确认', '确认添加选择的手工单到供应商退货单？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsBackInvoiceDetailAddWmsInc/',
                            type: "post",
                            dataType: "json",
                            data: { "wmsIncManAccJson": $.toJSON(selectRows) },
                            success: function (r) {
                                if (r.result) {
                                    var rows = $('#dgWmsBackInvoiceDetail').datagrid('getRows');
                                    $.each(r.entity, function (i, item) {
                                        if (item.warelocationId == 0) {
                                            item.warelocationId = "";
                                        }
                                        var isAdd = true;
                                        if (item.id == 0) {
                                            maxId = maxId - 1;
                                            item.id = maxId;
                                        }
                                        $.each(rows, function (_i, _item) {
                                            if (_item.docNumber == item.docNumber && _item.materialDocNumber == item.materialDocNumber) {
                                                isAdd = false;
                                                return;
                                            }
                                        });
                                        if (isAdd)
                                            $('#dgWmsBackInvoiceDetail').datagrid('appendRow', item);
                                    });
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
            var checkRows = $('#dgWmsBackInvoiceDetail').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要移除的退货物料！", "info");
                return;
            }
            $.messager.confirm('确认', '确认从供应商退货单中移除选择的物料？', function (result) {
                if (result) {
                    var vArry = new Array();
                    var rows = $('#dgWmsBackInvoiceDetail').datagrid('getRows');
                    if (result) {
                        var num = 0;
                        $.each(rows, function (index, rowsItem) {
                            var isAdd = true;
                            $.each(checkRows, function (i, item) {
                                if (item.id == rowsItem.id) {
                                    isAdd = false;
                                    return;
                                }
                            });
                            if (isAdd) {
                                vArry[num] = rowsItem;
                                num = num + 1;
                            }

                        });
                    }
                    $('#dgWmsBackInvoiceDetail').datagrid('loadData', vArry);
                }
            })
        }
    </script>
    <%-- 编辑供应商退货数量 --%>
    <script type="text/javascript">
        $.extend($.fn.numberbox.defaults.rules, {
            checkQuantity: {
                validator: function (value, param) {
                    if (editIndex == undefined) { return true; }
                    var deliveryQuantity = $('#dgWmsBackInvoiceDetail').datagrid('getRows')[editIndex].deliveryQuantity;
                    return value <= deliveryQuantity  && value > 0;
                },
                message: '退货数量应大于0且不能大于送货数量.'
            }
        });
        var warehouseIsChanged = false;
        $.extend($.fn.combobox.defaults.rules, {
            checkInput: {
                validator: function (value, param) {
                    var selectRow = $('#dgWmsBackInvoiceDetail').datagrid('getSelected');
                    var rowIndex = $('#dgWmsBackInvoiceDetail').datagrid('getRowIndex', selectRow);
                    if (rowIndex != editIndex) { return true; }
                    var edWarehouse = $('#dgWmsBackInvoiceDetail').datagrid('getEditor', { index: editIndex, field: 'warehouseId' });
                    var flag = false;
                    if (edWarehouse) {
                        var data = $(edWarehouse.target).combobox('getData');
                        $.each(data, function (i, item) {
                            if (item.id == value) {
                                flag = true;
                                return;
                            }
                        });
                        if (flag && warehouseIsChanged) {
                            warehouseIsChanged = false;

                            var edWarelocation = $('#dgWmsBackInvoiceDetail').datagrid('getEditor', { index: editIndex, field: 'warelocationId' });
                            var materialDocNumber = $('#dgWmsBackInvoiceDetail').datagrid('getRows')[editIndex]['materialDocNumber'];
                            $(edWarelocation.target).combobox({
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WareLocationCbList',
                                onBeforeLoad: function (param) {
                                    param.warehouseId = value;
                                    param.materialDocNumber = materialDocNumber;
                                },
                                onLoadSuccess: function () {
                                    if (selectRow.warehouseId == value && selectRow.warelocationId) {
                                        $(edWarelocation.target).combobox('setValue', selectRow.warelocationId);
                                    } else {
                                        var _data = $(this).combobox('getData');
                                        if (_data.length > 0)
                                            $(edWarelocation.target).combobox('setValue', _data[0].id);
                                    }
                                }
                            }).combobox('clear');
                        }
                    }
                    return flag;
                },
                message: '子库输入错误.'
            }
        });

        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgWmsBackInvoiceDetail').datagrid('validateRow', editIndex)) {
                //子库
                var edWarehouse = $('#dgWmsBackInvoiceDetail').datagrid('getEditor', { index: editIndex, field: 'warehouseId' });
                var warehouseId = $(edWarehouse.target).combobox('getText');
                $('#dgWmsBackInvoiceDetail').datagrid('getRows')[editIndex]['warehouseId'] = warehouseId;
                //货位
                var edWarelocation = $('#dgWmsBackInvoiceDetail').datagrid('getEditor', { index: editIndex, field: 'warelocationId' });
                var docNumber = $(edWarelocation.target).combobox('getText');
                $('#dgWmsBackInvoiceDetail').datagrid('getRows')[editIndex]['warelocationDoc'] = docNumber;

                $('#dgWmsBackInvoiceDetail').datagrid('endEdit', editIndex);
                editIndex = undefined;
                return true;
            } else {
                return false;
            }
        }
        function clickWmsBackInvoiceDetailRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgWmsBackInvoiceDetail').datagrid('selectRow', index)
							.datagrid('beginEdit', index);

                    var ed = $('#dgWmsBackInvoiceDetail').datagrid('getEditor', { index: index, field: 'warelocationId' });
                    $(ed.target).css({ "width":"92px" });
                    /*    
                    var ed = $('#dgWmsBackInvoiceDetail').datagrid('getEditor', { index: index, field: 'warelocationId' });
                    var queryWarehouseId = $('#dgWmsBackInvoiceDetail').datagrid('getRows')[index]['warehouseId'];
                    var queryMaterialDocNumber = $('#dgWmsBackInvoiceDetail').datagrid('getRows')[index]['materialDocNumber'];
                    var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WareLocationCbList/?warehouseId=' + encodeURIComponent(queryWarehouseId) + '&materialDocNumber=' + encodeURIComponent(queryMaterialDocNumber);
                    $(ed.target).combobox('reload', url);
                    */
                    editIndex = index;
                } else {
                    $('#dgWmsBackInvoiceDetail').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptWmsBackInvoiceDetail() {
            if (endEditing()) {
                $('#dgWmsBackInvoiceDetail').datagrid('acceptChanges');
            }
        }
    </script>
</asp:Content>
