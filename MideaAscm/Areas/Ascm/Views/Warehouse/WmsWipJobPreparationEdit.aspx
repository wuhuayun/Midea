﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业备料
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:140px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" id="btnPreparationConfirm" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="confirmWmsPreparation();">单据确认</a>
                        <%} %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="exportToPdf();">导出</a>
                        <iframe id="iframeExportToPdf" name="iframeExportToPdf" scrolling="auto" frameborder="0"  src="" style="width:0px;height:0px;display:none;"></iframe>
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
                    <form id="editWmsPreparationMainForm" method="post" style="">
                        <table style="width:690px;" border="0" cellpadding="0" cellspacing="0">
                            <tr style="height:24px">
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>作业号：</span>
						        </td>
                                <td style="width:140px">
                                    <input type="text" name="jobWipEntityName" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobWipEntityName%>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>单据号：</span>
						        </td>
                                <td style="width:140px">
                                    <input type="text" name="docNumber" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.docNumber%>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>单据状态：</span>
						        </td>
                                <td style="width:140px">
							        <input id="statusCn" name="statusCn" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.statusCn%>"/>
                                    <input id="status" name="status" type="hidden" value="<%=Model.status %>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>打单员：</span>
						        </td>
						        <td style="width:140px">
                                    <input type="text" name="createUser" style="width:130px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.createUser%>"/> 
						        </td> 
                            </tr>
                            <tr style="height:24px">
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>作业说明：</span>
						        </td>
                                <td colspan="3">
                                    <input type="text" name="jobDescription" style="width:330px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobDescription%>"/>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>作业状态：</span>
						        </td>
                                <td>
                                   <input type="text" name="jobStatusTypeCn" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobStatusTypeCn%>"/>  
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>打单时间：</span>
						        </td>
                                <td>
                                    <input type="text" name="createTimeShow" style="width:130px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.createTimeShow%>"/> 
						        </td>
                            </tr>
                            <tr style="height:24px">
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>装配件编码：</span>
						        </td>
                                <td>
                                    <input type="text" name="jobPrimaryItemDoc" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobPrimaryItemDoc%>"/> 
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>装配件描述：</span>
						        </td>
						        <td colspan="3">
                                    <input type="text" name="jobPrimaryItemDes" style="width:330px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobPrimaryItemDes%>"/> 
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>计划时间：</span>
						        </td>
                                <td>
                                   <input type="text" name="jobScheduledStartDate" style="width:130px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobScheduledStartDate%>"/>  
                                </td>
                            </tr>
                            <tr style="height:24px">
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>计划数量：</span>
						        </td>
                                <td>
                                    <input type="text" name="jobQuantity" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobQuantity%>"/>  
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
                                    <span>产线：</span>
                                </td>
                                <td>
                                   <input type="text" name="jobProductionLine" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobProductionLine%>"/>    
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>车间：</span>
						        </td>
                                <td>
                                   <input type="text" name="jobScheduleGroupsName" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobScheduleGroupsName%>"/>  
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
                                    <span>供应子库：</span>
                                </td>
                                <td>
                                   <input type="text" name="jobWarehouseSegment" style="width:130px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.jobWarehouseSegment%>"/>   
                                </td>
                            </tr>
				        </table>
                    </form>
                </div>
            </div>
        </div>    
    </div>
    <div region="center" title="" border="true" style="padding:0px;overflow:hidden;">
        <table id="dgWmsPreparationDetail" title="备料清单明细" class="easyui-datagrid" style="" border="false"
              data-options="fit: true,
                            singleSelect: true,
                            checkOnSelect: false,
                            selectOnCheck: false,
                            noheader: true,
                            rownumbers: true,
                            idField: 'id',
                            sortName: 'id',
                            sortOrder: '',
                            striped: true,
                            toolbar: '#tbDetail',
                            loadMsg: '数据加载中，请稍候...',
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationDetailList/<%=Model.id%>?isLoadERP=false',
                            onSelect: function(rowIndex, rowRec){
                                currentDetailId = rowRec.id;
                            },
                            onClickRow:function(rowIndex,rowRec){
                                <% if (ynWebRight.rightEdit){ %>
                                clickWmsPreparationDetailRow(rowIndex);
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
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'id',hidden:'true'"></th>
                    <th data-options="field:'wipEntityName',width:110,align:'left'">作业名称</th> 
                    <th data-options="field:'materialDocNumber',width:90,align:'center'">物料编码</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'materialName',width:230,align:'left'">物料名称</th>
                    <th data-options="field:'materialUnit',width:60,align:'center'">物料单位</th>
                    <th data-options="field:'warehouseId',width:80,align:'center',
                        styler:function(value,row,index){
                            return 'background-color:#98FB98';
                        },
                        editor:{
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
                    <th data-options="field:'dateRequired',width:110,align:'center'">需求日期</th>
                    <th data-options="field:'prepareQuantity',width:60,align:'center',
                        styler:function(value,row,index){
                            return 'background-color:#98FB98';
                        },
                        editor:{ 
                            type: 'validatebox',
                            options: { validType: 'checkNumber[5]' }
                        }">备料数量</th>
                    <th data-options="field:'planQuantity',width:60,align:'center'">计划数量</th>
                    <th data-options="field:'containerBindNumber',width:60,align:'center'">已备数量</th>
                    <th data-options="field:'receivedQuantity',width:60,align:'center'">已领数量</th>
                    <th data-options="field:'onhandQuantity',width:60,align:'center'">ERP库存</th>
                </tr>
            </thead>
		</table>
        <div id="tbDetail">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="reloadDetail();">刷新</a>
            <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" id="btnDetailAdd" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="addDetail();">加入</a>
            <a href="javascript:void(0);" id="btnDetailCancel" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="cancelDetail();">取消加入</a>
			<a href="javascript:void(0);" id="btnDetailSetLocation" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="setDetailLocation();">设置货位</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="reloadERPDetail();">刷新ERP数据</a>
            <%} %>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 主表 --%>
    <script type="text/javascript">
        $(function () {
            setButtons(<%=Model.locked %>, '<%=Model.status %>');

             //$('#btnDetailAdd').show();
        })
        function setButtons(locked, status) {       
            $('#btnPreparationConfirm').hide();
            $('#btnDetailAdd').hide();
            $('#btnDetailCancel').hide();
            $('#btnDetailSetLocation').hide();
            if (!locked) {
                if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.unPrepare %>') {
                    $('#btnDetailAdd').show();
                    $('#btnDetailSetLocation').show();
                } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparingUnConfirm %>') {
                    $('#btnPreparationConfirm').show();
                    $('#btnDetailAdd').show();
                    $('#btnDetailCancel').show();
                    $('#btnDetailSetLocation').show();
                }else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparing %>') {
                    $('#btnDetailAdd').show();
                    $('#btnDetailCancel').show();
                    $('#btnDetailSetLocation').show();
                } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.prepared %>') {
                    $('#btnDetailCancel').show();
                    $('#btnDetailSetLocation').show();
                }else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparingUnPick %>') {
                    $('#btnDetailSetLocation').show();
                } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparedUnPick %>') {
                    $('#btnDetailSetLocation').show();
                } else if (status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.picked %>') {
                    
                }
            }
        }
    </script>
    <%-- 明细 --%>
    <script type="text/javascript">
        var currentDetailId = "";
        function reloadDetail()
        {
            var options = $('#dgWmsPreparationDetail').datagrid('options');
            options.queryParams.isLoadERP = false;

            $('#dgWmsPreparationDetail').datagrid('reload');
        }

        function reloadERPDetail()
        {
            var options = $('#dgWmsPreparationDetail').datagrid('options');
            options.queryParams.isLoadERP = true;

            $('#dgWmsPreparationDetail').datagrid('reload');
        }

        function addDetail() {
            if (endEditing()) {
                var detailRows = $('#dgWmsPreparationDetail').datagrid('getChecked');
                if (detailRows.length == 0) {
                    $.messager.alert("提示", "请勾选备料明细！", "info");
                    return;
                }
                var message = "";
                $.each(detailRows, function (i, row) {
                    if (row.warelocationId == null || row.warelocationId == undefined || row.warelocationId == "" || row.warelocationId <= 0) {
                        message = "备料清单有未指定货位的记录，请先设置货位！";
                        return;
                    }
                });
                if (message != "") {
                    $.messager.alert("提示", message, "info");
                    return;
                }
                $.messager.confirm("确认", "确认执行【加入】？", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationDetailAdd/<%=Model.id%>',
                            type: "post",
                            dataType: "json",
                            data: { "preparationDetailRows": $.toJSON(detailRows) },
                            success: function (r) {
                                if (r.result) {
                                    if (r.entity != null) {
                                        $('#statusCn').val(r.entity.statusCn);
                                        $('#status').val(r.entity.status);
                                        setButtons(r.entity.locked, r.entity.status);
                                    }
                                    $.messager.alert('提示', '加入成功！', 'info', function () { reloadDetail(); });
                                } else {
                                    $.messager.alert('错误', '加入失败！' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function cancelDetail() {
            var detailRows = $('#dgWmsPreparationDetail').datagrid('getChecked');
            if (detailRows.length == 0) {
                $.messager.alert("提示", "请勾选备料明细！", "info");
                return;
            }
            var materialIds = "";
            $.each(detailRows, function (i, row) {
                if (row.containerBindNumber > 0) {
                    if (materialIds != "")
                        materialIds += ",";
                    materialIds += row.materialId;
                }
            });
            if (materialIds == "") {
                $.messager.alert("提示", "所勾选的备料明细不能执行【取消加入】操作！", "info");
                return;
            }
            $.messager.confirm("确认", "确认执行【取消加入】？", function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationDetailCancel/<%=Model.id%>',
                        type: "post",
                        dataType: "json",
                        data: { "wipEntityId": "<%=Model.wipEntityId%>", "materialIds": materialIds },
                        success: function (r) {
                            if (r.result) {
                                if (r.entity != null) {
                                    $('#statusCn').val(r.entity.statusCn);
                                    $('#status').val(r.entity.status);
                                    setButtons(r.entity.locked, r.entity.status);
                                }
                                $.messager.alert('提示', '取消加入成功！', 'info', function () { reloadDetail(); });
                            } else {
                                $.messager.alert('错误', '取消加入失败！' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function setDetailLocation() {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationDetailSetLocation/<%=Model.id%>',
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
        function confirmWmsPreparation() {
            var detailRows = $('#dgWmsPreparationDetail').datagrid('getRows');
            if (detailRows.length == 0) {
                $.messager.alert("提示", "缺少备料物料明细！", "info");
                return;
            }
            var message = "";
            $.each(detailRows, function (i, row) {
                if (row.warelocationId == null || row.warelocationId == undefined || row.warelocationId == "" || row.warelocationId <= 0) {
                    message = "备料清单有未指定货位的记录，请先设置货位！";
                    return;
                }
            });

            var isScheduled = "<%=Model.jobIsScheduled %>" == "True";
            if (!isScheduled) {
                var jobName = "<%=Model.jobWipEntityName %>";
                var msg = '<span style="text-align:left;color:red;font-weight:bold;">未排产作业</span><br /><span>' + jobName + '</span>';
                $.messager.alert("提示", msg, "info");
                return;
            }

            if (message != "") {
                $.messager.alert("提示", message, "info");
                return;
            }
            $.messager.confirm("确认", "确认执行【单据确认】？", function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationConfirm/<%=Model.id%>',
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                if (r.entity != null) {
                                    $('#statusCn').val(r.entity.statusCn);
                                    $('#status').val(r.entity.status);
                                    setButtons(r.entity.locked, r.entity.status);
                                }
                                $.messager.alert('提示', '单据确认成功', 'info');
                            } else {
                                $.messager.alert('错误', '单据确认失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
    </script>
    <%-- 编辑数量 --%>
    <script type="text/javascript">
        $.extend($.fn.validatebox.defaults.rules, {
            checkNumber: {
                validator: function (value, param) {
                    if (!isNotNullOrEmpty(value))
                        value = 0;

                    var reg = new RegExp("^[0-9]+\.{0,1}[0-9]{0," + param[0] + "}$");
                    return reg.test(value);
                },
                message: '数字输入非法，最多支持{0}位小数.'
            }
        });
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

        var editIndex = undefined;
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
    <%-- 导出 --%>
    <script type="text/javascript">
        function exportToPdf() {
            $.messager.confirm('确认', '确认导出作业备料单' + '' + '？', function (result) {
                if (result) {
                    var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationExportToPdf?mainIdList=<%=Model.id%>&bSingle=true&pattern=<%=Model.pattern%>';
                    $('#iframeExportToPdf').attr("src",url).trigger("beforeload");
                }
            });
        }
    </script>
</asp:Content>
