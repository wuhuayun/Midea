﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.Warehouse.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	备料管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:hidden;">
        <table id="dgWmsPreparationMain" title="备料单管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true,
                          singleSelect : true,
                          checkOnSelect: false,
                          selectOnCheck: false,
                          idField: 'id', 
                          sortName: 'docNumber',
                          sortOrder: 'desc',
                          striped: true,
                          toolbar: '#tb',
                          pagination: true,
                          pageSize: 100,
                          pageList: [50, 100, 150, 200],
                          loadMsg: '数据加载中，请稍候...',
                          onSelect: function (rowIndex, rowData) {
                              currentId=rowData.id;
                          },
                          onCheck: function (rowIndex, rowRec) {
                              if (rowRec.locked) {
                                  $(this).datagrid('uncheckRow', rowIndex);
                              }
                          },
                          onCheckAll: function (rows) {
                              $.each(rows, function (index, row) {
                                  if (row.locked) {
                                      var rowIndex = $('#dgWmsPreparationMain').datagrid('getRowIndex', row);
                                      $('#dgWmsPreparationMain').datagrid('uncheckRow', rowIndex);
                                  }
                              });
                          },
                          onDblClickRow: function (rowIndex, rowData) {
                              editWmsPreparation();
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                   
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'id',hidden:true"></th>
                    <th data-options="field:'docNumber',width:90,align:'center'">备料单号</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'jobWipEntityName',width:110,align:'left'">作业号</th>
                    <th data-options="field:'requireWipEntitySegment',width:220,align:'left',hidden:true">作业号</th>
                    <th data-options="field:'jobScheduledStartDate',width:110,align:'left'">作业时间</th>
                    <th data-options="field:'requireScheduledDateSegment',width:220,align:'left',hidden:true">作业时间</th>
                    <th data-options="field:'jobPrimaryItemDoc',width:90,align:'left'">装配件编码</th>
                    <th data-options="field:'jobPrimaryItemDes',width:230,align:'left'">装配件描述</th>
                    <th data-options="field:'jobQuantity',width:60,align:'center'">作业数量</th>
                    <th data-options="field:'totalNumber',width:60,align:'center'">需求数量</th>
                    <th data-options="field:'containerBindNumber',width:60,align:'center'">备料数量</th>
                    <th data-options="field:'jobWarehouseSegment',width:120,align:'center'">子库</th>
                    <th data-options="field:'jobMtlCategorySegment',width:70,align:'center'">物料大类</th>
                    <th data-options="field:'jobScheduleGroupsName',width:60,align:'left'">计划组</th>
                    <th data-options="field:'requireScheduleGroupsName',width:60,align:'left',hidden:true">计划组</th>
                    <th data-options="field:'jobProductionLine',width:90,align:'left'">产线</th>
                    <th data-options="field:'requireProductionLine',width:90,align:'left',hidden:true">产线</th>
                    <th data-options="field:'statusCn',width:90,align:'center'">状态</th>
                    <th data-options="field:'patternCn',width:60,align:'center'">备料类型</th>
                    <th data-options="field:'createUser',width:70,align:'center'">打单员</th>
                    <th data-options="field:'createTimeShow',width:110,align:'center'">打单时间</th>
                </tr>
            </thead>
        </table>
        <div id="tb" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>作业日期：</span>
                <input class="easyui-datebox" id="startScheduledStartDate" type="text" style="width:120px;" value="" data-options="validType:'checkDate',onChange:scheduledStartDate_Change"/>-<input class="easyui-datebox" id="endScheduledStartDate" type="text" style="width:120px;" value="" data-options="validType:'checkDate',onChange:scheduledEndDate_Change"/>
                <span>单据日期：</span>
                <input class="easyui-datebox" id="startCreateTime" type="text" style="width:120px;" value="" data-options="validType:'checkDate'"/>-<input id="endCreateTime" type="text" style="width:120px;" value="" data-options="validType:'checkDate'"/>
                <span>类型：</span>
                <select id="pattern">
                    <%foreach (string patternDefine in AscmWmsPreparationMain.PatternDefine.GetList()){ %>
                    <option value="<%=patternDefine %>"><%=AscmWmsPreparationMain.PatternDefine.DisplayText(patternDefine) %></option>  
                    <%} %>    
                </select>
            </div>
            <div style="margin-bottom:5px;">
                <span>&nbsp;&nbsp;&nbsp;作业号：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startWipEntityName", width = "120px", panelWidth = 500, queryParams = "'filterWipEntityStatus':false" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endWipEntityName", width = "120px", panelWidth = 500, queryParams = "'filterWipEntityStatus':false" }); %>
                <span>备料单号：</span>
                <input id="startDocNumber" type="text" maxlength="12" style="width:110px;" />-<input id="endDocNumber" type="text" maxlength="12" style="width:110px;" />
                <span>状态：</span><%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/PrepareStatusMultipleSelect.ascx");%>
                <%--<%= Html.DropDownList("status", (IEnumerable<SelectListItem>)ViewData["listPrepareStatus"], new { style = "width:100px;" })%>--%>
            </div>
            <div style="margin-bottom:5px;">
                <span>物料编码：</span>
                <input class="easyui-validatebox" id="startMaterialDocNumber" type="text" data-options="validType:'checkLength[12]'" maxlength="12" style="width:115px;" />-<input class="easyui-validatebox" id="endMaterialDocNumber" type="text" data-options="validType:'checkLength[12]'" maxlength="12" style="width:115px;" />
                <span>打单员：</span><%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "100px" });%>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="query();">查询</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" id="exportToPdf" plain="true" icon="icon-print" onclick="exportToPdf(false);">导出</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" id="singleExportToPdf" plain="true" icon="icon-print" onclick="exportToPdf(true);">单页导出</a>
                <iframe id="iframeExportToPdf" name="iframeExportToPdf" scrolling="auto" frameborder="0"  src="" style="width:0px;height:0px;display:none;"></iframe>
                <% if (ynWebRight.rightEdit || ynWebRight.rightAdd){ %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="editWmsPreparation();">备料</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="addWmsPreparationDetail();">加入</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="confirmWmsPreparation();">单据确认</a>
                <%} %>
            </div> 
        </div>
        <div id="wMessager" class="easyui-dialog" title="提示..." style="width:400px;height:200px;padding:5px" data-options="closed: true,modal: true,shadow: true">
	    </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
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
        $.extend($.fn.validatebox.defaults.rules, {
            checkLength: {
                validator: function (value, param) {
                    return value.length == param[0];
                },
                message: '请输入{0}位字符.'
            }
        });
        $(function () {
            $('#startCreateTime').datebox({
                validType: 'checkDate',
                onChange: function (newValue, oldValue) {
                    $('#startDocNumber').val("");
                    setDocNumber($('#startDocNumber'), newValue);
                }
            });
            $('#endCreateTime').datebox({
                validType: 'checkDate',
                onChange: function (newValue, oldValue) {
                    $('#endDocNumber').val("");
                    setDocNumber($('#endDocNumber'), newValue);
                }
            });

            $('#pattern').change(function () {
                $('#startDocNumber').val("");
                setDocNumber($('#startDocNumber'), $('#startCreateTime').datebox('getText'));
                $('#endDocNumber').val("");
                setDocNumber($('#endDocNumber'), $('#endCreateTime').datebox('getText'));
            });

            $('#startMaterialDocNumber').keypress(function (e) {
                var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keyCode == 13) {
                    tagKeypress($('#startMaterialDocNumber'));
                }
            })
            $('#endMaterialDocNumber').keypress(function (e) {
                var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keyCode == 13) {
                    tagKeypress($('#endMaterialDocNumber'));
                }
            })

            setButtons();
        })
        function setDocNumber(tag, value) {
            var t = Date.parse(value);
            var p = $('#pattern').val() == "<%=AscmWmsPreparationMain.PatternDefine.wipJob %>" ? "ZB" : "XB";
            if (!isNaN(t)) {
                var date = new Date(t);
                var y = date.getFullYear().toString();
                y = y.substr(y.length - 2);
                var m = (date.getMonth() + 1).toString();
                m = m.length < 2 ? "0" + m : m;
                var d = date.getDate().toString();
                d = d.length < 2 ? "0" + d : d;
                tag.val(p + y + m + d + "0001");
            }
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
        function setButtons() {
            if ($('#pattern').val() == '<%=AscmWmsPreparationMain.PatternDefine.wipJob %>') {
                $('#exportToPdf').show();
            }
            else {
                $('#exportToPdf').hide();
            }
        }
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
        function wipEntitiesCombogrid_Refresh() {
            var b_options = $('#startWipEntityName').combogrid('options');
            b_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            b_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            var b_g = $('#startWipEntityName').combogrid('grid');
            b_g.datagrid('reload');

            var e_options = $('#endWipEntityName').combogrid('options');
            e_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            e_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            var e_g = $('#endWipEntityName').combogrid('grid');
            e_g.datagrid('reload');
        }

        var currentId = null;
        var startScheduledStartDate = "", endScheduledStartDate = "";
        function query() {
            if (!$('#startScheduledStartDate').datebox('isValid') || !$('#endScheduledStartDate').datebox('isValid')) {
                $.messager.alert("错误", "作业日期格式输入错误.", "error");
                return;
            }
            if (!$('#startCreateTime').datebox('isValid') || !$('#endCreateTime').datebox('isValid')) {
                $.messager.alert("错误", "单据日期格式输入错误.", "error");
                return;
            }
            var options = $('#dgWmsPreparationMain').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationList';
            var startDocNumber = $('#startDocNumber').val();
            if (isNotNullOrEmpty(startDocNumber))
                options.queryParams.startDocNumber = $.trim(startDocNumber);
            var endDocNumber = $('#endDocNumber').val();
            if (isNotNullOrEmpty(endDocNumber))
                options.queryParams.endDocNumber = $.trim(endDocNumber);
            options.queryParams.startScheduledStartDate = startScheduledStartDate;
            options.queryParams.endScheduledStartDate = endScheduledStartDate;
            options.queryParams.startCreateTime = $('#startCreateTime').datebox('getText');
            options.queryParams.endCreateTime = $('#endCreateTime').datebox('getText');
            options.queryParams.createUserId = $('#warehouseUserSelect').combogrid('getValue');
            options.queryParams.status = $('#prepareStatus').combobox('getValue');  //$('#status').val();
            options.queryParams.startWipEntityName = getWipEntityName("startWipEntityName");
            options.queryParams.endWipEntityName = getWipEntityName("endWipEntityName");
            options.queryParams.pattern = $('#pattern').val();
            options.queryParams.startMaterialDocNumber = $('#startMaterialDocNumber').val();
            options.queryParams.endMaterialDocNumber = $('#endMaterialDocNumber').val();
            if ($('#pattern').val() == '<%=AscmWmsPreparationMain.PatternDefine.wipJob %>') {
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'requireWipEntitySegment');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'requireScheduledDateSegment');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'requireScheduleGroupsName');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'requireProductionLine');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'jobWipEntityName');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'jobQuantity');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'jobScheduledStartDate');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'jobPrimaryItemDoc');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'jobPrimaryItemDes');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'jobScheduleGroupsName');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'jobProductionLine');
            } else if ($('#pattern').val() == '<%=AscmWmsPreparationMain.PatternDefine.wipRequire %>') {
                $('#dgWmsPreparationMain').datagrid('showColumn', 'requireWipEntitySegment');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'requireScheduledDateSegment');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'requireScheduleGroupsName');
                $('#dgWmsPreparationMain').datagrid('showColumn', 'requireProductionLine');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'jobWipEntityName');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'jobQuantity');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'jobScheduledStartDate');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'jobPrimaryItemDoc');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'jobPrimaryItemDes');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'jobScheduleGroupsName');
                $('#dgWmsPreparationMain').datagrid('hideColumn', 'jobProductionLine');
            }
            options.loadMsg = '数据加载中，请稍候...';
            $('#dgWmsPreparationMain').datagrid('reload');
            $('#dgWmsPreparationMain').datagrid('clearChecked');
            setButtons();
        }
        function editWmsPreparation() {
            var selectRow = $('#dgWmsPreparationMain').datagrid('getSelected');
            if (selectRow) {
                if (selectRow.pattern == '<%=AscmWmsPreparationMain.PatternDefine.wipJob %>') {
                    var url = '<%=Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipJobPreparationEdit/' + selectRow.id + '?mi=<%=Request["mi"].ToString() %>';
                    parent.openTab('作业备料单_' + selectRow.docNumber, url);
                } else if (selectRow.pattern == '<%=AscmWmsPreparationMain.PatternDefine.wipRequire %>') {
                    var url = '<%=Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsWipRequirePreparationEidt/' + selectRow.id + '?mi=<%=Request["mi"].ToString() %>';
                    parent.openTab('需求备料单_' + selectRow.docNumber, url);
                }
            } else {
                $.messager.alert('提示', '请选择备料单！', 'info');
            }
        }
        function addWmsPreparationDetail() {
            var checkRows = $('#dgWmsPreparationMain').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要加入的备料单！", "info");
                return;
            }
            var preparationMainIds = "";
            for (var i = 0; i < checkRows.length; i++) {
                //处于“待备料”、“备料中_未确认”和“备料中_已确认”的单据执行【加入】操作
                if (checkRows[i].status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.unPrepare %>' ||
                    checkRows[i].status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparingUnConfirm %>' ||
                    checkRows[i].status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparing %>') {
                    if (preparationMainIds != "")
                        preparationMainIds += ",";
                    preparationMainIds += checkRows[i].id;
                }
            }
            if (preparationMainIds == "") {
                $.messager.alert("提示", "所勾选的备料单不能执行【加入】操作！", "info");
                return;
            }
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationBatchAdd/',
                type: "post",
                dataType: "json",
                data: {
                    "preparationMainIds": preparationMainIds,
                    "startMaterialDocNumber": $('#startMaterialDocNumber').val(),
                    "endMaterialDocNumber": $('#endMaterialDocNumber').val()
                },
                beforeSend: function () {
                    var options = $('#dgWmsPreparationMain').datagrid('options');
                    options.loadMsg = '备料加入中，请稍候...';
                    $('#dgWmsPreparationMain').datagrid('loading');
                },
                success: function (r) {
                    $('#dgWmsPreparationMain').datagrid('loaded');
                    if (r.result) {
                        query();
                        $.messager.alert('提示', '加入成功！', 'info');
                    } else {
//                        var message = "<div style='width:200px; height:200px;'><marquee direction='up' width='98%' height='190px' scrollamount='2'>";
//                        message += r.message;
//                        message += "</marquee></div>";
                        $.messager.alert('错误', '加入失败！' + message, '');
                    }
                }
            });
        }
        function confirmWmsPreparation() {
            var checkRows = $('#dgWmsPreparationMain').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要确认的备料单！", "info");
                return;
            }

            var j = 0;
            var preparationMainIds = "", unScheduleWipEntities = "";
            for (var i = 0; i < checkRows.length; i++) {
                //处于“备料中_未确认”的单据执行【单据确认】操作
                if (checkRows[i].status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsPreparationMain.StatusDefine.preparingUnConfirm %>') {
                    if (preparationMainIds != "")
                        preparationMainIds += ",";
                    preparationMainIds += checkRows[i].id;

                    // 对“未排产”的作业提示
                    if (!checkRows[i].jobIsScheduled) {
                        j++;
                        if (j % 2 != 0)
                            unScheduleWipEntities += "<tr style=\"height:24px\">";
                        unScheduleWipEntities += "<td style=\"width: 50%;\">" + checkRows[i].jobWipEntityName + "</td>";
                        if (j % 2 == 0)
                            unScheduleWipEntities += "</tr>";
                    }
                }
            }
            if (j % 2 != 0) {
                unScheduleWipEntities += "<td style=\"width: 50%;\"></td></tr>";
            }
            if (preparationMainIds == "") {
                $.messager.alert("提示", "所勾选的备料单不能执行【单据确认】操作！", "info");
                return;
            }
            if (unScheduleWipEntities != "") {
                $('#wMessager').dialog({
                    content: '<table style="width:100%;"><thead><tr style="height:24px"><th colspan="2" style="text-align:left;color:red;">未排产作业</th></tr></thead><tbody>' + unScheduleWipEntities + '</tbody></table>',
                    buttons: [{
                        text: '确定',
                        iconCls: 'icon-ok',
                        handler: function () {
                            execWipJobPrepareConfirm(preparationMainIds);
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
                execWipJobPrepareConfirm(preparationMainIds);
            }
        }
        function execWipJobPrepareConfirm(preparationMainIds) {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationBatchConfirm/',
                type: "post",
                dataType: "json",
                data: { "preparationMainIds": preparationMainIds },
                beforeSend: function () {
                    var options = $('#dgWmsPreparationMain').datagrid('options');
                    options.loadMsg = '单据确认中，请稍候...';
                    $('#dgWmsPreparationMain').datagrid('loading');
                },
                success: function (r) {
                    $('#dgWmsPreparationMain').datagrid('loaded');
                    if (r.result) {
                        query();
                        $.messager.alert('提示', '单据确认成功！', 'info');
                    } else {
                        $.messager.alert('错误', '单据确认失败！' + r.message, 'error');
                    }
                }
            });
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
    </script>
    <!--导出-->
    <script type="text/javascript">
        function exportToPdf(bSingle) {
            var checkRows=$('#dgWmsPreparationMain').datagrid('getChecked');
            if(checkRows.length>0)
            {
                var ids=null;
                for(i=0;i<checkRows.length;i++)
                {
                    if(ids!=null)
                        ids+=",";
                    ids+=checkRows[i].id;
                }
                $.messager.confirm('确认', '确认导出备料单' + '' + '？', function (result) {
                    if (result) {
                        var url = "<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationExportToPdf?mainIdList="+ids+"&bSingle="+bSingle+"&pattern="+$('#pattern').val();
                        $('#iframeExportToPdf').attr("src",url).trigger("beforeload");
                    }
                });
            }
            else
            {
                $.messager.alert('提示', '请选择备料单！', 'info');
            }
        }
    </script>
</asp:Content>