<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--选择部门-->
<div id="departmentSelect" class="easyui-window" title="选择部门" style="padding: 5px;width:540px;height:480px;"
    iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true" style="background:#fafafa;">
	    <div region="north" border="true" style="height:30px;overflow:hidden;">
            <div class="div_toolbar" style="float:left; height:28px; width:100%;border:0px solid #99BBE8;padding:1px;">
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="DepartmentSelectOk()">确认</a>
			    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#departmentSelect').window('close');">取消</a> 
	        </div>
	    </div>
        <div region="center" border="false" style="background:#fff;padding:2px 0px 0px 0px;">
            <div class="easyui-layout" style="" data-options="fit:true,border:false">
                <div region="center" border="false" title="">
                    <div class="easyui-panel" title="部门" data-options="fit:true,border:true">
	                    <ul id="treeDepartment" class="easyui-tree" data-options="
			                    animate: true,
				                onBeforeLoad: function(node,param){
					                if (!node) {
                                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentTree';
                                        $(this).tree('options').url = url;
                                        param.id = 0;
                                    }else{
                                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentTree';
                                        $(this).tree('options').url = url;
                                    }
				                },
                                onDblClick: function(node){
                                    DepartmentSelectOk();
                                }
		                    "></ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<!--选择部门-->
<script type="text/javascript">
    $(function () {
    })
    function SelectDepartment() {
        $('#departmentSelect').window('open');
    }
    function DepartmentSelectOk() {
        var selectNode = $('#treeDepartment').tree('getSelected');
        if (!selectNode) {
            alert("请选择部门！");
            return;
        }
        try {
            DepartmentSelected(selectNode);
        } catch (e) { ; }
        $('#departmentSelect').window('close');
    }
</script>
