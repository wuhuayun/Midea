using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using YnFrame.Dal.Entities;
using YnFrame.Services;
using YnFrame.Services.Form;
using YnFrame.Dal.Form.Entities;
using YnFrame.Dal;

namespace YnWS
{
    /// <summary>
    /// YnWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class YnWebService// : System.Web.Services.WebService
    {

        #region 变量
        #endregion

        #region 验证
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        public bool IsTicketValid(string decryptTicket)
        {
            try
            {
                if (decryptTicket != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(decryptTicket);
                    if (ticket != null)
                    {
                        string sUserData = ticket.UserData;
                        //userDataTicket = Newtonsoft.Json.JsonConvert.DeserializeObject<YnUser>(sUserData);
                        YnUserTicket ynUserTicket = Newtonsoft.Json.JsonConvert.DeserializeObject<YnUserTicket>(sUserData);
                        if (ynUserTicket != null)
                        {
                            return true;
                            //ynUser = ynUserDataTicket.ynUser;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        [WebMethod()]
        public bool UserAuthentication(string userId, string userPwd, string hostIP, ref string message, ref string encryptTicket)
        {
            try
            {
                message = "";

                string sql = "from YnUser where userId='" + userId + "' or userName ='" + userId + "'";
                IList<YnUser> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sql);
                if (ilist == null || ilist.Count() == 0)
                {
                    return false;
                }
                YnFrame.Dal.Entities.YnUser ynUser = ilist[0];
                if (YnFrame.Services.YnUserService.GetInstance().ValidateUser(ynUser.userId, userPwd))
                {
                    //YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Services.YnUserService.GetInstance().Get(userId);

                    string sUserData = Newtonsoft.Json.JsonConvert.SerializeObject(ynUser.GetTicket());
                    bool createPersistentCookie = false;
                    DateTime dt = createPersistentCookie ? DateTime.Now.AddMinutes(99999) : DateTime.Now.AddDays(365);//用一年看他还过不过期
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                                                                        1, // 票据版本号
                                                                                        userId.ToString(), // 票据持有者
                                                                                        DateTime.Now, //分配票据的时间
                                                                                        dt, // 失效时间
                                                                                        createPersistentCookie, // 需要用户的 cookie 
                                                                                        sUserData, // 用户数据，这里其实就是用户的角色
                                                                                        FormsAuthentication.FormsCookiePath);//cookie有效路径

                    //System.Web.Security.FormsAuthenticationTicket ticket = YnFrame.Web.FormsAuthenticationService.GetInstance().GetTicket(userId, 0, ynUser, false);
                    string hash = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, hash); //加密之后的cookie
                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }
                    encryptTicket = hash;
                    //添加cookie到页面请求响应中
                    HttpContext.Current.Response.Cookies.Add(cookie);


                    //写日志
                    ynUser.lastLoginIp = hostIP;
                    ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    YnUserService.GetInstance().Update(ynUser);

                    return true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        #endregion

        #region YnFrame_Base
        [WebMethod()]
        public long GetCount(string ticket, string sql, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                long iCount = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                return iCount;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return -1;
        }
        [WebMethod()]
        public long GetMaxId(string ticket, string sql, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                long maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetCount(sql);
                return maxId;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return -1;
        }
        [WebMethod()]
        public object GetObject(string ticket, string sql, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                object object1 = YnDaoHelper.GetInstance().nHibernateHelper.GetObject(sql);
                return object1;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return -1;
        }
        #endregion

        #region YnFrame_User
        [WebMethod()]
        public string GetUserBaseInfo(string userId, ref string message)
        {
            try
            {
                message = "";

                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Services.YnUserService.GetInstance().Get(userId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynUser.GetTicket());
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetUser(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnFrame.Dal.Entities.YnUser ynUser = YnFrame.Services.YnUserService.GetInstance().Get(userId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynUser);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetUserList(string ticket, string _ynPage, string whereOther, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");


                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                List<YnUser> listYnUser = listYnUser = YnUserService.GetInstance().GetList("", ynPage, "", "", "", whereOther);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnUser);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool SaveUser(string ticket, string _ynUser, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnUser ynUser = (YnUser)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnUser), _ynUser);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnUserService.GetInstance().Save(ynUser);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateUser(string ticket, string _ynUser, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnUser ynUser = (YnUser)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnUser), _ynUser);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnUserService.GetInstance().Update(ynUser);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeleteUser(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnUserService.GetInstance().Delete(userId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public bool IsAdministrator(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                return YnUserService.GetInstance().IsAdministrator(userId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public string GetUserListNotInModuleFunction(string ticket, int functionId, string whereOther, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnUser> listYnUser = YnUserService.GetInstance().GetListNotInModuleFunction(functionId, whereOther);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnUser);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetUserRoleLinkListByRoleId(string ticket, int roleId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnUserRoleLink> listYnUserRoleLink = YnUserRoleLinkService.GetInstance().GetListByRoleId(roleId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnUserRoleLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetUserRoleLinkListByUserId(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnUserRoleLink> listYnUserRoleLink = YnUserRoleLinkService.GetInstance().GetListByUserId(userId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnUserRoleLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region YnFrame_Role
        [WebMethod()]
        public string GetRoleListNotInModuleFunction(string ticket, int functionId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnRole> listYnRole = YnRoleService.GetInstance().GetListNotInModuleFunction(functionId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnRole);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetRoleList(string ticket,ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnRole> listYnRole = listYnRole = YnRoleService.GetInstance().GetList( "", "", "");
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnRole);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string SaveRole(string ticket, string _ynRole, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnRole ynRole = (YnRole)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnRole), _ynRole);

                YnRoleService.GetInstance().Save(ynRole);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynRole);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool UpdateRole(string ticket, string _ynRole, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnRole ynRole = (YnRole)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnRole), _ynRole);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnRoleService.GetInstance().Update(ynRole);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeleteRole(string ticket, int roleId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnRoleService.GetInstance().Delete(roleId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public string GetRoleListInUser(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnRole);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        #endregion

        #region YnFrame_Position
        [WebMethod()]
        public string GetPositionList(string ticket, string _ynPage, string whereOther, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");


                YnBaseDal.YnPage ynPage = (YnBaseDal.YnPage)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnBaseDal.YnPage), _ynPage);

                List<YnPosition> listYnPosition = listYnPosition = YnPositionService.GetInstance().GetList("", ynPage, "", "", "", whereOther);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnPosition);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string SavePosition(string ticket, string _ynPosition, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnPosition ynPosition = (YnPosition)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnPosition), _ynPosition);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnPositionService.GetInstance().Save(ynPosition);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynPosition);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool UpdatePosition(string ticket, string _ynPosition, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnPosition ynPosition = (YnPosition)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnPosition), _ynPosition);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnPositionService.GetInstance().Update(ynPosition);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeletePosition(string ticket, int positionId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnPositionService.GetInstance().Delete(positionId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region YnFrame_Department
        [WebMethod()]
        public string GetDepartmentList(string ticket, int parentId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnDepartment> listYnDepartment = YnDepartmentService.GetInstance().GetList("", parentId, "", "");
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnDepartment);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool SaveDepartment(string ticket, string _ynDepartment, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnDepartment ynDepartment = (YnDepartment)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnDepartment), _ynDepartment);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnDepartmentService.GetInstance().Save(ynDepartment);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateDepartment(string ticket, string _ynDepartment, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnDepartment ynDepartment = (YnDepartment)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnDepartment), _ynDepartment);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnDepartmentService.GetInstance().Update(ynDepartment);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeleteDepartment(string ticket, int departmentId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnDepartmentService.GetInstance().Delete(departmentId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public string GetDepartmentPositionLink(string ticket, int departmentId, int positionId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnDepartmentPositionLinkPK ynDepartmentPositionLinkPK = new YnDepartmentPositionLinkPK();
                ynDepartmentPositionLinkPK.departmentId = departmentId;
                ynDepartmentPositionLinkPK.positionId = positionId;

                YnDepartmentPositionLink ynDepartmentPositionLink = YnDepartmentPositionLinkService.GetInstance().Get(ynDepartmentPositionLinkPK);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynDepartmentPositionLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void SaveDepartmentPositionLinkList(string ticket, string _listYnDepartmentPositionLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                List<YnDepartmentPositionLink> listYnDepartmentPositionLink = (List<YnDepartmentPositionLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnDepartmentPositionLink>), _listYnDepartmentPositionLink);

                YnDepartmentPositionLinkService.GetInstance().Save(listYnDepartmentPositionLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public void DeleteDepartmentPositionLinkList(string ticket, string _listYnDepartmentPositionLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                List<YnDepartmentPositionLink> listYnDepartmentPositionLink = (List<YnDepartmentPositionLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnDepartmentPositionLink>), _listYnDepartmentPositionLink);

                YnDepartmentPositionLinkService.GetInstance().DeleteList(listYnDepartmentPositionLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region YnFrame_Menu
        [WebMethod()]
        public string GetFormMainMenuList(string ticket, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormMainMenu> listYnFormMainMenu = YnFormMainMenuService.GetInstance().GetList();
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormMainMenu);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormSubMenu(string ticket, int subMenuId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnFormSubMenu ynFormSubMenu = YnFormSubMenuService.GetInstance().Get(subMenuId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynFormSubMenu);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormSubMenuList(string ticket, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormSubMenu> listYnFormSubMenu = YnFormSubMenuService.GetInstance().GetList();
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormSubMenu);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public bool SaveFormMainMenu(string ticket, string _ynFormMainMenu, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnFormMainMenu ynFormMainMenu = (YnFormMainMenu)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnFormMainMenu), _ynFormMainMenu);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormMainMenuService.GetInstance().Save(ynFormMainMenu);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateFormMainMenu(string ticket, string _ynFormMainMenu, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnFormMainMenu ynFormMainMenu = (YnFormMainMenu)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnFormMainMenu), _ynFormMainMenu);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormMainMenuService.GetInstance().Update(ynFormMainMenu);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeleteFormMainMenu(string ticket, int menuId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnFormMainMenuService.GetInstance().Delete(menuId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public bool SaveFormSubMenu(string ticket, string _ynFormSubMenu, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnFormSubMenu ynFormSubMenu = (YnFormSubMenu)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnFormSubMenu), _ynFormSubMenu);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormSubMenuService.GetInstance().Save(ynFormSubMenu);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateFormSubMenu(string ticket, string _ynFormSubMenu, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnFormSubMenu ynFormSubMenu = (YnFormSubMenu)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnFormSubMenu), _ynFormSubMenu);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormSubMenuService.GetInstance().Update(ynFormSubMenu);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public void DeleteFormSubMenu(string ticket, int menuId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnFormSubMenuService.GetInstance().Delete(menuId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region YnFrame_Module
        [WebMethod()]
        public string GetFormModuleFunction(string ticket, int functionId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnFormModuleFunction ynFormModuleFunction = YnFormModuleFunctionService.GetInstance().Get(functionId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynFormModuleFunction);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionList(string ticket, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunction> listYnFormModuleFunction = YnFormModuleFunctionService.GetInstance().GetList();
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormModuleFunction);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionListNotInYnUser(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunction> listYnFormModuleFunction = YnFormModuleFunctionService.GetInstance().GetListNotInYnUser(userId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormModuleFunction);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionListNotInYnRole(string ticket, int roleId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunction> listYnFormModuleFunction = YnFormModuleFunctionService.GetInstance().GetListNotInYnRole(roleId.ToString());
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormModuleFunction);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionUserLinkListByUserId(string ticket, string userId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionUserLink> listYnFormModuleFunctionUserLink = YnFormModuleFunctionUserLinkService.GetInstance().GetListByUserId(userId);
                if (listYnFormModuleFunctionUserLink != null)
                    return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormModuleFunctionUserLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionRoleLinkListByRoleId(string ticket, string _listRoleId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<int> listRoleId = (List<int>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<int>), _listRoleId);
                List<YnFormModuleFunctionRoleLink> listYnFormModuleFunctionRoleLink = YnFormModuleFunctionRoleLinkService.GetInstance().GetListByRoleId(listRoleId);
                if (listYnFormModuleFunctionRoleLink!=null)
                    return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormModuleFunctionRoleLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionUserLink(string ticket, int functionId, string uerId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnFormModuleFunctionUserLinkPK ynFormModuleFunctionUserLinkPK = new YnFormModuleFunctionUserLinkPK();
                ynFormModuleFunctionUserLinkPK.ynFormModuleFunction = new YnFormModuleFunction();
                ynFormModuleFunctionUserLinkPK.ynFormModuleFunction.id = functionId;
                ynFormModuleFunctionUserLinkPK.ynUser = new YnUser();
                ynFormModuleFunctionUserLinkPK.ynUser.userId = uerId;
                YnFormModuleFunctionUserLink ynFormModuleFunctionUserLink = YnFormModuleFunctionUserLinkService.GetInstance().Get(ynFormModuleFunctionUserLinkPK);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(ynFormModuleFunctionUserLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionRoleLinkByModuleFunctionId(string ticket, int functionId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionRoleLink> listYnFormModuleFunctionRoleLink  = YnFormModuleFunctionRoleLinkService.GetInstance().GetListByModuleFunctionId(functionId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormModuleFunctionRoleLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public string GetFormModuleFunctionUserLinkByModuleFunctionId(string ticket, int functionId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionUserLink> listYnFormModuleFunctionUserLink = YnFormModuleFunctionUserLinkService.GetInstance().GetListByModuleFunctionId(functionId);
                return YnBaseClass2.Helper.ObjectHelper.Serialize(listYnFormModuleFunctionUserLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;
        }
        [WebMethod()]
        public void DeleteFormModuleFunction(string ticket, int functionId, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");
                YnFormModuleFunctionService.GetInstance().Delete(functionId);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
        }
        [WebMethod()]
        public bool SaveFormModuleFunction(string ticket, string _ynFormModuleFunction, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnFormModuleFunction ynFormModuleFunction = (YnFormModuleFunction)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnFormModuleFunction), _ynFormModuleFunction);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionService.GetInstance().Save(ynFormModuleFunction);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateFormModuleFunction(string ticket, string _ynFormModuleFunction, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                YnFormModuleFunction ynFormModuleFunction = (YnFormModuleFunction)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(YnFormModuleFunction), _ynFormModuleFunction);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionService.GetInstance().Update(ynFormModuleFunction);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool SaveFormModuleFunctionUserLink(string ticket, string _listYnFormModuleFunctionUserLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionUserLink> listYnFormModuleFunctionUserLink = (List<YnFormModuleFunctionUserLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnFormModuleFunctionUserLink>), _listYnFormModuleFunctionUserLink);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionUserLinkService.GetInstance().Save(listYnFormModuleFunctionUserLink);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool DeleteFormModuleFunctionUserLink(string ticket, string _listYnFormModuleFunctionUserLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionUserLink> listYnFormModuleFunctionUserLink = (List<YnFormModuleFunctionUserLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnFormModuleFunctionUserLink>), _listYnFormModuleFunctionUserLink);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionUserLinkService.GetInstance().Delete(listYnFormModuleFunctionUserLink);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateFormModuleFunctionUserLink(string ticket, string _listYnFormModuleFunctionUserLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionUserLink> listYnFormModuleFunctionUserLink = (List<YnFormModuleFunctionUserLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnFormModuleFunctionUserLink>), _listYnFormModuleFunctionUserLink);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionUserLinkService.GetInstance().Update(listYnFormModuleFunctionUserLink);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool SaveFormModuleFunctionRoleLink(string ticket, string _listYnFormModuleFunctionRoleLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionRoleLink> listYnFormModuleFunctionRoleLink = (List<YnFormModuleFunctionRoleLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnFormModuleFunctionRoleLink>), _listYnFormModuleFunctionRoleLink);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionRoleLinkService.GetInstance().Save(listYnFormModuleFunctionRoleLink);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool DeleteFormModuleFunctionRoleLink(string ticket, string _listYnFormModuleFunctionRoleLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionRoleLink> listYnFormModuleFunctionRoleLink = (List<YnFormModuleFunctionRoleLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnFormModuleFunctionRoleLink>), _listYnFormModuleFunctionRoleLink);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionRoleLinkService.GetInstance().Delete(listYnFormModuleFunctionRoleLink);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        [WebMethod()]
        public bool UpdateFormModuleFunctionRoleLink(string ticket, string _listYnFormModuleFunctionRoleLink, ref string message)
        {
            try
            {
                message = "";
                if (!IsTicketValid(ticket))
                    throw new Exception("票证验证失败！");

                List<YnFormModuleFunctionRoleLink> listYnFormModuleFunctionRoleLink = (List<YnFormModuleFunctionRoleLink>)YnBaseClass2.Helper.ObjectHelper.Deserialize(typeof(List<YnFormModuleFunctionRoleLink>), _listYnFormModuleFunctionRoleLink);
                //ynUser.lastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                YnFormModuleFunctionRoleLinkService.GetInstance().Update(listYnFormModuleFunctionRoleLink);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;
        }
        #endregion
    }
}
