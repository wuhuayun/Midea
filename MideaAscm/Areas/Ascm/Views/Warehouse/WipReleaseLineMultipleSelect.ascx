<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择领料单明细--%>
<div id="wipReleaseLineSelect" class="easyui-window" title="选择领料单明细" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWipReleaseLine" title="领料单明细" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'releaseLineId',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWipReleaseLineSelect',
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
                        <th data-options="field:'releaseLineId',hidden:'true'"></th>
                        <th data-options="field:'materialDocNumber',width:100,align:'center'">物料编码</th> 
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'materialName',width:300,align:'left'">物料描述</th>
                        <th data-options="field:'materialUnit',width:60,align:'center'">单位</th>
                        <th data-options="field:'subInventory',width:115,align:'center'">子库</th>
                        <th data-options="field:'ascmWipRequirementOperations_wipSupplyTypeCn',width:80,align:'center'">供应类型</th>
                        <th data-options="field:'printQuantity',width:80,align:'center'">计划数</th>
                        <th data-options="field:'QUANTITY_AV',width:80,align:'center'">未发数</th>
                        <th data-options="field:'transaction_quantity',width:80,align:'center'">现有数</th>
                        <th data-options="field:'to_org_primary_quantity',width:80,align:'center'">接受中</th> 
                    </tr>
                </thead>
		    </table>
            <div id="tbWipReleaseLineSelect" style="padding:5px;height:auto;">
                <div style="margin-bottom:5px;">
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WipReleaseLineSelectOk()">确认</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wipReleaseLineSelect').window('close');">取消</a> 
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWipReleaseLine(releaseHeaderId, notInMaterialIds) {
        $('#wipReleaseLineSelect').window('open');
        WipReleaseLineSelectSearch(releaseHeaderId, notInMaterialIds);
    }
    function WipReleaseLineSelectSearch(releaseHeaderId, notInMaterialIds) {
        var options = $('#dgSelectWipReleaseLine').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/CuxWipReleaseLinesList';
        options.queryParams.id = releaseHeaderId;
        options.queryParams.notInMaterialIds = notInMaterialIds;
        $('#dgSelectWipReleaseLine').datagrid('reload');
    }
    function WipReleaseLineSelectOk() {
        var checkRows = $('#dgSelectWipReleaseLine').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择作业领料单！", "info");
            return;
        }
        try {
            WipReleaseLineSelected(checkRows);
        } catch (e) { }
        $('#wipReleaseLineSelect').window('close');
    }
</script>
