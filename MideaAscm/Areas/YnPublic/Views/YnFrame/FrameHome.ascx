<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<YnFrame.Dal.YnFrameHome>" %>
<%if(Model!=null){ %>
<div title="<%=Model.title %>" style="padding: 2px; overflow: hidden;">
    <iframe id="frameHome" scrolling="auto" frameborder="0" src="" style="width:100%;height:100%;"></iframe>
</div>
<script language="javascript" type="text/javascript">
    $(function () {
        $("#frameHome").attr("src", "<%=Model.url %>");
    });
</script>
<%} %>