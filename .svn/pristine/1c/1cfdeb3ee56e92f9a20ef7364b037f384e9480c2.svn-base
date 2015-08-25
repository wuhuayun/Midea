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
    public class AscmWmsBackInvoiceDetailService
    {
        private static AscmWmsBackInvoiceDetailService ascmWmsBackInvoiceDetailServices;
        public static AscmWmsBackInvoiceDetailService GetInstance()
        {
            if (ascmWmsBackInvoiceDetailServices == null)
                ascmWmsBackInvoiceDetailServices = new AscmWmsBackInvoiceDetailService();
            return ascmWmsBackInvoiceDetailServices;
        }

        public int GetMaxId()
        {
            int maxId = 0;
            try
            {
                maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmWmsBackInvoiceDetail");
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsBackInvoiceDetail MaxId)", ex);
                throw ex;
            }
            return maxId;
        }
        public AscmWmsBackInvoiceDetail Get(int id)
        {
            AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail = null;
            try
            {
                ascmWmsBackInvoiceDetail = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmWmsBackInvoiceDetail>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
            return ascmWmsBackInvoiceDetail;
        }
        public List<AscmWmsBackInvoiceDetail> GetList(string sql)
        {
            List<AscmWmsBackInvoiceDetail> list = null;
            try
            {
                IList<AscmWmsBackInvoiceDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceDetail>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsBackInvoiceDetail>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsBackInvoiceDetail> GetList(string sortName, string sortOrder, int? mainId, string queryWord, string whereOther)
        {
            return GetList(null, sortName, sortOrder, mainId, queryWord, whereOther);
        }
        public List<AscmWmsBackInvoiceDetail> GetList(YnPage ynPage, string sortName, string sortOrder, int? mainId, string queryWord, string whereOther)
        {
            List<AscmWmsBackInvoiceDetail> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }

                string sql = "from AscmWmsBackInvoiceDetail ";
                
                string where = "", whereQueryWord = "";
                if (mainId.HasValue)
                    where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " mainId=" + mainId);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmWmsBackInvoiceDetail> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceDetail>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWmsBackInvoiceDetail>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWmsBackInvoiceDetail>(ilist);
                    SetDeliveryOrderBatch(list);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmWmsBackInvoiceDetail> GetListByMainId(int mainId)
        {
            return GetList("", "", mainId, "", "");
        }

        public void Save(List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmWmsBackInvoiceDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
        }
        public void Save(AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmWmsBackInvoiceDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
        }
        public void Update(AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmWmsBackInvoiceDetail>(ascmWmsBackInvoiceDetail);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmWmsBackInvoiceDetail)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail = Get(id);
                Delete(ascmWmsBackInvoiceDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmWmsBackInvoiceDetail>(ascmWmsBackInvoiceDetail);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmWmsBackInvoiceDetail> listAscmWmsBackInvoiceDetail)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmWmsBackInvoiceDetail);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmWmsBackInvoiceDetail)", ex);
                throw ex;
            }
        }

        private void SetDeliveryOrderBatch(List<AscmWmsBackInvoiceDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsBackInvoiceDetail.batchId;
                }
                string sql = "from AscmDeliveryOrderBatch where id in (" + ids + ")";
                IList<AscmDeliveryOrderBatch> ilistAscmDeliveryOrderBatch = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql);
                if (ilistAscmDeliveryOrderBatch != null && ilistAscmDeliveryOrderBatch.Count > 0)
                {
                    List<AscmDeliveryOrderBatch> listAscmDeliveryOrderBatch = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderBatch>(ilistAscmDeliveryOrderBatch);
                    foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in list)
                    {
                        ascmWmsBackInvoiceDetail.ascmDeliveryOrderBatch = listAscmDeliveryOrderBatch.Find(e => e.id == ascmWmsBackInvoiceDetail.batchId);
                    }
                }
            }
        }
        private void SetMaterial(List<AscmWmsBackInvoiceDetail> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmWmsBackInvoiceDetail.materialId;
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmWmsBackInvoiceDetail ascmWmsBackInvoiceDetail in list)
                    {
                        ascmWmsBackInvoiceDetail.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmWmsBackInvoiceDetail.materialId);
                    }
                }
            }
        }
    }
}
