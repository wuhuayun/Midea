using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Dal.Warehouse.Entities;
using MideaAscm.Services.SupplierPreparation;
using MideaAscm.Services.Warehouse;

namespace MideaAscm.Services.FromErp
{
    public class AscmDeliveryOrderBatchService
    {
        private static AscmDeliveryOrderBatchService ascmDeliveryOrderBatchServices;
        public static AscmDeliveryOrderBatchService GetInstance()
        {
            //return ascmDeliveryOrderBatchServices ?? new AscmDeliveryOrderBatchService();
            if (ascmDeliveryOrderBatchServices == null)
                ascmDeliveryOrderBatchServices = new AscmDeliveryOrderBatchService();
            return ascmDeliveryOrderBatchServices;
        }
        public AscmDeliveryOrderBatch Get(int id)
        {
            AscmDeliveryOrderBatch ascmDeliveryNotify = null;
            try
            {
                ascmDeliveryNotify = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliveryOrderBatch>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliveryOrderBatch)", ex);
                throw ex;
            }
            return ascmDeliveryNotify;
        }
        public AscmDeliveryOrderBatch Get(int id, string sessionKey)
        {
            AscmDeliveryOrderBatch ascmDeliveryNotify = null;
            try
            {
                ascmDeliveryNotify = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliveryOrderBatch>(id, sessionKey);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliveryOrderBatch)", ex);
                throw ex;
            }
            return ascmDeliveryNotify;
        }
        public List<AscmDeliveryOrderBatch> GetList(string sql)
        {
            List<AscmDeliveryOrderBatch> list = null;
            try
            {
                IList<AscmDeliveryOrderBatch> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderBatch>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderBatch)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderBatch> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDeliveryOrderBatch> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                sort = ""; //不能在里面加order ，否则效率非常低
                string sql = "from AscmDeliveryOrderBatch";
                string _materialId = "select materialId from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id) and  rownum=1";
                string detailCount = "select count(*) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id)";
                string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId = a.id)";
                //string appointmentStartTime = "select appointmentStartTime from AscmDeliveryNotifyMain where id = (select mainId from AscmDeliveryNotifyDetail where id=(select notifyDetailId from AscmDeliveryOrderDetail where mainId = (select id from AscmDeliveryOrderMain where batchId =a.id)))";
                //string appointmentEndTime = "select appointmentEndTime from AscmDeliveryNotifyMain where id = (select mainId from AscmDeliveryNotifyDetail where id=(select notifyDetailId from AscmDeliveryOrderDetail where mainId = (select id from AscmDeliveryOrderMain where batchId =a.id)))";
                string sql1 = "select new AscmDeliveryOrderBatch(a,(" + detailCount + "),(" + totalNumber + "),(" + _materialId + ")) from AscmDeliveryOrderBatch a ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (barCode like '%" + queryWord.Trim() + "%')";
                    //whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }
                
                //IList<AscmDeliveryOrderBatch> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql + sort);
                IList<AscmDeliveryOrderBatch> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderBatch>(ilist);
                    SetSupplier(list);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderBatch)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderBatch> GetIncomingAcceptanceList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDeliveryOrderBatch> list = null;
            try
            {
                string sort = string.Empty;
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                sort = ""; //不能在里面加order ，否则效率非常低
                string sql = "from AscmDeliveryOrderBatch";
                string _materialId = "select materialId from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id) and  rownum=1";
                string detailCount = "select count(*) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id)";
                string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId = a.id)";
                string receivedQuantity = "select sum(receivedQuantity) from AscmDeliBatOrderLink where batchId=a.id";
                string sql1 = "select new AscmDeliveryOrderBatch(a,(" + detailCount + "),(" + totalNumber + "),(" + _materialId + "),(" + receivedQuantity + ")) from AscmDeliveryOrderBatch a ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }

                IList<AscmDeliveryOrderBatch> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderBatch>(ilist);
                    SetSupplier(list);
                    SetMaterial(list);
                    SetAssignWarelocation(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderBatch)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderBatch> GetSupplierCurrentList(int supplierId, int? deliveryBatchSumMainId)
        {
            List<AscmDeliveryOrderBatch> list = null;
            try
            {
                string _materialId = "select materialId from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id) and  rownum=1";
                string detailCount = "select count(*) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id)";
                string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId = a.id)";
                string sql = "select new AscmDeliveryOrderBatch(a,(" + detailCount + "),(" + totalNumber + "),(" + _materialId + ")) from AscmDeliveryOrderBatch a where supplierId=" + supplierId + " and status='" + AscmDeliveryOrderBatch.StatusDefine.open + "'";
                if (deliveryBatchSumMainId.HasValue)
                {
                    sql += " and id in (select batchId from AscmDeliBatSumDetail where mainId=" + deliveryBatchSumMainId.Value + ")";
                }
                //IList<AscmDeliveryOrderBatch> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql + sort);
                IList<AscmDeliveryOrderBatch> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderBatch>(ilist);
                    //SetSupplier(list);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderBatch)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliveryOrderBatch> GetMonitorList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDeliveryOrderBatch> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                sort = ""; //不能在里面加order ，否则效率非常低

                string sql = "from AscmDeliveryOrderBatch";
                string _materialId = "select materialId from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id) and  rownum=1";
                string detailCount = "select count(*) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId =a.id)";
                string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId = a.id)";
                string containerBindNumber = "select sum(quantity) from AscmContainerDelivery where deliveryOrderBatchId=a.id";
                string palletBindNumber = "select sum(quantity) from AscmPalletDelivery where deliveryOrderBatchId=a.id";
                string driverBindNumber = "select sum(quantity) from AscmDriverDelivery where deliveryOrderBatchId=a.id";
                string deliveryStatus = "select status from AscmDeliBatSumMain where id=(select mainId from AscmDeliBatSumDetail where batchId=a.id and rownum=1)";
                string sql1 = "select new AscmDeliveryOrderBatch(a,(" + detailCount + "),(" + totalNumber + "),(" + _materialId + "),(" + containerBindNumber + "),(" + palletBindNumber + "),(" + driverBindNumber + "),(" + deliveryStatus + ")) from AscmDeliveryOrderBatch a ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }

                IList<AscmDeliveryOrderBatch> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql1 + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderBatch>(ilist);
                    SetSupplier(list);
                    SetMaterial(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryOrderBatch)", ex);
                throw ex;
            }
            return list;
        }


        private void SetSupplier(List<AscmDeliveryOrderBatch> list)
        {
            if (list != null && list.Count > 0)
            {
                var supplierIds = list.Select(P => P.supplierId).Distinct();
                var count = supplierIds.Count();
                string ids = string.Empty;
                for (int i = 0; i < count; i++)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += supplierIds.ElementAt(i);
                    if ((i + 1) % 500 == 0 || (i + 1) == count)
                    {
                        if (!string.IsNullOrEmpty(ids))
                        {
                            string sql = "from AscmSupplier where id in (" + ids + ")";
                            List<AscmSupplier> listAscmSupplier = null;
                            IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                            if (ilistAscmSupplier != null)
                                listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                            if (listAscmSupplier != null && listAscmSupplier.Count > 0)
                            {
                                MideaAscm.Services.Base.AscmSupplierService.GetInstance().SetSupplierAddress(listAscmSupplier);
                                foreach (AscmDeliveryOrderBatch deliveryOrderBatch in list)
                                {
                                    AscmSupplier ascmSupplier = listAscmSupplier.Find(P => P.id == deliveryOrderBatch.supplierId);
                                    if (ascmSupplier != null)
                                        deliveryOrderBatch.ascmSupplier = ascmSupplier;
                                }
                            }
                        }
                        ids = string.Empty;
                    }
                }
            }
        }
        public void SetMaterial(List<AscmDeliveryOrderBatch> list)
        {
            if (list == null || list.Count == 0)
                return;

            var materialIds = list.Select(P => P.materialIdTmp).Distinct();
            var count = materialIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += materialIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string hql = "from AscmMaterialItem where id in (" + ids + ")";
                        IList<AscmMaterialItem> ilistMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(hql);
                        if (ilistMaterialItem != null && ilistMaterialItem.Count > 0)
                        {
                            List<AscmMaterialItem> listMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistMaterialItem);
                            foreach (AscmDeliveryOrderBatch deliveryOrderBatch in list)
                            {
                                AscmMaterialItem materialItem = listMaterialItem.Find(P => P.id == deliveryOrderBatch.materialIdTmp);
                                if (materialItem != null)
                                    deliveryOrderBatch.ascmMaterialItem = materialItem;
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        public void SetAssignWarelocation(List<AscmDeliveryOrderBatch> list)
        {
            if (list == null || list.Count == 0)
                return;

            var batchIds = list.Select(P => P.id).Distinct();
            var count = batchIds.Count();
            string ids = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(ids))
                    ids += ",";
                ids += batchIds.ElementAt(i);
                if ((i + 1) % 500 == 0 || (i + 1) == count)
                {
                    if (!string.IsNullOrEmpty(ids))
                    {
                        string hql = "select new AscmAssignWarelocation(aaw,aw.docNumber) from AscmAssignWarelocation aaw,AscmWarelocation aw";
                        string where = string.Empty;
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "aaw.warelocationId=aw.id");
                        where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "aaw.batchId in(" + ids + ")");
                        hql += " where " + where;
                        List<AscmAssignWarelocation> listAssignWarelocation = AscmAssignWarelocationService.GetInstance().GetList(hql);
                        if (listAssignWarelocation != null && listAssignWarelocation.Count > 0)
                        {
                            foreach (AscmDeliveryOrderBatch deliveryOrderBatch in list)
                            {
                                var result = listAssignWarelocation.Where(P => P.batchId == deliveryOrderBatch.id);
                                if (result != null)
                                {
                                    deliveryOrderBatch.assignWarelocation = string.Join("、", result.Select(P => P.locationDocNumber));
                                }
                            }
                        }
                    }
                    ids = string.Empty;
                }
            }
        }
        public void SetDeliveryNotifyMain(List<AscmDeliveryOrderBatch> list)
        {
            if (list != null && list.Count > 0)
            {
                List<AscmDeliveryOrderDetail> listAscmDeliveryOrderDetail = AscmDeliveryOrderDetailService.GetInstance().GetListByBatch(list);
                AscmDeliveryOrderDetailService.GetInstance().SetMain(listAscmDeliveryOrderDetail);
                foreach (AscmDeliveryOrderBatch deliveryOrderBatch in list)
                {
                    List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain = new List<AscmDeliveryNotifyMain>();

                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in listAscmDeliveryOrderDetail)
                    {
                        if (ascmDeliveryOrderDetail.ascmDeliveryOrderMain == null || ascmDeliveryOrderDetail.ascmDeliveryOrderMain.batchId != deliveryOrderBatch.id)
                            continue;
                        if (ascmDeliveryOrderDetail.ascmDeliveryNotifyDetail != null && ascmDeliveryOrderDetail.ascmDeliveryNotifyDetail.ascmDeliveryNotifyMain != null)
                        {
                            listAscmDeliveryNotifyMain.Add(ascmDeliveryOrderDetail.ascmDeliveryNotifyDetail.ascmDeliveryNotifyMain);
                        }
                    }
                    string appointmentStartTime = "", appointmentEndTime = "";
                    AscmDeliBatSumMainService.GetInstance().GetAppointmentTimeOriginal(listAscmDeliveryNotifyMain, ref appointmentStartTime, ref appointmentEndTime);
                    deliveryOrderBatch.appointmentStartTime = appointmentStartTime;
                    deliveryOrderBatch.appointmentEndTime = appointmentEndTime;
                }
            }
        }
    }
}
