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
    public class AscmWmsIncManAccMainService
    {
        private static AscmWmsIncManAccMainService ascmWmsIncManAccMainServices;
        public static AscmWmsIncManAccMainService GetInstance()
        {
            if (ascmWmsIncManAccMainServices == null)
                ascmWmsIncManAccMainServices = new AscmWmsIncManAccMainService();
            return ascmWmsIncManAccMainServices;
        }

        public AscmWmsIncManAccMain Get(int id)
        {
            AscmWmsIncManAccMain ascmWmsIncManAccMain = null;
            try
            {
                ascmWmsIncManAccMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsIncManAccMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsIncManAccMain)", ex);
                throw ex;
            }
            return ascmWmsIncManAccMain;
        }
        public List<AscmWmsIncManAccMain> GetList(string sql)
        {
            List<AscmWmsIncManAccMain> list = null;
            try
            {
                IList<AscmWmsIncManAccMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsIncManAccMain>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsIncManAccMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsIncManAccMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsIncManAccMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmWmsIncManAccMain> list = null;
            try
            {
                string sort = " order by docNumber ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmWmsIncManAccMain ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " docNumber like '%" + queryWord.Trim() + "%'";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmWmsIncManAccMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsIncManAccMain>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsIncManAccMain>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsIncManAccMain>(ilist);
                    SetSupplier(list);
                    SetSupplierAddress(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsIncManAccMain)", ex);
                throw ex;
            }
            return list;
        }
        public void SetSupplier(List<AscmWmsIncManAccMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsIncManAccMain ascmWmsIncManAccMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmWmsIncManAccMain.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    foreach (AscmWmsIncManAccMain ascmWmsIncManAccMain in list)
                    {
                        ascmWmsIncManAccMain.ascmSupplier = listAscmSupplier.Find(e => e.id == ascmWmsIncManAccMain.supplierId);
                    }
                }
            }
        }
        public void SetSupplierAddress(List<AscmWmsIncManAccMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsIncManAccMain ascmWmsIncManAccMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmWmsIncManAccMain.supplierAddressId + "";
                }
                string sql = "from AscmSupplierAddress where id in (" + ids + ")";
                IList<AscmSupplierAddress> ilistAscmSupplierAddress = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplierAddress>(sql);
                if (ilistAscmSupplierAddress != null && ilistAscmSupplierAddress.Count > 0)
                {
                    List<AscmSupplierAddress> listAscmSupplierAddress = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplierAddress>(ilistAscmSupplierAddress);
                    foreach (AscmWmsIncManAccMain ascmWmsIncManAccMain in list)
                    {
                        ascmWmsIncManAccMain.ascmSupplierAddress = listAscmSupplierAddress.Find(e => e.vendorSiteId == ascmWmsIncManAccMain.supplierAddressId);
                    }
                }
            }
        }
        public void Save(List<AscmWmsIncManAccMain> listAscmWmsIncManAccMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsIncManAccMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsIncManAccMain)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsIncManAccMain ascmWmsIncManAccMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsIncManAccMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsIncManAccMain)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsIncManAccMain ascmWmsIncManAccMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsIncManAccMain>(ascmWmsIncManAccMain);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsIncManAccMain)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsIncManAccMain)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsIncManAccMain ascmWmsIncManAccMain = Get(id);
                Delete(ascmWmsIncManAccMain);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsIncManAccMain ascmWmsIncManAccMain)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsIncManAccMain>(ascmWmsIncManAccMain);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsIncManAccMain)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsIncManAccMain> listAscmWmsIncManAccMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsIncManAccMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsIncManAccMain)", ex);
                throw ex;
            }
        }
    }
}
