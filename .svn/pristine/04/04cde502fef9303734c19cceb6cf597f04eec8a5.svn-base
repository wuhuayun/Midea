using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using NHibernate;
using MideaAscm.Dal; 
using YnBaseDal;
using YnFrame.Dal.Entities;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmAllocateRuleService
    {
        private static AscmAllocateRuleService service;
        public static AscmAllocateRuleService GetInstance()
        {
            if (service == null)
                service = new AscmAllocateRuleService();
            return service;
        }

        public AscmAllocateRule Get(int id)
        {
            AscmAllocateRule ascmAllocateRule = null;
            try
            {
                ascmAllocateRule = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmAllocateRule>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmAllocateRule)", ex);
                throw ex;
            }
            return ascmAllocateRule;
        }

        public List<AscmAllocateRule> GetList(string sql, bool isSetWorker = true, bool isSetZRanker = true, bool isSetDRanker = true)
        {
            List<AscmAllocateRule> list = null;
            try
            {
                IList<AscmAllocateRule> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilist);
                    if (isSetWorker)
                        SetWorker(list);
                    if (isSetZRanker)
                        SetZRanker(list);
                    if (isSetDRanker)
                        SetDRanker(list);

                    SetLogisticsClass(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmAllocateRule)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmAllocateRule> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, string userLogistisClass)
        {
            List<AscmAllocateRule> list = new List<AscmAllocateRule>();
            try
            {
                string sort = "";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                else
                {
                    sort = " order by id ";
                }
                string sql = " from AscmAllocateRule ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "workerName = '" + queryWord.Trim() + "'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                if (!string.IsNullOrEmpty(sort))
                    sql += sort;
                IList<AscmAllocateRule> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql,sql,ynPage);
                if (ilist != null)
                {
                    List<AscmAllocateRule> listAscmAllocateRule = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilist);
                    SetLogisticsClass(listAscmAllocateRule);
                    if (!string.IsNullOrEmpty(userLogistisClass))
                    {
                        foreach (AscmAllocateRule ascmAllocateRule in listAscmAllocateRule)
                        {
                            if (ascmAllocateRule.logisticsClassId > 0)
                            {
                                if (userLogistisClass.IndexOf(ascmAllocateRule.ascmLogisticsClassInfo.logisticsClass) > -1)
                                {
                                    list.Add(ascmAllocateRule);
                                }
                            }
                            else if (ascmAllocateRule.logisticsClassId == 0)
                            {
                                list.Add(ascmAllocateRule);
                            }
                        }
                    }
                    else
                    {
                        list = listAscmAllocateRule;
                    }
                    SetWorker(list);
                    SetZRanker(list);
                    SetDRanker(list);
                    SetLogisticsClass(list);
                    SetLogisticsClassName(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmAllocateRule)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmAllocateRule> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmAllocateRule> list = new List<AscmAllocateRule>();

            try
            {
                string sort = "";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                else
                {
                    sort = " order by id ";
                }

                string sql = " from AscmAllocateRule ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "workerName like '" + queryWord.Trim() + "%' or zRankerName like '" + queryWord.Trim() + "%' or dRankerName like '" + queryWord.Trim() + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                if (!string.IsNullOrEmpty(sort))
                    sql += sort;

                IList<AscmAllocateRule> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(sql);

                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmAllocateRule)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmAllocateRule> listAscmAllocateRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmAllocateRule);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmAllocateRule)", ex);
                throw ex;
            }
        }

        public void Save(AscmAllocateRule ascmAllocateRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmAllocateRule);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmAllocateRule)", ex);
                throw ex;
            }
        }

        public void Update(AscmAllocateRule ascmAllocateRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmAllocateRule>(ascmAllocateRule);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmAllocateRule)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmAllocateRule)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmAllocateRule> listAscmAllocateRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmAllocateRule);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmAllocateRule)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmAllocateRule)", ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmAllocateRule ascmAllocateRule = Get(id);
                Delete(ascmAllocateRule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmAllocateRule ascmAllocateRule)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmAllocateRule>(ascmAllocateRule);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmAllocateRule)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmAllocateRule> listAscmAllocateRule)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmAllocateRule);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmAllocateRule)", ex);
                throw ex;
            }
        }

        // 获取物流组排产员名称@2014/5/7
        public string GetLogisticsRankerName(string userLogisticsClass, string userName, bool isRole = false)
        {
            string ids_userId = string.Empty;
            try
            {
                string hql = "from AscmAllocateRule where workerName in {0}";
                if (!isRole)
                {
                    
                    string hql_Param = "(select userId from AscmUserInfo where logisticsClass = '" + userLogisticsClass + "')";
                    hql = string.Format(hql, hql_Param);
                }
                else
                {
                    hql = hql.Replace("in", "=");
                    string hql_Param = "'" + userName + "'";
                    hql = string.Format(hql, hql_Param);
                }

                IList<AscmAllocateRule> ilistAscmAllocateRule = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAllocateRule>(hql);
                if (ilistAscmAllocateRule != null && ilistAscmAllocateRule.Count > 0)
                {
                    List<AscmAllocateRule> listAscmAllocateRule = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAllocateRule>(ilistAscmAllocateRule);
                    foreach (AscmAllocateRule ascmAllocateRule in listAscmAllocateRule)
                    {
                        if (!string.IsNullOrEmpty(ids_userId) && !string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                            ids_userId += ",";
                        if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                            ids_userId += "'" + ascmAllocateRule.zRankerName + "'";
                        if (!string.IsNullOrEmpty(ids_userId) && !string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                            ids_userId += ",";
                        if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                            ids_userId += "'" + ascmAllocateRule.dRankerName + "'";
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("获取物流组排产员(Get LogisticsRankerName)", ex);
                throw ex;
            }

            return ids_userId;
        }

        public void SetWorker(List<AscmAllocateRule> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmAllocateRule ascmAllocateRule in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    if(!string.IsNullOrEmpty(ascmAllocateRule.workerName))
                        ids += "'" + ascmAllocateRule.workerName + "'";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmUserInfo where " + where;
                    IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmUserInfo> listAscmUserInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                        foreach (AscmAllocateRule ascmAllocateRule in list)
                        {
                            ascmAllocateRule.ascmUserInfoWorker = listAscmUserInfo.Find(e => e.userId == ascmAllocateRule.workerName);
                        }
                    }
                }
            }
        }

        public void SetZRanker(List<AscmAllocateRule> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmAllocateRule ascmAllocateRule in list)
                {
                    if ((!string.IsNullOrEmpty(ids)) && (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName)))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                        ids += "'" + ascmAllocateRule.zRankerName + "'";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmUserInfo where " + where;
                    IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmUserInfo> listAscmUserInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                        foreach (AscmAllocateRule ascmAllocateRule in list)
                        {
                            if (!string.IsNullOrEmpty(ascmAllocateRule.zRankerName))
                                ascmAllocateRule.ascmUserInfoZRanker = listAscmUserInfo.Find(e => e.userId == ascmAllocateRule.zRankerName);
                        }
                    }
                }
            }
        }

        public void SetDRanker(List<AscmAllocateRule> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmAllocateRule ascmAllocateRule in list)
                {
                    if ((!string.IsNullOrEmpty(ids)) && (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName)))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                        ids += "'" + ascmAllocateRule.dRankerName + "'";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmUserInfo where " + where;
                    IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmUserInfo> listAscmUserInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                        foreach (AscmAllocateRule ascmAllocateRule in list)
                        {
                            if (!string.IsNullOrEmpty(ascmAllocateRule.dRankerName))
                                ascmAllocateRule.ascmUserInfoDRanker = listAscmUserInfo.Find(e => e.userId == ascmAllocateRule.dRankerName);
                        }
                    }
                }
            }
        }

        public void SetLogisticsClass(List<AscmAllocateRule> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmAllocateRule ascmAllocateRule in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmAllocateRule.logisticsClassId ;
                }

                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                string sql = "from AscmLogisticsClassInfo";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    IList<AscmLogisticsClassInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmLogisticsClassInfo> listAscmLogisticsClassInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLogisticsClassInfo>(ilist);
                        foreach (AscmAllocateRule ascmAllocateRule in list)
                        {
                            if (!string.IsNullOrEmpty(ascmAllocateRule.logisticsClassId.ToString()) && ascmAllocateRule.logisticsClassId != 0)
                            {
                                ascmAllocateRule.ascmLogisticsClassInfo = listAscmLogisticsClassInfo.Find(e => e.id == ascmAllocateRule.logisticsClassId);
                            }
                        }
                    }
                }
            }
        }

        public void SetLogisticsClass(AscmAllocateRule ascmAllocateRule)
        {
            if (ascmAllocateRule != null)
            {
                string ids = string.Empty;
                if (!string.IsNullOrEmpty(ascmAllocateRule.logisticsClassId.ToString()) && ascmAllocateRule.logisticsClassId != 0)
                    ids = ascmAllocateRule.logisticsClassId.ToString();

                if (!string.IsNullOrEmpty(ids))
                {
                    string sql = "from AscmLogisticsClassInfo where id = " + ids;

                    IList<AscmLogisticsClassInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmLogisticsClassInfo> listAscmLogisticsClassInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLogisticsClassInfo>(ilist);
                        ascmAllocateRule.ascmLogisticsClassInfo = listAscmLogisticsClassInfo.Find(e => e.id == ascmAllocateRule.logisticsClassId);
                    }
                }
            }
        }

        public void SetLogisticsClassName(List<AscmAllocateRule> list)
        {
            if (list != null && list.Count > 0)
            {
                foreach (AscmAllocateRule ascmAllocateRule in list)
                {
                    ascmAllocateRule.logisticsClassName = AscmCommonHelperService.GetInstance().DisplayLogisticsClass(ascmAllocateRule.logisticsClass);
                }
            }
        }
    }
}
