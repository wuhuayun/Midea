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
    public class AscmMaterialCategoryService
    {
        private static AscmMaterialCategoryService service;
        public static AscmMaterialCategoryService GetInstance()
        {
            if (service == null)
                service = new AscmMaterialCategoryService();
            return service;
        }

        public AscmMaterialCategory Get(int id)
        {
            AscmMaterialCategory ascmMaterialCategory = null;
            try
            {
                ascmMaterialCategory = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMaterialCategory>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMaterialCategory)");
                throw ex;
            }
            return ascmMaterialCategory;
        }

        public AscmMaterialCategory GetId(string categoryCode)
        {
            AscmMaterialCategory MaterialCategory = null;
            try
            {
                string sql = " from AscmMaterialCategory";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(categoryCode))
                    whereQueryWord = "categorycode = '" + categoryCode + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMaterialCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialCategory>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    foreach (AscmMaterialCategory ascmMaterialCategory in ilist)
                    {
                        MaterialCategory = ascmMaterialCategory;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialItem)", ex);
                throw ex;
            }
            return MaterialCategory;
        }

        public List<AscmMaterialCategory> GetList(string sql)
        {
            List<AscmMaterialCategory> list = null;
            try
            {
                IList<AscmMaterialCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialCategory>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialCategory>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialCategory)");
                throw ex;
            }
            return list;
        }

        public List<AscmMaterialCategory> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmMaterialCategory> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmMaterialCategory ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    if (queryWord.Length > 1)
                    {
                        string temp = queryWord.Trim().Substring(0, 2);
                        if (temp == "20")
                        {
                            whereQueryWord = " (categoryCode like '" + queryWord.Trim() + "%')";
                        }
                    }
                }
                else
                {
                    whereQueryWord = "(categoryCode like '20%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where + " order by categoryCode";
                IList<AscmMaterialCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialCategory>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialCategory>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialCategory)",ex);
                throw ex;
            }
            return list;
        }

        public void Save(List<AscmMaterialCategory> listAscmMaterialCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmMaterialCategory);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMaterialCategory)",ex);
                throw ex;
            }
        }

        public void Save(AscmMaterialCategory ascmMaterialCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmMaterialCategory);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMaterialCategory)",ex);
                throw ex;
            }
        }

        public void Update(AscmMaterialCategory ascmMaterialCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmMaterialCategory>(ascmMaterialCategory);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMaterialCategory)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmMaterialCategory)", ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmMaterialCategory ascmMaterialCategory = Get(id);
                Delete(ascmMaterialCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmMaterialCategory ascmMaterialCategory)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmMaterialCategory>(ascmMaterialCategory);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmMaterialCategory)",ex);
                throw;
            }
        }

        public void Delete(List<AscmMaterialCategory> listAscmMaterialCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmMaterialCategory);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmMaterialCategory)",ex);
                throw;
            }
        }
    }
}
