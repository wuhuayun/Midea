<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsMtlRequisitionMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库领料单
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:120px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <% if (ynWebRight.rightEdit){ %>
			            <%--<a href="javascript:void(0);" id="requisitionOk" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="RequisitionOk();">领料确认</a>--%>
                        <%} %>
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
                    <form id="editWmsPreparationMainForm" method="post" style="">
                        <table style="width:690px;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>单据号：</span>
						        </td>
						        <td style="width:180px">
                                    <input type="text" name="docNumber" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.docNumber%>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>手工单号：</span>
						        </td>
						        <td style="width:180px">
							        <input type="text" name="manualDocNumber" style="width:140px;background-color:#F0F0F0;" value="<%=Model.manualDocNumber%>"/>
						        </td>
					        </tr>
					        <%--<tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>仓库：</span>
						        </td>
						        <td>
							        <input id="warehouseId" name="warehouseId" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.warehouseId%>"/>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>状态：</span>
						        </td>
						        <td>
                                    <input id="statusCn" name="statusCn" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.statusCn%>"/>
                                    <input id="status" name="status" type="hidden" value="<%=Model.status%>" />
						        </td>
					        </tr>--%>
                            <tr style="height:24px">
						        <td style="text-align:right;" nowrap="nowrap">
							        <span>描述：</span>
						        </td>
						        <td colspan="3">
							        <input type="text" id="description" name="description" style="width:490px;background-color:#F0F0F0;" value="<%=Model.description %>"/>
						        </td>
					        </tr>
				        </table>
                    </form>
                </div>
            </div>
        </div>    
    </div>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dgDetailSum" title="领料单明细" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          idField: 'materialId',
                          striped: true,
                          loadMsg: '更新数据...... ',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsRequisitionDetailView/<%=Model.id%>'">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="field:'materialId',hidden:true"></th>
                    <th data-options="field:'wipEntityName',width:110,align:'center'">作业名称</th>
                    <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th>
                    <th data-options="field:'materialName',width:360,align:'left'">物料描述</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                    <th data-options="field:'warehouseId',width:80,align:'center'">供应子库</th>
                    <th data-options="field:'warelocationdocNumber',width:80,align:'center'">货位</th>
                    <th data-options="field:'quantity',width:80,align:'center'">实领数量</th>
                </tr>
            </thead>
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $(function () {
            SetButtons();
        })
        function SetButtons() {
            var status = $('#status').val();
            $('#requisitionOk').hide();
        }
        function RequisitionOk() {
            $.messager.confirm('确认', '确认提交领料单？', function (result) {
                if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsRequisitionOk/<%=Model.id%>',
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                $('#status').val(r.entity.status);
                                $('#statusCn').val(r.entity.statusCn);
                                SetButtons();
                                $.messager.alert('提示', '提交完成!', 'info');
                            } else {
                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            })
        }
    </script>
</asp:Content>
