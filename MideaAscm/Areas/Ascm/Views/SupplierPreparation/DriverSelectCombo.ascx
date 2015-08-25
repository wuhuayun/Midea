<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MideaAscm.Code.SelectCombo>" %>
<% MideaAscm.Code.SelectCombo selectCombo = Model ?? new MideaAscm.Code.SelectCombo(); %>
<% selectCombo.id = string.IsNullOrEmpty(selectCombo.id) ? "driverSelect" : selectCombo.id; %>
<% selectCombo.width = string.IsNullOrEmpty(selectCombo.width) ? "200px" : selectCombo.width; %>
<% selectCombo.panelWidth = selectCombo.panelWidth == 0 ? 450 : selectCombo.panelWidth; %>
<select id="<%=selectCombo.id %>" name="<%=selectCombo.id %>" class="easyui-combogrid" style="width:<%=selectCombo.width %>;"
    data-options="panelWidth: <%=selectCombo.panelWidth %>,
                  delay: 300,
                  idField: 'id',
                  textField: 'sn',
                  pagination: true,
                  pageSize: 30,
                  mode: 'remote',
                  fitColumns: true,
                  <% if (!string.IsNullOrEmpty(selectCombo.queryParams)){ %>
                  queryParams: { <%=selectCombo.queryParams %> },
                  <%} %>
                  url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/SupplierPreparation/DriverAscxList',
                  frozenColumns: [[
                      { field:'id', hidden:true },
                      { field:'sn', title:'司机编号', width:100, align:'center' }  
                  ]],
                  columns: [[
                      { field:'name', title:'姓名', width:80, align:'center' },
                      { field:'mobileTel', title:'手机号', width:100, align:'center' },
                      { field:'typeCn', title:'类型', width:80, align:'center' },
                      { field:'plateNumber', title:'车牌号', width:80, align:'center' },
                      { field:'load', title:'载重', width:60, align:'center' }     
                  ]]
                  <% if (!string.IsNullOrEmpty(selectCombo.onChange)){ %>
                  ,onChange: <%=selectCombo.onChange %>
                  <%} %>">
</select>
<script type="text/javascript">
    function getDriverId() {
        var driverId = null;
        var inputTxt = $('#<%=selectCombo.id %>').combogrid('getText');
        if (inputTxt != null && inputTxt != "" && inputTxt != undefined) {
            var g = $('#<%=selectCombo.id %>').combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].sn == inputTxt || rows[i].name == inputTxt || rows[i].plateNumber == inputTxt) {
                    driverId = rows[i].id;
                    break;
                }
            }
        }
        return driverId;
    }
</script>