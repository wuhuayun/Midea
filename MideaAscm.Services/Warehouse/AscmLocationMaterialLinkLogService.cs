using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;

namespace MideaAscm.Services.Warehouse
{
    public class AscmLocationMaterialLinkLogService
    {
        private static AscmLocationMaterialLinkLogService ascmLocationMaterialLinkLogService;
        public static AscmLocationMaterialLinkLogService GetInstance()
        {
            if (ascmLocationMaterialLinkLogService == null)
                ascmLocationMaterialLinkLogService = new AscmLocationMaterialLinkLogService();
            return ascmLocationMaterialLinkLogService;
        }
        public AscmLocationMaterialLinkLog Get(int id)
        {
            AscmLocationMaterialLinkLog ascmLocationMaterialLinkLog = null;
            try
            {
                ascmLocationMaterialLinkLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmLocationMaterialLinkLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmLocationMaterialLinkLog)", ex);
                throw ex;
            }
            return ascmLocationMaterialLinkLog;
        }
        public List<AscmLocationMaterialLinkLog> GetList(string sql)
        {
            List<AscmLocationMaterialLinkLog> list = null;
            try
            {
                IList<AscmLocationMaterialLinkLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLinkLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLocationMaterialLinkLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmLocationMaterialLinkLog)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmLocationMaterialLinkLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmLocationMaterialLinkLog> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmLocationMaterialLinkLog ";

                string where = "", whereQueryWord = "";

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmLocationMaterialLinkLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmLocationMaterialLinkLog>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmLocationMaterialLinkLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmLocationMaterialLinkLog)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmLocationMaterialLinkLog> list)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(list);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmLocationMaterialLinkLog)", ex);
                throw ex;
            }
        }
        public void Save(AscmLocationMaterialLinkLog ascmLocationMaterialLinkLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmLocationMaterialLinkLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmLocationMaterialLinkLog)", ex);
                throw ex;
            }
        }
    }
}
