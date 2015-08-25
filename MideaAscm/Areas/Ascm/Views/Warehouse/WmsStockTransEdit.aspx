<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsStockTransMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库转移
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:160px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                      
                        <a href="javascript:void(0);" id="mainAdd" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">新增</a>
                        <a href="javascript:void(0);" id="mainSave" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="Save('draft');">保存</a>
                        <a href="javascript:void(0);" id="mainSubmit" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="Submit();">提交</a>
                      
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
                    <form id="editWmsStockTransMainForm" method="post" style="">
                        <table style="width:720px;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>单据号：</span>
						        </td>
						        <td style="width:160px">
                                    <input id="docNumber" type="text" name="docNumber" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.docNumber%>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>手工单号：</span>
						        </td>
						        <td style="width:160px">
                                    <input id="manualDocNumber" type="text" name="manualDocNumber" style="width:120px;" value="<%=Model.manualDocNumber %>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>责任人：</span>
						        </td>
						        <td style="width:160px">
                                    <input id="responsiblePerson" type="text" name="responsiblePerson" style="width:120px;background-color:#F0F0F0;" readonly="readonly"/>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>事务类型：</span>
						        </td>
                                <td nowrap="nowrap">
                                    <input id="transTypeName" name="transTypeName" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.transType%>"/>
                                    <input type="hidden" id="transType" name="transType"/>
							        <a href="javascript:void(0);" id="selectTransType" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择事务类型" onclick="SelectMesTransStyle();"></a>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>转移原因：</span>
						        </td>
                                <td nowrap="nowrap">
                                    <input id="reasonName" name="reasonName" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.reasonName%>"/>
                                    <input id="reasonId" name="reasonId" type="hidden" value="<%=Model.reasonId %>"/>
							        <a href="javascript:void(0);" id="selectReason" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择原因" onclick="SelectReason();"></a>
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
                                     <span>目标子库组长：</span>
                                </td>
                                <td>
                                    <select id="tUser" name="tUser" class="easyui-combogrid" style="width:120px;"
                                        data-options="panelWidth: 450,
                                                      editable: false,
                                                      delay: 500,
                                                      idField: 'userId',
                                                      textField: 'userName',
                                                      pagination: true,
                                                      pageSize: 30,
                                                      mode: 'remote',
                                                      fitColumns: false,
                                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseUserAscxList',
                                                      columns: [[
                                                          { field: 'userId', title: '用户账号', width: 120, align: 'center' },
                                                          { field: 'userName', title: '用户名称', width: 120, align: 'center' }
                                                      ]]">
                                    </select>
                                    <input id="toWarehouseUser" name="toWarehouseUser" type="hidden" value="<%=Model.toWarehouseUser %>"/>
                                </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>来源仓库：</span>
						        </td>
						        <td nowrap="nowrap">
                                    <input id="fromWarehouseId" name="fromWarehouseId" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.fromWarehouseId%>"/>
							        <a href="javascript:void(0);" id="selectFromWarehouse" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择仓库" onclick="selectWarehouse='selectFromWarehouse';SelectWarehouse();"></a>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>目标仓库：</span>
						        </td>
						        <td nowrap="nowrap">
                                    <input id="toWarehouseId" name="toWarehouseId" type="text" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.toWarehouseId%>"/>
							        <a href="javascript:void(0);" id="selectToWarehouse" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择仓库" onclick="selectWarehouse='selectToWarehouse';SelectWarehouse();"></a>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
                                    <span>来源子库仓管员：</span>
                                </td>
                                <td>
                                    <select id="fUser" name="fUser" class="easyui-combogrid" style="width:120px;"
                                        data-options="panelWidth: 450,
                                                      editable: false,
                                                      delay: 500,
                                                      idField: 'userId',
                                                      textField: 'userName',
                                                      pagination: true,
                                                      pageSize: 30,
                                                      mode: 'remote',
                                                      fitColumns: false,
                                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseUserAscxList',
                                                      columns: [[
                                                          { field: 'userId', title: '用户账号', width: 120, align: 'center' },
                                                          { field: 'userName', title: '用户名称', width: 120, align: 'center' }
                                                      ]]">
                                    </select>
                                     <input id="fromWarehouseUser" name="fromWarehouseUser" type="hidden" value="<%=Model.fromWarehouseUser %>"/>
                                </td>
					        </tr>
                            <tr>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>参考：</span>
						        </td>
                                <td colspan="5">
                                    <input type="text" id="reference" name="reference" style="width:480px;" value="<%=Model.reference %>"/>
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
                    <%-- 选择事务类型 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/AscmMesTransStyleSelect.ascx"); %>
                    <%-- 选择转移原因 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/TransactionReasonSelect.ascx"); %>
                </div>
            </div>
        </div>
    </div>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dgWmsStockTransDetail" title="转移单明细" class="easyui-datagrid" style="" border="false"
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
                              currentDetailId = rowRec.id;
                          },
                          onClickRow:function(rowIndex,rowRec){
                               <% if (ynWebRight.rightEdit){ %>
                              <%--DetailEdit();--%>
                              clickWmsStockTransDetailRow(rowIndex);
                              <%} %>
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              
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
                    <th data-options="field:'materialDescription',width:280,align:'left'">物料描述</th>
                    <th data-options="field:'fromWarelocationDocNumber',width:100,align:'center'">来源货位</th>
                    <%--<th data-options="field:'toWarelocationDocNumber',width:100">目标货位</th>--%>
                    <th data-options="field:'toWarelocationId',width:100,align:'center',
                        formatter:function(value,row){
                            return row.toWarelocationDocNumber;
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
                    }">目标货位</th>
                    <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                    <th data-options="field:'fromQuantity',width:100,align:'center'">来源货位物料数量</th>
                    <th data-options="field:'quantity',width:70,align:'center',editor:'numberbox'">转移数量</th>
                    <th data-options="field:'reference',width:120,align:'center',editor:'text'">参考</th>
                </tr>
            </thead>
        </table>
        <div id="tbDetail">
            <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DetailReload();">刷新</a>--%>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" id="detailAdd" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="DetailAdd();">添加</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" id="detailRemove" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="DetailRemove();">移除</a>
            <%} %>
        </div>
        <%-- 选择货位物料 --%>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/LocationMaterialMultipleSelect.ascx"); %>
        <!--进度条-->           
        <div id="wProgressbar" class="easyui-window" title="完成转移......" style="padding: 10px;width:440px;height:80px;"
            data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
            <div id="p" class="easyui-progressbar" style="width:400px;"></div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 仓库转移主表 --%>
    <script type="text/javascript">
        var selectWarehouse;
        function WarehouseSelected(r) {
            if (r) {
                var detailRows = $('#dgWmsStockTransDetail').datagrid('getRows');
                if(detailRows.length>0)
                {
                    $.messager.confirm('确认', '更改仓库会清空仓库转移单明细，是否确认更改？', function (result) {
                         if (result) {
                            $('#dgWmsStockTransDetail').datagrid('loadData',[]);
                            if (selectWarehouse == 'selectFromWarehouse')
                            {
                                $('#fromWarehouseId').val(r.id);
                            }
                            else if (selectWarehouse == 'selectToWarehouse')
                            {
                                $('#toWarehouseId').val(r.id);
                            }
                         }
                    });
                }
                else
                {
                    if (selectWarehouse == 'selectFromWarehouse')
                    {
                        $('#fromWarehouseId').val(r.id);
                    }
                    else if (selectWarehouse == 'selectToWarehouse')
                    {
                        $('#toWarehouseId').val(r.id);
                    }
                }
            }
        }
        function SetButtons() {
            var status = $('#status').val();
            
            $('#mainSave').hide();
            $('#detailAdd').hide();
            $('#detailRemove').hide();
            $('#selectWarehouse').hide();
            $('#selectTransType').hide();
            $('#selectReason').hide();
            $('#selectFromWarehouse').hide();
            $('#selectToWarehouse').hide();
            <% if (ynWebRight.rightEdit){ %>
            if (status=="") {
                $('#mainSave').show();
                $('#detailAdd').show();
                $('#detailRemove').show();
                $('#selectWarehouse').show();
                $('#selectTransType').show();
                $('#selectReason').show();
                $('#selectFromWarehouse').show();
                $('#selectToWarehouse').show();
            }
            <%} %>
        }
        /*
         *  清空表单数据 显示按钮
         */
        function Add(){
            $('#docNumber').val("");
            $('#manualDocNumber').val("");
            $('#responsiblePerson').val("");
            $('#transTypeName').val("");
            $('#reasonName').val("");
            $('#fromWarehouseId').val("");
            $('#toWarehouseId').val("");
            $('#reference').val("");
            $('#fUser').combogrid('setValue', '');
            $('#tUser').combogrid('setValue', '');
            
            $('#mainSubmit').show();
            $('#mainSave').show();
            $('#detailAdd').show();
            $('#detailRemove').show();
            $('#selectWarehouse').show();
            $('#selectTransType').show();
            $('#selectReason').show();
            $('#selectFromWarehouse').show();
            $('#selectToWarehouse').show();

            $('#dgWmsStockTransDetail').datagrid('loadData', []);
            id=null;
        }
        var id = null;
        var statusOption = "";//初始状态为草稿
        function Submit() {
            statusOption = "pending"; //状态改为待审核
            if(id == null)
            {
                Save(statusOption);//保存为待审核
                
            }else{
                $.messager.confirm("确认", "是否提交？", function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransUpdate',
                            type: "post",
                            dataType: "json",
                            data: { "id": id, "statusOption": statusOption },
                            success: function (r) {
                                if (r.result) {
                                    $.messager.alert('提示', '提交完成!', 'info');
                                    $('#dataGridWmsStockTrans').datagrid('reload');
                                } else {
                                    $.messager.alert('错误', '提交失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function Save(statusOption) {
            var detailRows = $('#dgWmsStockTransDetail').datagrid('getRows');
            if (detailRows.length == 0) {
                $.messager.alert('错误', '没有添加仓库转移明细!', 'error');
                return;
            }
            acceptProcedureArgument();
            //获取姓名
            var toWarehouseUser = $('#tUser').combogrid('getValue');
            var fromWarehouseUser = $('#fUser').combogrid('getValue');
            $('#toWarehouseUser').val(toWarehouseUser);
            $('#fromWarehouseUser').val(fromWarehouseUser);

            //验证转移明细数量
            var message = "";
            $.each(detailRows, function (index, item) {
                if(item.toWarelocationId==null||item.toWarelocationId==""||item.toWarelocationId=="0")
                {
                    message += "<li>转移单明细有未选择目标货位项，请重新填写后保存！</li>";
                    $('#dgWmsStockTransDetail').datagrid('selectRecord', item.id);
                    return;
                }
                if (item.quantity > item.fromQuantity) {
                    message += "<li>物料<font color='red'>" + item.materialDocNumber + "</font>转移数量[" + item.quantity + "]大于来源货位物料数量[" + item.fromQuantity + "]</li>";
                    $('#dgWmsStockTransDetail').datagrid('selectRecord', item.id);
                    return;
                }
            })
            if (message != "") {
                $.messager.alert('错误', "<ul>" + message + "</ul>", 'error');
                return;
            }
            $.messager.confirm("确认", "确认提交保存？", function (r) {
                if (r) {
                    var data = {
                        "detailJson": $.toJSON(detailRows),
                        "statusOption": statusOption
                    };
                    var options = {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Warehouse/WmsStockTransSave/',
                        type: 'POST',
                        dataType: 'json',
                        data: data,
                        beforeSubmit: function () {
                            var flag = $('#editWmsStockTransMainForm').form('validate');
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

                                $("#editWmsStockTransMainForm input[name='docNumber']").val(r.entity.docNumber);
                                //$("#editWmsStockTransMainForm input[name='manualDocNumber']").val(r.entity.manualDocNumber);
                                $("#editWmsStockTransMainForm input[name='responsiblePerson']").val(r.entity.responsiblePerson);
                                id = r.entity.id;

                                SetButtons();
                                if(statusOption == "pending")
                                    $('#mainSubmit').hide();
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
                    $('#editWmsStockTransMainForm').ajaxForm(options);
                    $('#editWmsStockTransMainForm').submit();
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
    <%-- 仓库转移明细 --%>
    <script type="text/javascript">
        var currentDetailId = "";
        function DetailAdd() {
            var fromWarehouseId = $('#fromWarehouseId').val();
            if (fromWarehouseId == "" || fromWarehouseId == null || fromWarehouseId == "0") {
                $.messager.alert("提示", "请先选择来源仓库！", "info");
                $('#selectFromWarehouse').focus();
                return;
            }
            var toWarehouseId = $('#toWarehouseId').val();
            if (toWarehouseId == "" || toWarehouseId == null || toWarehouseId == "0") {
                $.messager.alert("提示", "请先选择目标仓库！", "info");
                return;
            }
            if (toWarehouseId == fromWarehouseId)
            {
                $.messager.alert("提示", "来源仓库和目标仓库不能为同一仓库，请重新选择！", "info");
                return;
            }
            var fUser =  $('#fUser').combogrid('getValue')
            if (fUser == "" || fUser == null) {
                $.messager.alert("提示", "请选择来源子库仓管员！", "info");
                return;
            }
            var tUser = $('#tUser').combogrid('getValue')
            if (tUser == "" || tUser == null) {
                $.messager.alert("提示", "请选择目标子库组长！", "info");
                return;
            }
            SelectLocationMaterial(fromWarehouseId);
        }
        var maxId = 0;
        function LocationMaterialSelected(selectRows) {
            if (selectRows) {
                $.messager.confirm('确认', '确认添加选择的库位物料到仓库转移单？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransDetailAdd/?toWarehouseId=' + encodeURIComponent($('#toWarehouseId').val()),
                            type: "post",
                            dataType: "json",
                            data: { "locationMaterialJson": $.toJSON(selectRows) },
                            success: function (r) {
                                if (r.result) {
                                    var rows = $('#dgWmsStockTransDetail').datagrid('getRows');
                                    $.each(r.entity, function (i, item) {
                                        var isAdd = true;
                                        if (item.toWarelocationId == 0)
                                            item.toWarelocationId = "";
                                        if (item.id == 0) {
                                            maxId = maxId - 1;
                                            item.id = maxId;
                                        }
                                        $.each(rows, function (_i, _item) {
                                            if (_item.fromWarelocationId == item.fromWarelocationId && _item.materialId == item.materialId) {
                                                isAdd = false;
                                                return;
                                            }
                                        });
                                        if (isAdd) {
                                            var reference = $('#reference').val();
                                            item.reference = reference;
                                            $('#dgWmsStockTransDetail').datagrid('appendRow', item);
                                        }
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
            var checkRows = $('#dgWmsStockTransDetail').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要移除的库位物料！", "info");
                return;
            }
            $.messager.confirm('确认', '确认移除勾选的物料？', function (result) {
                if (result) {
                    var vArry = new Array();
                    var rows = $('#dgWmsStockTransDetail').datagrid('getRows');
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
                    $('#dgWmsStockTransDetail').datagrid('loadData', vArry);
                }
            })
        }
        function MesTransStyleSelected(r) {
            if (r) {
                $('#transTypeName').val(r.name);
                $('#transType').val(r.code);
            }
        }
        function ReasonSelected(r) {
            if (r) {
                $('#reasonId').val(r.reasonId);
                $('#reasonName').val(r.reasonName);
            }
        }
    </script>
    <%-- 单击编辑 --%>
    <script type="text/javascript">
        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgWmsStockTransDetail').datagrid('validateRow', editIndex)) {
                var ed = $('#dgWmsStockTransDetail').datagrid('getEditor', { index: editIndex, field: 'toWarelocationId' });
                var docNumber = $(ed.target).combobox('getText');
                $('#dgWmsStockTransDetail').datagrid('getRows')[editIndex]['toWarelocationDocNumber'] = docNumber;
                $('#dgWmsStockTransDetail').datagrid('endEdit', editIndex);
                editIndex = undefined;

                return true;
            } else {
                return false;
            }
        }
        function clickWmsStockTransDetailRow(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgWmsStockTransDetail').datagrid('selectRow', index)
							.datagrid('beginEdit', index);

                    var ed = $('#dgWmsStockTransDetail').datagrid('getEditor', { index: index, field: 'toWarelocationId' });
                    var mtlDoc = $('#dgWmsStockTransDetail').datagrid('getRows')[index]['materialDocNumber'];
                    var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WareLocationCbList/?warehouseId=' + encodeURIComponent($('#toWarehouseId').val()) + "&materialDocNumber=" + mtlDoc;
                    $(ed.target).combobox('reload', url);

                    editIndex = index;

                } else {
                    $('#dgWmsStockTransDetail').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptProcedureArgument() {
            if (endEditing()) {
                $('#dgWmsStockTransDetail').datagrid('acceptChanges');
            }
        }
    </script>
</asp:Content>
