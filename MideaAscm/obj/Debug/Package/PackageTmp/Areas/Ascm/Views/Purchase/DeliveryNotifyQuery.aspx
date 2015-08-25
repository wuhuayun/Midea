<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	送货通知查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryNotify" title="送货通知查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb">
		</table>
        <div id="tb">
            <div style="padding:1px 0px"><span>供&nbsp;应&nbsp;商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx"); %>
            <span>物料：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Public/MaterialItemSelectCombo.ascx"); %>
            <span>仓库：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx"); %>
            <span>到期预警：</span>
            <select id="queryAlert" name="queryAlert" style="width:80px;">
                <option value=""></option>
                <% List<string> listAlertDefine = MideaAscm.Dal.FromErp.Entities.AscmDeliveryNotifyMain.AlertDefine.GetList(); %>
                <% if (listAlertDefine != null && listAlertDefine.Count > 0)
                    { %>
                <% foreach (string alertDefine in listAlertDefine)
                    { %>
                <option value="<%=alertDefine %>"><%=MideaAscm.Dal.FromErp.Entities.AscmDeliveryNotifyMain.AlertDefine.DisplayText(alertDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <input type="checkbox" id="queryFilter" name="queryFilter" value="" onclick="javascript:if($('#queryFilter')[0].checked) $('#queryFilter')[0].value = 'filter'; else $('#queryFilter')[0].value = '' "/>缺料筛选
            <input type="checkbox" id="queryWipEntityIds" name="queryWipEntityIds" value="" onclick="javascript:if($('#queryWipEntityIds')[0].checked) $('#queryWipEntityIds')[0].value = 'wipEntityIds'; else $('#queryWipEntityIds')[0].value = '' "/>含作业
            </div>
            <div style="padding:1px 0px">下单时间：<input class="easyui-datebox" id="queryStartReleasedTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndReleasedTime" type="text" style="width:100px;" />
            需求时间：<input class="easyui-datebox" id="queryStartNeedTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndNeedTime" type="text" style="width:100px;" />
            <span>状态：</span>
			<select id="queryStatus" name="queryStatus" style="width:80px;">
                <option value=""></option>
                <% List<string> listStatusDefine = MideaAscm.Dal.FromErp.Entities.AscmDeliveryNotifyMain.StatusDefine.GetList(); %>
                <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in listStatusDefine)
                    { %>
                <option value="<%=statusDefine %>"><%=MideaAscm.Dal.FromErp.Entities.AscmDeliveryNotifyMain.StatusDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <span>采&nbsp;购&nbsp;员：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Public/EmployeeSelectCombo.ascx", new MideaAscm.Code.SelectCombo { queryParams = "'id':'" + ViewData["employeeId"] + "'" }); %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印统计表</a>
        </div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>

    <script type="text/javascript">
        $(function () {
            $('#dataGridDeliveryNotify').datagrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryNotifyMainList',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px"><table id="deliveryNotifyDetail_' + index + '"></table></div>';
                },
                //showFooter: true,
                loadMsg: "加载中...",
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '通知编号', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'wipEntitiesName', title: '任务号', width: 110, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' },
                    { field: 'materialName', title: '物料描述', width: 200, align: 'left' },
                    { field: 'supplierShortName', title: '供应商简称', width: 200, align: 'left' },
                    { field: 'statusCn', title: '状态', width: 70, align: 'center' },
                    { field: 'releasedQuantity', title: '下单数量', width: 60, align: 'center' },
                    { field: 'promisedQuantity', title: '承诺数量', width: 60, align: 'center' },
                    { field: 'IN_DELIVERY_QTY', title: '在途数量', width: 60, align: 'center' },
                    { field: 'containerBindQuantity', title: '容器绑定数量', width: 60, align: 'center' },
                    { field: 'SUBINV_QTY', title: '入库数量', width: 60, align: 'center' },
                    { field: 'TOTAL_RECEIPT_QTY', title: '总接收数量', width: 60, align: 'center' },
                    //{ field: 'totalReceiveQuantity', title: '总数量', width: 60, align: 'center' },
                    { field: 'ascmWipDiscreteJobsScheduledStartTimeCn', title: '作业开始日期', width: 110, align: 'center' },
                    { field: 'releasedTimeShow', title: '下单日期', width: 110, align: 'center' },
                    { field: 'needTimeShow', title: '需求日期', width: 110, align: 'center' },
                    { field: 'appointmentStartTime', title: '预约开始日期', width: 110, align: 'center' },
                    { field: 'appointmentEndTime', title: '预约最后日期', width: 110, align: 'center' },
                    { field: 'materialBuyerName', title: '采购员', width: 60, align: 'center' },
                    { field: 'createUserEmployeeName', title: '下单人', width: 60, align: 'center' },
                    { field: 'warehouseId', title: '子库', width: 80, align: 'center' },
                    { field: 'ascmFndLookupValuesMeaning', title: '备料类型', width: 60, align: 'center' },
                    { field: 'promiseTime', title: '承诺日期', width: 110, align: 'center' },
                    { field: 'confirmTime', title: '确认日期', width: 110, align: 'center' },
                    { field: 'recevieTime', title: '接收日期', width: 110, align: 'center' },
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onExpandRow: function (index, row) {
                    $('#deliveryNotifyDetail_' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryNotifyDetailList/' + row.id,
                        fitColumns: true,
                        singleSelect: true,
                        height: 'auto',
                        columns: [[
						    { field: 'id', title: 'ID', width: 20, hidden: true },
						    { field: 'lineNumber', title: '序号', width: 50 },
						    { field: 'PO_LINE_ID', title: 'PO_LINE_ID', width: 100 },
						    { field: 'quantity', title: '数量', width: 120 },
						    { field: 'deliveryQuantity', title: '在途数量', align: 'center', width: 120 }
					    ]],
                        onResize: function () {
                            $('#dataGridDeliveryNotify').datagrid('fixDetailRowHeight', index);
                        },
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                $('#dataGridDeliveryNotify').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        }
                    });
                    $('#dataGridDeliveryNotify').datagrid('fixDetailRowHeight', index);
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })
        })
        function Query() {
            var queryParams = $('#dataGridDeliveryNotify').datagrid('options').queryParams;
            //queryParams.queryWord = $('#supplierSearch').val();
            queryParams.supplier = $('#supplierSelect').combogrid('getValue');
            queryParams.materialItem = $('#materialItemSelect').combogrid('getValue');
            queryParams.warehouse = $('#warehouseSelect').combogrid('getValue');
            queryParams.alert = $('#queryAlert').val();
            queryParams.filter = $('#queryFilter').val();
            queryParams.startReleasedTime = $("#queryStartReleasedTime").datebox('getText');
            queryParams.endReleasedTime = $("#queryEndReleasedTime").datebox('getText');
            queryParams.startNeedTime = $("#queryStartNeedTime").datebox('getText');
            queryParams.endNeedTime = $("#queryEndNeedTime").datebox('getText');
            queryParams.status = $('#queryStatus').val();
            queryParams.employeeBuyer = $('#employeeSelect').combogrid('getValue');
            queryParams.wipEntityIds = $('#queryWipEntityIds').val();
            //$('#dataGridDeliveryNotify').datagrid('reload');
            $('#dataGridDeliveryNotify').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryNotifyMainList'
            });
        }
        var currentId = null;
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/DeliveryNotifyPrint.aspx?supplier=' + $('#supplierSelect').combogrid('getValue');
            url += "&materialItem=" + $('#materialItemSelect').combogrid('getValue');
            url += "&warehouse=" + $('#warehouseSelect').combogrid('getValue');
            url += "&alert=" + $('#queryAlert').val();
            url += "&startReleasedTime=" + $("#queryStartReleasedTime").datebox('getText');
            url += "&endReleasedTime=" + $("#queryEndReleasedTime").datebox('getText');
            url += "&startNeedTime=" + $("#queryStartNeedTime").datebox('getText');
            url += "&endNeedTime=" + $("#queryEndNeedTime").datebox('getText');
            url += "&status=" + $('#queryStatus').val();
            url += "&employeeBuyer=" + $('#employeeSelect').combogrid('getValue');
            parent.openTab('送货通知打印', url);
        }
    </script>
</asp:Content>
