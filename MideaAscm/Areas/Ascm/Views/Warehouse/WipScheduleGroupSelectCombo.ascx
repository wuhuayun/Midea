<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MideaAscm.Code.SelectCombo>" %>
<% MideaAscm.Code.SelectCombo selectCombo = Model ?? new MideaAscm.Code.SelectCombo(); %>
<% selectCombo.id = string.IsNullOrEmpty(selectCombo.id) ? "wipScheduleGroupSelect" : selectCombo.id; %>
<% selectCombo.width = string.IsNullOrEmpty(selectCombo.width) ? "120px" : selectCombo.width; %>
<% selectCombo.panelWidth = selectCombo.panelWidth == 0 ? 450 : selectCombo.panelWidth; %>
<select id="<%=selectCombo.id %>" name="<%=selectCombo.id %>" class="easyui-combogrid" style="width:<%=selectCombo.width %>;"
    data-options="panelWidth: <%=selectCombo.panelWidth %>,
                  delay: 500,
                  idField: 'scheduleGroupId',
                  textField: 'scheduleGroupName',
                  pagination: true,
                  pageSize: 30,
                  mode: 'remote',
                  fitColumns: true,
				  <% if (selectCombo.multiple){ %>
                  multiple: true,
                  <%} %>
                  <% if (!string.IsNullOrEmpty(selectCombo.queryParams)){ %>
                  queryParams: { <%=selectCombo.queryParams %> },
                  <%} %>
                  url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Warehouse/WipScheduleGroupAscxList',
                  columns: [[
					  <% if (selectCombo.multiple){ %>
						{ field:'ck',checkbox:true},
					  <%} %>
                      { field: 'scheduleGroupId', hidden: 'center' },
                      { field: 'scheduleGroupName', title: '车间', width: 200, align: 'left' }
                  ]]
                  <% if (!string.IsNullOrEmpty(selectCombo.onChange)){ %>
                  ,onChange: <%=selectCombo.onChange %>
                  <%} %>">
</select>

