<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnCMS.Dal.CMS.Entities.YnWebChannel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 频道管理 -->
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
		<table id="dataGridChannel" title="频道管理" style="" border="false" fit="true" singleSelect="true"
		    idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb">
		</table>
        <div id="tb">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="channelReload();">刷新</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="channelAdd();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="channelEdit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="channelDelete();">删除</a>
            <%} %>
        </div>
		<div id="editChannel" class="easyui-window" title="频道编辑" style="padding: 10px;width:640px;height:420px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" border="false" style="padding:10px;background:#eff3ff;border:1px solid #ccc;">
		            <form id="editChannelForm" method="post" style="">
			            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				            <tr style="height:24px">
					            <td style="width:18%; text-align:right;" nowrap>
						            <span>频道名称：</span>
					            </td>
					            <td style="width:82%;" colspan="3">
						            <input class="easyui-validatebox" required="true" id="name" name="name" type="text" style="width:96%;"/><span style="color:Red;">*</span>
					            </td>
				            </tr>
				            <tr style="height:24px">
					            <td style="width:18%; text-align:right;">
						            <span>序号：</span>
					            </td>
					            <td style="width:32%;">
						            <input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" validType="number" style="text-align:right"/>
					            </td>
					            <td style="width:18%; text-align:right;" nowrap>
						            <span>是否启用：</span>
					            </td>
					            <td style="width:32%">
						            <input class="easyui-validatebox" type="checkbox" id="enabled" name="enabled" value=""/>
					            </td>
				            </tr>
                            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>描述：</span>
					            </td>
					            <td style="" colspan="3">
						            <input class="easyui-validatebox" id="description" name="description" type="text" style="width:96%;"/>
					            </td>
				            </tr>
                            <tr style="height:24px">
					            <td style="text-align:right;" nowrap>
						            <span>链接地址：</span>
					            </td>
					            <td style="" colspan="3">
						            <input class="easyui-validatebox" id="url" name="url" type="text" style="width:96%;"/>
					            </td>
				            </tr>
			            </table>
		            </form>
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="channelSave()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editChannel').window('close');">取消</a>
				</div>
			</div>
		</div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $("#dataGridChannel").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ChannelList/',
                frozenColumns: [[
	                { field: 'name', title: '频道名称', width: 80, sortable: true, align: 'center' }
				]],
                columns: [[
					{ field: 'description', title: '描述', width: 100, align: 'left' },
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
                    channelEdit();
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
        function channelReload() {
            //$('#dataGridChannel').datagrid('reload');
            $("#dataGridChannel").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ChannelList/'
            })
        }
        function channelSearch() {
            var queryParams = $('#dataGridChannel').datagrid('options').queryParams;
            queryParams.queryWord = $('#channelSearch').val();
            $('#dataGridChannel').datagrid('reload');
        }
        var currentId = null;
        function channelAdd() {
            $("#editChannel").window("open");
            $("#editChannelForm")[0].reset();

            $("#editChannelForm input[name='name']").focus();
            currentId = null;
        }
        function channelEdit() {
            var selectRow = $('#dataGridChannel').datagrid('getSelected');
            if (selectRow) {
                $("#editChannel").window("open");
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ChannelEdit/' + selectRow.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        $("#editChannelForm")[0].reset();

                        $("#editChannelForm input[name='name']").val(r.name);
                        $("#editChannelForm input[name='sortNo']").val(r.sortNo);
                        $("#editChannelForm input[name='url']").val(r.url);
                        $("#editChannelForm input[name='description']").val(r.description);
                        $("#enabled").prop("checked", r.enabled);

                        $("#editChannelForm input[name='name']").focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert("提示", "请选中要编辑的一行数据！", "info");
            }
        }
        function channelSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ChannelSave/' + currentId;
                    $('#editChannelForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            $("#enabled").val($("#enabled").prop("checked"));
                            return $('#editChannelForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editChannel').window('close');
                                currentId=rVal.id;
                                channelReload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })
                }
            });

        }
        function channelDelete() {
            var selectRow = $('#dataGridChannel').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除频道[<font style='color:red;'>" + selectRow.name + "</font>]，删除此频道会同时删除频道下栏目和文章", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ChannelDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    channelReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + retValue.message, 'error');
                                }
                            }

                        });
                    }
                })
            } else {
                $.messager.alert("提示", "请选中要删除的记录！", "info");
            }
        }
    </script>
</asp:Content>
