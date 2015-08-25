<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    容器出库管理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 供应商选择-->
    <div class="easyui-layout" style="width: 100%; height: 100%;">
        <div data-options="region:'west',collapsible:false" title="请在这里选择供应商" style="width: 380px">
            <table id="dgSuplier" title="请在这里选择供应商" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                          rownumbers: true,
                          singleSelect: true,
                          checkOnSelect: true, 
                          selectOnCheck: true,
                          url:'<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/SupplierSelectList/?IsOut=true',
                          remoteSort:false,
                          idField: 'id',
                          sortName: 'name',
                          sortOrder: 'desc',
                          striped: true,
                          toolbar:clearTool,
                          pagination: false,
                          loadMsg: '更新数据......',
                          onCheck: function(index,row){                          
                                    Read();              
                          },
                          onLoadSuccess: function(data){
                            //  $(this).datagrid('clearSelections');
                          }">
                <thead>
                    <tr>
                        <th data-options="checkbox:true">
                        </th>
                        <th data-options="field:'name',width:400,align:'left',sortable:'true'">
                            供应商名称
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="clearTool" style="padding: 5px; height: auto;">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload"
                onclick="clearSelect();">清空</a>
        </div>
        <div data-options="region:'center',collapsible:false, fit:true" title="" border="true"
            style="width: 600px">
            <table id="dgContainer" title="读取到的容器列表" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                          rownumbers: true,
                          singleSelect : false,
                          checkOnSelect:true,
                          remoteSort:false,
                          idField: 'sn',
                          sortName: 'rfid',
                          sortOrder: 'desc',
                          striped: true,
                          toolbar: '#tb1',
                          pagination: false,
                          loadMsg: '更新数据......',
                          onLoadSuccess: function(data){
                              $(this).datagrid('clearSelections');
                          }">
                <thead>
                    <tr>
                       <th data-options="checkbox:true">
                    </th>
                    <th data-options="field:'sn',width:80,align:'center',sortable:'true' ">
                        容器编号
                    </th>
                    <th data-options="field:'rfid',width:160,align:'center',sortable:'true' ">
                        EPC
                    </th>                                                   
                    <th data-options="field:'spec',width:60,align:'center',sortable:'true' ">
                        规格
                    </th>
                    <th data-options="field:'supplierName',width:200,align:'left',sortable:'true' ">
                        供应商名称
                    </th>
                </thead>
            </table>
        </div>
        <div id="tb1" style="padding: 5px; height: auto;">
            <div>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove"
                    onclick="deleteEx();">一键删除异常</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                        plain="true" icon="icon-add" onclick="Read();">获取读写器数据</a> <a href="javascript:void(0);"
                            class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-save"
                    onclick="Summit();">确认出库</a>
                <input type="checkbox" name="print" id="print" />
                <span>打印单据</span>
            </div>
        </div>
    </div>
    <!--供应商手动添加已注释-->
    <div id="dlg" class="easyui-dialog" title="" data-options="iconCls:'icon-save',modal:true, closed:true, buttons: '#dlg-buttons'"
        style="width: 600px; height: 400px; padding: 10px">
        <div class="easyui-layout" fit="true">
            <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                <table cellpadding="0" cellspacing="0" style="width: 100%; background-color: Yellow">
                    <tr>
                        <td style="padding-left: 2px;">
                            <a href="javascript:void(0)" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true">
                                清空</a>
                        </td>
                        <td style="text-align: right; padding-right: 2px">
                            <%-- <input class="easyui-searchbox" data-options="prompt:'Please input somthing'" style="width:150px"></input>--%>
                        </td>
                    </tr>
                </table>
                <form id="editContainerSpecForm" method="post" style="">
                <table style="width: 80%;" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height: 24px">
                        <td style="width: 20%; text-align: right;" nowrap>
                            <span>供应商：</span>
                        </td>
                        <td style="width: 80%">
                            <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
                        </td>
                    </tr>
                    <tr style="height: 24px">
                        <td style="text-align: right;" nowrap>
                            <span>规格：</span>
                        </td>
                        <td>
                            <input class="easyui-combobox" name="language" data-options="
                    url:'<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerSpecList',
                    method:'get',
                    valueField:'id',
                     textField:'spec',
                    panelHeight:'auto'
            " />
                            <input class="easyui-validatebox" id="description" name="description" type="text"
                                style="width: 300px; display: none" />
                        </td>
                    </tr>
                    <tr style="height: 24px">
                        <td style="text-align: right;" nowrap>
                            <span>数量：</span>
                        </td>
                        <td>
                            <input class="easyui-numberbox" />
                            <input class="easyui-validatebox" id="Text1" name="description" type="text" style="width: 300px;
                                display: none" />
                        </td>
                    </tr>
                </table>
                </form>
                </div>
                </div>
        <div id="dlg-buttons">
            <a href="javascript:void(0)" class="easyui-linkbutton" onclick="javascript:alert('save')">
                确定</a> <a href="javascript:void(0)" class="easyui-linkbutton" onclick="javascript:$('#dlg').dialog('close')">
                    关闭</a>
        </div>
   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var docNumber = "";
        var supplerId = "";
        var jsonStr=null;
        var rows = new Array();
        var wrong = new Array();
        var errorRows = new Array();
        var deleteUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ReadLogDelet/';
        var readUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ReadContainerList/';
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/StoreOut/';         
        function clearSelect() {
            $('#dgSuplier').datagrid('clearChecked');
            $('#dgSuplier').datagrid('clearSelections');
            $('#dgSuplier').datagrid('reload');
        }
        $(function () {
            $('#dgContainer').datagrid({
                rowStyler: function (index, row) {
                    if (row.status > 3 || row.status == 0 || row.status == 7) {
                        return 'background-color:red;color:black;font-weight:bold;';
                    }
                }
            });
        });
        function statu(value, row, index) {
            e = "";
            if (row.status < 4) {
                e = value;
            }
            if (row.status == 4) {
                e = "未注册";
            }
            if (row.status == 5) {
                e = "未绑定";
            }
            if (row.status == 6) {
                e = "重复出库";
            }
            if (row.status == 7) {
                e = "未入库，不能出库";
            }
            return e;
        }
        function Connect() {
            $.messager.alert('确认', '连接成功', 'info');
        }
        function Read() {
          //  rows.length = 0;
            var rows = $('#dgSuplier').datagrid('getChecked');
            if (rows.length==0) {
                supplerId = "";
            }
            else {
                supplerId = rows[0].id;
            }
            var options = $('#dgContainer').datagrid('options');
            options.url = readUrl;
            options.queryParams.secods = supplerId;
            options.queryParams.direction =' STOREOUT';
            $('#dgContainer').datagrid('reload');
            $('#dgSuplier').datagrid('reload');
        }
        function deleteEx() {
            $.each($('#dgContainer').datagrid('getData').rows, function (key, value) {
                if (!jQuery.isEmptyObject(value)) {
                    if (value.status == 2 || value.status == 3) {
                        rows.push(value);
                    }
                    else {
                        errorRows.push(value);
                    }
                }
            });
            if (errorRows.length > 0) {
                jsonStr = $.toJSON(errorRows);
                $.ajax({
                    url: deleteUrl,
                    type: "post",
                    dataType: "json",
                    data: { data: jsonStr,
                        direction: 'STOREOUT'
                    },
                    success: function (r) {
                        if (r.result) {
                            $('#dgContainer').datagrid('reload');
                            $.messager.alert('确认', '删除成功！', 'info');
                            errorRows.length = 0;
                            rows.length = 0;
                        } else {
                            $.messager.alert('确认', '删除失败:' + r.message, 'error');
                        }
                    }
                });
            }         
        }
        function Summit() {
             rows = $('#dgContainer').datagrid('getRows');
            if (jQuery.isEmptyObject(rows)) {
                $.messager.alert('错误', '没有数据', 'error');
                return;
            }
            else {
             if ($('#dgSuplier').datagrid('getRows').length > 1) {
                    if ($('#dgSuplier').datagrid('getChecked').length==0) {
                        $.messager.alert('错误', '请选择供应商', 'error');
                        return;
                    }
                $.each(rows, function (key, value) {
                    if (!jQuery.isEmptyObject(value)) {
                        if (value.status > 3 || value.status == 0 || value.status == 1) {
                            wrong.push(value);
                            return false;
                        }
                    }
                });
                if (wrong.length > 0) {
                    $.messager.alert('错误', '有异常数据，无法提交！', 'error');
                    wrong.length=0;
                    return;
                }
            }
            jsonStr = $.toJSON(rows);       
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                data: { data: jsonStr,
                    PdocNumber: docNumber
                },
                success: function (r) {
                    if (r.result) {
                        docNumber = r.message;
                        rows.length = 0;
                        wrong.length = 0;
                        $.messager.alert('确认', '出库成功！', 'info');                       
                        if ($("#print")[0].checked) {
                            Print(r.message);
                        }
                        $('#dgContainer').datagrid('loadData', { total: 0, rows: [] });
                    } else {
                        $.messager.alert('确认', '出库失败:' + r.message, 'error');
                    }
                }
            });
        }
        function Print(docNumber) {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/ContainerHistoryPrint.aspx?docNumber=' + docNumber;
            //            url += "&direction=" + $('#queryDirection').val();
            //            url += "&startTime=" + $("#queryStartTime").datebox('getText');
            //            url += "&endTime=" + $("#queryEndTime").datebox('getText');
            parent.openTab('容器出入库单据打印' + docNumber.toString(), url);
        }
        function Delete() {
            var drows = $('#dgContainer').datagrid('getChecked');
            $.each(drows, function (key, value) {
                if (!jQuery.isEmptyObject(value)) {                   
                        wrong.push(value);
                        return false;                   
                }
            });
            if (wrong.length > 0) {
                jsonStr = $.toJSON(wrong);
                $.ajax({
                    url: deleteUrl,
                    type: "post",
                    dataType: "json",
                    data: { data: jsonStr,
                        direction: 'STOREOUT'
                    },
                    success: function (r) {
                        if (r.result) {
                            if (!jQuery.isEmptyObject(drows)) {
                                do {
                                    var index = $('#dgContainer').datagrid('getRowIndex', $('#dgContainer').datagrid('getChecked')[0]); //获取某行的行号		
                                    $('#dgContainer').datagrid('deleteRow', index); //通过行号移除该行	
                                }
                                while (!jQuery.isEmptyObject($('#dgContainer').datagrid('getChecked')));
                            }
                            wrong.length = 0;
                            $.messager.alert('确认', '删除成功！', 'info');
                        } else {
                            $.messager.alert('确认', '删除失败:' + r.message, 'error');
                        }
                    }
                });
            }
        }
    </script>
    <style type="text/css">
        #dgSuplier
        {
            font-size: larger;
        }
    </style>
    <script type="text/javascript">
        //        var editIndex = undefined;
        //        function endEditing() {
        //            if (editIndex == undefined) { return true }
        //            if ($('#Table1').datagrid('validateRow', editIndex)) {
        //                var ed = $('#Table1').datagrid('getEditor', { index: editIndex, field: 'spec' });
        //                var productname = $(ed.target).combobox('getText');
        //                $('#Table1').datagrid('getRows')[editIndex]['spec'] = productname;
        //                $('#Table1').datagrid('endEdit', editIndex);
        //                editIndex = undefined;
        //                return true;
        //            } else {
        //                return false;
        //            }
        //        }
        //        function onClickCell(index, field) {
        //            if (editIndex != index) {
        //                if (endEditing()) {
        //                    $('#dg').datagrid('selectRow', index)
        //                            .datagrid('beginEdit', index);
        //                    var ed = $('#dg').datagrid('getEditor', { index: index, field: field });
        //                    ($(ed.target).data('textbox') ? $(ed.target).textbox('textbox') : $(ed.target)).focus();
        //                    editIndex = index;
        //                } else {
        //                    $('#dg').datagrid('selectRow', editIndex);
        //                }
        //            }
        //        }
        //        function append() {
        //            if (endEditing()) {
        //                $('#Table1').datagrid('appendRow', { supplierName: '22222222' });
        //                editIndex = $('#Table1').datagrid('getRows').length - 1;
        //                $('#Table1').datagrid('selectRow', editIndex)
        //                        .datagrid('beginEdit', editIndex);
        //            }
        //        }
        //        function onClickRow(index) {
        //            if (editIndex != index) {
        //                if (endEditing()) {
        //                    $('#Table1').datagrid('selectRow', index)
        //							.datagrid('beginEdit', index);
        //                    editIndex = index;
        //                } else {
        //                    $('#Table1').datagrid('selectRow', editIndex);
        //                }
        //            }
        //        }
        //        function removeit() {
        //            if (editIndex == undefined) { return }
        //            $('#dg').datagrid('cancelEdit', editIndex)
        //                    .datagrid('deleteRow', editIndex);
        //            editIndex = undefined;
        //        }
        //        function accept() {
        //            //            if (endEditing()) {
        //            $('#Table1').datagrid('acceptChanges');
        //            //}
        //        }
        //        function reject() {
        //            $('#Table1').datagrid('rejectChanges');
        //            editIndex = undefined;
        //        }
        //        function getChanges() {
        //            var rows = $('#dg').datagrid('getChanges');
        //            alert(rows.length + ' rows are changed!');
        //        }
    </script>
</asp:Content>
