using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
//using MideaAscm.Models;
using YnFrame.Dal.Entities;
using YnFrame.Services;
using YnFrame.Web;
using YnFrame.Models;
using YnFrame.Controllers;
using MideaAscm.Services.Base;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Controllers
{

    [HandleError]
    [YnActionFilter(false)]
    public class AccountController : YnBaseController
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }
        public ActionResult ViewPage1() {
            return View();

        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************       

        public ActionResult LogOn()
        {
            System.Web.HttpContext.Current.Application.Clear();

            LogOnModel model = new LogOnModel();
            #region 暂屏蔽单点登录
            //string usercode = HttpUtility.UrlDecode(Request.QueryString["UserName"]);
            //string userpwd = HttpUtility.UrlDecode(Request.QueryString["Password"]);
            //if (!string.IsNullOrEmpty(usercode) && !string.IsNullOrEmpty(userpwd))
            //{
            //    string errorMsg = string.Empty;
            //    try
            //    {
            //        YnUser ynUser = AscmUserInfoService.GetInstance().TryGet(usercode);
            //        if (ynUser != null)
            //        {
            //            if (ynUser.password == FormsAuthentication.HashPasswordForStoringInConfigFile(userpwd, "MD5"))
            //            {
            //                if (!string.IsNullOrEmpty(ynUser.extExpandType) && ynUser.extExpandType.Trim() == "erp")
            //                {
            //                    string _userName = HttpUtility.UrlEncode(model.UserName);
            //                    Response.Cookies["userName"].Value = _userName;
            //                    Response.Cookies["userName"].Expires = DateTime.MaxValue;

            //                    int days = 0;
            //                    int iExpires = 0;
            //                    if (days > 0)
            //                        iExpires = 60 * 60 * 24 * days;//保存天数

            //                    FormsAuthenticationService.GetInstance().SignIn(ynUser.userId, 0, ynUser, true);
            //                    PageInit(null);
            //                    this.ynSite.systemUser = true;//是系统用户
            //                    this.ynSite.siteAdmin = YnUserService.GetInstance().IsSuperAdministrator(ynUser.userId) || YnUserService.GetInstance().IsAdministrator(ynUser.userId);
            //                    this.ynSite.userName = ynUser.userName;//是系统用户
            //                    return RedirectToAction("YnFrame", "YnPublic");
            //                }
            //                else
            //                {
            //                    errorMsg = "账号异常";
            //                }
            //            }
            //            else
            //            {
            //                errorMsg = "密码错误";
            //            }
            //        }
            //        else
            //        {
            //            errorMsg = "用户不存在"; 
            //        }
            //    }
            //    catch
            //    {
            //        errorMsg = "账号异常";
            //    }
            //    if (string.IsNullOrEmpty(errorMsg))
            //        errorMsg = "账号异常";
            //    RouteValueDictionary rvd = new RouteValueDictionary();
            //    rvd.Add("errorMsg", errorMsg);
            //    return RedirectToAction("LogFail", rvd);
            //}
            //else
            //{
            //    HttpCookie cookie = Request.Cookies.Get("userName");
            //    if (cookie != null)
            //    {
            //        model.UserName = HttpUtility.UrlDecode(cookie.Value);
            //    }
            //}
            #endregion
            HttpCookie cookie = Request.Cookies.Get("userName");
            if (cookie != null)
            {
                model.UserName = HttpUtility.UrlDecode(cookie.Value);
            }
            return View(model);
        }

        [HttpPost]
        /*[AcceptVerbs(HttpVerbs.Post)]*/
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    YnUser ynUser = AscmUserInfoService.GetInstance().TryGet(model.UserName);
                    /*
                    YnUser ynUser = YnUserService.GetInstance().Get(model.UserName);
                    if (ynUser == null)
                    {
                        //throw new Exception("用户不存在");
                        //判断是否erp用户
                        ynUser = YnUserService.GetInstance().Get("erp_"+model.UserName);
                        if (ynUser == null)
                        {
                            //判断是否mes用户
                        }
                    }
                    */
                    if (ynUser == null)
                    {
                        throw new Exception("用户不存在");
                    }
                    if (!string.IsNullOrEmpty(ynUser.extExpandType))
                    {
                        if (ynUser.extExpandType.Trim() == "erp")
                        {
                            //供应商登录显示供应商名称
                            AscmUserInfo ascmUserInfo = (AscmUserInfo)ynUser;
                            AscmUserInfoService.GetInstance().SetSupplier(ascmUserInfo);
                            if (ascmUserInfo.ascmSupplier != null)
                                ascmUserInfo.userName1 = ascmUserInfo.ascmSupplier.name;

                            if (!MembershipService.ValidateUser(ynUser.userId, model.Password))
                            {
                                if (Request.IsAjaxRequest())
                                {
                                    return Content("用户密码错误！");
                                }
                                //ModelState.AddModelError("", "提供的用户名或密码不正确。");
                                throw new Exception("提供的用户名或密码不正确。");
                            }
                        }
                        else if (ynUser.extExpandType.Trim() == "mes")
                        {
                            cn.com.midea.mespda.TransferService service = new cn.com.midea.mespda.TransferService();
                            cn.com.midea.mespda.OutputWebMessage message = service.UserLogin(model.UserName, model.Password);
                            if (!message.IsSuccess)
                            {
                                throw new Exception(message.ErrorMessage);
                            }
                        }
                    }
                    else
                    {
                        if (!MembershipService.ValidateUser(ynUser.userId, model.Password))
                        {
                            if (Request.IsAjaxRequest())
                            {
                                return Content("用户密码错误！");
                            }
                            //ModelState.AddModelError("", "提供的用户名或密码不正确。");
                            throw new Exception("提供的用户名或密码不正确。");
                        }
                    }

                    //HttpCookie cookie = Request.Cookies.Get("userName");
                    //Response.Cookies["userName"].Value = model.UserName;
                    string _userName = HttpUtility.UrlEncode(model.UserName);
                    Response.Cookies["userName"].Value = _userName;
                    Response.Cookies["userName"].Expires = DateTime.MaxValue;

                    int days = 0;
                    int iExpires = 0;
                    if (days > 0)
                        iExpires = 60 * 60 * 24 * days;//保存天数

                    //YnUser ynUser = YnUserService.GetInstance().Get(model.UserName);
                    FormsAuthenticationService.GetInstance().SignIn(ynUser.userId, 0, ynUser, true);
                    PageInit(null);
                    //this.ynSite.systemUser = true;//是系统用户
                    //this.ynSite.siteAdmin = YnUserService.GetInstance().IsSuperAdministrator(ynUser.userId) || YnUserService.GetInstance().IsAdministrator(ynUser.userId);
                    //this.ynSite.userName = ynUser.userName;//是系统用户
                    if (Request.IsAjaxRequest())
                    {
                        return JavaScript("document.location.href='';");
                    }
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        //return RedirectToAction("Index", "Home");
                        return RedirectToAction("YnFrame", "YnPublic");
                    }

                }
                catch (Exception ex)
                {
                    //throw ex;
                    ModelState.AddModelError("", ex.Message);
                }

            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            System.Web.HttpContext.Current.Application.Clear();

            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    //FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult LogFail(string errorMsg)
        {
            ViewData["ErrorMessage"] = errorMsg;
            return View();
        }
    }
}
