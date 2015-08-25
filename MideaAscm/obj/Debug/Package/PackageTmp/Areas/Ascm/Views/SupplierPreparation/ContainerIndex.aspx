﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	容器管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgContainer" title="容器管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          <%--checkOnSelect: false,
                          selectOnCheck: true,--%>
                          idField: 'sn',
                          sortName: 'sn',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据......',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.sn;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              Edit();
                              <%} %>
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                     
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <%--<th data-options="checkbox:true"></th>--%>
                    <th data-options="field:'sn',width:100,align:'center'">容器编号</th>
                    <th data-options="field:'spec',width:120,align:'center'">规格</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'rfid',width:100,align:'center'">RFID</th>
                    <th data-options="field:'supplierName',width:180,align:'left'">供应商名称</th>
                    <th data-options="field:'statusCn',width:80,align:'center'">状态</th>
                    <th data-options="field:'description',width:160,align:'left'">描述</th>
                    <th data-options="field:'createUser',width:80,align:'center'">创建人</th>
                    <th data-options="field:'_createTime',width:120,align:'center'">创建时间</th>
                    <th data-options="field:'modifyUser',width:100,align:'center'">最后更新人</th>
                    <th data-options="field:'_modifyTime',width:120,align:'center'">最后更新时间</th>
                </tr>
            </thead>
		</table>
        <div id="tb1" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>供应商：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
                <span>状态：</span>
                <select id="queryStatus" name="queryStatus" style="width:80px;">
                    <option value=""></option>
                    <% List<int> listStatusDefine = MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.GetList(); %>
                    <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                        { %>
                    <% foreach (int statusDefine in listStatusDefine)
                        { %>
                    <option value="<%=statusDefine %>"><%=MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.DisplayText(statusDefine)%></option>
                    <% } %>
                    <% } %>
                </select>
            </div>
            <div>
                <span>编号段：</span>
                <input id="queryStartSn" name="queryStartSn" type="text" style="width:100px;text-align:right;" />--
                <input id="queryEndSn" name="queryEndSn" type="text" style="width:100px;text-align:right;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
                <% if (ynWebRight.rightAdd){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
                <%} %>
                <% if (ynWebRight.rightEdit){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
                <%} %>
                <% if (ynWebRight.rightDelete){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="$('#deleteContainer').window('open');">删除</a>
                <%} %>
            </div>
        </div>
		<div id="editContainer" class="easyui-window" title="容器修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editContainerForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>供应商：</span>
						        </td>
						        <td style="width:80%">
							        <span id="spanSupplier"><%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "supplierId" }); %></span>
                                    <input type="text" id="supplierName" style="width:300px;background-color:#CCCCCC;" readonly="readonly"/><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>规格：</span>
						        </td>
						        <td style="width:80%">
							        <span id="spanSpec"><%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/ContainerSpecSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "specId" }); %></span>
                                    <input type="text" id="spec" style="width:300px;background-color:#CCCCCC;" readonly="readonly"/><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>RFID编号：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="startSn" name="startSn" required="true" type="text" style="width:100px;text-align:right;" />--
                                    <input class="easyui-validatebox" id="endSn" name="endSn" required="true" type="text" style="width:100px;text-align:right;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>状态：</span>
						        </td>
						        <td style="width:80%">
                                    <input id="statusCn" name="statusCn" type="text" style="width:100px;background-color:#CCCCCC;" readonly="readonly"/>
                                    <input id="status" name="status" type="hidden" />
						        </td>
					        </tr>
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
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editContainer').window('close');">取消</a>
				</div>
			</div>
		</div>
        <div id="deleteContainer" class="easyui-window" title="容器删除" style="padding: 10px;width:540px;height:320px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
				    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					    <tr style="height:24px">
						    <td style="width: 20%; text-align:right;" nowrap>
							    <span>编号：</span>
						    </td>
						    <td style="width:80%">
							    <input class="easyui-validatebox" id="deleteStartSn" name="deleteStartSn" required="true" type="text" style="width:100px;text-align:right;" />--
                                <input class="easyui-validatebox" id="deleteEndSn" name="deleteEndSn" required="true" type="text" style="width:100px;text-align:right;" /><span style="color:Red;">*</span>
						    </td>
					    </tr>
				    </table>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightDelete){ %>
					<a id="btnOk" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Delete()">确认</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#deleteContainer').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        function Query() {
            var options = $('#dgContainer').datagrid('options');
            options.queryParams.supplierId = $('#supplierSelect').combogrid('getValue');
            options.queryParams.status = $('#queryStatus').val();
            options.queryParams.queryStartSn = $('#queryStartSn').val();
            options.queryParams.queryEndSn = $('#queryEndSn').val();
            $('#dgContainer').datagrid('reload');
        }
        var currentId = "";
        function Add() {
            $('#spanSupplier').show();
            $('#supplierName').hide();
            $('#spanSpec').show();
            $('#spec').hide();
            $('#startSn').css('background-color', '');
            $('#endSn').css('background-color', '');
            $('#startSn').removeAttr('readonly');
            $('#endSn').removeAttr('readonly');
            $('#editContainer').window('open');
            $("#editContainerForm")[0].reset();
            $('#statusCn').val('<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.DisplayText(MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.unuse) %>');
            $('#status').val('<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.unuse %>');

            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dgContainer').datagrid('getSelected');
            if (selectRow) {
                $('#editContainer').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerEdit/' + currentId;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editContainerForm")[0].reset();
                            $('#spanSupplier').hide();
                            $('#supplierName').show();
                            $('#supplierName').val(r.supplierName);
                            $('#spanSpec').hide();
                            $('#spec').show();
                            $('#spec').val(r.spec);
                            $('#startSn').val(r.startSn);
                            $('#endSn').val(r.endSn);
                            $('#status').val(r.status);
                            $('#statusCn').val(r.statusCn);
                            $('#description').val(r.description);

                            $('#startSn').css('background-color', '#CCCCCC');
                            $('#endSn').css('background-color', '#CCCCCC');
                            $('#startSn').attr('readonly', true);
                            $('#endSn').attr('readonly', true);
                        }
                    }
                });
                currentId = selectRow.sn;
            } else {
                $.messager.alert('提示', '请选择容器', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editContainerForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerSave/' + currentId,
                        onSubmit: function () {
                            return $('#editContainerForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editContainer').window('close');
                                if (rVal.id)
                                    currentId = rVal.id;
                                Query();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
        function Delete() {
            if ($('#deleteStartSn').validatebox('isValid') && $('#deleteEndSn').validatebox('isValid')) {
                var startSn = $('#deleteStartSn').val();
                var endSn = $('#deleteEndSn').val();
                $.messager.confirm('确认', '确认删除编号段【<font color="red">' + startSn + '</font>-<font color="red">' + endSn + '</font>】的容器？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerDelete/';
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data: { "startSn": startSn, "endSn": endSn },
                            success: function (r) {
                                if (r.result) {
                                    $('#deleteContainer').window('close');
                                    Query();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
    </script>
</asp:Content>