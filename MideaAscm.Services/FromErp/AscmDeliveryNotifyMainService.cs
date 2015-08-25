using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.FromErp.Entities;
using Oracle.DataAccess.Client;
using System.Data;
using MideaAscm.Dal.SupplierPreparation.Entities;
using System.Collections;

namespace MideaAscm.Services.FromErp
{
    public class AscmDeliveryNotifyMainService
    {
        private static AscmDeliveryNotifyMainService ascmDeliveryNotifyMainServices;
        public static AscmDeliveryNotifyMainService GetInstance()
        {
            //return ascmDeliveryNotifyMainServices ?? new AscmDeliveryNotifyMainService();
            if (ascmDeliveryNotifyMainServices == null)
                ascmDeliveryNotifyMainServices = new AscmDeliveryNotifyMainService();
            return ascmDeliveryNotifyMainServices;
        }
        public AscmDeliveryNotifyMain Get(int id)
        {
            AscmDeliveryNotifyMain ascmDeliveryNotify = null;
            try
            {
                ascmDeliveryNotify = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliveryNotifyMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliveryNotifyMain)", ex);
                throw ex;
            }
            return ascmDeliveryNotify;
        }
        public List<AscmDeliveryNotifyMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDeliveryNotifyMain> list = new List<AscmDeliveryNotifyMain>();
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
                //string sql = "from AscmDeliveryNotifyMain ";//where id>36614045
                //string detailCount = "select count(*) from AscmDeliveryNotifyDetail where mainId= a.id";
                //string containerBindNumber = "select t1.quantity from AscmContainerDelivery t1, AscmDeliveryOrderMain t2, AscmDeliveryOrderDetail t3, AscmDeliveryNotifyDetail t4  where t3.notifyDetailId = t4.id and t2.id = t3.mainId and t1.deliveryOrderBatchId=t2.batchId and t4.mainId = a.id";
                //string containerBindNumber = "select sum(t1.quantity) from AscmContainerDelivery t1, (select distinct t2.batchId, t4.mainId from AscmDeliveryOrderMain t2, AscmDeliveryOrderDetail t3, AscmDeliveryNotifyDetail t4  where t3.notifyDetailId = t4.id and t2.id = t3.mainId) t where t1.deliveryOrderBatchId=t.batchId and t.mainId = a.id";

                ////string containerBindNumber = "select sum(t1.quantity) from AscmContainerDelivery t1 where t1.deliveryOrderBatchId in "
                //                              + "(select distinct t2.batchId from AscmDeliveryOrderMain t2, AscmDeliveryOrderDetail t3, AscmDeliveryNotifyDetail t4 "
                //                              + "where t3.notifyDetailId = t4.id and t2.id = t3.mainId and t4.mainId = a.id)";
                //string receiveTime = "select t.acceptTime from AscmDeliBatSumMain t, AscmDeliBatSumDetail t1, AscmDeliveryOrderMain t2, AscmDeliveryOrderDetail t3, AscmDeliveryNotifyDetail t4  where t.id = t1.mainId and t1.batchId = t2.batchId and  t2.id = t3.mainId and t3.notifyDetailId = t4.id and t4.mainId = a.id";
                //string totalNumber = "select sum(deliveryQuantity) from AscmDeliveryOrderDetail where mainId= a.id";
                //string sql1 = "select new AscmDeliveryNotifyMain(a,(" + detailCount + "),(" + containerBindNumber + "),(" + receiveTime + ")) from AscmDeliveryNotifyMain a ";
                //string sql1 = "select new AscmDeliveryNotifyMain(a,(" + detailCount + ")) from AscmDeliveryNotifyMain a left j ";
                //string sql = "select count(a.*) from Ascm_Delivery_Notify_Main a left join Ascm_Delivery_Notify_Detail b on a.id = b.mainId left join Ascm_Delivery_Order_Detail c on b.id = c.notifydetailid left join Ascm_Delivery_Order_Main d on c.mainid = d.id left join ascm_deli_bat_order_link e on d.batchid = e.batchid  ";
                //string sql = "select count(1) from (select distinct a.id from Ascm_Delivery_Notify_Main a left join Ascm_Delivery_Notify_Detail b on a.id = b.mainId left join Ascm_Delivery_Order_Detail c on b.id = c.notifydetailid left join Ascm_Delivery_Order_Main d on c.mainid = d.id left join ascm_deli_bat_order_link e on d.batchid = e.batchid  left join ascm_deli_bat_sum_detail f on e.batchid = f.batchid left join ascm_deli_bat_sum_main g on f.mainid = g.id ";

                //保存ERP上获取的总接收数
                //List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain = AscmDeliveryNotifyMainService.GetInstance().GetList(null, "", "", "", whereOther);
                //if (listAscmDeliveryNotifyMain != null && listAscmDeliveryNotifyMain.Count > 0)
                //    AscmDeliveryNotifyMainService.GetInstance().Update(listAscmDeliveryNotifyMain);

                string sql = "select count(1) from (select distinct a.id from Ascm_Delivery_Notify_Main a ";
                string sql1 = "select distinct a.id ids,a.*,e.batchid,g.accepttime from Ascm_Delivery_Notify_Main a left join Ascm_Delivery_Notify_Detail b on a.id = b.mainId left join Ascm_Delivery_Order_Detail c on b.id = c.notifydetailid left join Ascm_Delivery_Order_Main d on c.mainid = d.id left join ascm_deli_bat_order_link e on d.batchid = e.batchid  left join ascm_deli_bat_sum_detail f on e.batchid = f.batchid left join ascm_deli_bat_sum_main g on f.mainid = g.id ";
                string sql_other = "select t.*, rownum rn from ({0}) t";
                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%' or description like '%" + queryWord.Trim() + "%')";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (where.IndexOf("a.totalReceiveQuantity") > -1)
                    where = where.Replace("a.totalReceiveQuantity", "nvl(a.totalReceiveQuantity,0)");

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where + ")";
                    sql1 += " where " + where;
                    sql1 = string.Format(sql_other, sql1);
                }
                if (whereOther == "")
                {
                    sql1 = "select * from (" + sql1 + " where rownum <=" + ynPage.currentPage * ynPage.pageSize + ") where rn >" + (ynPage.currentPage - 1) * ynPage.pageSize;
                }
                else
                {
                    sql1 = "select * from (" + sql1 + " where rownum <=" + ynPage.currentPage * ynPage.pageSize + ") where rn >" + (ynPage.currentPage - 1) * ynPage.pageSize;
                }
                ArrayList arrayList = (ArrayList)YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql);
                for (int i = 0; i < arrayList.Count; i++)
                {
                    Object[] o = (Object[])arrayList[i];
                    ynPage.recordCount = int.Parse(o[0].ToString());
                }
                //IList<AscmDeliveryNotifyMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryNotifyMain>(sql + sort);
                ArrayList arrayList1 = (ArrayList)YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql1);
                for (int i = 0; i < arrayList1.Count; i++)
                {
                     Object[] o = (Object[])arrayList1[i];
                    AscmDeliveryNotifyMain a = new AscmDeliveryNotifyMain();
                    int j = 1;
                    a.id = int.Parse(o[j + 0].ToString());
                    a.organizationId = int.Parse(o[j + 1].ToString() == "" ? "0" : o[j + 1].ToString());
                    a.createUser = o[j + 2].ToString();
                    a.createTime = o[j + 3].ToString();
                    a.modifyUser = o[j + 4].ToString();
                    a.modifyTime = o[j + 5].ToString();
                    a.docNumber = o[j + 6].ToString();
                    a.supplierId = int.Parse(o[j + 7].ToString() == "" ? "0" : o[j + 7].ToString());
                    a.warehouseId = o[j + 8].ToString();
                    a.materialId = int.Parse(o[j + 9].ToString() == "" ? "0" : o[j + 9].ToString());
                    a.releasedQuantity = int.Parse(o[j + 10].ToString() == "" ? "0" : o[j + 10].ToString());
                    a.promisedQuantity = int.Parse(o[j + 11].ToString() == "" ? "0" : o[j + 11].ToString());
                    a.deliveryQuantity = int.Parse(o[j + 12].ToString() == "" ? "0" : o[j + 12].ToString());
                    a.cancelQuantity = int.Parse(o[j + 13].ToString() == "" ? "0" : o[j + 13].ToString());
                    a.status = o[j + 14].ToString();
                    a.releasedTime = o[j + 15].ToString();
                    a.needTime = o[j + 16].ToString();
                    a.promisedTime = o[j + 17].ToString();
                    a.confirmTime = o[j + 18].ToString();
                    a.comments = o[j + 19].ToString();
                    a.purchasingAgentId = int.Parse(o[j + 20].ToString() == "" ? "0" : o[j + 20].ToString());
                    a.wipEntityId = int.Parse(o[j + 21].ToString() == "" ? "0" : o[j + 21].ToString());
                    a.departmentId = int.Parse(o[j + 22].ToString() == "" ? "0" : o[j + 22].ToString());
                    a.locationId = int.Parse(o[j + 23].ToString() == "" ? "0" : o[j + 23].ToString());
                    a.fdSourceType = o[j + 24].ToString();
                    a.appointmentStartTime = o[j + 25].ToString();
                    a.appointmentEndTime = o[j + 26].ToString();
                    a.totalReceiveQuantity = int.Parse(o[j + 28].ToString() == "" ? "0" : o[j + 28].ToString());
                    string sql2 = "select sum(a.quantity) from ascm_container_delivery a where a.DELIVERYORDERBATCHID = '" + o[j + 29].ToString()+"'";
                    ArrayList arrayList2 = (ArrayList)YnDaoHelper.GetInstance().nHibernateHelper.ExecuteReader(sql2);
                    for (int i1 = 0; i1 < arrayList2.Count; i1++)
                    {
                        Object[] o1 = (Object[])arrayList2[i1];
                        a.containerBindQuantity = int.Parse(o1[0].ToString() == "" ? "0" : o1[0].ToString());
                    }
                    a.receiveTime = o[j + 30].ToString();
                    list.Add(a);
                }
                //IList<AscmDeliveryNotifyMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryNotifyMain>(sql1 + sort, sql, ynPage);
                //if (ilist != null)
                //{
                    //list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryNotifyMain>(ilist);
                SetSupplier(list);
                SetMaterial(list);
                SetCreateUser(list);
                SetWipEntities(list);
                SetBomDepartments(list);
                SetHrLocations(list);
                SetLookupValues(list);
                getShipmentQuantities(list);
                //}
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliveryNotifyMain)", ex);
                throw ex;
            }
            return list;
        }
        private void SetSupplier(List<AscmDeliveryNotifyMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotifyMain.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    MideaAscm.Services.Base.AscmSupplierService.GetInstance().SetSupplierAddress(listAscmSupplier);
                    foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                    {
                        ascmDeliveryNotifyMain.ascmSupplier = listAscmSupplier.Find(e => e.id == ascmDeliveryNotifyMain.supplierId);
                    }
                }
            }
        }
        private void SetMaterial(List<AscmDeliveryNotifyMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotifyMain.materialId + "";
                }
                string sql = "from AscmMaterialItem where id in (" + ids + ")";
                IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                if (ilistAscmMaterialItem != null && ilistAscmMaterialItem.Count > 0)
                {
                    List<AscmMaterialItem> listAscmMaterialItem = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmMaterialItem>(ilistAscmMaterialItem);
                    MideaAscm.Services.Base.AscmMaterialItemService.GetInstance().SetBuyerEmployee(listAscmMaterialItem);
                    foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                    {
                        ascmDeliveryNotifyMain.ascmMaterialItem = listAscmMaterialItem.Find(e => e.id == ascmDeliveryNotifyMain.materialId);
                    }
                }
            }
        }
        private void SetWipEntities(List<AscmDeliveryNotifyMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotifyMain.wipEntityId + "";
                }
                string sql = "from AscmWipEntities where wipEntityId in (" + ids + ")";
                IList<AscmWipEntities> ilistAscmWipEntities = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipEntities>(sql);
                sql = "from AscmWipDiscreteJobs where wipEntityId in (" + ids + ")";
                IList<AscmWipDiscreteJobs> ilistAscmWipDiscreteJobs = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWipDiscreteJobs>(sql);
                List<AscmWipEntities> listAscmWipEntities = null;
                List<AscmWipDiscreteJobs> listAscmWipDiscreteJobs = null;
                if (ilistAscmWipEntities != null && ilistAscmWipEntities.Count > 0)
                    listAscmWipEntities = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipEntities>(ilistAscmWipEntities);
                if (ilistAscmWipDiscreteJobs != null && ilistAscmWipDiscreteJobs.Count > 0)
                    listAscmWipDiscreteJobs = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWipDiscreteJobs>(ilistAscmWipDiscreteJobs);
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (listAscmWipEntities != null)
                        ascmDeliveryNotifyMain.ascmWipEntities = listAscmWipEntities.Find(e => e.wipEntityId == ascmDeliveryNotifyMain.wipEntityId);
                    if (listAscmWipDiscreteJobs != null)
                        ascmDeliveryNotifyMain.ascmWipDiscreteJobs = listAscmWipDiscreteJobs.Find(e => e.wipEntityId == ascmDeliveryNotifyMain.wipEntityId);
                }
            }
        }
        private void SetBomDepartments(List<AscmDeliveryNotifyMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotifyMain.departmentId + "";
                }
                string sql = "from AscmBomDepartments where id in (" + ids + ")";
                IList<AscmBomDepartments> ilistAscmBomDepartments = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmBomDepartments>(sql);
                if (ilistAscmBomDepartments != null && ilistAscmBomDepartments.Count > 0)
                {
                    List<AscmBomDepartments> listAscmBomDepartments = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmBomDepartments>(ilistAscmBomDepartments);
                    foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                    {
                        ascmDeliveryNotifyMain.ascmBomDepartments = listAscmBomDepartments.Find(e => e.id == ascmDeliveryNotifyMain.departmentId);
                    }
                }
            }
        }
        private void SetHrLocations(List<AscmDeliveryNotifyMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDeliveryNotifyMain.locationId + "";
                }
                string sql = "from AscmHrLocationsAll where id in (" + ids + ")";
                IList<AscmHrLocationsAll> ilistAscmHrLocationsAll = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmHrLocationsAll>(sql);
                if (ilistAscmHrLocationsAll != null && ilistAscmHrLocationsAll.Count > 0)
                {
                    List<AscmHrLocationsAll> listAscmHrLocationsAll = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmHrLocationsAll>(ilistAscmHrLocationsAll);
                    foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                    {
                        ascmDeliveryNotifyMain.ascmHrLocationsAll = listAscmHrLocationsAll.Find(e => e.id == ascmDeliveryNotifyMain.locationId);
                    }
                }
            }
        }
        private void SetLookupValues(List<AscmDeliveryNotifyMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmDeliveryNotifyMain.fdSourceType + "'";
                }
                string sql = "from AscmFndLookupValues where type ='" + AscmFndLookupValues.AttributeCodeDefine.feedTypes + "' and code in (" + ids + ")";
                IList<AscmFndLookupValues> ilistAscmFndLookupValues = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmFndLookupValues>(sql);
                if (ilistAscmFndLookupValues != null && ilistAscmFndLookupValues.Count > 0)
                {
                    List<AscmFndLookupValues> listAscmFndLookupValues = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmFndLookupValues>(ilistAscmFndLookupValues);
                    foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                    {
                        ascmDeliveryNotifyMain.ascmFndLookupValues = listAscmFndLookupValues.Find(e => e.code == ascmDeliveryNotifyMain.fdSourceType);
                    }
                }
            }
        }
        private void SetCreateUser(List<AscmDeliveryNotifyMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmDeliveryNotifyMain.createUser + "'";
                }
                string sql = "from AscmUserInfo where extExpandType='erp' and extExpandId in (" + ids + ")";
                //sql = "from AscmEmployee where id in (" + sql + ")";
                IList<AscmUserInfo> ilistAscmUserInfo = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUserInfo>(sql);
                if (ilistAscmUserInfo != null && ilistAscmUserInfo.Count > 0)
                {
                    List<AscmUserInfo> listAscmUserInfo = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUserInfo>(ilistAscmUserInfo);
                    MideaAscm.Services.Base.AscmUserInfoService.GetInstance().SetEmployee(listAscmUserInfo);
                    foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in list)
                    {
                        ascmDeliveryNotifyMain.ascmUserInfo_createUser = listAscmUserInfo.Find(e => e.extExpandType == "erp" && e.extExpandId.ToString() == ascmDeliveryNotifyMain.createUser);
                    }
                }
            }
        }
        public void Update(AscmDeliveryNotifyMain ascmDeliveryNotify)
        {
            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDeliveryNotifyMain>(ascmDeliveryNotify);
                    tx.Commit();//正确执行提交
                }
                catch (Exception ex)
                {
                    tx.Rollback();//回滚
                    YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDeliveryNotifyMain)", ex);
                    throw ex;
                }
            }
        }
        public void Update(List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmDeliveryNotifyMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("更新失败(Update listAscmDeliveryNotifyMain)", ex);
                throw ex;
            }
        }

        // 执行ERP过程获取送货通知相关数量
        private void getShipmentQuantities(List<AscmDeliveryNotifyMain> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (AscmDeliveryNotifyMain notifyMain in list)
                    {
                        OracleParameter[] commandParameters = new OracleParameter[] {
                            // 送货通知头表ID
                            new OracleParameter {
                                ParameterName = "p_notify_header_id",
                                OracleDbType = OracleDbType.Int32,
                                Value = notifyMain.id,
                                Direction = ParameterDirection.Input
                            },
                            // 总接收
                            new OracleParameter {
                                ParameterName = "x_total_received_qty",
                                OracleDbType = OracleDbType.Int32,
                                Direction = ParameterDirection.Output
                            },
                            // 总退回给供应商数量
                            new OracleParameter {
                                ParameterName = "x_total_returned_qty",
                                OracleDbType = OracleDbType.Int32,
                                Direction = ParameterDirection.Output
                            },
                            // 接收未检验
                            new OracleParameter {
                                ParameterName = "x_in_received_qty",
                                OracleDbType = OracleDbType.Int32,
                                Direction = ParameterDirection.Output
                            },
                            // 接收未入库
                            new OracleParameter {
                                ParameterName = "x_in_inspected_qty",
                                OracleDbType = OracleDbType.Int32,
                                Direction = ParameterDirection.Output
                            },
                            // 入库量
                            new OracleParameter {
                                ParameterName = "x_in_subinv_qty",
                                OracleDbType = OracleDbType.Int32,
                                Direction = ParameterDirection.Output
                            }
                        };
                        MesInterface.AscmMesService.GetInstance().ExecuteOraProcedure("cux_fd_del_notify_qty_utl.get_shipment_quantities", ref commandParameters);
                        // 总接收
                        int total_received_qty = 0;
                        int.TryParse(commandParameters[1].Value.ToString(), out total_received_qty);
                        notifyMain.TOTAL_RECEIPT_QTY = total_received_qty;
                        // 总退回给供应商数量
                        int total_returned_qty = 0;
                        int.TryParse(commandParameters[2].Value.ToString(), out total_returned_qty);
                        notifyMain.TOTAL_RETURN_VENDOR_QTY = total_returned_qty;
                        // 接收未检验
                        int in_received_qty = 0;
                        int.TryParse(commandParameters[3].Value.ToString(), out in_received_qty);
                        notifyMain.IN_RECEIVED_QTY = in_received_qty;
                        // 接收未入库
                        int in_inspected_qty = 0;
                        int.TryParse(commandParameters[4].Value.ToString(), out in_inspected_qty);
                        notifyMain.IN_INSPECTED_QTY = in_inspected_qty;
                        // 入库量
                        int in_subinv_qty = 0;
                        int.TryParse(commandParameters[5].Value.ToString(), out in_subinv_qty);
                        notifyMain.SUBINV_QTY = in_subinv_qty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
