<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	导入排产管理
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgDiscreteJobs" title="导入排产管理" style="" border="false" fit="true"
            singleselect="true" idfield="id" sortname="" sortorder="" striped="true" toolbar="#tb1">
        </table>
        <div id="tb1" style="padding:5px;height:auto;">
            <div style="margin-bottom:5px;">
                <span>上传日期：</span><input class="easyui-datebox" id="queryStartDate" type="text" style="width:130px;" /> ~
                <input class="easyui-datebox" id="queryEndDate" type="text" style="width:130px;" />
                <span>&nbsp;作业日期：</span><input class="easyui-datebox" id="queryStartJobDate" type="text" style="width:130px;" /> ~
                <input class="easyui-datebox" id="queryEndJobDate" type="text" style="width:130px;" />
            </div>
            <div style="margin-bottom:5px;">
                <span>作业类型：</span><span><select id="queryType" name="queryType" style="width: 100px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                    <option value="">　</option>
                    <% List<int> listTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.GetList(); %>
                    <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                        { %>
                    <% foreach (int statusDefine in listTypeDefine)
                        { %>
                    <option value="<%=statusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.DisplayText(statusDefine)%></option>
                    <% } %>
                    <% } %>
                </select></span>
                <% if (ynWebRight.rightVerify)
                   { %>
                <span>排产员：</span>
                <input id="queryPerson" type="text" style="width: 100px;" />
                <%} %>

                <% if (ynWebRight.rightAdd)
                   { %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                    onclick="ImportExcel();">上传</a>
                <%} %>
                <% if (ynWebRight.rightEdit)
                   { %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit"
                    onclick="Edit();">修改</a>
                <%} %>
                <% if (ynWebRight.rightDelete)
                   { %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
                    onclick="Delete()">删除</a>
                <%} %>

                <% if (ynWebRight.rightAdd)
                   { %>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove"
                    onclick="ExportExcel();">导出</a>
                <%} %>

                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                    onclick="Query();">查询</a> 
            </div>
        </div>
        <div id="popImport" class="easyui-window" title="上传排产单" style="padding: 10px;width:380px;height:200px;"
            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" id="FileUpload" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
					<form id="FormUpload" enctype="multipart/form-data" method="post" ><br />
                    <p>
                        <span>作业类型：</span><span><select id="identificationIdType" name="identificationId" style="width: 100px;" class="easyui-combobox" data-options="panelHeight:'auto'">
                        <option value="">　</option>
                        <% listTypeDefine = MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.GetList(); %>
                        <% if (listTypeDefine != null && listTypeDefine.Count > 0)
                            { %>
                        <% foreach (int statusDefine in listTypeDefine)
                            { %>
                        <option value="<%=statusDefine %>"><%=MideaAscm.Dal.GetMaterialManage.Entities.AscmDiscreteJobs.IdentificationIdDefine.DisplayText(statusDefine)%></option>
                        <% } %>
                        <% } %>
                    </select></span>
                    
                   
                    <span>角色：</span><span> <%Html.RenderPartial("~/Areas/Ascm/Views/Logistics/roleSelectCombo.ascx"); %></span>
                    </p>
                    <br />
                        <p><input type="file" id="fileImport" name="fileImport" size="35" value=''/>&nbsp;&nbsp;</p>
                    </form>
                </div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="ImportOk()">保存</a>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#popImport').window('close');">取消</a>
				</div>
            </div>
        </div>
        <div id="progressbarDiv" class="easyui-window" title="" style="padding: 12px;width:460px;height:90px;"
            closed="true" closable="false" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div id="p" class="easyui-progressbar" style="width:400px;"></div> 
        </div>
        <div id="editDiscreteJobs" class="easyui-window" title="修改排产单" style="padding: 10px; width: 540px;
            height: 420px;" iconcls="icon-edit" closed="true" maximizable="false" minimizable="false"
            resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editDiscreteJobsForm" method="post" style="">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="width: 15%; text-align: right;" nowrap>
                                <span>作业号：</span>
                            </td>
                            <td style="width: 35%">
                                <input id="jobId" name="jobId" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                            <td style="width: 15%; text-align: right;" nowrap>
                                <span>类型：</span>
                            </td>
                            <td style="width: 35%">
                                <input id="sIdentificationId" name="sIdentificationId" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                                <input type="hidden" id="identificationId" name="identificationId" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>装配件：</span>
                            </td>
                            <td>
                                <input id="jobInfoId" name="jobInfoId" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>所属排产员：</span>
                            </td>
                            <td>
                                <input type="hidden" id="workerId" name="workerId" />
                                <input id="rankerName" name="rankerName" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>数量：</span>
                            </td>
                            <td>
                                <input id="count" name="count" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                            <td style="text-align: right;" nowrap>
                                <span>生产线：</span>
                            </td>
                            <td>
                                <input id="productLine" name="productLine" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>顺序：</span>
                            </td>
                            <td>
                                <input id="sequence" name="sequence" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                            <td style="width: 15%; text-align: right;" nowrap>
                                <span>上线时间：</span>
                            </td>
                            <td>
                                <input id="onlineTime" name="onlineTime" type="text" style="width: 100px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>装配件描述：</span>
                            </td>
                            <td colspan="3">
                                <input id="jobDesc" name="jobDesc" type="text" style="width: 345px; background-color: #CCCCCC;" readonly="readonly" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>备注：</span>
                            </td>
                            <td colspan="3">
                                <textarea class="easyui-validatebox" id="tip" name="tip" rows="3"
                                    cols="342" style="width: 345px;"></textarea>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="A2" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save()">确定</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editDiscreteJobs').window('close');">
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

        $(function () {
            $('#dgDiscreteJobs').datagrid({
                pagination: true,
                rownumbers: true,
                pageSize: 50,
                singleSelect: false,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/ImportWipDiscreteJobsList/',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { checkbox: true },
                    { field: 'sIdentificationId', title: '类型', width: 60, align: 'center' },
                    { field: 'jobId', title: '作业号', width: 120, align: 'center' }
                ]],
                columns: [[
                    { field: 'jobDate', title: '作业日期', width: 90, align: 'center' },
                    { field: 'jobInfoId', title: '装配件', width: 100, align: 'center' },
                    { field: 'personName', title: '角色', width: 100, align: 'center' },
                    { field: 'jobDesc', title: '装配件描述', width: 200, align: 'left' },
                    { field: 'count', title: '数量', width: 60, align: 'center' },
                    { field: 'lineAndSequence', title: '生产线', width: 60, align: 'center' },
                    { field: 'tip', title: '备注', width: 150, align: 'left' },
                    { field: 'onlineTime', title: '上线时间', width: 80, align: 'center' },
                    { field: 'which', title: '第几次', width: 60, align: 'center' },
                    { field: 'personName', title: '角色', width: 100, align: 'center' },
                    { field: 'rankerName', title: '所属排产员', width: 80, align: 'center' },
                    { field: 'time', title: '上传时间', width: 120, align: 'center' },
                    { field: '_status', title: '状态', width: 60, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    //Edit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })

            //初始化默认日期为当天
            var date = new Date();
            var yyyy = date.getFullYear();
            var MM = date.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            var dd = date.getDate();
            dd = dd < 10 ? "0" + dd : dd;
            $("#queryStartDate").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        });
    </script>
    <script type="text/javascript">
        function Query() {
            var option = $('#dgDiscreteJobs').datagrid('options').queryParams;
            option.queryStartDate = $('#queryStartDate').datebox('getText');
            option.queryEndDate = $('#queryEndDate').datebox('getText');
            option.queryStartJobDate = $('#queryStartJobDate').datebox('getText');
            option.queryEndJobDate = $('#queryEndJobDate').datebox('getText');
            option.queryType = $('#queryType').combobox('getValue');

            <% if (ynWebRight.rightVerify)
                { %>
            option.queryPerson = $('#queryPerson').val();
            <%} %>

            $('#dgDiscreteJobs').datagrid('reload');
        }
    </script>
    <script type="text/javascript">
        function Edit() {
            var selectRow = $('#dgDiscreteJobs').datagrid('getSelected');
            if (selectRow) {
                $('#editDiscreteJobs').window('open');
                $("#editDiscreteJobsForm")[0].reset();
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/ImportWipDiscreteJobsEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $('#jobId').val(r.jobId);
                        $('#identificationId').val(r.identificationId);
                        $('#sIdentificationId').val(r.sIdentificationId);
                        $('#jobInfoId').val(r.jobInfoId);
                        $('#jobDesc').val(r.jobDesc);
                        $('#count').val(r.count);
                        $('#productLine').val(r.productLine);
                        $('#sequence').val(r.sequence);
                        $('#onlineTime').val(r.onlineTime);
                        $('#workerId').val(r.workerId);
                        $('#rankerName').val(r.rankerName);
                        $('#tip').val(r.tip);
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择排产单', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editDiscreteJobsForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/ImportWipDiscreteJobsEditSave/' + currentId,
                        onSubmit: function () {
                            return $('#editDiscreteJobsForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editDiscreteJobs').window('close');
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
            var releaseHeaderIds = "";
            var checkRows = $('#dgDiscreteJobs').datagrid('getChecked');
            if (checkRows.length == 0) {
                $.messager.alert("提示", "请勾选将要删除的排产单信息！", "info");
                return;
            }
            $.each(checkRows, function (i, item) {
                if (releaseHeaderIds != "")
                    releaseHeaderIds += ",";
                releaseHeaderIds += item.id;
            });
            $.messager.confirm('确认', '确认删除排产单信息？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/DiscreteJobsDelete/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { "releaseHeaderIds": releaseHeaderIds },
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
        }
    </script>
    <script type="text/javascript">
        function updateProgress() {
            var value = $('#p').progressbar('getValue');
            if (value < 99) {
                $('#p').progressbar('setValue', value + 1);
            }
        }
        function ImportExcel() {
            $('#popImport').window('open');
        }
        function ImportOk() {
            var _selFile = $('#FormUpload input[Name=fileImport]').val();
            if (_selFile == "") {
                $.messager.alert('警告', "请选择文件！", 'warning');
                return;
            }
            var _selidentificationId = $('#identificationIdType').combobox('getValue');
            if (_selidentificationId == null || _selidentificationId == "") {
                $.messager.alert('警告', "请选择作业类型！", 'warning');
                return;
            }
            $('#progressbarDiv').window({ title: '导入进度' });
            $('#progressbarDiv').window('open');
            $('#p').progressbar('setValue', 0);
            setInterval(updateProgress, 600);
            $('#FormUpload').form('submit', {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/ImportWipDiscreteJobs/',
                onSubmit: function () {
                    return $('#FormUpload').form('validate');
                },
                success: function (result) {
                    $('#p').progressbar('setValue', 100);
                    $('#progressbarDiv').window('close');
                    var retValue = eval('(' + result + ')');
                    $('#popImport').window('close');
                    if (retValue.result) {
                        if (retValue.message != null && retValue.message != "") {
                            $.messager.alert('错误', retValue.message, '');
                        }
                        Query();
                    }
                    else {
                        $.messager.alert('错误', retValue.message, '');
                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
    var queryPerson = "";
        function ExportExcel() {
            <% if (ynWebRight.rightVerify)
                { %>
            queryPerson = $('#queryPerson').val();
            <%} %>
            var data = { "queryWord": "",
                "queryStartDate": $('#queryStartDate').datebox('getText'),
                "queryEndDate": $('#queryEndDate').datebox('getText'),
                "queryStartJobDate": $('#queryStartJobDate').datebox('getText'),
                "queryEndJobDate": $('#queryEndJobDate').datebox('getText'),
                "queryType": $('#queryType').combobox('getValue'),
                "queryPerson": queryPerson
            };
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Logistics/ExportWipDiscreteJobs/';
            sUrl += "?" + $.param(data, true);
            window.location.href = sUrl;
        }
    </script>
</asp:Content>
