<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择物料--%>
<div id="materialItemSelect" class="easyui-window" title="选择物料" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectMaterialItem" title="物料信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: '',
                              striped: true,
                              toolbar: '#tbMaterialItemSelect',
                              pagination: true,
                              pageSize: 30,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  <%--MaterialItemSelectOk();--%>
                                  try {
                                      var checkRows = [];
                                      checkRows.push(rowRec);
                                      MaterialItemSelected(checkRows);
                                  } catch (e) { }
                                  $('#materialItemSelect').window('close');
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');
                                  $(this).datagrid('clearChecked');                      
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
                        <th data-options="checkbox:true"></th>
                        <th data-options="field:'docNumber',width:100,align:'center'">编码</th>
                    </tr>
                </thead>
                <thead>
                    <tr>
                        <th data-options="field:'description',width:400,align:'left'">描述</th>
                    </tr>
                </thead>
		    </table>
            <div id="tbMaterialItemSelect">
                <input id="materialItemSelectSearch" type="text" style="width:120px;" />
                <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="MaterialItemSelectSearch();"></a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="MaterialItemSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#materialItemSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $('#materialItemSelectSearch').keypress(function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode == 13) {
                MaterialItemSelectSearch();
            }
        })
    })
    function SelectMaterialItem(categoryCode) {
        $('#materialItemSelect').window('open');
        $('#materialItemSelectSearch').val("");
        var options = $('#dgSelectMaterialItem').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/MaterialList';
        options.queryParams.queryWord = $('#materialItemSelectSearch').val();
        options.queryParams.categoryCode = categoryCode;
        $('#dgSelectMaterialItem').datagrid('reload');
    }
    function MaterialItemSelectSearch() {
        var queryParams = $('#dgSelectMaterialItem').datagrid('options').queryParams;
        queryParams.queryWord = $('#materialItemSelectSearch').val();
        $('#dgSelectMaterialItem').datagrid('reload');
    }
    function MaterialItemSelectOk() {
        var checkRows = $('#dgSelectMaterialItem').datagrid('getChecked');
        if (checkRows.length == 0) {
            $.messager.alert("提示", "请选择物料！", "info");
            return;
        }
        try {
            MaterialItemSelected(checkRows);
        } catch (e) { }
        $('#materialItemSelect').window('close');
    }
</script>
