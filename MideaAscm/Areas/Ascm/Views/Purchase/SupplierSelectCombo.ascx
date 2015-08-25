<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MideaAscm.Code.SelectCombo>" %>
<% MideaAscm.Code.SelectCombo selectCombo = Model ?? new MideaAscm.Code.SelectCombo(); %>
<% selectCombo.id = string.IsNullOrEmpty(selectCombo.id) ? "supplierSelect" : selectCombo.id; %>
<% selectCombo.width = string.IsNullOrEmpty(selectCombo.width) ? "200px" : selectCombo.width; %>
<% selectCombo.panelWidth = selectCombo.panelWidth == 0 ? 650 : selectCombo.panelWidth; %>
<select id="<%=selectCombo.id %>" name="<%=selectCombo.id %>" class="easyui-combogrid" style="width:<%=selectCombo.width %>;"
    data-options="panelWidth: <%=selectCombo.panelWidth %>,
                  delay: 300,
                  idField: 'id',
                  textField: 'name',
                  pagination: true,
                  pageSize: 30,
                  mode: 'remote',
                  fitColumns: true,
                  <% if (!string.IsNullOrEmpty(selectCombo.queryParams)){ %>
                  queryParams: { <%=selectCombo.queryParams %> },
                  <%} %>
                  url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Purchase/SupplierAscxList',
                  columns: [[
                      { field: 'id', hidden: true },
                      { field: 'docNumber', title: '编号', width: 100, align: 'center' },
                      { field: 'name', title: '供应商全称', width: 300, align: 'left' },
                      { field: 'description', title: '描述', width: 100, align: 'left' },
                      { field: 'status', title: '状态', width: 50, align: 'left' },
                      { field: 'enabled', title: '在用', width: 50, align: 'left' }      
                  ]]
                  <% if (!string.IsNullOrEmpty(selectCombo.onChange)){ %>
                  ,onChange: <%=selectCombo.onChange %>
                  <%} %>">
</select>
<script type="text/javascript">
    function getSupplierId() {
        var supplierId = null;
        var inputTxt = $('#<%=selectCombo.id %>').combogrid('getText');
        if (inputTxt != null && inputTxt != "" && inputTxt != undefined) {
            var g = $('#<%=selectCombo.id %>').combogrid('grid');
            var rows = g.datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].name == inputTxt || rows[i].docNumber == inputTxt) {
                    supplierId = rows[i].id;
                    break;
                }
            }
        }
        return supplierId;
    }
</script>

