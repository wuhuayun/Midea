using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal;
using MideaAscm.Dal.Warehouse.Entities;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.Warehouse
{
    public class AscmMesInteractiveLogService
    {
        private static AscmMesInteractiveLogService ascmMesInteractiveLogServices;
        public static AscmMesInteractiveLogService GetInstance()
        {
            if (ascmMesInteractiveLogServices == null)
                ascmMesInteractiveLogServices = new AscmMesInteractiveLogService();
            return ascmMesInteractiveLogServices;
        }
        public AscmMesInteractiveLog Get(int id)
        {
            AscmMesInteractiveLog ascmMesInteractiveLog = null;
            try
            {
                ascmMesInteractiveLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMesInteractiveLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMesInteractiveLog)", ex);
                throw ex;
            }
            return ascmMesInteractiveLog;
        }
        public List<AscmMesInteractiveLog> GetList(string sql)
        {
            List<AscmMesInteractiveLog> list = null;
            try
            {
                IList<AscmMesInteractiveLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMesInteractiveLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMesInteractiveLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMesInteractiveLog)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmMesInteractiveLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmMesInteractiveLog> list = null;
            try
            {
                string sort = string.Empty;
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmMesInteractiveLog ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " docNumber like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " id in (select max(id) from AscmMesInteractiveLog group by docNumber) ");

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMesInteractiveLog> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMesInteractiveLog>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMesInteractiveLog>(sql + sort);

                if (ilist != null)
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMesInteractiveLog>(ilist);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMesInteractiveLog)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmMesInteractiveLog> listAscmMesInteractiveLog)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().Clear();
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmMesInteractiveLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMesInteractiveLog)", ex);
                throw ex;
            }
        }
        public void Save(AscmMesInteractiveLog ascmMesInteractiveLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmMesInteractiveLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMesInteractiveLog)", ex);
                throw ex;
            }
        }
    }
}
