<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	未完成作业统计
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: false;">
        <table id="dgNotFinalDiscreteJobs" title="未完成作业统计" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                    rownumbers: true,  
                    singleSelect: true,
                    idField: 'id',
                    striped: true,
                    toolbar: '#tb',
                    pagination: true,
                    pageSize: 50,
                    loadMsg: '更新数据...',
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/NotFinalDiscreteJobsList/',
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
                        <th data-options="field:'wipEntitiesId',hidden:true">
                        </th>
                        <th data-options="field:'jobId',width:120,align:'center'">
                            作业号
                        </th>
                        <th data-options="field:'sIdentificationId',width:50,align:'center'">
                            类型
                        </th>
                        <th data-options="field:'jobDate',width:80,align:'center'">
                            作业日期
                        </th>
                        <th data-options="field:'jobInfoId',width:90,align:'center'">
                            装配件
                        </th>
                        <th data-options="field:'jobDesc',width:270,align:'center'">
                            装配件描述
                        </th>
                        <th data-options="field:'count',width:60,align:'center'">
                            数量
                        </th>
                        <th data-options="field:'lineAndSequence',width:70,align:'center'">
                            生产线
                        </th>
                        <th data-options="field:'onlineTime',width:80,align:'center'">
                            上线时间
                        </th>
                        <th data-options="field:'rankerName',width:80,align:'center'">
                            所属排产员
                        </th>
                        <th data-options="field:'detail',width:50,align:'center',formatter:jobFormat">
                            详情
                        </th>
                    </tr>
                </thead>
            </table>
        <div id="tb">
            <span>需求时间：</span>
            <input id="queryStartDate" class="easyui-datebox" type="text" style="width: 100px" />-
            <input id="queryEndDate" class="easyui-datebox" type="text" style="width: 100px" />
            <span>作业号：</span>
            <input id="queryDocnumber" type="text" style="width: 100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="Search();">查询</a>
        </div>
        <div id="DivJobBom" class="easyui-window" title="作业Bom清单" style="padding: 5px;width:640px;height:480px;"
                        data-options="iconCls: 'icon-search',
                                      closed: true,
                                      maximizable: false,
                                      minimizable: false,
                                      resizable: false,
                                      collapsible: false,
                                      modal: true,
                                      shadow: true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
                    <table id="dgJobBom" class="easyui-datagrid" title="作业日志" style="" border="false" 
                        data-options="rownumbers: true,
                                        noheader: true,
                                        fit: true,
                                        singleSelect: true,
                                        idField: 'id',
                                        sortName: 'ascmMaterialItem_DocNumber',
                                        sortOrder: 'asc',
                                        striped: true,
                                        pagination: true,
                                        pageSize: 30,
                                        toolbar: '#tb2',
                                        loadMsg: '加载数据...'">
                        <thead>
                            <tr>
                                <th data-options="field:'id',hidden:true">
                                </th>
                                <th data-options="field:'ascmMaterialItem_DocNumber',width:100,align:'center',sortable:true">
                                    组件
                                </th>
                                <th data-options="field:'mpsDateRequiredStr',width:80,align:'center',sortable:true">
                                    需求日期
                                </th>
                                <th data-options="field:'ascmMaterialItem_Description',width:200,align:'left'">
                                    组件说明
                                </th>
                                <th data-options="field:'quantityPerAssembly',width:60,align:'center'">
                                    每个装
                                </th>
                                <th data-options="field:'wipSupplyTypeCn',width:60,align:'center'">
                                    供应类型
                                </th>
                                <th data-options="field:'supplySubinventory',width:80,align:'center'">
                                    子库
                                </th>
                                <th data-options="field:'requiredQuantity',width:60,align:'center'">
                                    需求数量
                                </th>
                                <th data-options="field:'getMaterialQuantity',width:60,align:'center'">
                                    领料数量
                                </th>
                                <th data-options="field:'quantityGetMaterialDifference',width:60,align:'center'">
                                    差异数量
                                </th>
                            </tr>
                        </thead>
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="jobBomReload();">刷新</a>
                        <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#DivJobBom').window('close');">关闭</a> 
                    </div>
                </div>
            </div>
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
</script>
<script type="text/javascript">
    function jobFormat(value, row, index) {
        return "<a href='javascript:void(0)' onclick='loadJobDetail(\"" + row.wipEntitiesId + "\");'>详情</a>";
    }
    function loadJobDetail(id) {
        $('#DivJobBom').window('open');
        var options = $('#dgJobBom').datagrid('options');
        options.queryParams.id = id;
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LoadBom';
        $('#dgJobBom').datagrid('reload');
    }
    function jobBomReload() {
        $('#dgJobBom').datagrid('reload');
    }
</script>
<script type="text/javascript">
    function Search() {
        var options = $('#dgNotFinalDiscreteJobs').datagrid('options');
        options.queryParams.queryStartDate = $('#queryStartDate').datebox('getText');
        options.queryParams.queryEndDate = $('#queryEndDate').datebox('getText');
        options.queryParams.queryDocnumber = $('#queryDocnumber').val();
        $('#dgNotFinalDiscreteJobs').datagrid('reload');
    }
</script>
</asp:Content>
