<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.FromErp.Entities.AscmCuxWipReleaseHeaders>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业领料单
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:180px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        作业领料单
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
				    <table style="width:690px;" border="0" cellpadding="0" cellspacing="0">
					    <tr style="height:24px">
						    <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>作业号：</span>
						    </td>
						    <td style="width:150px">
                                <input type="text" name="wipEntitiesName" style="width:140px;" readonly="readonly" value="<%=Model.wipEntitiesName%>"/>
						    </td>
                            <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>条码号：</span>
						    </td>
						    <td style="width:150px">
							    <input type="text" name="releaseNumber" style="width:140px;" readonly="readonly" value="<%=Model.releaseNumber%>"/>
						    </td>
                            <td style="width:80px; text-align:right;" nowrap="nowrap">
							    <span>计划时间：</span>
						    </td>
						    <td style="width:150px">
							    <input type="text" name="ascmWipDiscreteJobsScheduledStartDate" style="width:140px;" readonly="readonly" value="<%=Model.ascmWipDiscreteJobsScheduledStartDate%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>作业说明：</span>
						    </td>
						    <td colspan="5">
							    <input type="text" name="ascmWipDiscreteJobsDescription" style="width:600px;" readonly="readonly" value="<%=Model.ascmWipDiscreteJobsDescription%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>车间：</span>
						    </td>
						    <td>
							    <input type="text" name="ascmWipDiscreteJobsAscmWipScheduleGroupsName" style="width:140px;" readonly="readonly" value="<%=Model.ascmWipDiscreteJobsAscmWipScheduleGroupsName%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>生产线：</span>
						    </td>
						    <td>
                                <input type="text" name="ascmWipDiscreteJobsAscmWipScheduleGroupsName" style="width:140px;" readonly="readonly" value=""/>
						    </td>
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>供应类型：</span>
						    </td>
						    <td>
							    <input type="text" name="ascmWipDiscreteJobsWipSupplyType_Cn" style="width:140px;" readonly="readonly" value="<%=Model.ascmWipDiscreteJobsWipSupplyType_Cn%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>作业状态：</span>
						    </td>
						    <td>
							    <input type="text" name="ascmWipDiscreteJobsStatusType_Cn" style="width:140px;" readonly="readonly" value="<%=Model.ascmWipDiscreteJobsStatusType_Cn%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>供应子库：</span>
						    </td>
						    <td>
                                <input type="text" name="ascmWipDiscreteJobsAscmWipScheduleGroupsName" style="width:140px;" readonly="readonly" value=""/>
						    </td>
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>计划数量：</span>
						    </td>
						    <td>
							    <input type="text" name="ascmWipDiscreteJobsNetQuantity" style="width:140px;" readonly="readonly" value="<%=Model.ascmWipDiscreteJobsNetQuantity%>"/>
						    </td>
					    </tr>
					    <tr style="height:24px">
						    <td style="text-align:right;" nowrap="nowrap">
							    <span>装配件：</span>
						    </td>
						    <td colspan="5">
							    <input type="text" name="ascmWipDiscreteJobs_ascmMaterialItem_DocNumber" style="width:600px;" readonly="readonly" value="<%=Model.ascmWipDiscreteJobs_ascmMaterialItem_DocNumber%>  <%=Model.ascmWipDiscreteJobs_ascmMaterialItem_Description%>"/>
						    </td>
					    </tr>
				    </table>
                </div>
            </div>
        </div>
    </div>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridCuxWipReleaseLines" title="作业领料明细" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true">
		</table>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dataGridCuxWipReleaseLines').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CuxWipReleaseLinesList/<%=Model.releaseHeaderId%>',
                rownumbers: true,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'releaseLineId', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'materialName', title: '物料描述', width: 300, align: 'left' },
                    { field: 'materialUnit', title: '单位', width: 40, align: 'center' },
                    { field: 'subInventory', title: '供应子库', width: 115, align: 'center' },
                    { field: 'ascmWipRequirementOperations_wipSupplyTypeCn', title: '供应类型', width: 80, align: 'center' },
                    { field: 'printQuantity', title: '计划数', width: 80, align: 'center' },
                    { field: 'QUANTITY_AV', title: '未发数', width: 80, align: 'center' },
                    { field: 'transaction_quantity', title: '现有数', width: 80, align: 'center' },
                    { field: 'to_org_primary_quantity', title: '接受中', width: 80, align: 'center' }

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
    </script>
</asp:Content>
