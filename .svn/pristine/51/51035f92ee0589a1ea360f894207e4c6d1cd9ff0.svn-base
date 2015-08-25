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
    public class AscmDriverDeliveryService
    {
        private static AscmDriverDeliveryService ascmDriverDeliveryServices;
        public static AscmDriverDeliveryService GetInstance()
        {
            if (ascmDriverDeliveryServices == null)
                ascmDriverDeliveryServices = new AscmDriverDeliveryService();
            return ascmDriverDeliveryServices;
        }
        #region base
        public AscmDriverDelivery Get(int id)
        {
            AscmDriverDelivery ascmDriverDelivery = null;
            try
            {
                ascmDriverDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDriverDelivery>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDriverDelivery)", ex);
                throw ex;
            }
            return ascmDriverDelivery;
        }
        public List<AscmDriverDelivery> GetListByDriverSn(string driverSn, int batSumMainId)
        {
            List<AscmDriverDelivery> list = null;
            try
            {
                //string sort = " order by sn ";
                string sql = "from AscmDriverDelivery where driverSn='" + driverSn + "' and batSumMainId=" + batSumMainId;

                IList<AscmDriverDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriverDelivery>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriverDelivery>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDriverDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDriverDelivery> GetListByDeliverySumMainId(int mainId)
        {
            List<AscmDriverDelivery> list = null;
            try
            {
                //string sort = " order by sn ";
                string sql = "from AscmDriverDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId=" + mainId + ")";

                IList<AscmDriverDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriverDelivery>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriverDelivery>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDriverDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmDriverDelivery> listAscmDriverDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmDriverDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        public void Save(AscmDriverDelivery ascmDriverDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDriverDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        public void Update(AscmDriverDelivery ascmDriverDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDriverDelivery>(ascmDriverDelivery);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDriverDelivery)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        //public void Delete(int id)
        //{
        //    try
        //    {
        //        AscmDriverDelivery ascmDriverDelivery = Get(id);
        //        Delete(ascmDriverDelivery);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void Delete(AscmDriverDelivery ascmDriverDelivery)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmDriverDelivery>(ascmDriverDelivery);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmDriverDelivery> listAscmDriverDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDriverDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        private void SetMaterial(List<AscmDriverDelivery> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDriverDelivery ascmDriverDelivery in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDriverDelivery.materialId + "";
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmDriverDelivery ascmDriverDelivery in list)
                    {
                        ascmDriverDelivery.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmDriverDelivery.materialId);
                    }
                }
            }
        }
        #endregion

        #region 应用
        public AscmDriverDelivery Add(string driverSn, int batSumMainId, int deliveryOrderBatchId, int materialId, decimal quantity)
        {
            try
            {
                AscmDriverDelivery ascmDriverDelivery = new AscmDriverDelivery();
                ascmDriverDelivery.organizationId = 0;
                ascmDriverDelivery.createUser = "";
                ascmDriverDelivery.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmDriverDelivery.modifyUser = "";
                ascmDriverDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmDriverDelivery.driverSn = driverSn;
                ascmDriverDelivery.batSumMainId = batSumMainId;
                ascmDriverDelivery.deliveryOrderBatchId = deliveryOrderBatchId;
                ascmDriverDelivery.deliveryOrderId = 0;
                ascmDriverDelivery.materialId = materialId;
                ascmDriverDelivery.quantity = quantity;
                //ascmDriverDelivery.status = "";
                ascmDriverDelivery.memo = "";

                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmDriverDelivery");
                maxId++;
                ascmDriverDelivery.id = maxId;
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDriverDelivery);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
                return ascmDriverDelivery;
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        public void Update(int id, decimal quantity)
        {
            try
            {
                AscmDriverDelivery ascmDriverDelivery = Get(id);
                if (ascmDriverDelivery == null)
                    throw new Exception("编号错误，没有此编号托盘!");
                ascmDriverDelivery.modifyUser = "";
                ascmDriverDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmDriverDelivery.quantity = quantity;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDriverDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmDriverDelivery ascmDriverDelivery = Get(id);
                if (ascmDriverDelivery == null)
                    throw new Exception("编号错误，没有此编号托盘!");

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmDriverDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        public void Clear(string driverSn, int batSumMainId)
        {
            try
            {
                List<AscmDriverDelivery> listAscmDriverDelivery = GetListByDriverSn(driverSn, batSumMainId);
                if (listAscmDriverDelivery == null)
                    throw new Exception("编号错误，没有此编号托盘!");

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDriverDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriverDelivery)", ex);
                throw ex;
            }
        }
        #endregion
    }
}
