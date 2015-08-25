<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	送货批接收
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="west" title="" border="false" style="width:215px; padding:0px 2px 0px 0px; overflow:hidden;">
        <div class="easyui-panel" title="送货批接收" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" border="false" style="overflow:hidden;" title="">
                    <div style="margin:5px;width:200px;">
                        <span>批条码号：</span>
                        <input type="text" id="batchBarCode" name="batchBarCode" style="width:130px;" value=""/>
                    </div>
                    <div style="margin:5px;text-align:right;width:200px;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" icon="icon-ok" onclick="AddDeliBatToDetail();">添加</a>
                    </div>
                </div>
                <div region="center" border="false" style="overflow:auto; border-top:1px solid #95B8E7;" title="">
                    <ul id="ulAccExeResultMsg"></ul>
                </div>
            </div>
        </div>
    </div>
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false" style="overflow:hidden;">
			<div class="easyui-layout" fit="true" style="">
                <div region="south" border="true" fit="false" style="height:130px;padding:0px 0px 0px 0px;overflow:hidden;">
                    <table id="dgDeliBatOrderLink" title="" class="easyui-datagrid" border="false"
                        data-options="fit: true,
                                      rownumbers: true, 
                                      idField: 'id',
                                      loadMsg: '更新数据......',
                                      rowStyler: function (index, rowRec) {
                                          if (rowRec.wipEntityStatus == <%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.StatusTypeDefine.ygb %>) {
                                              return 'color:red;';
                                          } else if (rowRec.wipEntityStatus == <%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.StatusTypeDefine.yqx %>) {
                                              return 'color:red;';
                                          }
                                      },">
                        <thead>
                            <tr>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                                <th data-options="field:'materialName',width:280,align:'left'">物料描述</th>
                                <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                                <th data-options="field:'deliveryQuantity',width:80,align:'right'">送货数量</th>
                                <th data-options="field:'receivedQuantity',width:80,align:'right'">接收数量</th>
                                <th data-options="field:'wipEntityName',width:120,align:'center'">作业号</th>
                                <th data-options="field:'wipEntityStatusCn',width:90,align:'center'">作业状态</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div region="center" border="false" fit="false" style="padding:0px 0px 2px 0px;overflow:hidden;">
                    <!--送货批单-->
                    <table id="dgDeliveryOrderBatch" title="送货批清单" style="" border="true"></table>    
                    <div id="tb">
                        <% if (ynWebRight.rightEdit){ %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DetailRemove();">移除</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="AssignWarelocation();">分配货位</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="IncomingAcceptanceComplete();">接收确认</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="DeliBatMaterialPrint();">物料标签打印</a>
                        <%} %>
                    </div>
                    <!--进度条-->           
                    <div id="wProgressbar" class="easyui-window" title="完成接收......" style="padding: 10px;width:440px;height:80px;"
                        data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
                        <div id="p" class="easyui-progressbar" style="width:400px;"></div>
                    </div>
                    <!--分配货位-->
                    <div id="wAssignWarelocation" class="easyui-window" title="分配货位" style="padding: 5px;width:540px;height:380px;"
                        data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
                                <table id="dgAssignWarelocation" title="货位信息" class="easyui-datagrid" style="" border="false"
                                    data-options="fit: true,
                                                  noheader: true,
                                                  rownumbers: true,
                                                  singleSelect : true,
                                                  idField: 'id',
                                                  striped: true,
                                                  toolbar: '#tbAssignWarelocation',
                                                  loadMsg: '更新数据......',
                                                  onClickRow: clickAssignWarelocationRow,
                                                  onLoadSuccess: function(){
                                                      editIndex = undefined;                     
                                                  }">
                                    <thead data-options="frozen:true">
                                        <tr>
                                            <th data-options="field:'id',hidden:true"></th>
                                            <th data-options="field:'warelocationId',hidden:true"></th>
                                            <th data-options="field:'locationDocNumber',width:90,align:'center'">货位编号</th>
                                        </tr>
                                    </thead>
                                    <thead>
                                        <tr>
                                            <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                                            <th data-options="field:'categoryCode',width:60,align:'center'">物料大类</th>
                                            <th data-options="field:'batchBarCode',width:80,align:'center'">批条码号</th>
                                            <th data-options="field:'quantity',width:70,align:'center'">已存数量</th>
                                            <th data-options="field:'assignQuantity',width:70,align:'center',editor:{ type: 'text' }">分配数量</th>
                                        </tr>
                                    </thead>
		                        </table>
                                <div id="tbAssignWarelocation">
                                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="AssignWarelocationSave();">确认</a>
                                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wAssignWarelocation').window('close');">取消</a> 
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--物料标签打印-->
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/DeliBatIncAccPrintSelect.ascx"); %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/lodop/LodopFuncs.js"></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/jquery/howler.min.js"></script>
    <style type="text/css">
        #ulAccExeResultMsg li
        {
        	margin-top: 5px;
        	border-bottom: 1px dashed #95B8E7;
        }
        #ulAccExeResultMsg li div
        {
            text-indent: 5px;	
        }
    </style>
    <%-- 初始化 --%>
    <script type="text/javascript">
        $(function () {
            $('#batchBarCode').keypress(function (e) {
                var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keyCode == 13) {
                    AddDeliBatToDetail();
                    $(this).select();
                }          
            })

            $('#batchBarCode').focus();

            $('#dgDeliveryOrderBatch').datagrid({
                fit: true,
                rownumbers: true,
                singleSelect: true,
                checkOnSelect: false,
                selectOnCheck: false,
                idField: 'id',
                sortName: 'id',
                sortOrder: 'asc',
                striped: true,
                toolbar: '#tb',
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { checkbox: true },
                    { field: 'id', hidden: true },
                    { field: 'exeResult', title: '执行结果', width: 60, align: 'center' },
                    { field: 'barCode', title: '批条码号', width: 70, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 90, align: 'center' }
                ]],
                columns: [[
                    { field: 'materialName', title: '物料描述', width: 230, align: 'left' },
                    { field: 'totalNumber', title: '送货数量', width: 60, align: 'center' },
                    { field: 'receivedQuantity', title: '接收数量', width: 60, align: 'center',
                        editor: {
                            type: 'validatebox',
                            options: {
                                validType: 'checkQuantity[5]'
                            }
                        }
                    },
                    { field: 'receivedWarehouseId', title: '收货子库', width: 100,
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
                        }
                    },
                    { field: 'assignWarelocation', title: '分配货位', width: 100,
                        editor: {
                            type: 'text'
                        }
                    },
                    { field: 'createTimeShow', title: '生成日期', width: 110, align: 'center' },
                    { field: 'statusCn', title: '状态', width: 60, align: 'center' },
                    { field: 'wipLine', title: '送货地点', width: 80, align: 'center' }
                ]],
                rowStyler: function (index, row) {
                    if (row.exeResult) {
                        return 'color:red;';
                    }
                },
                onSelect: function (index, row) {
                    DeliBatOrderLinkReload(row.id);
                },
                onClickRow: clickDeliOrderBatRow,
                onDblClickRow: function (rowIndex, rowRec) {
                    AssignWarelocation();
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    $(this).datagrid('clearChecked');
                    editDeliOrderBatIndex = undefined;
                    var row = $('#dgDeliveryOrderBatch').datagrid('getSelected');
                    var rowId = row ? row.id : "";
                    DeliBatOrderLinkReload(rowId);
                }
            });
        })
        var LODOP;
        function DeliBatMaterialPrint() {
            var downloadLodop32 = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/install_lodop32.exe';
            var downloadLodop64 = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/install_lodop64.exe';
            LODOP = getLodop(document.getElementById('LODOP'), document.getElementById('LODOP_EM'), downloadLodop32, downloadLodop64);

            getDeliBatMaterial();
        }
        var sound = new Howl({ urls: ['../../Content/welcome.mp3'] });
        //送货批明细
        function DeliBatOrderLinkReload(id) {
            if (id == "") {
                $('#dgDeliBatOrderLink').datagrid('loadData', { "total": 0, "rows": [] });
            } else {
                var options = $('#dgDeliBatOrderLink').datagrid('options');
                options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatOrderLinkDetail/' + id;
                $('#dgDeliBatOrderLink').datagrid('reload');
            }
        }
        //添加送货批
        function AddDeliBatToDetail() {
            if (editDeliOrderBatEditing()) {
                var batchBarCode = $('#batchBarCode').val();
                if (batchBarCode == undefined || batchBarCode == "") {
                    $.messager.alert("提示", "请输入送货批条码号", "info", function () {
                        $('#batchBarCode').focus();
                    });
                    return;
                }
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatIncAccAdd/',
                    type: "post",
                    dataType: "json",
                    data: { "batchBarCode": batchBarCode },
                    success: function (r) {
                        sound.stop().play();
                        if (r.result) {
                            if (r.entity.ascmStatus == '<%=MideaAscm.Dal.FromErp.Entities.AscmDeliveryOrderBatch.AscmStatusDefine.received %>') {
                                AddAccExeResultMsg(batchBarCode, "批单已接收");
                                //$('#batchBarCode').val("");
                                $('#batchBarCode').focus();
                            } else {
                                var rows = $('#dgDeliveryOrderBatch').datagrid('getRows');
                                var row = null;
                                $.each(rows, function (i, item) {
                                    if (item.id == r.entity.id) {
                                        row = item;
                                        return;
                                    }
                                });   
                                if (row) {
                                    editDeliOrderBatIndex = $('#dgDeliveryOrderBatch').datagrid('getRowIndex', row);
                                } else {
                                    $('#dgDeliveryOrderBatch').datagrid('appendRow', r.entity);
                                    editDeliOrderBatIndex = $('#dgDeliveryOrderBatch').datagrid('getRows').length - 1;
                                }
                                $('#dgDeliveryOrderBatch').datagrid('selectRow', editDeliOrderBatIndex)
                                            .datagrid('beginEdit', editDeliOrderBatIndex);
                                var ed = $('#dgDeliveryOrderBatch').datagrid('getEditor', { index: editDeliOrderBatIndex, field: 'assignWarelocation' });
                                $(ed.target).attr('readonly', 'readonly');
                                $(ed.target).css({ 'background-color': '#FBEC88', 'border': '0px', 'cursor': 'default' });

                                //$('#batchBarCode').val("");
                                $('#batchBarCode').focus();
                            }
                        } else {
                            AddAccExeResultMsg(batchBarCode, r.message);
                           //$('#batchBarCode').val("");
                            $('#batchBarCode').focus();
                        }
                    }
                });
            }
        }
        function DetailRemove() {
            var checkRows = $('#dgDeliveryOrderBatch').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert('提示', '请勾选要移除的批单', 'info');
                return;
            }
            $.messager.confirm('确认', '确认移除勾选的批单？', function (result) {
                if (result) {
                    var rows = $('#dgDeliveryOrderBatch').datagrid('getRows');
                    var newRows = new Array();
                    $.each(rows, function (i, row) {
                        var isAdd = true;
                        $.each(checkRows, function (j, item) {
                            if (item.id ==row.id) {
                                isAdd = false;
                                return;
                            }
                        });
                        if (isAdd) {
                            newRows.push(row);
                        }
                    });
                    $('#dgDeliveryOrderBatch').datagrid('loadData', newRows);
                }
            });
        }
    </script>
    <%-- 编辑送货批接收数量 --%>
    <script type="text/javascript">
        $.extend($.fn.validatebox.defaults.rules, {
            checkQuantity: {
                validator: function (value, param) {
                    if (!isNotNullOrEmpty(value))
                        value = 0;

                    //验证数字输入合法性
                    var reg = new RegExp("^[0-9]+\.{0,1}[0-9]{0," + param[0] + "}$");
                    if (!reg.test(value)) { return false; }

                    var selectRow = $('#dgDeliveryOrderBatch').datagrid('getSelected');
                    var rowIndex = $('#dgDeliveryOrderBatch').datagrid('getRowIndex', selectRow);
                    if (rowIndex != editDeliOrderBatIndex) { return true; }

                    value = Number(value);
                    if (value <= selectRow.totalNumber && value > 0) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatReceivedQuantityUpdate/',
                            type: "post",
                            dataType: "json",
                            data: { "batchId": selectRow.id, "receivedQuantity": value },
                            success: function (r) {
                                if (r.result) {
                                    DeliBatOrderLinkReload(selectRow.id);
                                }
                            }
                        });
                        return true;
                    } else {
                        return false;
                    }
                },
                message: '接收数量必须大于0且不能大于总数量.'
            }
        });
        var warehouseIsChanged = false;
        $.extend($.fn.combobox.defaults.rules, {
            checkInput: {
                validator: function (value, param) {
                    var selectRow = $('#dgDeliveryOrderBatch').datagrid('getSelected');
                    var rowIndex = $('#dgDeliveryOrderBatch').datagrid('getRowIndex', selectRow);
                    if (rowIndex != editDeliOrderBatIndex) { return true; }
                    var ed = $('#dgDeliveryOrderBatch').datagrid('getEditor', { index: rowIndex, field: 'receivedWarehouseId' });
                    var flag = false;
                    if (ed) {
                        var data = $(ed.target).combobox('getData');
                        $.each(data, function (i, item) {
                            if (item.id == value) {
                                flag = true;
                                return;
                            }
                        });
                        if (flag && warehouseIsChanged) {
                            warehouseIsChanged = false;
                            var receivedQuantity = 0;
                            var rows = $('#dgDeliBatOrderLink').datagrid('getRows');
                            $.each(rows, function (i, item) {
                                receivedQuantity += Number(item.receivedQuantity);
                            })
                            $.ajax({
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatReceivedWarehouseUpdate/',
                                type: "post",
                                dataType: "json",
                                data: {
                                    "batchId": selectRow.id,
                                    "receivedWarehouseId": value,
                                    "materialId": selectRow.materialIdTmp,
                                    "receivedQuantity": receivedQuantity
                                },
                                beforeSend: function () {
                                    return true;
                                },
                                success: function (r) {
                                    if (r.result) {
                                        var _ed = $('#dgDeliveryOrderBatch').datagrid('getEditor', { index: rowIndex, field: 'assignWarelocation' });
                                        if (_ed)
                                            $(_ed.target).val(r.entity.assignWarelocation);
                                    }
                                }
                            });
                        }
                    }
                    return flag;
                },
                message: '子库输入错误.'
            }
        });

        var editDeliOrderBatIndex = undefined;
        function editDeliOrderBatEditing() {
            if (editDeliOrderBatIndex == undefined) { return true }
            if ($('#dgDeliveryOrderBatch').datagrid('validateRow', editDeliOrderBatIndex)) {
                var ed = $('#dgDeliveryOrderBatch').datagrid('getEditor', { index: editDeliOrderBatIndex, field: 'receivedWarehouseId' });
                var warehouseId = $(ed.target).combobox('getText');
                $('#dgDeliveryOrderBatch').datagrid('getRows')[editDeliOrderBatIndex]['receivedWarehouseId'] = warehouseId;

                $('#dgDeliveryOrderBatch').datagrid('endEdit', editDeliOrderBatIndex);
                editDeliOrderBatIndex = undefined;
                return true;
            } else {
                return false;
            }
        }
        function clickDeliOrderBatRow(index, row) {
            if (editDeliOrderBatIndex != index) {
                if (editDeliOrderBatEditing()) {
                    $('#dgDeliveryOrderBatch').datagrid('selectRow', index)
							.datagrid('beginEdit', index);

                    var ed = $('#dgDeliveryOrderBatch').datagrid('getEditor', { index: index, field: 'assignWarelocation' });
                    $(ed.target).attr('readonly', 'readonly');
                    $(ed.target).css({ 'background-color': '#FBEC88', 'border': '0px', 'cursor': 'default' });

                    editDeliOrderBatIndex = index;
                } else {
                    $('#dgDeliveryOrderBatch').datagrid('selectRow', editDeliOrderBatIndex);
                }
            }
        }
        function acceptDeliOrderBat() {
            if (editDeliOrderBatEditing()) {
                $('#dgDeliveryOrderBatch').datagrid('acceptChanges');
            }
        }
    </script>
    <%-- 编辑货位预分配数量 --%>
    <script type="text/javascript">
        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgAssignWarelocation').datagrid('validateRow', editIndex)) {
                $('#dgAssignWarelocation').datagrid('endEdit', editIndex);
                editIndex = undefined;
                return true;
            } else {
                return false;
            }
        }
        function clickAssignWarelocationRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgAssignWarelocation').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
                    editIndex = index;
                } else {
                    $('#dgAssignWarelocation').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptAssignWarelocation() {
            if (endEditing()) {
                $('#dgAssignWarelocation').datagrid('acceptChanges');
            }
        }
    </script>
    <%-- 分配货位 --%>
    <script type="text/javascript">
        function AssignWarelocation() {
            if (editDeliOrderBatEditing()) {
                var selectRow = $('#dgDeliveryOrderBatch').datagrid('getSelected');
                if (!selectRow) {
                    $.messager.alert('提示', '请选中一行送货批单', 'info');
                    return;
                }
                var options = $('#dgAssignWarelocation').datagrid('options');
                options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatAssignWarelocation';
                options.queryParams.batchIds = selectRow.id;
                $('#dgAssignWarelocation').datagrid('reload');
                $('#wAssignWarelocation').window('open');
            }
        }
        function AssignWarelocationSave() {
            var assignWarelocationRows = $('#dgAssignWarelocation').datagrid('getRows');
            if (assignWarelocationRows.length > 0) {
                var selectRow = $('#dgDeliveryOrderBatch').datagrid('getSelected');
                if (!selectRow) {
                    $.messager.alert('提示', '请选中一行送货批单', 'info');
                    return;
                }
                acceptAssignWarelocation();
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatAssignWarelocationSave/',
                    type: "post",
                    dataType: "json",
                    data: { "batchIds": selectRow.id, "assignWarelocationJson": $.toJSON(assignWarelocationRows) },
                    beforeSend: function () {
                        var checkQuantity = true;
                        var assignQuantity = 0, receivedQuantity = 0;
                        $.each(assignWarelocationRows, function (i, item) {
                            assignQuantity += Number(item.assignQuantity);
                        })
                        var rows = $('#dgDeliBatOrderLink').datagrid('getRows');
                        $.each(rows, function (i, item) {
                            receivedQuantity += Number(item.receivedQuantity);
                        })
                        if (assignQuantity != receivedQuantity) {
                            $.messager.alert('错误', '送货批[<font color=\"red\">' + selectRow.barCode + '</font>]接收数量[<font color=\"red\">' + receivedQuantity + '</font>]与货位分配总数量[<font color=\"red\">' + assignQuantity + '</font>]不相等', 'error');
                            checkQuantity = false;
                        }
                        return checkQuantity;
                    },
                    success: function (r) {
                        if (r.result) {
                            //分配货位后刷新批单货位分配
                            $.each(r.entity, function (i, item) {
                                if (item.id == selectRow.id) {
                                    var rowIndex = $('#dgDeliveryOrderBatch').datagrid('getRowIndex', selectRow);
                                    $('#dgDeliveryOrderBatch').datagrid('updateRow', {
                                        index: rowIndex,
                                        row: { assignWarelocation: item.assignWarelocation }
                                    });
                                }
                            });
                            $('#wAssignWarelocation').window('close');
                        } else {
                            $.messager.alert('错误', r.message, 'error');
                        }
                    }
                })
            }
        }
    </script>
    <%-- 接收确认 --%>
    <script type="text/javascript">
        var receivedBatchIds = ""; //记录单次接收成功的批单，提供给“物料标签打印”
        function IncomingAcceptanceComplete() {
            if (editDeliOrderBatEditing()) {
                $('#dgDeliveryOrderBatch').datagrid('acceptChanges');
                var checkRows = $('#dgDeliveryOrderBatch').datagrid('getChecked');
                if (checkRows.length == 0) {
                    $.messager.alert('提示', '请勾选送货批单', 'info');
                    return;
                }
                var message = "";
                var batchIds = "";
                $.each(checkRows, function (index, item) {
                    if (batchIds)
                        batchIds += ",";
                    batchIds += item.id;
                    if (!item.assignWarelocation) {
                        message += "<li>送货批[<font color='red'>" + item.barCode + "</font>]未预分配货位</li>";
                    }
                })
                receivedBatchIds = batchIds;
                if (message != "") {
                    $.messager.alert('提示', "<ul>" + message + "</ul>", 'info');
                    return;
                }
                $.messager.confirm("确认", "确认接收完成？", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/DeliBatAssignIncAccComplete/',
                            type: "post",
                            dataType: "json",
                            data: { "batchIds": batchIds },
                            beforeSend: function () {
                                $('#wProgressbar').window('open');
                                $('#p').progressbar('setValue', 0);
                                setInterval(updateProgress, 200);
                                return true;
                            },
                            success: function (r) {
                                if (r.result) {
                                    $('#p').progressbar('setValue', 100);
                                    $('#wProgressbar').window('close');

                                    if (r.message)
                                        $.messager.alert('警告', r.message, 'warning', function () {
                                            acceptanceCompleted(checkRows, r);
                                            $('#dgDeliveryOrderBatch').datagrid('clearSelections');
                                            $('#dgDeliveryOrderBatch').datagrid('clearChecked');
                                        });
                                    else {
                                        $.messager.alert('确认', '接收完成', 'info', function () {
                                            acceptanceCompleted(checkRows, r);
                                            $('#dgDeliveryOrderBatch').datagrid('clearSelections');
                                            $('#dgDeliveryOrderBatch').datagrid('clearChecked');
                                        });
                                    }
                                } else {
                                    $('#p').progressbar('setValue', 100);
                                    $('#wProgressbar').window('close');
                                    $.messager.alert('错误', r.message, 'error');
                                }
                            }
                        })
                    }
                })
            }
        }
        function acceptanceCompleted(checkRows, r) {
            var successIds = new Array();
            $.each(checkRows, function (i, row) {
                $.each(r.entity, function (j, item) {
                    if (item.billId == row.id) {
                        AddAccExeResultMsg(item.docNumber, item.returnMessage);
                        if (item.returnCode == "0") {
                            successIds.push(item.billId);
                        } else {
                            var rowIndex = $('#dgDeliveryOrderBatch').datagrid('getRowIndex', row);
                            $('#dgDeliveryOrderBatch').datagrid('updateRow', {
                                index: rowIndex,
                                row: { exeResult: '接收失败' }
                            });
                        }
                    }
                });
            });
            if (successIds.length > 0) {
                var rows = $('#dgDeliveryOrderBatch').datagrid('getRows');
                var newRows = new Array();
                $.each(rows, function (i, row) {
                    var isAdd = true;
                    $.each(successIds, function (j, item) {
                        if (item == row.id) {
                            isAdd = false;
                            return;
                        }
                    });
                    if (isAdd) {
                        newRows.push(row);
                    }
                });
                $('#dgDeliveryOrderBatch').datagrid('loadData', newRows);
            }
        }
        function updateProgress() {
            var value = $('#p').progressbar('getValue');
            if (value < 99) {
                $('#p').progressbar('setValue', value + 1);
            }
        }
    </script>
    <%-- 执行信息 --%>
    <script type="text/javascript">
        function AddAccExeResultMsg(barCode, resultMsg) {
            var newResultMsg = "<li>"
                             + "<div>批条码号：" + barCode + "</div>"
                             + "<div>信息：<span style=\"color:red;\">" + resultMsg + "</span></div>"
                             + "<div>时间：" + GetCurrentDateTime() + "</div>"
                             + "</li>";
            $("#ulAccExeResultMsg").prepend(newResultMsg);
        }
        function GetCurrentDateTime() {
            var date = new Date();
            var yyyy = date.getFullYear();
            var MM = date.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            var dd = date.getDate();
            dd = dd < 10 ? "0" + dd : dd;
            var HH = date.getHours();
            HH = HH < 10 ? "0" + HH : HH;
            var mm = date.getMinutes();
            mm = mm < 10 ? "0" + mm : mm;
            return yyyy + "-" + MM + "-" + dd + " " + HH + ":" + mm;
        }
    </script>
</asp:Content>
