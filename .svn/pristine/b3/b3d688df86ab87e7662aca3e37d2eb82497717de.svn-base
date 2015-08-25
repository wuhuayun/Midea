<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnCMS.Dal.CMS.Entities.YnWebChannel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <!--频道栏目-->
    <div region="west" split="false" border="false" title="" style="width:250px;padding:0px 2px 0px 0px;overflow:auto;">
		<div class="easyui-panel" title="频道栏目" fit="true" border="true">
			<div class="easyui-layout" fit="true" style="" border="false">
            <div id="header"></div>
                <div region="north" title="" border="false" style="height:30px;overflow:hidden;">
                    <div style="float:left; height:26px; width:100%;background:#F4F4F4;border-bottom:1px solid #DDDDDD; padding:1px;">
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="ChannelClassTreeReload();">刷新</a>
	                </div>
                </div>
			    <div region="center" title="" border="false">
	                <ul id="treeChannelClass">
	                </ul>
			    </div>
			</div>
		</div>
	</div>
    <!--文章-->
    <div region="center" title="" border="false" style="padding:0px 0px 0px 0px;">
		<div class="easyui-panel" fit="true" border="false">
			<div class="easyui-layout" fit="true" style="">
			    <div region="center" border="true" fit="true" style="padding:0px 0px 0px 0px;overflow:auto;">
		            <table id="dataGridArticle" title="文章管理" style="" border="false" fit="true" singleSelect="true"
		                idField="id" sortName="sortNo" sortOrder="asc" striped="true" toolbar="#tb2">
		            </table>
                    <div id="tb2">
                        <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-reload" onclick="ArticleReload();">刷新</a>
                        <% if (ynWebRight.rightAdd){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-add" onclick="ArticleAdd();">增加</a>
                        <%} %>
                        <% if (ynWebRight.rightEdit){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" id="articleEdit" plain="true" icon="icon-edit" onclick="ArticleEdit();">修改</a>
			            <a href="javascript:void(0);" class="easyui-linkbutton" id="articlePublish" plain="true" onclick="ArticlePublish();"><img alt='' border="0" src='<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/images/checkvalues.gif' width="16" height="16"  style="vertical-align:middle;"/>同意发布</a>
                        <%} %>
                        <% if (ynWebRight.rightDelete){ %>
			            <a href="javascript:void(0);" class="easyui-linkbutton" plain="true" icon="icon-cancel" onclick="ArticleDelete();">删除</a>
                        <%} %>
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
        var classId = null;
        $(function () {
            $('#treeChannelClass').tree({
                checkbox: false,
                url: '/YnPublic/YnCMS/ChannelClassTree',
                onBeforeExpand: function (node, param) {
                    //$('#MyTree').tree('options').url = "/category/getCategorys.java?Id=" + node.id;
                },
                onClick: function (node) {
//                    if (node.id > 0) {
//                        classId = node.id;
//                        ArticleReload();
//                    }
                },
                onSelect: function (node) {
                    if (node.id > 0) {
                        classId = node.id;
                        ArticleReload();
                    }
                }
            });
        })
        function ChannelClassTreeReload() {
            $('#treeChannelClass').tree({
                url: '/YnPublic/YnCMS/ChannelClassTree'
            });
        }
    </script>
    <!--文章--->
    <script type="text/javascript">
        $(function () {
            $("#dataGridArticle").datagrid({
                //url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleList/',
                frozenColumns: [[
	                { field: 'title', title: '文章名称', width: 150, sortable: true, align: 'left' }
				]],
                columns: [[
					{ field: 'titleColor', title: '标题颜色', width: 60, align: 'center' },
					{ field: 'addTime', title: '录入时间', width: 120, align: 'center' },
					{ field: 'summary', title: '简介', width: 300, align: 'left' },
					{ field: 'editor', title: '编辑', width: 40, align: 'center' },
					{ field: 'viewNum', title: '浏览次数', width: 55, align: 'center' },
					{ field: 'statusName', title: '状态', width: 40, align: 'center' },
					{ field: 'isImg', title: '有图片', width: 45, align: 'center' },
					{ field: 'isTop', title: '是否推荐', width: 55, align: 'center' },
					{ field: 'isFocus', title: '是否焦点', width: 55, align: 'center' },
					{ field: 'isHead', title: '是否置顶', width: 55, align: 'center' },
					{ field: 'userId', title: '投稿者', width: 45, align: 'center' }
				]],
                pagination: true,
                pageSize: 50,
                loadMsg: '加载数据......',
                onSelect: function (rowIndex, rowData) {
                    currentId=rowData.id;
                    SetButton();
                },
                onDblClickRow: function (rowIndex, rowData) {
                    <% if (ynWebRight.rightEdit){ %>
                    ArticleEdit();
                    <%} %>
                },
                onLoadSuccess: function (data) {
                    $(this).datagrid('clearSelections');
                    if (currentId) {
                        $(this).datagrid('selectRecord', currentId);
                    }
                    SetButton();
                }
            })
        })
        function ArticleReload() {
            var title = '频道栏目文章';
            var node = $('#treeChannelClass').tree('getSelected');
            if (node&&node.id>0) {
                $.ajax({
                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ClassEdit/' + node.id,
                    type: 'post',
                    dataType: 'json',
                    success: function (r) {
                        title = '频道栏目[<font color="red">' +r.ynWebChannel.name+ "->"+r.title + '</font>]文章';
                        $("#dataGridArticle").datagrid({
                            title: title,
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleList/'+classId
                        })
                    }
                });
            }
        }
        var currentId = null;
        function ArticleAdd() {
            var node = $('#treeChannelClass').tree('getSelected');
            if (!node) {
                $.messager.alert("提示", "请先选中频道栏目！", "info");
                return;
            }
            var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleEdit?mi=<%=Request["mi"].ToString() %>&classId='+node.id;
            parent.openTab('文章_新增', url);


//            $("#editArticle").window("open");
//            $("#editArticleForm")[0].reset();

//            //$("#editArticleForm input[name='parentId']").val(selectRow.id);
//            $("#editArticleForm input[name='title']").focus();
//            currentId = null;
        }
        function ArticleEdit() {
            var selectRow = $('#dataGridArticle').datagrid('getSelected');
            if (selectRow) {
                var url = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleEdit/' + selectRow.id+"?mi=<%=Request["mi"].ToString() %>";
                parent.openTab('文章_' + selectRow.id, url);
//                $("#editArticle").window("open");
//                $.ajax({
//                    url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleEdit/' + selectRow.id,
//                    type: 'post',
//                    dataType: 'json',
//                    success: function (r) {
//                        $("#editArticleForm input[name='title']").val(r.title);
//                        //$("#editArticleForm input[name='sortNo']").val(r.sortNo);
//                    }
//                });
                currentId = selectRow.id;
            } else {
                $.messager.alert("提示", "请先选中要编辑的一行数据！", "info");
            }
        }
        function ArticleDelete() {
            var selectRow = $('#dataGridArticle').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm("确认", "确认删除文章[<font style='color:red;'>" + selectRow.title + "</font>]！", function (r) {
                    if (r) {
                        $.ajax({
                            url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleDelete/' + selectRow.id,
                            type: "post",
                            dataType: "json",
                            success: function (retValue) {
                                if (retValue.result) {
                                    currentId = null;
                                    ArticleReload();
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
        function SetButton() {
            var status=null;
            var selectRow = $('#dataGridArticle').datagrid('getSelected');
            $('#articlePublish').hide();
            if (selectRow){
                status=selectRow.status;
            }
            if(status==<% =YnCMS.Dal.CMS.Entities.YnModuleArticle.StatusType.draft.value%>){
                $('#articlePublish').show();
            }
        }
        function ArticlePublish() {
            var selectRow = $('#dataGridArticle').datagrid('getSelected');
            if (selectRow) {
                $.messager.confirm('确认', '确认同意发布[<font color="red">' + selectRow.title + '</font>]？', function (result) {
                    if (result) {
                        var sUrl = '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticlePublish/' + selectRow.id;
                        $.ajax({
                            url: sUrl,
                            type: "post",
                            dataType: "json",
                            success: function (r) {
                                if (r.result) {
                                    ArticleReload();
                                } else {
                                    $.messager.alert('确认', '发布失败:' + r.message, 'error');
                                }
                            }
                        });
                    }
                });
            } else {
                $.messager.alert('提示', '请选择要发布的文章', 'info');
            }
        }
    </script>
</asp:Content>
