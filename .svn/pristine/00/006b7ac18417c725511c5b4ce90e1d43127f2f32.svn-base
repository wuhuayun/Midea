<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	汇总物料编码
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:false;">
        <table id="dgMaterialSum" title="汇总物料编码" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/SumGetMaterialQuantityList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){

                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                     
                          }">
                        <thead>
                            <tr>
                                <th data-options="field:'id',hidden:true">
                                </th>
                                <th data-options="field:'docNumber',width:100,align:'center'">
                                    组件
                                </th>
                                <th data-options="field:'description',width:250,align:'left'">
                                    组件说明
                                </th>
                                <th data-options="field:'wipSupplyTypeCn',width:60,align:'center'">
                                    供应类型
                                </th>
                                <th data-options="field:'requiredQuantity',width:60,align:'center'">
                                    需求数量
                                </th>
                                <th data-options="field:'quantityIssued',width:60,align:'center'">
                                    发料数量
                                </th>
                                <th data-options="field:'getMaterialQuantity',width:60,align:'center'">
                                    领料数量
                                </th>
                                <th data-options="field:'quantityDifference',width:80,align:'center'">
                                    发料差异数量
                                </th>
                                <th data-options="field:'quantityGetMaterialDifference',width:80,align:'center'">
                                    领料差异数量
                                </th>
                            </tr>
                        </thead>
                    </table>
        <div id="tb1">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="MaterialSumReload();">刷新</a>
            <span>需求时间：</span><input class="easyui-datebox" id="queryStartDate" type="text" style="width:100px;" />-
            <input class="easyui-datebox" id="queryEndDate" type="text" style="width:100px;" />
            <span>物料编码：</span>
            <input id="queryDocnumber" type="text" style="width: 100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            //初始化默认日期为当天
            var date = new Date();
            var yyyy = date.getFullYear();
            var MM = date.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            var dd = date.getDate() - 1;
            dd = dd < 10 ? "0" + dd : dd;
            var DD = date.getDate();
            DD = DD < 10 ? "0" + DD : DD;
            $("#queryStartDate").datebox("setValue", yyyy + "-" + MM + "-" + dd);
            $("#queryEndDate").datebox("setValue", yyyy + "-" + MM + "-" + DD);
        });
        function Query() {
            var options = $('#dgMaterialSum').datagrid('options');
            options.queryParams.queryStartDate = $('#queryStartDate').datebox('getText');
            options.queryParams.queryEndDate = $('#queryEndDate').datebox('getText');
            options.queryParams.queryDocnumber = $('#queryDocnumber').val();
            $('#dgMaterialSum').datagrid('reload');
        }
        function MaterialSumReload() {
            $('#dgMaterialSum').datagrid('reload');
        }
    </script>
</asp:Content>
