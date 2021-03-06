﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业退料管理
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
                         <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="exportToPdf();">退料单打印</a>
                        <iframe id="iframeExportToPdf" name="iframeExportToPdf" scrolling="auto" frameborder="0"  src="" style="width:0px;height:0px;display:none;"></iframe>
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
                                    <input type="text" name="docNumber" style="width:140px;background-color:#F0F0F0;" readonly="readonly"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>作业号：</span>
						        </td>
						        <td style="width:180px">
                                    <input id="wipEntitiesName" name="wipEntitiesName" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=ViewData["WipEntityName"] %>"/>
						            <input id="wipEntityId" name="wipEntityId" type="hidden"/>
                                </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>仓库：</span>
						        </td>
						        <td>
							        <input id="warehouseId" name="warehouseId" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=ViewData["WarehouseId"] %>"/>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>退货区域：</span>
						        </td>
						        <td>
                                    <%=Html.DropDownList("returnArea", (IEnumerable<SelectListItem>)ViewData["listReturnArea"], new { style = "width:146px;" })%>
						        </td>
					        </tr>
                            <%--<tr style="height:24px">
						        <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>物料编码：</span>
						        </td>
						        <td style="width:180px">
                                    <input type="text" id="queryStartMaterialDocNumber" name="queryStartMaterialDocNumber" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=ViewData["StartMaterialDocNumber"] %>"/>
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span></span>
						        </td>
						        <td>
                                    <input id="queryEndMaterialDocNumber" name="queryEndMaterialDocNumber" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=ViewData["EndMaterialDocNumber"] %>"/>
						        </td>
					        </tr>--%>
                            <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>退料原因：</span>
						        </td>
                                <td>
                                    <input id="reasonName" name="reasonName" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly"/>
                                    <input id="reasonId" name="reasonId" type="hidden"/>
							        <a href="javascript:void(0);" id="selectReason" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择原因" onclick="SelectReason();"></a> 
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>备注：</span>
						        </td>
						        <td>
							        <input type="text" id="memo" name="memo" style="width:140px;"/>
                                    <input id="queryWipSupplyType" name="queryWipSupplyType" type="hidden" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=ViewData["WipSupplyType"] %>"/>
						        </td>
					        </tr>
				        </table>
                    </form>
                    <%-- 选择退货原因 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/TransactionReasonSelect.ascx"); %>    
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
                          toolbar: '#tb_Detail',
                          loadMsg: '更新数据......',
                          onSelect: function(rowIndex, rowRec){
                              currentDetailId = rowRec.sn;
                          },
                          onClickRow:function(rowIndex,rowRec){
                              <%--<%if (ynWebRight.rightEdit){ %>--%>
                              clickWmsStockTransDetailRow(rowIndex);
                              <%--<%} %>--%>
                          },
                          onDblClickRow: function(rowIndex, rowRec){
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
                            editable:false,
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
        <div id="tb_Detail">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="reloadDetail();">刷新</a>
            <a href="javascript:void(0);" id="btnDetailSetLocation" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="setDetailLocation();">设置货位</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" id="detaliAdd" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DetailAdd();">添加</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" id="detailRemove" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DetailRemove();">移除</a>
            <%} %>
        </div>
        <div id="materialItemSelect" class="easyui-window" title="选择物料" style="padding: 5px;width:640px;height:480px;"
            data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
                    <table id="dgSelectMaterialItem" title="物料信息" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                                      noheader: true,
                                      rownumbers: true,
                                      idField: 'id',
                                      sortName: '',
                                      sortOrder: '',
                                      striped: true,
                                      toolbar: '#tbMaterialItemSelect',
                                      pagination: true,
                                      pageSize: 30,
                                      loadMsg: '更新数据......',
                                      onSelect: function(rowIndex, rowRec){
                                
                                      },
                                      onDblClickRow: function(rowIndex, rowRec){
                                      },
                                      onLoadSuccess: function(){
                                          $(this).datagrid('clearSelections');
                                          $(this).datagrid('clearChecked');                      
                                      }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'materialDocNumber',width:100,align:'center'">编码</th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'materialDescription',width:320,align:'left'">物料描述</th>
                                <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                                <th data-options="field:'warelocationId',width:100,align:'center',hidden:true">货位编码</th>
                                <th data-options="field:'quantity',width:80,align:'center'">退料数量</th>
                                <th data-options="field:'warehouseIdName',width:120,align:'center'">供应子库</th>
                                <th data-options="field:'requiredQuantity',width:80,align:'center'">需求数量</th>
                                <th data-options="field:'quantityIssued',width:80,align:'center'">发料数量</th>
                                <th data-options="field:'quantityDifference',width:80,align:'center'">差异数量</th>
                            </tr>
                        </thead>
		            </table>
                    <div id="tbMaterialItemSelect">
                        <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="MaterialItemSelectOk()">确认</a>
                        <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#materialItemSelect').window('close');">取消</a> 
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 作业退料主表 --%>
    <script type="text/javascript">
        $(function(){
            $('#reasonName').val("k9改错");
            $('#reasonId').val("10");
            singleWipEntitiesQuery();
        });
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
        var allRows = null;
        function singleWipEntitiesQuery() {
            var queryWipEntityName =$('#wipEntitiesName').val();
            queryWarehouseId = $('#warehouseId').val();
            var queryWipSupplyType = $('#queryWipSupplyType').val();
//            var queryStartMaterialDocNumber = $('#queryStartMaterialDocNumber').val();
//            var queryEndMaterialDocNumber = $('#queryEndMaterialDocNumber').val();
            if (queryWipEntityName == null || queryWipEntityName == "" || queryWipEntityName == "0") {
                $.messager.alert('提示', '请选择作业号！', 'info');
                return;
            }
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsToMtlReturnAdd/',
                type: "post",
                dataType: "json",
                data: { "wipEntityName": queryWipEntityName, "queryWarehouseId": queryWarehouseId, "queryWipSupplyType": queryWipSupplyType },
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
                        allRows = $('#dgWmsMtlReturnDetail').datagrid('getRows');
                        editIndex = undefined;
                        if (r.rows.length > 0) {
                            $('#wipEntityId').val(r.rows[0].wipEntityId);
                        }
                    } else {
                        $.messager.alert('确认', '添加失败:' + r.message, 'error'); 
                    }
                }
            });
        }
    </script>
    <%-- 作业退料明细表 --%>
    <script type="text/javascript">
        function reloadDetail() {
            singleWipEntitiesQuery();
        }
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
        //设置货位
        function setDetailLocation() {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationDetailSetLocation/<%=Request["MainId"].ToString()%>',
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r.result) {
                        $.messager.alert('提示', '货位设置成功！', 'info', function () { reloadDetail(); });
                    } else {
                        $.messager.alert('错误', '货位设置失败！', 'error');
                    }
                }
            });
        }
        var currentDetailId = null;
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
        function DetailAdd() {
            var detailRows = $('#dgWmsMtlReturnDetail').datagrid('getRows');
            $('#materialItemSelect').window('open');
            var vArry = new Array();
            var num = 0;
            $.each(allRows, function (index, item) {
                var isAdd = true;
                $.each(detailRows, function (i, rowsItem) {
                    if (item.id == rowsItem.id) {
                        isAdd = false;
                        return;
                    }
                });
                if (isAdd) {
                    vArry[num] = item;
                    num = num + 1;
                }
            });
            $('#dgSelectMaterialItem').datagrid('loadData', vArry);
        }
        function MaterialItemSelectOk() {
            var checkRows = $('#dgSelectMaterialItem').datagrid('getChecked');
            $.each(checkRows, function (index, row) {
                $('#dgWmsMtlReturnDetail').datagrid('appendRow', row);
            })
            $('#materialItemSelect').window('close');
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
     <%-- 导出 --%>
    <script type="text/javascript">
        function exportToPdf() {
            $.messager.confirm('确认', '确认导出作业退料单' + '' + '？', function (result) {
                if (result) {
                    var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationExportToPdf?mainIdList=<%=Request["MainId"].ToString()%>&bSingle=true&pattern=wipReturn';
                    $('#iframeExportToPdf').attr("src", url).trigger("beforeload");
                }
            });
        }
    </script>
</asp:Content>