using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using YnFrame.Dal.Entities;
using YnFrame.Services;
using YnFrame.Web;

namespace MideaAscm.Code
{
    public class YnWebRight
    {
        public int moduleId { get; set; }
        public bool rightManage { get; set; }
        public bool rightAdd { get; set; }
        public bool rightEdit { get; set; }
        public bool rightDelete { get; set; }
        public bool rightVerify { get; set; }
    }
    public class YnPermission
    {
        private static YnPermission ynPermission;
        public static YnPermission GetInstance()
        {
            //return ynPermission ?? new YnPermission();
            if (ynPermission == null)
                ynPermission = new YnPermission();
            return ynPermission;
        }
        public void AccMenu()
        {
            List<YnWebAccMenu> listYnWebAccMenu = null;
            try
            {
                YnUser ynUser = FormsAuthenticationService.GetInstance().GetTicketUserData();
                //ynUser = YnFrame.Services.YnUserService.GetInstance().Get(ynUser.userId);
                if (ynUser != null)
                {
                    listYnWebAccMenu = YnPermission.GetInstance().GetYnWebAccMenuList(ynUser);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        public List<YnWebAccMenu> GetYnWebAccMenuList(YnUser ynUser)
        {
            List<YnWebAccMenu> listYnWebAccMenu = null;
            if (ynUser != null)
            {
                List<YnWebAccMenuTree> listYnWebAccMenuTree = GetYnWebAccMenuTreeList(ynUser);
                if (listYnWebAccMenuTree != null && listYnWebAccMenuTree.Count > 0)
                {
                    listYnWebAccMenu = new List<YnWebAccMenu>();
                    IEnumerable<IGrouping<YnWebAccMenu, YnWebAccMenuTree>> result = listYnWebAccMenuTree.GroupBy(P => P.ynWebAccMenu);
                    foreach (IGrouping<YnWebAccMenu, YnWebAccMenuTree> ig in result)
                    {
                        YnWebAccMenu ynWebAccMenu = ig.Key;
                        StringBuilder sb = new StringBuilder();
                        int count = ig.Count();
                        if (count > 0)
                        {
                            int parentId = ig.Min(P => P.parentId);
                            List<YnWebAccMenuTree> listAccMenuTree = ig.ToList();
                            sb.Append("<ul id=\"" + parentId + "\" class=\"easyui-tree\" animate=\"true\">");// dnd=\"true\"
                            List<YnWebAccMenuTree> parentList = listAccMenuTree.Where(P => P.parentId == parentId).ToList();
                            foreach (YnWebAccMenuTree ynWebAccMenuTree in parentList)
                            {
                                string url = string.Empty;
                                if (ynWebAccMenuTree.ynWebModule != null && ynWebAccMenuTree.ynWebModule.url != null && !string.IsNullOrEmpty(ynWebAccMenuTree.ynWebModule.url.Trim()))
                                {
                                    url = ynWebAccMenuTree.ynWebModule.url.Trim();
                                    url += "?mi=" + ynWebAccMenuTree.ynWebModule.id;
                                    if (!string.IsNullOrEmpty(ynWebAccMenuTree.ynWebModule.parameter))
                                    {
                                        url += "&parameter=" + System.Web.HttpContext.Current.Server.UrlEncode(ynWebAccMenuTree.ynWebModule.parameter);
                                    }
                                }
                                string onclick = "";
                                if (!string.IsNullOrEmpty(url))
                                    onclick = "onclick=\"openTab('" + ynWebAccMenuTree.name + "','" + url.Trim() + "')\"";

                                sb.Append("<li iconCls=\"" + ynWebAccMenuTree.iconCls + "\">");
                                sb.Append("<span><a href=\"javascript:void(0);\" " + onclick + ">" + ynWebAccMenuTree.name + "</a></span>");
                                List<YnWebAccMenuTree> childList = listAccMenuTree.Where(P => P.parentId == ynWebAccMenuTree.id).ToList();
                                if (childList.Count > 0)
                                {
                                    GetAccMenuTree(ynWebAccMenuTree.id, listAccMenuTree, sb);
                                }
                                sb.Append("</li>");
                            }
                            sb.Append("</ul>");
                        }
                        ynWebAccMenu.accMenuTree = sb.ToString();
                        listYnWebAccMenu.Add(ynWebAccMenu);
                    }
                }
            }
            if (listYnWebAccMenu != null)
            {
                //排序
                YnBaseClass2.YnUc.SortableBindingList<YnWebAccMenu> sortableBindingLis1 = new YnBaseClass2.YnUc.SortableBindingList<YnWebAccMenu>(listYnWebAccMenu);
                sortableBindingLis1.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                sortableBindingLis1.DefaultSortItem = "sortNo";
                //return listYnWebAccMenu;
                return sortableBindingLis1.ToList();
            }
            return null;
        }
        public void GetAccMenuTree(int parentId, List<YnWebAccMenuTree> listAccMenuTree, StringBuilder sb)
        {
            List<YnWebAccMenuTree> parentList = listAccMenuTree.Where(P => P.parentId == parentId).ToList();
            sb.Append("<ul>");
            foreach (YnWebAccMenuTree ynWebAccMenuTree in parentList)
            {
                string url = string.Empty;
                if (ynWebAccMenuTree.ynWebModule != null && ynWebAccMenuTree.ynWebModule.url != null && !string.IsNullOrEmpty(ynWebAccMenuTree.ynWebModule.url.Trim()))
                {
                    url = ynWebAccMenuTree.ynWebModule.url.Trim();
                    url += "?mi=" + ynWebAccMenuTree.ynWebModule.id;
                    if (!string.IsNullOrEmpty(ynWebAccMenuTree.ynWebModule.parameter))
                    {
                        url += "&parameter=" + System.Web.HttpContext.Current.Server.UrlEncode(ynWebAccMenuTree.ynWebModule.parameter);
                    }
                }
                string onclick = "";
                if (!string.IsNullOrEmpty(url))
                    onclick = "onclick=\"openTab('" + ynWebAccMenuTree.name + "','" + url.Trim() + "')\"";

                sb.Append("<li iconCls=\"" + ynWebAccMenuTree.iconCls + "\">");
                sb.Append("<span><a href=\"javascript:void(0);\" " + onclick + ">" + ynWebAccMenuTree.name + "</a></span>");
                List<YnWebAccMenuTree> childList = listAccMenuTree.Where(P => P.parentId == ynWebAccMenuTree.id).ToList();
                if (childList.Count > 0)
                {
                    GetAccMenuTree(ynWebAccMenuTree.id, listAccMenuTree, sb);
                }
                sb.Append("</li>");
            }
            sb.Append("</ul>");
        }

        public string GetRoleIds(string userId)
        {
            string roleIds = string.Empty;
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    List<YnRole> listYnRole = YnRoleService.GetInstance().GetListInUser(userId);
                    if (listYnRole != null && listYnRole.Count > 0)
                    {
                        foreach (YnRole ynRole in listYnRole)
                        {
                            if (!string.IsNullOrEmpty(roleIds))
                            {
                                roleIds += ",";
                            }
                            roleIds += ynRole.id;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return roleIds;
        }
        public List<YnWebAccMenuTree> GetYnWebAccMenuTreeList_Old(string userId)
        {
            List<YnWebAccMenuTree> listYnWebAccMenuTree = null;
            if (!string.IsNullOrEmpty(userId))
            {
                if (YnUserService.GetInstance().IsSuperAdministrator(userId) || YnUserService.GetInstance().IsAdministrator(userId))
                {
                    try
                    {
                        listYnWebAccMenuTree = YnWebAccMenuTreeService.GetInstance().GetList(string.Empty, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    #region 非管理员
                    List<int> listModuleId = null;
                    List<YnWebModule> listYnWebModuleByUser = GetYnWebModuleListByUser(userId);
                    if (listYnWebModuleByUser != null)
                    {
                        listModuleId = new List<int>();
                        foreach (YnWebModule ynWebModule in listYnWebModuleByUser)
                        {
                            listModuleId.Add(ynWebModule.id);
                        }
                    }
                    string roleIds = GetRoleIds(userId);
                    List<YnWebModule> listYnWebModuleByRole = GetYnWebModuleListByRole(roleIds);
                    if (listYnWebModuleByRole != null)
                    {
                        listModuleId = listModuleId ?? new List<int>();
                        foreach (YnWebModule ynWebModule in listYnWebModuleByRole)
                        {
                            if (!listModuleId.Contains(ynWebModule.id))
                            {
                                listModuleId.Add(ynWebModule.id);
                            }
                        }
                    }
                    if (listModuleId != null && listModuleId.Count > 0)
                    {
                        string moduleIds = string.Empty;
                        foreach (int moduleId in listModuleId)
                        {
                            if (!string.IsNullOrEmpty(moduleIds))
                            {
                                moduleIds += ",";
                            }
                            moduleIds += moduleId.ToString();
                        }
                        try
                        {
                            listYnWebAccMenuTree = YnWebAccMenuTreeService.GetInstance().GetListByModule(moduleIds);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #endregion
                }
            }
            return listYnWebAccMenuTree;
        }
        public List<YnWebAccMenuTree> GetYnWebAccMenuTreeList(YnUser ynUser)
        {
            try
            {
                List<YnWebAccMenuTree> listYnWebAccMenuTree = null;
                if (YnUserService.GetInstance().IsSuperAdministrator(ynUser.userId) || YnUserService.GetInstance().IsAdministrator(ynUser.userId))
                {
                    listYnWebAccMenuTree = YnWebAccMenuTreeService.GetInstance().GetList(string.Empty, string.Empty, string.Empty);
                }
                else
                {
                    listYnWebAccMenuTree = YnWebAccMenuTreeService.GetInstance().GetListByUser(ynUser);
                }
                return listYnWebAccMenuTree;
                /*if(listYnWebAccMenuTree!=null){
				//根据tree获取accmenu
				List<YnWebAccMenu> listYnWebAccMenu = new List<YnWebAccMenu>();
				foreach(YnWebAccMenuTree ynWebAccMenuTree in listYnWebAccMenuTree){
					if(ynWebAccMenuTree.getYnWebAccMenu()!=null){
						YnWebAccMenu ynWebAccMenu=null;
						for(YnWebAccMenu _ynWebAccMenu :listYnWebAccMenu){
							if(ynWebAccMenuTree.getYnWebAccMenu().getId().equals(_ynWebAccMenu.getId())){
								ynWebAccMenu=_ynWebAccMenu;
								break;
							}
						}
						if(ynWebAccMenu==null){
							listYnWebAccMenu.add(ynWebAccMenuTree.getYnWebAccMenu());
						}
					}
				}
				GenerationMenu( listYnWebAccMenu, listYnWebAccMenuTree);*/
                /*//根据accmenu生成菜单
                for(YnWebAccMenu _ynWebAccMenu :listYnWebAccMenu){
                    if(ynWebAccMenuTree.getYnWebAccMenu().getId()==_ynWebAccMenu.getId()){
                        ynWebAccMenu=_ynWebAccMenu;
                        break;
                    }
                }*/
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public List<YnWebModule> GetYnWebModuleListByUser(string userId)
        {
            List<YnWebModule> listYnWebModule = null;
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    List<YnWebModuleUserLink> listYnWebModuleUserLink = YnWebModuleUserLinkService.GetInstance().GetListByUser(userId);
                    if (listYnWebModuleUserLink != null && listYnWebModuleUserLink.Count > 0)
                    {
                        listYnWebModule = listYnWebModuleUserLink.Select<YnWebModuleUserLink, YnWebModule>(P => P.ynWebModule).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return listYnWebModule;
        }
        public List<YnWebModule> GetYnWebModuleListByRole(string roleIds)
        {
            List<YnWebModule> listYnWebModule = null;
            if (!string.IsNullOrEmpty(roleIds))
            {
                try
                {
                    List<YnWebModuleRoleLink> listYnWebModuleRoleLink = YnWebModuleRoleLinkService.GetInstance().GetListByRoles(roleIds);
                    if (listYnWebModuleRoleLink != null && listYnWebModuleRoleLink.Count > 0)
                    {
                        listYnWebModule = listYnWebModuleRoleLink.Select<YnWebModuleRoleLink, YnWebModule>(P => P.ynWebModule).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return listYnWebModule;
        }

        public void SaveToCache(YnUser ynUser, int moduleId)
        {
            if (ynUser != null)
            {
                try
                {
                    bool isSaveToCache = false;
                    YnWebRight ynWebRight = new YnWebRight();
                    ynWebRight.moduleId = moduleId;
                    if (YnUserService.GetInstance().IsSuperAdministrator(ynUser.userId) || YnUserService.GetInstance().IsAdministrator(ynUser.userId))
                    {
                        isSaveToCache = true;
                        ynWebRight.rightManage = true;
                        ynWebRight.rightAdd = true;
                        ynWebRight.rightEdit = true;
                        ynWebRight.rightDelete = true;
                        ynWebRight.rightVerify = true;
                    }
                    else
                    {
                        YnWebModuleUserLink ynWebModuleUserLink = YnWebModuleUserLinkService.GetInstance().Get(ynUser.userId, moduleId);
                        if (ynWebModuleUserLink != null)
                        {
                            isSaveToCache = true;
                            ynWebRight.rightManage = ynWebModuleUserLink.rightManage;
                            ynWebRight.rightAdd = ynWebRight.rightManage || ynWebModuleUserLink.rightAdd;
                            ynWebRight.rightEdit = ynWebRight.rightManage || ynWebModuleUserLink.rightEdit;
                            ynWebRight.rightDelete = ynWebRight.rightManage || ynWebModuleUserLink.rightDelete;
                            ynWebRight.rightVerify = ynWebModuleUserLink.rightVerify;// ynWebRight.rightManage || 
                        }
                        string roleIds = GetRoleIds(ynUser.userId);
                        if (!string.IsNullOrEmpty(roleIds))
                        {
                            List<YnWebModuleRoleLink> listYnWebModuleRoleLink = YnWebModuleRoleLinkService.GetInstance().GetList(roleIds, moduleId);
                            if (listYnWebModuleRoleLink != null && listYnWebModuleRoleLink.Count > 0)
                            {
                                isSaveToCache = true;
                                foreach (YnWebModuleRoleLink ynWebModuleRoleLink in listYnWebModuleRoleLink)
                                {
                                    ynWebRight.rightManage = ynWebModuleRoleLink.rightManage || ynWebRight.rightManage;
                                    ynWebRight.rightAdd = (ynWebRight.rightManage || ynWebModuleRoleLink.rightAdd) || ynWebRight.rightAdd;
                                    ynWebRight.rightEdit = (ynWebRight.rightManage || ynWebModuleRoleLink.rightEdit) || ynWebRight.rightEdit;
                                    ynWebRight.rightDelete = (ynWebRight.rightManage || ynWebModuleRoleLink.rightDelete) || ynWebRight.rightDelete;
                                    ynWebRight.rightVerify = (ynWebModuleRoleLink.rightVerify) || ynWebRight.rightVerify;//ynWebRight.rightManage || 
                                }
                            }
                        }
                    }
                    if (isSaveToCache)
                        //SaveToCache(ynUser.userId, ynWebRight, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);    
                        SaveToCache(ynUser.userId + "_" + moduleId.ToString(), ynWebRight);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public YnWebRight GetYnWebRight()
        {
            YnWebRight ynWebRight = new YnWebRight();
            //string mi = System.Web.HttpContext.Request["mi"];
            //System.Web.HttpContext.Current.Server.UrlEncode(ynWebAccMenuTree.ynWebModule.parameter);
            string mi = System.Web.HttpContext.Current.Request["mi"];

            YnFrame.Dal.Entities.YnUser ynUser = FormsAuthenticationService.GetInstance().GetTicketUserData();
            //ynUser = YnFrame.Services.YnUserService.GetInstance().Get(ynUser.userId);
            if (ynUser != null && mi != null)
            {
                ynWebRight = GetYnWebRight(ynUser.userId, mi);
            }
            return ynWebRight;
        }
        public YnWebRight GetYnWebRight(string userId, string mi)
        {
            YnWebRight ynWebRight = new YnWebRight();
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(mi))
            {
                object cacheObject = GetCacheObject(userId + "_" + mi);
                if (cacheObject != null)
                {
                    ynWebRight = (YnWebRight)cacheObject;
                }
            }
            return ynWebRight;
        }

        public void SaveToCache(string key, object obj)
        {
            try
            {
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                cache.Insert(key, obj);
            }
            catch
            {
                throw new Exception();
            }
        }
        //public void SaveToCache(string key, object obj, System.Web.Caching.CacheDependency dependency, DateTime dateTime, TimeSpan timeSpan)
        //{
        //    try
        //    {
        //        System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
        //        cache.Insert(key, obj, dependency, dateTime, timeSpan, System.Web.Caching.CacheItemPriority.NotRemovable, new System.Web.Caching.CacheItemRemovedCallback(RemoveCacheObject));
        //    }
        //    catch
        //    {
        //        throw new Exception();
        //    }
        //}
        public object GetCacheObject(string key)
        {
            object objReturn = null;
            try
            {
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                objReturn = cache[key];
            }
            catch
            {
                throw new Exception();
            }
            return objReturn;
        }
        public void RemoveCacheObject(string key)
        {
            try
            {
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                if (cache[key] != null)
                    cache.Remove(key);
            }
            catch
            {
                throw new Exception();
            }
        }
        //public void RemoveCacheObject(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        //{
        //    try
        //    {
        //        System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
        //        if (cache[key] != null)
        //            cache.Remove(key);
        //    }
        //    catch
        //    {
        //        throw new Exception();
        //    }
        //}
    }
}