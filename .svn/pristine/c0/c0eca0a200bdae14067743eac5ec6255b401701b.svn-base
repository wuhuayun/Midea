using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Services.Vehicle
{
    public class AscmUnloadingPointLogService
    {
        private static AscmUnloadingPointLogService ascmUnloadingPointLogServices;
        public static AscmUnloadingPointLogService GetInstance()
        {
            if (ascmUnloadingPointLogServices == null)
                ascmUnloadingPointLogServices = new AscmUnloadingPointLogService();
            return ascmUnloadingPointLogServices;
        }

        public AscmUnloadingPointLog Get(string id)
        {
            AscmUnloadingPointLog ascmUnloadingPointLog = null;
            try
            {
                ascmUnloadingPointLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUnloadingPointLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUnloadingPointLog)", ex);
                throw ex;
            }
            return ascmUnloadingPointLog;
        }
        public List<AscmUnloadingPointLog> GetList(string sql)
        {
            List<AscmUnloadingPointLog> list = null;
            try
            {
                IList<AscmUnloadingPointLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointLog)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmUnloadingPointLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmUnloadingPointLog> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmUnloadingPointLog ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (id like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmUnloadingPointLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointLog>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPointLog)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmUnloadingPointLog> listAscmUnloadingPointLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmUnloadingPointLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointLog)", ex);
                throw ex;
            }
        }
        public void Save(AscmUnloadingPointLog ascmUnloadingPointLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmUnloadingPointLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPointLog)", ex);
                throw ex;
            }
        }
        public void Delete(string id)
        {
            try
            {
                AscmUnloadingPointLog ascmUnloadingPointLog = Get(id);
                Delete(ascmUnloadingPointLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmUnloadingPointLog ascmUnloadingPointLog)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmUnloadingPointLog>(ascmUnloadingPointLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointLog)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmUnloadingPointLog> listAscmUnloadingPointLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmUnloadingPointLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPointLog)", ex);
                throw ex;
            }
        }
        public void AddLog(int unloadingPointId, string unloadingPointName, string unloadingPointSn, string unloadingPointStatus, DateTime createTime)
        {
            try
            {
                AscmUnloadingPointLog ascmUnloadingPointLog = GetAddLog(unloadingPointId, unloadingPointName, unloadingPointSn, unloadingPointStatus, createTime);

                Save(ascmUnloadingPointLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("增加失败(Add AscmUnloadingPointLog)", ex);
                throw ex;
            }
        }
        public AscmUnloadingPointLog GetAddLog(int unloadingPointId, string unloadingPointName, string unloadingPointSn, string unloadingPointStatus, DateTime createTime)
        {
            try
            {
                DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmUnloadingPointLog");

                AscmUnloadingPointLog ascmUnloadingPointLog = new AscmUnloadingPointLog();
                ascmUnloadingPointLog.createTime = createTime.ToString("yyyy-MM-dd HH:mm:ss");
                ascmUnloadingPointLog.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");

                ascmUnloadingPointLog.unloadingPointId = unloadingPointId;
                ascmUnloadingPointLog.unloadingPointName = unloadingPointName;
                ascmUnloadingPointLog.unloadingPointSn = unloadingPointSn;
                ascmUnloadingPointLog.unloadingPointStatus = unloadingPointStatus;

                return ascmUnloadingPointLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
