<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<MideaAscm.Dal.Warehouse.Entities.AscmWmsStockTransMain>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	子库存转移单
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="false" style="height:180px; padding:0px 0px 2px 0px; overflow:hidden;">
        <div class="easyui-panel" title="" fit="true" border="true" style="overflow:hidden;">
            <div class="easyui-layout" fit="true">
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:1px solid #99BBE8; padding:1px;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
                        <% if (Model.status != "draft" && Model.status != "confirm" && Model.createUser == Page.User.Identity.Name) 
                           { %>
                        <a href="javascript:void(0);" id="btnUndo" class="easyui-linkbutton" plain="true" icon="icon-undo" onclick="Undo();">撤回</a>
                        <%} %>
                        <% if (Model.status == "pending" && Model.toWarehouseUser == Page.User.Identity.Name)
                           { %>
                        <a href="javascript:void(0);" id="btnReview" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="Review();">审核</a>
                        <%} %>
                         <% if (Model.status == "review" && Model.toWarehouseUser == Page.User.Identity.Name)
                           { %>
                        <a href="javascript:void(0);" id="btnCheck" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="Check();">确认</a>
                        <%} %>
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
							    <span>来源仓库：</span>
						    </td>
						    <td >
							    <input type="text" name="fromWarehouseId" style="width:140px;" readonly="readonly" value="<%=Model.fromWarehouseId%>"/>
						    </td>
                            <td style="text-align:right;" nowrap="nowrap">
							    <span>目标仓库：</span>
						    </td>
						    <td >
							    <input type="text" name="toWarehouseId" style="width:140px;" readonly="readonly" value="<%=Model.toWarehouseId%>"/>
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
		<table id="dataGridWmsStockTransDetial" title="子库存转移单明细" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true">
		</table>
	</div>
    <!--进度条-->           
        <div id="wProgressbar" class="easyui-window" title="完成确认......" style="padding: 10px;width:440px;height:80px;"
            data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
            <div id="p" class="easyui-progressbar" style="width:400px;"></div>
        </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#dataGridWmsStockTransDetial').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransDetialList/<%=Model.id%>',
                rownumbers: true,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'materialDescription', title: '物料描述', width: 350, align: 'left' },
                    { field: 'fromWarelocationDocNumber', title: '来源货位', width: 115, align: 'center' },
                    { field: 'toWarelocationDocNumber', title: '目标货位', width: 115, align: 'center' },
                    { field: 'materialUnit', title: '单位', width: 80, align: 'center' },
                    { field: 'fromQuantity', title: '来源货位物料数量', width: 100, align: 'center' },
                    { field: 'quantity', title: '转移数量', width: 100, align: 'center' }
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
        var statusOption = "";
        function Review(){
            statusOption = "review";//状态改为已审核
            $.messager.confirm("确认", "是否审核？", function (result) {
            if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransUpdate',
                        type: "post",
                        dataType: "json",
                        data: { "id": '<%=Model.id%>', "statusOption": statusOption },
                        success: function (r) {
                            if (r.result) {
                                $.messager.alert('提示', '审核成功', 'info');
                                $('#btnReview').hide();//隐藏按钮
                            }else {
                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            }); 
        }
        function Undo(){
            statusOption = "draft";//状态改为草稿          
            $.messager.confirm("确认", "是否撤回？", function (result) {
            if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransUpdate',
                        type: "post",
                        dataType: "json",
                        data: { "id": '<%=Model.id%>', "statusOption": statusOption },
                        success: function (r) {
                            if (r.result) {
                                $.messager.alert('错误', '撤回成功', 'info');
                                 $('#btnUndo').hide();//隐藏按钮
                            }else {
                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            }); 
        }
        function Check(){
            $.messager.confirm("确认", "是否确认？", function (result) {
            if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransCheck',
                        type: "post",
                        dataType: "json",
                        data: { "id": '<%=Model.id%>' },
                        beforeSend: function () {
                            $('#wProgressbar').window('open');
                            $('#p').progressbar('setValue', 0);
                            setInterval(updateProgress, 200);
                            return true;
                        },
                        success: function (r) {
                                $('#p').progressbar('setValue', 100);
                                $('#wProgressbar').window('close');

                            if (r.result) {
                                $.messager.alert('提示', '提交成功!', 'info');
                                $('#btnCheck').hide();//隐藏按钮
                            }else {
                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function updateProgress() {
            var value = $('#p').progressbar('getValue');
            if (value < 99) {
                $('#p').progressbar('setValue', value + 1);
            }
        }
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/WmsStockTransDetailPrint.aspx';
            url+="?mainId="+<%=Model.id%>;
            parent.openTab('子库存转移明细日志打印', url);
        }
    </script>
</asp:Content>
