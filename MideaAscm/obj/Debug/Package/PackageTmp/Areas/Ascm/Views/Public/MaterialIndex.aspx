<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	物料表
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 物料类型 -->
	 <%-- <div region="west" split="false" border="false" title="" style="width:360px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="物料类型" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" title="" border="false">
	            <div region="north" border="false" style="height:30px;overflow:hidden;">
                    <div style="float:left; height:26px; width:100%;background:#F4F4F4;border-bottom:1px solid #DDDDDD; padding:1px;">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="MaterialTypeReload();">刷新</a>
	                </div>
	            </div>
                <div region="center" border="false" style="background:#fff;">
                    <div class="easyui-panel" title="" data-options="fit:true,border:false">
	                    <ul id="treeMaterialType" class="easyui-tree" data-options="
			                    animate: true,
				                onBeforeLoad: function(node,param){
					                if (!node) {
                                        var url = '';
                                        $(this).tree('options').url = url;
                                        param.id = 0;
                                    }else{
                                        var url = '';
                                        $(this).tree('options').url = url;
                                    }
				                },
                                onSelect: function (node) {
                                    currentMaterialTypeId=null;
                                    currentMaterialTypeName='';

                                    
                                    MaterialReload();
                                }
		                    "></ul>
                    </div>
                </div>
			</div>
		</div>
	</div>--%>
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
        <table id="dataGridMaterial" title="物料信息" style="" border="false" fit="true" checkOnSelect="false" selectOnCheck="false"  singleSelect="true"
			idField="id" sortName="id" sortOrder="asc" striped="true" toolbar="#tbMaterial">
		</table>
        <div id="tbMaterial">
            <%--<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="MaterialReload();">刷新</a>
			<input id="materialSearch" type="text" style="width:100px;" />
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="MaterialSearch();"></a>--%>

            <span>物料编码：</span><input id="startInventoryItemId" type="text" style="width:100px;" />-<input id="endInventoryItemId" type="text" style="width:100px;" />
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="MaterialSearch();"></a>
            <% if (ynWebRight.rightVerify){ %>
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-print" onclick="MaterialPrint();">打印物料条码</a>
            <%} %>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <%-- 表单类型 --%>
    <script type="text/javascript">
        var currentMaterialTypeId = null;
        var currentMaterialTypeName = "";
        function MaterialTypeReload() {
            $('#treeMaterialType').tree('reload');
        }
    </script>
    <script type="text/javascript">
        var currentMaterialId = null;
        $(function () {
            $('#dataGridMaterial').datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/MaterialQueryList/',
                rownumbers: true,
                pagination: true,
                pageSize: 50,
                loadMsg: '更新数据......',
                frozenColumns: [[
//                    { field: 'id', title: 'ID', width: 100, align: 'center'},
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
                    //userRoleReload();
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentMaterialId) {
                        $(this).datagrid('selectRecord', currentMaterialId);
                    }
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    //MaterialTypeEditUser();
                    <%} %>
                }
            });
        })
        function MaterialReload() {
//            var title = '物料类型';
//            if (currentDepartmentName != "" && currentPositionName!="") {
//                title = '物料类型[<font color="red">' + currentDepartmentName + ">" + currentPositionName + '</font>]';
//            }
//            $("#dataGridMaterial").datagrid({
//                title: title,
//                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/MaterialList/?typeId=1'
//            })
            $('#dataGridMaterial').datagrid('reload');
        }
		function MaterialSearch(){
			var queryParams = $('#dataGridMaterial').datagrid('options').queryParams;
//			queryParams.queryWord = $('#materialSearch').val();
            queryParams.startInventoryItemId = $('#startInventoryItemId').val();
            queryParams.endInventoryItemId = $('#endInventoryItemId').val();
			$('#dataGridMaterial').datagrid('reload');
		}

        function MaterialPrint() {
            var selectRow = $('#dataGridMaterial').datagrid('getSelected');
            if (selectRow) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/Ascm/ReportViewer/MaterialPrint.aspx?id=' + selectRow.id;
                parent.openTab('物料条码打印', url);
            } else {
                $.messager.alert('提示', '请选择要打印条码的物料', 'info');
            }
        }
    </script>
</asp:Content>
