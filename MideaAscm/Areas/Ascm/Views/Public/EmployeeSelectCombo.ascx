<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MideaAscm.Code.SelectCombo>" %>
<% MideaAscm.Code.SelectCombo selectCombo = Model ?? new MideaAscm.Code.SelectCombo(); %>
<% selectCombo.id = string.IsNullOrEmpty(selectCombo.id) ? "employeeSelect" : selectCombo.id; %>
<% selectCombo.width = string.IsNullOrEmpty(selectCombo.width) ? "120px" : selectCombo.width; %>
<% selectCombo.panelWidth = selectCombo.panelWidth == 0 ? 450 : selectCombo.panelWidth; %>
<% selectCombo.onLoadSuccess = string.IsNullOrEmpty(selectCombo.onLoadSuccess) ? "DefaultOnLoadSuccess" : selectCombo.onLoadSuccess; %>
<select id="<%=selectCombo.id %>" name="<%=selectCombo.id %>" class="easyui-combogrid" style="width:<%=selectCombo.width %>;"
    data-options="panelWidth: <%=selectCombo.panelWidth %>,
                  delay: 300,
                  idField: 'id',
                  textField: 'docNumber',
                  pagination: true,
                  pageSize: 30,
                  mode: 'remote',
                  fitColumns: true,
                  <% if (!string.IsNullOrEmpty(selectCombo.queryParams)){ %>
                  queryParams: { <%=selectCombo.queryParams %> },
                  <%} %>
                  url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeAscxList/',
                  columns: [[
                    { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                    { field: 'docNumber', title: '员工编号', width: 80, align: 'center' },
				    { field: 'name', title: '姓名', width: 80, align: 'center' },
				    { field: 'sex', title: '性别', width: 50, align: 'center' },
				    { field: 'email', title: 'EMail', width: 120, align: 'center' },
				    { field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
				    { field: 'mobileTel', title: '移动电话', width: 100, align: 'center' }
                  ]]
                  <% if (!string.IsNullOrEmpty(selectCombo.onChange)){ %>
                  ,onChange: <%=selectCombo.onChange %>
                  <%} %>
                  <% if (!string.IsNullOrEmpty(selectCombo.onLoadSuccess)){ %>
                  ,onLoadSuccess: <%=selectCombo.onLoadSuccess %>
                  <%} %>">
</select>
<script type="text/javascript">
    function DefaultOnLoadSuccess(data) {
        var queryParams = $('#<%=selectCombo.id %>').combogrid('options').queryParams;
        if (queryParams.id) {
            $('#<%=selectCombo.id %>').combogrid('setValue', queryParams.id);
            queryParams.id = null;
        }
    }
</script>

<%--<select id="employeeSelect" name="employeeSelect" style="width:120px;"></select>
<script type="text/javascript">
    $(function () {
    })
    function InitEmployeeSelect(employeeId) {
        $('#employeeSelect').combogrid({
            panelWidth: 450,
            delay: 300,
            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Ascm/Public/EmployeeAscxList/',
            queryParams: { id: employeeId },
            idField: 'id',
            textField: 'name',
            pagination: true,
            pageSize: 30,
            mode: 'remote',
            fitColumns: true,
            columns: [[
                { field: 'id', title: 'ID', width: 20, align: 'center', hidden: 'true' },
                { field: 'docNumber', title: '员工编号', width: 80, align: 'center' },
				{ field: 'name', title: '姓名', width: 80, align: 'center' },
				{ field: 'sex', title: '性别', width: 50, align: 'center' },
				{ field: 'email', title: 'EMail', width: 120, align: 'center' },
				{ field: 'officeTel', title: '办公电话', width: 100, align: 'center' },
				{ field: 'mobileTel', title: '移动电话', width: 100, align: 'center' }
            ]],
            onLoadSuccess: function (data) {
                if (employeeId != null && employeeId != "") {
                    $('#employeeSelect').combogrid('setValue', employeeId);
                    var queryParams = $(this).combogrid('options').queryParams;
                    queryParams.id = null;
                    employeeId = null;
                }
            }
        })
    }
</script>--%>

