using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Dal.FromErp.Entities;

namespace MideaAscm.Services.Warehouse
{
    public class AscmWmsBackInvoiceMainService
    {
        private static AscmWmsBackInvoiceMainService ascmWmsBackInvoiceMainServices;
        public static AscmWmsBackInvoiceMainService GetInstance()
        {
            if (ascmWmsBackInvoiceMainServices == null)
                ascmWmsBackInvoiceMainServices = new AscmWmsBackInvoiceMainService();
            return ascmWmsBackInvoiceMainServices;
        }

        public int GetMaxId()
        {
            int maxId = 0;
            try
            {
                maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsBackInvoiceMain");
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsBackInvoiceMain MaxId)", ex);
                throw ex;
            }
            return maxId;
        }
        public AscmWmsBackInvoiceMain Get(int id)
        {
            AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain = null;
            try
            {
                ascmWmsBackInvoiceMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsBackInvoiceMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
            return ascmWmsBackInvoiceMain;
        }
        public List<AscmWmsBackInvoiceMain> GetList(string sql)
        {
            List<AscmWmsBackInvoiceMain> list = null;
            try
            {
                IList<AscmWmsBackInvoiceMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceMain>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsBackInvoiceMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsBackInvoiceMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWmsBackInvoiceMain> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (docNumber like '%" + queryWord.Trim() + "%' or manualDocNumber like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                string sql = "from AscmWmsBackInvoiceMain ";
                
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsBackInvoiceMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceMain>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceMain>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsBackInvoiceMain>(ilist);
                    SetSupplier(list);
                    SetTransactionReason(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmWmsBackInvoiceMain> listAscmWmsBackInvoiceMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsBackInvoiceMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsBackInvoiceMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsBackInvoiceMain>(ascmWmsBackInvoiceMain);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsBackInvoiceMain)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain = Get(id);
                Delete(ascmWmsBackInvoiceMain);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsBackInvoiceMain>(ascmWmsBackInvoiceMain);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsBackInvoiceMain> listAscmWmsBackInvoiceMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsBackInvoiceMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsBackInvoiceMain)", ex);
                throw ex;
            }
        }

        private void SetSupplier(List<AscmWmsBackInvoiceMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsBackInvoiceMain.supplierId;
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    foreach (AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain in list)
                    {
                        ascmWmsBackInvoiceMain.ascmSupplier = listAscmSupplier.Find(e => e.id == ascmWmsBackInvoiceMain.supplierId);
                    }
                }
            }
        }
        private void SetTransactionReason(List<AscmWmsBackInvoiceMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsBackInvoiceMain.reasonId;
                }
                string sql = "from AscmMtlTransactionReasons where id in (" + ids + ")";
                IList<AscmMtlTransactionReasons> ilistAscmMtlTransactionReasons = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMtlTransactionReasons>(sql);
                if (ilistAscmMtlTransactionReasons != null && ilistAscmMtlTransactionReasons.Count > 0)
                {
                    List<AscmMtlTransactionReasons> listAscmMtlTransactionReasons = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMtlTransactionReasons>(ilistAscmMtlTransactionReasons);
                    foreach (AscmWmsBackInvoiceMain ascmWmsBackInvoiceMain in list)
                    {
                        ascmWmsBackInvoiceMain.ascmMtlTransactionReasons = listAscmMtlTransactionReasons.Find(e => e.reasonId == ascmWmsBackInvoiceMain.reasonId);
                    }
                }
            }
        }

    }
}
