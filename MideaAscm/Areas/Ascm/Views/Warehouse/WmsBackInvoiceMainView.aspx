<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsBackInvoiceMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商退货明细
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
				    <table style="width:690px;" border="0" cellpadding="0" cellspacing="0">
					    <tr style="height:24px">
						    <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>单据号：</span>
						    </td>
						    <td style="width:150px">
                                <input type="text" name="docNumber" style="width:140px;" readonly="readonly" value="<%=Model.docNumber%>"/>
						    </td>
                            <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>手工单号：</span>
						    </td>
						    <td style="width:150px">
							    <input type="text" name="manualDocNumber" style="width:140px;" readonly="readonly" value="<%=Model.manualDocNumber%>"/>
						    </td>
                            <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>责任人：</span>
						    </td>
						    <td style="width:150px">
							    <input type="text" name="responsiblePerson" style="width:140px;" readonly="readonly" value="<%=Model.responsiblePerson%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>默认仓库：</span>
						    </td>
						    <td >
							    <input type="text" name="warehouseId" style="width:140px;" readonly="readonly" value="<%=Model.warehouseId%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>状态：</span>
						    </td>
						    <td >
							    <input type="text" name="statusCn" style="width:140px;" readonly="readonly" value="<%=Model.statusCn%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>退货原因：</span>
						    </td>
						    <td >
							    <input type="text" name="reasonName" style="width:140px;" readonly="readonly" value="<%=Model.reasonName%>"/>
						    </td>
					    </tr>
                        <tr style="height:24px">
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>供方编码：</span>
						    </td>
						    <td >
							    <input type="text" name="supplierDocNumber" style="width:140px;" readonly="readonly" value="<%=Model.supplierDocNumber%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>供方名称：</span>
						    </td>
						    <td >
							    <input type="text" name="supplierName" style="width:140px;" readonly="readonly" value="<%=Model.supplierName%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>备注：</span>
						    </td>
						    <td colspan="5">
							    <input type="text" name="memo" style="width:600px;" readonly="readonly" value="<%=Model.memo%> "/>
						    </td>
					    </tr>
				    </table>
                </div>
            </div>
        </div>
    </div>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridWmsBackInvoiceDetial" title="供应商退货明细" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true">
		</table>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dataGridWmsBackInvoiceDetial').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsBackInvoiceDetialList/<%=Model.id%>',
                rownumbers: true,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'materialDescription', title: '物料描述', width: 350, align: 'left' },
                    { field: 'materialUnit', title: '单位', width: 115, align: 'center' },
                    { field: 'deliveryQuantity', title: '送货数量', width: 115, align: 'center' },
                    { field: 'returnQuantity', title: '退货数量', width: 80, align: 'center' },
                    { field: 'warelocationDoc', title: '货位', width: 100, align: 'center' },
                    { field: 'docNumber', title: '批条码号', width: 100, align: 'center' }
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
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/WmsBackInvoiceDetailPrint.aspx';
            url+="?mainId="+<%=Model.id%>;
            parent.openTab('供应商退货明细日志打印', url);
        }
    </script>
</asp:Content>

