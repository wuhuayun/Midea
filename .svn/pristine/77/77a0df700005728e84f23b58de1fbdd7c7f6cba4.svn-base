<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    容器入库管理
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
     <div class="easyui-layout" style="width: 100%; height: 100%;">
        <div data-options="region:'west',collapsible:false" title="请在这里选择供应商" style="width: 380px">
            <table id="dgSuplier" title="请在这里选择供应商" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                          rownumbers: true,
                          singleSelect: true,
                          checkOnSelect: true, 
                          selectOnCheck: true,
                          url:'<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/SupplierSelectList/?IsOut=false',
                          remoteSort:false,
                          idField: 'id',
                          sortName: 'name',
                          sortOrder: 'desc',
                          striped: true,
                          toolbar:clearTool,
                          pagination: false,
                          loadMsg: '更新数据......',
                          onCheck: function(index,row){                          
                             read();
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
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
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
                </tr>
            </thead>
        </table>
        <div id="tb1" style="padding: 5px; height: auto;">
            <div>
                <%-- <span>连接地址：</span>
                <input id="ipAddress" name="ipAddress" type="text" value="192.168.1.100" style="width:100px;text-align:right;" />--%>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-remove" onclick="deleteEx();">一键清除异常数据</a>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="Read('');">读取</a>
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
			    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-save" onclick="Summit();">确认入库</a>
                <input type="checkbox" name="print" id="print"/> <span>打印单据</span>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var docNumber = "";
        var jsonStr = null;
        var rows = new Array();
        var wrong = new Array();
        var errorRows = new Array();
        var deleteUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ReadLogDelet/';
        var readUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ReadContainerList/';
        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/StoreIn/';       
        function clearSelect() {
            $('#dgSuplier').datagrid('clearChecked');
            $('#dgSuplier').datagrid('clearSelections');
            $('#dgSuplier').datagrid('reload');
        }
        $(function () {
            $('#dgContainer').datagrid({
                rowStyler: function (index, row) {
                    if (row.status > 3 || row.status == 0 || row.place == "容器监管中心") {
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
                e = "重复入库";
            }
            return e;
        }
        function Read(secods) {
            var rows = $('#dgSuplier').datagrid('getChecked');
            if (rows.length == 0) {
                supplerId = "";
            }
            else {
                supplerId = rows[0].id;
            }
            var options = $('#dgContainer').datagrid('options');
            options.url = readUrl;
            options.queryParams.secods = supplerId;
            options.queryParams.direction = ' STOREIN';
            $('#dgContainer').datagrid('reload');
            $('#dgSuplier').datagrid('reload');
            rows.length=0;
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

                }
                $.each(rows, function (key, value) {
                    if (!jQuery.isEmptyObject(value)) {
                        if (value.status > 3 || value.status == 0 || value.status == 1 || value.place == "容器监管中心" ) {
                            wrong.push(value);
                            return false;
                        }
                    }
                });
                if (wrong.length > 0) {
                    $.messager.alert('错误', '有异常数据，无法提交！', 'error');
                    wrong = new Array();
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
                        $.messager.alert('确认', '入库成功！', 'info');
                        if ($("#print")[0].checked) {
                            Print(r.message);
                        }
                        $('#dgContainer').datagrid('loadData', { total: 0, rows: [] });
                    } else {
                        $.messager.alert('确认', '入库失败:' + r.message, 'error');
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
            var drow = $('#dgContainer').datagrid('getChecked');
            if (!jQuery.isEmptyObject(drow)) {
                do {
                    var index = $('#dgContainer').datagrid('getRowIndex', $('#dgContainer').datagrid('getChecked')[0]); //获取某行的行号		
                    $('#dgContainer').datagrid('deleteRow', index); //通过行号移除该行	
                }
                while (!jQuery.isEmptyObject($('#dgContainer').datagrid('getChecked')));
            }
            if (drows.length > 0) {
                jsonStr = $.toJSON(drows);
                $.ajax({
                    url: deleteUrl,
                    type: "post",
                    dataType: "json",
                    data: { data: jsonStr,
                        direction: 'STOREIN'
                    },
                    success: function (r) {
                        if (r.result) {
                            $.messager.alert('确认', '删除成功！', 'info');

                        } else {
                            $.messager.alert('确认', '删除失败:' + r.message, 'error');
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
