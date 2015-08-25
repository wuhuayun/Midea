﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	排配规则管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:false;">
        <table id="dgRule" title="排配规则管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/AllocateRuleList',
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
                                <th data-options="field:'worker',width:80,align:'center'">
                                    领料员
                                </th>
                                <th data-options="field:'zRanker',width:80,align:'center'">
                                    总装排产员
                                </th>
                                <th data-options="field:'dRanker',width:80,align:'center'">
                                    电装排产员
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'logisticsClassName',width:80,align:'center'">
                                    所属车间
                                </th>
                                <th data-options="field:'ruleCode',width:240,align:'left'">
                                    排配规则
                                </th>
                                <th data-options="field:'other',width:150,align:'left'">
                                    手动任务
                                </th>
                                <th data-options="field:'tip1',width:60,align:'center'">
                                    描述
                                </th>
                                <th data-options="field:'count',width:80,align:'center'">
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
            <span>领料员：</span>
            <span><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerName" }); %></span>
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
                                <span id="spanWorker"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerName" }); %></span>
                                <input type="text" id="worker" style="width: 108px; background-color: #CCCCCC;"
                                    readonly="readonly" /><span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>总装排产员：</span>
                            </td>
                            <td>
                                <span id="spanZRanker"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewZUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "zRankerName" }); %></span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>电装排产员：</span>
                            </td>
                            <td>
                                <span id="spanDRanker"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewDUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "dRankerName" }); %></span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>排配规则：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="ruleCode" name="ruleCode" type="text"
                                    style="width: 300px;" />
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<script type="text/javascript">
    var currentId = null;
    function Query() {
        var options = $('#dgRule').datagrid('options');
        options.queryParams.queryWord = $('#workerName').combogrid('getValue');
        $('#dgRule').datagrid('reload');
    }
    function Add() {
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
    function Edit() {
        var selectRow = $('#dgRule').datagrid('getSelected');
        if (selectRow) {
            $('#editRule').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/AllocateRuleEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#editRuleForm")[0].reset();
                        $('#spanWorker').hide();
                        $('#worker').show();
                        $('#worker').val(r.worker);
                        $('#zRankerName').combogrid('setValue', r.zRanker);
                        $('#dRankerName').combogrid('setValue', r.dRanker);
                        $('#ruleCode').val(r.ruleCode);
                        $('#other').val(r.other);
                        $('#tip1').val(r.tip1);
                        $('#tip2').val(r.tip2);

                        $('#ruleCode').focus();
                    }
                }
            });
            currentId = selectRow.id;
        } else {
            $.messager.alert('提示', '请填写必填项', 'info');
        }
    }
    function Save() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editRuleForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/AllocateRuleSave/' + currentId,
                    onSubmit: function () {
                        return $('#editRuleForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editRule').window('close');
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
    function Delete() {
        var selectRow = $('#dgRule').datagrid('getSelected');
        if (selectRow) {
            $.messager.confirm('确认', '确认删除该领料员的领料排配规则[<font color="red">' + selectRow.id + '</font>]？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/AllocateRuleDelete/' + selectRow.id;
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
            $.messager.alert('提示', '请选择要删除的领料排配规则', 'info');
        }
    }
    </script>
</asp:Content>
