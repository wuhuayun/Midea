<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnNews>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 新闻信息 -->
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
		<table id="dataGridNews" title="新闻发布" style="" border="false" fit="true" singleSelect="true"
		idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb">
			<thead>
				<tr>
					<th field="id" width="20" align="center" hidden="true">ID</th>
                    <th field="sortNo" width="60" align="center" sortable="true">序号</th>
					<th field="title" width="320" align="left">标题</th>
                    <th field="createUser" width="120" align="center">发布者</th>
                    <th field="createTime" width="180" align="center" sortable="true">发布时间</th>
				</tr>
			</thead>
		</table>
        <div id="tb">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="newsReload();">刷新</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="newsAdd();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="newsEdit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="newsDelete();">删除</a>
            <%} %>
            <span>新闻类型：</span><%=Html.DropDownList("listNewsType")%> 
        </div>
		<form id="editNewsForm" method="post" style="display:none;">
			<table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				<tr style="height:24px">
					<td style="width:10%; text-align:right;" nowrap>
						<span>标题：</span>
					</td>
					<td style="width:80%">
						<input class="easyui-validatebox" required="true" id="title" name="title" type="text" style="width:96%;" /><span style="color:Red;">*</span>
					</td>
				</tr>
				<tr style="height:24px">
					<td style="text-align:right;" nowrap>
						<span>序号：</span>
					</td>
					<td>
						<input class="easyui-validatebox" id="sortNo" name="sortNo" type="text" validType="number" style="text-align:right"/>
					</td>
				</tr>
                <tr style="height:24px">
					<td style="width:100%;" colspan="2">
						<textarea class="jquery_ckeditor" cols="80" rows="10"></textarea>
                        <input type="hidden" id="newsContent" name="newsContent"/>
					</td>
				</tr>
			</table>
		</form>
		<div id="editNews" class="easyui-window" title="发布新闻" style="padding: 10px;width:850px;height:540px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" id="editNewsContent" border="false" style="padding:10px;background:#eff3ff;border:1px solid #ccc;">
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="newsSave()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editNews').window('close');">取消</a>
				</div>
			</div>
		</div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/ckeditor/adapters/jquery.js"></script>
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $("#dataGridNews").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnNews/NewsList/' + $("#listNewsType").val(),
                pagination: true,
                pageSize: 50,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    newsEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })
            var config = {};
            $('.jquery_ckeditor').ckeditor(config);
            $("#listNewsType").change(function () { newsReload(); });
        })
        function newsReload() {
            //$('#dataGridNews').datagrid('reload');
            $("#dataGridNews").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnNews/NewsList/' + $("#listNewsType").val()
            })
        }
        function roleSearch() {
            var queryParams = $('#dataGridNews').datagrid('options').queryParams;
            queryParams.queryWord = $('#newsSearch').val();
            $('#dataGridNews').datagrid('reload');
        }
        var currentId = null;
        function newsAdd() {
            $("#editNews").window("open");
            $("#editNewsForm").show();
            $("#editNewsForm").appendTo("#editNewsContent");
            $("#editNewsForm")[0].reset();
            $('.jquery_ckeditor').val("");

            $("#editNewsForm input[name='title']").focus();
            currentId = null;
        }
        function newsEdit() {
            var selectRow = $('#dataGridNews').datagrid('getSelected');
            if (selectRow) {
                $("#editNews").window("open");
                $("#editNewsForm").show();
                $("#editNewsForm").appendTo("#editNewsContent");
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnNews/NewsEdit/' + selectRow.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        $("#editNewsForm")[0].reset();

                        $("#editNewsForm input[name='title']").val(r.title);
                        $("#editNewsForm input[name='sortNo']").val(r.sortNo);
                        $('.jquery_ckeditor').val(r.newsContent);

                        $("#editNewsForm input[name='title']").focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert("提示", "请选中要编辑的一行数据！", "info");
            }
        }
        function newsSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnNews/NewsSave/' + currentId;
                    var params = {
                        typeId: $("#listNewsType").val()
                    };
                    sUrl += '?' + $.param(params);
                    $('#editNewsForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            $('#newsContent').val(escape($('.jquery_ckeditor').val()));
                            return $('#editNewsForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editNews').window('close');
                                newsReload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })
                }
            });
        }
        function newsDelete() {
            var selectRow = $('#dataGridNews').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除新闻[<font style='color:red;'>" + selectRow.title + "</font>]", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnNews/NewsDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    newsReload();
                                } else {
                                    $.messager.alert('确认', '删除失败:' + retValue.message, 'error');
                                }
                            }

                        });
                    }
                })
            } else {
                $.messager.alert("提示", "请选中要删除的一行数据！", "info");
            }
        }
    </script>
</asp:Content>
