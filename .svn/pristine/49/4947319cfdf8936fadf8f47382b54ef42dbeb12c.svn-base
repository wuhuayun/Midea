using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;

namespace MideaAscm.Services.SupplierPreparation
{
    public class AscmContainerSpecService
    {
        private static AscmContainerSpecService ascmContainerSpecServices;
        public static AscmContainerSpecService GetInstance()
        {
            if (ascmContainerSpecServices == null)
                ascmContainerSpecServices = new AscmContainerSpecService();
            return ascmContainerSpecServices;
        }

        public AscmContainerSpec Get(int id, string sessionKey = null)
        {
            AscmContainerSpec ascmContainerSpec = null;
            try
            {
                ascmContainerSpec = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmContainerSpec>(id, sessionKey);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmContainerSpec)", ex);
                throw ex;
            }
            return ascmContainerSpec;
        }
        public List<AscmContainerSpec> GetList(string sql)
        {
            List<AscmContainerSpec> list = null;
            try
            {
                IList<AscmContainerSpec> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerSpec>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerSpec>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainerSpec)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmContainerSpec> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmContainerSpec> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmContainerSpec ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (spec like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmContainerSpec> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerSpec>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerSpec>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmContainerSpec)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmContainerSpec> listAscmContainerSpec)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmContainerSpec);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmContainerSpec)", ex);
                throw ex;
            }
        }
        public void Save(AscmContainerSpec ascmContainerSpec)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmContainerSpec);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmContainerSpec)", ex);
                throw ex;
            }
        }
        public void Update(AscmContainerSpec ascmContainerSpec)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmContainerSpec>(ascmContainerSpec);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmContainerSpec)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmContainerSpec)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmContainerSpec ascmContainerSpec = Get(id);
                Delete(ascmContainerSpec);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmContainerSpec ascmContainerSpec)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmContainerSpec>(ascmContainerSpec);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmContainerSpec)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmContainerSpec> listAscmContainerSpec)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmContainerSpec);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmContainerSpec)", ex);
                throw ex;
            }
        }
    }
}
