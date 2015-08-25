<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnOrganization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--单位-->
	<div region="center" title="" border="true" style="padding:0px;overflow:auto;">
		<div class="easyui-panel" title="单位管理 " fit="true" border="false">
			<div class="easyui-layout" fit="true">
			    <div region="center" border="false">
			         <table id="dataGridOrganization" title="" style="" border="false" fit="true"
					    idField="id" treeField="name" animate="true" sortName="sortNo" sortOrder="asc" toolbar="#tb2">
	                <%--<table title="" class="easyui-treegrid" style="" border="false" fit="true"
			                data-options="
				                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationList',
				                rownumbers: true,
				                pagination: true,
				                pageSize: 2,
				                pageList: [2,10,20],
				                idField: 'id',
				                treeField: 'name',
				                onBeforeLoad: function(row,param){
					                if (!row) {	// load top level rows
						                param.id = 0;	// set id=0, indicate to load new page rows
					                }
				                }
			                ">--%>
                        <thead>
				            <tr>
					            <th field="id" width="20" align="center" hidden="true">ID</th>
					            <th field="name" width="120" align="left">单位名称</th>
					            <th field="description" width="300" align="left">描述</th>
                                <th field="sortNo" width="50" align="center" sortable="true">序号</th>
				            </tr>
			            </thead>
					</table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="organizationReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="organizationAddRoot();">增加一级单位</a>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="organizationAddChild();">增加子单位</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="organizationEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="organizationDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editOrganization" class="easyui-window" title="单位" style="padding: 10px;width:500px;height:300px;"
			        iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
					        <div region="center" id="editOrganizationContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
                                <form id="editOrganizationForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>单位名称：</span>
						                    </td>
						                    <td style="width:80%">
                                                <input id="parentId" name="parentId" type="text" style="width:0px;display:none" />
							                    <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:200px;" /><span style="color:Red;">*</span>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>序号：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" validType="number" style="text-align:right;width:200px;"/>
						                    </td>
					                    </tr>
					                    <tr style="height:24px">
						                    <td style="width: 20%; text-align:right;" nowrap>
							                    <span>描述：</span>
						                    </td>
						                    <td style="width:80%">
							                    <input class="easyui-validatebox" id="description" name="description" type="text" style="width:96%;" />
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="organizationSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editOrganization').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
			    </div>
			</div>
		</div>
	</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--单位-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridOrganization').treegrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationList',
                //pagination: true,
                //pageSize: 30,
                loadMsg: '更新数据......',
                onClickRow: function (row) {
                    currentId=row.id;
                    parentId=row.parentId;
                },
                onDblClickRow: function (row) {
                    <% if (ynWebRight.rightEdit){ %>
                    organizationEdit();
                    <%} %>
                },
                onLoadSuccess: function () {
                    $('#dataGridOrganization').treegrid('unselectAll');
                    if (currentId) {
                        $(this).treegrid('select', currentId);
                    }
                },
				onBeforeLoad: function(row,param){
					if (!row) {	// load top level rows
                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationList?root=true';
                        $(this).treegrid("options").url = url;
						param.id = 0;	// set id=0, indicate to load new page rows
					}else{
                        var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationList?root=false';
                        $(this).treegrid("options").url = url;
                    }
				}
            });
        });
        function organizationReload() {
            //$('#dataGridOrganization').treegrid('reload');
            if(parentId)
            {
                $('#dataGridOrganization').treegrid('reload', parentId);
            }else{
                $('#dataGridOrganization').treegrid('reload');
            }
        }
        var currentId = null,parentId=null;
        function organizationAddRoot() {
            $('#editOrganization').window('open');
            $("#editOrganizationForm")[0].reset();

            $('#parentId').val(0);
            $('#name').focus();
            currentId = null;
            parentId=0;
        }
        function organizationAddChild() {
            var selectRow = $('#dataGridOrganization').treegrid('getSelected');
            if (!selectRow) {
                $.messager.alert("提示", "请选中父单位！", "info");
                return;
            }
            $('#editOrganization').window('open');
            $("#editOrganizationForm")[0].reset();
            $('#parentId').val(selectRow.id);
            $('#name').focus();
            parentId=selectRow.id;
            currentId = null;
        }
        function organizationEdit() {
            var selectRow = $('#dataGridOrganization').treegrid('getSelected');
            if (selectRow) {
                $('#editOrganization').window('open');
                var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationEdit/' + selectRow.id;
                $.ajax({
                    url: sUrl,
                    type: "post",
                    dataType: "json",
                    success: function (r) {
                        $("#editOrganizationForm")[0].reset();

                        $('#name').val(r.name);
                        $('#sortNo').val(r.sortNo);
                        $('#parentId').val(r.parentId);
                        $('#description').val(r.description);

                        $('#name').focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert('提示', '请选择单位', 'info');
            }
        }
        function organizationSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    $('#editOrganizationForm').form('submit', {
                        url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationSave/' + currentId,
                        onSubmit: function () {
                            return $('#editOrganizationForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editOrganization').window('close');
                                if(rVal.entity.parentId)
                                {
                                    $('#dataGridOrganization').treegrid('update', {
                                        id: rVal.entity.parentId,
                                        row: {
                                            state: "closed"
                                        }
                                    });
                                }
                                currentId=rVal.id;
                                organizationReload()
//                                $('#dataGridOrganization').treegrid({
//                                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationList',
//                                    onLoadSuccess: function () {
//                                        $('#dataGridOrganization').treegrid('unselectAll');
//                                        $('#dataGridOrganization').treegrid('select', rVal.id);
//                                    }
//                                });
                            } else {
                                $.messager.alert('确认', '修改信息失败:' + rVal.message, 'error');
                            }
                        }
                    });
			    }
			});

        }
        function organizationDelete() {
            var selectRow = $('#dataGridOrganization').treegrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认删除单位[<font color="red">' + selectRow.name + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnOrganization/OrganizationDelete/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    currentId=null;
                                    organizationReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要删除的单位', 'info');
            }
        }
    </script>
</asp:Content>
