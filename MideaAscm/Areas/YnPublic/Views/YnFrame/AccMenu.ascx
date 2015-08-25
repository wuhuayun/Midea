<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<YnFrame.Dal.Entities.YnWebAccMenu>>" %>
<% if(Model!=null){ %>
<% foreach (var node in Model){ %>
   <%if(node!=null){%>
        <div title="<%=node.name%>" style="padding:10px;">
	        <%=node.accMenuTree %>
        </div>
   <%}%>
<% } %>
<% } %>
