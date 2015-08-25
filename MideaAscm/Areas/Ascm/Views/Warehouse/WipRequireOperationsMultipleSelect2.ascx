﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择作业需求--%>
<div id="wipRequireOperationSelect" class="easyui-window" title="选择作业需求" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectWipRequireOperation" title="作业需求报表" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbWipRequireOperationSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'id',hidden:'true'"></th>
                        <th data-options="field:'ascmMaterialItem_DocNumber',width:110,align:'center'">物料编码</th>
                        <th data-options="field:'ascmMaterialItem_Description',width:300,align:'left'">物料描述</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'ascmMaterialItem_unit',width:80,align:'center'">物料单位</th>
                        <th data-options="field:'supplySubinventory',width:120,align:'center'">供应子库</th>
                        <th data-options="field:'requiredQuantity',width:100,align:'center'">需求数量</th>
                        <th data-options="field:'quantityIssued',width:100,align:'center'">发料数量</th>
                        <th data-options="field:'dateRequired',width:110,align:'left'">需求日期</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbWipRequireOperationSelect" style="padding:5px;height:auto;">
                <div style="margin-bottom:5px;">
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="WipRequireOperationSelectOk()">确认</a>
                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#wipRequireOperationSelect').window('close');">取消</a> 
                </div>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectWipRequireOperation(wipEntityId, notInMaterialIds) {
        $('#wipRequireOperationSelect').window('open');
        WipRequireOperationSelectSearch(wipEntityId, notInMaterialIds);
    }
    function WipRequireOperationSelectSearch(wipEntityId, notInMaterialIds) {
        var options = $('#dgSelectWipRequireOperation').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipRequireOperationAscx';
        options.queryParams.wipEntityId = wipEntityId;
        options.queryParams.notInMaterialIds = notInMaterialIds;
        $('#dgSelectWipRequireOperation').datagrid('reload');
    }
    function WipRequireOperationSelectOk() {
        var checkRows = $('#dgSelectWipRequireOperation').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择作业需求！", "info");
            return;
        }
        try {
            WipRequireOperationSelected(checkRows);
        } catch (e) { }
        $('#wipRequireOperationSelect').window('close');
    }
</script>
