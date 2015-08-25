<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	需求备料
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:100px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" id="btnPreparationOk" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="PreparationOk();">备料确认</a>
                        <%} %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
	                </div> 
                </div>
                <div region="center" border="false" style="overflow:auto;">
                    <form id="editWmsPreparationMainForm" method="post" style="">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr style="height:24px">
                               <td style="width:70px; text-align:right;" nowrap="nowrap">
							        <span>计划时间：</span>
						        </td>
                                <td style="width:240px">
                                    <input type="text" name="requireScheduledDateSegment" style="width:230px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.requireScheduledDateSegment%>"/>
                                </td>
                                <td style="width:70px; text-align:right;" nowrap="nowrap">
							        <span>子库：</span>
						        </td>
                                <td style="width:160px">
                                    <input type="text" name="jobWarehouseSegment" style="width:150px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobWarehouseSegment%>"/>
                                </td>
                                <td style="width:60px; text-align:right;" nowrap="nowrap">
							        <span>单据号：</span>
						        </td>
                                <td style="width:120px">
                                    <input type="text" name="docNumber" style="width:110px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.docNumber%>"/>
						        </td>
                                <td style="width:70px; text-align:right;" nowrap="nowrap">
							        <span>单据状态：</span>
						        </td>
                                <td style="width:120px">
							        <input id="statusCn" name="statusCn" type="text" style="width:110px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.statusCn%>"/>
                                    <input id="status" name="status" type="hidden" value="<%=Model.status %>"/>
						        </td> 
                            </tr>
                            <tr style="height:24px">
                               <td style="text-align:right;" nowrap="nowrap">
							        <span>作业号：</span>
						        </td>
                                <td>
                                    <input type="text" name="requireWipEntitySegment" style="width:230px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.requireWipEntitySegment%>"/>
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>物料大类：</span>
						        </td>
                                <td>
                                    <input type="text" name="jobMtlCategorySegment" style="width:150px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobMtlCategorySegment%>"/>
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>打单员：</span>
						        </td>
						        <td>
                                    <input type="text" name="createUser" style="width:110px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.createUser%>"/> 
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>打单时间：</span>
						        </td>
                                <td>
                                    <input type="text" name="createTimeShow" style="width:110px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.createTimeShow%>"/> 
						        </td>   
                            </tr>
				        </table>
                    </form>
                </div>
            </div>
        </div>    
    </div>
    <div region="center" title="" border="false" style="padding:0px;">
        <div class="easyui-panel" fit="true" border="false" style="overflow:hidden;">
			<div class="easyui-layout" fit="true" style="">
                <div region="west" border="false" fit="false" style="width:560px;padding:0px 2px 0px 0px;overflow:hidden;">
                    <table id="dgWmsPreparationDetailSum" title="物料合计" class="easyui-datagrid" style="" border="true"
                          data-options="fit: true,
                                        singleSelect: true,
                                        checkOnSelect: false,
                                        selectOnCheck: false,
                                        noheader: true,
                                        rownumbers: true,
                                        idField: 'materialId',
                                        sortName: 'materialId',
                                        sortOrder: '',
                                        striped: true,
                                        toolbar: '#tbSumDetail',
                                        loadMsg: '数据加载中,请稍候...',
                                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipRequirePreparationDetailSumList/<%=Model.id%>',
                                        onSelect: function(rowIndex, rowRec){
                                            GetDetailRowsByMaterialId(rowRec.materialId);
                                        },
                                        onClickRow:function(rowIndex,rowRec){
                                            <% if (ynWebRight.rightEdit){ %>
                                            clickWmsPreparationSumDetailRow(rowIndex);
                                            <%} %>
                                        },
                                        onLoadSuccess: function(){
                                            editSumIndex = undefined;
                                            $(this).datagrid('clearSelections');
                                            $(this).datagrid('clearChecked');                         
                                        }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'materialId',hidden:true"></th>
                                <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th> 
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'materialName',width:160,align:'left'">物料名称</th>
                                <th data-options="field:'materialUnit',width:40,align:'center'">单位</th>
                                <th data-options="field:'prepareQuantity',width:60,align:'center',
                                    styler:function(value,row,index){
                                        return 'background-color:#98FB98';
                                    },
                                    editor:{ 
                                        type: 'numberbox', 
                                        options: { min: 0, validType: 'checkSumPrepareQuantity' } 
                                    }">备料数量</th>                            
                                <th data-options="field:'planQuantity',width:55,align:'center'">计划数量</th>
                                <th data-options="field:'containerBindNumber',width:55,align:'center'">已备数量</th>
                                <th data-options="field:'onhandQuantity',width:60,align:'center'">ERP库存</th>
                            </tr>
                        </thead>
		            </table>
                    <div id="tbSumDetail">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DetailSumReload();">刷新</a>
                        <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" id="detailSave" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DetailSumSave();">加入</a>
                        <%} %>
                    </div>
                </div>
                <div region="center" title="" border="true" style="padding:0px;">
                    <table id="dgWmsPreparationDetail" title="备料单明细" class="easyui-datagrid" style="" border="false"
                          data-options="fit: true,
                                        singleSelect: true,
                                        checkOnSelect: false,
                                        selectOnCheck: false,
                                        noheader: true,
                                        idField: 'id',
                                        sortName: 'id',
                                        sortOrder: '',
                                        striped: true,
                                        toolbar: '#tbDetail',
                                        loadMsg: '数据加载中,请稍候...',
                                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipRequirePreparationDetailList/<%=Model.id%>',
                                        onCheck: function(rowIndex, rowData){
                                            assignPrepareQuantityReload();
                                        },
                                        onUncheck: function(rowIndex, rowData){
                                            assignPrepareQuantityReload();
                                        },
                                        onSelect: function(rowIndex, rowRec){
                                            
                                        },
                                        onClickRow:function(rowIndex,rowRec){
                                            <% if (ynWebRight.rightEdit){ %>
                                            clickWmsPreparationDetailRow(rowIndex);
                                            <%} %>
                                        },
                                        onLoadSuccess: function(data){
                                            editIndex = undefined;
                                            $(this).datagrid('clearSelections'); 
                                            $(this).datagrid('clearChecked');
                                            var selectRow = $('#dgWmsPreparationDetailSum').datagrid('getSelected');
                                            if (!selectRow){
                                                detailDataRows = data.rows;   
                                            }                          
                                        }">
                        <thead data-options="frozen:true">
                            <tr>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'id',hidden:'true'"></th>
                                <th data-options="field:'wipEntityName',width:120,align:'left'">作业名称</th> 
                                <th data-options="field:'jobPrimaryItemDoc',width:90,align:'center'">装配件编码</th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <%--<th data-options="field:'jobPrimaryItemDes',width:160,align:'left'">装配件描述</th>--%>
                                <th data-options="field:'warehouseId',width:80,align:'center',
                                    styler:function(value,row,index){
                                        return 'background-color:#98FB98';
                                    },
                                    editor: {
                                        type: 'combobox',
                                        options: {
                                            valueField: 'id',
                                            textField: 'id',
                                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseComboboxList',
                                            multiple: false,
                                            required: true,
                                            mode: 'local',
                                            width: 80,
                                            panelWidth: 80,
                                            validType: 'checkInput',
                                            filter: function (q, row) {
                                                var opts = $(this).combobox('options');
                                                return row[opts.textField].indexOf(q) == 0;
                                            },
                                            onChange: function (newValue, oldValue) {
                                                warehouseIsChanged = true;
                                            }
                                        }
                                    }">供应子库</th>
                                <th data-options="field:'warelocationId',width:70,align:'center',
                                    styler:function(value,row,index){
                                        return 'background-color:#98FB98';
                                    },
                                    formatter:function(value, row){
                                        return row.locationDocNumber;
                                    },
                                    editor:{
                                        type:'combobox',
                                        options:
                                        {
                                            valueField: 'id',
                                            textField: 'docNumber',
                                            multiple: false,
                                            editable: false,
                                            mode: 'local',
                                            width: 70,
                                            panelWidth: 70
                                        }
                                    }">货位</th>
                                <th data-options="field:'prepareQuantity',width:60,align:'center'">备料数量</th>
                                <th data-options="field:'planQuantity',width:55,align:'center'">计划数量</th>
                                <th data-options="field:'containerBindNumber',width:55,align:'center'">已备数量</th>
                            </tr>
                        </thead>
		            </table>
                    <div id="tbDetail">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DetailReload();">刷新</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
  //  <%-- 备料明细 --%>
    <script type="text/javascript">
        var detailDataRows = [];
            var editIndex = undefined;
        var editSumIndex = undefined;
        function GetDetailRowsByMaterialId(materialId) {
            var _detailDataRows = [];
            for (var i = 0; i < detailDataRows.length; i++) {
                if (detailDataRows[i].materialId == materialId) {
                    _detailDataRows.push(detailDataRows[i]);
                }
            }
            $('#dgWmsPreparationDetail').datagrid('loadData', _detailDataRows);
        }
        $(function () {
            setButtons('<%=Model.status %>');
        })
        function setButtons(status) {
            $('#btnPreparationOk').hide();
            $('#detailSave').hide();
            if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.unPrepare %>') {
                $('#detailSave').show();
            } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparing %>') {
                $('#detailSave').show();
                //$('#btnPreparationOk').show();
            } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparingUnConfirm %>') {
                $('#btnPreparationOk').show();
            } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.prepared %>') {
                $('#detailSave').show();
                $('#btnPreparationOk').show();
            } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.picked %>') {

            }
        }
        function DetailSumReload() {
            $('#dgWmsPreparationDetailSum').datagrid('reload');
            $('#dgWmsPreparationDetail').datagrid('reload');
        }
        function DetailSumSave() {
            if (endEditing() && endSumEditing()) {
                var detailSumRows = $('#dgWmsPreparationDetailSum').datagrid('getChecked');
                if (detailSumRows.length == 0) {
                    $.messager.alert("提示", "缺少需求备料物料明细！", "info");
                    return;
                }
                var detailRows = [];
                for (var i = 0; i < detailSumRows.length; i++) {
                    for (var j = 0; j < detailDataRows.length; j++) {
                        if (detailDataRows[j].materialId == detailSumRows[i].materialId) {
                            detailRows.push(detailDataRows[j]);
                        }
                    }
                }
                var message = "";
                $.each(detailRows, function (i, row) {
                    if (row.warelocationId == null || row.warelocationId == undefined || row.warelocationId == "" || row.warelocationId <= 0) {
                        message = "备料清单有未指定货位的记录，请重新填写后保存！";
                       // return;
                    }
                });
                if (message != "") {
                    $.messager.alert("提示", message, "info");
                    return;
                }
                $.messager.confirm("确认", "确认提交保存？", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationDetailSave/<%=Model.id%>',
                            type: "post",
                            dataType: "json",
                            data: { "wmsPreparationDetailRows": $.toJSON(detailRows) },
                            success: function (r) {
                                if (r.result) {
                                    if (r.entity != null) {
                                        $('#statusCn').val(r.entity.statusCn);
                                        $('#status').val(r.entity.status);
                                        setButtons(r.entity.status);
                                    }
                                    $.messager.alert('提示', r.message, 'info');
                                } else {
                                    $.messager.alert('错误', '备料失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function PreparationOk() {
            var detailSumRows = $('#dgWmsPreparationDetailSum').datagrid('getChecked');
            if (detailSumRows.length == 0) {
                $.messager.alert("提示", "缺少需求备料物料明细！", "info");
                return;
            }
            var detailRows = [];
            for (var i = 0; i < detailSumRows.length; i++) {
                for (var j = 0; j < detailDataRows.length; j++) {
                    if (detailDataRows[j].materialId == detailSumRows[i].materialId) {
                        detailRows.push(detailDataRows[j]);
                    }
                }
            }
            var message = "";
            $.each(detailRows, function (i, row) {
                if (row.warelocationId == null || row.warelocationId == undefined || row.warelocationId == "" || row.warelocationId <= 0) {
                    message = "备料清单有未指定货位的记录，请重新填写后保存！";
                    return;
                }
            });
            if (message != "") {
                $.messager.alert("提示", message, "info");
                return;
            }
            $.messager.confirm("确认", "提交需求备料确认？", function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationConfirm/<%=Model.id%>',
                        type: "post",
                        dataType: "json",
                        data: { "wmsPreparationDetailRows": $.toJSON(detailRows) },
                        success: function (r) {
                            if (r.result) {
                                $.messager.alert('提示', '确认成功', 'info');
                            } else {
                                $.messager.alert('错误', '确认失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/WmsPreparationPrint.aspx';
            url += "?docNumber=" + $("input[name='docNumber']").val();
            parent.openTab('备料单打印', url);
        }
//    <%-- 编辑物料合计 --%>    
        function endSumEditing() {
            if (editSumIndex == undefined) { return true }
            if ($('#dgWmsPreparationDetailSum').datagrid('validateRow', editSumIndex)) {
                $('#dgWmsPreparationDetailSum').datagrid('endEdit', editSumIndex);
                editSumIndex = undefined;
                return true;
            } else {
                return false;
            }
        }
        function clickWmsPreparationSumDetailRow(index) {
            if (editSumIndex != index) {
                if (endSumEditing()) {
                    $('#dgWmsPreparationDetailSum').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
                    editSumIndex = index;
                } else {
                    $('#dgWmsPreparationDetailSum').datagrid('selectRow', editSumIndex);
                }
            }
        }
        var oldValue;
        $.extend($.fn.numberbox.defaults.rules, {
            checkSumPrepareQuantity: {
                validator: function (value, param) {
                    var result = true;
                    var selectRow = $('#dgWmsPreparationDetailSum').datagrid('getSelected');
                    var rowIndex = $('#dgWmsPreparationDetailSum').datagrid('getRowIndex', selectRow);
                    if (rowIndex != editSumIndex) { return true; }
                    result = value <=(selectRow.prepareQuantity - selectRow.containerBindNumber);
                    if (result) {
                        if (oldValue != value) {
                            if (endEditing())
                                assignPrepareQuantity(value);
                        }
                        oldValue = value;
                    }
                    return result;
                },
                message: '备料数量超出.'
            }
        });
        function assignPrepareQuantity(sumPrepareQuantity) {
            var detailRows = $('#dgWmsPreparationDetail').datagrid('getRows'); //物料合计
            var checkRows = $('#dgWmsPreparationDetail').datagrid('getChecked'); //
            for (var i = 0; i < detailRows.length; i++) {
                if (checkRows.length == 0) {
                    var prepareQuantity = detailRows[i].planQuantity - detailRows[i].containerBindNumber; // containerBindNumber：已备数量  quantity：
                    var assignPrepareQuantity = sumPrepareQuantity >= prepareQuantity ? prepareQuantity : sumPrepareQuantity;
                    detailRows[i].prepareQuantity = assignPrepareQuantity; //备料数量
                    sumPrepareQuantity -= assignPrepareQuantity;
                    $('#dgWmsPreparationDetail').datagrid('updateRow', { index: i, row: detailRows[i] });
                }
                else {
                    detailRows[i].prepareQuantity = 0;
                    $('#dgWmsPreparationDetail').datagrid('updateRow', { index: i, row: detailRows[i] });
                    for (var j = 0; j < checkRows.length; j++) {
                        if (detailRows[i].id == checkRows[j].id) {
                            var prepareQuantity = detailRows[i].planQuantity - detailRows[i].containerBindNumber; // containerBindNumber：已备数量  quantity：
                            var assignPrepareQuantity = sumPrepareQuantity >= prepareQuantity ? prepareQuantity : sumPrepareQuantity;
                            detailRows[i].prepareQuantity = assignPrepareQuantity; //备料数量
                            sumPrepareQuantity -= assignPrepareQuantity;
                            $('#dgWmsPreparationDetail').datagrid('updateRow', { index: i, row: detailRows[i] });
                            break;

                        }
                      
                    }

                 }
            }
//            for (var i = 0; i < detailRows.length; i++) {
//                var unAssigned = true;
//                if (checkRows.length > 0){
//                    for (var j = 0; j < checkRows.length; j++) {
//                        if (detailRows[i].id == checkRows[j].id) {
//                            unAssigned = false;
//                            break;
//                        }
//                    }
//					}
//                else {
//                    unAssigned = false;
//                    }
//            }
//            if (unAssigned) {
//                var prepareQuantity = detailRows[i].quantity - detailRows[i].containerBindNumber;
//                var assignPrepareQuantity = sumPrepareQuantity >= prepareQuantity ? prepareQuantity : sumPrepareQuantity;
//                detailRows[i].prepareQuantity = assignPrepareQuantity;
//                sumPrepareQuantity -= assignPrepareQuantity;
//                $('#dgWmsPreparationDetail').datagrid('updateRow', { index: i, row: detailRows[i] }); 
//            }           
            for (var i = 0; i < detailRows.length; i++) {
                for (var j = 0; j < detailDataRows.length; j++) {
                    if (detailRows[i].id == detailDataRows[j].id) {
                        detailDataRows[j] = detailRows[i];
                        break;
                    }
                }
            }
        }
        function DetailReload() {
            var selectSumRow = $('#dgWmsPreparationDetailSum').datagrid('getSelected');
            if (selectSumRow) {
                assignPrepareQuantityReload();
            } else {
                $('#dgWmsPreparationDetail').datagrid('reload');
            }
        }
        function assignPrepareQuantityReload(selectSumRow) {
            var selectSumRow = $('#dgWmsPreparationDetailSum').datagrid('getSelected');
            if (selectSumRow) {
                var rowIndex = $('#dgWmsPreparationDetailSum').datagrid('getRowIndex', selectSumRow);
                var ed = $('#dgWmsPreparationDetailSum').datagrid('getEditor', { index: rowIndex, field: 'prepareQuantity' });
                var sumPrepareQuantity;
                if (ed)
                    sumPrepareQuantity = Number($(ed.target).numberbox('getValue'));
                else
                    sumPrepareQuantity = selectSumRow.prepareQuantity;
                assignPrepareQuantity(sumPrepareQuantity);
            }
        }
//    <%-- 编辑备料明细 --%>
        var warehouseIsChanged = false;
        $.extend($.fn.combobox.defaults.rules, {
            checkInput: {
                validator: function (value, param) {
                    var selectRow = $('#dgWmsPreparationDetail').datagrid('getSelected');
                    var rowIndex = $('#dgWmsPreparationDetail').datagrid('getRowIndex', selectRow);
                    if (rowIndex != editIndex) { return true; }
                    var edWarehouse = $('#dgWmsPreparationDetail').datagrid('getEditor', { index: editIndex, field: 'warehouseId' });
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

                            var edWarelocation = $('#dgWmsPreparationDetail').datagrid('getEditor', { index: editIndex, field: 'warelocationId' });
                            var materialDocNumber = $('#dgWmsPreparationDetail').datagrid('getRows')[editIndex]['materialDocNumber'];
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

    
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgWmsPreparationDetail').datagrid('validateRow', editIndex)) {
                //子库
                var edWarehouse = $('#dgWmsPreparationDetail').datagrid('getEditor', { index: editIndex, field: 'warehouseId' });
                var warehouseId = $(edWarehouse.target).combobox('getText');
                $('#dgWmsPreparationDetail').datagrid('getRows')[editIndex]['warehouseId'] = warehouseId;
                //货位
                var edWarelocation = $('#dgWmsPreparationDetail').datagrid('getEditor', { index: editIndex, field: 'warelocationId' });
                var docNumber = $(edWarelocation.target).combobox('getText');
                $('#dgWmsPreparationDetail').datagrid('getRows')[editIndex]['locationDocNumber'] = docNumber;

                //变更子库或货位后同步更新
                var id = $('#dgWmsPreparationDetail').datagrid('getRows')[editIndex]['id'];
                for (var i = 0; i < detailDataRows.length; i++) {
                    if (detailDataRows[i].id.toString() == id.toString()) {
                        detailDataRows[i].warehouseId = warehouseId;
                        detailDataRows[i].warelocationId = $(edWarelocation.target).combobox('getValue');
                    }
                }

                $('#dgWmsPreparationDetail').datagrid('endEdit', editIndex);

                editIndex = undefined;

                return true;
            } else {
                return false;
            }
        }
        function clickWmsPreparationDetailRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgWmsPreparationDetail').datagrid('selectRow', index)
							.datagrid('beginEdit', index);

                    var ed = $('#dgWmsPreparationDetail').datagrid('getEditor', { index: index, field: 'warelocationId' });
                    $(ed.target).css({ "width": "92px" });
                    editIndex = index;
                } else {
                    $('#dgWmsPreparationDetail').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptWmsPreparationDetail() {
            if (endEditing()) {
                $('#dgWmsPreparationDetail').datagrid('acceptChanges');
            }
        }
    </script>
</asp:Content>
