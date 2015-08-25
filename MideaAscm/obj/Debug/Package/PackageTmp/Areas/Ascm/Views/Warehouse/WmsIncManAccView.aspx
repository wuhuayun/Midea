<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsIncManAccMain>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	手工单明细
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:180px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
				    <table style="width:720px;" border="0" cellpadding="0" cellspacing="0">
					    <tr style="height:24px">
						    <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>送货单号：</span>
						    </td>
						    <td style="width:150px">
                                <input type="text" name="docNumber" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.docNumber%>"/>
						    </td>
                            <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>生成时间：</span>
						    </td>
						    <td style="width:150px">
							    <input type="text" name="createTime" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.createTimeShow%>"/>
						    </td>
                            <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>责任人：</span>
						    </td>
						    <td style="width:150px">
							    <input type="text" name="responsiblePerson" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.responsiblePerson%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>供方编码：</span>
						    </td>
						    <td>
							    <input type="text" id="supplierDocNumber" name="supplierDocNumber" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.supplierDocNumber%>"/>
							    <input id="supplierId" name="supplierId" type="hidden" value="<%=Model.supplierId%>"/>
						    </td>
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>供方名称：</span>
						    </td>
						    <td colspan="3">
							    <input type="text" id="supplierName" name="supplierName" style="width:385px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.supplierName%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>收货仓库：</span>
						    </td>
						    <td>
                                <input id="warehouseId" name="warehouseId" type="text" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.warehouseId%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>供应子库：</span>
						    </td>
						    <td>
                                <input type="text" name="supperWarehouse" style="width:140px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.supperWarehouse%>"/>
						    </td>
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>运输车牌：</span>
						    </td>
						    <td>
							    <input type="text" name="supperPlateNumber" style="width:140px;background-color:#F0F0F0;" value="<%=Model.supperPlateNumber%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>联系电话：</span>
						    </td>
						    <td>
							    <input type="text" name="supperTelephone" style="width:140px;background-color:#F0F0F0;" value="<%=Model.supperTelephone%>"/>
						    </td>
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>备注：</span>
						    </td>
						    <td colspan="3">
							    <input type="text" name="memo" style="width:385px;background-color:#F0F0F0;" value="<%=Model.memo%>"/>
						    </td>
					    </tr>
				    </table>
                </div>
            </div>
        </div>
    </div>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridWmsIncManAccDetialList" title="手工单明细" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true">
		</table>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dataGridWmsIncManAccDetialList').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsIncManAccDetialList/<%=Model.id%>',
                rownumbers: true,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'releaseLineId', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'materialName', title: '物料描述', width: 300, align: 'left' },
                    { field: 'warelocationdocNumber', title: '货位', width: 110, align: 'center' },
                    { field: 'materialUnit', title: '单位', width: 40, align: 'center' },
                    { field: 'receivedQuantity', title: '实收数量', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function Print() { 
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/WmsIncManAccDetailPrint.aspx';
            url+="?mainId="+<%=Model.id%>;
            parent.openTab('手工单明细日志打印', url);
        }
    </script>
</asp:Content>

