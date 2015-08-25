<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	容器规格管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgContainerSpec" title="容器规格管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerSpecList',
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
            <thead>
                <tr>
                    <th data-options="field:'id',hidden:true"></th>
                    <th data-options="field:'spec',width:100,align:'center'">规格</th>
                    <th data-options="field:'description',width:120,align:'center'">描述</th>
                    <th data-options="field:'createUser',width:80,align:'center'">创建人</th>
                    <th data-options="field:'_createTime',width:120,align:'center'">创建时间</th>
                    <th data-options="field:'modifyUser',width:120,align:'center'">最后更新人</th>
                    <th data-options="field:'_modifyTime',width:120,align:'center'">最后更新时间</th>
                </tr>
            </thead>
		</table>
        <div id="tb1">
            <input id="search" type="text" style="width: 100px;" />
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
		<div id="editContainerSpec" class="easyui-window" title="容器规格" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editContainerSpecForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>规格：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" required="true" id="spec" name="spec" type="text" style="width:300px;" /><span style="color:Red;">*</span>
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
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editContainerSpec').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        var currentId = null;
        function Query() {
            var options = $('#dgContainerSpec').datagrid('options');
            options.queryParams.queryWord = $('#search').val();
            $('#dgContainerSpec').datagrid('reload');
        }
        function Add() {
            $('#editContainerSpec').window('open');
            $("#editContainerSpecForm")[0].reset();

            $('#spec').focus();
            currentId = null;
        }
        function Edit() {
            var selectRow = $('#dgContainerSpec').datagrid('getSelected');
            if (selectRow) {
                $('#editContainerSpec').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerSpecEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editContainerSpecForm")[0].reset();
                            $('#spec').val(r.spec);
                            $('#description').val(r.description);

                            $('#spec').focus();
                        }
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择容器规格', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editContainerSpecForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerSpecSave/' + currentId,
                        onSubmit: function () {
                            return $('#editContainerSpecForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editContainerSpec').window('close');
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
            var selectRow = $('#dgContainerSpec').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除容器规格[<font color="red">' + selectRow.spec + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/ContainerSpecDelete/' + selectRow.id;
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
                $.messager.alert('提示', '请选择要删除的容器规格', 'info');
            }
        }
    </script>
</asp:Content>
