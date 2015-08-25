using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MideaAscm.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "欢迎使用 ASP.NET MVC!";

            string usercode = HttpUtility.UrlDecode(Request.QueryString["usercode"]);
            string userpwd = HttpUtility.UrlDecode(Request.QueryString["userpwd"]);
            string url = "/Account/LogOn";
            if (!string.IsNullOrEmpty(usercode) && !string.IsNullOrEmpty(userpwd)) 
            {
                url = string.Format(url + "?usercode={0}&userpwd={1}", HttpUtility.UrlEncode(usercode), HttpUtility.UrlEncode(userpwd));
            } 
            return Redirect(url);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
