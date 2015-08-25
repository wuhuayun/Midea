<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	预约送货管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<table id="dataGridDeliveryNotify" title="预约送货管理" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tb">
		</table>
        <div id="tb">
            <div style="padding:1px 0px"><span>供&nbsp;应&nbsp;商：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx"); %>
            <span>物料：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Public/MaterialItemSelectCombo.ascx"); %>
            <span>仓库：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx"); %></div>
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
            </select></div>
            <span>采&nbsp;购&nbsp;员：</span>
            <%Html.RenderPartial("~/Areas/Ascm/Views/Public/EmployeeSelectCombo.ascx", new MideaAscm.Code.SelectCombo { queryParams = "'id':'" + ViewData["employeeId"] + "'" }); %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit();">修改</a>
            <%} %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印统计表</a>
        </div>
		<div id="editAppointmentDelivery" class="easyui-window" title="预约送货调整" style="padding: 10px;width:640px;height:480px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                    <form id="editAppointmentDeliveryForm" method="post" style="">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>送货通知编号：</span>
						        </td>
						        <td style="width:80%">
							        <input id="docNumber" name="docNumber" type="text" style="width:120px;" readonly="readonly"/>
						        </td>
					        </tr>	
					        <tr style="height:24px">
						        <td style="width: 20%; text-align:right;" nowrap>
							        <span>预约送货时间：</span>
						        </td>
						        <td>
                                    <%-- <input type="text" name="appointmentStartTime" id="appointmentStartTime" value="" style="width:120px;"/>--%>
                                    <input id="appointmentStartTime" name="appointmentStartTime" class="easyui-datetimebox" data-options="showSeconds:false" style="width:145px"/>
                                    -
                                    <%--<input type="text" name="appointmentEndTime" id="appointmentEndTime" value="" style="width:120px;"/>--%>
                                    <input id="appointmentEndTime" name="appointmentEndTime" class="easyui-datetimebox" data-options="showSeconds:false" style="width:145px"/>
						        </td>
					        </tr>					
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>备注：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="description" name="description" type="text" style="width:400px;" />
						        </td>
					        </tr>
				        </table>
			        </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="Save()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editAppointmentDelivery').window('close');">取消</a>
				</div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <%--
    <style type="text/css">
        .ui-timepicker-div .ui-widget-header { margin-bottom: 8px; }
        .ui-timepicker-div dl { text-align: left; }
        .ui-timepicker-div dl dt { height: 25px; margin-bottom: -25px; }
        .ui-timepicker-div dl dd { margin: 0 10px 10px 65px; }
        .ui-timepicker-div td { font-size: 90%; }
        .ui-tpicker-grid-label { background: none; border: none; margin: 0; padding: 0; }

        .ui-timepicker-rtl{ direction: rtl; }
        .ui-timepicker-rtl dl { text-align: right; }
        .ui-timepicker-rtl dl dd { margin: 0 65px 10px 10px; }
    </style>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jqueryui/jquery-ui-1.9.0.custom.min.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jqueryui/jquery-ui-timepicker-addon.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jqueryui/jquery.ui.datepicker-zh-CN.js"></script>
    <link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/jqueryui/themes/base/jquery-ui.css" rel="stylesheet" type="text/css" />--%>
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
                    { field: 'docNumber', title: '通知编号', width: 100, align: 'center' },
                ]],
                columns: [[
                    { field: 'wipEntitiesName', title: '任务号', width: 110, align: 'center' },
                    { field: 'wipEntitiesScheduledStartTime', title: '作业开始日期', width: 110, align: 'center' },
                    { field: 'releasedTimeShow', title: '下单日期', width: 110, align: 'center' },
                    { field: 'needTimeShow', title: '需求日期', width: 110, align: 'center' },
                    { field: 'supplierDocNumber', title: '供应商编号', width: 90, align: 'center' },
                    { field: 'supplierName', title: '供应商', width: 200, align: 'left' },
                    { field: 'warehouseId', title: '子库', width: 80, align: 'center' },
                    { field: 'materialDocNumber', title: '物料编码', width: 100, align: 'center' },
                    { field: 'materialName', title: '物料', width: 200, align: 'left' },
                    { field: 'releasedQuantity', title: '下单数量', width: 60, align: 'center' },
                    { field: 'promisedQuantity', title: '承诺数量', width: 60, align: 'center' },
                    { field: 'deliveryQuantity', title: '在途数量', width: 60, align: 'center' },
                    { field: 'detailCount', title: 'PO数', width: 60, align: 'center' },
                    { field: 'materialBuyerName', title: '采购员', width: 60, align: 'center' },
                    { field: 'createUserEmployeeName', title: '下单人', width: 60, align: 'center' },
                    { field: 'ascmHrLocationsAllCode', title: '厂区', width: 60, align: 'center' },
                    { field: 'bomDepartmentsCode', title: '车间', width: 60, align: 'center' },
                    { field: 'ascmFndLookupValuesMeaning', title: '备料类型', width: 60, align: 'center' },
                    { field: 'appointmentStartTimeShow', title: '预约开始送货时间', width: 110, align: 'center' },
                    { field: 'appointmentEndTimeShow', title: '预约截止送货时间', width: 110, align: 'center' },
                    { field: 'statusCn', title: '状态', width: 70, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    Edit();
                    <%} %>
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
            <%--$('#appointmentStartTime').datetimepicker({
                dateFormat: 'yy-mm-dd',
	            timeFormat: "hh:mm",//格式化时间
                showSecond: false, //显示秒
                stepHour: 1,//设置步长
                stepMinute: 10,
                stepSecond: 5
            });
            $('#appointmentEndTime').datetimepicker({
                dateFormat: 'yy-mm-dd',
	            timeFormat: "hh:mm",//格式化时间
                showSecond: false, //显示秒
                stepHour: 1,//设置步长
                stepMinute: 10,
                stepSecond: 5
            });--%>
        })
        function Query() {
            var queryParams = $('#dataGridDeliveryNotify').datagrid('options').queryParams;
            //queryParams.queryWord = $('#supplierSearch').val();
            queryParams.supplier = $('#supplierSelect').combogrid('getValue');
            queryParams.materialItem = $('#materialItemSelect').combogrid('getValue');
            queryParams.warehouse = $('#warehouseSelect').combogrid('getValue');
            queryParams.startReleasedTime = $("#queryStartReleasedTime").datebox('getText');
            queryParams.endReleasedTime = $("#queryEndReleasedTime").datebox('getText');
            queryParams.startNeedTime = $("#queryStartNeedTime").datebox('getText');
            queryParams.endNeedTime = $("#queryEndNeedTime").datebox('getText');
            queryParams.status = $('#queryStatus').val();
            queryParams.employeeBuyer = $('#employeeSelect').combogrid('getValue');
            //$('#dataGridDeliveryNotify').datagrid('reload');
            $('#dataGridDeliveryNotify').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryNotifyMainList'
            });
        }
        var currentId = null;
        function Edit() {
            var selectRow = $('#dataGridDeliveryNotify').datagrid('getSelected');
            if (selectRow) {
                $('#editAppointmentDelivery').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryNotifyMainEdit/'+selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editAppointmentDeliveryForm")[0].reset();

                        $('#docNumber').val(r.docNumber);
                        //$('#appointmentStartTime').val(r.appointmentStartTime);
                        //$('#appointmentEndTime').val(r.appointmentEndTime);
                        $('#appointmentStartTime').datetimebox('setValue',r.appointmentStartTime);
                        $('#appointmentEndTime').datetimebox('setValue',r.appointmentEndTime);
                        //$('#appointmentStartTime').datebox('setValue', r.appointmentStartTime);

                        $('#docNumber').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择送货通知', 'info');
            }
        }
        function Save() {
            //$.messager.confirm("确认", "确认提交保存?", function (r) {
                //if (r) {
                if(confirm('确认提交保存?')){
                    $('#editAppointmentDeliveryForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/DeliveryNotifyMainSave/' + currentId,
                        onSubmit: function () {
                            return $('#editAppointmentDeliveryForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editAppointmentDelivery').window('close');
                                Query();
                            } else {
                                //$.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                                alert('修改信息失败:' + rVal.message);
                            }
                        }
                    });
                }
                //}
            //});
        }
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/DeliveryNotifyPrint.aspx?supplier=' + $('#supplierSelect').combogrid('getValue');
            url += "&materialItem=" + $('#materialItemSelect').combogrid('getValue');
            url += "&warehouse=" + $('#warehouseSelect').combogrid('getValue');
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
