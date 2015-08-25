<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!--选择部门岗位-->
<div id="departmentPositionSelect" class="easyui-window" title="选择部门岗位" style="padding: 5px;width:540px;height:480px;"
    iconCls="icon-search" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
    <div class="easyui-layout" fit="true" style="background:#fafafa;">
	    <div region="north" border="true" style="height:30px;overflow:hidden;">
            <div class="div_toolbar" style="float:left; height:28px; width:100%;border:0px solid #99BBE8;padding:1px;">
                <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="DepartmentPositionSelectOk()">确认</a>
			    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#departmentPositionSelect').window('close');">取消</a> 
	        </div>
	    </div>
        <div region="center" border="false" style="background:#fff;padding:2px 0px 0px 0px;">
            <div class="easyui-panel" title="" data-options="fit:true,border:true">
	            <ul id="treeDepPosition" class="easyui-tree" data-options="
			            animate: true,
				        onBeforeLoad: function(node,param){
					        if (!node) {
                                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentTree?withPosition=1';
                                $(this).tree('options').url = url;
                                param.id = 0;
                            }else{
                                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnDepartment/DepartmentTree?withPosition=1';
                                $(this).tree('options').url = url;
                            }
				        },
                        onDblClick: function(node){
                            DepartmentPositionSelectOk();
                        }
		            "></ul>
            </div>
        </div>
    </div>
</div>
<!--选择部门-->
<script type="text/javascript">
    $(function () {
    })
    function SelectDepartmentPosition() {
        $('#departmentPositionSelect').window('open');
    }
    function DepartmentPositionSelectOk() {
        var currentDepartmentId = null, currentPositionId = null;
        var selectNode = $('#treeDepPosition').tree('getSelected');
        if (!selectNode || selectNode.id <= 1000000) {
            alert("请选择部门岗位！");
            return;
        }
        currentPositionId = selectNode.id - 1000000;

        var parentNode = $(this).tree('getParent', selectNode.target);
        if (parentNode) {
            currentDepartmentId = parentNode.id;
        }
        //selectNode.text = selectNode.attributes.name;
        try {
            DepartmentPositionSelected(currentDepartmentId, currentPositionId);
        } catch (e) { ; }
        $('#departmentPositionSelect').window('close');
    }
</script>
