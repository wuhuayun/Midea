<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	容器查询列表
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dgContainer" title="容器查询列表" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: false,
                          singleSelect : true,
                          checkOnSelect: false,
                          selectOnCheck: false,
                          idField: 'sn',
                          sortName: 'sn',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据......',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerOuntList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.sn;
                          },
                          onLoadSuccess: function(data){
                              $(this).datagrid('clearChecked');
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }
                          }">
              <thead>
                <tr>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'sn',width:100,align:'center'">容器编号</th>
                    <th data-options="field:'rfid',width:180,align:'center'">EPC</th>
                    <th data-options="field:'spec',width:80,align:'center'">规格</th>
                    <th data-options="field:'supplierName',width:200,align:'left'">供应商名称</th>
                    <th data-options="field:'ReadingHeadAddress',width:80,align:'center'">位置</th>
                    <%--<th data-options="field:'description',width:80,align:'left'">描述</th>--%>
                    <th data-options="field:'statusCn',width:80,align:'center'">状态</th>
                    <th data-options="field:'_modifyTime',width:120,align:'center'">最后更新时间</th>
                </tr>
            </thead>
		</table>
        <div id="tb1" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>供应商：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
                 <span>规格：</span>
                <input id="cmb_spec" class="easyui-combobox" name="cmb_spec" style="width: 90px"  data-options="panelHeight:'400px',valueField:'id',textField:'spec',url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/RfidSpeac'"/>
                <span>容器编号：</span>
                <input id="querySn" name="querySn" type="text" style="width:100px;text-align:right;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
                <% if (ynWebRight.rightAdd){ %>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="Summit();">确认出库</a>
                <%} %>

                <input type="checkbox" name="print" id="print"> <span>打印单据</span>
            </div>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
<script type="text/javascript">
    function Query() {
        var options = $('#dgContainer').datagrid('options');
        options.queryParams.supplierId = $('#supplierSelect').combogrid('getValue');
        options.queryParams.status = $('#queryStatus').val();
        options.queryParams.querySn = $('#querySn').val();
        options.queryParams.spec = $('#cmb_spec').combobox('getValue');
        $('#dgContainer').datagrid('reload');
    }
    var currentId = "";
    var EditOrAdd = "";
    var docNumber = "";
    function Summit() {
        var rows = $('#dgContainer').datagrid('getChecked');
        if (jQuery.isEmptyObject(rows)) {
            $.messager.alert('错误', '没有数据', 'error');
            return;
        }
        var jsonStr = $.toJSON(rows);
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/StoreOut/';
        $.ajax({
            url: sUrl,
            type: "post",
            dataType: "json",
            data: { data: jsonStr ,
                PdocNumber: docNumber
            },
            success: function (r) {
                if (r.result) {
                    docNumber = r.message;
                    $.messager.alert('确认', '出库成功！', 'info');
                    $('#dgContainer').datagrid('reload');
                    if ($("#print")[0].checked)
                        Print(r.message);
                } else {
                    $.messager.alert('确认', '出库失败:' + r.message, 'error');
                }
            }
        });
    }

    function Print(docNumber) {
        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/ContainerHistoryPrint.aspx?docNumber=' + docNumber;
        //            url += "&direction=" + $('#queryDirection').val();
        //            url += "&startTime=" + $("#queryStartTime").datebox('getText');
        //            url += "&endTime=" + $("#queryEndTime").datebox('getText');
        parent.openTab('容器出库单据打印' + docNumber.toString(), url);
    }
    </script>
</asp:Content>
