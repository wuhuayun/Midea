<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MideaAscm.Code.SelectCombo>" %>
<% MideaAscm.Code.SelectCombo selectCombo = Model ?? new MideaAscm.Code.SelectCombo(); %>
<% selectCombo.id = string.IsNullOrEmpty(selectCombo.id) ? "workerSelect" : selectCombo.id; %>
<% selectCombo.width = string.IsNullOrEmpty(selectCombo.width) ? "83px" : selectCombo.width; %>
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
                  url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/GetMaterialManage/WorkerAscxList',
                  columns: [[
                      { field: 'num', title: '工号', width: 100, align: 'center' },
                      { field: 'name', title: '姓名', width: 100, align: 'center' },
                      { field: '_logisticsClass', title: '归属主体', width: 100, align: 'center' },
                      { field: '_status', title: '状态', width: 100, align: 'center' }  
                  ]]
                  <% if (!string.IsNullOrEmpty(selectCombo.onChange)){ %>
                  ,onChange: <%=selectCombo.onChange %>
                  <%} %>">
</select>