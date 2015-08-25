<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库物料查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--物料列表-->
    <div region="west" split="false" border="false" title="" style="width:500px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
                    <table id="dgMaterial" title="仓库物料查询" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GetSumMaterialFromErpList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
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
                                <th data-options="field:'id',hidden:true">
                                <th data-options="field:'ascmMateiralItem_Docnumber',width:110,align:'center'">
                                    编码
                                </th>
                                <th data-options="field:'subinventoryCode',width:110,align:'center'">
                                    仓库
                                </th>
                                <th data-options="field:'transactionQuantity',width:70,align:'center'">
                                    库存量
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'ascmMaterialItem_Unit',width:80,align:'center'">
                                    单位
                                </th>
                                <th data-options="field:'ascmMaterialItem_Description',width:220,align:'center'">
                                    描述
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb1" style="padding: 5px; height: auto;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                            onclick="MaterialReload();">刷新</a> <span>物料编码：</span>
                        <%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewMaterialDocnumberSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWord", width = "150px" }); %>
                        
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Query();">查询</a>
                    </div>
                </div>
			</div>
		</div>
	</div>
    <!--作业列表-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
                    <table id="dgDiscreteJobs" title="关联作业" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb2',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          <%--url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GetMtlRelatedWipDiscreteJobs',--%>
                          onSelect: function(rowIndex, rowRec){
                              tempId = rowRec.id;
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (tempId) {
                                  $(this).datagrid('selectRecord', tempId);
                              }                  
                          }">
                        <thead data-options="frozen:true">
                            <tr>
                                <%--<th data-options="checkbox:true"></th>--%>
                                <th data-options="field:'id',hidden:true">
                                <th data-options="field:'jobId',width:120,align:'center'">
                                    作业号
                                </th>
                                <th data-options="field:'jobDate',width:90,align:'center'">
                                    作业日期
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'jobInfoId',width:110,align:'center'">
                                    装配件
                                </th>
                                <th data-options="field:'jobDesc',width:250,align:'center'">
                                    装配件描述
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DiscreteJobReload();">刷新</a>
                        <span>物料编码：</span>
                        <input id="queryDocnumber" type="text" style="width: 100px; background-color:#E8E8E8;" readonly="readonly" />
                        <span>作业日期：</span>
                        <input class="easyui-datebox" id="queryStartTime" type="text" style="width: 100px;" />-
                        <input class="easyui-datebox" id="queryEndTime" type="text" style="width: 100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Search();">查询</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        var currentId = null;
        function Query() {
            var options = $('#dgMaterial').datagrid('options').queryParams;
            options.materialId = $('#queryWord').combogrid('getValue');

            $('#queryDocnumber').val($('#queryWord').combogrid('getText'));

            $('#dgMaterial').datagrid('reload');
        }
    </script>
    <script type="text/javascript">
        var tempId = null;
        function Search() {
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/GetMtlRelatedWipDiscreteJobs/';
            var options = $('#dgDiscreteJobs').datagrid('options');
            options.url = sUrl;
            options.queryParams.queryStartTime = $("#queryStartTime").datebox('getText');
            options.queryParams.queryEndTime = $("#queryEndTime").datebox('getText');
            options.queryParams.queryDocnumber = $("#queryDocnumber").val();

            $('#dgDiscreteJobs').datagrid('reload');
        }
    </script>
</asp:Content>
