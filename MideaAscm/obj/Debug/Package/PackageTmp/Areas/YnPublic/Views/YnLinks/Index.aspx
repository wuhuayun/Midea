<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnFrame.Dal.Entities.YnLinks>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!-- 友情链接 -->
    <div region="center" title="" border="true" style="padding:0px 0px 0px 0px;">
		<table id="dataGridLinks" title="友情链接" style="" border="false" fit="true" singleSelect="true"
		idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb">
			<thead>
				<tr>
					<th field="id" width="20" align="center" hidden="true">ID</th>
                    <th field="sortNo" width="60" align="center" sortable="true">序号</th>
					<th field="title" width="180" align="left">标题</th>
                    <th field="url" width="320" align="center">链接地址</th>
				</tr>
			</thead>
		</table>
        <div id="tb">
            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="linksReload();">刷新</a>
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="linksAdd();">增加</a>
            <%} %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-edit" onclick="linksEdit();">修改</a>
            <%} %>
            <% if (ynWebRight.rightDelete){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="linksDelete();">删除</a>
            <%} %>
            <span>类型：</span><%=Html.DropDownList("listLinksType")%> 
        </div>
		<form id="editLinksForm" method="post" style="display:none;">
			<table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				<tr style="height:24px">
					<td style="width:10%; text-align:right;" nowrap>
						<span>标题：</span>
					</td>
					<td style="width:80%">
						<input class="easyui-validatebox" required="true" id="title" name="title" type="text" style="width:96%;"/><span style="color:Red;">*</span>
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
					<td style="text-align:right;" nowrap>
						<span>链接地址：</span>
					</td>
					<td style="width:80%">
						<input class="easyui-validatebox" id="url" name="url" type="text" style="width:96%;"/>
					</td>
				</tr>
			</table>
		</form>
		<div id="editLinks" class="easyui-window" title="发布链接" style="padding: 10px;width:500px;height:340px;"
			iconCls="icon-edit" closed="true" maximizable="false" minimizable="false" resizable="false" collapsible="false" modal="true" shadow="true">
			<div class="easyui-layout" fit="true">
				<div region="center" id="editLinksContent" border="false" style="padding:10px;background:#eff3ff;border:1px solid #ccc;">
				</div>
				<div region="south" border="false" style="text-align:right;height:30px;line-height:30px;">
                    <% if (ynWebRight.rightAdd || ynWebRight.rightEdit){ %>
					<a id="btnSave" class="easyui-linkbutton" iconCls="icon-ok" href="javascript:void(0)" onclick="linksSave()">保存</a>
                    <%} %>
					<a class="easyui-linkbutton" iconCls="icon-cancel" href="javascript:void(0)" onclick="$('#editLinks').window('close');">取消</a>
				</div>
			</div>
		</div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        $(function () {
            $("#dataGridLinks").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnLinks/LinksList/' + $("#listLinksType").val(),
                pagination: true,
                pageSize: 50,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    linksEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                }
            })
            $("#listLinksType").change(function () { linksReload(); });
        })
        function linksReload() {
            //$('#dataGridLinks').datagrid('reload');
            $("#dataGridLinks").datagrid({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnLinks/LinksList/' + $("#listLinksType").val()
            })
        }
        function linksSearch() {
            var queryParams = $('#dataGridLinks').datagrid('options').queryParams;
            queryParams.queryWord = $('#linksSearch').val();
            $('#dataGridLinks').datagrid('reload');
        }
        var currentId = null;
        function linksAdd() {
            $("#editLinks").window("open");
            $("#editLinksForm").show();
            $("#editLinksForm").appendTo("#editLinksContent");
            $("#editLinksForm")[0].reset();

            $("#editLinksForm input[name='title']").focus();
            currentId = null;
        }
        function linksEdit() {
            var selectRow = $('#dataGridLinks').datagrid('getSelected');
            if (selectRow) {
                $("#editLinks").window("open");
                $("#editLinksForm").show();
                $("#editLinksForm").appendTo("#editLinksContent");
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnLinks/LinksEdit/' + selectRow.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        $("#editLinksForm")[0].reset();

                        $("#editLinksForm input[name='title']").val(r.title);
                        $("#editLinksForm input[name='sortNo']").val(r.sortNo);
                        $("#editLinksForm input[name='url']").val(r.url);

                        $("#editLinksForm input[name='title']").focus();
                    }
                });
                currentId = selectRow.id;
            } else {
                $.messager.alert("提示", "请选中要编辑的一行数据！", "info");
            }
        }
        function linksSave() {
            $.messager.confirm("确认", "确认提交保存?", function (r) {
                if (r) {
                    var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnLinks/LinksSave/' + currentId;
                    var params = {
                        typeId: $("#listLinksType").val()
                    };
                    sUrl += '?' + $.param(params);
                    $('#editLinksForm').form('submit', {
                        url: sUrl,
                        onSubmit: function () {
                            return $('#editLinksForm').form('validate');
                        },
                        success: function (r) {
                            var rVal = $.parseJSON(r);
                            if (rVal.result) {
                                $('#editLinks').window('close');
                                linksReload();
                            } else {
                                $.messager.alert('确认', '保存失败:' + rVal.message, 'error');
                            }
                        }
                    })
                }
            });

        }
        function linksDelete() {
            var selectRow = $('#dataGridLinks').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除链接[<font style='color:red;'>" + selectRow.title + "</font>]", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnLinks/LinksDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    linksReload();
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
