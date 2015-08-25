<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	供应商与物料关联
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <table id="dataGridMaterial" title="供应商物料信息" style="" border="false" fit="true" checkOnSelect="false" selectOnCheck="false"  singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tbMaterial">
		</table>
        <div id="tbMaterial">
            <span>物料编码：</span><input id="startInventoryItemId" type="text" style="width:100px;" />-<input id="endInventoryItemId" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="MaterialSearch();"></a>
            <% if (ynWebRight.rightVerify){ %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="MaterialPrint();">打印物料条码</a>
            <%} %>
            <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ImportExcel();">导入</a>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="ExportExcel();">模板导出</a>
            <% } %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="Delete();">删除</a>
            <%} %>
            <iframe id="iframeExportToWord" name="iframeExportToWord" scrolling="auto" frameborder="0"  src="" style="width:0px;height:0px;display:none;"></iframe> 
        </div>
        <div id="popImport" class="easyui-window" title="数据导入" style="padding: 10px;width:380px;height:200px;"
            iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div class="easyui-layout" fit="true">
                <div region="center" id="FileUpload" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
					<form id="FormUpload" enctype="multipart/form-data" method="post" ><br />
                        <p><input type="file" id="fileImport" name="fileImport" size="35" value=''/>&nbsp;&nbsp;</p>
                    </form>
                </div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
					<a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="ImportOk()">保存</a>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#popImport').window('close');">取消</a>
				</div>
            </div>
        </div>
        <div id="progressbarDiv" class="easyui-window" title="" style="padding: 12px;width:460px;height:80px;"
            closed="true" closable="false" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
            <div id="p" class="easyui-progressbar" style="width:400px;"></div> 
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var currentMaterialId = null;
        var vSupplierId=null;
        <% if (ViewData.Keys.Contains("supplierId")){ %>
            vSupplierId=<%=ViewData["supplierId"] %> 
        <%} %>
        $(function () {
            $('#dataGridMaterial').datagrid({
                rownumbers:true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                queryParams: { supplierId:vSupplierId },
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/SupplierMaterialList/',
                frozenColumns: [[
                    {checkbox:true},
                    { field: 'id', title: 'ID', hidden:true},
                    { field: 'docNumber', title: '编号', width: 100, align: 'center' }
                ]],
                columns: [[
                    { field: 'unit', title: '单位', width: 50, align: 'center' },
                    { field: 'description', title: '描述', width: 400, align: 'left' },
                    { field: 'memo', title: '备注', width: 100, align: 'left' }
                ]],
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentMaterialId=rowData.userId;
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentMaterialId) {
                        $(this).datagrid('selectRecord', currentMaterialId);
                    }
                },
                onDblClickRow: function (rowIndex, rowData) {
                }
            });
        })
        function MaterialReload() {
            $('#dataGridMaterial').datagrid('reload');
        }
		function MaterialSearch(){
			var queryParams = $('#dataGridMaterial').datagrid('options').queryParams;
            queryParams.startInventoryItemId = $('#startInventoryItemId').val();
            queryParams.endInventoryItemId = $('#endInventoryItemId').val();
			$('#dataGridMaterial').datagrid('reload');
            $('#dataGridMaterial').datagrid('clearSelections');
            $('#dataGridMaterial').datagrid('clearChecked');
		}

        function MaterialPrint() {
            var selectRows = $('#dataGridMaterial').datagrid('getChecked');
            var ids=null;
            $.each(selectRows,function(index,item){
                if(ids!=null&&ids!="")
                    ids+=","
                ids+=item.id;
            });
            if (selectRows) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/SupplierMaterialPrint.aspx?ids=' + ids;
                parent.openTab('物料条码打印', url);
            } else {
                $.messager.alert('提示', '请选择要打印条码的物料', 'info');
            }
        }

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
            $('#progressbarDiv').window({ title: '导入进度' });
            $('#progressbarDiv').window('open');
            $('#p').progressbar('setValue', 0);
            setInterval(updateProgress, 600);
            $('#FormUpload').form('submit', {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/SupplierMaterialImport/?supplierId='+vSupplierId,
                onSubmit: function () {
                    return $('#FormUpload').form('validate');
                },
                success: function (result) {
                    $('#p').progressbar('setValue', 100);
                    $('#progressbarDiv').window('close');
                    var retValue = eval('(' + result + ')');
                    $('#popImport').window('close');
                    if (retValue.result) {
                        $.messager.alert('确认', retValue.message, '');
                        MaterialReload();
                    }
                    else {
                        $.messager.alert('错误', retValue.message, 'error');
                    }
                }
            });
        }
        function ExportExcel() {
              var url = "<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/供应商物料导入模板.xlsx";
              $('#iframeExportToWord').attr("src",url).trigger("beforeload");
        }
        function Delete()
        {
            var selectRows = $('#dataGridMaterial').datagrid('getChecked');
            if (selectRows.length>0) {
                var ids=null;
                $.each(selectRows,function(index,item){
                    if(ids!=null&&ids!="")
                        ids+=","
                    ids+=item.id;
                });
               $.messager.confirm('确认', '确认删除选中物料？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/SupplierMaterialDelete/?ids=' + ids+"&supplierId="+vSupplierId;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    MaterialReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的物料', 'info');
            }
        }
    </script>
</asp:Content>
