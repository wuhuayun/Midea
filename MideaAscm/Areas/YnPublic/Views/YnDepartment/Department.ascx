<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<select id="department" name="department" style="width:256px;"></select>
<script type="text/javascript">
    $(function () {
        $('#customer').combogrid({
            panelWidth: 545,
            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Invoicing/YnDepartment/DepartmentAscxList',       
            idField: 'id',
            textField: 'name',
            //pagination: true,
            //pageSize: 10,
            mode: 'remote',
            fitColumns: true,
            columns: [[
			    { field: 'id', title: '标识', align: 'center', hidden: true },
                { field: 'sortNo', title: '序号', align: 'center', width: 60 },
                { field: 'name', title: '部门名称', align: 'center', width: 200 },
			]]
        })
    })
</script>

