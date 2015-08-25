using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Services.FromErp;

namespace MideaAscm.Services.SupplierPreparation
{
    public class AscmDeliBatOrderLinkService
    {
        private static AscmDeliBatOrderLinkService ascmDeliBatOrderLinkServices;
        public static AscmDeliBatOrderLinkService GetInstance()
        {
            if (ascmDeliBatOrderLinkServices == null)
                ascmDeliBatOrderLinkServices = new AscmDeliBatOrderLinkService();
            return ascmDeliBatOrderLinkServices;
        }

        public AscmDeliBatOrderLink Get(int id)
        {
            AscmDeliBatOrderLink ascmDeliBatOrderLink = null;
            try
            {
                ascmDeliBatOrderLink = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliBatOrderLink>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatOrderLink)", ex);
                throw ex;
            }
            return ascmDeliBatOrderLink;
        }
        public List<AscmDeliBatOrderLink> GetList(string sql)
        {
            List<AscmDeliBatOrderLink> list = null;
            try
            {
                IList<AscmDeliBatOrderLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatOrderLink>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliBatOrderLink>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliBatOrderLink)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliBatOrderLink> GetListByBatch(int batchId)
        {
            List<AscmDeliBatOrderLink> list = null;
            try
            {
                List<AscmDeliveryOrderDetail> listDeliOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetListByBatch(batchId);
                if (listDeliOrderDetail != null)
                {
                    string ids = "";
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in listDeliOrderDetail)
                    {
                        if (ids != "")
                            ids += ",";
                        ids += ascmDeliveryOrderDetail.id;
                    }
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string sql = "from AscmDeliBatOrderLink ";
                        string where = "";
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, " id in(" + ids + ")");
                        if (!string.IsNullOrEmpty(where))
                            sql += " where " + where;
                        IList<AscmDeliBatOrderLink> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatOrderLink>(sql);
                        if (ilist != null && ilist.Count > 0)
                        {
                            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliBatOrderLink>(ilist);
                            foreach (AscmDeliBatOrderLink deliBatOrderLink in list)
                            {
                                AscmDeliveryOrderDetail deliveryOrderDetail = listDeliOrderDetail.Find(P => P.id == deliBatOrderLink.id);
                                deliBatOrderLink.ascmDeliveryOrderDetail = deliveryOrderDetail;
                                deliBatOrderLink.batchId = batchId;
                                deliBatOrderLink.batchBarCode = deliveryOrderDetail.mainBatchBarCode;
                                deliBatOrderLink.batchDocNumber = deliveryOrderDetail.mainBatchDocNumber;
                                deliBatOrderLink.warehouseId = deliveryOrderDetail.warehouseId;
                                deliBatOrderLink.mainId = deliveryOrderDetail.mainId;
                            }
                        }
                        else
                        {
                            list = new List<AscmDeliBatOrderLink>();
                            foreach (AscmDeliveryOrderDetail deliveryOrderDetail in listDeliOrderDetail)
                            {
                                AscmDeliBatOrderLink ascmDeliBatOrderLink = new AscmDeliBatOrderLink();
                                ascmDeliBatOrderLink.id = deliveryOrderDetail.id;
                                ascmDeliBatOrderLink.ascmDeliveryOrderDetail = deliveryOrderDetail;
                                ascmDeliBatOrderLink.batchId = batchId;
                                ascmDeliBatOrderLink.batchBarCode = deliveryOrderDetail.mainBatchBarCode;
                                ascmDeliBatOrderLink.batchDocNumber = deliveryOrderDetail.mainBatchDocNumber;
                                ascmDeliBatOrderLink.warehouseId = deliveryOrderDetail.warehouseId;
                                ascmDeliBatOrderLink.mainId = deliveryOrderDetail.mainId;
                                ascmDeliBatOrderLink.deliveryQuantity = deliveryOrderDetail.deliveryQuantity;
                                ascmDeliBatOrderLink.receivedQuantity = deliveryOrderDetail.deliveryQuantity;
                                list.Add(ascmDeliBatOrderLink);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliBatOrderLink)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmDeliBatOrderLink> listAscmDeliBatOrderLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmDeliBatOrderLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDeliBatOrderLink)", ex);
                throw ex;
            }
        }
        public void Save(AscmDeliBatOrderLink ascmDeliBatOrderLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDeliBatOrderLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDeliBatOrderLink)", ex);
                throw ex;
            }
        }
        public void Update(AscmDeliBatOrderLink ascmDeliBatOrderLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDeliBatOrderLink>(ascmDeliBatOrderLink);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDeliBatOrderLink)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmDeliBatOrderLink)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmDeliBatOrderLink ascmDeliBatOrderLink = Get(id);
                Delete(ascmDeliBatOrderLink);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmDeliBatOrderLink ascmDeliBatOrderLink)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmDeliBatOrderLink>(ascmDeliBatOrderLink);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDeliBatOrderLink)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmDeliBatOrderLink> listAscmDeliBatOrderLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDeliBatOrderLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDeliBatOrderLink)", ex);
                throw ex;
            }
        }
        public void SaveOrUpdate(List<AscmDeliBatOrderLink> listAscmDeliBatOrderLink)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAscmDeliBatOrderLink);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDeliBatOrderLink)", ex);
                throw ex;
            }
        }

        public void SetWipEntities(List<AscmDeliBatOrderLink> list)
        {
            if (list == null && list.Count == 0)
                return;

            var mainIds = list.Select(P => P.mainId).Distinct();
            var count = mainIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += mainIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string hql = "select new AscmDeliveryOrderMain(m.id,m.wipEntityId,e.name,j.statusType) from AscmDeliveryOrderMain m,AscmWipEntities e,AscmWipDiscreteJobs j "
                                   + "where m.wipEntityId=e.wipEntityId "
                                   + "and m.wipEntityId=j.wipEntityId "
                                   + "and m.id in(" + ids + ")";
                        IList<AscmDeliveryOrderMain> ilistDeliveryOrderMain = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderMain>(hql);
                        if (ilistDeliveryOrderMain != null && ilistDeliveryOrderMain.Count > 0)
                        {
                            List<AscmDeliveryOrderMain> listDeliveryOrderMain = ilistDeliveryOrderMain.ToList();
                            foreach (AscmDeliBatOrderLink deliBatOrderLink in list)
                            {
                                AscmDeliveryOrderMain deliveryOrderMain = listDeliveryOrderMain.Find(P => P.id == deliBatOrderLink.mainId);
                                if (deliveryOrderMain != null)
                                {
                                    deliBatOrderLink.wipEntityId = deliveryOrderMain.wipEntityId;
                                    deliBatOrderLink.wipEntityName = deliveryOrderMain.wipEntity;
                                    deliBatOrderLink.wipEntityStatus = deliveryOrderMain.wipEntityStatus;
                                }
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
    }
}
