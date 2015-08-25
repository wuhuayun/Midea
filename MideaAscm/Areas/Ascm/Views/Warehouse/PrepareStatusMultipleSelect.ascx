<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="MideaAscm.Dal.Warehouse.Entities" %>
<select id="prepareStatus" name="prepareStatus" class="easyui-combobox" style="width: 200px" data-options="editable: false"></select>
<div id="divPrepareStatus">
    <div style="color: #99BBE8; background: #fafafa; padding: 5px;">选择备料单状态</div>
    <% List<string> listPrepareStatus = AscmWmsPreparationMain.StatusDefine.GetList(); %>
    <% int i = 0; %>
    <% foreach (string status in listPrepareStatus){ i++; %>
       <input type="checkbox" name="status" value="<%=status %>" /><span><%=AscmWmsPreparationMain.StatusDefine.DisplayText(status)%></span>
       <% if (i < listPrepareStatus.Count){ %><br /><%} %>
    <%} %>
</div>
<script type="text/javascript">
    $(function () {
        $('#divPrepareStatus').appendTo($('#prepareStatus').combobox('panel'));
        $('#divPrepareStatus input').click(function () {
            var cbo_v = "", cbo_t = "";
            $('#divPrepareStatus input').each(function () {
                if ($(this).is(':checked')) {
                    if (cbo_v != "")
                        cbo_v += ",";
                    cbo_v += $(this).val();
                    if (cbo_t != "")
                        cbo_t += ",";
                    cbo_t += $(this).next('span').text();
                }
            })
            $('#prepareStatus').combobox('setValue', cbo_v).combobox('setText', cbo_t);
        });
    });
</script>
