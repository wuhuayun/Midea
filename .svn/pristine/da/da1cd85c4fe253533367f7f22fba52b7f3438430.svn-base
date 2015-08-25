<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    RFID管理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dataGridRfid" title="RFID管理" style="" border="false" fit="true" singleselect="true"
            idfield="id" sortname="id" sortorder="asc" striped="true" toolbar="#tb1">
        </table>
        <div id="tb1">
          <span>标签类型：</span>
            <select id="queryType" name="queryType" style="width: 120px;">
                <option value=""></option>
                <% List<string> listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.GetList(); %>
                <% List<string> listBindStatusDefine = MideaAscm.Dal.Base.Entities.AscmRfid.StatusDefine.GetList(); %>
                <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                   { %>
                <% foreach (string bindTypeDefine in listBindTypeDefine)
                   { %>
                <option value="<%=bindTypeDefine %>">
                    <%=MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
                <% } %>
                <% } %>
            </select>
             <span>标签编号：</span>
            <input id="rfidSearch" type="text" style="width: 100px;" />
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
            <br />
             <form id="filePost" method="post" enctype="multipart/form-data">
            <label>
                Excel导入:<input type="file" name="file" id="file" size="23" style="width: 300px;" />
            </label>
            <a id="btnExcel" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                onclick="Upload()">保存</a>
            </form>
        </div>
        <div id="addRfid" class="easyui-window" title="添加RFID" style="padding: 0px; width: 540px;
            height: 420px;" iconcls="icon-edit" closed="true" maximizable="false" minimizable="false"
            resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 0px; background: #fff; border: 1px solid #ccc;">
                    <form id="addRfidForm" method="post" style="" fit="true">
                    <div id="tb2">
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;">
                                <span>标签类型：</span>
                            </td>
                            <td style="width: 80%">
                                <select id="addTagType" name="addTagType" style="width: 120px;">
                                    <option value=""></option>
                                    <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                       { %>
                                    <% foreach (string bindTypeDefine in listBindTypeDefine)
                                       { %>
                                    <option value="<%=bindTypeDefine %>">
                                        <%=MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                    </div>
                    <ul id="addTagList" title="" style="height: 320px;" border="false" singleselect="true"
                        idfield="id" sortname="sortNo" sortorder="asc" striped="true" toolbar="#tb2">
                    </ul>
                    <input id="addTagText" name="addTagText" type="text" style="display: none;" />
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="addSave()">增加</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#addRfid').window('close');">
                        取消</a>
                </div>
            </div>
        </div>
        <div id="editRfid" class="easyui-window" title="修改RFID" style="padding: 10px; width: 540px;
            height: 420px;" iconcls="icon-edit" closed="true" maximizable="false" minimizable="false"
            resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editRfidForm" method="post" style="">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>标签编号：</span>
                            </td>
                            <td style="width: 40%">
                                <input id="editId" name="editId" type="text" disabled="disabled" style="width: 100px;" />
                            </td>
                            <td style="width: 40%">
                                <input id="edit_Id" name="edit_Id" type="text" style="width: 100px; display: none;" />
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>指定类型：</span>
                            </td>
                            <td style="width: 80%">
                                <select id="editType" name="editType" style="width: 145px;">
                                    <option value=""></option>
                                    <% listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.GetList(); %>
                                    <% if (listBindTypeDefine != null && listBindTypeDefine.Count > 0)
                                       { %>
                                    <% foreach (string bindTypeDefine in listBindTypeDefine)
                                       { %>
                                    <option value="<%=bindTypeDefine %>">
                                        <%=MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.DisplayText(bindTypeDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>状态：</span>
                            </td>
                            <td style="width: 80%">
                                <select id="editStatus" name="editStatus" style="width: 145px;">
                                    <option value=""></option>
                                    <% listBindStatusDefine = MideaAscm.Dal.Base.Entities.AscmRfid.StatusDefine.GetList(); %>
                                    <% if (listBindStatusDefine != null && listBindStatusDefine.Count > 0)
                                       { %>
                                    <% foreach (string bindStatusDefine in listBindStatusDefine)
                                       { %>
                                    <option value="<%=bindStatusDefine %>">
                                        <%=MideaAscm.Dal.Base.Entities.AscmRfid.StatusDefine.DisplayText(bindStatusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="A1" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="editSave()">确定</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editRfid').window('close');">
                        取消</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
 function Upload() {
            $('#filePost').form('submit', {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/Upload',
                onSubmit: function () {
                    var file = $('#file').val();
                    if (file == undefined || $.trim(file) == "") {
                        $.messager.alert('错误', '请选择上传文件!');
                        return false;
                    } else {
                        var fileArr = file.split("\\");
                        var fileTArr = fileArr[fileArr.length - 1].toLowerCase().split(".");
                        var filetype = fileTArr[fileTArr.length - 1];
                        if (filetype != "xls") {
                            $.messager.alert('错误', '上传文件必须为Excel 2003文件!');
                            return false;
                        }
                    }                    
                },
                success: function (r) {
                    $.messager.alert('提示', r.toString());
                    Reload();
                    $('#filePost').form('reset');
                }
            });
        }
        $(function () {
            $('#dataGridRfid').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/TagList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: '标签编号', width: 120, align: 'center'},
                    { field: 'epcId', title: 'EPC', width: 180, align: 'left' },
					{ field: '_bindTypeCn', title: '类型', width: 100, align: 'center' },
                    { field: 'createUser', title: '创建人', width: 100, align: 'center' },
                    { field: 'createTime', title: '创建时间', width: 150, align: 'center' },
                    { field: 'modifyUser', title: '最后更新人', width: 100, align: 'center' },
                    { field: 'modifyTime', title: '最后更新时间', width: 150, align: 'center' },
					{ field: '_status', title: '状态', width: 80, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    Edit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
            $('#queryType').change(function(){
                var queryParams = $('#dataGridRfid').datagrid('options').queryParams;
                queryParams.queryType = $('#queryType').val();
    			queryParams.queryWord = $('#rfidSearch').val();
                $('#dataGridRfid').datagrid('reload');

            })
        });
        function Reload() {
            $('#dataGridRfid').datagrid('reload');
        }
		function Query(){
			var queryParams = $('#dataGridRfid').datagrid('options').queryParams;
            queryParams.queryType = $('#queryType').val();
			queryParams.queryWord = $('#rfidSearch').val();
			$('#dataGridRfid').datagrid('reload');
		}
        var currentId = null;

        function Add() {
            $('#addRfid').window('open');
            $("#addRfidForm")[0].reset();
            currentId = "";

            $('#addTagList').treegrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/AddTagList',
                pagination: false,
                treeField:'epcId',
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'epcId', title: 'EPC', width: 150, align: 'center' },
					//{ field: '_bindTypeCn', title: '类型', width: 100, align: 'center' },
                    { field: 'createUser', title: '创建人', width: 100, align: 'center' },
                    { field: 'createTime', title: '创建时间', width: 120, align: 'center' }
                ]]
            });
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/AddTagList/';
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                data:{rfid:true},
                success: function (r) {
                    $("#addTagText").val("");
				    $("#addTagText").val(r.message);
                    $('#addTagList').treegrid('loadData',r.rows);
                }
            });
        }
        //覃小华于2013/07/15修改
        function addSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    if($('#addTagType').val()==""){
                     $.messager.alert('错误', '没有选择标签类型！','error');
                     return;
                    }
                    else{
                    if($('#addTagText').val()=="")
                    {
                     $.messager.alert('错误', '没有读取到标签，无法保存！','error');
                     return;
                    }
                    $('#addRfidForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/AddTagSave/',
                        onSubmit: function () {
                            return $('#addRfidForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                if(rVal.message!="")
                                {
                                   //$.messager.alert('提示', '以下标签重复已被系统自动过滤:<br>' + rVal.message, 'inf');
                                }
                                $('#addRfid').window('close');
                                Reload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    });
                    }
			    }
			});
        }
        function Edit() {
            var selectRow = $('#dataGridRfid').datagrid('getSelected');
            if (selectRow) {
                $('#editRfid').window('open');
                $("#editRfidForm")[0].reset();
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/EditTag/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        
				        $("#editId").val(r.id);
                        $("#edit_Id").val(r.id);
				        $("#editType").val(r.bindType);
				        $("#editStatus").val(r.status);
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择RFID', 'info');
            }
        }
        function editSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editRfidForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/EditTagSave/',
                        onSubmit: function () {
                            return $('#editRfidForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editRfid').window('close');
                                //currentId = rVal.id;
                                Reload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});
        }
    </script>
</asp:Content>
