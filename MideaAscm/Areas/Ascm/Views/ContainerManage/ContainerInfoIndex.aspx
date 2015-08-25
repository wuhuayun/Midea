<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    容器基础信息管理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: false;">
        <table id="dgContainer" title="容器基础信息管理" class="easyui-datagrid" style="" border="false"
            data-options="fit: true,
                          rownumbers: false,
                          singleSelect : true,
                          remoteSort:false,
                          idField: 'sn',
                          sortName: 'sn',
                          sortOrder: 'asc',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: true,
                          pageSize: 50,
                          loadMsg: '更新数据......',
                          url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerList',
                          onSelect: function(rowIndex, rowRec){
                              currentId = rowRec.sn;
                          },
                          onDblClickRow: function(rowIndex, rowRec){
                              <% if (ynWebRight.rightEdit){ %>
                              Edit();
                              <%} %>
                          },
                          onLoadSuccess: function(data){
                              $(this).datagrid('clearSelections');
                              if (currentId) {
                                  $(this).datagrid('selectRecord', currentId);
                              }
                          }">
            <thead>
                <tr>
                    <th data-options="checkbox:true"></th>
                    <th data-options="field:'sn',width:80,align:'center',sortable:'true'">
                        容器编号
                    </th>
                    <th data-options="field:'rfid',width:80,align:'center'">
                        EPC
                    </th>                                                   
                    <th data-options="field:'spec',width:60,align:'center'">
                        规格
                    </th>
                    <th data-options="field:'supplierName',width:200,align:'left'">
                        供应商名称
                    </th>
                    <th data-options="field:'ReadingHeadAddress',width:120,align:'center',sortable:'true'">位置</th>
                    <th data-options="field:'description',width:100,align:'left'">
                        描述
                    </th>
                    <th data-options="field:'statusCn',width:80,align:'center'">
                        状态
                    </th>
                    <th data-options="field:'_modifyTime',width:120,align:'center'">
                        最后更新时间
                    </th>
                </tr>
            </thead>
        </table>
        <div id="tb1" style="padding: 5px; height: auto;">
            <div style="margin-bottom: 5px;">
                <span>供应商：</span>
                &nbsp;&nbsp;<%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
                <span>规格：</span>
                <input id="cmb_spec" class="easyui-combobox" name="cmb_spec" style="width: 90px"  data-options="panelHeight:'400px',valueField:'id',textField:'spec',url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/RfidSpeac'"/>
                 <span>位置：</span>
                <input id="cmb_place" class="easyui-combobox" name="cmb_place" style="width: 140px"  data-options="panelHeight:'400px',valueField:'id',textField:'address',url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerPlace'"/>
                <span>状态：</span>
                <select id="queryStatus" name="queryStatus" style="width: 80px;">
                    <option value=""></option>
                    <% List<int> listStatusDefine = MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.GetListII(); %>
                    <% if (listStatusDefine != null && listStatusDefine.Count > 0)
                       {
                           listStatusDefine.Remove(4); listStatusDefine.Remove(5); listStatusDefine.Remove(6); listStatusDefine.Remove(7);
                          %>
                    <% foreach (int statusDefine in listStatusDefine)
                       { %>
                    <option value="<%=statusDefine %>">
                        <%=MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.DisplayText(statusDefine)%></option>
                    <% } %>
                    <% } %>
                </select>
             </div>
            <div>
                <span>容器编号：</span>
                <input id="querySn" name="querySn" type="text" style="width: 100px; text-align: right;" />
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
               <%-- <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
                    onclick="Delete()">删除</a>--%>
                <%} %>
                 <a id="btnExprot" class="easyui-linkbutton" iconcls="icon-download" href="#" onclick ="ExpoerBascInf()"
                >导出数据</a>
            </div>             
            <form id="filePost" method="post" enctype="multipart/form-data">
            <label>
                Excel导入:<input type="file" name="file" id="file" size="23" style="width: 300px;" />
            </label>
            <input id="txt_hide" type="text" name="isRfid" value="false" style="display:none" />
            <a id="btnExcel" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                onclick="Upload()">保存</a>
            </form>
        </div>
        <div id="editContainer" class="easyui-window" title="容器修改" style="padding: 10px;
            width: 540px; height: 420px;" iconcls="icon-edit" closed="true" maximizable="false"
            minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                    <form id="editContainerForm" method="post" style="">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                     <tr id='rfidTr' style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>RFID编号：</span>
                            </td>
                            <td style="width: 80%">
                                <input class="easyui-validatebox" id="rfid" name="rfid" type="text" style="width: 200px;
                                    text-align: right;" /><span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>供应商：</span>
                            </td>
                            <td style="width: 80%">
                                <span id="spanSupplier">
                                    <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "supplierId" }); %></span>
                                <input type="text" id="supplierName" style="width: 300px; background-color: #CCCCCC;" /><span
                                    style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>规格：</span>
                            </td>
                            <td style="width: 80%">
                                <span id="spanSpec">
                                    <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/ContainerSpecSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "specId" }); %></span>
                                <input type="text" id="spec" style="width: 300px; background-color: #CCCCCC;" /><span
                                    style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="width: 20%; text-align: right;" nowrap>
                                <span>状态：</span>
                            </td>
                            <td style="width: 80%">
                                <%--<input id="statusCn" name="statusCn" type="text" style="width:100px;background-color:#CCCCCC;" readonly="readonly"/>
                                    <input id="status" name="status" type="hidden" />--%>
                                <select id="status" name="status" style="width: 80px;">
                                    <%--     <option value=""></option>--%>
                                    <% List<int> StatusDefine = MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.GetListII();  %>
                                    <% if (StatusDefine != null && StatusDefine.Count > 0)
                                       {
                                           StatusDefine.Remove(0); StatusDefine.Remove(4); StatusDefine.Remove(5); StatusDefine.Remove(6); StatusDefine.Remove(7);%>
                                    <% foreach (int statusDefine in StatusDefine)
                                       { %>
                                    <option value="<%=statusDefine %>">
                                        <%=MideaAscm.Dal.SupplierPreparation.Entities.AscmContainer.StatusDefine.DisplayText(statusDefine)%></option>
                                    <% } %>
                                    <% } %>
                                </select><span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="text-align: right;" nowrap>
                                <span>描述：</span>
                            </td>
                            <td>
                                <input class="easyui-validatebox" id="description" name="description" type="text"
                                    style="width: 300px;" />
                            </td>
                        </tr>                       
                    </table>
                    <div id="rfidListTr">
                        <ul id="rfidList" title="RFID列表" style="height: 320px;" border="false" singleselect="true"
                            idfield="id" sortname="sortNo" sortorder="asc" striped="true">
                        </ul>
                    </div>
                    <input id="rfidText" name="rfidText" type="text" style="display: none;" />
                 </form>
                </div>
                <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit)
                       { %>
                    <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                        onclick="Save()">保存</a>
                    <%} %>
                    <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)" onclick="$('#editContainer').window('close');">
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
                    $('#filePost').form('reset');
                    Query();
                }
            });
        }
        function Query() {
            var options = $('#dgContainer').datagrid('options');
            options.queryParams.supplierId = $('#supplierSelect').combogrid('getValue');
            options.queryParams.status = $('#queryStatus').val();
            options.queryParams.querySn = $('#querySn').val();
            options.queryParams.spec = $('#cmb_spec').combobox('getValue');
            options.queryParams.place = $('#cmb_place').combobox('getValue');
            $('#dgContainer').datagrid('reload');
        }
        var currentId = "";
        var EditOrAdd = "";
        function Add() {
            $('#spanSupplier').show();
            $('#supplierName').hide();
            $('#spanSpec').show();
            $('#spec').hide();

            $('#rfid').css('background-color', '');
            $('#rfidTr').hide();
            $('#rfidListTr').show();

            $('#editContainer').window({ title: '新增容器' });
            $('#editContainer').window('open');

            $("#editContainerForm")[0].reset();
            currentId = "";
            EditOrAdd = "Add";

            $('#rfidList').treegrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/AddTagList',
                pagination: false,
                treeField: 'epcId',
                loadMsg: '更新数据......',
                columns: [[
                    { field: 'epcId', title: 'EPC', width: 150, align: 'center' },
					{ field: '_bindTypeCn', title: '类型', width: 100, align: 'center' },
                    { field: 'createUser', title: '创建人', width: 100, align: 'center' },
                    { field: 'createTime', title: '创建时间', width: 100, align: 'center' }
                ]]
            });

            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/AddTagList/';
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (r) {
                    $("#rfidText").val(r.message);
                    $('#rfidList').treegrid('loadData', r.rows);
                }
            });
        }
        function Edit() {
            var selectRow = $('#dgContainer').datagrid('getSelected');
            if (selectRow) {
                $('#editContainer').window({ title: '容器修改' });
                $('#editContainer').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerEdit/' + selectRow.sn;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        if (r) {
                            $("#editContainerForm")[0].reset();
                            $('#spanSupplier').hide();
                            $('#supplierName').show();
                            $('#supplierName').val(r.supplierName);

                            $('#spanSpec').hide();
                            $('#spec').show();
                            $('#spec').val(r.spec);

                            $('#status').val(r.status);
                            $('#description').val(r.description);

                            $('#rfidTr').show();
                            $('#rfidListTr').hide();
                            $('#rfid').css('background-color', '#CCCCCC');
                            $('#rfid').attr('readonly', true);
                            $('#rfid').val(r.sn);
                        }
                    }
                });
                currentId = selectRow.sn;
                EditOrAdd = "Edit";
            } else {
                $.messager.alert('提示', '请选择容器', 'info');
            }
        }
        function Save() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    switch (EditOrAdd) {
                        case ("Edit"):
                            $('#editContainerForm').form('submit', {
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerEditSave/' + currentId,
                                onSubmit: function () {
                                    return $('#editContainerForm').form('validate');
                                },
                                success: function (r) {
                                    var rVal = $.parseJSON(r);
                                    if (rVal.result) {
                                        $('#editContainer').window('close');
                                        (rVal.id)
                                        currentId = rVal.id;
                                        Query();
                                    } else {
                                        $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                                    }
                                }
                            });
                            break
                        case ("Add"):
                            $('#editContainerForm').form('submit', {
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerAddSave/',
                                onSubmit: function () {
                                    if ($('#rfidText').val() == "") {
                                        $.messager.alert('提示', '没有标签数据，无法保持！', 'inf');
                                        return false;
                                    }
                                    return $('#editContainerForm').form('validate');
                                },
                                success: function (r) {
                                    var rVal = $.parseJSON(r);
                                    if (rVal.result) {
                                        if (rVal.message != "") {
                                            $.messager.alert('提示', '以下标签重复已被系统自动过滤:<br>' + rVal.message, 'inf');
                                        }
                                        $('#editContainer').window('close');
                                        if (rVal.id)
                                            currentId = rVal.id;
                                        Query();
                                        $('#editContainerForm').form('clear');
                                    } else {
                                        $.messager.alert('确认', '添加信息失败:' + rVal.message, 'error');
                                    }
                                }
                            });
                            break
                    }
                }
            });

        }
        function Delete() {
            var selectRow = $('#dgContainer').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerDelete/';
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data: { sn: selectRow.sn },
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
            else {
                $.messager.alert('提示', '请选择容器', 'info');
            }
        }
       function ExpoerBascInf() {
             Query();
            var adress="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ExportContainerInfExcell/?querySn ="+ $('#querySn').val()+"&supplierId="+$('#supplierSelect').combogrid('getValue')+"&status = "+$('#queryStatus').val()+"&spec="+ $('#cmb_spec').combobox('getValue')+"&place="+$('#cmb_place').combobox('getValue')+"";
            $('#btnExprot').attr('href', adress);
        }
        

    </script>
</asp:Content>
