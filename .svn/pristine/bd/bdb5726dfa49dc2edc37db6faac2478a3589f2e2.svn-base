<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    超期预警
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgContainerWarn" title="超期预警" style="" border="false" fit="true" singleselect="true"
            idfield="sn" sortname="sn" sortorder="asc" striped="true" toolbar="#tb1">
        </table>
    </div>
    <div id="tb1">
        <span>供应商：</span>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
        &nbsp;&nbsp;&nbsp;&nbsp; <span>超期日期：</span>
        <input id="txtDate" name="startDate" class="easyui-datebox" data-options="formatter:myformatter,parser:myparser"
            style="width: 150px" />
        &nbsp;&nbsp;&nbsp;&nbsp; <a href="javascript:void(0);" class="easyui-linkbutton"
            plain="true" icon="icon-search" onclick="Search();">查询</a> <a id="linkbtn_export" class="easyui-linkbutton" iconcls="icon-download" href="#" onclick ="ExportWar()"
                >导出</a><a href="javascript:void(0);"
                class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="ResetAll()">重置</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
            onclick="AddRule()">预警规则管理</a>
    </div>
    <div id="tbTool">
        <span>供应商：</span>
        <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { width = "222px" }); %>
        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
            onclick="SupplierSearch();">查询</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                plain="true" icon="icon-add" onclick="Edit(true);">增加</a> <a href="javascript:void(0);"
                    class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="Edit(false);">修改</a>
        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel"
            onclick="Save(true)">重置</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                plain="true" icon="icon-back" onclick="BackTo()">返回到预警</a>
    </div>
    <div id="editSupplier" class="easyui-window" title="供应商" style="padding: 10px; width: 640px;
        height: 480px;" iconcls="icon-edit" closed="true" maximizable="false" minimizable="false"
        resizable="false" collapsible="false" modal="true" shadow="true">
        <div class="easyui-layout" fit="true">
            <div region="center" border="false" style="padding: 10px; background: #fff; border: 1px solid #ccc;">
                <form id="editSupplierForm" method="post" style="">
                <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height: 24px">
                        <td style="width: 20%; text-align: right;" nowrap>
                            <span>供应商编号：</span>
                        </td>
                        <td style="width: 80%">
                            <input class="easyui-validatebox" required="true" id="id" name="id" type="text" style="width: 120px;
                                background-color: #CCCCCC;" readonly="readonly" />
                        </td>
                    </tr>
                    <tr style="height: 24px">
                        <td style="width: 20%; text-align: right;" nowrap>
                            <span>全称：</span>
                        </td>
                        <td>
                            <input class="easyui-validatebox" required="true" id="name" name="name" type="text"
                                style="width: 400px; background-color: #CCCCCC;" readonly="readonly" /><span style="color: Red;">*</span>
                        </td>
                    </tr>
                    <tr style="height: 24px">
                        <td style="text-align: right;" nowrap>
                            <span>预警时间：</span>
                        </td>
                        <td>
                            <input type="text" class="easyui-numberbox" id="warnHours" name="warnHours" />(小时)单位
                            <%--  </input> --%>
                            <%--<input class="easyui-validatebox" type="text" id="warnHours" name="warnHours" />--%>
                        </td>
                    </tr>
                    <tr style="height: 24px">
                        <td style="text-align: right;" nowrap>
                            <span>描述：</span>
                        </td>
                        <td>
                            <input class="easyui-validatebox" id="description" name="description" type="text"
                                style="width: 400px; background-color: #CCCCCC;" readonly="readonly" />
                        </td>
                    </tr>
                </table>
                </form>
            </div>
            <div region="south" border="false" style="text-align: right; height: 30px; line-height: 30px;">
                <a id="btnSave" class="easyui-linkbutton" iconcls="icon-ok" href="javascript:void(0)"
                    onclick="Save(false)">保存</a> <a class="easyui-linkbutton" iconcls="icon-cancel" href="javascript:void(0)"
                        onclick="$('#editSupplier').window('close');">取消</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        function myformatter(date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
        }
        function myparser(s) {
            if (!s) return new Date();
            var ss = (s.split('-'));
            var y = parseInt(ss[0], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[2], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d);
            } else {
                return new Date();
            }

        }  
    </script>
    <script type="text/javascript">
        var currentId = null;
        $(function () {
            $('#tbTool').hide();
            $('#dgContainerWarn').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerWarnList',
                pagination: true,
                pageSize: 50,
                remoteSort: false,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'sn', title: '容器编号', width: 110, align: 'center', sortable: 'true' }
                ]],
                columns: [[
                    { field: '_supplierName', title: '供应商全称', width: 250, align: 'left', sortable: 'true' },
                    { field: 'storeInTime', title: '入厂日期', width: 200, align: 'left', sortable: 'true' },
                    { field: 'deadline', title: '截止时间', width: 200, align: 'left', sortable: 'true' },
                    { field: 'extendedTime', title: '是否超期', width: 110, align: 'left',sortable:'true', formatter: function (value, row, index) { var e = ''; if (value > 0 || value ==0) { e = '已超期'; } if (value > -25 && value < 0) { e = '即将超期'; } return e; } }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                rowStyler: function (index, row) {
                    if (row.extendedTime > 0 || row.extendedTime==0) {
                        return 'background-color:red;color:black;font-weight:bold;';
                    }
                    if (row.extendedTime < 0 && row.extendedTime>-25) {
                        return 'background-color: #FFFF66;color:black;font-weight:bold;';
                    }
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });
        function Search() {
            var options = $('#dgContainerWarn').datagrid('options');
            options.queryParams.queryWord = $('#supplierSelect').combogrid('getValue'); ;
            //options.queryParams.supplierName = $('#txtSupplerName').val();
            options.queryParams.DateRequired = $('#txtDate').datebox('getValue');
            //options.queryParams.extendedTime = $('#txtextendedTime').val();
            $('#dgContainerWarn').datagrid('reload');
        }
        function ResetAll() {
            $('#supplierSelect').combogrid('setValue', '');
            $('#txtDate').datebox('setValue', '');
            //  $('#txtextendedTime').val("");
        }
        function AddRule() {
            $('#tb1').hide();
            $('#dgContainerWarn').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/WarnRuleList',
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                toolbar: '#tbTool',
                frozenColumns: [[
                    { field: 'id', title: '供应商编号', width: 10, align: 'center', hidden: 'true' },
                //{ field: 'id', title: 'ID', width: 110, align: 'center' },
                    {field: 'docNumber', title: '供应商编号', width: 110, align: 'center' }
                ]],
                columns: [[
                    { field: 'name', title: '供应商全称', width: 400, align: 'left' },
                    { field: 'warnHours', title: '报警时间', width: 70, align: 'left' },
                    { field: 'description', title: '描述', width: 70, align: 'left' }
                //{ field: 'warnTime', title: '预警时间', width: 70, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                },
                onDblClickRow: function (rowIndex, rowData) {
                    Edit(false);
                }
            });

        }
        function Edit(boolAdd) {
            var selectRow;
            if (boolAdd) {
                selectRow = $('#supplierSelect').combogrid('getValue');
            }
            else {
                selectRow = $('#dgContainerWarn').datagrid('getSelected');

            }
            if (selectRow) {
                var sUrl;
                $('#editSupplier').window('open');
                if (boolAdd) {
                    sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierEdit/' + selectRow;
                    currentId = selectRow;
                }
                else {
                    sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierEdit/' + selectRow.id;
                    currentId = selectRow.id;
                }
                // var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editSupplierForm")[0].reset();
                        $('#id').val(r.id);
                        $('#name').val(r.name);
                        $('#description').val(r.description);
                        $("#warnHours").numberbox('setValue', r.warnHours);
                        //$("#warnHours").val(r.warnHours);
                        $('#warnHours').focus();
                    }
                });
                //currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择供应商', 'info');
            }
        }
        function checkInfo(ascmSupplier_Model, id) {
            this.ascmSupplier_Model = ascmSupplier_Model;
            this.id = id;
        }
        function Save(booldelete) {
            if (booldelete) {
                if (!jQuery.isEmptyObject($('#dgContainerWarn').datagrid('getSelected'))) {
                    $.messager.confirm("确认", "确认重置！", function (r) {
                        if (r) {
                            currentId = $('#dgContainerWarn').datagrid('getSelected').id;
                            $.ajax({
                                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/SupplierDelete/',
                                type: "post",
                                dataType: "json",
                                data: { id: currentId },
                                success: function (r) {
                                    // var rVal = $.parseJSON(r);
                                    if (r.result) {
                                        //$('#editSupplier').window('close');
                                        // currentId = rVal.id;
                                        $('#dgContainerWarn').datagrid('reload');
                                    } else {
                                        $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                                    }
                                },
                                error: function () {
                                    //Ischeck = false;
                                    $.messager.alert('失败', '失败:请重试', 'error');
                                }
                            });
                            return;
                        }

                        return;

                    });
                }
                else {
                    $.messager.alert('提示', '请选择供应商', 'info');
                }
            }
            else {
                $.messager.confirm("确认", "确认提交保存?", function (r) {
                    if (r) {
                        $('#editSupplierForm').form('submit', {
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierSave/' + currentId,
                            onSubmit: function () {
                                $("#warnHours").val($("#warnHours").val());
                                return $('#editSupplierForm').form('validate');

                            },
                            success: function (r) {
                                var rVal = $.parseJSON(r);
                                if (rVal.result) {
                                    $('#editSupplier').window('close');
                                    currentId = rVal.id;
                                    $('#dgContainerWarn').datagrid('reload');
                                } else {
                                    $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                                }
                            }
                        });
                    }
                });
            }
        }
        function SupplierSearch() {
            var g = $('#supplierSelect').combogrid('grid'); // get datagrid object
            // var r = g.datagrid('getSelected');
            var options = $('#dgContainerWarn').datagrid('options');
            if (!jQuery.isEmptyObject(g.datagrid('getSelected'))) {
                options.queryParams.queryWord = g.datagrid('getSelected').name;
            }
            else {
                $('#supplierSelect').combogrid('setValue', '');
            }
            g.datagrid('clearSelections');

            $('#dgContainerWarn').datagrid('reload');
            options.queryParams.queryWord = "";
        }
        function BackTo() {
            $('#tbTool').hide();
            $('#tb1').show();
            $('#dgContainerWarn').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerWarnList',
                pagination: true,
                toolbar: '#tb1',
                pageSize: 50,
                fitColumns: true,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'sn', title: '容器编号', width: 110, align: 'center' }
                ]],
                columns: [[
                    { field: '_supplierName', title: '供应商全称', width: 110, align: 'left' },
                    { field: 'storeInTime', title: '入厂日期', width: 70, align: 'left' },
                    { field: 'deadline', title: '截止时间', width: 70, align: 'left' },
                    { field: 'extendedTime', title: '是否超期', width: 110, align: 'left', formatter: function (value, row, index) { var e = ''; if (value > 0 || value == 0) { e = '已超期'; } if (value > -25 && value < 0) { e = '即将超期'; } return e; } }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                rowStyler: function (index, row) {
                    if (row.extendedTime > 0 || row.extendedTime == 0) {
                        return 'background-color:red;color:black;font-weight:bold;';
                    }
                    if (row.extendedTime < 0 && row.extendedTime > -25) {
                        return 'background-color: #FFFF66;color:black;font-weight:bold;';
                    }
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                },
                onDblClickRow: function (rowIndex, rowData) {
                    return;
                }
            });
        }
          function ExportWar() {
           Search();
          var adress="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ExprotWarmContainerDate/?queryWord="+$('#supplierSelect').combogrid('getValue')+"&DateRequired ="+$('#txtDate').datebox('getValue')+"";
          $('#linkbtn_export').attr('href', adress);
        }
    </script>
</asp:Content>
