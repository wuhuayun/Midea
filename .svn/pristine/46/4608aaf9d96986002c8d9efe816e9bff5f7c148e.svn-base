<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物流车辆管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:false;">
        <table id="dgForklift" title="领料车辆管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsForkliftList',
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
                                <th data-options="field:'assetsId',width:80,align:'center'">
                                    资产号
                                </th>
                                <th data-options="field:'tagId',width:80,align:'center'">
                                    标签号
                                </th>
                                <th data-options="field:'forkliftNumber',width:80,align:'center'">
                                    车辆号码
                                </th>
                                <th data-options="field:'workerName',width:70,align:'center'">
                                    责任人
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'_forkliftType',width:120,align:'center'">
                                    车种
                                </th>
                                <th data-options="field:'_forkliftSpec',width:60,align:'center'">
                                    车辆规格
                                </th>
                                <th data-options="field:'workContent',width:120,align:'center'">
                                    作业内容
                                </th>
                                <th data-options="field:'forkliftWay',width:120,align:'center'">
                                    领料路线
                                </th>
                                <th data-options="field:'actionLimits',width:120,align:'center'">
                                    允许活动范围
                                </th>
                                <th data-options="field:'logisticsClassName',width:80,align:'center'">
                                    归属主体
                                </th>
                                <th data-options="field:'_status',width:60,align:'center'">
                                    状态
                                </th>
                                <th data-options="field:'tip',width:160,align:'left'">
                                    备注
                                </th>
                                <th data-options="field:'createUser',width:80,align:'center'">
                                    创建人
                                </th>
                                <th data-options="field:'_createTime',width:120,align:'center'">
                                    创建时间
                                </th>
                                <th data-options="field:'modifyUser',width:100,align:'center'">
                                    最后更新人
                                </th>
                                <th data-options="field:'_modifyTime',width:120,align:'center'">
                                    最后更新时间
                                </th>
                            </tr>
                        </thead>
                    </table>
        <div id="tb1" style="padding:5px;height:auto;">
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
                        
            <% if (ynWebRight.rightVerify){ %>
                <span>所属班组：</span>
                <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewLogisticsClassSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "logisticsName", panelWidth = 500 }); %></span>
            <%} %>
            <span>车种:</span>
            <select id="queryType" name="queryType" style="width: 130px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                <option value=""></option>
                <% List<string> listTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.GetList(); %>
                <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                    { %>
                <% foreach (string statusDefine in listTypeDefine)
                    { %>
                <option value="<%=statusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.DisplayText(statusDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <input id="search_forklift" type="text" style="width: 100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="Query();">查询</a>
        </div>
        <div id="editForklift" class="easyui-window" title="领料车辆管理" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editForkliftForm" method="post" style="">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="width: 10%; text-align: right;" nowrap>
                                <span>资产号：</span>
                            </td>
                            <td style="width: 25%">
                                <input class="easyui-validatebox" required="true" id="assetsId" name="assetsId" type="text"
                                    style="width: 109px;" /><span style="color: Red;">*</span>
                            </td>
                            <td style="width: 17%; text-align: right;" nowrap>
                                <span>车辆号码：</span>
                            </td>
                            <td style="width: 35%">
                                <input class="easyui-validatebox" id="forkliftNumber" name="forkliftNumber" type="text"
                                    style="width: 110px;" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>标签号：</span>
                            </td>
                            <td>
                                <span id="spanTagId"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewForkliftRfidSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "tagId" }); %></span>
                                <input type="text" id="tag" style="width: 110px; background-color: #CCCCCC;"
                                    readonly="readonly" /><span style="color: Red;">*</span>
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>车种：</span>
                            </td>
                            <td>
                                <select id="forkliftType" name="forkliftType" style="width: 115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <% List<string> listBindTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.GetList(); %>
                                    <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                        { %>
                                    <% foreach (string bindTypeDefine in listBindTypeDefine)
                                        { %>
                                    <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.DisplayText(bindTypeDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>车辆规格：</span>
                            </td>
                            <td>
                                <select id="forkliftSpec" name="forkliftSpec" style="width: 115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <% listBindTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftSpecDefine.GetList(); %>
                                    <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                        { %>
                                    <% foreach (string bindTypeDefine in listBindTypeDefine)
                                        { %>
                                    <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftSpecDefine.DisplayText(bindTypeDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>归属主体：</span>
                            </td>
                            <td>
                                <%--<select id="sLogisticsClass" name="LogisticsClass" style="width: 115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <option value=""></option>
                                    <% listBindTypeDefine = MideaAscm.Services.GetMaterialManage.AscmCommonHelperService.GetInstance().GetLogisticsClassRelfectList(); %>
                                    <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                        { %>
                                    <% foreach (string bindTypeDefine in listBindTypeDefine)
                                        { %>
                                    <option value="<%=bindTypeDefine %>">
                                        <%=MideaAscm.Services.GetMaterialManage.AscmCommonHelperService.GetInstance().DisplayLogisticsClass(bindTypeDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>--%>
                                <span><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewLogisticsClassSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "logisticsClass", panelWidth = 500 }); %></span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>责任人：</span>
                            </td>
                            <td>
                                <span id="spanWorker"><%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerId", width = "115px", panelWidth = 500, queryParams = "'queryRole':'领料员'" }); %></span>
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span> 状态：</span>
                            </td>
                            <td>
                                <select id="sStatus" name="Status" style="width: 115px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                                    <% listBindTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.StatusDefine.GetList(); %>
                                    <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                        { %>
                                    <% foreach (string bindTypeDefine in listBindTypeDefine)
                                        { %>
                                    <option value="<%=bindTypeDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.StatusDefine.DisplayText(bindTypeDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>作业内容：</span>
                            </td>
                            <td colspan="3">
                                <textarea class="easyui-validatebox" id="workContent" name="workContent" rows="3" cols="342"
                                    style="width:342px;"></textarea>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>领料路线：</span>
                            </td>
                            <td colspan="3">
                                <textarea class="easyui-validatebox" id="forkliftWay" name="forkliftWay" rows="3" cols="342"
                                    style="width:342px;"></textarea>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>活动范围：</span>
                            </td>
                            <td colspan="3">
                                <input class="easyui-validatebox" id="actionLimits" name="actionLimits" type="text"
                                    style="width: 342px;" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>备注：</span>
                            </td>
                            <td colspan="3">
                                <textarea class="easyui-validatebox" id="tip" name="tip" rows="3" cols="342"
                                    style="width:342px;"></textarea>
                            </td>
                        </tr>

                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                        { %>
                    <a id="A1" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save()">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editForklift').window('close');">
                        取消</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
    function Query() {
        var options = $('#dgForklift').datagrid('options').queryParams;

        <% if (ynWebRight.rightVerify){ %>
               options.queryLogisticsClass = $('#logisticsName').combogrid('getValue');
        <%} %>

        options.queryType = $('#queryType').val();
        options.queryWord = $('#search_forklift').val();
        $('#dgForklift').datagrid('reload');
    }
    </script>
    <script type="text/javascript">
    function Add() {
        $('#spanTagId').show();
        $('#tag').hide();

        $('#editForklift').window('open');
        $("#editForkliftForm")[0].reset();

        $('#assetsId').focus();
        currentId = null;
    }
    </script>
    <script type="text/javascript">
    function Edit() {
        var selectRow = $('#dgForklift').datagrid('getSelected');
        if (selectRow) {
            $('#editForklift').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsForkliftEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#editForkliftForm")[0].reset();

                        $('#assetsId').val(r.assetsId);
                        $('#forkliftNumber').val(r.forkliftNumber);
                        $('#spanTagId').hide();
                        $('#tag').show();
                        $('#tag').val(r.tag);
                        $('#forkliftType').combobox('setValue', r.forkliftType);
                        $('#forkliftSpec').combobox('setValue', r.forkliftSpec);
                        $('#logisticsClass').combogrid('setValue', r.logisticsClassName);
                        $('#workerId').combogrid('setValue', r.workerName)
                        $('#sStatus').combobox('setValue', r._status);
                        $('#workContent').val(r.workContent);
                        $('#forkliftWay').val(r.forkliftWay);
                        $('#actionLimits').val(r.actionLimits);
                        $('#tip').val(r.tip);

                        $('#assetsId').focus();
                    }
                }
            });
            currentId = selectRow.id;
        } else {
            $.messager.alert('提示', '请选择领料车辆信息', 'info');
        }
    }
    </script>
    <script type="text/javascript">
    function Save() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editForkliftForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsForkliftSave/' + currentId,
                    onSubmit: function () {
                        return $('#editForkliftForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editForklift').window('close');
                            currentId = rVal.id;
                            Query();
                        } else {
                            $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                        }
                    }
                });
            }
        });
    }
    </script>
    <script type="text/javascript">
    function Delete() {
        var selectRow = $('#dgForklift').datagrid('getSelected');
        if (selectRow) {
            $.messager.confirm('确认', '确认删除领料车辆信息[<font color="red">' + selectRow.assetsId + '</font>]？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsForkliftDelete/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                Query();
                            } else {
                                $.messager.alert('确认', '删除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('提示', '请选择要删除的领料车辆信息', 'info');
        }
    }
    </script>
</asp:Content>
