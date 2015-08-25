using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal;
using NHibernate;
using YnBaseDal;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmMarkTaskLogService
    {
        private static AscmMarkTaskLogService service;
        public static AscmMarkTaskLogService GetInstance()
        {
            if (service == null)
                service = new AscmMarkTaskLogService();
            return service;
        }

        public AscmMarkTaskLog Get(int id)
        {
            AscmMarkTaskLog ascmMarkTaskLog = null;
            try
            {
                ascmMarkTaskLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMarkTaskLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMarkTaskLog)", ex);
                throw;
            }
            return ascmMarkTaskLog;
        }

        public List<AscmMarkTaskLog> GetList(string sql, bool isSetWipEntities = true, bool isSetGetMaterialTask = true)
        {
            List<AscmMarkTaskLog> list = null;
            try
            {
                IList<AscmMarkTaskLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                    if (isSetWipEntities)
                        SetWipEntities(list);
                    if (isSetGetMaterialTask)
                        SetGetMaterialTask(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMarkTaskLog)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmMarkTaskLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmMarkTaskLog> list = null;
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
                string sql = " from AscmMarkTaskLog ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (wipEntityId = " + queryWord.Trim() + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }

                if (!string.IsNullOrEmpty(whereOther))
                {
                    whereOther = "(taskId = " + whereOther.Trim() + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                }

                whereQueryWord = "isMark = 1";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                if (!string.IsNullOrEmpty(sort))
                    sql += sort;
                IList<AscmMarkTaskLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(sql, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);
                    SetWipEntities(list);
                    SetGetMaterialTask(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMarkTaskLog)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmMarkTaskLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool isSetWipEntities = true, bool isSetGetMaterialTask = true)
        {
            List<AscmMarkTaskLog> list = null;

            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string hql = "from AscmMarkTaskLog";
                string hql_Param = "select wipEntityId from AscmWipEntities where name like '{0}%'";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    hql_Param = string.Format(hql_Param, queryWord.Trim());
                    whereQueryWord = "wipEntityId in (" + hql_Param + ")";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                whereQueryWord = "isMark = 1";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    hql += " where " + where;
                if (!string.IsNullOrEmpty(sort))
                    hql += sort;

                IList<AscmMarkTaskLog> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(hql, hql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMarkTaskLog>(hql);

                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMarkTaskLog>(ilist);

                    if (isSetWipEntities)
                        SetWipEntities(list);
                    if (isSetGetMaterialTask)
                        SetGetMaterialTask(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMarkTaskLog)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmMarkTaskLog> listAscmMarkTaskLog)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmMarkTaskLog);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMarkTaskLog)", ex);
                    throw ex;
                }
            }
        }

        public void Save(AscmMarkTaskLog ascmMarkTaskLog)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmMarkTaskLog);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMarkTaskLog)", ex);
                    throw ex;
                }
            }
        }

        public void Update(AscmMarkTaskLog ascmMarkTaskLog)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmMarkTaskLog>(ascmMarkTaskLog);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMarkTaskLog)", ex);
                    throw ex;
                }
            }
        }

        public void Update(List<AscmMarkTaskLog> listAscmMarkTaskLog)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmMarkTaskLog);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMarkTaskLog)", ex);
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmMarkTaskLog ascmMarkTaskLog = Get(id);
                Delete(ascmMarkTaskLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmMarkTaskLog ascmMarkTaskLog)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmMarkTaskLog>(ascmMarkTaskLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmMarkTaskLog)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmMarkTaskLog> listAscmMarkTaskLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmMarkTaskLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmMarkTaskLog)", ex);
            }
        }


        public void SetWipEntities(List<AscmMarkTaskLog> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmMarkTaskLog.wipEntityId;
                }

                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "wipentityid");
                string sql = "from AscmWipEntities";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;

                    IList<AscmWipEntities> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmWipEntities> listAscmMarkTaskLog = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilist);
                        foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
                        {
                            ascmMarkTaskLog.ascmWipEntities = listAscmMarkTaskLog.Find(e => e.wipEntityId.ToString() == ascmMarkTaskLog.wipEntityId.ToString());
                        }
                    }
                }
            }
        }

        public void SetGetMaterialTask(List<AscmMarkTaskLog> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmMarkTaskLog.taskId;
                }

                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                string sql = "from AscmGetMaterialTask";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;

                    IList<AscmGetMaterialTask> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialTask>(sql);
                    if (ilist != null && ilist.Count > 0)
                    {
                        List<AscmGetMaterialTask> listAscmGetMaterialTask = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialTask>(ilist);
                        foreach (AscmMarkTaskLog ascmMarkTaskLog in list)
                        {
                            ascmMarkTaskLog.ascmGetMaterialTask = listAscmGetMaterialTask.Find(e => e.id == ascmMarkTaskLog.taskId);
                        }
                    }
                }
            }
        }
    }
}
