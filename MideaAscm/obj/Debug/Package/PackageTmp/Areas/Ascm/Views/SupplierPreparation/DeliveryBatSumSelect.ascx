<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--选择送货合单--%>
<div id="batSumMainSelect" class="easyui-window" title="选择送货合单" style="padding: 5px;width:640px;height:480px;"
    data-options="iconCls:'icon-edit',closed:true,maximizable:false,minimizable:false,resizable:false,collapsible:false,modal:true,shadow:true">
    <div class="easyui-layout" fit="true">
        <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
            <table id="dgSelectBatSumMain" title="合单信息" class="easyui-datagrid" style="" border="false"
                data-options="fit: true,
                              noheader: true,
                              rownumbers: true,
                              singleSelect : true,
                              idField: 'id',
                              sortName: '',
                              sortOrder: 'asc',
                              striped: true,
                              toolbar: '#tbBatSumMainSelect',
                              pagination: true,
                              pageSize: 20,
                              loadMsg: '更新数据......',
                              onSelect: function(rowIndex, rowRec){
                                
                              },
                              onDblClickRow: function(rowIndex, rowRec){
                                  BatSumMainSelectOk();
                              },
                              onLoadSuccess: function(){
                                  $(this).datagrid('clearSelections');                     
                              }">
                <thead data-options="frozen:true">
                    <tr>
                        <th data-options="field:'id',hidden:true"></th>
					    <th data-options="field:'docNumber',width:100,align:'center'">合单号</th>
                        <th data-options="field:'driverSn',width:80,align:'center'">司机编号</th>
                        <th data-options="field:'supplierDocNumber',width:80,align:'center'">供方编号</th>
                        <th data-options="field:'supplierName',width:80,align:'center'">供方名称</th>
                        <th data-options="field:'driverPlateNumber',width:100,align:'center'">车牌号</th>
                    </tr>
                </thead>
                <thead>
				    <tr>                              
                        <th data-options="field:'statusCn',width:60,align:'center'">状态</th>
                        <th data-options="field:'appointmentStartTimeShow',width:120,align:'center'">预约开始时间</th>
                        <th data-options="field:'appointmentEndTimeShow',width:120,align:'center'">预约最后时间</th>
                        <th data-options="field:'totalNumber',width:60,align:'center'">总数量</th>
                        <th data-options="field:'confirmor',width:80,align:'center'">确认人</th>
                        <th data-options="field:'_confirmTime',width:100,align:'center'">确认时间</th>
                        <th data-options="field:'_toPlantTime',width:100,align:'center'">到厂时间</th>
                        <th data-options="field:'receiver',width:80,align:'center'">接受人</th>
                        <th data-options="field:'_acceptTime',width:100,align:'center'">接受时间</th>
				    </tr>
			    </thead>
		    </table>
            <div id="tbBatSumMainSelect">
                <span>司机编号：</span>
                <%Html.RenderPartial("~/Areas/Ascm/Views/SupplierPreparation/DriverSelectCombo.ascx", new MideaAscm.Code.SelectCombo()); %>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="BatSumMainSelectOk()">确认</a>
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#batSumMainSelect').window('close');">取消</a> 
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function SelectBatSumMain() {
        $('#batSumMainSelect').window('open');
        $('#driverSelect').combogrid('clear');
        var options = $('#dgSelectBatSumMain').datagrid('options');
        options.url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DeliveryBatSumQueryList';
        options.queryParams.status = '<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.unConfirm %>,<%=MideaAscm.Dal.SupplierPreparation.Entities.AscmDeliBatSumMain.StatusDefine.Confirm %>';
        options.queryParams.driverId = $('#driverSelect').combogrid('getValue');
        $('#dgSelectBatSumMain').datagrid('reload');
    }
    function BatSumMainSelectOk() {
        var selectRow = $('#dgSelectBatSumMain').datagrid('getSelected');
        if (selectRow) {
            try {
                BatSumMainSelected(selectRow);
            } catch (e) { }
        } else {
            $.messager.alert("提示", "没有选择送货合单！", "info");
            return;
        }
        $('#batSumMainSelect').window('close');
    }
</script>
