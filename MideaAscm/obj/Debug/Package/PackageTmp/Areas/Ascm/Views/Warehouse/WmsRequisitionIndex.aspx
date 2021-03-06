﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.Warehouse.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	仓库领料
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dgWmsRequisitionMain" title="领料单管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: true, 
                          singleSelect : true,
                          checkOnSelect: false,
                          selectOnCheck: false,
                          idField: 'id',
                          sortName: 'createTime',
                          sortOrder: 'desc',
                          striped: true,
                          toolbar: '#tb',
                          pagination: true,
                          pageSize: 50,
                          pageList: [50, 100, 150, 200],
                          loadMsg: '数据加载中，请稍候...',
                          onSelect: function (rowIndex, rowData) {
                              currentId=rowData.id;
                          },
                          onCheck: function (rowIndex, rowRec) {
                              if (rowRec.statusCn == '<%=AscmWmsMtlRequisitionMain.StatusDefine.DisplayText(AscmWmsMtlRequisitionMain.StatusDefine.succeeded)%>') {
                                  $(this).datagrid('uncheckRow', rowIndex);
                              }
                          },
                          onCheckAll: function (rows) {
                              $.each(rows, function (index, row) {
                                  if (row.statusCn == '<%=AscmWmsMtlRequisitionMain.StatusDefine.DisplayText(AscmWmsMtlRequisitionMain.StatusDefine.succeeded)%>') {
                                      var rowIndex = $('#dgWmsRequisitionMain').datagrid('getRowIndex', row);
                                      $('#dgWmsRequisitionMain').datagrid('uncheckRow', rowIndex);
                                  }
                              });
                          },
                          rowStyler: function (index, rowRec) {
                              if (rowRec.statusCn == '<%=AscmWmsMtlRequisitionMain.StatusDefine.DisplayText(AscmWmsMtlRequisitionMain.StatusDefine.failed)%>') {
                                  return 'color:red;';
                              }
                          },
                          onDblClickRow: function (rowIndex, rowData) {
                              WmsRequisitionView();
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                     
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="field:'id',hidden:true"></th>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'docNumber',width:110,align:'center'">领料单号</th>
                    <th data-options="field:'jobWipEntityName',width:110,align:'left'">作业号</th>
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'jobScheduledStartDate',width:110,align:'left'">作业日期</th>
                    <th data-options="field:'jobPrimaryItemDoc',width:90,align:'left'">装配件编码</th>
                    <%--<th data-options="field:'jobPrimaryItemDes',width:230,align:'left'">装配件描述</th>--%>
                    <th data-options="field:'preparationDocNumbers',width:230,align:'left'">备料单</th>
                    <th data-options="field:'jobScheduleGroupsName',width:60,align:'center'">计划组</th>
                    <th data-options="field:'jobProductionLine',width:70,align:'center'">产线</th>
                    <th data-options="field:'createTimeCn',width:110,align:'left'">单据日期</th>
                    <th data-options="field:'statusCn',width:60,align:'center'">状态</th>
					<th data-options="field:'uploadStatusCn',width:60,align:'center'">上传状态</th>
					<th data-options="field:'uploadTimeShow',width:60,align:'center'">上传日期</th>
                </tr>
            </thead>
        </table>
        <div id="tb" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>作业日期：</span>
                <input class="easyui-datebox" id="startScheduledStartDate" type="text" style="width:120px;" value="" data-options="validType:'checkDate',onChange:scheduledStartDate_onChange"/>-<input class="easyui-datebox" id="endScheduledStartDate" type="text" style="width:120px;" value="" data-options="validType:'checkDate',onChange:scheduledEndDate_onChange"/>
                <span>单据日期：</span>
                <input class="easyui-datebox" id="queryStartCreateTime" type="text" style="width:100px;" />-<input class="easyui-datebox" id="queryEndCreateTime" type="text" style="width:100px;" />
            </div>
            <div style="margin-bottom:5px;">
                <span>&nbsp;&nbsp;&nbsp;作业号：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startWipEntitiesName", width = "120px", panelWidth = 500, queryParams = "'filterWipEntityStatus':false" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endWipEntitiesName", width = "120px", panelWidth = 500, queryParams = "'filterWipEntityStatus':false" }); %>
                <span>领料单号：</span>
                <input id="startDocNumberPrefix" type="text" maxlength="10" style="width:80px;" /><input class="easyui-validatebox" id="queryStartDocNumber" type="text" maxlength="5" style="width:50px;" />-<input id="endDocNumberPrefix" type="text" maxlength="10" style="width:80px;" /><input class="easyui-validatebox" id="queryEndDocNumber" type="text" maxlength="5" style="width:50px;" />
            </div>
			<div style="margin-bottom:5px;">
				<span> 上传状态： </span>
			    <select id="queryReturnCode" name="queryReturnCode" style="width:80px;">
					<option value="" selected="selected">所有</option>
                    <option value="0" >正常</option>
					<option value="-1" >异常</option>
                </select>

				<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
                <a href="javascript:void(0);" id="btnRequisitionOk" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="RequisitionOk();">领料重传</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" id="exportToExcel" plain="true" icon="icon-print" onclick="ExportExcel();">导出</a>
			</div>
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
                message: '日期格式输入错误.'
            }
        });
        $(function () {
            $('#queryStartCreateTime').datebox({
                validType: 'checkDate',
                onChange: function (newValue, oldValue) {
                    $('#startDocNumberPrefix').val("");
                    setDocNumberPrefix($('#startDocNumberPrefix'), newValue);
                }
            });
            $('#queryEndCreateTime').datebox({
                validType: 'checkDate',
                onChange: function (newValue, oldValue) {
                    $('#endDocNumberPrefix').val("");
                    setDocNumberPrefix($('#endDocNumberPrefix'), newValue);
                }
            });
        })
        function setDocNumberPrefix(tag, value) {
            var t = Date.parse(value);
            if (!isNaN(t)) {
                var date = new Date(t);
                var y = date.getFullYear().toString();
                var m = (date.getMonth() + 1).toString();
                m = m.length < 2 ? "0" + m : m;
                var d = date.getDate().toString();
                d = d.length < 2 ? "0" + d : d;
                tag.val("LL" + y + m + d);
            }
        }
        var startScheduledStartDate = "";
        var endScheduledStartDate = "";
        function scheduledStartDate_onChange(newValue, oldValue) {
            startScheduledStartDate = newValue;
            wipEntitiesCombogrid_Refresh();
        }
        function scheduledEndDate_onChange(newValue, oldValue) {
            endScheduledStartDate = newValue;
            wipEntitiesCombogrid_Refresh();
        }
        function wipEntitiesCombogrid_Refresh() {
            var b_options = $('#startWipEntitiesName').combogrid('options');
            b_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            b_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            var b_g = $('#startWipEntitiesName').combogrid('grid');
            b_g.datagrid('reload');

            var e_options = $('#endWipEntitiesName').combogrid('options');
            e_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            e_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            var e_g = $('#endWipEntitiesName').combogrid('grid');
            e_g.datagrid('reload');
        }
        var currentId = null;
        function Query() {
            var options = $('#dgWmsRequisitionMain').datagrid('options');
            options.url='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsRequisitionList';
            options.queryParams.startScheduledStartDate = startScheduledStartDate;
            options.queryParams.endScheduledStartDate = endScheduledStartDate;
            options.queryParams.startCreateTime = $('#queryStartCreateTime').datebox('getText');
            options.queryParams.endCreateTime = $('#queryEndCreateTime').datebox('getText');
            options.queryParams.startWipEntityName = getWipEntityName("startWipEntitiesName");
            options.queryParams.endWipEntityName = getWipEntityName("endWipEntitiesName");
			options.queryParams.returnCode = $('#queryReturnCode').val();

            var startDocNumberPrefix = $('#startDocNumberPrefix').val();
            var startDocNumber = $('#queryStartDocNumber').val();
            if (isNotNullOrEmpty(startDocNumberPrefix) && isNotNullOrEmpty(startDocNumber)) {
                options.queryParams.startDocNumber = $.trim(startDocNumberPrefix) + $.trim(startDocNumber);
            }
            var endDocNumberPrefix = $('#endDocNumberPrefix').val();
            var endDocNumber = $('#queryEndDocNumber').val();
            if (isNotNullOrEmpty(endDocNumberPrefix) && isNotNullOrEmpty(endDocNumber)) {
                options.queryParams.endDocNumber = $.trim(endDocNumberPrefix) + $.trim(endDocNumber);
            }
            $('#dgWmsRequisitionMain').datagrid('reload');
        }
        function WmsRequisitionView() {
            var selectRow = $('#dgWmsRequisitionMain').datagrid('getSelected');
            if (selectRow) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsRequisitionView/' + selectRow.id+"?mi=<%=Request["mi"].ToString() %>";
                parent.openTab('领料单_' + selectRow.docNumber, url);
            } else {
                $.messager.alert('提示', '请选择领料单！', 'info');
            }
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
    <%-- 领料确认 --%>
    <script type="text/javascript">
        function RequisitionOk() {
            var checkRows = $('#dgWmsRequisitionMain').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要重传的领料单！", "info");
                return;
            }
            var requisitionMainIds = "";
            for (var i = 0; i < checkRows.length; i++) {
                if (checkRows[i].status == '<%=MideaAscm.Dal.Warehouse.Entities.AscmWmsMtlRequisitionMain.StatusDefine.failed %>') {
                    if (requisitionMainIds != "") {
                        requisitionMainIds += ",";
                    }
                    requisitionMainIds += checkRows[i].id;
                }
            }
            if (requisitionMainIds == "") {
                $.messager.alert("提示", "请勾选领料失败的领料单！", "info");
                return;
            }
            $.messager.confirm("确认", "确认提交？", function (r) {
                if (r) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsMtlRequisitionOk',
                        type: "post",
                        dataType: "json",
                        data: { "requisitionMainIds": requisitionMainIds },
                        beforeSend: function () {
                            $('#dgWmsRequisitionMain').datagrid('loading');
                        },
                        success: function (r) {
                            if (r.result) {
                                $('#dgWmsRequisitionMain').datagrid('loaded');
                                if (r.message)
                                    $.messager.alert('警告', r.message, 'warning');
                                else
                                    $.messager.alert('提示', '领料完成', 'info');
                                Query();
                            } else {
                                $('#dgWmsRequisitionMain').datagrid('loaded');
                                $.messager.alert('错误', '领料失败:' + r.message, 'error');
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
             var detailRows = $('#dgWmsRequisitionMain').datagrid('getRows');
             var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsRequisitionExport';
             var mainIds = "";
             if (detailRows == null || detailRows.length == 0) {
                 $.messager.alert('确认', '没有作业监控明细!', 'error');
                 return;
             }         
             var startCreateTime = $('#queryStartCreateTime').datebox('getText');
             var endCreateTime = $('#queryEndCreateTime').datebox('getText');
             var startWipEntityName = getWipEntityName("startWipEntitiesName");
             var endWipEntityName = getWipEntityName("endWipEntitiesName");
             var returnCode = $('#queryReturnCode').val();

             var startDocNumberPrefix = $('#startDocNumberPrefix').val();
             var startDocNumber = $('#queryStartDocNumber').val();
             var startDocNumber = "";
             var endDocNumber = "";

             if (isNotNullOrEmpty(startDocNumberPrefix) && isNotNullOrEmpty(startDocNumber)) {
                 startDocNumber = $.trim(startDocNumberPrefix) + $.trim(startDocNumber);
             }
             var endDocNumberPrefix = $('#endDocNumberPrefix').val();
             var endDocNumber = $('#queryEndDocNumber').val();
             if (isNotNullOrEmpty(endDocNumberPrefix) && isNotNullOrEmpty(endDocNumber)) {
                 endDocNumber = $.trim(endDocNumberPrefix) + $.trim(endDocNumber);
             }

             var params = {
                'startCreateTime': startCreateTime,
                'endCreateTime': endCreateTime,
                'startWipEntityName': startWipEntityName,
                'endWipEntityName': endWipEntityName,
                'startScheduledStartDate': startScheduledStartDate,
                'endScheduledStartDate': endScheduledStartDate,
                'startDocNumber': startDocNumber,
                'endDocNumber': endDocNumber,
                'returnCode': returnCode
             };
             var iframe = document.createElement("iframe");
             iframe.src = url + "?" + $.param(params);
             iframe.style.display = "none";
             document.body.appendChild(iframe);
         }
     </script>
</asp:Content>
