<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	信息关联管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--领料员管理-->
    <div region="west" split="false" border="false" title="" style="width:450px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
                    <table id="dgWorker" title="领料员管理" class="easyui-datagrid" style="" border="false"
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
                                      url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/YnUserList',
                                      onSelect: function(rowIndex, rowRec){
                                          currentId = rowRec.id;
                                      },
                                      onDblClickRow: function(rowIndex, rowRec){
                                          <% if (ynWebRight.rightEdit){ %>
                                          Edit_Worker();
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
                                <th data-options="field:'userId',width:70,align:'center'">
                                    用户账号
                                </th>
                                <th data-options="field:'userName',width:70,align:'center'">
                                    用户姓名
                                </th>
                                <th data-options="field:'sex',width:60,align:'center'">
                                    性别
                                </th>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th data-options="field:'email',width:120,align:'center'">
                                    EMail
                                </th>
                                <th data-options="field:'officeTel',width:150,align:'left'">
                                    联系电话
                                </th>
                                <th data-options="field:'mobileTel',width:150,align:'center'">
                                    移动电话
                                </th>
                                <th data-options="field:'isAccountLocked',width:80,align:'center'">
                                    账号锁定
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="tb1" style="padding:5px;height:auto;">
                        <input id="search" type="text" style="width: 100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Query_Worker();">查询</a>
                    </div>
			    </div>
			</div>
		</div>
	</div>
    <!--领料车辆管理-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px; overflow:false;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:false;">
                    <table id="dgForklift" title="领料车辆管理" class="easyui-datagrid" style="" border="false"
                        data-options="fit: true,
                          rownumbers: true,  
                          singleSelect: true,
                          idField: 'id',
                          striped: true,
                          toolbar: '#tb2',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据...',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/ForkliftList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.id;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              Edit_Forklift();
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
                    <div id="tb2" style="padding:5px;height:auto;">
                        <span>车种:</span>
                        <select id="queryType" name="queryType" style="width: 130px;">
                            <option value=""></option>
                            <% List<string> listTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.GetList(); %>
                            <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                                { %>
                            <% foreach (string statusDefine in listTypeDefine)
                                { %>
                            <option value="<%=statusDefine %>">
                                <%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.DisplayText(statusDefine)%></option>
                            <% } %>
                            <% } %>
                        </select>
                        <input id="search_forklift" type="text" style="width: 100px;" />
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                            onclick="Query_Forklift();">查询</a>
                        <% if (ynWebRight.rightAdd)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                            onclick="Add_Forklift();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                            onclick="Edit_Forklift();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete)
                           { %>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
                            onclick="Delete_Forklift();">删除</a>
                        <%} %>
                        
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
                                                style="width: 108px;" /><span style="color: Red;">*</span>
                                        </td>
                                        <td style="width: 17%; text-align: right;" nowrap>
                                            <span>车辆号码：</span>
                                        </td>
                                        <td style="width: 35%">
                                            <input class="easyui-validatebox" id="forkliftNumber" name="forkliftNumber" type="text"
                                                style="width: 108px;" />
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td style="text-align: right;" nowrap>
                                            <span>标签号：</span>
                                        </td>
                                        <td>
                                            <span id="spanTagId"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/RfidSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "tagId" }); %></span>
                                            <input type="text" id="tag" style="width: 108px; background-color: #CCCCCC;"
                                                readonly="readonly" /><span style="color: Red;">*</span>
                                        </td>
                                        <td style="text-align: right;" nowrap>
                                            <span>车种：</span>
                                        </td>
                                        <td>
                                            <select id="forkliftType" name="forkliftType" style="width: 115px;">
                                                <!--<option value=""></option>-->
                                                <% List<string> listBindTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.GetList(); %>
                                                <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                                   { %>
                                                <% foreach (string bindTypeDefine in listBindTypeDefine)
                                                   { %>
                                                <option value="<%=bindTypeDefine %>">
                                                    <%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftTypeDefine.DisplayText(bindTypeDefine)%></option>
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
                                            <select id="forkliftSpec" name="forkliftSpec" style="width: 115px;">
                                                <!--<option value=""></option>-->
                                                <% listBindTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftSpecDefine.GetList(); %>
                                                <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                                   { %>
                                                <% foreach (string bindTypeDefine in listBindTypeDefine)
                                                   { %>
                                                <option value="<%=bindTypeDefine %>">
                                                    <%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.ForkliftSpecDefine.DisplayText(bindTypeDefine)%></option>
                                                <% } %>
                                                <% } %>
                                            </select>
                                        </td>
                                        <td style="text-align: right;" nowrap>
                                            <span>归属主体：</span>
                                        </td>
                                        <td>
                                            <select id="sLogisticsClass" name="LogisticsClass" style="width: 115px;">
                                                <!--<option value=""></option>-->
                                                <% listBindTypeDefine = MideaAscm.Services.GetMaterialManage.AscmCommonHelperService.GetInstance().GetLogisticsClassRelfectList(); %>
                                                <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                                   { %>
                                                <% foreach (string bindTypeDefine in listBindTypeDefine)
                                                   { %>
                                                <option value="<%=bindTypeDefine %>">
                                                    <%=MideaAscm.Services.GetMaterialManage.AscmCommonHelperService.GetInstance().DisplayLogisticsClass(bindTypeDefine)%></option>
                                                <% } %>
                                                <% } %>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr style="height: 24px">
                                        <td style="text-align: right;" nowrap>
                                            <span>责任人：</span>
                                        </td>
                                        <td>
                                            <span id="spanWorker"><%Html.RenderPartial("~/Areas/Ascm/Views/GetMaterialManage/ViewUserSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "workerId" }); %></span>
                                            <input type="text" id="workerName" style="width: 108px; background-color: #CCCCCC;"
                                                readonly="readonly" /><span style="color: Red;">*</span>
                                        </td>
                                        <td style="text-align: right;" nowrap>
                                            <span> 状态：</span>
                                        </td>
                                        <td>
                                            <select id="sStatus" name="Status" style="width: 115px;">
                                                <!--<option value=""></option>-->
                                                <% listBindTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.StatusDefine.GetList(); %>
                                                <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                                   { %>
                                                <% foreach (string bindTypeDefine in listBindTypeDefine)
                                                   { %>
                                                <option value="<%=bindTypeDefine %>">
                                                    <%=MideaAscm.Dal.GetMaterialManage.Entities.AscmForklift.StatusDefine.DisplayText(bindTypeDefine)%></option>
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
                                    onclick="Save_Forklift()">保存</a>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        var currentId = null;
        //领料员操作
        function Query_Worker() {
            var options = $('#dgWorker').datagrid('options');
            options.queryParams.queryWord = $('#search').val();
            $('#dgWorker').datagrid('reload');
        }
    </script>
    <script type="text/javascript">
        //领料车辆操作
        function Query_Forklift() {
            var options = $('#dgForklift').datagrid('options');
            options.queryParams.queryWord = $('#queryType').val();
            options.queryParams.queryOtherWord = $('#search_forklift').val();
            $('#dgForklift').datagrid('reload');
        }
        function Add_Forklift() {
            $('#spanTagId').show();
            $('#tag').hide();
            $('#spanWorker').show();
            $('#workerName').hide();

            $('#editForklift').window('open');
            $("#editForkliftForm")[0].reset();

            $('#assetsId').focus();
            currentId = null;
        }
        function Edit_Forklift() {
            var selectRow = $('#dgForklift').datagrid('getSelected');
            if (selectRow) {
                $('#editForklift').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/ForkliftEdit/' + selectRow.id;
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
                            $('#forkliftType').val(r.forkliftType);
                            $('#forkliftSpec').val(r.forkliftSpec);
                            $('#sLogisticsClass').val(r.logisticsClass);
                            $('#spanWorker').hide();
                            $('#workerName').show();
                            $('#workerId').val(r.workerId)
                            $('#workerName').val(r.workerName);
                            $('#sStatus').val(r.Status);
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
        function Save_Forklift() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editForkliftForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/ForkliftSave/' + currentId,
                        onSubmit: function () {
                            return $('#editForkliftForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editForklift').window('close');
                                currentId = rVal.id;
                                Query_Forklift();
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function Delete_Forklift() {
            var selectRow = $('#dgForklift').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除领料车辆信息[<font color="red">' + selectRow.assetsId + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/ForkliftDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    Query_Forklift();
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
