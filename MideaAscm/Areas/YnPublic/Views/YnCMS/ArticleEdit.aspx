<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<YnCMS.Dal.CMS.Entities.YnWebChannel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	文章
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <div region="north" title="" border="true" style="height:30px;overflow:hidden;">
        <div class="div_toolbar" style="float:left; height:26px; width:100%; border-bottom:0px solid #99BBE8; padding:1px;">
            <% if (ynWebRight.rightAdd){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" id="articleNew" plain="true" icon="icon-add" onclick="ArticleNew();">新文章</a>
            <% } %>
            <% if (ynWebRight.rightEdit){ %>
			<a href="javascript:void(0);" class="easyui-linkbutton" id="articleSave" plain="true" icon="icon-save" onclick="ArticleSave();">保存</a>
			<% } %>
            <a href="javascript:void(0);" class="easyui-linkbutton" id="articlePrint" plain="true" icon="icon-print" onclick="ArticlePrint();">打印</a>
	    </div>
	</div>
    <!--文章-->
    <div region="center" title="" border="true" style="padding:10px 10px 10px 10px;background:#eff3ff;">
		<form id="editArticleForm" method="post" style="" enctype="multipart/form-data">
			<table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
				<tr style="height:24px">
					<td style="width:70px; text-align:right;" nowrap>
						<span>文章标题：</span>
					</td>
					<td style="" colspan="3">
						<input class="easyui-validatebox" required="true" name="title" type="text" style="width:345px;" /><span style="color:Red;">*</span>
					</td>
					<td style="width:70px;text-align:right;">
						<span>状态：</span>
					</td>
					<td style="width:140px;">
						<input name="statusName" type="text" style="width:120px;background-color:#CCCCCC" readonly="readonly"/>
					</td>
					<td style="">
					</td>
				</tr>
				<tr style="height:24px">
					<td style="width:70px;text-align:right;">
						<span>标题颜色：</span>
					</td>
					<td style="width:140px;">
						<input class="easyui-validatebox" name="titleColor" type="text" style="width:120px;"/>
					</td>
					<td style="width:70px;text-align:right;">
						<span>作者：</span>
					</td>
					<td style="width:140px;">
						<input class="easyui-validatebox" name="author" type="text" style="width:120px;"/>
					</td>
					<td style="width:70px;text-align:right;">
						<span>浏览次数：</span>
					</td>
					<td style="width:140px;">
						<input name="viewNum" type="text" style="width:120px;background-color:#CCCCCC" readonly="readonly"/>
					</td>
					<td style="">
					</td>
				</tr>
				<tr style="height:24px">
					<td style="text-align:right;">
						<span>有图片：</span>
					</td>
					<td style="">
						<input class="easyui-validatebox" type="checkbox" id="isImg" name="isImg" value=""/>
					</td>
					<td style="text-align:right;">
						<span>是否推荐：</span>
					</td>
					<td style="">
						<input class="easyui-validatebox" type="checkbox" id="isTop" name="isTop" value=""/>
					</td>
					<td style="text-align:right;">
						<span>是否焦点：</span>
					</td>
					<td style="">
						<input class="easyui-validatebox" type="checkbox" id="isFocus" name="isFocus" value=""/>
					</td>
					<td style="">
					</td>
				</tr>
                <tr style="height:50px">
					<td style="text-align:right; vertical-align:top">
						<span>简介：</span>
					</td>
					<td style="" colspan="6">
                        <textarea id="summary" name="summary" rows="1" cols="3" style="width:572px;border:1px solid #A4BED4;height:40px;"></textarea>
					</td>
				</tr>
                <tr style="height:24px">
					<td style="text-align:right; vertical-align:top">
						<span>缩略图：</span>
					</td>
                    <td style="" colspan="6">
                        <input type="file" id="fileUpload" name="fileUpload" size="23" style="width:472px;"/></br>
                        <img src='' alt="" id="img"/>
					</td>
				</tr>
                <tr style="height:24px">
					<td style="text-align:right; vertical-align:top">
						<span>文章内容：</span>
					</td>
                    <td style="" colspan="6">
						<textarea id="ckeditor_content" class="jquery_ckeditor" rows="20" cols="80" style="width:98%;border:1px solid #A4BED4;height:600px;"></textarea>
                        <input type="hidden" id="content" name="content"/>
					</td>
				</tr>
			</table>
		</form>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/ckeditor/adapters/jquery.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/ckfinder/ckfinder.js"></script>
    <script type="text/javascript" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/AjaxFileUploader/ajaxfileupload.js"></script>
    <link href="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/AjaxFileUploader/ajaxfileupload.css" rel="stylesheet" type="text/css" />

    <% YnFrame.Code.YnWebRight ynWebRight = YnFrame.Code.YnPermission.GetInstance().GetYnWebRight(); %>
    <script type="text/javascript">
        var ckeditor_img = null;
        var ckeditor_content = null;
        $(function () {
            var config_content = {
                height: 500
            };
            ckeditor_content = CKEDITOR.replace('ckeditor_content', config_content);
            CKFinder.setupCKEditor(ckeditor_content, '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Areas/YnPublic/Content/ckfinder/');
            //CKFinder.SetupCKEditor(editor, '../ckfinder/');
        })
    </script>
    <script type="text/javascript">
        var articleId = null;
        var classId = null;
        $(function () {
            <% if (ViewData.ContainsKey("classId")){ %>
                classId = <%=Request["classId"].ToString()%>
            <%}%>
            <% if (ViewData.ContainsKey("id")){ %>
                articleId = <%=ViewData["id"].ToString() %>
                LoadArticle(articleId);
            <%}else{ %>
                ArticleNew();
            <%} %>
        })
        function ArticleNew() {
            articleId = null;
            $("#editArticleForm")[0].reset();
            //$('.jquery_ckeditor').val("");
            ckeditor_img.setData("");
            ckeditor_content.setData("");
            SetButton(status);
        }
        function LoadArticle(id) {
            $.ajax({
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleLoad/' + id,
                cache: false,
                dataType: 'json',
                success: function(r){
                    if (r) {
                        $("#editArticleForm")[0].reset();
                        $("#editArticleForm input[name='title']").val(r.title);
                        $("#editArticleForm input[name='statusName']").val(r.statusName);
                        $("#editArticleForm input[name='titleColor']").val(r.titleColor);
                        $("#editArticleForm input[name='author']").val(r.author);
                        $("#editArticleForm input[name='viewNum']").val(r.viewNum);
                        //$("#editArticleForm input[name='summary']").val(r.summary);
                        $('#summary').val(r.summary);
                        $("#isImg").prop("checked", r.isImg);
                        $("#isTop").prop("checked", r.isTop);
                        $("#isFocus").prop("checked", r.isFocus);
                        $("#img").prop("src", r.img);

                        //$('.jquery_ckeditor').val(r.content);
                        ckeditor_content.setData(r.content);

                        SetButton(r.status);
                    }else{
                        SetButton(false);
                    }
                }
            })
        }
        function SetButton(status) {
            $('#articleNew').show();
            $('#articleSave').show();
            $('#articlePrint').hide();

            if (!articleId) {
                $('#articlePrint').hide();
            }else{
                $('#articleNew').hide();
                if(status==<% =YnCMS.Dal.CMS.Entities.YnModuleArticle.StatusType.draft.value%>){
                }else{
                }
            }
        }
        function ArticleSave(statusType) {
            $.messager.confirm("确认", "确认保存？", function (r) {
                if (r) {
                    ArticleSave1();
                }
            })
        }
        function ArticleSave1(statusType) {
            $("#isImg").val($("#isImg").prop("checked"));
            $("#isTop").val($("#isTop").prop("checked"));
            $("#isFocus").val($("#isFocus").prop("checked"));
            //$('#content').val(escape($('.jquery_ckeditor').val()));
            $('#content').val(escape(ckeditor_content.getData()));
            var options = {
                url: '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnCMS/ArticleSave/' + articleId + "?classId=" + classId,
                type: 'POST',
                dataType: 'json',
                beforeSubmit: function () {
                    return $('#editArticleForm').form('validate');
                },
                success: function (r) {
                    if (r.result) {
                        articleId = r.id;
                        $.messager.alert('确认', r.message, 'info');
                    } else {
                        $.messager.alert('错误', '保存失败:' + r.message, 'error');
                    }
                }
            };
            $('#editArticleForm').ajaxForm(options);
            $('#editArticleForm').submit();
        }
    </script>
</asp:Content>
