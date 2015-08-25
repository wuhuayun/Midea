using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal;
using NHibernate;
using YnBaseDal;
using YnFrame.Dal.Entities;
using MideaAscm.Dal.Base.Entities;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmLogisticsClassInfoService
    {
        private static AscmLogisticsClassInfoService service;
        public static AscmLogisticsClassInfoService GetInstance()
        {
            if (service == null)
                service = new AscmLogisticsClassInfoService();
            return service;
        }

        public AscmLogisticsClassInfo Get(int id)
        {
            AscmLogisticsClassInfo ascmLogisticsClassInfo = null;
            try
            {
                ascmLogisticsClassInfo = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmLogisticsClassInfo>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmLogisticsClassInfo)", ex);
                throw ex;
            }
            return ascmLogisticsClassInfo;
        }

        public List<AscmLogisticsClassInfo> GetList(string sql, bool isSetGroupLeader = true, bool isSetMonitorLeader = true)
        {
            List<AscmLogisticsClassInfo> list = null;
            try
            {
                IList<AscmLogisticsClassInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLogisticsClassInfo>(ilist);
                    if (isSetGroupLeader)
                        SetGroupLeader(list);
                    if (isSetMonitorLeader)
                        SetMonitorLeader(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmLogisticsClassInfo)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmLogisticsClassInfo> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOhter, bool isSetGroupLeader = true, bool isSetMonitorLeader = true)
        {
            List<AscmLogisticsClassInfo> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = " from AscmLogisticsClassInfo ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    string whereWord = string.Empty;
                    whereQueryWord = "groupLeader = '" + queryWord.Trim() + "'";
                    whereWord = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereWord, whereQueryWord);
                    whereQueryWord = "monitorLeader = '" + queryWord.Trim() + "'";
                    whereWord = YnBaseClass2.Helper.StringHelper.SqlWhereOrAdd(whereWord, whereQueryWord);

                    whereWord = "(" + whereWord + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOhter);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                if (!string.IsNullOrEmpty(sort))
                    sql += sort;
                IList<AscmLogisticsClassInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLogisticsClassInfo>(sql, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLogisticsClassInfo>(ilist);
                    if (isSetGroupLeader)
                        SetGroupLeader(list);
                    if (isSetMonitorLeader)
                        SetMonitorLeader(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmLogisticsClassInfo)", ex);
                throw ex;
            }
            return list;
        }


        public void Save(List<AscmLogisticsClassInfo> listAscmLogisticsClassInfo)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmLogisticsClassInfo);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmLogisticsClassInfo)", ex);
                throw ex;
            }
        }

        public void Save(AscmLogisticsClassInfo ascmLogisticsClassInfo)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmLogisticsClassInfo);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmLogisticsClassInfo)", ex);
                throw ex;
            }
        }

        public void Update(AscmLogisticsClassInfo ascmLogisticsClassInfo)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmLogisticsClassInfo>(ascmLogisticsClassInfo);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmLogisticsClassInfo)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmLogisticsClassInfo)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmLogisticsClassInfo> listAscmLogisticsClassInfo)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmLogisticsClassInfo);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmLogisticsClassInfo)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmLogisticsClassInfo)", ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmLogisticsClassInfo ascmLogisticsClassInfo = Get(id);
                Delete(ascmLogisticsClassInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmLogisticsClassInfo ascmLogisticsClassInfo)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmLogisticsClassInfo>(ascmLogisticsClassInfo);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmLogisticsClassInfo)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmLogisticsClassInfo> listAscmLogisticsClassInfo)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmLogisticsClassInfo);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmLogisticsClassInfo)", ex);
            }
        }


        public void SetGroupLeader(List<AscmLogisticsClassInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmLogisticsClassInfo.groupLeader))
                        ids += "'" + ascmLogisticsClassInfo.groupLeader + "'";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmUserInfo where " + where;
                    IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmUserInfo> listUser = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                        foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in list)
                        {
                            ascmLogisticsClassInfo.groupUser = listUser.Find(e => e.userId == ascmLogisticsClassInfo.groupLeader);
                        }
                    }
                }
            }
        }

        public void SetMonitorLeader(List<AscmLogisticsClassInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    if (!string.IsNullOrEmpty(ascmLogisticsClassInfo.monitorLeader))
                        ids += "'" + ascmLogisticsClassInfo.monitorLeader + "'";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "userId");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmUserInfo where " + where;
                    IList<AscmUserInfo> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmUserInfo> listUser = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilist);
                        foreach (AscmLogisticsClassInfo ascmLogisticsClassInfo in list)
                        {
                            ascmLogisticsClassInfo.monitorUser = listUser.Find(e => e.userId == ascmLogisticsClassInfo.monitorLeader);
                        }
                    }
                }
            }
        }
    }
}
