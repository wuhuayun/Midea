using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.GetMaterialManage.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.GetMaterialManage
{
    public class AscmGetMaterialLogService
    {
        private static AscmGetMaterialLogService service;
        public static AscmGetMaterialLogService GetInstance()
        {
            if (service == null)
                service = new AscmGetMaterialLogService();
            return service;
        }

        public AscmGetMaterialLog Get(int id)
        {
            AscmGetMaterialLog ascmGetMaterialLog = null;
            try
            {
                ascmGetMaterialLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmGetMaterialLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGetMaterialLog)", ex);
                throw ex;
            }
            return ascmGetMaterialLog;
        }

        public List<AscmGetMaterialLog> GetList(string sql)
        {
            List<AscmGetMaterialLog> list = null;
            try
            {
                IList<AscmGetMaterialLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGetMaterialLog)", ex);
                throw ex;
            }

            return list;
        }

        public List<AscmGetMaterialLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmGetMaterialLog> list = new List<AscmGetMaterialLog>();

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

                string sql = " from AscmGetMaterialLog ";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = "workerId like '%" + queryWord + "%'";
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                if (!string.IsNullOrEmpty(sort))
                    sql += sort;

                IList<AscmGetMaterialLog> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialLog>(sql, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmGetMaterialLog>(sql);

                if (ilist != null && ilist.Count > 0)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmGetMaterialLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmGetMaterialLog)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmGetMaterialLog> listAscmGetMaterialLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmGetMaterialLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmGetMaterialLog)", ex);
                throw ex;
            }
        }

        public void Save(AscmGetMaterialLog ascmGetMaterialLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmGetMaterialLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmGetMaterialLog)", ex);
                throw ex;
            }
        }

        public void Update(AscmGetMaterialLog ascmGetMaterialLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmGetMaterialLog>(ascmGetMaterialLog);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGetMaterialLog)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGetMaterialLog)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmGetMaterialLog> listAscmGetMaterialLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmGetMaterialLog);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGetMaterialLog)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmGetMaterialLog)", ex);
                throw ex;
            }
        }
    }
}
