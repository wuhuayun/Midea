<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库发料校验
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <table id="dgStoreIssueCheck" title="发料校验" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          idField: 'id',
                          sortName: '',
                          sortOrder: '',
                          pagination: true,
                          pageSize: 50,
                          striped: true,
                          toolbar: '#tb',
                          loadMsg: '数据加载中，请稍候...',
                          onSelect: function (rowIndex, rowData) {
                              
                          },
                          onLoadSuccess: function(){
                                                 
                          }">
            <thead>
                <tr>
                    <th data-options="field:'id',hidden:'true'"></th>
                    <th data-options="field:'issueDate',width:110,align:'left'">发料时间</th>
                    <th data-options="field:'pickQuantity',width:90,align:'left'">领料容器数量</th>
                    <th data-options="field:'readQuantity',width:230,align:'left'">读取容器数量</th>
                    <th data-options="field:'checkedResult',width:70,align:'center'">校验合格</th>
                </tr>
            </thead>
        </table>            
        <div id="tb">
            <span>发料时间：</span>
            <input class="easyui-datebox" id="startIssueDate" type="text" options="validType:'checkDate'" style="width:100px;" value="<%=DateTime.Now.ToString("yyyy-MM-dd")%>" />-<input class="easyui-datebox" id="endIssueDate" type="text" options="validType:'checkDate'" style="width:100px;" value="<%=DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") %>" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $.extend($.fn.datebox.defaults.rules, {
            checkDate: {
                validator: function (value, param) {
                    var t = Date.parse(value);
                    return !isNaN(t);
                },
                message: '日期输入错误.'
            }
        });
        function Query() {
            if (!$('#startIssueDate').datebox('isValid') || !$('#endIssueDate').datebox('isValid')) {
                return;
            }
            var options = $('#dgStoreIssueCheck').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStoreIssueCheckList';
            options.queryParams.startIssueDate = $('#startIssueDate').datebox('getText');
            options.queryParams.endIssueDate = $('#endIssueDate').datebox('getText');
            $('#dgStoreIssueCheck').datagrid('reload');
        }
    </script>
</asp:Content>
