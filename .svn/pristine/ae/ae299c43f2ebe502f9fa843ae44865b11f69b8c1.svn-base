<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnCMS.Dal.CMS.Entities.YnWebChannel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--频道-->
    <div region="west" split="false" border="false" title="" style="width:250px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="频道" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
            <div id="header"></div>
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div style="float:left; height:26px; width:100%;background:#F4F4F4;border-bottom:1px solid #DDDDDD; padding:1px;">
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="ChannelTreeReload();">刷新</a>
	                </div>
                </div>
			    <div region="center" title="" border="false">
	                <ul id="treeChannel">
	                </ul>
			    </div>
			</div>
		</div>
	</div>
    <!--栏目-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
		            <table id="dataGridClass" title="栏目管理" style="" border="false" fit="true" singleSelect="true"
		                idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb2">
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="ClassReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ClassAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="ClassEdit();">修改</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="ClassDelete();">删除</a>
                        <%} %>
                    </div>
			        <div id="editClass" class="easyui-window" title="栏目编辑" style="padding: 10px;width:500px;height:300px;"
				    iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
				        <div class="easyui-layout" fit="true">
			                <div region="center" border="false" style="padding:10px;background:#fff;border:1px solid #ccc;">
			                    <form id="editClassForm" method="post" style="">
				                    <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					                    <tr style="height:24px">
						                    <td style="width:10%; text-align:right;" nowrap>
							                    <span>栏目名称：</span>
						                    </td>
						                    <td style="width:90%">
                                                <input name="parentId" type="text" style="width:10px;display:none;" />
							                    <input class="easyui-validatebox" required="true" name="title" type="text" style="width:96%;" /><span style="color:Red;">*</span>
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
						                    </td>
					                    </tr>
                                        <tr style="height:24px">
						                    <td style="width:10%; text-align:right;" nowrap>
							                    <span>图标：</span>
						                    </td>
						                    <td style="width:90%">
							                    <input name="iconCls" type="text" style="width:96%;" />
						                    </td>
					                    </tr>
				                    </table>
			                    </form>
					        </div>
					        <div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                                <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
						        <a class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="ClassSave()">保存</a>
                                <%} %>
						        <a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editClass').window('close');">取消</a>
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
    <!--频道--->
    <script type="text/javascript">
        var channelId = null;
        $(function () {
            $('#treeChannel').tree({
                checkbox: false,
                url: '/YnPublic/YnCMS/ChannelTree',
                onBeforeExpand: function (node, param) {
                    //$('#MyTree').tree('options').url = "/category/getCategorys.java?Id=" + node.id;
                },
                onClick: function (node) {
                    channelId = node.id;
                    ClassReload(node.id);
                    //                    var state = node.state;
                    //                    if (!state) {                                   //判断当前选中的节点是否为根节点
                    //                        currentId = node.id;
                    //                        $("#chooseOk").attr("disabled", false);   //如果为根节点 则OK按钮可用
                    //                    } else {
                    //                        $("#chooseOk").attr("disabled", true);    //如果不为根节点 则OK按钮不可用
                    //                    }
                },
                onSelect: function (node) {
                    //alert(node.id)
                }
            });
        })
        function ChannelTreeReload() {
            $('#treeChannel').tree({
                url: '/YnPublic/YnCMS/ChannelTree'
            });
        }
    </script>
    <!--栏目--->
    <script type="text/javascript">
        $(function () {
            $("#dataGridClass").datagrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ClassList/',
                frozenColumns: [[
	                { field: 'title', title: '栏目名称', width: 80, sortable: true, align: 'center' }
				]],
                columns: [[
					{ field: 'summary', title: '描述', width: 100, align: 'left' },
					{ field: 'url', title: 'URL', width: 150, align: 'left' },
					{ field: 'enabled', title: '是否启用', width: 60, align: 'center' },
				]],
                //pagination: true,
                //pageSize: 50,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    ClassEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })
        })
        function ClassReload() {
            var title = '频道栏目';
            var node = $('#treeChannel').tree('getSelected');
            if (node) {
                title = '频道[<font color="red">' + node.text + '</font>]栏目';
            }
            //$('#dataGridChannel').datagrid('reload');
            $("#dataGridClass").datagrid({
                title: title,
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ClassList/'+channelId
            })
        }
        var currentId = null;
        function ClassAdd() {
            var node = $('#treeChannel').tree('getSelected');
            if (!node) {
                $.messager.alert("提示", "请先选中频道！", "info");
                return;
            }
            $("#editClass").window("open");
            $("#editClassForm")[0].reset();

            //$("#editClassForm input[name='parentId']").val(selectRow.id);
            $("#editClassForm input[name='title']").focus();
            currentId = null;
        }
        function ClassEdit() {
            var selectRow = $('#dataGridClass').datagrid('getSelected');
            if (selectRow) {
                $("#editClass").window("open");
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ClassEdit/' + selectRow.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        $("#editClassForm")[0].reset();

                        $("#editClassForm input[name='title']").val(r.title);
                        $("#editClassForm input[name='sortNo']").val(r.sortNo);
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert("提示", "请先选中要编辑的一行数据！", "info");
            }
        }
        function ClassSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var node = $('#treeChannel').tree('getSelected');
                    if (!node) {
                        return;
                    }
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ClassSave/' + currentId + "?channelId=" + node.id;
                    $('#editClassForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            return $('#editClassForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editClass').window('close');
                                currentId = rVal.id;
                                ClassReload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })
                }
            });
        }
        function ClassDelete() {
            var selectRow = $('#dataGridClass').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除栏目[<font style='color:red;'>" + selectRow.title + "</font>]，删除此栏目会同时删除栏目下文章！", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ClassDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    currentId = null;
                                    ClassReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + retValue.message, 'error');
                                }
                            }

                        });
                    }
                })
            } else {
                $.messager.alert("提示", "请先选中要删除的记录！", "info");
            }
        }
    </script>
</asp:Content>
