<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/YnPublic/Views/Shared/Module.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div region="center" title="">
        <div class="easyui-tabs" fit="true" border="false" id="centerTabs">
        <%-- 
			<div title="用户授权" style="padding:2px;overflow:hidden;">
				<iframe id="iframeUser" scrolling="yes" frameborder="0"  src="" style="width:100%;height:100%;"></iframe> 
			</div>--%>
			<div title="角色授权" style="padding:2px;overflow:hidden;">
				<iframe id="iframeRole" scrolling="yes" frameborder="0"  src="" style="width:100%;height:100%;"></iframe> 
			</div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentScriptAndCss" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#centerTabs').tabs({
                onSelect: function (title) {
                    LoadTab();
                }
            });
        })
        function LoadTab(){
			var tab = $('#centerTabs').tabs('getSelected');
            if(tab){
                title=tab.panel('options').title;
				if(title=="用户授权"){
				    if ($('#iframeUser').attr('src') != '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/UserIndex?mi=<%=Request["mi"].ToString() %>') {
				        $('#iframeUser').attr('src', '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/UserIndex?mi=<%=Request["mi"].ToString() %>');
					}
				}else if(title=="角色授权"){
				    if ($('#iframeRole').attr('src') != '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/RoleIndex?mi=<%=Request["mi"].ToString() %>') {
				        $('#iframeRole').attr('src', '<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/YnPublic/YnWebRight/RoleIndex?mi=<%=Request["mi"].ToString() %>');
					}
				}
            }
        }
    </script>
</asp:Content>
