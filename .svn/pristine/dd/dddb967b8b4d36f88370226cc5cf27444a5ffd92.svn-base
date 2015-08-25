<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库员物料管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgContainer" title="仓库员物料管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          idField: 'sn',
                          sortName: 'sn',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据......',
                          url: '',
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
                    <th data-options="field:'id',hidden:'true'">id</th>
                    <th data-options="field:'keeperName',width:120,align:'left'">仓管员</th>
                    <th data-options="field:'startCode',width:160,align:'center'">物料编码起</th>
                    <th data-options="field:'endCode',width:160,align:'center'">物料编码止</th>
                </tr>
            </thead>
		</table>
        <div id="tb1" style="padding:5px;height:auto;">
            <div style="">
                <span>仓管员：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
                <input id="search" name="search" type="text" style="width:120px;" />
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
		<div id="editContainer" class="easyui-window" title="修改" style="padding: 10px;width:540px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editContainerForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>仓管员：</span>
						        </td>
						        <td style="width:80%">
							        <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "supplierId" }); %><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>物料编码：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" id="startSn" name="startSn" required="true" type="text" style="width:100px;text-align:right;" />--
                                    <input class="easyui-validatebox" id="endSn" name="endSn" required="true" type="text" style="width:100px;text-align:right;" /><span style="color:Red;">*</span>
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
            $('#editContainer').window('open');
            $("#editContainerForm")[0].reset();
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

        }
    </script>
</asp:Content>
