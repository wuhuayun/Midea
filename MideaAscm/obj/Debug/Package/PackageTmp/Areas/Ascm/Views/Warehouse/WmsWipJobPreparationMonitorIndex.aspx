<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业备料监控
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <table id="dgWipJobsPreparation" title="作业备料监控" style="" border="false"></table>
        <div id="tb" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>作业日期：</span>
                <input class="easyui-datebox" id="startScheduledStartDate" type="text" style="width:150px;" value="" data-options="validType:'checkDate',onChange:scheduledStartDate_Change"/>-<input class="easyui-datebox" id="endScheduledStartDate" type="text" style="width:150px;" value="" data-options="validType:'checkDate',onChange:scheduledEndDate_Change"/>
                <span>&nbsp;作&nbsp;业&nbsp;号：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startWipEntityName", width = "150px", panelWidth = 500, queryParams = "'filterWipEntityStatus':true" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endWipEntityName", width = "150px", panelWidth = 500, queryParams = "'filterWipEntityStatus':true" }); %>
            </div>
            <div style="margin-bottom:5px;">
                <span>供应子库：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startSupplySubInventory", width = "150px" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endSupplySubInventory", width = "150px" });%>
                <span>供应类型：</span>
                <select id="wipSupplyType">
                    <option value=""></option>
                    <option value="pushType">推式</option>
                    <option value="pullType">拉式</option>
                </select>
                <span>备料状态：</span><%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/AscmPrepareStatusMultipleSelect.ascx");%>
                <%--<%= Html.DropDownList("prepareStatus", (IEnumerable<SelectListItem>)ViewData["listPrepareStatus"], new { style = "width:100px;" })%>--%>

                <span>负责人：</span>
			    <select id="queryLeader" name="queryLeader" style="width:80px;">
                    <option value=""></option>
                    <option value="ME" selected="selected">本人</option>
                    <option value="ALL">全部</option>
                </select>
                <a href="javascript:void(0);" class="easyui-linkbutton" id="exportToExcel" plain="true" icon="icon-print" onclick="ExportExcel();">导出</a>
            </div>
            <div style="margin-bottom:5px;">
                <span>物料编码：</span>
                <input class="easyui-validatebox" id="startMaterialDocNumber" type="text" data-options="validType:'checkLength[12]'" maxlength="12" style="width:145px;" />-<input class="easyui-validatebox" id="endMaterialDocNumber" type="text" data-options="validType:'checkLength[12]'" maxlength="12" style="width:145px;" />
                <span>&nbsp;计&nbsp;划&nbsp;组：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipScheduleGroupSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startScheduleGroup", width = "150px", onChange = "scheduleGroup_Change", queryParams = "fromMonitor:true" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipScheduleGroupSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endScheduleGroup", width = "150px", onChange = "scheduleGroup_Change", queryParams = "fromMonitor:true" });%>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="query();">查询</a>
                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="confirmWmsWipJobPreparation()">备料确认</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="cancelWmsWipJobPreparation()">备料取消</a>
				<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="stopJobPreparation()">停止</a>
                <%} %>
                
            </div>
        </div>
        <div id="wMessager" class="easyui-dialog" title="提示..." style="width:400px;height:200px;padding:5px" data-options="closed: true,modal: true,shadow: true">
	    </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <script type="text/javascript">
        function validateDate(value) {
            value = value.replace(/-/g, '/'); //兼容ie9以下版本的浏览器
            var t = Date.parse(value);
            return !isNaN(t);
        }
        $.extend($.fn.datebox.defaults.rules, {
            checkDate: {
                validator: function (value, param) {
                    return validateDate(value);
                },
                message: '日期格式输入错误.'
            }
        });
        $(function ()
        {
        	$('#dgWipJobsPreparation').datagrid({
        		fit: true,
        		noheader: false,
        		singleSelect: true,
        		checkOnSelect: false,
        		selectOnCheck: false,
        		rownumbers: true,
        		idField: 'wipEntityId',
        		sortName: '',
        		sortOrder: '',
        		striped: true,
        		toolbar: '#tb',
        		pagination: true,
        		pageSize: 25,
        		pageList: [25, 50, 100, 150],
        		loadMsg: '数据加载中，请稍候...',
        		view: detailview,
        		detailFormatter: function (index, row)
        		{
        			return '<div style="padding:2px"><table id="dgWmsPreparationMain_' + index + '"></table></div>';
        		},
        		frozenColumns: [[
                    { checkbox: true },
                    { field: 'wipEntityId', hidden: true },
                    { field: '_scheduledStartDate', title: '作业日期', width: 110, align: 'center' }
                ]],
        		columns: [[
                    { field: 'productionLine', title: '产线', width: 80, align: 'left' },
                    { field: 'isScheduled', title: '是否排产', width: 80, align: 'left' },
                    { field: 'ascmWipEntities_Name', title: '作业号', width: 120, align: 'left' },
					{ field: 'isStop', title: '停止', width: 65, align: 'center',
						formatter: function (value, row, index)
						{
							if (row.isStop)
							{
								return "停止";
							}

							return "";
						}
					},
                    { field: 'ascmMaterialItem_DocNumber', title: '装配件编码', width: 90, align: 'left' },
                    { field: 'ascmMaterialItem_Description', title: '装配件描述', width: 230, align: 'left' },
                    { field: 'ascmWipScheduleGroupsName', title: '计划组', width: 60, align: 'center' },
                    { field: 'netQuantity', title: '作业数量', width: 60, align: 'center' },
                    { field: 'jobLackOfMaterial', title: '缺料情况', width: 160, align: 'left', formatter: jobLackOfMaterialFormat },
                    { field: 'jobCompoundedPerson', title: '已配料人员', width: 80, align: 'left' },
                    { field: 'containerQuantity', title: '容器数', width: 50, align: 'center',
                    	styler: function (value, row, index)
                    	{
                    		if (row.containerQuantity != row.checkQuantity)
                    			return 'color:red;';
                    		return;
                    	}
                    },
                    { field: 'checkQuantity', title: '校验数', width: 50, align: 'center',
                    	styler: function (value, row, index)
                    	{
                    		if (row.containerQuantity != row.checkQuantity)
                    			return 'color:red;';
                    		return;
                    	}
                    },
                    { field: 'leaderName', title: '负责人', width: 80, align: 'left' },
                    { field: 'subStatusCn', title: '备料状态', width: 60, align: 'center',
                    	styler: function (value, row, index)
                    	{
                    		switch (row.subStatus)
                    		{
                    			case '<%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.AscmStatusDefine.unPrepare %>':
                    				return 'color:#FF0000;';
                    			case '<%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.AscmStatusDefine.preparing %>':
                    				return 'color:#0000FF;';
                    			case '<%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.AscmStatusDefine.unPick %>':
                    				return 'color:#00FF00;';
                    			case '<%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.AscmStatusDefine.picked %>':
                    				return 'color:red;';
                    			default: return;
                    		}
                    	}
                    }
                ]],
        		onExpandRow: function (index, row)
        		{
        			$('#dgWmsPreparationMain_' + index).datagrid({
        				url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationList/',
        				queryParams: { "wipEntityId": row.wipEntityId, "leaderId": row.leaderId },
        				fitColumns: true,
        				singleSelect: true,
        				height: 'auto',
        				columns: [[
						    { field: 'id', hidden: true },
						    { field: 'docNumber', title: '备料单号', width: 100, align: 'center' },
                            { field: 'jobWarehouseSegment', title: '子库', width: 120, align: 'center' },
                            { field: 'jobMtlCategorySegment', title: '物料大类', width: 70, align: 'center' },
                            { field: 'statusCn', title: '状态', width: 60, align: 'center',
                            	styler: function (value, row, index)
                            	{
                            		switch (row.status)
                            		{
                            			case '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.unPrepare %>':
                            				return 'color:#FF0000;';
                            			case '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparing %>':
                            				return 'color:#0000FF;';
                            			case '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.prepared %>':
                            				return 'color:#00FF00;';
                            			default: return;
                            		}
                            	}
                            },
                            { field: 'createUserName', title: '打印人', width: 70, align: 'center' },
                            { field: 'createTimeShow', title: '打印时间', width: 110, align: 'center' }
					    ]],
        				onResize: function ()
        				{
        					$('#dgWipJobsPreparation').datagrid('fixDetailRowHeight', index);
        				},
        				onDblClickRow: function (rowIndex, rowData)
        				{
        					var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationEdit/' + rowData.id + '?mi=<%=Request["mi"].ToString() %>';
        					parent.openTab('作业备料单_' + rowData.docNumber, url);
        				},
        				onLoadSuccess: function ()
        				{
        					setTimeout(function ()
        					{
        						$('#dgWipJobsPreparation').datagrid('fixDetailRowHeight', index);
        					}, 0);
        				}
        			});
        			$('#dgWipJobsPreparation').datagrid('fixDetailRowHeight', index);
        		},
        		onLoadSuccess: function ()
        		{
        			$(this).datagrid('clearSelections');
        			$(this).datagrid('clearChecked');
        		}
        	});
        	$('#startMaterialDocNumber').keypress(function (e)
        	{
        		var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        		if (keyCode == 13)
        		{
        			tagKeypress($('#startMaterialDocNumber'));
        		}
        	});
        	$('#endMaterialDocNumber').keypress(function (e)
        	{
        		var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        		if (keyCode == 13)
        		{
        			tagKeypress($('#endMaterialDocNumber'));
        		}
        	});
        });
        function jobLackOfMaterialFormat(value, row, index) {
            if (row.jobLackOfMaterial) {
                return "<a href='javascript:void(0)' onclick='jobLackOfMaterialQuery(\"" + row.wipEntityId + "\",\"" + row.ascmWipEntities_Name + "\");'>" + row.jobLackOfMaterial + "</a>";
            }
        }
        function jobLackOfMaterialQuery(wipEntityId, wipEntityName) {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobLackOfMaterialQuery/' + wipEntityId + '?mi=<%=Request["mi"].ToString() %>';
            parent.openTab('备料缺料查询_' + wipEntityName, url);
        }
        function tagKeypress(tag) {
            var value = tag.val();
            if (value != undefined && value != null && value != "" && value.length > 0) {
                var lastChar = value.substr(value.length - 1);
                while (value.length < 12) {
                    value += lastChar.toString();
                }
                tag.val(value);
            }
        }
        $.extend($.fn.validatebox.defaults.rules, {
            checkLength: {
                validator: function (value, param) {
                    return value.length == param[0];
                },
                message: '请输入{0}位字符.'
            }
        });
        //变更"作业日期"，刷新作业
        function scheduledStartDate_Change(newValue, oldValue) {
            startScheduledStartDate = "";
            if (validateDate(newValue)) {
                startScheduledStartDate = newValue;
            }
            wipEntitiesCombogrid_Refresh();
        }
        function scheduledEndDate_Change(newValue, oldValue) {
            endScheduledStartDate = "";
            if (validateDate(newValue)) {
                endScheduledStartDate = newValue;
            }
            wipEntitiesCombogrid_Refresh();
        }
        //变更"计划组"，刷新作业
        function scheduleGroup_Change(newValue, oldValue) {
            wipEntitiesCombogrid_Refresh();
        }
        //根据"作业日期"、"计划组"刷新作业
        function wipEntitiesCombogrid_Refresh() {
            startScheduleGroupName = getScheduleGroupName("startScheduleGroup");
            endScheduleGroupName = getScheduleGroupName("endScheduleGroup");

            var b_options = $('#startWipEntityName').combogrid('options');
            b_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            b_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            b_options.queryParams.startScheduleGroupName = startScheduleGroupName;
            b_options.queryParams.endScheduleGroupName = endScheduleGroupName;
            var b_g = $('#startWipEntityName').combogrid('grid');
            b_g.datagrid('reload');

            var e_options = $('#endWipEntityName').combogrid('options');
            e_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            e_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            e_options.queryParams.startScheduleGroupName = startScheduleGroupName;
            e_options.queryParams.endScheduleGroupName = endScheduleGroupName;
            var e_g = $('#endWipEntityName').combogrid('grid');
            e_g.datagrid('reload');
        }
        var wipSupplyType = "", queryLeader = "";
        var startSupplySubInventory = "", endSupplySubInventory = "";
        var startMaterialDocNumber = "", endMaterialDocNumber = "";
        var startScheduledStartDate = "", endScheduledStartDate = "";
        var startScheduleGroupName = "", endScheduleGroupName = "";
        function query() {
            if (!$('#startScheduledStartDate').datebox('isValid') || !$('#endScheduledStartDate').datebox('isValid')) {
                $.messager.alert("错误", "日期格式输入错误.", "error");
                return;
            }
            wipSupplyType = $('#wipSupplyType').val();
            queryLeader = $('#queryLeader').val();

            //开始子库大小写通用
            startSupplySubInventory = getSubInventoryId("startSupplySubInventory");
            //结束子库大小写通用
            endSupplySubInventory = getSubInventoryId("endSupplySubInventory");
            //物料
            startMaterialDocNumber = $('#startMaterialDocNumber').val();
            endMaterialDocNumber = $('#endMaterialDocNumber').val();

            var options = $('#dgWipJobsPreparation').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationMonitorList';
            options.queryParams.startScheduledStartDate = startScheduledStartDate;
            options.queryParams.endScheduledStartDate = endScheduledStartDate;
            //开始作业大小写通用
            options.queryParams.startWipEntitiesName = getWipEntityName("startWipEntityName");
            //结束作业大小写通用
            options.queryParams.endWipEntitiesName = getWipEntityName("endWipEntityName");
            options.queryParams.wipSupplyType = wipSupplyType;
            options.queryParams.queryLeader = queryLeader;
            options.queryParams.prepareStatus = $('#prepareStatus').combobox('getValue'); //$('#prepareStatus').val();
            options.queryParams.startSupplySubInventory = startSupplySubInventory;
            options.queryParams.endSupplySubInventory = endSupplySubInventory;
            options.queryParams.startMaterialDocNumber = startMaterialDocNumber;
            options.queryParams.endMaterialDocNumber = endMaterialDocNumber;
            options.queryParams.startScheduleGroupName = startScheduleGroupName;
            options.queryParams.endScheduleGroupName = endScheduleGroupName;
            $('#dgWipJobsPreparation').datagrid('reload');
        }
        function confirmWmsWipJobPreparation() {
            var checkRows = $('#dgWipJobsPreparation').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要备料确认的作业！", "info");
                return;
            }

            //检查是否停止备料
            var isStop = false;
            $.each(checkRows, function (i, row)
            {
               	if (row.isStop) isStop = true;
            });
            if (isStop)
            {
            	$.messager.alert("提示", "备料确认的作业已经停止！", "info");
            	return;
			}
            
            var j = 0;
            var wipEntityIds = "", unScheduleWipEntities = "", leaderIds = "";
            $.each(checkRows, function (i, row) {
                //处于“备料中”的作业执行【备料确认】操作
                if (row.subStatus == '<%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.AscmStatusDefine.preparing %>') {
                    if (wipEntityIds != "") wipEntityIds += ",";
                    wipEntityIds += row.wipEntityId;

                    if (leaderIds != "") leaderIds += ",";
                    if (row.leaderId == undefined || row.leaderId == null || row.leaderId == "") {
                        leaderIds += " ";
                    } else {
                        leaderIds += row.leaderId;
                    }

                    // 对“未排产”的作业提示
                    if (!row.isScheduled) {
                        j++;
                        if (j % 2 != 0)
                            unScheduleWipEntities += "<tr style=\"height:24px\">";
                        unScheduleWipEntities += "<td style=\"width: 50%;\">" + row.ascmWipEntities_Name + "</td>";
                        if (j % 2 == 0)
                            unScheduleWipEntities += "</tr>";
                    }
                }
            });
            if (j % 2 != 0) {
                unScheduleWipEntities += "<td style=\"width: 50%;\"></td></tr>";
            }
            if (wipEntityIds == "") {
                $.messager.alert("提示", "所勾选的作业不能执行【备料确认】操作！", "info");
                return;
            }
            if (unScheduleWipEntities != "") {
                $('#wMessager').dialog({
                    content: '<table style="width:100%;"><thead><tr style="height:24px"><th colspan="2" style="text-align:left;color:red;">未排产作业</th></tr></thead><tbody>' + unScheduleWipEntities + '</tbody></table>',
                    buttons: [{
                        text: '确定',
                        iconCls: 'icon-ok',
                        handler: function () {
                            execWipJobPrepareConfirm(wipEntityIds, leaderIds);
                            $('#wMessager').dialog('close');
                        }
                    }, {
                        text: '取消',
                        iconCls: 'icon-cancle',
                        handler: function () {
                            $('#wMessager').dialog('close');
                        }
                    }]
                }).dialog('open');
            } else {
                execWipJobPrepareConfirm(wipEntityIds, leaderIds);
            }
        }
        function execWipJobPrepareConfirm(wipEntityIds, leaderIds) {
            $.messager.confirm("确认", "确认执行【备料确认】？", function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationConfirm/',
                        type: "post",
                        dataType: "json",
                        data: { "wipEntityIds": wipEntityIds, "leaderIds": leaderIds },
                        success: function (r) {
                            if (r.result) {
                                $.messager.alert('提示', '备料确认成功！', 'info', function () {
                                    query();
                                });
                            } else {
                                $.messager.alert('错误', '备料确认失败！' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function cancelWmsWipJobPreparation() {
            var checkRows = $('#dgWipJobsPreparation').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要取消备料确认的作业！", "info");
                return;
            }

            //检查是否停止备料
            var isStop = false;
            $.each(checkRows, function (i, row)
            {
				 if (row.isStop) isStop = true;
            });
            if (isStop)
            {
				$.messager.alert("提示", "备料确认的作业已经停止！", "info");
				return;
            }


            var wipEntityIds = "", leaderIds = "";
            $.each(checkRows, function (i, row) {
                //处于“待领料”的作业执行【备料取消】操作
                if (row.subStatus == '<%=MideaAscm.Dal.FromErp.Entities.AscmWipDiscreteJobs.AscmStatusDefine.unPick %>') {
                    if (wipEntityIds != "")
                        wipEntityIds += ",";
                    wipEntityIds += row.wipEntityId;

                    if (leaderIds != "") leaderIds += ",";
                    if (row.leaderId == undefined || row.leaderId == null || row.leaderId == "") {
                        leaderIds += " ";
                    } else {
                        leaderIds += row.leaderId;
                    }
                }
            });
            if (wipEntityIds == "") {
                $.messager.alert("提示", "所勾选的作业不能执行【备料取消】操作！", "info");
                return;
            }
            $.messager.confirm("确认", "确认执行【备料取消】？", function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationCancel/',
                        type: "post",
                        dataType: "json",
                        data: { "wipEntityIds": wipEntityIds, "leaderIds": leaderIds },
                        success: function (r) {
                            if (r.result) {
                                $.messager.alert('提示', '备料取消成功！', 'info', function () {
                                    query();
                                });
                            } else {
                                $.messager.alert('错误', '备料取消失败！' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }

        function stopJobPreparation()
        {
        	var checkRows = $('#dgWipJobsPreparation').datagrid('getChecked');
        	if (checkRows.length == 0)
        	{
        		$.messager.alert("提示", "请勾选要取消备料确认的作业！", "info");
        		return;
        	}
        	var statusIds = "";
        	$.each(checkRows, function (i, row)
        	{
        		if (statusIds != "") statusIds += ",";
        		statusIds += row.statusId;
        	});
        	
        	$.messager.confirm("确认", "确认执行【停止】备料吗？", function (r)
        	{
        		if (r)
        		{
        			$.ajax({
        				url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationStop/',
        				type: "post",
        				dataType: "json",
        				data: { "statusIds": statusIds },
        				success: function (r)
        				{
        					if (r.result)
        					{
        						$.messager.alert('提示', '停止备料成功！', 'info', function ()
        						{
        							query();
        						});
        					} else
        					{
        						$.messager.alert('错误', '停止备料失败！' + r.message, 'error');
        					}
        				}
        			});
        		}
        	});
        }
    </script>
    <%--导出excel--%>
     <script type="text/javascript">
         function ExportExcel() {
             var detailRows = $('#dgWipJobsPreparation').datagrid('getRows');
             var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationExport';
             var wipEntityIds = "";
             if (detailRows == null || detailRows.length == 0) {
                 $.messager.alert('确认', '没有作业监控明细!', 'error');
                 return;
             }
             var params = {
                 wipSupplyType : $('#wipSupplyType').val(),
                queryLeader : $('#queryLeader').val(),
                //开始子库大小写通用
                startSupplySubInventory : getSubInventoryId("startSupplySubInventory"),
                //结束子库大小写通用
                endSupplySubInventory : getSubInventoryId("endSupplySubInventory"),
                //物料
                startMaterialDocNumber : $('#startMaterialDocNumber').val(),
                endMaterialDocNumber : $('#endMaterialDocNumber').val(),

                startScheduledStartDate : startScheduledStartDate,
                endScheduledStartDate : endScheduledStartDate,
                //开始作业大小写通用
                startWipEntitiesName : getWipEntityName("startWipEntityName"),
                //结束作业大小写通用
                endWipEntitiesName : getWipEntityName("endWipEntityName"),
                prepareStatus : $('#prepareStatus').combobox('getValue'),
                startScheduleGroupName : startScheduleGroupName,
                endScheduleGroupName : endScheduleGroupName
             };
             var iframe = document.createElement("iframe");
             iframe.src = url + "?" + $.param(params);
             iframe.style.display = "none";
             document.body.appendChild(iframe);
         }
     </script>
    <%--大小写通用--%>
    <script type="text/javascript">
        //获取子库ID，大小写通用
        function getSubInventoryId(cboId) {
            var subInventoryId = $('#' + cboId).combobox('getValue');
            var g = $('#' + cboId).combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].id.toUpperCase() == subInventoryId.toUpperCase()) {
                    subInventoryId = rows[i].id;
                    break;
                }
            }
            return subInventoryId;
        }
        //获取作业名称，大小写通用
        function getWipEntityName(cboId) {
            var wipEntityName = $('#' + cboId).combobox('getValue');
            var g = $('#' + cboId).combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].wipEntityId == wipEntityName || rows[i].name.toUpperCase() == wipEntityName.toUpperCase()) {
                    wipEntityName = rows[i].name;
                    break;
                }
            }
            return wipEntityName;
        }
        //获取计划组名称，大小写通用
        function getScheduleGroupName(cboId) {
            var scheduleGroupName = $('#' + cboId).combobox('getValue');
            var g = $('#' + cboId).combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].scheduleGroupId == scheduleGroupName || rows[i].scheduleGroupName.toUpperCase() == scheduleGroupName.toUpperCase()) {
                    scheduleGroupName = rows[i].scheduleGroupName;
                    break;
                }
            }
            return scheduleGroupName;
        }
    </script>
</asp:Content>
