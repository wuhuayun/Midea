<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsMtlReturnMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业退料登记
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:140px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" id="mainSave" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="Save();">退料确认</a>
                        <%} %>
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
                    <form id="editWmsMtlReturnMainForm" method="post" style="">
                        <table style="width:690px;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>单据号：</span>
						        </td>
						        <td style="width:180px">
                                    <input type="text" name="docNumber" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.docNumber%>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>作业号：</span>
						        </td>
						        <td style="width:180px">
                                    <input id="wipEntitiesName" name="wipEntitiesName" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.wipEntityName%>"/>
                                    <input id="wipEntityId" name="wipEntityId" type="hidden" value="<%=Model.wipEntityId %>"/>
							        <a href="javascript:void(0);" id="selectWipEntity" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择作业" onclick="$('#wSingleWipEntities').window('open');"></a> 
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>仓库：</span>
						        </td>
						        <td>
							        <input id="warehouseId" name="warehouseId" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.warehouseId%>"/>
							        <%--<a href="javascript:void(0);" id="selectWarehouse" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择仓库" onclick="SelectWarehouse();"></a>--%>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>退货区域：</span>
						        </td>
						        <td>
                                    <%=Html.DropDownList("returnArea", (IEnumerable<SelectListItem>)ViewData["listReturnArea"], new { style = "width:146px;" })%>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>退料原因：</span>
						        </td>
                                <td>
                                    <input id="reasonName" name="reasonName" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.reasonName%>"/>
                                    <input id="reasonId" name="reasonId" type="hidden" value="<%=Model.reasonId %>"/>
							        <a href="javascript:void(0);" id="selectReason" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择原因" onclick="SelectReason();"></a> 
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>备注：</span>
						        </td>
						        <td>
							        <input type="text" id="memo" name="memo" style="width:140px;" value="<%=Model.memo %>"/>
						        </td>
					        </tr>
							<tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>上传状态：</span>
						        </td>
						        <td>
							        <input type="text" id="uploadStatusCn" name="uploadStatusCn" style="width:120px;" readonly="readonly" value="<%=Model.uploadStatusCn%>" />
						        </td>
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>&nbsp;</span>
						        </td>
						        <td>
							         <span>&nbsp;</span>
						        </td>
					        </tr>
				        </table>
                    </form>
                    <%-- 选择作业 --%>
                    <%--<%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipDiscreteJobSelect.ascx"); %>--%>
                    <%-- 选择仓库 --%>
                    <%--<%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelect.ascx"); %>--%>
                    <%-- 选择退货原因 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/TransactionReasonSelect.ascx"); %>    
                </div>
                <%-- 选择作业号 --%>
                <div id="wSingleWipEntities" class="easyui-window" title="选择作业号" style="padding: 10px;width:540px;height:420px;"
			        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			        <div class="easyui-layout" fit="true">
				        <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
				            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					            <tr style="height:24px">
						            <td style="width: 20%; text-align:right;" nowrap>
							            <span>作业号：</span>
						            </td>
						            <td style="width:80%">
                                        <input id="queryWipEntityName" name="queryWipEntityName" type="text" style="width:140px;"/>
                                        <%--<input id="Hidden1" name="wipEntityId" type="hidden" value="<%=Model.wipEntityId %>"/>--%>
                                        <%--<%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "wipEntitiesName", width = "150px",panelWidth=550 });%>--%>
						            </td>
					            </tr>
					            <tr style="height:24px">
						            <td style="width: 20%; text-align:right;" nowrap>
							            <span>供应类型：</span>
						            </td>
						            <td style="width:80%">
							            <select id="queryWipSupplyType">
                                            <option value="pushType">推式</option>
                                            <option value="pullType">拉式</option>
                                        </select>
						            </td>
					            </tr>
                                <tr style="height:24px">
						            <td style="width: 20%; text-align:right;" nowrap>
							            <span>供应子库：</span>
						            </td>
						            <td style="width:80%">
                                        <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWarehouseId", width = "150px" });%>
                                    </td>
					            </tr>
                                <tr style="height:24px">
						            <td style="width: 20%; text-align:right;" nowrap>
							            <span>物料编码：</span>
						            </td>
						            <td style="width:80%">
                                        <input class="easyui-validatebox" id="queryStartMaterialDocNumber" type="text" style="width:145px;" />-<input class="easyui-validatebox" id="queryEndMaterialDocNumber" type="text" style="width:145px;" />
							        </td>
					            </tr>
				            </table>
				        </div>
				        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                            <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="singleWipEntitiesQuery()">查询</a>
                            <%} %>
					        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#wSingleWipEntities').window('close');">取消</a>
				        </div>
			        </div>
		        </div>
            </div>
        </div>
    </div>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dgWmsMtlReturnDetail" title="作业退料单明细" class="easyui-datagrid" style="" border="false"
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
                          onSelect: function(rowIndex, rowRec){
                              currentDetailId = rowRec.sn;
                          },
                          onClickRow:function(rowIndex,rowRec){
                              <%if (ynWebRight.rightEdit){ %>
                              clickWmsStockTransDetailRow(rowIndex);
                              <%} %>
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              <%--DetailEdit();--%>
                              <%} %>
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              $(this).datagrid('clearChecked');
                              if (currentDetailId) {
                                  $(this).datagrid('selectRecord', currentDetailId);
                              editIndex = undefined;
                              }                     
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
                    <th data-options="field:'materialDescription',width:320,align:'left'">物料描述</th>
                    <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                    <th data-options="field:'warelocationId',width:100,align:'center',
                    formatter:function(value,row){
                        return row.locationDocNumber;
                    },
                    editor:{
                        type:'combobox',
                        options:
                        {
                            valueField:'id',
                            textField:'docNumber',
                            multiple:false,
                            mode:'remote'
                        }
                    }">货位编码</th>
                    <th data-options="field:'quantity',width:80,align:'center',editor: { type: 'numberbox', options: { min: 0, validType: 'checkQuantity'}}">退料数量</th>
                    <th data-options="field:'warehouseIdName',width:120,align:'center'">供应子库</th>
                    <th data-options="field:'requiredQuantity',width:80,align:'center'">需求数量</th>
                    <th data-options="field:'quantityIssued',width:80,align:'center'">发料数量</th>
                    <th data-options="field:'quantityDifference',width:80,align:'center'">差异数量</th>
                </tr>
            </thead>
        </table>
        <div id="tbDetail">
            <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DetailReload();">刷新</a>--%>
            <% if (ynWebRight.rightAdd){ %>
			<%--<a href="javascript:void(0);" id="detailAdd" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DetailAdd();">添加</a>--%>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
            <%--<a href="javascript:void(0);" id="detailEdit" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="DetailEdit();">修改</a>--%>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" id="detailRemove" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DetailRemove();">移除</a>
            <%} %>
        </div>
        <div id="editDetail" class="easyui-window" title="退料明细修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editDetailForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>物料编码：</span>
						        </td>
						        <td style="width:80%">
                                    <input type="text" id="materialDocNumber" style="width:140px;background-color:#CCCCCC;" readonly="readonly"/>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>货位：</span>
						        </td>
						        <td style="width:80%">
							        <input type="text" id="locationDocNumber" name="locationDocNumber" style="width:140px;background-color:#CCCCCC;" readonly="readonly"/>
                                    <input type="hidden" id="warelocationId" name="warelocationId"/>
                                    <a href="javascript:void(0);" id="selectWarelocation" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择货位" onclick="SelectWarelocation();"></a>
                                </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>单位：</span>
						        </td>
						        <td style="width:80%">
							        <input type="text" id="materialUnit" style="width:140px;background-color:#CCCCCC;" readonly="readonly"/>
                                </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>数量：</span>
						        </td>
						        <td style="width:80%">
							        <input type="text" id="quantity" name="quantity" class="easyui-numberbox" data-options="min:0" style="width:140px;"/>
                                </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right; vertical-align:top;" nowrap>
							        <span>物料描述：</span>
						        </td>
						        <td style="width:80%">
							        <textarea id="materialDescription" name="materialDescription" rows="2" cols="1" readonly="readonly" style="width:300px;background-color:#CCCCCC;"></textarea>
						        </td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnDetailSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="DetailSave()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDetail').window('close');">取消</a>
				</div>
                <%-- 选择货位 --%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarelocationSelect.ascx"); %>
			</div>
		</div>
        <%-- 选择作业需求 --%>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipRequireOperationsMultipleSelect2.ascx"); %>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 作业退料主表 --%>
    <script type="text/javascript">
        $(function(){
            $('#reasonName').val("k9改错");
            $('#reasonId').val("10");
        });
        function WipDiscreteJobSelected(r) {
            if (r) {
                $('#wipEntityName').val(r.ascmWipEntities_Name);
                $('#wipEntityId').val(r.wipEntityId);
            }
        }
        function WarehouseSelected(r) {
            if (r) {
                $('#warehouseId').val(r.id);
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
            $('#selectWipEntity').hide();
            $('#selectReason').hide();
            $('#detailRemove').hide();
            <% if (ynWebRight.rightEdit){ %>
            if (status=="") {
                $('#mainSave').show();
                $('#selectWipEntity').show();
                $('#selectReason').show();
                $('#detailRemove').show();
            }
            <%} %>
        }
        function Save() 
        {
            var detailRows = $('#dgWmsMtlReturnDetail').datagrid('getRows');
            if (detailRows.length == 0) {
                $.messager.alert('错误', '没有添加作业退料明细!', 'error');
                return;
            }
            else {
                acceptProcedureArgument();
                for (var i = 0; i < detailRows.length; i++) {
                    var warelocationDocNumber = $('#dgWmsMtlReturnDetail').datagrid('getRows')[i]['warelocationId'];
                    if (warelocationDocNumber == null || warelocationDocNumber == "" || warelocationDocNumber == "0") {
                        $.messager.alert('错误', '退料单明细有未选择货位编码项，请重新填写后保存！', 'error');
                        return;
                    }
                }
            }
            $.messager.confirm("确认", "即将提交【" + detailRows.length + "】条退料单明细，是否确认提交保存？", function (r) {
                if (r) {
                    var data = {
                        "detailJson": $.toJSON(detailRows)
                    };
                    var options = {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsJobMtlReturnSave/',
                        type: 'POST',
                        dataType: 'json',
                        data: data,
                        beforeSubmit: function () {
                            return $('#editWmsMtlReturnMainForm').form('validate');
                        },
                        success: function (r) {
                            if (r.result) {
                                $("#editWmsMtlReturnMainForm input[name='docNumber']").val(r.entity.docNumber);
                                $.messager.alert('提示', '提交完成!', 'info');
                                SetButtons();

								//上传状态
								setMainUploadStatus(r.entity.returnCode);
                            } else {
                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    };
                    $('#editWmsMtlReturnMainForm').ajaxForm(options);
                    $('#editWmsMtlReturnMainForm').submit();
                }
            });
        }
        var queryWarehouseId = null;
        function singleWipEntitiesQuery() {
            //            var wipEntitiesId = $('#wipEntitiesName').combobox('getValue');
            var queryWipEntityName = $('#queryWipEntityName').val();
            queryWarehouseId = $('#queryWarehouseId').combobox('getText');
            var queryWipSupplyType = $('#queryWipSupplyType').find("option:selected").val();
            var queryStartMaterialDocNumber = $('#queryStartMaterialDocNumber').val();
            var queryEndMaterialDocNumber = $('#queryEndMaterialDocNumber').val();
            if (queryWipEntityName == null || queryWipEntityName == "" || queryWipEntityName == "0") {
                $.messager.alert('提示', '请选择作业号！', 'info');
                return;
            }
            $('#warehouseId').val(queryWarehouseId);
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsToMtlReturnAdd/',
                type: "post",
                dataType: "json",
                data: { "wipEntityName": queryWipEntityName, "queryWarehouseId": queryWarehouseId, "queryWipSupplyType": queryWipSupplyType, "queryStartMaterialDocNumber": queryStartMaterialDocNumber, "queryEndMaterialDocNumber": queryEndMaterialDocNumber },
                success: function (r) {
                    if (r.result) {
                        var maxId = 0;
                        $.each(r.rows, function (i, item) {
                            if(item.warelocationId==0)
                                item.warelocationId = "";
                            if (item.id == 0) {
                                maxId = maxId - 1;
                                item.id = maxId;
                            }
                        });
                        $('#dgWmsMtlReturnDetail').datagrid('loadData', r.rows);
                        editIndex = undefined;
                        $('#wipEntitiesName').val($('#queryWipEntityName').val());
                        if (r.rows.length > 0) {
                            $('#wipEntityId').val(r.rows[0].wipEntityId);
                        }
                        $('#wSingleWipEntities').window('close');
                    } else {
                        $.messager.alert('确认', '添加失败:' + r.message, 'error'); 
                    }
                }
            });
        }
    </script>
    <%-- 作业退料明细表 --%>
    <script type="text/javascript">
        $.extend($.fn.numberbox.defaults.rules, {
            checkQuantity: {
                validator: function (value, param) {
                    if (editIndex == undefined) { return true; }
                    var quantityIssued = $('#dgWmsMtlReturnDetail').datagrid('getRows')[editIndex].quantityIssued;
                    return value <= quantityIssued;
                },
                message: '退料数量必须小于发料数量！'
            }
        });
        var currentDetailId = "";
        function WarelocationSelected(r) {
            if (r) {
                $('#warelocationDocNumber').val(r.docNumber);
                $('#warelocationId').val(r.id);
            }
        }
        function DetailAdd() {
            var _wipEntityId = $('#wipEntityId').val();
            if (_wipEntityId == null || _wipEntityId == "" || _wipEntityId == 0) {
                $.messager.alert('提示', '请先选择作业', 'info');
                return;
            }
            var rows = $('#dgWmsMtlReturnDetail').datagrid('getRows');
            var notInMaterialIds = "";
            $.each(rows, function (i, item) {
                if (notInMaterialIds != "")
                    notInMaterialIds += ",";
                notInMaterialIds += item.materialId;
            });
            SelectWipRequireOperation(_wipEntityId, notInMaterialIds);
        }
        function WipRequireOperationSelected(checkRows) {
            if (checkRows) {
                $.messager.confirm('确认', '确认添加勾选的物料需求？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipRequireOperationToMtlReturnAdd/',
                            type: "post",
                            dataType: "json",
                            data: { "wipRequireOperationRows": $.toJSON(checkRows) },
                            success: function (r) {
                                if (r.result) {
                                    $('#dgWmsMtlReturnDetail').datagrid('loadData', r.rows);
                                } else {
                                    $.messager.alert('确认', '添加失败:' + r.message, 'error');
                                }
                            }
                        });     
                    } 
                });
            }
        }
        function DetailEdit() {
            var selectRow = $('#dgWmsMtlReturnDetail').datagrid('getSelected');
            if (selectRow) {
                $('#editDetail').window('open');
                $('#editDetailForm')[0].reset();
                $('#materialDocNumber').val(selectRow.materialDocNumber);
                $('#materialDescription').val(selectRow.materialDescription);
                $('#warelocationDocNumber').val(selectRow.warelocationDocNumber);
                $('#toWarelocationId').val(selectRow.toWarelocationId);
                $('#materialUnit').val(selectRow.materialUnit);
                $('#quantity').numberbox('setValue', selectRow.quantity);
            } else {
                $.messager.alert('提示', '请选择一行明细', 'info');
            }
        }
        function DetailSave() {
            var _warelocationId = $('#warelocationId').val();
            if (_warelocationId == null || _warelocationId == "" || _warelocationId == 0) {
                $.messager.alert('提示', '请选择货位', 'info');
                return;
            }
            var _quantity = $('#quantity').numberbox('getValue');
            if (isNaN(_quantity) || _quantity == 0) {
                $.messager.alert('提示', '必须输入数量，且数量不能为0', 'info');
                return;
            }
            var selectRow = $('#dgWmsMtlReturnDetail').datagrid('getSelected');
            selectRow.warelocationId = _warelocationId;
            selectRow.locationDocNumber = $('#locationDocNumber').val();
            selectRow.quantity = _quantity;
            var selectRowIndex = $('#dgWmsMtlReturnDetail').datagrid('getRowIndex', selectRow);
            $('#dgWmsMtlReturnDetail').datagrid('updateRow', {
                index: selectRowIndex,
                row: selectRow
            });
            $('#editDetail').window('close');
        }
        function DetailRemove() {
            var checkRows = $('#dgWmsMtlReturnDetail').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要移除的物料！", "info");
                return;
            }
            $.messager.confirm('确认', '确认移除勾选的物料？', function (result) {
                if (result) {
                    var vArry = new Array();
                    var rows = $('#dgWmsMtlReturnDetail').datagrid('getRows');
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
                    $('#dgWmsMtlReturnDetail').datagrid('loadData', vArry);
                    editIndex = undefined;
                }
            })
        }
    </script>
    <%-- 单击编辑 --%>
    <script type="text/javascript">
        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgWmsMtlReturnDetail').datagrid('validateRow', editIndex)) {
                var ed = $('#dgWmsMtlReturnDetail').datagrid('getEditor', { index: editIndex, field: 'warelocationId' });
                var docNumber = $(ed.target).combobox('getText');
                $('#dgWmsMtlReturnDetail').datagrid('getRows')[editIndex]['locationDocNumber'] = docNumber;
                $('#dgWmsMtlReturnDetail').datagrid('endEdit', editIndex);
                editIndex = undefined;

                return true;
            } else {
                return false;
            }
        }
        function clickWmsStockTransDetailRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgWmsMtlReturnDetail').datagrid('selectRow', index)
							.datagrid('beginEdit', index);

                    var ed = $('#dgWmsMtlReturnDetail').datagrid('getEditor', { index: index, field: 'warelocationId' });
                    var mtlDoc = $('#dgWmsMtlReturnDetail').datagrid('getRows')[index]['materialDocNumber'];
                    var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WareLocationCbList/?warehouseId=' + encodeURIComponent(queryWarehouseId) + "&materialDocNumber=" + mtlDoc;
                    $(ed.target).combobox('reload', url);

                    editIndex = index;

                } else {
                    $('#dgWmsMtlReturnDetail').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptProcedureArgument() {
            if (endEditing()) {
                $('#dgWmsMtlReturnDetail').datagrid('acceptChanges');
            }
        }
    </script>
</asp:Content>
