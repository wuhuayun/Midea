<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MideaAscm.Code.SelectCombo>" %>
<% MideaAscm.Code.SelectCombo selectCombo = Model ?? new MideaAscm.Code.SelectCombo(); %>
<% selectCombo.id = string.IsNullOrEmpty(selectCombo.id) ? "rfidSelect" : selectCombo.id; %>
<% selectCombo.width = string.IsNullOrEmpty(selectCombo.width) ? "115px" : selectCombo.width; %>
<% selectCombo.panelWidth = selectCombo.panelWidth == 0 ? 450 : selectCombo.panelWidth; %>
<select id="<%=selectCombo.id %>" name="<%=selectCombo.id %>" class="easyui-combogrid" style="width:<%=selectCombo.width %>;"
    data-options="panelWidth: <%=selectCombo.panelWidth %>,
                  delay: 300,
                  idField: 'id',
                  textField: 'id',
                  pagination: true,
                  pageSize: 30,
                  mode: 'remote',
                  fitColumns: true,
                  <% if (!string.IsNullOrEmpty(selectCombo.queryParams)){ %>
                  queryParams: { <%=selectCombo.queryParams %> },
                  <%} %>
                  url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/RfidAscxList',
                  columns: [[
                      { field: 'id', title: 'Id', width: 100, align: 'center' },
                      { field: 'epcId', title: 'EPC', width: 100, align: 'center' },
                      { field: '_bindTypeCn', title: '类型', width: 100, align: 'center' }  
                  ]]
                  <% if (!string.IsNullOrEmpty(selectCombo.onChange)){ %>
                  ,onChange: <%=selectCombo.onChange %>
                  <%} %>">
</select>