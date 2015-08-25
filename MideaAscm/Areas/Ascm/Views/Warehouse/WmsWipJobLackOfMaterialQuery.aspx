<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	备料缺料查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:120px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
	                </div>
                </div>
                <div region="center" border="false" style="overflow:auto;">
                    <form id="editWmsPreparationMainForm" method="post" style="">
                        <table style="width:690px;" border="0" cellpadding="0" cellspacing="0">
                            <tr style="height:24px">
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
							        <span>作业号：</span>
						        </td>
                                <td style="width:140px">
                                    <input type="text" name="wipEntityName" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.ascmWipEntities_Name%>"/>
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
                                    <span>作业状态：</span>
						        </td>
                                <td style="width:140px">
                                    <input type="text" name="status" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.statusTypeCn%>"/>  
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
                                    <span>计划时间：</span>
						        </td>
                                <td style="width:140px">
                                    <input type="text" name="scheduledStartDate" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.scheduledStartDateCn%>"/>   
						        </td>
                                <td style="width:80px; text-align:right;" nowrap="nowrap">
                                    <span>计划数量：</span>
						        </td>
						        <td style="width:140px">
                                    <input type="text" name="netQuantity" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.netQuantity%>"/>  
						        </td> 
                            </tr>
                            <tr style="height:24px">
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>作业说明：</span>
						        </td>
                                <td colspan="3">
                                    <input type="text" name="description" style="width:330px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.ascmWipentities_Description%>"/>
						        </td>
                                <td style="text-align:right;" nowrap="nowrap">
							       <span>产线：</span> 
						        </td>
                                <td>
                                  <input type="text" name="productionLine" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.productionLine%>"/>     
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
                                   <span>车间：</span> 
						        </td>
                                <td>
                                   <input type="text" name="scheduleGroupName" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.ascmWipScheduleGroupsName%>"/>   
						        </td>
                            </tr>
                            <tr style="height:24px">
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>装配件编码：</span>
						        </td>
                                <td>
                                    <input type="text" name="primaryItemDocNumber" style="width:120px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.ascmMaterialItem_DocNumber%>"/> 
                                </td>
                                <td style="text-align:right;" nowrap="nowrap">
							        <span>装配件描述：</span>
						        </td>
						        <td colspan="5">
                                    <input type="text" name="primaryItemDescription" style="width:540px;background-color:#F0F0F0;" readonly="readonly" value="<%=Model.ascmMaterialItem_Description%>"/> 
						        </td>
                            </tr>
				        </table>
                    </form>
                </div>
            </div>
        </div>    
    </div>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dgWipRequirementOperations" title="作业物料清单" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          idField: 'materialId',
                          striped: true,
                          loadMsg: '更新数据......',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobLackOfMaterialList/<%=Model.wipEntityId%>'">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="field:'id',hidden:'true'"></th>
                    <th data-options="field:'ascmMaterialItem_DocNumber',width:100,align:'center'">物料编码</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'ascmMaterialItem_Description',width:320,align:'left'">物料描述</th>
                    <th data-options="field:'ascmMaterialItem_unit',width:60,align:'center'">物料单位</th>
                    <th data-options="field:'supplySubinventory',width:80,align:'center'">供应子库</th>
                    <th data-options="field:'dateRequired',width:110,align:'center'">需求日期</th>
                    <th data-options="field:'requiredQuantity',width:60,align:'center'">需求数量</th>
                    <th data-options="field:'ascmPreparedQuantity',width:60,align:'center'">已备数量</th>
                    <th data-options="field:'transactionQuantity',width:60,align:'center'">库存</th>
                </tr>
            </thead>
        </table>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    
</asp:Content>
