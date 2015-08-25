<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<object  id="LODOP" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0"> 
    <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"></embed>
</object>
<!--进度条-->           
<div id="wLoadPrintProgressbar" class="easyui-window" title="加载打印数据..." style="padding: 10px;width:440px;height:80px;"
    data-options="collapsible:false,minimizable:false,maximizable:false,resizable:false,modal:true,shadow:true,closed:true">
    <div id="pLoadPrint" class="easyui-progressbar" style="width:400px;"></div>
</div>
<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/lodop/LodopFuncs.js"></script>
<%--<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/CustomMaterialPrint.js"></script>--%>
<script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/_data/CustomMaterialPrint2.js"></script>
<script type="text/javascript">
    <%--打印预览--%>
    function printPreview2(rows) {
        //var imgUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/lodop/MdLogo.jpg';
        //materialPrintPreview(LODOP, rows, imgUrl);      
    }
    function printPreview(rows) {
        $.ajax({
            type: "POST",
            url: '<%=Url.Action("WmsMaterialLabelPrint", "Warehouse", new { Area = "Ascm" })%>',
            cache: false,
            dataType: 'json',
            data: { "dataJson": $.toJSON(rows) },
            beforeSend: function () {
                //$('#wLoadPrintProgressbar').window('open');
                //$('#pLoadPrint').progressbar('setValue', 0);
                //setInterval(updateProgress, 100);
                var options = $('#dgDeliBatMaterial').datagrid('options');
                options.loadMsg = '加载打印数据...';
                $('#dgDeliBatMaterial').datagrid('loading');
                return true;
            },
            success: function (result) {
                //$('#pLoadPrint').progressbar('setValue', 100);
                //$('#wLoadPrintProgressbar').window('close');
                $('#dgDeliBatMaterial').datagrid('loaded');
                if (result.success) {
                    materialPrintPreview(LODOP, result.data);
                } else if (result.errorMsg) {
                    $.messager.alert('错误', result.errorMsg, 'error');
                }
            }
        });
    }
    function updateProgress() {
        var value = $('#pLoadPrint').progressbar('getValue');
        if (value < 99) {
            $('#pLoadPrint').progressbar('setValue', value + 1);
        }
    }
</script>

