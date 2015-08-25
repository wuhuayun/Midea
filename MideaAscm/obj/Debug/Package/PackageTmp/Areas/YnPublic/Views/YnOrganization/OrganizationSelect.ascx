<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--选择单位-->
<div id="organizationSelect" class="easyui-window" title="选择单位" style="padding: 5px;width:420px;height:480px;"
    iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true" style="background:#fafafa;border:1px solid #99BBE8;">
	    <div region="north" border="false" style="height:30px;overflow:hidden;">
            <div class="div_toolbar" style="float:left; height:26px; width:100%;border-bottom:1px solid #99BBE8; padding:1px;">
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="OrganizationSelectOk()">确认</a>
			    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#organizationSelect').window('close');">取消</a> 
	        </div>
	    </div>
        <div region="center" border="false" style="background:#fff;">
            <div class="easyui-panel" title="" data-options="fit:true,border:false">
	            <ul id="treeOrganization" class="easyui-tree" data-options="
			            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationTree?hasNo=1',
			            animate: true,
				        onBeforeLoad: function(node,param){
					        if (!node) {	// load top level rows
						        param.id = 0;	// set id=0, indicate to load new page rows
                            }
				        },
                        onDblClick: function(node){
                            OrganizationSelectOk();
                        }
		            "></ul>
            </div>
        </div>
    </div>
</div>
<!--选择单位-->
<script type="text/javascript">
    $(function () {
    })
    function SelectOrganization() {
        $('#organizationSelect').window('open');
    }
    function OrganizationSelectOk() {
        var selectNode = $('#treeOrganization').tree('getSelected');
        if (!selectNode) {
            alert("请选择单位！");
            return;
        }
        try {
            OrganizationSelected(selectNode);
        } catch (e) { ; }
        $('#organizationSelect').window('close');
    }
</script>
