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
    public class AscmPalletService
    {
        private static AscmPalletService ascmPalletServices;
        public static AscmPalletService GetInstance()
        {
            if (ascmPalletServices == null)
                ascmPalletServices = new AscmPalletService();
            return ascmPalletServices;
        }

        public AscmPallet Get(string sn)
        {
            AscmPallet ascmPallet = null;
            try
            {
                ascmPallet = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmPallet>(sn);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmPallet)", ex);
                throw ex;
            }
            return ascmPallet;
        }
        public List<AscmPallet> GetList(string sql)
        {
            List<AscmPallet> list = null;
            try
            {
                IList<AscmPallet> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPallet>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPallet>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmPallet)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmPallet> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmPallet> list = null;
            try
            {
                string sort = " order by sn ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmPallet ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (sn like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmPallet> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPallet>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPallet>(ilist);
                    SetSupplier(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmPallet)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmPallet> listAscmPallet)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmPallet);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPallet)", ex);
                throw ex;
            }
        }
        public void Save(AscmPallet ascmPallet)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmPallet);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPallet)", ex);
                throw ex;
            }
        }
        public void Update(AscmPallet ascmPallet)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmPallet>(ascmPallet);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmPallet)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmPallet)", ex);
                throw ex;
            }
        }
        public void Delete(string sn)
        {
            try
            {
                AscmPallet ascmPallet = Get(sn);
                Delete(ascmPallet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmPallet ascmPallet)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmPallet>(ascmPallet);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmPallet)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmPallet> listAscmPallet)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmPallet);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmPallet)", ex);
                throw ex;
            }
        }

        private void SetSupplier(List<AscmPallet> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmPallet ascmPallet in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmPallet.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    foreach (AscmPallet ascmPallet in list)
                    {
                        ascmPallet.supplier = listAscmSupplier.Find(e => e.id == ascmPallet.supplierId);
                    }
                }
            }
        }
    }
}
