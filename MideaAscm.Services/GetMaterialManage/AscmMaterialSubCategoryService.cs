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
    public class AscmMaterialSubCategoryService
    {
        private static AscmMaterialSubCategoryService service;
        public static AscmMaterialSubCategoryService GetInstance()
        {
            if (service == null)
                service = new AscmMaterialSubCategoryService();
            return service;
        }

        public AscmMaterialSubCategory Get(int id)
        {
            AscmMaterialSubCategory ascmMaterialSubCategory = null;
            try
            {
                ascmMaterialSubCategory = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmMaterialSubCategory>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMaterialSubCategory)", ex);
                throw ex;
            }
            return ascmMaterialSubCategory;
        }

        public AscmMaterialSubCategory GetId(string combinationCode)
        {
            AscmMaterialSubCategory MaterialSubCategory = null;
            try
            {
                string sql = "from AscmMaterialSubCategory";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(combinationCode))
                    whereQueryWord = "combinationCode = '"  + combinationCode + "'";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where,whereQueryWord);
                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmMaterialSubCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    foreach (AscmMaterialSubCategory ascmMaterialSubCategory in ilist)
                    {
                        MaterialSubCategory = ascmMaterialSubCategory;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmMaterialSubCategory)", ex);
                throw ex;
            }

            return MaterialSubCategory;
        }

        public List<AscmMaterialSubCategory> GetList(string sql, bool isSetMaterialCategory)
        {
            List<AscmMaterialSubCategory> list = null;
            try
            {
                IList<AscmMaterialSubCategory> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialSubCategory>(ilist);
                    if (isSetMaterialCategory)
                        SetMaterialCategory(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMaterialSubCategory)", ex);
                throw ex;
            }
            return list;
        }

        public List<AscmMaterialSubCategory> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther, bool isSetMaterialCategory = true)
        {
            List<AscmMaterialSubCategory> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmMaterialSubCategory ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (subCategoryCode like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                if (!string.IsNullOrEmpty(sort))
                    sql += sort;

                IList<AscmMaterialSubCategory> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialSubCategory>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialSubCategory>(ilist);
                    if (isSetMaterialCategory)
                        SetMaterialCategory(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmMaterialSubCategory)", ex);
                throw ex;
            }

            return list;
        }

        public void Save(List<AscmMaterialSubCategory> listAscmMaterialSubCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmMaterialSubCategory);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMaterialSubCategory)", ex);
                throw ex;
            }
        }

        public void Save(AscmMaterialSubCategory ascmMaterialSubCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmMaterialSubCategory);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception)
                    {
                        tx.Rollback();//回滚
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmMaterialSubCategory)", ex);
                throw ex;
            }
        }

        public void Update(AscmMaterialSubCategory ascmMaterialSubCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmMaterialSubCategory>(ascmMaterialSubCategory);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMaterialSubCategory)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Get AscmMaterialSubCategory)", ex);
                throw ex;
            }
        }

        public void Update(List<AscmMaterialSubCategory> listAscmMaterialSubCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmMaterialSubCategory);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmMaterialSubCategory)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Get AscmMaterialSubCategory)", ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmMaterialSubCategory ascmMaterialSubCategory = Get(id);
                Delete(ascmMaterialSubCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(AscmMaterialSubCategory ascmMaterialSubCategory)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmMaterialSubCategory>(ascmMaterialSubCategory);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmMaterialSubCategory)", ex);
                throw ex;
            }
        }

        public void Delete(List<AscmMaterialSubCategory> listAscmMaterialSubCategory)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmMaterialSubCategory);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmMaterialSubCategory)", ex);
                throw ex;
            }
        }



        private void SetMaterialCategory(List<AscmMaterialSubCategory> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmMaterialSubCategory ascmMaterialSubCategory in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmMaterialSubCategory.categoryId + "";
                }
                string where = AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "id");
                if (!string.IsNullOrEmpty(where))
                {
                    string sql = "from AscmMaterialCategory where " + where;
                    IList<AscmMaterialCategory> ilistAscmMaterialCategory = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialCategory>(sql);
                    if (ilistAscmMaterialCategory != null && ilistAscmMaterialCategory.Count > 0)
                    {
                        List<AscmMaterialCategory> listAscmMaterialCategory = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialCategory>(ilistAscmMaterialCategory);
                        foreach (AscmMaterialSubCategory ascmMaterialSubCategory in list)
                        {
                            ascmMaterialSubCategory.ascmMaterialCategory = listAscmMaterialCategory.Find(e => e.id == ascmMaterialSubCategory.categoryId);
                        }
                    }
                }
            }
        }
    }
}
