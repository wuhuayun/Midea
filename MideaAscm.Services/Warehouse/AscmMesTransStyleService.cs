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
    public class AscmMesTransStyleService
    {
        private static AscmMesTransStyleService ascmMesTransStyleServices;
        public static AscmMesTransStyleService GetInstance()
        {
            if (ascmMesTransStyleServices == null)
                ascmMesTransStyleServices = new AscmMesTransStyleService();
            return ascmMesTransStyleServices;
        }
        public AscmMesTransStyle Get(int id)
        {
            AscmMesTransStyle ascmMesTransStyle = null;
            try
            {
                ascmMesTransStyle = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMesTransStyle>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMesTransStyle)", ex);
                throw ex;
            }
            return ascmMesTransStyle;
        }
        public List<AscmMesTransStyle> GetList(string sql)
        {
            List<AscmMesTransStyle> list = null;
            try
            {
                if (string.IsNullOrEmpty(sql))
                    sql = "from AscmMesTransStyle";
                IList<AscmMesTransStyle> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMesTransStyle>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMesTransStyle>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMesTransStyle)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmMesTransStyle> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmMesTransStyle> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmMesTransStyle ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " code like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMesTransStyle> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMesTransStyle>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMesTransStyle>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMesTransStyle)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmMesTransStyle> listAscmMesTransStyle)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmMesTransStyle);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMesTransStyle)", ex);
                throw ex;
            }
        }
        public void Save(AscmMesTransStyle ascmMesTransStyle)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmMesTransStyle);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMesTransStyle)", ex);
                throw ex;
            }
        }
    }
}
