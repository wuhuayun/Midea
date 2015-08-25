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
    public class AscmPalletDeliveryService
    {
        private static AscmPalletDeliveryService ascmPalletDeliveryServices;
        public static AscmPalletDeliveryService GetInstance()
        {
            if (ascmPalletDeliveryServices == null)
                ascmPalletDeliveryServices = new AscmPalletDeliveryService();
            return ascmPalletDeliveryServices;
        }
        #region base
        public AscmPalletDelivery Get(int id)
        {
            AscmPalletDelivery ascmPalletDelivery = null;
            try
            {
                ascmPalletDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmPalletDelivery>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmPalletDelivery)", ex);
                throw ex;
            }
            return ascmPalletDelivery;
        }
        public List<AscmPalletDelivery> GetList(string palletSn,int batSumMainId)
        {
            List<AscmPalletDelivery> list = null;
            try
            {
                //string sort = " order by sn ";
                string sql = "from AscmPalletDelivery where palletSn='" + palletSn + "' and batSumMainId=" + batSumMainId;

                IList<AscmPalletDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPalletDelivery>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmPalletDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmPalletDelivery> GetCurrentList(string containerSn)
        {
            List<AscmPalletDelivery> list = null;
            try
            {
                //string sort = " order by sn ";

                string sql = "from AscmPalletDelivery where containerSn='" + containerSn + "' order by id desc";

                IList<AscmPalletDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    AscmDeliBatSumMain ascmDeliBatSumMain = AscmDeliBatSumMainService.GetInstance().Get(ilist[0].batSumMainId);
                    if (ascmDeliBatSumMain != null && ascmDeliBatSumMain.status == AscmDeliBatSumMain.StatusDefine.outPlant)
                    {
                        sql = "from AscmPalletDelivery where batSumMainId =" + ilist[0].batSumMainId + "  and containerSn='" + containerSn + "'";
                        ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql);
                        if (ilist != null)
                        {
                            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPalletDelivery>(ilist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmPalletDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmPalletDelivery> GetCurrentList(string containerSn, string sessionKey)
        {
            List<AscmPalletDelivery> list = null;
            try
            {
                //string sort = " order by sn ";

                string sql = "from AscmPalletDelivery where containerSn='" + containerSn + "' order by id desc";

                IList<AscmPalletDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql, sessionKey);
                if (ilist != null && ilist.Count > 0)
                {
                    AscmDeliBatSumMain ascmDeliBatSumMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliBatSumMain>(ilist[0].batSumMainId, sessionKey);
                    if (ascmDeliBatSumMain != null && ascmDeliBatSumMain.status == AscmDeliBatSumMain.StatusDefine.outPlant)
                    {
                        sql = "from AscmPalletDelivery where batSumMainId =" + ilist[0].batSumMainId + "  and containerSn='" + containerSn + "'";
                        ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql, sessionKey);
                        if (ilist != null)
                        {
                            list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPalletDelivery>(ilist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmPalletDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmPalletDelivery> GetListByDeliverySumMainId(int mainId)
        {
            List<AscmPalletDelivery> list = null;
            try
            {
                //string sort = " order by sn ";
                string sql = "from AscmPalletDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId=" + mainId + ")";

                IList<AscmPalletDelivery> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPalletDelivery>(ilist);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmPalletDelivery)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmPalletDelivery> listAscmPalletDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmPalletDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        public void Save(AscmPalletDelivery ascmPalletDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmPalletDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        public void Update(AscmPalletDelivery ascmPalletDelivery, string sessionKey = null)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession(sessionKey).BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmPalletDelivery>(ascmPalletDelivery, sessionKey);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmPalletDelivery)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        //public void Delete(int id)
        //{
        //    try
        //    {
        //        AscmPalletDelivery ascmPalletDelivery = Get(id);
        //        Delete(ascmPalletDelivery);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void Delete(AscmPalletDelivery ascmPalletDelivery)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmPalletDelivery>(ascmPalletDelivery);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmPalletDelivery> listAscmPalletDelivery)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmPalletDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        private void SetMaterial(List<AscmPalletDelivery> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmPalletDelivery ascmPalletDelivery in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmPalletDelivery.materialId + "";
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    foreach (AscmPalletDelivery ascmPalletDelivery in list)
                    {
                        ascmPalletDelivery.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmPalletDelivery.materialId);
                    }
                }
            }
        }
        #endregion

        #region 应用
        public AscmPalletDelivery Add(string palletSn, int batSumMainId, int deliveryOrderBatchId, int materialId, decimal quantity)
        {
            try
            {
                AscmPalletDelivery ascmPalletDelivery = new AscmPalletDelivery();
                ascmPalletDelivery.organizationId = 0;
                ascmPalletDelivery.createUser = "";
                ascmPalletDelivery.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmPalletDelivery.modifyUser = "";
                ascmPalletDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmPalletDelivery.palletSn = palletSn;
                ascmPalletDelivery.batSumMainId = batSumMainId;
                ascmPalletDelivery.deliveryOrderBatchId = deliveryOrderBatchId;
                ascmPalletDelivery.deliveryOrderId = 0;
                ascmPalletDelivery.materialId = materialId;
                ascmPalletDelivery.quantity = quantity;
                //ascmPalletDelivery.status = "";
                ascmPalletDelivery.memo = "";

                int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmPalletDelivery");
                maxId++;
                ascmPalletDelivery.id = maxId;
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmPalletDelivery);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        throw ex;
                    }
                }
                return ascmPalletDelivery;
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        public void Update(int id, decimal quantity)
        {
            try
            {
                AscmPalletDelivery ascmPalletDelivery = Get(id);
                if (ascmPalletDelivery == null)
                    throw new Exception("编号错误，没有此编号托盘!");
                ascmPalletDelivery.modifyUser = "";
                ascmPalletDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                ascmPalletDelivery.quantity = quantity;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmPalletDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmPalletDelivery ascmPalletDelivery = Get(id);
                if (ascmPalletDelivery == null)
                    throw new Exception("编号错误，没有此编号托盘!");

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Delete(ascmPalletDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        public void Clear(string palletSn, int batSumMainId)
        {
            try
            {
                List<AscmPalletDelivery> listAscmPalletDelivery = GetList(palletSn, batSumMainId);
                if (listAscmPalletDelivery == null)
                    throw new Exception("编号错误，没有此编号托盘!");

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmPalletDelivery);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmPalletDelivery)", ex);
                throw ex;
            }
        }
        #endregion
    }
}
