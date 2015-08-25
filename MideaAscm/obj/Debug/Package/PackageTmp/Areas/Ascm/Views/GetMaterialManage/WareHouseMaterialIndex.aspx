<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库物料查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
   <!--物料列表-->
    <div region="west" split="false" border="false" title="" style="width:600px;padding:0px 2px 0px 0px;overflow:auto;">
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
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/WareHouseMaterialList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                              DiscreteJobReload();
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }
                              DiscreteJobReload();                     
                          }">
                        <thead data-options="frozen:true">
                            <tr>
                                <%--<th data-options="checkbox:true"></th>--%>
                                <th data-options="field:'id',width:80,align:'center'">
                                    ID
                                </th>
                                <th data-options="field:'docNumber',width:110,align:'center'">
                                    编码
                                </th>
                                <th data-options="field:'warehouseName',width:110,align:'center'">
                                    仓库
                                </th>
                                <th data-options="field:'totalNumber',width:70,align:'center'">
                                    库存量
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'unit',width:80,align:'center'">
                                    单位
                                </th>
                                <th data-options="field:'description',width:220,align:'center'">
                                    描述
                                </th>
                                <th data-options="field:'memo',width:80,align:'center'">
                                    备注
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb1" style="padding: 5px; height: auto;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                            onclick="MaterialReload();">刷新</a> <span>物料编码：</span>
                        <%--<input id="queryWord" type="text" style="width: 100px;" />--%>
                        <%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/MaterialDocNumberSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "queryWord" }); %>
                        <span>需求时间：</span>
                        <input class="easyui-datebox" id="queryStartTime" type="text" style="width: 100px;" />-
                        <input class="easyui-datebox" id="queryEndTime" type="text" style="width: 100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Query();">查询</a>
                    </div>
                </div>
			</div>
		</div>
	</div>
    <!--任务列表-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
                    <table id="dgDiscreteJob" title="作业清单" style="" border="false" fit="true" singleselect="true"
                        idfield="wipEntityId" treefield="wipEntityId" animate="true" sortname="sortNo" sortorder="asc"
                        toolbar="#tb2" pagination="true" pageSize="30">
                        <thead>
                            <tr>
                                <th field="wipEntityId" width="20" align="center" hidden="true">
                                    ID
                                </th>
                                <th field="ascmWipEntities_Name" width="200" align="left">
                                    作业号
                                </th>
                                <th field="bomRevisionDate" width="200" align="center" sortable="true">
                                    需求时间
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="DiscreteJobReload();">刷新</a>
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
        var queryParams = $('#dgMaterial').datagrid('options').queryParams;
        queryParams.queryWord = $('#queryWord').combogrid('getValue');
        queryParams.startTime = $("#queryStartTime").datebox('getText');
        queryParams.endTime = $("#queryEndTime").datebox('getText');
        $('#dgMaterial').datagrid('reload');
    }
    $(function () {
        var date = new Date();
        var yyyy = date.getFullYear();
        var MM = date.getMonth() + 1;
        MM = MM < 10 ? "0" + MM : MM;
        var dd = date.getDate();
        dd = dd < 10 ? "0" + dd : dd;
        $("#queryEndTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        var DD = date.getDate() - 1;
        DD = DD < 10 ? "0" + DD : DD;
        var mm = date.getMonth();
        mm = mm < 10 ? "0" + mm : mm;
        $("#queryStartTime").datebox("setValue", yyyy + "-" + mm + "-" + dd);
    });
    function MaterialReload() {
        $('#dgMaterial').datagrid('reload');
    }
    
</script>


<script type="text/javascript">
    $(function () {
        $("#dgDiscreteJob").treegrid({
            onLoadSuccess: function () {
                $(this).treegrid('unselectAll');
            },
            onDblClickRow: function (rowIndex, rowData) {

            },
            onClickRow: function (row) {

            }
        })
    });
    function DiscreteJobReload() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/MaterialOfDiscreteJobList';
        $.ajax({
            url: sUrl,
            type: "post",
            data: { materialId: currentId,
                    startTime: $("#queryStartTime").datebox('getText'),
                    endTime: $("#queryEndTime").datebox('getText')
                  },
            dataType: "json",
            success: function (result) {
                $('#dgDiscreteJob').treegrid('loadData', result);
            }
        });
    }
</script>
</asp:Content>
