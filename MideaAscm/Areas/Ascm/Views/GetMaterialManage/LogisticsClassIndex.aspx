<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	车间信息管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--车间信息-->
    <div region="west" split="false" border="false" title="" style="width:450px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
                    <table id="dgLogisticsClassInfo" title="车间信息管理" class="easyui-datagrid" style="" border="false"
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
                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsClassInfoList',
                                      onSelect: function(rowIndex, rowRec){
                                          currentId = rowRec.id;
                                          LoadLogisticsWorker();
                                          GetLogisticsName();
                                      },
                                      onDblClickRow: function(rowIndex, rowRec){
                                          <% if (ynWebRight.rightEdit){ %>
                                          Edit_Class();
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
                                <th data-options="field:'createUser',width:120,align:'center'">
                                    创建人
                                </th>
                                <th data-options="field:'_createTime',width:150,align:'left'">
                                    创建时间
                                </th>
                                <th data-options="field:'modifyUser',width:150,align:'center'">
                                    最后更新人
                                </th>
                                <th data-options="field:'_modifyTime',width:80,align:'center'">
                                    最后更新时间
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb1" style="padding:5px;height:auto;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                            onclick="Query_Class();">刷新</a>
                        <% if (ynWebRight.rightAdd)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                            onclick="Add_Class();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                            onclick="Edit_Class();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
                            onclick="Delete_Class();">删除</a>
                        <%} %>
                    </div>
                    <div id="editLogisticsClassInfo" class="easyui-window" title="车间信息管理" style="padding: 10px;
                        width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
                        minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                                <form id="editLogisticsClassInfoForm" method="post" style="">
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
                                    onclick="Save_Class();">保存</a>
                                <%} %>
                                <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editForklift').window('close');">
                                    取消</a>
                            </div>
                        </div>
                    </div>
			    </div>
			</div>
		</div>
	</div>
    <!--车间管理领料员-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px; overflow:false;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:false;">
                    <table id="dgWorker" title="领料员管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb2',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          <%--url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/AllocateRuleList',--%>
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
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
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'zRanker',width:80,align:'center'">
                                    总装排产员
                                </th>
                                <th data-options="field:'dRanker',width:80,align:'center'">
                                    电装排产员
                                </th>
                                <th data-options="field:'logisticsClassName',width:80,align:'center'">
                                    所属车间
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
                    <div id="tb2" style="padding: 5px; height: auto;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                            onclick="Query_Worker();">刷新</a>
                        <input id="slogisticsClass" type="text" style="width: 60px; background-color:#E8E8E8;" readonly="readonly" />
                        <% if (ynWebRight.rightAdd)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                            onclick="Add_Worker();">添加</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
                            onclick="Delete_Worker();">删除</a>
                        <%} %>
                    </div>
                    <div id="editWorker" class="easyui-window" title="领料员管理" style="padding: 10px;
                        width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
                        minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                                <form id="editWorkerForm" method="post">
                                <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                                    <tr style="height: 24px">
                                        <td style="width: 20%; text-align: right;" nowrap>
                                            <span>领料员：</span>
                                        </td>
                                        <td style="width: 80%">
                                            <span id="spanWorker"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerName" }); %></span>
                                        </td>
                                    </tr>
                                </table>
                                </form>
                            </div>
                            <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                                   { %>
                                <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                                    onclick="Save_Worker()">保存</a>
                                <%} %>
                                <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editRule').window('close');">
                                    取消</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
<script type="text/javascript">
    var currentId = null;
    function Query_Class() {
        $('#dgLogisticsClassInfo').datagrid('reload');
    }

    function Add_Class() {
        $('#editLogisticsClassInfo').window('open');
        $("#editLogisticsClassInfoForm")[0].reset();

        currentId = null;
        $('#monitorLeader').combogrid('clear');
        $('#groupLeader').combogrid('clear');
    }
    function Edit_Class() {
        var selectRow = $('#dgLogisticsClassInfo').datagrid('getSelected');
        if (selectRow) {
            $('#editLogisticsClassInfo').window('open');
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsClassEdit/' + selectRow.id;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    if (r) {
                        $("#editLogisticsClassInfoForm")[0].reset();
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
    function Save_Class() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editLogisticsClassInfoForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsClassSave/' + currentId,
                    onSubmit: function () {
                        return $('#editLogisticsClassInfoForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editLogisticsClassInfo').window('close');
                            Query_Class();
                        } else {
                            $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                        }
                    }
                });
            }
        });
    }
    function Delete_Class() {
        var selectRow = $('#dgLogisticsClassInfo').datagrid('getSelected');
        if (selectRow) {
            $.messager.confirm('确认', '确认删除该车间信息[<font color="red">' + selectRow.logisticsName + '</font>]？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsClassDelete/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                Query_Class();
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
<script type="text/javascript">
    var tempId = null;
    function Query_Worker() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsWorkerList/';
        var options = $('#dgWorker').datagrid('options');
        options.url = sUrl;
        options.queryParams.queryWord = $('#slogisticsClass').val();

        $('#dgWorker').datagrid('reload');
    }
    function Add_Worker() {
        $('#editWorker').window('open');

        currentId = null;
        $("#editWorkerForm").form('clear');
        var option = $('#workerName').combogrid('grid');
        option.datagrid('reload');
    }
    function Save_Worker() {
        $.messager.confirm("确认", "确认提交保存?", function (r) {
            if (r) {
                $('#editWorkerForm').form('submit', {
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsWorkerSave/' + tempId,
                    onSubmit: function () {
                        return $('#editRuleForm').form('validate');
                    },
                    success: function (r) {
                        var rVal = $.parseJSON(r);
                        if (rVal.result) {
                            $('#editWorker').window('close');
                            Query_Worker();
                        } else {
                            $.messager.alert('确认', '添加领料员失败:' + rVal.message, 'error');
                        }
                    }
                });
            }
        });
    }
    function Delete_Worker() {
        var selectRow = $('#dgWorker').datagrid('getSelected');
        if (selectRow) {
            $.messager.confirm('确认', '确认删除该领料员[<font color="red">' + selectRow.id + '</font>]？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsWorkerDelete/' + selectRow.id;
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        success: function (r) {
                            if (r.result) {
                                Query_Worker();
                            } else {
                                $.messager.alert('确认', '删除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('提示', '请选择删除的领料员', 'info');
        }
    }
</script>
<script type="text/javascript">
    function LoadLogisticsWorker() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LoadLogisticsWorkerList/';
        var options = $('#dgWorker').datagrid('options');
        options.url = sUrl;
        options.queryParams.logisticsClassId = currentId;
        tempId = currentId;
        $('#dgWorker').datagrid('reload');
    }
    function GetLogisticsName() {
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/LogisticsClassEdit/' + tempId;
        $.ajax({
            url: sUrl,
            type: "post",
            dataType: "json",
            success: function (r) {
                if (r) {
                    var logisticsClassStr = r.logisticsClassName;
                    $('#slogisticsClass').val(logisticsClassStr);
                }
            }
        });
    }
</script>
</asp:Content>
