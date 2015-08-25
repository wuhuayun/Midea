<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    容器流转历史
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div id="Tb1Div" region="center" title="" border="true" style="padding: 0px; overflow: auto;">
        <table id="dgStoreInOut" title="容器流转历史查询" style="" border="false" fit="true" singleselect="true"
            idfield="id" sortname="id" sortorder="asc" striped="true" toolbar="#tb1">
        </table>
        <div id="tb1">
            <span>按单号查询：</span>
            <input id="docNumber" name="docNumber" type="text" style="width: 120px; text-align: left;" />
            <span>供应商：</span> <span id="spanSupplier">
                <%Html.RenderPartial("~/Areas/Ascm/Views/Purchase/SupplierSelectCombo.ascx", new MideaAscm.Code.SelectCombo { id = "supplierId" }); %></span>
            <span>方向：</span>
            <select id="queryDirection" name="queryDirection" style="width: 100px;">
                <option value=""></option>
                <% List<string> listDirectionDefine = MideaAscm.Dal.ContainerManage.Entities.AscmStoreInOut.DirectionDefine.GetList(); %>
                <% if (listDirectionDefine != null && listDirectionDefine.Count > 0)
                   { %>
                <% foreach (string directionDefine in listDirectionDefine)
                   { %>
                <option value="<%=directionDefine %>">
                    <%=MideaAscm.Dal.ContainerManage.Entities.AscmStoreInOut.DirectionDefine.DisplayText(directionDefine)%></option>
                <% } %>
                <% } %>
            </select>
            <span>时间段：</span><input class="easyui-datebox" id="queryStartTime" type="text" style="width: 100px;" />~<input
                class="easyui-datebox" id="queryEndTime" type="text" style="width: 100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
                onclick="Query();">查询</a> <a href="javascript:void(0);" class="easyui-linkbutton"
                    plain="true" icon="icon-print" onclick="Print();">打印单据</a>&nbsp;&nbsp;&nbsp;&nbsp;
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add"
                onclick="flowInfo();">流转历史查询</a>
            <br />
            <%if (ynWebRight.rightDelete)
              { %>
            &nbsp;&nbsp; <a href="javascript:void(0);" class="easyui-linkbutton" plain="true"
                icon="icon-remove" onclick="Delete(false)">删除单条</a>&nbsp; <a href="javascript:void(0);"
                    class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete(true)">
                    按单号删除</a>
            <%}%>
        </div>
    </div>
      <div id="tb2">
        <span>标签编号：</span>
        <input id="txtSn" name="sn" type="text" style="width: 100px;" />
        <% List<string> listBindTypeDefine = MideaAscm.Dal.Base.Entities.AscmRfid.BindTypeDefine.GetList(); %>
        <span>类型：</span>
        <select id="QTagType" name="addTagType" style="width: 120px;">
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
        &nbsp;&nbsp;&nbsp;&nbsp; <span>需求时间：</span>
        <input class="easyui-datebox" id="BtxtRequiredTime" type="text" style="width: 120px;" />~
        <input class="easyui-datebox" id="EtxtRequiredTime" type="text" style="width: 120px;" />
        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search"
            onclick="FlowQuery();">查询</a>
            <a id="linkbtn_export" class="easyui-linkbutton" iconcls="icon-download" href="#" onclick ="ExportFlow()"
                >导出</a>&nbsp;&nbsp;&nbsp;&nbsp; <a href="javascript:void(0);"
                class="easyui-linkbutton" plain="true" icon="icon-add" onclick="RerurnInfo();">返回流转历史查询</a>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
   <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/easyui/datagrid-detailview.js"></script>
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var dgView = null;
        $(function () {
            $('#QTagType').change(function () {
                var options = $('#dgStoreInOut').datagrid('options');
                options.queryParams.bindType = $('#QTagType').val();
                options.queryParams.BRequiredTime = $('#BtxtRequiredTime').datebox('getText');
                options.queryParams.ERequiredTime = $('#EtxtRequiredTime').datebox('getText');
                $('#dgStoreInOut').datagrid('reload');

            });
            $('#tb2').hide();
            $('#dgStoreInOut').datagrid({
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerHistoryList/',
                columns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'docNumber', title: '单号', width: 80, align: 'left' },
                    { field: 'containerId', title: '容器编号', width: 80, align: 'left' },
                    { field: 'epcId', title: 'EPC', width: 80, align: 'left' },
                    { field: 'specName', title: '规格', width: 60, align: 'left' },
                    { field: 'supplierNameCN', title: '供应商', width: 180, align: 'left' },
                    { field: 'statusCN', title: '当前状态', width: 100, align: 'left' },
                    { field: 'directionCN', title: '方向', width: 100, align: 'left' },
                    { field: 'readTimeCN', title: '时间', width: 120, align: 'left' },
                    { field: 'createUser', title: '创建人', width: 100, align: 'left' },
                    { field: 'tip', title: '备注', width: 100, align: 'left' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    dgView = $('#dgStoreInOut').datagrid('options').view.render;
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
            //     <%-- 初始化默认日期为当天 --%>
            //            var date = new Date();
            //            var yyyy = date.getFullYear();
            //            var MM = date.getMonth() + 1;
            //            MM = MM < 10 ? "0" + MM : MM;
            //            var dd = date.getDate();
            //            dd = dd < 10 ? "0" + dd : dd;
            //            $("#queryStartTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
            //            $("#queryEndTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        });
        function Query() {
            var options = $('#dgStoreInOut').datagrid('options');
            options.queryParams.supplierId = $('#supplierId').combogrid('getValue');
            options.queryParams.direction = $('#queryDirection').val();
            options.queryParams.queryStartTime = $("#queryStartTime").datebox('getText');
            options.queryParams.queryEndTime = $("#queryEndTime").datebox('getText');
            options.queryParams.docNumber = $("#docNumber").val();
            $('#dgStoreInOut').datagrid('reload');
        }
        var currentId = null;
        function Print() {
            if ($('#queryDirection').val() != "") {
                if (!($('#supplierId').combogrid('getValue') != "" || $("#queryStartTime").datebox('getText') != "" || $("#queryEndTime").datebox('getText') != "")) {
                    $.messager.alert('错误', '请选择供应商或者时间段！', 'error');
                    return;
                }
                if ($("#docNumber").val() != "") {
                    $.messager.alert('错误', '不能填写单号！', 'error');
                    return;
                }
            }
            if ($("#docNumber").val() == "" && $('#queryDirection').val() == "") {
                $.messager.alert('错误', '请填写单据号！', 'error');
                return;
            }
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/ContainerHistoryPrint.aspx?supplierId=' + $('#supplierId').combogrid('getValue');
            url += "&direction=" + $('#queryDirection').val();
            url += "&startTime=" + $("#queryStartTime").datebox('getText');
            url += "&endTime=" + $("#queryEndTime").datebox('getText');
            url += "&docNumber=" + $("#docNumber").val();
            parent.openTab('容器出入库单据打印', url);
        }
        function flowInfo() {
            $('#tb1').hide();
            $("#tb2").show();
            $('#dgStoreInOut').datagrid({
                pagination: true,
                toolbar: '#tb2',
                pageSize: 50,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ListflowInfo/',
                frozenColumns: [[
                // { field: 'id', hidden: 'true' },
                    {field: 'id', title: '流转编号', width: 100, align: 'left', hidden: 'true' },
                    { field: 'objectID', title: '编号', width: 100, align: 'left' },
                    { field: 'supplierName', title: '供应商', width: 240, align: 'left' }
                ]],
                columns: [[                   
                    { field: '_bindType', title: '类型', width: 180, align: 'center' },
                    { field: 'readTime', title: '读取时间', width: 140, align: 'left' },
                    { field: 'ReadingHeadAddress', title: '位置', width: 120, align: 'center' }
                //                    { field: 'description', title: '描述', width: 100, align: 'center' },
                //                    { field: 'tip', title: '备注', width: 120, align: 'center' },
                //                    { field: '_status', title: '状态', width: 100, align: 'center' }
                ]],
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div style="padding:2px"><table id="ddv-' + index + '"></table></div>';
                },
                onExpandRow: function (index, row) {
                    $('#ddv-' + index).datagrid({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/GetFlowDetailInfo/?sn=' + row.epcId+'',
                        fitColumns: true,
                        singleSelect: true,
                        rownumbers: true,
                        loadMsg: '',
                        height: 'auto',
                        columns: [[
                { field: 'readTime', title: '读取时间', width: 120, align: 'left' },
                { field: 'ReadingHeadAddress', title: '位置', width: 100, align: 'left' },
                { field: '', title: '', width: 100, align: 'left' }
               ]],
                        onResize: function () {
                            $('#dgStoreInOut').datagrid('fixDetailRowHeight', index);
                        },
                        onLoadSuccess: function () {
                            setTimeout(function () {
                                $('#dgStoreInOut').datagrid('fixDetailRowHeight', index);
                            }, 0);
                        }
                    });
                    $('#dgStoreInOut').datagrid('fixDetailRowHeight', index)
                },
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        }
        function FlowQuery() {
            var options = $('#dgStoreInOut').datagrid('options');
            options.queryParams.bindType = $('#QTagType').val();
            options.queryParams.BRequiredTime = $('#BtxtRequiredTime').datebox('getText');
            options.queryParams.ERequiredTime = $('#EtxtRequiredTime').datebox('getText');
            options.queryParams.sn = $('#txtSn').val();
            $('#dgStoreInOut').datagrid('reload');
        }
        function RerurnInfo() {
            $('#tb2').hide();
            $("#tb1").show();
             $('#dgStoreInOut').datagrid({
                pagination: true,
                toolbar: '#tb1',
                pageSize: 50,
                view: $.fn.datagrid.defaults.view,
                loadMsg: '更新数据......',
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ContainerHistoryList/',
                frozenColumns: [[
                    { field: 'id', hidden: 'true' },
                    { field: 'docNumber', title: '单号', width: 80, align: 'left' },
                    { field: 'containerId', title: '容器编号', width: 80, align: 'left' },
                    { field: 'epcId', title: 'EPC', width: 80, align: 'center' },
                    { field: 'specName', title: '规格', width: 60, align: 'left' },
                    { field: 'supplierNameCN', title: '供应商', width: 180, align: 'center' },
                    { field: 'statusCN', title: '当前状态', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'directionCN', title: '方向', width: 100, align: 'center' },
                    { field: 'readTimeCN', title: '时间', width: 120, align: 'center' },
                    { field: 'createUser', title: '创建人', width: 100, align: 'center' },
                    { field: 'tip', title: '备注', width: 100, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
            //            <%-- 初始化默认日期为当天 --%>
            //            var date = new Date();
            //            var yyyy = date.getFullYear();
            //            var MM = date.getMonth() + 1;
            //            MM = MM < 10 ? "0" + MM : MM;
            //            var dd = date.getDate();
            //            dd = dd < 10 ? "0" + dd : dd;
            //            $("#queryStartTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
            //            $("#queryEndTime").datebox("setValue", yyyy + "-" + MM + "-" + dd);
        }



        function Delete(IsDoc) {
            var selectRow = $('#dgStoreInOut').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除？', function (result) {
                    if (result) {
                        var doc;  //编号
                        // var isDocNember; //是否是单号
                        if (IsDoc) {
                            doc = selectRow.docNumber;
                            // isDocNember = false;
                        } else {
                            doc = selectRow.id;
                            //isDocNember = true;
                        }
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/DeleteRecod/';
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            data: { docNumber: doc,
                                IsDoc: IsDoc
                            },
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
                $.messager.alert('提示', '请选择要删除的记录', 'info');
            }
        }
        function ExportFlow() {
        FlowQuery();
          var adress="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/ContainerManage/ExportflowInfExcell/?bindType="+$('#QTagType').val()+"&BRequiredTime="+$('#BtxtRequiredTime').datebox('getText')+"&ERequiredTime="+ $('#EtxtRequiredTime').datebox('getText')+"&sn ="+ $('#txtSn').val()+"";
            $('#linkbtn_export').attr('href', adress);
        }
       
    </script>
</asp:Content>
