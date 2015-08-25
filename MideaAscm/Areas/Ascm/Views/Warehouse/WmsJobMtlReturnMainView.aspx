<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsMtlReturnMain>"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业退料明细
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
							    <span>作业号：</span>
						    </td>
						    <td style="width:150px">
							    <input type="text" name="manualDocNumber" style="width:140px;" readonly="readonly" value="<%=Model.wipEntityName%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>仓库：</span>
						    </td>
						    <td >
							    <input type="text" name="warehouseId" style="width:140px;" readonly="readonly" value="<%=Model.warehouseId%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>退料区域：</span>
						    </td>
						    <td >
							    <input type="text" name="statusCn" style="width:140px;" readonly="readonly" value="<%=Model.returnAreaCn%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>退料原因：</span>
						    </td>
						    <td >
							    <input type="text" name="reasonName" style="width:140px;" readonly="readonly" value="<%=Model.reasonName%>"/>
						    </td>
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>备注：</span>
						    </td>
						    <td>
							    <input type="text" name="memo" style="width:140px;" readonly="readonly" value="<%=Model.memo%> "/>
						    </td>
					    </tr>
				    </table>
                </div>
            </div>
        </div>
    </div>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridAscmWmsMtlReturnDetial" title="作业退料明细" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true">
		</table>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dataGridAscmWmsMtlReturnDetial').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsJobMtlReturnDetialList/<%=Model.id%>',
                rownumbers: true,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'materialDescription', title: '物料描述', width: 350, align: 'left' },
                    { field: 'materialUnit', title: '单位', width: 50, align: 'center' },
                    { field: 'locationDocNumber', title: '货位编码', width: 115, align: 'center' },
                    { field: 'quantity', title: '退料数量', width: 80, align: 'center' },
                    { field: 'warehouseId', title: '供应子库', width: 100, align: 'center' },
                    { field: 'requiredQuantity', title: '需求数量', width: 100, align: 'center' },
                    { field: 'quantityIssued', title: '发料数量', width: 100, align: 'center' },
                    { field: 'quantityDifference', title: '差异数量', width: 100, align: 'center' }
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
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/WmsJobMtlReturnDetialPrint.aspx';
            url+="?mainId="+<%=Model.id%>;
            parent.openTab('作业退料明细日志打印', url);
        }
    </script>
</asp:Content>
