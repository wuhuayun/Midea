using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using YnFrame.Dal.Entities;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Services.Base;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmForkliftService
    {
        private static AscmForkliftService service;
        public static AscmForkliftService GetInstance()
        {
            if (service == null)
                service = new AscmForkliftService();
            return service;
        }

        public AscmForklift Get(int id)
        {
            AscmForklift ascmForklift = null;
            try
            {
                ascmForklift = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmForklift>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmForklift)",ex);
                throw ex;
            }
            return ascmForklift;
        }

        public List<AscmForklift> GetList(string sql,bool isSetWorker)
        {
            List<AscmForklift> list = null;
            try
            {
                IList<AscmForklift> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmForklift>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmForklift>(ilist);
                    if (isSetWorker)
                        SetWorker(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmForklift)",ex);
                throw ex;
            }
            return list;
        }
        public List<AscmForklift> GetList(string sql,string sessionKey)
        {
            List<AscmForklift> list = null;
            try
            {
                IList<AscmForklift> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmForklift>(sql, sessionKey);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmForklift>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmForklift)", ex);
                throw ex;
            }
            return list;
        }
        //public List<AscmForklift> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string queryOtherWord)
        //{
        //    List<AscmForklift> list = null;
        //    try
        //    {
        //        string sort = " order by id ";
        //        if (!string.IsNullOrEmpty(sortName))
        //        {
        //            sort = " order by " + sortName.Trim() + " ";
        //            if (!string.IsNullOrEmpty(sortOrder))
        //                sort += sortOrder.Trim();
        //        }
        //        string sql = "from AscmForklift";

        //        string where = "", whereQueryWord = "";
        //        if (!string.IsNullOrEmpty(queryWord))
        //        {
        //            whereQueryWord = "forkliftType = '" + queryWord.Trim() + "'";
        //            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
        //        }
        //        if (!string.IsNullOrEmpty(queryOtherWord))
        //        {
        //            string sSql = " from YnUser where userName like '" + queryOtherWord + "%'";
        //            IList<YnUser> ilistYnUser = YnFrame.Dal.YnDaoHelper.GetInstance().nHibernateHelper.Find<YnUser>(sSql);
        //            if (ilistYnUser != null && ilistYnUser.Count > 0)
        //            {
        //                queryOtherWord = "";
        //                foreach (YnUser ynUser in ilistYnUser)
        //                {
        //                    if (!string.IsNullOrEmpty(queryOtherWord))
        //                        queryOtherWord += ",";
        //                    queryOtherWord += "'" + ynUser.userName + "'";
        //                }
        //            }
        //            whereQueryWord = "workerId in (" + queryOtherWord + ")";
        //            where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
        //        }

        //        if (!string.IsNullOrEmpty(where))
        //            sql += " where " + where;
        //        IList<AscmForklift> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmForklift>(sql, sql, ynPage);
        //        if (ilist != null)
        //        {
        //            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmForklift>(ilist);
        //            SetWorker(list);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmForklift)",ex);
        //        throw ex;
        //    }
        //    return list;
        //}

        public List<AscmForklift> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool isSetWorker = true)
        {
            List<AscmForklift> list = null;

            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                
                string sql = "from AscmForklift";
                string where = "", whereQueryWord = "";

                if (!string.IsNullOrEmpty(queryWord))
                {
                    string whereOtherWord = "";
                    whereQueryWord = "userName like '" + queryWord + "%'";
                    List<AscmUserInfo> listAscmUserInfo = AscmUserInfoService.GetInstance().GetList(null, "", "", "", whereQueryWord);
                    string ids = string.Empty;
                    if (listAscmUserInfo != null && listAscmUserInfo.Count > 0)
                    {
                        foreach (AscmUserInfo ascmUserInfo in listAscmUserInfo)
                        {
                            if (!string.IsNullOrEmpty(ids))
                                ids += ",";
                            ids += ascmUserInfo.userId;
                        }

                        whereQueryWord = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "workerId");
                        whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOtherWord, whereQueryWord);
                    }

                    whereQueryWord = "forkliftNumber like '%" + queryWord + "%'";
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOtherWord, whereQueryWord);

                    whereQueryWord = "assetsId like '%" + queryWord + "%'";
                    whereOtherWord = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereOtherWord, whereQueryWord);

                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOtherWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                if (!string.IsNullOrEmpty(sort))
                    sql += sort;

                IList<AscmForklift> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmForklift>(sql, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmForklift>(sql);

                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmForklift>(ilist);
                    if (isSetWorker)
                        SetWorker(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmForklift)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmForklift> listAscmForklift)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmForklift);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmForklift)",ex);
                throw ex;
            }
        }

        public void Save(AscmForklift ascmForklift)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmForklift);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmForklift)",ex);
                throw ex;
            }
        }

        public void Update(AscmForklift ascmForklift)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmForklift>(ascmForklift);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmForklift)",ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmForklift)",ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmForklift ascmForklift = Get(id);
                Delete(ascmForklift);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmForklift ascmForklift)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmForklift>(ascmForklift);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmForklift)",ex);
                throw ex;
            }
        }

        public void Delete(List<AscmForklift> listAscmForklift)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmForklift);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmForklift)",ex);
                throw ex;
            }
        }



        public void SetWorker(List<AscmForklift> list, bool isSetLogisticsClassName = true)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmForklift ascmForklift in list)
                {
                    if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(ascmForklift.workerId))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmForklift.workerId))
                    ids += "'" + ascmForklift.workerId + "'";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmUserInfo where " + where;
                    IList<AscmUserInfo> ilistAscmUserInfo = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                    if (ilistAscmUserInfo != null && ilistAscmUserInfo.Count > 0)
                    {
                        List<AscmUserInfo> listAscmUserInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilistAscmUserInfo);
                        if (isSetLogisticsClassName)
                            AscmUserInfoService.GetInstance().SetUserLogisticsClass(listAscmUserInfo);
                        foreach (AscmForklift ascmForklift in list)
                        {
                            if (!string.IsNullOrEmpty(ascmForklift.workerId))
                                ascmForklift.ascmUserInfo = listAscmUserInfo.Find(e => e.userId == ascmForklift.workerId);
                            if (isSetLogisticsClassName && ascmForklift.ascmUserInfo != null)
                                ascmForklift.logisticsClassName = ascmForklift.ascmUserInfo.logisticsClassName;
                        }
                    }
                }
            }
        }
    }
}
