using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;

namespace MideaAscm.Services.Warehouse
{
    public class AscmAssignWarelocationService
    {
        private static AscmAssignWarelocationService ascmAssignWarelocationServices;
        public static AscmAssignWarelocationService GetInstance()
        {
            if (ascmAssignWarelocationServices == null)
                ascmAssignWarelocationServices = new AscmAssignWarelocationService();
            return ascmAssignWarelocationServices;
        }

        public AscmAssignWarelocation Get(int id)
        {
            AscmAssignWarelocation ascmAssignWarelocation = null;
            try
            {
                ascmAssignWarelocation = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmAssignWarelocation>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmAssignWarelocation)", ex);
                throw ex;
            }
            return ascmAssignWarelocation;
        }
        public List<AscmAssignWarelocation> GetList(string sql)
        {
            List<AscmAssignWarelocation> list = null;
            try
            {
                IList<AscmAssignWarelocation> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAssignWarelocation>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAssignWarelocation>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmAssignWarelocation)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmAssignWarelocation> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmAssignWarelocation> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmAssignWarelocation ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " batchBarCode like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmAssignWarelocation> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAssignWarelocation>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmAssignWarelocation>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmAssignWarelocation>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmAssignWarelocation)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmAssignWarelocation> listAscmAssignWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmAssignWarelocation);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmAssignWarelocation)", ex);
                throw ex;
            }
        }
        public void Save(AscmAssignWarelocation ascmAssignWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmAssignWarelocation);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmAssignWarelocation)", ex);
                throw ex;
            }
        }
        public void Update(AscmAssignWarelocation ascmAssignWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmAssignWarelocation>(ascmAssignWarelocation);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmAssignWarelocation)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmAssignWarelocation)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmAssignWarelocation ascmAssignWarelocation = Get(id);
                Delete(ascmAssignWarelocation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmAssignWarelocation ascmAssignWarelocation)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmAssignWarelocation>(ascmAssignWarelocation);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmAssignWarelocation)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmAssignWarelocation> listAscmAssignWarelocation)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmAssignWarelocation);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmAssignWarelocation)", ex);
                throw ex;
            }
        }
    }
}
