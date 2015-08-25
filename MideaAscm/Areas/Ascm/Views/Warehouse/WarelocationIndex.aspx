<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.Warehouse.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	货位管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--仓库子库列表-->
    <div region="west" split="false" border="false" title="" style="width:350px;padding:0px 2px 0px 0px;overflow:hidden;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
                    <table id="dgWarehouse" title="仓库子库" class="easyui-treegrid" border="false"
                        data-options="fit: true,
                                      idField: 'id',
                                      treeField: 'name',
                                      toolbar: '#tb',
                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/BuildingWarehouseList/',
                                      onClickRow: function (rowData) {
                                          if (currentId != rowData.id)
                                              currentWarelocationId = null;      
                                          currentId = rowData.id;
                                          WarelocationReload();
                                      },
                                      onLoadSuccess: function () {
                                          $(this).treegrid('clearSelections');
                                          if (currentId) {
                                              $(this).treegrid('select', currentId);
                                          }
                                          WarelocationReload();
                                      }">
                        <thead>
                            <tr>
                                <th data-options="field:'id',hidden:true"></th>
                                <th data-options="field:'name',width:280,align:'left'">名称</th>
                                <th data-options="field:'code',width:60,align:'center'">代号</th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="Reload();">刷新</a>
                        <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ImportExcel();">导入</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ExportExcel();">导出</a>
                        <%--<a href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MaterialExport/" class="easyui-linkbutton" plain="true" icon="icon-add"/>导出</a>--%>
                        <% } %>
                    </div>
                    <div id="popImport" class="easyui-window" title="数据导入" style="padding: 10px;width:380px;height:200px;"
                        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" id="FileUpload" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
					            <form id="FormUpload" enctype="multipart/form-data" method="post" ><br />
                                    <p><input type="file" id="fileImport" name="fileImport" size="35" value=''/>&nbsp;&nbsp;</p>
                                </form>
                            </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					            <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="ImportOk()">保存</a>
                                <% } %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#popImport').window('close');">取消</a>
					        </div>
                        </div>
                    </div>
                    <div id="wProgressbar" class="easyui-window" title="正在导入......" style="padding: 10px;width:440px;height:80px;"
                        data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
                        <div id="p" class="easyui-progressbar" style="width:400px;"></div>
                    </div>
			    </div>
			</div>
		</div>
	</div>
    <!--货位管理-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false" style="overflow:hidden;">
			<div class="easyui-layout" fit="true" style="">
			    <div region="north" title="" border="false" fit="false" style="height:280px;padding:0px 0px 2px 0px;overflow:hidden;">
                    <div id="pWarelocation" class="easyui-panel" title="货位管理" fit="true" border="true" style="overflow:hidden;">
					<table id="dgWarelocation" title="" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="shelfNo,layer,No" sortOrder="asc" striped="true" toolbar="#tb2">
		            </table>
                    <div id="tb2">
                        <input id="warelocationSearch" type="text" style="width:100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WarelocationReload();"></a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="WarelocationReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="WarelocationAdd();">增加</a>--%>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="WarelocationEdit();">修改</a>--%>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="WarelocationDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editWarelocation" class="easyui-window" title="货位管理" style="padding: 10px;width:500px;height:380px;"
				        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
			                    <form id="editWarelocationForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>货位编码：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input id="docNumber" name="docNumber" type="text" style="width:200px;background-color:#CCCCCC;" readonly="readonly"/>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>厂房名称：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input id="buildingName" name="buildingName" type="text" style="width:200px;background-color:#CCCCCC;" readonly="readonly"/>
                                                <input id="buildingId" name="buildingId" type="hidden"/>
						                    </td>
					                    </tr>
                                        <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>子库名称：</span>
						                    </td>
						                    <td style="width:80%">
                                                <input id="warehouseId" name="warehouseId" type="text" style="width:200px;background-color:#CCCCCC;" readonly="readonly"/>
						                    </td>
					                    </tr>
                                        <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>物料大类：</span>
						                    </td>
						                    <td style="width:80%">
                                                <input class="easyui-validatebox" id="categoryCode" name="categoryCode" type="text" data-options="validType:'checkNumber[4]'" style="width:120px;" /><span>（物料编码前四位）</span><span style="color:Red;">*</span>
						                    </td>
					                    </tr>
                                        <tr style="height:24px">
                                            <td style="width: 20%; text-align:right;" nowrap>
							                    <span>货位形式：</span>
						                    </td>
						                    <td style="width:80%">
                                                <select id="type" name="type" style="width:125px;" onchange="typeOnChange();">
                                                <option value=""></option>
							                    <% List<int> listTypeDefine = AscmWarelocation.TypeDefine.GetList(); %>
                                                <% if (listTypeDefine != null){ %>
                                                <% foreach (int typeDefine in listTypeDefine){ %>
                                                <option value="<%=typeDefine %>"><%=AscmWarelocation.TypeDefine.DisplayText(typeDefine) %></option>
                                                <%} %>
                                                <%} %>
                                                </select>
						                    </td>
                                        </tr>
                                        <tr style="height:24px" id="trShelfNo">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>货架号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="shelfNo" name="shelfNo" type="text" data-options="validType:'checkNumber[2]'" style="width:120px;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>
                                        <tr style="height:24px" id="trRow">
						                    <td style="width: 20%; text-align:right;" nowrap>
                                                <span>行：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="floorRow" name="floorRow" type="text" data-options="validType:'checkNumber[2]'" style="width:120px;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>
                                        <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>层号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-numberbox" id="layer" name="layer" type="text" style="width:120px;" data-options="min:1" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>
                                        <tr style="height:24px" id="trNo">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>货位号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="No" name="No" type="text" data-options="validType:'checkNumber[2]'" style="width:120px;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>
                                        <tr style="height:24px" id="trColumn">
						                    <td style="width: 20%; text-align:right;" nowrap>
                                                <span>列：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="floorColumn" name="floorColumn" type="text" data-options="validType:'checkNumber[2]'" style="width:120px;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>
					                    <%--<tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>上限：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-numberbox" id="upperLimit" name="upperLimit" type="text" style="width:120px;" data-options="min:0" />
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>下限：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-numberbox" id="lowerLimit" name="lowerLimit" type="text" style="width:120px;" data-options="min:0" />
						                    </td>
					                    </tr>--%>
					                    <tr style="height:24px">
						                    <td style="text-align:right;" nowrap>
							                    <span>描述：</span>
						                    </td>
						                    <td>
							                    <input class="easyui-validatebox" id="description" name="description" type="text" style="width:300px;" />
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="WarelocationSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editWarelocation').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
                    </div>
                </div>
                <div region="center" border="true" fit="false" style="padding:0px 0px 0px 0px;overflow:hidden;">
                    <table id="dgLocationMaterialLink" title="关联物料" class="easyui-datagrid" border="false"
                        data-options="fit: true,
                                      idField: 'pk',
                                      toolbar: '#tb3',
                                      singleSelect: true,
                                      checkOnSelect: false,
                                      selectOnCheck: false,
                                      pagination: true,
                                      pageSize: 20,
                                      loadMsg: '更新数据......',
                                      onSelect: function (rowIndex, rowData) {
                                           
                                      },
                                      onClickRow: function (rowIndex, rowData) {
                                          <%--<% if (ynWebRight.rightEdit){ %>
                                              clickMaterial(rowIndex);
                                          <%} %>--%>
                                      },
                                      onLoadSuccess: function () {
                                          $(this).datagrid('clearSelections');
                                          $(this).datagrid('clearChecked');
                                          editIndex=undefined;
                                      }">
                        <thead>
                            <tr>
                                <th data-options="field:'pk',hidden:true"></th>
                                <th data-options="checkbox:true"></th>
                                <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                                <th data-options="field:'materialDescription',width:350,align:'left'">物料描述</th>
                                <th data-options="field:'quantity',width:80,align:'center'">物料数量</th>
                                <%--<th data-options="field:'quantity',width:80,align:'center',editor: { type: 'numberbox', options: { min: 0, validType: 'checkQuantity'}}">物料数量</th>--%>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb3">
                        <input id="materialSearch" type="text" style="width:100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="LinkReload();"></a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="LinkReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="LinkAdd();">添加</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="LinkRemove();">移除</a>
                        <%} %>
                        <%--<% if (ynWebRight.rightEdit){ %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="MaterialSave()">保存</a>
                        <%} %>--%>
                    </div>
                    <%-- 选择物料 --%>
                    <%Html.RenderPartial("~/Areas/Ascm/Views/Public/MaterialItemMultipleSelect.ascx"); %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%--仓库子库--%>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#warelocationSearch').keypress(function (e) {
                var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keyCode == 13) {
                    WarelocationReload();
                }
            })

            $('#trShelfNo').hide();
            $('#trRow').hide();
            $('#trNo').hide();
            $('#trColumn').hide();
        })
        function Reload() {
            $('#dgWarehouse').treegrid('reload');
        }
        function updateProgress() {
            var value = $('#p').progressbar('getValue');
            if (value < 99) {
                $('#p').progressbar('setValue', value + 1);
            }
        }
        function ImportExcel() {
            $('#popImport').window('open');
        }
        function ImportOk() {
            var _selFile = $('#FormUpload input[Name=fileImport]').val();
            if (_selFile == "") {
                $.messager.alert('警告', "请选择文件！", 'warning');
                return;
            }
            $('#wProgressbar').window('open');
            $('#p').progressbar('setValue', 0);
            setInterval(updateProgress, 600);
            $('#FormUpload').form('submit', {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MaterialImport/',
                onSubmit: function () {
                    return $('#FormUpload').form('validate');
                },
                success: function (result) {
                    $('#p').progressbar('setValue', 100);
                    $('#wProgressbar').window('close');
                    var retValue = eval('(' + result + ')');
                    $('#popImport').window('close');
                    if (retValue.result) {
                        $.messager.alert('确认', retValue.message, '');
                    }
                    else {
                        $.messager.alert('错误', retValue.message, 'error');
                    }
                }
            });
        }
        function ExportExcel() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MaterialExport/';
            var selectRow = $('#dgWarehouse').treegrid('getSelected');
            if (selectRow)
                url += "?buildingId=" + selectRow.buildingId;
            var iframe = document.createElement("iframe");
            iframe.src = url;
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>
    <%--货位--%>
    <script type="text/javascript">
        $.extend($.fn.validatebox.defaults.rules, {
            checkNumber: {
                validator: function (value, param) {
                    var reg = new RegExp("^[0-9]{" + param[0] + "}$");
                    return reg.test(value);
                },
                message: '必须输入{0}位数字.'
            }
        });
        $(function () {
            $('#dgWarelocation').datagrid({
                pagination: true,
                pageSize: 20,
                loadMsg: '更新数据......',
                /* 冻结列后滚动会出现错位 */
                /*frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'docNumber', title: '货位编码', width: 100, align: 'center' },
                    { field: 'categoryCode', title: '物料大类', width: 80, align: 'center' }
                ]],*/
                columns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'docNumber', title: '货位编码', width: 70, align: 'center' },
                    { field: 'warehouseId', title: '子库', width: 80, align: 'center' },
                    { field: 'categoryCode', title: '物料大类', width: 70, align: 'center' },
                    { field: 'shelfNo', title: '货架号', width: 60, align: 'center' },
                    { field: 'layer', title: '层号', width: 50, align: 'center' },
                    { field: 'warehouseUserName', title: '仓管员', width: 80, align: 'center' },
                    { field: 'totalNumber', title: '已存数量', width: 80, align: 'center' },
                    { field: 'description', title: '描述', width: 240, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentWarelocationId = rowData.id;
                    LinkReload();
                },
                onDblClickRow: function (rowIndex, rowData) {
//                    <% if (ynWebRight.rightEdit){ %>
//                    WarelocationEdit();
//                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    var selectRow = $(this).datagrid('getSelected');
                    if (selectRow) {
                        $(this).datagrid('selectRecord', selectRow.id);
                    } else {
                        LinkReload();
                    }
                }
            });
        });
        function WarelocationReload() {
            var title = "货位管理";
            var _buildingId = "", _warehouseId = ""; 
            var selectRow = $('#dgWarehouse').treegrid('getSelected');
            if (selectRow) {
                _buildingId = selectRow.buildingId;
                _warehouseId = selectRow.warehouseId;
                if (_warehouseId != null && _warehouseId != "" && _warehouseId != undefined) { 
                    var parentRow = $('#dgWarehouse').treegrid('getParent', selectRow.id);
                    if (parentRow) {
                        title = "货位管理【厂房：<font color='red'>" + parentRow.name + "</font> 子库：<font color='red'>" + selectRow.docNumber + "</font>】";
                    }
                } else {
                    title = "货位管理【厂房：<font color='red'>" + selectRow.name + "</font>】";
                    _warehouseId = "";
                }
            }
            $('#pWarelocation').panel({ title: title });
            var options = $('#dgWarelocation').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarelocationList/';
            options.queryParams.queryWord = $('#warelocationSearch').val();
            options.queryParams.buildingId = _buildingId;
            options.queryParams.warehouseId = _warehouseId;
            $('#dgWarelocation').datagrid('reload');
        }
        function typeOnChange() {
            $('#trShelfNo').hide();
            $('#trRow').hide();
            $('#trNo').hide();
            $('#trColumn').hide();
            var _type = $('#type').val();
            switch (_type) { 
                case '<%=AscmWarelocation.TypeDefine.shelf %>':
                case '<%=AscmWarelocation.TypeDefine.highShelf %>':
                    $('#trShelfNo').show();
                    $('#trNo').show();
                    break;
                case '<%=AscmWarelocation.TypeDefine.floor %>':
                    $('#trRow').show();
                    $('#trColumn').show();
                    break;
                default: break;
            }
        }
        var currentWarelocationId = null;
        function WarelocationAdd() {
            var selectRow = $('#dgWarehouse').treegrid('getSelected');
            if (selectRow) {
                $("#editWarelocation").window("open");
                $("#editWarelocationForm").form('clear');

                $('#categoryCode').focus();
                var selectParentRow = $('#dgWarehouse').treegrid('getParent', selectRow.id);
                $('#buildingId').val(selectParentRow.buildingId);
                $('#buildingName').val(selectParentRow.name);
                $('#warehouseId').val(selectRow.warehouseId);

                $('#trShelfNo').hide();
                $('#trRow').hide();
                $('#trNo').hide();
                $('#trColumn').hide();
                currentWarelocationId = null;
            } else {
                $.messager.alert('提示', '请选择子库', 'info');
            }
        }
        function WarelocationEdit() {
            var selectRow = $('#dgWarelocation').datagrid('getSelected');
            if (selectRow) {
                $('#editWarelocation').window('open');
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarelocationEdit/' + selectRow.id,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editWarelocationForm").form('clear');
                            $('#docNumber').val(r.docNumber);
                            $('#buildingId').val(r.buildingId);
                            $('#buildingName').val(r.buildingName);
                            $('#warehouseId').val(r.warehouseId);
                            $('#categoryCode').val(r.categoryCode);
                            $('#type').val(r.type);
                            if (r.type == '<%=AscmWarelocation.TypeDefine.shelf %>' || r.type == '<%=AscmWarelocation.TypeDefine.highShelf %>') {
                                $('#trShelfNo').show();
                                $('#trNo').show();
                                $('#shelfNo').val(r.shelfNo);
                                $('#No').val(r.No);
                            } else if (r.type == '<%=AscmWarelocation.TypeDefine.floor %>') {
                                $('#trRow').show();
                                $('#trColumn').show();
                                $('#floorRow').val(r.floorRow);
                                $('#floorColumn').val(r.floorColumn);
                            }
                            $('#layer').numberbox('setValue', r.layer);
                            //$('#upperLimit').numberbox('setValue', r.upperLimit);
                            //$('#lowerLimit').numberbox('setValue', r.lowerLimit);
                            $('#description').val(r.description);

                            $('#categoryCode').focus();
                        }
                    }
                });
                currentWarelocationId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择货位', 'info');
            }
        }
        function WarelocationSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var _warelocationId = "";
                    var selectRow = $('#dgWarelocation').datagrid('getSelected');
                    if (selectRow)
                        _warelocationId = selectRow.id;
                    $('#editWarelocationForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarelocationSave/' + _warelocationId,
                        onSubmit: function () {
                            return $('#editWarelocationForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editWarelocation').window('close');
                                currentWarelocationId = rVal.id;
                                WarelocationReload();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
        function WarelocationDelete() {
            var selectRow = $('#dgWarelocation').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除货位[<font color="red">' + selectRow.docNumber + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarelocationDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    currentWarelocationId = null;
                                    WarelocationReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择货位', 'info');
            }
        }
    </script>
    <%--货位物料--%>
    <script type="text/javascript">
        $.extend($.fn.numberbox.defaults.rules, {
            checkQuantity: {
                validator: function (value, param) {
                    if (editIndex == undefined) { return true; }
                    return value >= 0;
                },
                message: '数量必须大于等于0！'
            }
        });
        function LinkReload() {
            var _warelocationId = "";
            var selectRow = $('#dgWarelocation').datagrid('getSelected');
            if (selectRow) {
                _warelocationId = selectRow.id;
            }
            var options = $('#dgLocationMaterialLink').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationMaterialLinkList/';
            options.queryParams.warelocationId = _warelocationId;
            options.queryParams.queryWord = $('#materialSearch').val();
            $('#dgLocationMaterialLink').datagrid('reload');
        }
        function LinkAdd() {
            var selectRow = $('#dgWarelocation').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请先选择货位！", "info");
                return;
            }
            SelectMaterialItem(selectRow.categoryCode);
        }
        function MaterialItemSelected(selectRows) {
            if (selectRows) {
                $.messager.confirm('确认', '确认添加选择的物料到货位？', function (result) {
                    if (result) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationMaterialLinkAdd/',
                            type: "post",
                            dataType: "json",
                            data: { "warelocationId": currentWarelocationId, "materialItemJson": $.toJSON(selectRows) },
                            success: function (r) {
                                if (r.result) {
                                    LinkReload();
                                } else {
                                    $.messager.alert('确认', '添加失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function LinkRemove() {
            var selectRow = $('#dgWarelocation').datagrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请先选择货位！", "info");
                return;
            }
            var checkRows = $('#dgLocationMaterialLink').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert('提示', '请勾选要移除的物料！', 'info');
                return;
            }
            var linkIds = "";
            for (var i = 0; i < checkRows.length; i++) {
                if (linkIds) {
                    linkIds += ",";
                }
                linkIds += checkRows[i].pk.materialId;
            }
            $.messager.confirm('确认', '确认从货位中移除选择的物料？', function (result) {
                if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/LocationMaterialLinkRemove/' + selectRow.id,
                        type: "post",
                        dataType: "json",
                        data: { "linkIds": linkIds },
                        success: function (r) {
                            if (r.result) {
                                $.messager.alert('提示', r.message, 'info');
                                LinkReload();
                            } else {
                                $.messager.alert('确认', '移除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function MaterialSave() {
            var detailRow = $('#dgLocationMaterialLink').datagrid('getRows');
            acceptProcedureArgument();
            if (detailRow == null || detailRow.length == 0) {
                $.messager.alert('确认', '没有物料!', 'error');
                return;
            }
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MaterialSave/',
                        type: "post",
                        dataType: "json",
                        data: { "detailJson": $.toJSON(detailRow) },
                        success: function (r) {
                            if (r.result) {
                                $.messager.alert('提示', '物料数据更改完成!', 'info');
                            } else {
                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    })
                }
            });
        }
    </script>
    <%-- 单击编辑 --%>
    <script type="text/javascript">
        var editIndex = undefined;
        function endEditing() {
            if (editIndex == undefined) { return true }
            if ($('#dgLocationMaterialLink').datagrid('validateRow', editIndex)) {
                $('#dgLocationMaterialLink').datagrid('endEdit', editIndex);
                editIndex = undefined;

                return true;
            } else {
                return false;
            }
        }
        function clickMaterial(index) {
            if (editIndex != index) {
                if (endEditing()) {
                    $('#dgLocationMaterialLink').datagrid('selectRow', index)
							.datagrid('beginEdit', index);
                    editIndex = index;
                } else {
                    $('#dgLocationMaterialLink').datagrid('selectRow', editIndex);
                }
            }
        }
        function acceptProcedureArgument() {
            if (endEditing()) {
                $('#dgLocationMaterialLink').datagrid('acceptChanges');
            }
        }
    </script>
</asp:Content>
