<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	任务排配规则管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:false;">
        <table id="dgRule" title="任务排配规则管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsAllocateTaskRuleList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              Edit();
                              <%} %>
                          },
                          onLoadSuccess: function(){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }                     
                          }">
                        <thead data-options="frozen:true">
                            <tr>
                                <%--<th data-options="checkbox:true"></th>--%>
                                <th data-options="field:'id',hidden:true">
                                </th>
                                <th data-options="field:'userWorker',width:80,align:'center'">
                                    领料员
                                </th>
                                <th data-options="field:'userZRanker',width:80,align:'center'">
                                    总装排产员
                                </th>
                                <th data-options="field:'userDRanker',width:80,align:'center'">
                                    电装排产员
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'ruleCode',width:240,align:'left'">
                                    排配规则
                                </th>
                                <th data-options="field:'other',width:150,align:'left'">
                                    手动任务
                                </th>
                                <th data-options="field:'tip1',width:60,align:'center'">
                                    描述
                                </th>
                                <th data-options="field:'taskCount',width:80,align:'center'">
                                    平衡值
                                </th>
                                <th data-options="field:'tip2',width:120,align:'center'">
                                    备注
                                </th>
                                <th data-options="field:'_createTime',width:120,align:'center'">
                                    创建时间
                                </th>
                                <th data-options="field:'_modifyTime',width:120,align:'center'">
                                    最后更新时间
                                </th>
                            </tr>
                        </thead>
                    </table>
        <div id="tb1">
            <% if (ynWebRight.rightAdd)
               { %>
            <span>所属班组：</span>
            <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewLogisticsClassSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "logisticsName", panelWidth = 500 }); %></span>
            <%} %>
            <span><input id="queryWord" type="text" style="width: 100px;" /></span>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="Query();">查询</a> 
            
            <% if (ynWebRight.rightAdd)
               { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                onclick="Add();">增加</a>
            <%} %>

            <% if (ynWebRight.rightEdit)
               { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                onclick="Edit();">修改</a>
            <%} %>

            <% if (ynWebRight.rightDelete)
               { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
                onclick="Delete();">删除</a>
            <%} %>

            <% if (ynWebRight.rightVerify)
               { %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                onclick="SetWorkerTaskCount();">重置平衡值</a>
            <%} %>
        </div>
        <div id="editRule" class="easyui-window" title="排配规则管理" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editRuleForm" method="post">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>领料员：</span>
                            </td>
                            <td style="width: 80%">
                                <span id="spanWorker"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerName", width = "150px", panelWidth = 500, queryParams = "'queryRole':'领料员'" }); %></span>
                                <input type="text" id="worker" style="width: 145px; background-color: #CCCCCC;"
                                    readonly="readonly" /><span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>总装排产员：</span>
                            </td>
                            <td>
                                <span id="spanZRanker"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "zRankerName", width = "150px", panelWidth = 500, queryParams = "'queryRole':'总装排产员'" }); %></span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>电装排产员：</span>
                            </td>
                            <td>
                                <span id="spanDRanker"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "dRankerName", width = "150px", panelWidth = 500, queryParams = "'queryRole':'电装排产员'" }); %></span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>排配规则：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="ruleCode" name="ruleCode" type="text"
                                    style="width: 300px;" />
								<a href="javascript:void(0);" id="btnEditRuleCode" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="编辑详细规则" onclick="btnEditRuleCode_click();"></a>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>手动任务：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="other" name="other" type="text"
                                    style="width: 300px;" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>描述：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="tip1" name="tip1" type="text"
                                    style="width: 300px;" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>备注：</span>
                            </td>
                            <td>
                                <textarea class="easyui-validatebox" id="tip2" name="tip2" rows="3"
                                    cols="342" style="width: 342px;"></textarea>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save()">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editRule').window('close');">
                        取消</a>
                </div>
            </div>
        </div>

		<div id="winEditRuleCode" class="easyui-window" title="排配详细规则编辑" style="padding: 5px;
            width: 560px; height: 440px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 0px; background: #fff; border: 1px solid #ccc;">
                    <table style="width: 520px;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 30px">
                            <td style="width: 70px; text-align: left;" nowrap>
                                排配种类：
                            </td>
                            <td style="width: 190px">
                                <select id="cmbRuleType" name="cmbRuleType" style="width: 150px;" class="easyui-combobox" 
								data-options="panelHeight:'auto', onSelect: cmbRuleType_select">
									<option value=""></option>
									<option value="allocate">须配料</option>
									<option value="prepare">须备料</option>
									<option value="special">特殊子库</option>
								</select>
                            </td>
                        </tr>

						<tr style="height: 30px">
                            <td style="width: 70px; text-align:left;" nowrap>
                                子库：
                            </td>
                            <td style="width: 190px">
                                <%Html.RenderPartial("~/Areas/Ascm/Views/Warehouse/WarehouseSelectCombo.ascx",
									  new MideaAscm.Code.SelectCombo { id = "cmbSubInventory", width = "150px" });%>
                            </td>

							<td style="width: 70px; text-align: left;" nowrap>
                                物料：
                            </td>
                            <td style="width: 190px">
                                <%Html.RenderPartial("~/Areas/Ascm/Views/Public/MaterialItemSelectCombo.ascx",
									  new MideaAscm.Code.SelectCombo { id = "cmbMateriel", width = "150px" }); %>
								<a id="A2" class="easyui-linkbutton" iconcls="icon-save" plain="true"
								href="javascript:void(0)" onclick="SaveMaterielNO()"></a>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 80px; height: 5px;text-align: right;">
                                &nbsp;
                            </td>
                        </tr>

						<tr>
                            <td style="width: 260px;text-align: right; vertical-align:top" colspan="2" nowrap>
                                <table id="dgWarehouse" style="" border="true" singleSelect="true"
									idField="warehouseCode" striped="true" toolbar="#toolbar_dgWarehouse">
								</table>
								<div id="toolbar_dgWarehouse">
									<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="dgSub_Delete('#dgWarehouse');">删除</a>
								</div>
                            </td>
							<td style="width: 260px;text-align: right; vertical-align:top" colspan="2" nowrap>
                                <table id="dgMaterial" style="" border="true" singleSelect="true"
									idField="materialCode" striped="true" toolbar="#toolbar_dgMaterial">
								</table>
								<div id="toolbar_dgMaterial">
									<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="dgSub_Delete('#dgMaterial');">删除</a>
								</div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="A1" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="SaveEditRuleCode()">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#winEditRuleCode').window('close');">
                        取消
					</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
<script type="text/javascript">
    var currentId = null;
    function Query() {
        var options = $('#dgRule').datagrid('options').queryParams;
        options.queryWord = $('#queryWord').val();
        <% if (ynWebRight.rightAdd)
          { %>
        options.queryLogisticsClass = $('#logisticsName').combogrid('getValue');
        <%} %>

        $('#dgRule').datagrid('reload');
    }
</script>

<script type="text/javascript">
    var ruleEntity = {};

    $(function ()
    {
        $('#dgWarehouse').datagrid({
            rownumbers: true,
            width: 250,
            height: 270,
            columns: [[
                { field: 'warehouseCode', title: '子库代码', width: 100, align: 'left' }
            ]],
            onSelect: function (rowIndex, rowData)
            {
                deleteAllRows("#dgMaterial");
                var warehouse = rowData.warehouseCode;
                if (!warehouse) return;

                var ruleType = $('#cmbRuleType').combobox("getValue");
                if (!ruleType) return;

                if (ruleEntity[ruleType] && ruleEntity[ruleType][warehouse])
                {
                    var entityMaterial = ruleEntity[ruleType][warehouse];
                    for (var material in entityMaterial)
                    {
                        $("#dgMaterial").datagrid("appendRow", { materialCode: material });
                    }
                }
            }
        });

        $('#dgMaterial').datagrid({
            rownumbers: true,
            width: 250,
            height: 270,
            columns: [[
                { field: 'materialCode', title: '物料编号', width: 150, align: 'left' }
            ]],
            onSelect: function (rowIndex, rowData)
            {
                $('#cmbMateriel').combogrid('setText', rowData.materialCode);
            }
        });

        $('#cmbMateriel').combogrid({ idField: 'docNumber' });

        $('#cmbSubInventory').combogrid({ onSelect: cmbSubInventory_select });
        $('#cmbMateriel').combogrid({ onSelect: cmbMateriel_select });
    });

    function dgSub_Delete(ctrlID)
    {
        var row = $(ctrlID).datagrid('getSelected');
        if (!row) return;
        var rowIndex = $(ctrlID).datagrid('getRowIndex', row);

        var ruleType = $('#cmbRuleType').combobox("getValue");
        if (!ruleType || ruleType.toString().length == 0)
        {
            alert("请选择【排配种类】！");
            return;
        }

        var warehouse = null;
        var rowWarehouse = $('#dgWarehouse').datagrid("getSelected");
        if (rowWarehouse) warehouse = rowWarehouse.warehouseCode;
        if (!warehouse)
        {
            alert("请选择【子库】！");
            return;
        }

        if (ctrlID == "#dgMaterial")
        {
            var materiel = row.materialCode;
            delete ruleEntity[ruleType][warehouse][materiel];
        }
        else
        {
            delete ruleEntity[ruleType][warehouse];
            deleteAllRows("#dgMaterial");
        }

        $(ctrlID).datagrid('deleteRow', rowIndex);
    }

    function cmbSubInventory_select(selIndex, selRow)
    {
        //$('#cmbMateriel').combogrid({ queryParams: { subInventory: newValue} });
        if (!selRow || !selRow.id) return;

        var newValue = selRow.id.toString();
        var warehouse = newValue;
        if (newValue && newValue.length >= 4)
        {
            warehouse = newValue.substr(0, 4);
        }

        var ruleType = $('#cmbRuleType').combobox("getValue");
        if (!ruleType || ruleType.toString().length == 0)
        {
            alert("请选择【排配种类】！");
            $('#cmbSubInventory').combogrid("clear");
            return;
        }

        if (ruleEntity[ruleType] && ruleEntity[ruleType][warehouse])
        {
            alert("【子库】已经存在！");
            return;
        }

        $("#dgWarehouse").datagrid("appendRow", { warehouseCode: warehouse });
        ruleEntity[ruleType][warehouse] = {};
    }

    function cmbMateriel_select(selIndex, selRow)
    {
        if (!selRow || !selRow.docNumber) return;

        var materiel = selRow.docNumber.toString();
        var ruleType = $('#cmbRuleType').combobox("getValue");
        if (!ruleType || ruleType.toString().length == 0)
        {
            alert("请选择【排配种类】！");
            $('#cmbMateriel').combogrid("clear");
            return;
        }

        var warehouse = null;
        var row = $('#dgWarehouse').datagrid("getSelected");
        if (row) warehouse = row.warehouseCode;
        if (!warehouse)
        {
            alert("请选择【子库】！");
            $('#cmbMateriel').combogrid("clear");
            return;
        }

        if (ruleEntity[ruleType] && ruleEntity[ruleType][warehouse] && ruleEntity[ruleType][warehouse][materiel])
        {
            alert("【物料】已经存在！");
            return;
        }

        $("#dgMaterial").datagrid("appendRow", { materialCode: materiel });
        ruleEntity[ruleType][warehouse][materiel] = "rc";
    }

    function cmbRuleType_select(record)
    {
        deleteAllRows("#dgWarehouse");
        deleteAllRows("#dgMaterial");

        var ruleType = record.value;
        if (!ruleType || ruleType.toString().length == 0) return;

        var entityWarehouse = ruleEntity[ruleType];
        if (entityWarehouse)
        {
            for (var warehouse in entityWarehouse)
            {
                $("#dgWarehouse").datagrid("appendRow", { warehouseCode: warehouse });
            }
        }
    }

    function deleteAllRows(ctrlID)
    {
        var rows = $(ctrlID).datagrid("getRows");
        if (rows && rows.length > 0)
        {
            for (var i = rows.length - 1; i >= 0; i--)
            {
                $(ctrlID).datagrid("deleteRow", i);
            }
        }
    }

    function SaveMaterielNO()
    {
        var ruleType = $('#cmbRuleType').combobox("getValue");
        if (!ruleType || ruleType.toString().length == 0)
        {
            alert("请选择【排配种类】！");
            return;
        }

        var warehouse = null;
        var row = $('#dgWarehouse').datagrid("getSelected");
        if (row) warehouse = row.warehouseCode;
        if (!warehouse)
        {
            alert("请选择【子库】！");
            return;
        }

        var material = null;
        var row = $('#dgMaterial').datagrid("getSelected");
        var rowIndex = $('#dgMaterial').datagrid("getRowIndex", row);
        if (row && row.materialCode)
        {
            material = row.materialCode;
        }
        if (!material)
        {
            alert("请选择列表中的【物料】！");
            return;
        }

        var newMaterial = $('#cmbMateriel').combogrid('getText');
        if (newMaterial) newMaterial = newMaterial.replace(" ", "");
        if (!newMaterial || newMaterial.length == 0)
        {
            alert("【物料】为空！");
            return;
        }

        if (ruleEntity[ruleType] && ruleEntity[ruleType][warehouse] && ruleEntity[ruleType][warehouse][material])
        {
            delete ruleEntity[ruleType][warehouse][material];
        }

        ruleEntity[ruleType][warehouse][newMaterial] = "rc";
        $('#dgMaterial').datagrid('updateRow', {
            index: rowIndex,
            row: { materialCode: newMaterial }
        });

        alert("已保存！");
    }

    function SaveEditRuleCode()
    {
        var textRule = "";
        var objRule = {};
        for (var ruleType in ruleEntity)
        {
            var arrWarehouse = [];
            var arrWarehouseMateriel = [];
            for (var warehouse in ruleEntity[ruleType])
            {
                arrWarehouse.push(warehouse);

                var arrMateriel = [];
                for (var materiel in ruleEntity[ruleType][warehouse])
                {
                    arrMateriel.push(materiel);
                }
                if (arrMateriel.length > 0)
                {
                    arrWarehouseMateriel.push(warehouse + ":" + arrMateriel.join("%"));
                }
            }

            var textSubRule = "(" + arrWarehouse.join("|") + ")(" + arrWarehouseMateriel.join("|") + ")";

            if (ruleType == "allocate") objRule.textAllocate = textSubRule;
            else if (ruleType == "prepare") objRule.textPrepare = textSubRule;
            else if (ruleType == "special") objRule.textSpecial = textSubRule;
        }

        textRule = "[须配料" + objRule.textAllocate + "]&[须备料" + objRule.textPrepare + "]&[" + objRule.textSpecial + "]";

        $('#ruleCode').val(textRule);
        $('#winEditRuleCode').window('close');
    }

    function btnEditRuleCode_click()
    {
        deleteAllRows("#dgWarehouse");
        deleteAllRows("#dgMaterial");
        $('#cmbRuleType').combobox("clear");
        $('#cmbSubInventory').combogrid("clear");
        $('#cmbMateriel').combogrid("clear");

        setRuleEntity($('#ruleCode').val());
        $('#winEditRuleCode').window('open');
    }

    function setRuleEntity(strRuleCode)
    {
        ruleEntity = {};
        if (!strRuleCode) return;

        var strCodes = strRuleCode.split('&');
        for (var i = 0; i < strCodes.length; i++)
        {
            var strCode = strCodes[i];
            var rule_TYPE = getRuleType(strCode);
            var rule_PREFIX = getRuleCodePrefix(strCode);

            ruleEntity[rule_TYPE] = {};
            var strSubCodes = strCode.replace(rule_PREFIX, "").replace(")]", "").split(")(");
            if (strSubCodes && strSubCodes.length == 2)
            {
                var strWarehouses = strSubCodes[0].split('|');
                if (strWarehouses && strWarehouses.length > 0)
                {
                    for (var j = 0; j < strWarehouses.length; j++)
                    {
                        ruleEntity[rule_TYPE][strWarehouses[j]] = {};
                    }

                    var strWHMateriels = strSubCodes[1].split('|');
                    if (strWHMateriels && strWHMateriels.length > 0)
                    {
                        for (var k = 0; k < strWHMateriels.length; k++)
                        {
                            var strSubWHMateriels = strWHMateriels[k].split(':');
                            if (strSubWHMateriels && strSubWHMateriels.length == 2)
                            {
                                var subWHName = strSubWHMateriels[0];
                                var subMateriels = strSubWHMateriels[1].split('%');
                                if (subMateriels && subMateriels.length > 0)
                                {
                                    for (var m = 0; m < subMateriels.length; m++)
                                    {
                                        if (ruleEntity[rule_TYPE][subWHName])
                                        {
                                            ruleEntity[rule_TYPE][subWHName][subMateriels[m]] = "rc";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    function getRuleType(strSubCode)
    {
        if (strSubCode.indexOf("[须配料(") == 0) return "allocate";
        else if (strSubCode.indexOf("[须备料(") == 0) return "prepare";
        else if (strSubCode.indexOf("[(") == 0) return "special";
        else return "";
    }

    function getRuleCodePrefix(strSubCode)
    {
        if (strSubCode.indexOf("[须配料(") == 0) return "[须配料(";
        else if (strSubCode.indexOf("[须备料(") == 0) return "[须备料(";
        else if (strSubCode.indexOf("[(") == 0) return "[(";
        else return "";
    }
</script>

<script type="text/javascript">
    function Add()
    {
        $('#editRule').window('open');
        $("#editRuleForm")[0].reset();

        $('#spanWorker').show();
        $('#worker').hide();
        $('#ruleCode').focus();
        currentId = null;
        $('#zRankerName').combogrid('clear');
        $('#dRankerName').combogrid('clear');
        $('#workerName').combogrid('clear');
    }
</script>

<script type="text/javascript">
    function Edit()
    {
        var selectRow = $('#dgRule').datagrid('getSelected');
        if (selectRow)
        {
            $('#editRule').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsAllocateTaskRuleEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r)
                {
                    if (r)
                    {
                        $("#editRuleForm")[0].reset();
                        $('#spanWorker').hide();
                        $('#worker').show();
                        $('#worker').val(r.userWorker);
                        $('#zRankerName').combogrid('setValue', r.userZRanker);
                        $('#dRankerName').combogrid('setValue', r.userDRanker);
                        $('#ruleCode').val(r.ruleCode);
                        $('#other').val(r.other);
                        $('#tip1').val(r.tip1);
                        $('#tip2').val(r.tip2);

                        $('#ruleCode').focus();
                    }
                }
            });
            currentId = selectRow.id;
        } else
        {
            $.messager.alert('提示', '请填写必填项', 'info');
        }
    }

    function Save()
    {
        $.messager.confirm("确认", "确认提交保存?", function (r)
        {
            if (r)
            {
                $('#editRuleForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsAllocateTaskRuleSave/' + currentId,
                    onSubmit: function ()
                    {
                        return $('#editRuleForm').form('validate');
                    },
                    success: function (r)
                    {
                        var rVal = $.parseJSON(r);
                        if (rVal.result)
                        {
                            $('#editRule').window('close');
                            currentId = rVal.id;
                            Query();
                        } else
                        {
                            $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                        }
                    }
                });
            }
        });
    }
</script>

<script type="text/javascript">
    function Delete()
    {
        var selectRow = $('#dgRule').datagrid('getSelected');
        if (selectRow)
        {
            $.messager.confirm('确认', '确认删除该领料员的领料排配规则[<font color="red">' + selectRow.userWorker + '</font>]？', function (result)
            {
                if (result)
                {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsAllocateTaskRuleDelete/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r)
                        {
                            if (r.result)
                            {
                                Query();
                            } else
                            {
                                $.messager.alert('确认', '删除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        } else
        {
            $.messager.alert('提示', '请选择要删除的领料排配规则', 'info');
        }
    }
</script>

<script type="text/javascript">
    function SetWorkerTaskCount()
    {
        var selectRow = $('#dgRule').datagrid('getSelected');
        if (selectRow)
        {
            $.messager.confirm('确认', '确认须重置的领料员[<font color="red">' + selectRow.userWorker + '</font>]的平衡值？', function (result)
            {
                if (result)
                {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsAllocateTaskRuleCount/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r)
                        {
                            if (r.result)
                            {
                                Query();
                            } else
                            {
                                $.messager.alert('确认', '重置失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        } else
        {
            $.messager.alert('提示', '请选择须重置平衡值的领料员', 'info');
        }
    }
</script>
</asp:Content>
