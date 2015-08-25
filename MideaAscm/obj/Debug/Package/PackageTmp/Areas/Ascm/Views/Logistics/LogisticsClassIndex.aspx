<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物流组信息管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:false;">
        <table id="dgLogisticsClass" title="物流组信息管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                                      rownumbers: true,
                                      singleSelect : true,
                                      <%--checkOnSelect: false,
                                      selectOnCheck: true,--%>
                                      idField: 'id',
                                      sortName: 'id',
                                      sortOrder: 'asc',
                                      striped: true,
                                      toolbar: '#tb1',
                                      pagination: true,
                                      pageSize: 50,
                                      loadMsg: '更新数据......',
                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsClassList',
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
                                <th data-options="field:'logisticsName',width:70,align:'center'">
                                    归属主体
                                </th>
                                <th data-options="field:'_monitorLeader',width:70,align:'center'">
                                    班长
                                </th>
                                <th data-options="field:'_groupLeader',width:60,align:'center'">
                                    组长
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'createUser',width:80,align:'center'">
                                    创建人
                                </th>
                                <th data-options="field:'_createTime',width:120,align:'left'">
                                    创建时间
                                </th>
                                <th data-options="field:'modifyUser',width:120,align:'center'">
                                    最后更新人
                                </th>
                                <th data-options="field:'_modifyTime',width:120,align:'center'">
                                    最后更新时间
                                </th>
                            </tr>
                        </thead>
                    </table>
        <div id="tb1" style="padding:5px;height:auto;">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                onclick="Query();">刷新</a>
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
        <div id="editLogisticsClass" class="easyui-window" title="车间信息管理" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editLogisticsClassForm" method="post" style="">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>归属主体：</span>
                            </td>
                            <td>
                                <span id="spanLogisticsClass"><input type="text" id="logisticsName" name="logisticsName" style="width: 110px;" /></span>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>物流班长：</span>
                            </td>
                            <td>
                                <span id="spanMonitor"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewMonitorUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "monitorLeader" }); %></span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>物流组长：</span>
                            </td>
                            <td>
                                <span id="spanGrouper"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewGrouperUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "groupLeader" }); %></span>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                        { %>
                    <a id="A2" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save();">保存</a>
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
        var currentId = null;
        function Query() {
            $('#dgLogisticsClass').datagrid('reload');
        }

        function Add() {
            $('#editLogisticsClass').window('open');
            $("#editLogisticsClassForm")[0].reset();

            currentId = null;
            $('#monitorLeader').combogrid('clear');
            $('#groupLeader').combogrid('clear');
        }
        function Edit() {
            var selectRow = $('#dgLogisticsClass').datagrid('getSelected');
            if (selectRow) {
                $('#editLogisticsClass').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsClassEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editLogisticsClassForm")[0].reset();
                            $('#logisticsName').val(r.logisticsName);
                            $('#groupLeader').combogrid('setValue', r._groupLeader);
                            $('#monitorLeader').combogrid('setValue', r._monitorLeader);
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
                    $('#editLogisticsClassForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsClassSave/' + currentId,
                        onSubmit: function () {
                            return $('#editLogisticsClassForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editLogisticsClass').window('close');
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
            var selectRow = $('#dgLogisticsClass').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除该车间信息[<font color="red">' + selectRow.logisticsName + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/LogisticsClassDelete/' + selectRow.id;
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
                $.messager.alert('提示', '请选择删除的车间', 'info');
            }
        }
    </script>
</asp:Content>
