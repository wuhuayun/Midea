<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MideaAscm.Code.SelectCombo>" %>
<% MideaAscm.Code.SelectCombo selectCombo = Model ?? new MideaAscm.Code.SelectCombo(); %>
<% selectCombo.id = string.IsNullOrEmpty(selectCombo.id) ? "warehouseSelect" : selectCombo.id; %>
<% selectCombo.width = string.IsNullOrEmpty(selectCombo.width) ? "120px" : selectCombo.width; %>
<% selectCombo.panelWidth = selectCombo.panelWidth == 0 ? 450 : selectCombo.panelWidth; %>
<select id="<%=selectCombo.id %>" name="<%=selectCombo.id %>" class="easyui-combogrid" style="width:<%=selectCombo.width %>;"
    data-options="panelWidth: <%=selectCombo.panelWidth %>,
                  delay: 500,
                  idField: 'id',
                  textField: 'id',
                  pagination: true,
                  pageSize: 30,
                  mode: 'remote',
                  fitColumns: true,
                  <% if (!string.IsNullOrEmpty(selectCombo.queryParams)){ %>
                  queryParams: { <%=selectCombo.queryParams %> },
                  <%} %>
                  url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WarehouseAscxList',
                  columns: [[
                      { field: 'id', title: '子库名称', width: 100, align: 'center' },
                      { field: 'description', title: '描述', width: 200, align: 'left' },
                      { field: '_modifyTime', title: '最后更新时间', width: 110, align: 'center' },
                      { field: 'status', title: '状态', width: 50, align: 'left' },
                      { field: 'enabled', title: '在用', width: 50, align: 'left' }
                  ]]
                  <% if (!string.IsNullOrEmpty(selectCombo.onChange)){ %>
                  ,onChange: <%=selectCombo.onChange %>
                  <%} %>">
</select>

