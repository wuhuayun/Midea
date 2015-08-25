<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择备料清单--%>
<div id="wmsPreparationDetailMulSelect" class="easyui-window" title="选择备料清单" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectPreparationDetail" title="备料清单" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbPreparationDetail',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections'); 
                                  $(this).datagrid('clearChecked');                    
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'id',hidden:'true'"></th>
                        <th data-options="field:'materialDocNumber',width:90,align:'center'">物料编码</th>
                        <th data-options="field:'materialDescription',width:150,align:'left'">物料描述</th>
                     </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'materialUnit',width:70,align:'center'">物料单位</th>
                        <th data-options="field:'warehouseId',width:70,align:'center'">供应子库</th>
                        <th data-options="field:'locationDocNumber',width:100,align:'center'">货位</th>
                        <th data-options="field:'dateRequired',width:110,align:'center'">需求日期</th>
                        <th data-options="field:'quantity',width:80,align:'center'">需求数量</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbPreparationDetail">
                <%--<input id="warehouseSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="WarehouseSelectSearch();"></a>--%>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="PreparationDetailSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wmsPreparationDetailMulSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectPreparationDetail(mainId, preDetailList) {
        $('#wmsPreparationDetailMulSelect').window('open');
        var options = $('#dgSelectPreparationDetail').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WmsPreparationDetailAdd';
        options.queryParams.mainId = mainId;
        options.queryParams.preDetailList = preDetailList;
        $('#dgSelectPreparationDetail').datagrid('reload');
    }
    function PreparationDetailSelectOk() {
        var selectRows = $('#dgSelectPreparationDetail').datagrid('getSelections');
        if (selectRows.length == 0) {
            $.messager.alert("提示", "请选择备料清单！", "info");
            return;
        }
        try {
            PreparationDetailSelected(selectRows);
        } catch (e) { }
        $('#wmsPreparationDetailMulSelect').window('close');
    }
</script>
