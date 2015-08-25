<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	子库存转移查询
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px;overflow:auto;">
        <table id="dataGridWmsStockTrans" title="子库存转移查询" style="" border="false" fit="true" singleSelect="true"
			idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb1">
		</table>
        <div id="tb1">
            单号：<input id="docNumber" type="text" style="width:100px;" />
            更新时间：<input class="easyui-datebox" id="queryStartModifyTime" type="text" style="width:100px;" value="<%=ViewData["dtStart"] %>" />-<input class="easyui-datebox" id="queryEndModifyTime" type="text" style="width:100px;" value="<%=ViewData["dtEnd"] %>" />
            
			<span> 上传状态： </span>
			<select id="queryReturnCode" name="queryReturnCode" style="width:80px;">
				<option value="" selected="selected">所有</option>
                <option value="0" >正常</option>
				<option value="-1" >异常</option>
            </select>
			<input type="checkbox" id="watchPersonal" name="only" checked="checked"/>只看自己
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="Query();">查询</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="Print();">打印</a>
             
             <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-ok" onclick="Submit();">提交</a>`
            
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
        </div>
        
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $('#dataGridWmsStockTrans').datagrid({
                rownumbers: true,
                pagination: true,
                singleSelect : true,
                checkOnSelect: false,
                selectOnCheck: false,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'ck',checkbox:true},
                    { field: 'docNumber', title: '转移单号', width: 150, align: 'center' },
                    { field: 'manualDocNumber', title: '手工单号', width: 150, align: 'center' }
                ]],
                columns: [[
                    { field: 'fromWarehouseId', title: '来源仓库', width: 120, align: 'center' },
                    { field: 'toWarehouseId', title: '目标仓库', width: 120, align: 'center' },
                    { field: 'transType', title: '事务类型', width: 150, align: 'center' },
//                  { field: 'createTime', title: '创建时间', width: 150, align: 'center' },
                    { field: 'modifyTime', title: '修改时间', width: 150, align: 'center' },
//                  { field: 'responsiblePerson', title: '责任人', width: 100, align: 'center' },
                    { field: 'statusCn', title: '状态', width: 60, align: 'center' },
                    { field: 'createUserName', title: '创建人', width: 100, align: 'center' },
                    { field: 'modifyUserName', title: '修改人', width: 100, align: 'center' },
                    { field: 'fromWarehouseUserName', title: '来源子库仓管员', width: 100, align: 'center' },
                    { field: 'toWarehouseUserName', title: '目标子库组长', width: 100, align: 'center' },
                    { field: 'meno', title: '备注', width: 300, align: 'center' },
					{ field: 'uploadStatusCn', title: '上传状态', width: 100, align: 'center' },
					{ field: 'uploadTimeShow', title: '上传日期', width: 100, align: 'center' }
                ]],
                onSelect: function (rowIndex, rowData) {
                    currentId = rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    WmsStockTransMainView();
                },
                onLoadSuccess: function () {
                    $(this).datagrid('clearSelections');
                    $(this).datagrid('clearChecked');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            });
        });

        function Query() {
            var options = $('#dataGridWmsStockTrans').datagrid('options');
            if ($('#watchPersonal').attr("checked") == "checked") {
                options.queryParams.user = '<%=Page.User.Identity.Name %>';
            } else {
                options.queryParams.user = "";
            }
            options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransMainList';
            options.queryParams.queryWord = $('#docNumber').val();
            options.queryParams.startPlanTime = $('#queryStartModifyTime').datebox('getText');
            options.queryParams.endPlanTime = $('#queryEndModifyTime').datebox('getText');
			options.queryParams.returnCode = $('#queryReturnCode').val();
            $('#dataGridWmsStockTrans').datagrid('reload');
        }
        var currentId = null;
        function WmsStockTransMainView() {
            var selectRow = $('#dataGridWmsStockTrans').datagrid('getSelected');
            if (selectRow) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransMainView?id=' + selectRow.id;
                parent.openTab('库存转移单_' + selectRow.docNumber, url);
            } else {
                $.messager.alert('提示', '请选择库存转移单！', 'info');
            }
        }
        var statusOption = "";       
        function Submit() {
            var selectRow = $('#dataGridWmsStockTrans').datagrid('getSelected');
            statusOption = "pending"; //状态改为待审核

            if (!selectRow) {
                $.messager.alert('提示', '请选择要提交的转移单', 'info');
                return;
            }
            //如果流程不属于启动人
            if (selectRow.createUser != '<%=Page.User.Identity.Name %>'){
                $.messager.alert('提示', '该流程不属于当前用户！', 'info');
                return;
            }
            if (selectRow.status != "draft") {
                $.messager.alert('提示', '提交的库存转移单必须是草稿！', 'info');
                return;
            }
            $.messager.confirm("确认", "是否提交？", function (result) {
                if (result) {
                    $.ajax({
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransUpdate',
                        type: "post",
                        dataType: "json",
                        data: { "id": selectRow.id, "statusOption": statusOption },
                        success: function (r) {
                            if (r.result) {
                                $('#dataGridWmsStockTrans').datagrid('reload');
                            } else {
                                $.messager.alert('错误', '提交失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        
        
        function Delete(){
            var selectRows = $('#dataGridWmsStockTrans').datagrid('getChecked');
            if (selectRows.length == 0) {
                $.messager.alert('提示', '请勾择要删除的转移单', 'info');
                return;
            }
            //如果流程不属于启动人
            for (var i = 0; i < selectRows.length; i++) {
                if (selectRows[i].createUser != '<%=Page.User.Identity.Name %>') {
                    $.messager.alert('提示', '勾选的流程有不属于当前用户的！', 'info');
                    return;
                }
            }
            for(var i = 0;i < selectRows.length; i++){
                if(selectRows[i].status != "draft"){
                    $.messager.alert('提示', '删除的转移单必须是草稿！', 'info');
                    return;
                }
            }
            var docNumbers = "";
            var mainIds = "";
            for (var i = 0; i < selectRows.length; i++) {
                 if (docNumbers) {
                    docNumbers += ",</br>";
                    mainIds += ",";
                }
                docNumbers +=encodeURIComponent(selectRows[i].docNumber);
                mainIds +=encodeURIComponent(selectRows[i].id);
            }
            $.messager.confirm('确认', '确认删除转移单[<font color="red">' + docNumbers + '</font>]？', function (result) {
                if (result) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsStockTransDelete/';
                    $.ajax({
                        url: sUrl,
                        type: "post",
                        dataType: "json",
                        data: { mainIds: mainIds },
                        success: function (r) {
                            if (r.result) {
                                $('#dataGridWmsStockTrans').datagrid('reload');
                            } else {
                                $.messager.alert('确认', '删除失败:' + r.message, 'error');
                            }
                        }
                    });
                }
            });
        }
        function Print() {
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/WmsStockTransPrint.aspx';
            url += "?docNumber=" + $('#docNumber').val();
            url += "&startModifyTime=" + $("#queryStartModifyTime").datebox('getText');
            url += "&endModifyTime=" + $("#queryEndModifyTime").datebox('getText');
            parent.openTab('子库存转移日志打印', url);
        }
    </script>
</asp:Content>
