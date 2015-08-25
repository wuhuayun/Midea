<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>系统登录</title>
	<style type="text/css">
	    body {background: #f9fee8;margin: 0; padding: 20px; text-align:center; font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#666666;}
	    .error_page {width: 600px; padding: 50px; margin: auto;}
	    .error_page h1 {margin: 20px 0 0;}
	    .error_page p {margin: 10px 0; padding: 0;}		
	    a {color: #9caa6d; text-decoration:none;}
	    a:hover {color: #9caa6d; text-decoration:underline;}
	</style>
</head>
<body class="login">
  <div class="error_page">
    <img alt="登录失败" src="<%= Request.ApplicationPath=="/"?"":Request.ApplicationPath %>/Content/Images/logfail.gif" />
    <h1>提示...</h1>
    <h3 style="color:red;">登录失败</h3>
    <h3 style="color:red;"><%: ViewData["ErrorMessage"] %></h3>
    <!--<p><a href="http://kidmondo.com">Return to the homepage</a></p>-->
  </div>
</body>
</html>
