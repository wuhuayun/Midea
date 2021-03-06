﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsMtlReturnMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ERP退料单退料登记
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
							        <span>ERP退料单：</span>
						        </td>
						        <td style="width:180px">
                                    <input id="releaseNumber" name="releaseNumber" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.releaseNumber%>"/>
                                    <input id="releaseHeaderId" name="releaseHeaderId" type="hidden" value="<%=Model.releaseHeaderId %>"/>
							        <a href="javascript:void(0);" id="selectWipRelease" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择退料单" onclick="SelectWipRelease('RETURN');"></a> 
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>作业状态：</span>
						        </td>
						        <td>
							        <input id="releaseStatus" name="releaseStatus" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.releaseStatus%>"/>
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
                    <%-- 选择ERP退料单 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipReleaseSelect.ascx"); %>
                    <%-- 选择仓库 --%>
                    <%--<%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelect.ascx"); %>--%>
                    <%-- 选择退货原因 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/TransactionReasonSelect.ascx"); %>    
                </div>
            </div>
        </div>
    </div>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dgWmsMtlReturnDetail" title="ERP退料单退料明细" class="easyui-datagrid" style="" border="false"
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
                              editIndex = undefined;
                              $(this).datagrid('clearSelections');
                              $(this).datagrid('clearChecked');
                              if (currentDetailId) {
                                  $(this).datagrid('selectRecord', currentDetailId);
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
                    <th data-options="field:'warehouseId',width:120,align:'center'">仓库子库</th>
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
                            <%--required:true,--%>
                            mode:'remote'
                        }
                    }">货位编码</th>
                    <th data-options="field:'printQuantity',width:80,align:'center'">计划数</th>
                    <th data-options="field:'quantity',width:80,align:'center',editor:'numberbox'">退料数量</th>
                </tr>
            </thead>
        </table>
        <div id="tbDetail">
            <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DetailReload();">刷新</a>--%>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" id="detailAdd" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DetailAdd();">添加</a>
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
							        <span>仓库子库：</span>
						        </td>
						        <td style="width:80%">
                                    <input type="text" id="warehouseId" style="width:140px;background-color:#CCCCCC;" readonly="readonly"/>
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
        <!--进度条-->           
        <div id="wProgressbar" class="easyui-window" title="完成退料......" style="padding: 10px;width:440px;height:80px;"
            data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
            <div id="p" class="easyui-progressbar" style="width:400px;"></div>
        </div>
        <%-- 选择ERP退料单明细 --%>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipReleaseLineMultipleSelect.ascx"); %>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- ERP退料单退料主表 --%>
    <script type="text/javascript">
        $(function(){
            $('#reasonName').val('k1无需求退货');
            $('#reasonId').val('1');
        })
        function WipDiscreteJobSelected(r) {
            if (r) {
                $('#wipEntityName').val(r.ascmWipEntities_Name);
                $('#wipEntityId').val(r.wipEntityId);
            }
        }
        function WipReleaseSelected(r) {
            if (r) {
                var detialRows= $('#dgWmsMtlReturnDetail').datagrid('getRows');
                if(detialRows.length!=0)
                {
                    $.messager.confirm('确认', '更改ERP退料单会清空退料明细，是否确认更改？', function (result) {
                         if (result) {
                            $('#dgWmsMtlReturnDetail').datagrid('loadData',[]);
                            $('#releaseNumber').val(r.releaseNumber);
                            $('#releaseHeaderId').val(r.releaseHeaderId);
                        }
                    });
                }
                else
                {
                    $('#releaseNumber').val(r.releaseNumber);
                    $('#releaseHeaderId').val(r.releaseHeaderId);
                }
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
            $('#selectWipRelease').hide();
            $('#selectReason').hide();
            $('#detailAdd').hide();
            $('#detailEdit').hide();
            $('#detailRemove').hide();
            <% if (ynWebRight.rightEdit){ %>
            if (status=="") {
                $('#mainSave').show();
                $('#selectWipRelease').show();
                $('#selectReason').show();
                $('#detailAdd').show();
                $('#detailAdd').show();
                $('#detailEdit').show();
                $('#detailRemove').show();
            }
            <%} %>
        }
        function Save() {
            var detailRows = $('#dgWmsMtlReturnDetail').datagrid('getRows');
            if (detailRows.length == 0) {
                $.messager.alert('错误', '没有添加退料明细!', 'error');
                return;
            }
            acceptProcedureArgument();
            var message = "";
            $.each(detailRows, function (index, item) {
                if(item.warelocationId==null||item.warelocationId==""||item.warelocationId=="0")
                {
                    message += "<li>物料<font color='red'>【" + item.materialDocNumber + "】</font>未指定货位</li>";
                }
                if (item.quantity ==0) {
                    message += "<li>物料<font color='red'>【" + item.materialDocNumber + "】</font>退料数量为0</li>";
                }
            })
            if (message != "") {
                $.messager.alert('错误', "<ul>" + message + "</ul>", 'error');
                return;
            }
            $.messager.confirm("确认", "确认提交保存？", function (r) {
                if (r) {
                    var data = {
                        "detailJson": $.toJSON(detailRows)
                    };
                    var options = {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipReleaseLineToMtlReturnSave/',
                        type: 'POST',
                        dataType: 'json',
                        data: data,
                        beforeSubmit: function () {
                            var flag = $('#editWmsMtlReturnMainForm').form('validate');
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

                                $("#editWmsMtlReturnMainForm input[name='docNumber']").val(r.entity.docNumber);
                                $.messager.alert('提示', '提交完成!', 'info');
                                SetButtons();

								//上传状态
								setMainUploadStatus(r.entity.returnCode);
                            } else {
                                $('#p').progressbar('setValue', 100);
                                $('#wProgressbar').window('close');

                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    };
                    $('#editWmsMtlReturnMainForm').ajaxForm(options);
                    $('#editWmsMtlReturnMainForm').submit();
                }
            });
        }
        function updateProgress() {
            var value = $('#p').progressbar('getValue');
            if (value < 99) {
                $('#p').progressbar('setValue', value + 1);
            }
        }
    </script>
    <%-- ERP退料单退料明细表 --%>
    <script type="text/javascript">
        var currentDetailId = "";
        function WarelocationSelected(r) {
            if (r) {
                $('#warelocationDocNumber').val(r.docNumber);
                $('#warelocationId').val(r.id);
            }
        }
        function DetailAdd() {
            var _releaseHeaderId = $('#releaseHeaderId').val();
            if (_releaseHeaderId == null || _releaseHeaderId == "" || _releaseHeaderId == 0) {
                $.messager.alert('提示', '请先选择领料单', 'info');
                return;
            }
            var rows = $('#dgWmsMtlReturnDetail').datagrid('getRows');
            var notInMaterialIds = "";
            $.each(rows, function (i, item) {
                if (notInMaterialIds != "")
                    notInMaterialIds += ",";
                notInMaterialIds += item.materialId;
            });
            SelectWipReleaseLine(_releaseHeaderId, notInMaterialIds);
        }
        var maxId = 0;
        function WipReleaseLineSelected(checkRows) {
            if (checkRows) {
                $.messager.confirm('确认', '确认添加勾选的作业明细？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipReleaseLineToMtlReturnAdd/',
                            type: "post",
                            dataType: "json",
                            data: { "wipReleaseLineRows": $.toJSON(checkRows) },
                            success: function (r) {
                                if (r.result) {
                                    $.each(r.rows, function (i, item) {
                                        if (item.warelocationId == 0)
                                            item.warelocationId = "";
                                        if (item.id == 0) {
                                            maxId = maxId - 1;
                                            item.id = maxId;
                                        }
                                        $('#dgWmsMtlReturnDetail').datagrid('appendRow', item);
//                                        $('#dgWmsMtlReturnDetail').datagrid('appendRow', {
//                                            id: item.id,
//                                            materialDocNumber: item.materialDocNumber,
//                                            materialId: item.materialId,
//                                            materialDescription: item.materialDescription,
//                                            materialUnit: item.materialUnit,
//                                            warehouseId: item.warehouseId,
//                                            warelocationId: item.warelocationId,
//                                            warelocationDoc:item.locationDocNumber,
//                                            printQuantity:item.printQuantity,
//                                            quantity: 0
//                                        });
                                    });
                                    //                                    $('#dgWmsMtlReturnDetail').datagrid('appendRow',r.rows);
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
                $('#warehouseId').val(selectRow.warehouseId);
                $('#warelocationDocNumber').val(selectRow.warelocationDocNumber);
                $('#warelocationId').val(selectRow.warelocationId);
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
                    var queryWarehouseId = $('#dgWmsMtlReturnDetail').datagrid('getRows')[index]['warehouseId'];
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
