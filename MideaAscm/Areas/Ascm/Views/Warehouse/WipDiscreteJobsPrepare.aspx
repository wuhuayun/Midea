<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Ascm/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.Warehouse.Entities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	作业备料
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <table id="dgWipDiscreteJobs" title="作业物料查找" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          noheader: false,
                          singleSelect: false,
                          checkOnSelect: true,
                          selectOnCheck: true,
                          rownumbers: true,
                          idField: 'wipEntityId',
                          sortName: '',
                          sortOrder: '',
                          striped: true,
                          toolbar: '#tb',
                          pagination: true,
                          pageSize: 100,
                          pageList: [50, 100, 150, 200],
                          loadMsg: '数据加载中，请稍候...',
                          onDblClickRow: function(rowIndex, rowRec){
                                
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              $(this).datagrid('clearChecked');                    
                          }">
            <thead data-options="frozen:true">
                <tr>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'wipEntityId',hidden:'true'"></th>
                    <th data-options="field:'_scheduledStartDate',width:100,align:'center'">作业计划时间</th>
                    <th data-options="field:'ascmWipEntities_Name',width:120,align:'left'">作业名称</th> 
                </tr>
            </thead>
            <thead>
                <tr>
                    <th data-options="field:'ascmMaterialItem_DocNumber',width:90,align:'center'">装配件编码</th>
                    <th data-options="field:'netQuantity',width:60,align:'center'">作业数量</th>
                    <th data-options="field:'ascmWipScheduleGroupsName',width:60,align:'center'">计划组</th>
                    <th data-options="field:'productionLine',width:90,align:'left'">产线</th>
                    <th data-options="field:'statusTypeCn',width:70,align:'center'">作业状态</th>
                    <th data-options="field:'ascmMaterialItem_Description',width:230,align:'left'">装配件描述</th>
                    <th data-options="field:'scheduledCompletionDateShow',width:100,align:'center'">作业上线时间</th>
                    <th data-options="field:'description',width:280,align:'left'">作业描述</th>
                </tr>
            </thead>
		</table>
        <div id="tb" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>作业日期：</span>
                <input class="easyui-datebox" id="startScheduledStartDate" type="text" style="width:120px;" value="" data-options="validType:'checkDate',onChange:scheduledStartDate_Change"/>-<input class="easyui-datebox" id="endScheduledStartDate" type="text" style="width:120px;" value="" data-options="validType:'checkDate',onChange:scheduledEndDate_Change"/>
                <span>&nbsp;作&nbsp;业&nbsp;号：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startWipEntityName", width = "150px", panelWidth = 500, queryParams = "'filterWipEntityStatus':true" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipEntitySelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endWipEntityName", width = "150px", panelWidth = 500, queryParams = "'filterWipEntityStatus':true" }); %>
				<span>&nbsp;供应类型：</span>
                <select id="wipSupplyType">
                    <option value="pushType">推式</option>
                    <option value="pullType">拉式</option>
                </select>
			</div>
            <div style="margin-bottom:5px;">
                <span>供应子库：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startSupplySubInventory", width = "120px" });%>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "endSupplySubInventory", width = "120px" });%>
                <span>&nbsp;作业号开头：</span>
				<input type="checkbox" id="jobStartNO_M" name="jobStartNO" value="M" onclick="changeJobStartNO(this);"/>M &nbsp;
                <input type="checkbox" id="jobStartNO_X" name="jobStartNO" value="X" onclick="changeJobStartNO(this);"/>X &nbsp;
				<input type="checkbox" id="jobStartNO_V" name="jobStartNO" value="V" onclick="changeJobStartNO(this);"/>V &nbsp;
                <input type="checkbox" id="jobStartNO_U" name="jobStartNO" value="U" onclick="changeJobStartNO(this);"/>U &nbsp;
				<input type="checkbox" id="jobStartNO_E" name="jobStartNO" value="E" onclick="changeJobStartNO(this);"/>E &nbsp;
                <input type="checkbox" id="jobStartNO_B" name="jobStartNO" value="B" onclick="changeJobStartNO(this);"/>B &nbsp;
				<input type="checkbox" id="jobStartNO_G" name="jobStartNO" value="G" onclick="changeJobStartNO(this);"/>G &nbsp;
                <input type="checkbox" id="jobStartNO_J" name="jobStartNO" value="J" onclick="changeJobStartNO(this);"/>J &nbsp;
				<input type="checkbox" id="jobStartNO_P" name="jobStartNO" value="P" onclick="changeJobStartNO(this);"/>P &nbsp;
                <input type="checkbox" id="jobStartNO_R" name="jobStartNO" value="R" onclick="changeJobStartNO(this);"/>R &nbsp;
				<input type="checkbox" id="jobStartNO_T" name="jobStartNO" value="T" onclick="changeJobStartNO(this);"/>T &nbsp;
                <input type="checkbox" id="jobStartNO_F" name="jobStartNO" value="F" onclick="changeJobStartNO(this);"/>F &nbsp;
				<input type="checkbox" id="jobStartNO_OTHER" name="jobStartNO" value="OTHER" onclick="changeJobStartNO(this);"/>其它 &nbsp;
				<input type="checkbox" id="jobStartNO_ALL" name="jobStartNO" value="ALL" onclick="changeJobStartNO(this);"/>全选 &nbsp;
            </div>
            <div style="margin-bottom:5px;">
                <span>物料编码：</span>
                <input class="easyui-validatebox" id="startMaterialDocNumber" type="text" data-options="validType:'checkLength[12]'" maxlength="12" style="width:116px;" />-<input class="easyui-validatebox" id="endMaterialDocNumber" type="text" data-options="validType:'checkLength[12]'" maxlength="12" style="width:116px;" />&nbsp;
                
                <input type="checkbox" id="job_N" name="jobN" value="N" />内机(N) &nbsp;
                <input type="checkbox" id="job_W" name="jobW" value="W" />外机(W) &nbsp;

                <span>&nbsp;计&nbsp;划&nbsp;组：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WipScheduleGroupSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "startScheduleGroup", width = "150px", onChange = "scheduleGroup_Change",multiple=true });%>
                
                &nbsp;<a class="easyui-linkbutton" iconCls="icon-search" href="javascript:void(0)" onclick="queryWipDiscreteJobs()">查询</a>
                <%if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
                <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="prepareByWipJobs()">作业备料</a>
                <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="prepareByWipRequire()">需求备料</a>
                <%} %>
                <iframe id="iframeExportToPdf" name="iframeExportToPdf" scrolling="auto" frameborder="0"  src="" style="width:0px;height:0px;display:none;"></iframe>
            </div>
        </div>
        <div id="wWmsPreparation" class="easyui-window" title="作业备料清单" style="padding: 0px;width:720px;height:480px;"
            data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="">
                    <div id="tabWmsPreparation" class="easyui-tabs" fit="true" 
                        data-options="border: false,
                                      tools: '#tb2',
                                      onSelect: function(title,index){
                                          if (index == 1) {
                                              getWmsPreparationDetailSum();
                                          }  
                                      }">
                        <div title="备料清单" id="tabWmsPreparationDetail" style="padding:0px;overflow:auto;background:#fafafa;">
                            <table id="dgWmsPreparationDetail" title="备料清单明细" class="easyui-datagrid" style="" border="false"
                                  data-options="fit: true,
                                                noheader: true,
                                                rownumbers: true,
                                                view: scrollview,
                                                idField: 'id',
                                                sortName: 'id',
                                                sortOrder: '',
                                                striped: true,
                                                toolbar: '#tb2_1',
                                                autoRowHeight: false,
                                                pageSize: 30,
                                                loadMsg: '数据加载中，请稍候...',
                                                onCheck: datagrid_Check,
                                                onUncheck: datagrid_Uncheck,
                                                onCheckAll: datagrid_CheckAll,
                                                onUncheckAll: datagrid_UncheckAll,
                                                onLoadSuccess: datagrid_LoadSuccess">
                                <thead data-options="frozen:true">
                                    <tr>
                                        <th data-options="field:'select',checkbox:true"></th>
                                        <th data-options="field:'id',hidden:'true'"></th>
                                        <th data-options="field:'wipEntityName',width:120,align:'left'">作业名称</th> 
                                        <th data-options="field:'materialDocNumber',width:85,align:'center'">物料编码</th>
                                    </tr>
                                </thead>
                                <thead>
                                    <tr>
                                        <th data-options="field:'materialName',width:180,align:'left'">物料描述</th>
                                        <th data-options="field:'materialUnit',width:60,align:'center'">物料单位</th>
                                        <th data-options="field:'warehouseId',width:70,align:'center'">供应子库</th>
                                        <th data-options="field:'planQuantity',width:60,align:'center'">需求数量</th>
                                        <th data-options="field:'onhandQuantity',width:60,align:'center'">库存数量</th>
                                        <th data-options="field:'locationDocNumber',width:60,align:'center'">货位</th>
                                        <th data-options="field:'dateRequired',width:110,align:'center'">需求日期</th>
                                    </tr>
                                </thead>
		                    </table>
                            <div id="tb2_1" style="height:24px;padding-top:5px;padding-left:10px;">
                                <span>物料备料形式：</span>
                                <%= Html.DropDownList("materialPrepareType", (IEnumerable<SelectListItem>)ViewData["listMaterialPrepareType"], new { style = "width:120px;" })%>
                            </div>
                        </div>
                        <div title="查看合计" id="tabWmsPreparationDetailSum" style="padding:0px;overflow:auto;background:#fafafa;">
                            <table id="dgWmsPreparationDetailSum" title="" class="easyui-datagrid" style="" border="false"
                                data-options="fit: true,
                                              view: scrollview,
                                              noheader: true,
                                              rownumbers: true,
                                              singleSelect : true,
                                              autoRowHeight: false,
                                              pageSize: 30,
                                              idField: 'materialId',
                                              striped: true,
                                              loadMsg: '数据加载中，请稍候...'">
                                <thead data-options="frozen:true">
                                    <tr>
                                        <th data-options="field:'materialId',hidden:true"></th>
                                        <th data-options="field:'materialDocNumber',width:90,align:'center'">物料编码</th>
                                        <th data-options="field:'materialName',width:260,align:'left'">物料描述</th>
                                    </tr>
                                </thead>
                                <thead>
                                    <tr>
                                        <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                                        <th data-options="field:'warehouseId',width:70,align:'center'">供应子库</th>
                                        <th data-options="field:'wipSupplyType',width:80,align:'center'">供应类型</th>
                                        <th data-options="field:'planQuantity',width:60,align:'center'">数量</th>
                                        <th data-options="field:'onhandQuantity',width:60,align:'center'">库存</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div id="tb2" style="height:auto;">
                        <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="createWmsPreparation()">确认</a>
                        <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="removeWmsPreparationDetail()">移除</a>
                    </div>
                </div>
            </div>
        </div>
        <div id="wMessager" class="easyui-dialog" title="提示..." style="width:400px;height:200px;padding:10px"
			data-options="
                closed: true,
                modal: true,
                shadow: true,
				buttons: [{
					text:'确定',
					iconCls:'icon-ok',
					handler:function(){
						$('#wMessager').dialog('close');
					}
				}]
			">
	    </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-scrollview.js"></script>
    <style type="text/css">
        .datagrid-header-rownumber,.datagrid-cell-rownumber{ width:40px; }
    </style>
    <%--作业查询--%>
    <script type="text/javascript">
        function changeJobStartNO(objChk) {
            if ($(objChk).is(':checked')) {
                
                if (objChk.id == "jobStartNO_ALL") {
                    $("[name='jobStartNO']").attr("checked", "true");
                } else if (objChk.id == "jobStartNO_OTHER") {
                    $("[name='jobStartNO']").removeAttr("checked");
                    $(objChk).attr("checked", "true");
                } else {
                    $("#jobStartNO_OTHER").removeAttr("checked");
                }

            } else {

                if (objChk.id == "jobStartNO_ALL") {
                    $("[name='jobStartNO']").removeAttr("checked");
                } 

            }
        }

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
        });
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
            //endScheduleGroupName = getScheduleGroupName("endScheduleGroup");

            var b_options = $('#startWipEntityName').combogrid('options');
            b_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            b_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            b_options.queryParams.startScheduleGroupName = startScheduleGroupName;
            b_options.queryParams.endScheduleGroupName = endScheduleGroupName;
            b_options.queryParams.isMultiGroupName = true;
            var b_g = $('#startWipEntityName').combogrid('grid');
            b_g.datagrid('reload');

            var e_options = $('#endWipEntityName').combogrid('options');
            e_options.queryParams.startScheduledStartDate = startScheduledStartDate;
            e_options.queryParams.endScheduledStartDate = endScheduledStartDate;
            e_options.queryParams.startScheduleGroupName = startScheduleGroupName;
            e_options.queryParams.endScheduleGroupName = endScheduleGroupName;
            e_options.queryParams.isMultiGroupName = true;
            var e_g = $('#endWipEntityName').combogrid('grid');
            e_g.datagrid('reload');
        }
        var wipSupplyType = "";
        var startSupplySubInventory = "", endSupplySubInventory = "";
        var startMaterialDocNumber = "", endMaterialDocNumber = "";
        var startScheduledStartDate = "", endScheduledStartDate = "";
        var startScheduleGroupName = "", endScheduleGroupName = "";
        var wipEntityType = "";//内机外机
        function queryWipDiscreteJobs() {
            if (!$('#startScheduledStartDate').datebox('isValid') || !$('#endScheduledStartDate').datebox('isValid')) {
                $.messager.alert("错误", "日期格式输入错误.", "error");
                return;
            }
            wipSupplyType = $('#wipSupplyType').val();
            //开始子库大小写通用
            startSupplySubInventory = getSubInventoryId("startSupplySubInventory");
            //结束子库大小写通用
            endSupplySubInventory = getSubInventoryId("endSupplySubInventory");
            //物料
            startMaterialDocNumber = $('#startMaterialDocNumber').val();
            endMaterialDocNumber = $('#endMaterialDocNumber').val();
            if ($('#job_N').is(':checked') && !$('#job_W').is(':checked')) {
                wipEntityType = '1';
            } else if ($('#job_W').is(':checked') && !$('#job_N').is(':checked')) {
                wipEntityType = '2';
            } else {
                wipEntityType = '0';
            }

            var options = $('#dgWipDiscreteJobs').datagrid('options');
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipDiscreteJobsList';
            options.queryParams.startScheduledStartDate = startScheduledStartDate;
            options.queryParams.endScheduledStartDate = endScheduledStartDate;
            //开始作业大小写通用
            options.queryParams.startWipEntitiesName = getWipEntityName("startWipEntityName");
            //结束作业大小写通用
            options.queryParams.endWipEntitiesName = getWipEntityName("endWipEntityName");
            options.queryParams.wipSupplyType = wipSupplyType;
            options.queryParams.startSupplySubInventory = startSupplySubInventory;
            options.queryParams.endSupplySubInventory = endSupplySubInventory;
            options.queryParams.startMaterialDocNumber = startMaterialDocNumber;
            options.queryParams.endMaterialDocNumber = endMaterialDocNumber;
            options.queryParams.startScheduleGroupName = startScheduleGroupName;
            options.queryParams.endScheduleGroupName = "";
            options.queryParams.wipEntityType = wipEntityType;

            //作业开头号
            var strJobStartNO = "";
            if ($("#jobStartNO_ALL").is(':checked')) {
                strJobStartNO = "ALL";
            } else if ($("#jobStartNO_OTHER").is(':checked')) {
                strJobStartNO = "OTHER";
            } else {
                $('input[name="jobStartNO"]:checked').each(function () {
                    if (strJobStartNO.length > 0) {
                        strJobStartNO += ",";
                    }
                    strJobStartNO += "'" + $(this).val() + "'";
                });
            }
            options.queryParams.jobStartNO = strJobStartNO;

            $('#dgWipDiscreteJobs').datagrid('reload');
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
        //        function getScheduleGroupName(cboId) {
        //            var scheduleGroupName = $('#' + cboId).combobox('getValue');
        //            var g = $('#' + cboId).combogrid('grid');
        //            var rows = g.datagrid('getRows');
        //            for (var i = 0; i < rows.length; i++) {
        //                if (rows[i].scheduleGroupId == scheduleGroupName || rows[i].scheduleGroupName.toUpperCase() == scheduleGroupName.toUpperCase()) {
        //                    scheduleGroupName = rows[i].scheduleGroupName;
        //                    break;
        //                }
        //            }
        //            return scheduleGroupName;
        //        }

        //获取计划组名称，大小写通用 
        function getScheduleGroupName(cboId) {
            var scheduleGroupName = "";
            var g = $('#' + cboId).combogrid('grid');
            var rows = g.datagrid('getSelections');

            for (var i = 0; i < rows.length; i++) {
                if (scheduleGroupName.length > 0) {
                    scheduleGroupName += ",";
                }

                var myScheduleGroupName = "";
                if (rows[i].scheduleGroupName) {
                    //myScheduleGroupName = rows[i].scheduleGroupName.replace("一", "1").replace("二", "2").replace("三", "3").replace("四", "4").replace("五", "5").replace("六", "6").replace("七", "7").replace("八", "8").replace("九", "9");
                    myScheduleGroupName = rows[i].scheduleGroupName;
                }

                if (myScheduleGroupName && myScheduleGroupName != "") {
                    scheduleGroupName += "'" + myScheduleGroupName + "'";
                }
            }

            return scheduleGroupName;
        }
    </script>
    <%--仓库备料--%>
    <script type="text/javascript">
        //物料备料形式
        $(function () {
            $('#materialPrepareType').change(function () {
                var checkRows = $('#dgWipDiscreteJobs').datagrid('getChecked');
                var wipEntityIds = getWipEntityId(checkRows);
                loadWmsPreparationDetail(wipEntityIds);
            })
        });
        var pattern = "";
        //作业备料
        function prepareByWipJobs() {
            pattern = '<%=AscmWmsPreparationMain.PatternDefine.wipJob %>';
            getWmsPreparationDetail();
        }
        //需求备料
        function prepareByWipRequire() {
            pattern = '<%=AscmWmsPreparationMain.PatternDefine.wipRequire %>';
            getWmsPreparationDetail();
        }
        function getWipEntityId(rows) {
            var wipEntityIds = "";
            $.each(rows, function (i, row) {
                if (wipEntityIds != "")
                    wipEntityIds += ",";
                wipEntityIds += row.wipEntityId;
            });
            return wipEntityIds;
        }
        function getWmsPreparationDetail() {
            var checkRows = $('#dgWipDiscreteJobs').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert('提示', '请先勾选作业', 'info');
                return;
            }
            var wipEntityIds = getWipEntityId(checkRows);
            $('#materialPrepareType').val("");
            $('#tabWmsPreparation').tabs('select', 0);
            $('#wWmsPreparation').window('open');
            $('#dgWmsPreparationDetail').datagrid('clearChecked');
            loadWmsPreparationDetail(wipEntityIds);
        }
        function loadWmsPreparationDetail(wipEntityIds) {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/GetWmsPreparationDetailList/',
                type: "post",
                dataType: "json",
                data: {
                    "wipEntityIds": wipEntityIds,
                    "wipSupplyType": wipSupplyType,
                    "startSupplySubInventory": startSupplySubInventory,
                    "endSupplySubInventory": endSupplySubInventory,
                    "startMaterialDocNumber": startMaterialDocNumber,
                    "endMaterialDocNumber": endMaterialDocNumber,
                    "materialPrepareType": $('#materialPrepareType').val()
                },
                beforeSend: function () {
                    var options = $('#dgWmsPreparationDetail').datagrid('options');
                    options.loadMsg = '数据加载中，请稍候...';
                    $('#dgWmsPreparationDetail').datagrid('loading');
                },
                success: function (r) {
                    $('#dgWmsPreparationDetail').datagrid('loaded');
                    if (r.result) {
                        $('#dgWmsPreparationDetail').datagrid('loadData', r.rows);
                    } else {
                        $.messager.alert('错误', '加载作业物料清单失败', 'error');
                    }
                }
            });
        }
        //物料合计
        function getWmsPreparationDetailSum() {
            var detailRows = $('#dgWmsPreparationDetail').datagrid('getRows');
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/GetWmsPreparationDetailSumList/',
                type: "post",
                dataType: "json",
                data: { "preparationDetailRows": $.toJSON(detailRows) },
                beforeSend: function () {
                    $('#dgWmsPreparationDetailSum').datagrid('loading');
                },
                success: function (r) {
                    $('#dgWmsPreparationDetailSum').datagrid('loaded');
                    if (r.result) {
                        $('#dgWmsPreparationDetailSum').datagrid('loadData', r.rows);
                    } else {
                        $.messager.alert('错误', '作业物料清单合计失败', 'error');
                    }
                }
            });
        }
        //生成备料单
        function createWmsPreparation() {
            var rows = $('#dgWmsPreparationDetail').datagrid('getRows');
            var checkRows = [];
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].select) {
                    checkRows.push(rows[i]);
                }
            }
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选备料清单！", "info");
                return;
            }
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CreateWmsPreparation/',
                type: "post",
                dataType: "json",
                data: { "pattern": pattern, "detailJson": $.toJSON(checkRows) },
                beforeSend: function () {
                    var options = $('#dgWmsPreparationDetail').datagrid('options');
                    options.loadMsg = '备料单生成中，请稍候...';
                    $('#dgWmsPreparationDetail').datagrid('loading');
                },
                success: function (r) {
                    $('#dgWmsPreparationDetail').datagrid('loaded');
                    if (r.result) {
                        if (pattern == '<%=AscmWmsPreparationMain.PatternDefine.wipJob %>') {
                            $('#wMessager').dialog({
                                content: r.message,
                                toolbar: [{
                                    text: '导出',
                                    iconCls: 'icon-print',
                                    handler: function () {
                                        ExportToPdf(false, r.id);
                                    }
                                }, {
                                    text: '单页导出',
                                    iconCls: 'icon-print',
                                    handler: function () {
                                        ExportToPdf(true, r.id);
                                    }
                                }]
                            }).dialog('open');
                        } else {
                            $('#wMessager').dialog({
                                content: r.message,
                                toolbar: [{
                                    text: '单页导出',
                                    iconCls: 'icon-print',
                                    handler: function () {
                                        ExportToPdf(true, r.id);
                                    }
                                }]
                            }).dialog('open');
                        }
                        $('#wWmsPreparation').window('close');
                    } else {
                        $.messager.alert('错误', '备料单生成失败', 'error');
                    }
                }
            });
        }
        //移除备料明细
        function removeWmsPreparationDetail() {
            var rows = $('#dgWmsPreparationDetail').datagrid('getRows');
            var checkRows = [];
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].select) {
                    checkRows.push(rows[i]);
                }
            }
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选要移除的备料清单！", "info");
                return;
            }
            $.messager.confirm('确认', '确认从备料清单中移除勾选的记录？', function (result) {
                if (result) {
                    var vArry = new Array();
                    var rows = $('#dgWmsPreparationDetail').datagrid('getRows');
                    if (result) {
                        var num = 0;
                        $.each(rows, function (index, rowsItem) {
                            var isAdd = true;
                            $.each(checkRows, function (i, item) {
                                if (item.id == rowsItem.id) {
                                    isAdd = false;
                                    return;
                                }
                            });
                            if (isAdd) {
                                vArry[num] = rowsItem;
                                num = num + 1;
                            }
                        });
                    }
                    $('#dgWmsPreparationDetail').datagrid('loadData', vArry);
                }
            })
        }
        function datagrid_Check(rowIndex, rowData) {
            rowData.select = true;
        }
        function datagrid_Uncheck(rowIndex, rowData) {
            rowData.select = false;
        }
        function datagrid_CheckAll(rows) {
            for (var i = 0; i < rows.length; i++) {
                rows[i].select = true;
            }
        }
        function datagrid_UncheckAll(rows) {
            for (var i = 0; i < rows.length; i++) {
                rows[i].select = false;
            }
        }
        function datagrid_LoadSuccess(data) {
            var rows = data.rows;
            for (i = 0; i < rows.length; i++) {
                if (rows[i].select) {
                    var index = $('#dgWmsPreparationDetail').datagrid('getRowIndex', rows[i]);
                    $('#dgWmsPreparationDetail').datagrid('checkRow', index);
                }
            }
        }
    </script>
    <%--导出--%>
    <script type="text/javascript">
        function ExportToPdf(bSingle, ids) {
            if(ids) {
                var url = "<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationExportToPdf?mainIdList="+ids+"&bSingle="+bSingle+"&pattern="+pattern;
                $('#iframeExportToPdf').attr("src",url).trigger("beforeload");
            }
        }
    </script>
</asp:Content>
