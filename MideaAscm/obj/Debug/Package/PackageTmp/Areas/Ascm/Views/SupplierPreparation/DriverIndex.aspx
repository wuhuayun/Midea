<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	司机管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgDriver" title="司机管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          <%--checkOnSelect: false,
                          selectOnCheck: true,--%>
                          idField: 'id',
                          sortName: 'sn',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据......',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DriverList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
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
                    <th data-options="field:'id',hidden:true"></th>
                    <th data-options="field:'sn',width:100,align:'center'">司机编号</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'name',width:80,align:'center'">姓名</th>
                    <th data-options="field:'sex',width:60,align:'center'">性别</th>
                    <th data-options="field:'mobileTel',width:100,align:'center'">移动电话</th>
                    <th data-options="field:'rfid',width:100,align:'center'">RFID</th>
                    <th data-options="field:'supplierName',width:180,align:'left'">供应商名称</th>
                    <th data-options="field:'typeCn',width:80,align:'center'">类型</th>
                    <th data-options="field:'plateNumber',width:80,align:'center'">车牌号</th>
                    <th data-options="field:'load',width:60,align:'center'">载重</th>
                    <th data-options="field:'description',width:160,align:'left'">描述</th>
                    <th data-options="field:'createUser',width:80,align:'center'">创建人</th>
                    <th data-options="field:'_createTime',width:120,align:'center'">创建时间</th>
                    <th data-options="field:'modifyUser',width:100,align:'center'">最后更新人</th>
                    <th data-options="field:'_modifyTime',width:120,align:'center'">最后更新时间</th>
                </tr>
            </thead>
		</table>
        <div id="tb1">
            <span>供应商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx"); %>
            <input id="search" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Add();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
            <%} %>
        </div>
		<div id="editDriver" class="easyui-window" title="司机修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editDriverForm" method="post" style="">
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
							        <span>姓名：</span>
						        </td>
						        <td style="width:80%">
							        <input id="name" name="name" type="text" style="width:120px;" />
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>性别：</span>
						        </td>
						        <td style="width:80%">
							        <select id="sex" name="sex" style="width:100px;">
							            <option value="男">男</option>
							            <option value="女">女</option>
						            </select>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>手机号：</span>
						        </td>
						        <td style="width:80%">
							        <input id="mobileTel" name="mobileTel" type="text" style="width:120px;" />
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>RFID编号：</span>
						        </td>
						        <td style="width:80%">
			                        <select id="rfid1" name="rfid1" style="width:80px;">
                                        <% List<string> listPre24G = MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList(); %>
                                        <% if (listPre24G != null && listPre24G.Count > 0)
                                            { %>
                                        <% foreach (string pre24G in listPre24G)
                                            { %>
                                        <option value="<%=pre24G %>"><%=pre24G%></option>
                                        <% } %>
                                        <% } %>
                                    </select>
						            <input class="easyui-validatebox" id="rfid2" name="rfid2" type="text" style="width:120px;"/>
                                    <input class="easyui-validatebox" id="rfid" name="rfid" type="text" style="display:none;"/><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>车牌号：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="plateNumber" name="plateNumber" type="text" style="width:120px;" />
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>载重：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-numberbox" id="load" name="load" type="text" style="width:120px;text-align:right;" data-options="min:0" />(吨)
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>类型：</span>
						        </td>
						        <td style="width:80%">
							        <select id="type" name="type" style="width:80px;">
                                        <% List<string> listTypeDefine = MideaAscm.Dal.SupplierPreparation.Entities.AscmDriver.TypeDefine.GetList(); %>
                                        <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                                            { %>
                                        <% foreach (string typeDefine in listTypeDefine)
                                            { %>
                                        <option value="<%=typeDefine %>"><%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDriver.TypeDefine.DisplayText(typeDefine)%></option>
                                        <% } %>
                                        <% } %>
                                    </select>
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
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editDriver').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        function Query() {
            var options = $('#dgDriver').datagrid('options');
            options.queryParams.supplierId = $('#supplierSelect').combogrid('getValue');
            options.queryParams.queryWord = $('#search').val();
            $('#dgDriver').datagrid('reload');
        }
        var currentId = null;
        function Add() {
            $('#spanSupplier').show();
            $('#supplierName').hide();
            //$('#sn').css('background-color', '');
            //$('#sn').removeAttr('readonly');
            $('#editDriver').window('open');
            $("#editDriverForm")[0].reset();
            $('#rfid1').val("<%=MideaAscm.Dal.Base.Entities.AscmRfid.Pre24G.GetList()[0] %>");
            currentId = "";
        }
        function Edit() {
            var selectRow = $('#dgDriver').datagrid('getSelected');
            if (selectRow) {
                $('#editDriver').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DriverEdit/' + currentId;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editDriverForm")[0].reset();
                            $('#name').val(r.name);
                            $('#spanSupplier').hide();
                            $('#supplierName').show();
                            $('#supplierName').val(r.supplierName);
                            $('#rfid').val(r.rfid);
                            $('#sex').val(r.sex);
                            $('#mobileTel').val(r.mobileTel);
                            $('#plateNumber').val(r.plateNumber);
                            $('#load').numberbox('setValue', r.load);
                            $('#type').val(r.type);
                            $('#description').val(r.description);

                            $('#rfid1').val(r.rfid1);
                            $('#rfid2').val(r.rfid2);
                            //$('#sn').css('background-color', '#E8E8E8');
                            //$('#sn').attr('readonly', true);
                        }
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择司机', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editDriverForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DriverSave/' + currentId,
                        onSubmit: function () {
                            $('#rfid').val($('#rfid1').val() + $('#rfid2').val());
                            return $('#editDriverForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editDriver').window('close');
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
            var selectRow = $('#dgDriver').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除司机[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DriverDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    Query();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的司机', 'info');
            }
        }
    </script>
</asp:Content>
