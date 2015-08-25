<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnWebAccMenu>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	YnWebMenu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--导航菜单组-->
    <div region="west" split="false" border="false" title="" style="width:450px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
			    <div region="center" title="" border="false">
					<table id="dataGridAccMenu" title="菜单组" style="" border="false" fit="true" singleSelect="true"
			            idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb">
			            <thead>
				            <tr>
					            <th field="id" width="20" align="center" hidden="true">ID</th>
                                <th field="sortNo" width="60" align="center" sortable="true">序号</th>
					            <th field="name" width="230" align="left">名称</th>
				            </tr>
			            </thead>
		            </table>
                    <div id="tb">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="accMenuReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="accMenuAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="accMenuEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="accMenuDelete();">删除</a>
                        <%} %>
                        <input id="accMenuSearch" type="text" style="width:100px;" />
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="accMenuSearch();"></a> 
                    </div>
			        <form id="editAccMenuForm" method="post" style="display:none;">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width:10%; text-align:right;" nowrap>
							        <span>名称：</span>
						        </td>
						        <td style="width:80%">
							        <input class="easyui-validatebox" required="true" name="name" type="text" style="width:96%;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>序号：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" name="sortNo" type="text" validType="number" style="text-align:right"/>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width:10%; text-align:right;" nowrap>
							        <span>打开方式：</span>
						        </td>
						        <td style="width:80%">
							        <input name="target" type="text" style="width:96%;" />
						        </td>
					        </tr>
				        </table>
			        </form>
			        <div id="editAccMenu" class="easyui-window" title="菜单组" style="padding: 10px;width:500px;height:300px;"
				    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" id="editAccMenuContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="accMenuSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editAccMenu').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
			    </div>
			</div>
		</div>
	</div>
    <!--导航菜单树-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
                    <table id="dataGridAccMenuTree" title="菜单树" style="" border="false" fit="true" singleSelect="true"
			            idField="id"  treeField="name" animate="true" sortName="sortNo" sortOrder="asc" toolbar="#tb2"> 
			            <thead>
				            <tr>
					            <th field="id" width="20" align="center" hidden="true">ID</th>
                                <th field="name" width="220" align="left">名称</th>
                                <th field="sortNo" width="50" align="center" sortable="true">序号</th>
                                <th field="iconCls" width="60" align="center">图标</th>
                                <th field="moduleName" width="180" align="left">模块</th>
				            </tr>
			            </thead>
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="accMenuTreeReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="accMenuTreeAdd('root');">增加主菜单</a>
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="accMenuTreeAdd('sub');">增加子菜单</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="accMenuTreeEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="accMenuTreeDelete();">删除</a>
                        <%} %>
                    </div>
			        <form id="editAccMenuTreeForm" method="post" style="display:none;">
				        <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					        <tr style="height:24px">
						        <td style="width:10%; text-align:right;" nowrap>
							        <span>名称：</span>
						        </td>
						        <td style="width:80%">
                                    <input name="parentId" type="text" style="width:10px;display:none;" />
							        <input class="easyui-validatebox" required="true" name="name" type="text" style="width:96%;" /><span style="color:Red;">*</span>
						        </td>
					        </tr>
					        <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>序号：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" name="sortNo" type="text" validType="number" style="text-align:right"/>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="text-align:right;" nowrap>
							        <span>模块：</span>
						        </td>
						        <td>
							        <input class="easyui-validatebox" id="moduleName" name="moduleName" type="text" style="width:200px;" readonly="readonly" />
							        <input class="easyui-validatebox" id="moduleId" name="moduleId" type="text" style="width:10px;display:none"/>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-extract" title="选择模块" onclick="selectModule()">
                                        <%--<img src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/ICPROP.GIF" alt=""/>--%>
                                    </a>
							        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" iconCls="icon-remove" title="移除模块" onclick="removeModule()"></a>
						        </td>
					        </tr>
                            <tr style="height:24px">
						        <td style="width:10%; text-align:right;" nowrap>
							        <span>图标：</span>
						        </td>
						        <td style="width:80%">
							        <input name="iconCls" type="text" style="width:96%;" />
						        </td>
					        </tr>
				        </table>
			        </form>
			        <div id="editAccMenuTree" class="easyui-window" title="菜单树" style="padding: 10px;width:500px;height:300px;"
				    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" id="editAccMenuTreeContent" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="accMenuTreeSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editAccMenuTree').window('close');">取消</a>
					        </div>
				        </div>
			        </div>
                    <!--选择模块-->
                    <div id="moduleSelect" class="easyui-window" title="选择模块" style="padding: 5px;width:540px;height:320px;"
				    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
                        <div class="easyui-layout" fit="true">
                            <div region="center" border="false" style="background:#fff;border:1px solid #ccc;">
                                <table id="dataGridSelectModule" title="模块" style="" border="false" fit="true" singleSelect="true"
			                    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb3">
			                        <thead>
				                        <tr>
					                        <th field="id" width="20" align="center" hidden="true">ID</th>
                                            <th field="sortNo" width="60" align="center" sortable="true">序号</th>
					                        <th field="name" width="230" align="left">名称</th>
				                        </tr>
			                        </thead>
		                        </table>
                                <div id="tb3">
                                    <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="moduleSelectReload();">刷新</a>
                                    <input id="moduleSelectSearch" type="text" style="width:120px;" />
			                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-search" onclick="moduleSelectSearch();"></a>
                                    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-ok"  onclick="moduleSelectOk()">确认</a>
								    <a href="javascript:void(0)"  class="easyui-linkbutton" plain="true" iconCls="icon-cancel"  onclick="$('#moduleSelect').window('close');">取消</a> 
                                </div>
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
    <!--导航菜单组--->
    <script type="text/javascript">
        $(function () {
            $("#dataGridAccMenu").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuList',
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentAccMenuId=rowData.id;
                    accMenuTreeReload();
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    accMenuEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentAccMenuId) {
                        $(this).datagrid('selectRecord', currentAccMenuId);
                    }
                    accMenuTreeReload();
                }
            })
            //accMenuReload();
        })
        function accMenuReload() {
            $('#dataGridAccMenu').datagrid('reload');
        }
        function accMenuSearch() {
            var queryParams = $('#dataGridAccMenu').datagrid('options').queryParams;
            queryParams.queryWord = $('#accMenuSearch').val();
            $('#dataGridAccMenu').datagrid('reload');
        }
        var currentAccMenuId = null;
        function accMenuAdd() {
            $("#editAccMenu").window("open");
            $("#editAccMenuForm").show();
            $("#editAccMenuForm").appendTo("#editAccMenuContent");
            $("#editAccMenuForm")[0].reset();

            $("#editAccMenuForm input[name='name']").focus();
            currentAccMenuId = null;
        }
        function accMenuEdit() {
            var selectRow = $('#dataGridAccMenu').datagrid('getSelected');
            if (selectRow) {
                $("#editAccMenu").window("open");
                $("#editAccMenuForm").show();
                $("#editAccMenuForm").appendTo("#editAccMenuContent");
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuEdit/' + selectRow.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        $("#editAccMenuForm input[name='name']").val(r.name);
                        $("#editAccMenuForm input[name='sortNo']").val(r.sortNo);
                        $("#editAccMenuForm input[name='target']").val(r.target);

                        $("#editAccMenuForm input[name='name']").focus();
                    }
                });
                currentAccMenuId = selectRow.id;
            } else {
                $.messager.alert("提示", "请先选中要编辑的一行数据！", "info");
            }
        }
        function accMenuSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuSave/' + currentAccMenuId;
                    $('#editAccMenuForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            return $('#editAccMenuForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editAccMenu').window('close');
                                accMenuReload(rVal.id);
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })
                }
            });

        }
        function accMenuDelete() {
            var selectRow = $('#dataGridAccMenu').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除菜单组[<font style='color:red;'>" + selectRow.name + "</font>]，删除此组会同时删除相应的菜单！", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    accMenuReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + retValue.message, 'error');
                                }
                            }

                        });
                    }
                })
            } else {
                $.messager.alert("提示", "请先选中要删除的一行数据！", "info");
            }
        }
    </script>
    <!--导航菜单树--->
    <script type="text/javascript">
        $(function () {
            $("#dataGridAccMenuTree").treegrid({
                onLoadSuccess: function () {
                    $(this).treegrid('unselectAll');
                },
                onDblClickRow: function (rowIndex, rowData) {
                    accMenuTreeEdit();
                },
                onClickRow: function (row) {

                }
            })
        })
        function accMenuTreeReload() {
            var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuTreeList?accMenuId=' + currentAccMenuId;
            $.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                success: function (result) {
                    $('#dataGridAccMenuTree').treegrid('loadData', result);
                }
            });
        }
        var currentTreeId = null;
        function accMenuTreeAdd(addMode) {
            var selectAccMenuRow = $('#dataGridAccMenu').datagrid('getSelected');
            if (!selectAccMenuRow) {
                $.messager.alert("提示", "请先选择菜单组！", "info");
                return;
            }
            $("#editAccMenuTree").window("open");
            $("#editAccMenuTreeForm").show();
            $("#editAccMenuTreeForm").appendTo("#editAccMenuTreeContent");
            $("#editAccMenuTreeForm")[0].reset();
            if (addMode == "root") {
                $("#editAccMenuTreeForm input[name='parentId']").val(0);
            } else {
                var selectRow = $('#dataGridAccMenuTree').treegrid('getSelected');
                if (selectRow) {
                    $("#editAccMenuTreeForm input[name='parentId']").val(selectRow.id);
                } else {
                    $.messager.alert("提示", "请先选中父菜单！", "info");
                    return;
                }
                $("#editAccMenuTreeForm input[name='parentId']").val(selectRow.id);
            }
            $("#editAccMenuTreeForm input[name='name']").focus();
            currentTreeId = null;
        }
        function accMenuTreeEdit() {
            var selectRow = $('#dataGridAccMenuTree').treegrid('getSelected');
            if (selectRow) {
                $("#editAccMenuTree").window("open");
                $("#editAccMenuTreeForm").show();
                $("#editAccMenuTreeForm").appendTo("#editAccMenuTreeContent");
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuTreeEdit/' + selectRow.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        $("#editAccMenuTreeForm")[0].reset();

                        $("#editAccMenuTreeForm input[name='name']").val(r.name);
                        $("#editAccMenuTreeForm input[name='sortNo']").val(r.sortNo);
                        $("#editAccMenuTreeForm input[name='moduleId']").val(r.moduleId);
                        $("#editAccMenuTreeForm input[name='moduleName']").val(r.moduleName);
                        $("#editAccMenuTreeForm input[name='iconCls']").val(r.iconCls);
                        $("#editAccMenuTreeForm input[name='name']").focus();
                    }
                });
                currentTreeId = selectRow.id;
            } else {
                $.messager.alert("提示", "请先选中要编辑的一行数据！", "info");
            }
        }
        function accMenuTreeSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var selectAccMenuRow = $('#dataGridAccMenu').datagrid('getSelected');
                    var accMenuId = selectAccMenuRow.id;
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuTreeSave/' + currentTreeId + "?accMenuId=" + accMenuId;
                    $('#editAccMenuTreeForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            return $('#editAccMenuTreeForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editAccMenuTree').window('close');
                                currentTreeId = rVal.id;
                                accMenuTreeReload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })   
                }
            });
        }
        function accMenuTreeDelete() {
            var selectRow = $('#dataGridAccMenuTree').treegrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除菜单[<font style='color:red;'>" + selectRow.name + "</font>]，删除此菜单会同时删除其子菜单！", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebMenu/AccMenuTreeDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    var selectAccMenuRow = $('#dataGridAccMenu').datagrid('getSelected');
                                    var accMenuId = selectAccMenuRow.id;
                                    accMenuTreeReload(accMenuId);
                                } else {
                                    $.messager.alert('确认', '删除失败:' + retValue.message, 'error');
                                }
                            }

                        });
                    }
                })
            } else {
                $.messager.alert("提示", "请先选中要删除的一行数据！", "info");
            }
        }
    </script>
    <!--选择模块-->
    <script type="text/javascript">
        $(function () {
            $('#dataGridSelectModule').datagrid({
                pagination: true,
                pageSize: 30,
                noheader: true,
                loadMsg: '加载数据...',
				onSelect:function(rowIndex, rowData){

				},
				onDblClickRow: function (rowIndex, rowData) {
				    moduleSelectOk();
				},
				onLoadSuccess:function(){
				    $('#dataGridSelectModule').datagrid('clearSelections');
				}
			});
        })
        function moduleSelectReload() {
            $('#dataGridSelectModule').datagrid('reload');
        }
        function selectModule(){
			$('#moduleSelect').window('open');
			var queryParams = $('#dataGridSelectModule').datagrid('options').queryParams;
			queryParams.queryWord = $('#moduleSelectSearch').val();
			$('#dataGridSelectModule').datagrid({
			    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebModule/ModuleList'
			});
		}
		function moduleSelectSearch(){
		    var queryParams = $('#dataGridSelectModule').datagrid('options').queryParams;
			queryParams.queryWord = $('#moduleSelectSearch').val();
			$('#dataGridSelectModule').datagrid('reload');
		}
		function moduleSelectOk(){
		    var selectRow = $('#dataGridSelectModule').datagrid('getSelected');
			if(selectRow){
				$('#moduleId').val(selectRow.id);
				$('#moduleName').val(selectRow.name);
			}else{
				$.messager.alert("提示","没有选择模块！","info");
				return;
			}
			$('#moduleSelect').window('close');
		}
		function removeModule(){
			$('#moduleId').val("");
			$('#moduleName').val("");
		}
    </script>
</asp:Content>
