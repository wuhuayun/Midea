﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Dal.FromErp.Entities;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Services.SupplierPreparation
{
    public class AscmDeliBatSumMainService
    {
        #region base
        private static AscmDeliBatSumMainService ascmDeliBatSumMainServices;
        public static AscmDeliBatSumMainService GetInstance()
        {
            if (ascmDeliBatSumMainServices == null)
                ascmDeliBatSumMainServices = new AscmDeliBatSumMainService();
            return ascmDeliBatSumMainServices;
        }

        public int GetMaxId()
        {
            int maxId = 0;
            try
            {
                maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmDeliBatSumMain");
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain MaxId)", ex);
                throw ex;
            }
            return maxId;
        }
        public AscmDeliBatSumMain Get(int id)
        {
            AscmDeliBatSumMain ascmDeliBatSumMain = null;
            try
            {
                ascmDeliBatSumMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliBatSumMain>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return ascmDeliBatSumMain;
        }
        public AscmDeliBatSumMain Get(string hql)
        {
            AscmDeliBatSumMain ascmDeliBatSumMain = null;
            try
            {
                IList<AscmDeliBatSumMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.FindTop<AscmDeliBatSumMain>(hql, 1);
                if (ilist != null && ilist.Count > 0)
                    ascmDeliBatSumMain = ilist[0];
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return ascmDeliBatSumMain;
        }
        public AscmDeliBatSumMain Get(int id, string sessionKey)
        {
            AscmDeliBatSumMain ascmDeliBatSumMain = null;
            try
            {
                ascmDeliBatSumMain = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDeliBatSumMain>(id, sessionKey);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return ascmDeliBatSumMain;
        }
        public List<AscmDeliBatSumMain> GetByDriverId(int driverId)
        {
            List<AscmDeliBatSumMain> list = null;
            try
            {
                //string sql = "from AscmDeliBatSumMain where driverId=" + driverId;
                //IList<AscmDeliBatSumMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql);
                //if (ilist != null && ilist.Count > 0)
                //    ascmDeliBatSumMain = ilist[0];

                string sort = " order by id desc";

                string sql = "from AscmDeliBatSumMain";
                string where = string.Empty;
                string whereDriver = "driverId=" + driverId;
                string whereStatus = "status='" + AscmDeliBatSumMain.StatusDefine.confirm + "'";

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereDriver);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereStatus);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmDeliBatSumMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql + sort);
                list = ilist.ToList<AscmDeliBatSumMain>();
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return list;
        }
        public AscmDeliBatSumMain GetByDriverId(int driverId, string status)
        {
            AscmDeliBatSumMain ascmDeliBatSumMain = null;
            try
            {
                //string sql = "from AscmDeliBatSumMain where driverId=" + driverId;
                //IList<AscmDeliBatSumMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql);
                //if (ilist != null && ilist.Count > 0)
                //    ascmDeliBatSumMain = ilist[0];

                string sort = " order by id desc";

                string sql = "from AscmDeliBatSumMain";
                string where = string.Empty;
                string whereDriver = "driverId=" + driverId;
                string whereStatus = "status='" + status + "'";

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereDriver);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereStatus);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmDeliBatSumMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.FindTop<AscmDeliBatSumMain>(sql + sort, 1);
                if (ilist != null && ilist.Count > 0)
                {
                    ascmDeliBatSumMain = ilist[0];
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return ascmDeliBatSumMain;
        }
        public List<AscmDeliBatSumMain> GetList(string sql)
        {
            List<AscmDeliBatSumMain> list = null;
            try
            {
                IList<AscmDeliBatSumMain> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliBatSumMain>(ilist);
                    SetSupplier(list);
                    SetDriver(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliBatSumMain> GetList(YnPage ynPage, string hql, string newHql)
        {
            List<AscmDeliBatSumMain> list = null;
            try
            {
                IList<AscmDeliBatSumMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(newHql, hql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(newHql);
                if (ilist != null)
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliBatSumMain>(ilist);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliBatSumMain> GetSupplierCurrentList(int supplierId)
        {
            string whereOther = "";
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " (status is null or status='' or status='" + AscmDeliBatSumMain.StatusDefine.unConfirm + "')");
            whereOther = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(whereOther, " supplierId=" + supplierId);
            return GetList("", "", "", whereOther);
        }
        public List<AscmDeliBatSumMain> GetList(string sortName, string sortOrder, string queryWord, string whereOther)
        {
            return GetList(null, sortName, sortOrder, queryWord, whereOther);
        }
        public List<AscmDeliBatSumMain> GetList(YnPage ynPage, string sortName, string sortOrder, string whereOther)
        {
            List<AscmDeliBatSumMain> list = null;
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
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                string sql = "from AscmDeliBatSumMain ";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmDeliBatSumMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliBatSumMain>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDeliBatSumMain> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDeliBatSumMain> list = null;
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
                    whereQueryWord = " (docNumber like '%" + queryWord.Trim() + "%' or barcode like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                string sql = "from AscmDeliBatSumMain ";//distinct(containerSn)
                string totalNumber = "select sum(totalNumber) from AscmDeliBatSumDetail where mainId = a.id";
                string inWarehouseContainerBindNumber = "select sum(quantity) from AscmContainerDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId = a.id ) and batSumMainId=a.id and status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'";
                string containerBindNumber = "select sum(quantity) from AscmContainerDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId = a.id ) and batSumMainId=a.id";
                string palletBindNumber = "select sum(quantity) from AscmPalletDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId = a.id) and batSumMainId=a.id";
                string driverBindNumber = "select sum(quantity) from AscmDriverDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId = a.id) and batSumMainId=a.id";
                string containerNumber = "select count(distinct containerSn) from AscmContainerDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId = a.id ) and batSumMainId=a.id";
                string inWarehouseContainerNumber = "select count(distinct containerSn) from AscmContainerDelivery where deliveryOrderBatchId in (select batchId from AscmDeliBatSumDetail where mainId = a.id ) and batSumMainId=a.id and status='" + AscmContainerDelivery.StatusDefine.inWarehouseDoor + "'";
                string deliBatNumber = "select count(*) from AscmDeliBatSumDetail where mainId = a.id";
                string sql1 = "select new AscmDeliBatSumMain(a,(" + totalNumber + "),(" + inWarehouseContainerBindNumber + "),(" + containerBindNumber + "),(" + palletBindNumber + "),(" + driverBindNumber + "),(" + containerNumber + "),(" + inWarehouseContainerNumber + "),(" + deliBatNumber + ")) from AscmDeliBatSumMain a ";
                //string sql1 = "select new AscmDeliBatSumMain(a,(" + totalNumber + "),(" + containerBindNumber + "),(" + palletBindNumber + "),(" + driverBindNumber + ")) from AscmDeliBatSumMain a ";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                    sql1 += " where " + where;
                }
                IList<AscmDeliBatSumMain> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql1 + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumMain>(sql1 + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliBatSumMain>(ilist);
                    SetSupplier(list);
                    SetDriver(list);
                    SetAcceptTime(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmDeliBatSumMain> listAscmDeliBatSumMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmDeliBatSumMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDeliBatSumMain)", ex);
                throw ex;
            }
        }
        public void Save(AscmDeliBatSumMain ascmDeliBatSumMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDeliBatSumMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDeliBatSumMain)", ex);
                throw ex;
            }
        }
        public void Update(AscmDeliBatSumMain ascmDeliBatSumMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDeliBatSumMain>(ascmDeliBatSumMain);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDeliBatSumMain)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmDeliBatSumMain)", ex);
                throw ex;
            }
        }
        public void Update(List<AscmDeliBatSumMain> listAscmDeliBatSumMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmDeliBatSumMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDeliBatSumMain)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmDeliBatSumMain ascmDeliBatSumMain = Get(id);
                Delete(ascmDeliBatSumMain);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmDeliBatSumMain ascmDeliBatSumMain)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmDeliBatSumMain>(ascmDeliBatSumMain);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDeliBatSumMain)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmDeliBatSumMain> listAscmDeliBatSumMain)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDeliBatSumMain);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDeliBatSumMain)", ex);
                throw ex;
            }
        }

        public void SetSupplier(List<AscmDeliBatSumMain> list)
        {
            if (list != null && list.Count > 0)
            {
                //string ids = string.Empty;
                //foreach (AscmDeliBatSumMain ascmDeliBatSumMain in list)
                //{
                //    if (!string.IsNullOrEmpty(ids))
                //        ids += ",";
                //    ids += ascmDeliBatSumMain.supplierId;
                //}
                //string sql = "from AscmSupplier where id in (" + ids + ")";
                string supplierIds = string.Join(",", list.Select(P => P.supplierId).Distinct());
                string whereSupplierIds = MideaAscm.Services.GetMaterialManage.AscmCommonHelperService.GetInstance().IsJudgeListCount(supplierIds, "id");
                string sql = "from AscmSupplier where " + whereSupplierIds;
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    MideaAscm.Services.Base.AscmSupplierService.GetInstance().SetSupplierAddress(listAscmSupplier);
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain in list)
                    {
                        ascmDeliBatSumMain.ascmSupplier = listAscmSupplier.Find(e => e.id == ascmDeliBatSumMain.supplierId);
                    }
                }
            }
        }
        public void SetDriver(List<AscmDeliBatSumMain> list)
        {
            if (list != null && list.Count > 0)
            {
                //string ids = string.Empty;
                //foreach (AscmDeliBatSumMain ascmDeliBatSumMain in list)
                //{
                //    if (!string.IsNullOrEmpty(ids))
                //        ids += ",";
                //    ids += ascmDeliBatSumMain.driverId;
                //}
                //string sql = "from AscmDriver where id in (" + ids + ")";
                string driverIds = string.Join(",", list.Select(P => P.driverId).Distinct());
                string whereDriverIds = MideaAscm.Services.GetMaterialManage.AscmCommonHelperService.GetInstance().IsJudgeListCount(driverIds, "id");
                string sql = "from AscmDriver where " + whereDriverIds;
                IList<AscmDriver> ilistAscmDriver = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriver>(sql);
                if (ilistAscmDriver != null && ilistAscmDriver.Count > 0)
                {
                    List<AscmDriver> listAscmDriver = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriver>(ilistAscmDriver);
                    foreach (AscmDeliBatSumMain ascmDeliBatSumMain in list)
                    {
                        ascmDeliBatSumMain.ascmDriver = listAscmDriver.Find(e => e.id == ascmDeliBatSumMain.driverId);
                    }
                }
            }
        }
        public void SetAcceptTime(List<AscmDeliBatSumMain> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Join(",", list.Select(P => P.id));
                string whereIds = MideaAscm.Services.GetMaterialManage.AscmCommonHelperService.GetInstance().IsJudgeListCount(ids, "d.mainId");
                string hql = "select new AscmDeliBatSumDetail(d.mainId,min(l.createTime)) from AscmDeliBatSumDetail d,AscmMesInteractiveLog l";
                string where = string.Empty;
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "d.batchId=l.billId");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereIds);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.billType='" + MideaAscm.Dal.Warehouse.Entities.AscmMesInteractiveLog.BillTypeDefine.incAccSystem + "'");
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, "l.returnCode='0'");
                hql += " where " + where;
                hql += " group by d.mainId";
                List<AscmDeliBatSumDetail> listDetail = AscmDeliBatSumDetailService.GetInstance().GetList(hql);
                if (listDetail != null && listDetail.Count > 0)
                {
                    foreach (AscmDeliBatSumMain deliBatSumMain in list)
                    {
                        AscmDeliBatSumDetail deliBatSumDetail = listDetail.Find(P => P.mainId == deliBatSumMain.id);
                        if (deliBatSumDetail != null)
                            deliBatSumMain.acceptTime = deliBatSumDetail.acceptTime;
                    }
                }
            }
        }
        #endregion

        #region 业务
        public void GetAppointmentTimeOriginal(List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain, ref string appointmentStartTime, ref string appointmentEndTime)
        {
            if (listAscmDeliveryNotifyMain != null)
            {
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in listAscmDeliveryNotifyMain)
                {
                    if (DateTime.TryParse(ascmDeliveryNotifyMain.appointmentStartTime, out startTime))
                    {
                        if (appointmentStartTime == "" || Convert.ToDateTime(appointmentStartTime) > startTime)
                        {
                            appointmentStartTime = ascmDeliveryNotifyMain.appointmentStartTime;
                            appointmentEndTime = ascmDeliveryNotifyMain.appointmentEndTime;
                        }
                    }
                }
            }
        }
        /*
        public void GetAppointmentTime(List<AscmDeliveryNotifyMain> listAscmDeliveryNotifyMain, ref string appointmentStartTime, ref string appointmentEndTime)
        {
            //根据送货通知生成预约时间，取最早的通知时间
            if (listAscmDeliveryNotifyMain != null)
            {
                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                foreach (AscmDeliveryNotifyMain ascmDeliveryNotifyMain in listAscmDeliveryNotifyMain)
                {
                    if (DateTime.TryParse(ascmDeliveryNotifyMain.appointmentStartTime, out startTime))
                    {
                        if (appointmentStartTime == "" || Convert.ToDateTime(appointmentStartTime) > startTime)
                        {
                            appointmentStartTime = ascmDeliveryNotifyMain.appointmentStartTime;
                            appointmentEndTime = ascmDeliveryNotifyMain.appointmentEndTime;
                        }
                    }
                }
                //2013.11.28
                //批单最早到达时间和最晚到达时间一样的，最早到达时间向后取半点（例如：11：58取12：00，11:30取11:30）为Te,最晚到达时间为Te + Tc(当前Tc为4小时)
                double supplierPassDuration = (double)0.5;
                double.TryParse(YnFrame.Services.YnParameterService.GetInstance().GetValue(MideaAscm.Dal.Base.MyParameter.supplierPassDuration), out supplierPassDuration);
                if (supplierPassDuration < 0.5)
                    supplierPassDuration = 0.5;

                if (!string.IsNullOrEmpty(appointmentStartTime) && DateTime.TryParse(appointmentStartTime, out startTime) &&
                    !string.IsNullOrEmpty(appointmentEndTime) && DateTime.TryParse(appointmentEndTime, out endTime))
                {
                    if (startTime.ToString("yyyy-MM-dd HH:mm") == endTime.ToString("yyyy-MM-dd HH:mm"))
                    {
                        if (startTime.Minute > 30)
                            startTime = startTime.AddMinutes(30);
                        if (startTime > Convert.ToDateTime(startTime.ToString("yyyy-MM-dd 19:30")))
                            startTime = Convert.ToDateTime(startTime.AddDays(1).ToString("yyyy-MM-dd") + " 08:00");
                        appointmentStartTime = startTime.ToString("yyyy-MM-dd HH:") + (startTime.Minute >= 30 ? "30" : "00");
                        appointmentEndTime = startTime.AddHours(supplierPassDuration).ToString("yyyy-MM-dd HH:") + (startTime.Minute >= 30 ? "30" : "00");
                    }
                }
                //按30分钟取整
                if (!string.IsNullOrEmpty(appointmentStartTime) && DateTime.TryParse(appointmentStartTime, out startTime))
                {
                    appointmentStartTime = startTime.ToString("yyyy-MM-dd HH:") + (startTime.Minute >= 30 ? "30" : "00");
                }
                if (!string.IsNullOrEmpty(appointmentEndTime) && DateTime.TryParse(appointmentEndTime, out startTime))
                {
                    appointmentEndTime = startTime.ToString("yyyy-MM-dd HH:") + (startTime.Minute >= 30 ? "30" : "00");
                }
            }
        }*/
        public void GetAppointmentTime2(double _supplierPassDuration, List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail, ref string appointmentStartTime, ref string appointmentEndTime, bool allowNoIntersection)
        {
            //根据批单生成预约时间，取交集
            if (listAscmDeliBatSumDetail != null)
            {
                double supplierPassDuration = (double)0.5;
                double.TryParse(YnFrame.Services.YnParameterService.GetInstance().GetValue(MideaAscm.Dal.Base.MyParameter.supplierPassDuration), out supplierPassDuration);
                if (supplierPassDuration < 0.5)
                    supplierPassDuration = 0.5;

                if (_supplierPassDuration > 0)
                {
                    supplierPassDuration = _supplierPassDuration;
                }
                double t = supplierPassDuration;
                DateTime dtAppointmentStart = DateTime.Now;
                DateTime dtAppointmentEnd = DateTime.Now;
                bool firstUsefulBatch = true;
                for(int i = 0; i< listAscmDeliBatSumDetail.Count; i++)
                {
                    AscmDeliBatSumDetail ascmDeliBatSumDetail = listAscmDeliBatSumDetail[i];
                    DateTime startTime, endTime;
                    DateTime.TryParse(ascmDeliBatSumDetail.appointmentStartTime, out startTime);
                    DateTime.TryParse(ascmDeliBatSumDetail.appointmentEndTime, out endTime);

                    //最早时间与最晚时间一样则不参与合单时间计算
                    if (startTime == endTime)
                    {
                        continue;
                    }
                    //第一张有效的批单
                    if (firstUsefulBatch)
                    {
                        dtAppointmentStart = startTime;
                        dtAppointmentEnd = endTime;
                        firstUsefulBatch = false;
                        continue;
                    }
                    if (startTime <= dtAppointmentStart && dtAppointmentStart <= endTime && endTime <= dtAppointmentEnd)
                    {
                        dtAppointmentEnd = endTime;
                    }
                    if (dtAppointmentStart <= startTime && startTime <= endTime && endTime <= dtAppointmentEnd)
                    {
                        dtAppointmentStart = startTime;
                        dtAppointmentEnd = endTime;
                    }
                    if (dtAppointmentStart <= startTime && startTime <= dtAppointmentEnd && dtAppointmentEnd <= endTime)
                    {
                        dtAppointmentStart = startTime;
                    }
                    if (dtAppointmentEnd <= startTime)
                    {
                        throw new Exception("当前选择的批单没有时间交集，无法生成合单");
                    }
                    
                }
                //按30分钟取整
                if (dtAppointmentStart.Minute > 0 && dtAppointmentStart.Minute < 30)
                {
                    dtAppointmentStart = DateTime.Parse(dtAppointmentStart.ToString("yyyy-MM-dd HH:") + "30:00");
                }
                if (dtAppointmentStart.Minute > 30)
                {
                    dtAppointmentStart = DateTime.Parse(dtAppointmentStart.AddHours(1).ToString("yyyy-MM-dd HH:") + "00:00");
                }
                if (dtAppointmentEnd.Minute > 0 && dtAppointmentEnd.Minute < 30)
                {
                    dtAppointmentEnd = DateTime.Parse(dtAppointmentEnd.ToString("yyyy-MM-dd HH:") + "30:00");
                }
                if (dtAppointmentEnd.Minute > 30)
                {
                    dtAppointmentEnd = DateTime.Parse(dtAppointmentEnd.AddHours(1).ToString("yyyy-MM-dd HH:") + "00:00");
                }
                //批单交集时间小于1分钟
                if (dtAppointmentEnd.Ticks - dtAppointmentStart.Ticks < 60 * 10000000)
                {
                    dtAppointmentEnd = dtAppointmentEnd.AddHours(t);
                }
                //批单交集时间大于预约设置时间
                if (dtAppointmentEnd.Ticks - dtAppointmentStart.Ticks > t * 3600 * 10000000)
                {
                    dtAppointmentEnd = dtAppointmentStart.AddHours(t);
                }
                TimeSpan span = dtAppointmentEnd - dtAppointmentStart;
                //仓库收料时间为8：00 – 11：20，13：00 - 17：00，合单生成的预约到货时间为8：00 -17：00。
                if (dtAppointmentStart.Hour < 8)
                {
                    dtAppointmentStart = DateTime.Parse(dtAppointmentStart.ToString("yyyy-MM-dd") + " 08:00:00");
                    dtAppointmentEnd = dtAppointmentStart.Add(span);
                }
                if (dtAppointmentStart.Hour < 17 && (dtAppointmentEnd.Hour > 17 || (dtAppointmentEnd.Hour == 17 && dtAppointmentEnd.Minute > 0)))
                {
                    dtAppointmentEnd = dtAppointmentEnd.AddHours(15);
                }
                if (dtAppointmentStart.Hour >= 17)
                {
                    dtAppointmentStart = DateTime.Parse(dtAppointmentStart.AddDays(1).ToString("yyyy-MM-dd") + " 08:00:00");
                    dtAppointmentEnd = dtAppointmentStart.Add(span);
                }

                appointmentStartTime = dtAppointmentStart.ToString("yyyy-MM-dd HH:mm");
                appointmentEndTime = dtAppointmentEnd.ToString("yyyy-MM-dd HH:mm");
            }
        }
        //public void GetAppointmentTime2(List<AscmDeliBatSumDetail> listAscmDeliBatSumDetail, ref string appointmentStartTime, ref string appointmentEndTime, bool allowNoIntersection)
        //{
        //    //根据批单生成预约时间，取交集
        //    if (listAscmDeliBatSumDetail != null)
        //    {
        //        double supplierPassDuration = (double)0.5;
        //        double.TryParse(YnFrame.Services.YnParameterService.GetInstance().GetValue(MideaAscm.Dal.Base.MyParameter.supplierPassDuration), out supplierPassDuration);
        //        if (supplierPassDuration < 0.5)
        //            supplierPassDuration = 0.5;

        //        DateTime dtAppointmentStart = Convert.ToDateTime("1900-01-01"), dtAppointmentEnd = Convert.ToDateTime("3000-01-01");
        //        foreach (AscmDeliBatSumDetail ascmDeliBatSumDetail in listAscmDeliBatSumDetail)
        //        {
        //            DateTime startTime = DateTime.Now, endTime = DateTime.Now;
        //            if (DateTime.TryParse(ascmDeliBatSumDetail.appointmentStartTime, out startTime) && DateTime.TryParse(ascmDeliBatSumDetail.appointmentEndTime, out endTime))
        //            {
        //                //5)最晚到货时间早于当前时间的批单，该批单总是可以勾选入合单，但不参加合单生成预约到货时间计算
        //                if (endTime < DateTime.Now)
        //                    continue;
        //                if (dtAppointmentStart < startTime)
        //                {
        //                    dtAppointmentStart = startTime;
        //                }
        //                if (dtAppointmentEnd > endTime)
        //                {
        //                    dtAppointmentEnd = endTime;
        //                }
        //            }
        //        }
        //        if (dtAppointmentStart != Convert.ToDateTime("1900-01-01"))
        //        {
        //            appointmentStartTime = dtAppointmentStart.ToString("yyyy-MM-dd HH:mm");
        //        }
        //        if (dtAppointmentEnd != Convert.ToDateTime("3000-01-01"))
        //        {
        //            appointmentEndTime = dtAppointmentEnd.ToString("yyyy-MM-dd HH:mm");
        //        }
        //        else
        //        {
        //            //若合单中全部批单为此类单，则合单最早到达时间为当前时间向后取半点，最晚到达时间为Te + Tc(当前Tc为4小时)，当前时间取服务器时间
        //            DateTime now = DateTime.Now;
        //            if (now.Minute > 30)
        //                now = now.AddMinutes(30);
        //            appointmentStartTime = now.ToString("yyyy-MM-dd HH:mm");
        //            appointmentEndTime = now.AddHours(supplierPassDuration).ToString("yyyy-MM-dd HH:mm");
        //        }

        //        if (DateTime.TryParse(appointmentStartTime, out dtAppointmentStart) && DateTime.TryParse(appointmentEndTime, out dtAppointmentEnd))
        //        {
        //            if (!allowNoIntersection && dtAppointmentStart >= dtAppointmentEnd)
        //            {
        //                throw new Exception("当前选择的批单没有时间交集，无法生成合单");
        //            }
        //            if (dtAppointmentStart >= dtAppointmentEnd)
        //            {

        //                dtAppointmentStart = dtAppointmentEnd.AddMinutes(-(int)supplierPassDuration * 60);//-30
        //                appointmentStartTime = dtAppointmentStart.ToString("yyyy-MM-dd HH:mm");
        //            }
        //            if (dtAppointmentStart.ToString("yyyy-MM-dd") != dtAppointmentEnd.ToString("yyyy-MM-dd"))
        //            {
        //                //不同天
        //                dtAppointmentEnd = dtAppointmentStart.AddMinutes(supplierPassDuration * 60);
        //                if (dtAppointmentStart.ToString("yyyy-MM-dd") != dtAppointmentEnd.ToString("yyyy-MM-dd"))
        //                {
        //                    //超过24点了
        //                    dtAppointmentEnd = Convert.ToDateTime(dtAppointmentStart.ToString("yyyy-MM-dd") + " 23:30");
        //                }
        //                appointmentEndTime = dtAppointmentEnd.ToString("yyyy-MM-dd HH:mm");
        //            }
        //        }
        //        //按30分钟取整
        //        if (!string.IsNullOrEmpty(appointmentStartTime) && DateTime.TryParse(appointmentStartTime, out dtAppointmentStart))
        //        {
        //            appointmentStartTime = dtAppointmentStart.ToString("yyyy-MM-dd HH:") + (dtAppointmentStart.Minute >= 30 ? "30" : "00");
        //        }
        //        if (!string.IsNullOrEmpty(appointmentEndTime) && DateTime.TryParse(appointmentEndTime, out dtAppointmentEnd))
        //        {
        //            appointmentEndTime = dtAppointmentEnd.ToString("yyyy-MM-dd HH:") + (dtAppointmentEnd.Minute >= 30 ? "30" : "00");
        //        }
        //        //6)仓库收料时间为8：00 – 11：20，13：00 - 17：00，合单生成的预约到货时间为8：00 -17：00。
        //        if (!string.IsNullOrEmpty(appointmentStartTime) && DateTime.TryParse(appointmentStartTime, out dtAppointmentStart) && !string.IsNullOrEmpty(appointmentEndTime) && DateTime.TryParse(appointmentEndTime, out dtAppointmentEnd))
        //        {
        //            TimeSpan ts = dtAppointmentEnd - dtAppointmentStart;
        //            if (dtAppointmentStart.Hour < 8)
        //            {
        //                appointmentStartTime = dtAppointmentStart.ToString("yyyy-MM-dd 08:00");
        //                appointmentEndTime = Convert.ToDateTime(appointmentStartTime).AddMinutes(ts.TotalMinutes).ToString("yyyy-MM-dd HH:mm");
        //            }
        //            if (dtAppointmentEnd.Hour > 17)
        //            {
        //                appointmentEndTime = dtAppointmentEnd.ToString("yyyy-MM-dd 17:00");
        //                appointmentStartTime = Convert.ToDateTime(appointmentEndTime).AddMinutes(-ts.TotalMinutes).ToString("yyyy-MM-dd HH:mm");
        //            }
        //        }
        //    }
        //}
        public List<AscmMaterialItem> GetMaterialListBySumMain(int sumMainId)
        {
            List<AscmMaterialItem> list = null;
            try
            {
                string sort = " order by id ";
                string sql = "select batchId from AscmDeliBatSumDetail where mainId =" + sumMainId + "";
                sql = "from AscmDeliveryOrderDetail where mainId in (select id from AscmDeliveryOrderMain where batchId  in (" + sql + "))";

                string where = "", whereQueryWord = "";
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);

                if (!string.IsNullOrEmpty(where))
                    sql += " and " + where;
                IList<AscmDeliveryOrderDetail> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderDetail>(sql + sort);
                if (ilist != null)
                {
                    List<AscmDeliveryOrderDetail> listAscmDeliveryOrderDetail = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDeliveryOrderDetail>(ilist);
                    //SetMain(list);
                    MideaAscm.Services.FromErp.AscmDeliveryOrderDetailService.GetInstance().SetMaterial(listAscmDeliveryOrderDetail);
                    //SetNotifyDetail(list);
                    list = new List<AscmMaterialItem>();
                    foreach (AscmDeliveryOrderDetail ascmDeliveryOrderDetail in listAscmDeliveryOrderDetail)
                    {
                        AscmMaterialItem ascmMaterialItem = list.Find(p => p.id == ascmDeliveryOrderDetail.materialId);
                        if (ascmMaterialItem == null)
                        {
                            ascmMaterialItem = ascmDeliveryOrderDetail.ascmMaterialItem;
                            ascmMaterialItem.deliveryQuantity = 0;
                            list.Add(ascmMaterialItem);
                        }
                        ascmMaterialItem.deliveryQuantity += ascmDeliveryOrderDetail.deliveryQuantity;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public AscmDeliBatSumAllDetail GetAscmDeliBatSumAllDetail(int sumMainId)
        {
            AscmDeliBatSumAllDetail ascmDeliBatSumAllDetail = new AscmDeliBatSumAllDetail();
            try
            {
                string sql = "from AscmDeliBatSumDetail where mainId=" + sumMainId;
                IList<AscmDeliBatSumDetail> ilistAscmDeliBatSumDetail = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliBatSumDetail>(sql);
                if (ilistAscmDeliBatSumDetail != null && ilistAscmDeliBatSumDetail.Count > 0)
                {
                    ascmDeliBatSumAllDetail.listAscmDeliBatSumDetail = ilistAscmDeliBatSumDetail.ToList();

                    sql = "from AscmContainerDelivery where batSumMainId=" + sumMainId;
                    IList<AscmContainerDelivery> ilistAscmContainerDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerDelivery>(sql);
                    if (ilistAscmContainerDelivery != null)
                    {
                        ascmDeliBatSumAllDetail.listAscmContainerDelivery = ilistAscmContainerDelivery.ToList();
                    }

                    sql = "from AscmPalletDelivery where batSumMainId=" + sumMainId;
                    IList<AscmPalletDelivery> ilistAscmPalletDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql);
                    if (ilistAscmPalletDelivery != null)
                    {
                        ascmDeliBatSumAllDetail.listAscmPalletDelivery = ilistAscmPalletDelivery.ToList();
                    }

                    sql = "select new AscmMaterialItem(a.id,a.docNumber,a.description) from AscmMaterialItem a,AscmDeliBatSumDetail b where a.id=b.materialId and b.mainId=" + sumMainId;
                    IList<AscmMaterialItem> ilistAscmMaterialItem = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmMaterialItem>(sql);
                    if (ilistAscmMaterialItem != null)
                    {
                        ascmDeliBatSumAllDetail.listAscmMaterialItem = ilistAscmMaterialItem.Distinct(new AscmMaterialItemComparer()).ToList();
                    }

                    sql = "select new AscmDeliveryOrderBatch(a,b.materialId) from AscmDeliveryOrderBatch a,AscmDeliBatSumDetail b where a.id=b.batchId and b.mainId=" + sumMainId;
                    IList<AscmDeliveryOrderBatch> ilistAscmDeliveryOrderBatch = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDeliveryOrderBatch>(sql);
                    if (ilistAscmDeliveryOrderBatch != null)
                    {
                        ascmDeliBatSumAllDetail.listAscmDeliveryOrderBatch = ilistAscmDeliveryOrderBatch.ToList();
                    }

                    ascmDeliBatSumAllDetail.sumMainId = sumMainId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ascmDeliBatSumAllDetail;
        }
        public void SaveAscmDeliBatSumAllDetail(AscmDeliBatSumAllDetail ascmDeliBatSumAllDetail)
        {
            try
            {
                if (ascmDeliBatSumAllDetail != null)
                {
                    #region 容器备料
                    List<AscmContainerDelivery> listAscmContainerDelivery = ascmDeliBatSumAllDetail.listAscmContainerDelivery;
                    List<AscmContainerDelivery> listAscmContainerDeliverySave = null;
                    List<AscmContainerDelivery> listAscmContainerDeliveryUpdate = null;
                    List<AscmContainerDelivery> listAscmContainerDeliveryDelete = null;
                    if (listAscmContainerDelivery != null)
                    {
                        listAscmContainerDeliverySave = new List<AscmContainerDelivery>();
                        listAscmContainerDeliveryUpdate = new List<AscmContainerDelivery>();
                        listAscmContainerDeliveryDelete = new List<AscmContainerDelivery>();

                        List<AscmContainerDelivery> _listAscmContainerDelivery = null;
                        string sql = "from AscmContainerDelivery where batSumMainId=" + ascmDeliBatSumAllDetail.sumMainId;
                        IList<AscmContainerDelivery> ilistAscmContainerDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmContainerDelivery>(sql);
                        if (ilistAscmContainerDelivery != null && ilistAscmContainerDelivery.Count > 0)
                            _listAscmContainerDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmContainerDelivery>(ilistAscmContainerDelivery);
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmContainerDelivery");

                        foreach (AscmContainerDelivery ascmContainerDelivery in listAscmContainerDelivery)
                        {
                            AscmContainerDelivery findAscmContainerDelivery = null;
                            if (_listAscmContainerDelivery != null)
                                findAscmContainerDelivery = _listAscmContainerDelivery.Find(P => P.id == ascmContainerDelivery.id);
                            if (findAscmContainerDelivery == null)
                            {
                                ascmContainerDelivery.id = ++maxId;
                                ascmContainerDelivery.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmContainerDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                listAscmContainerDeliverySave.Add(ascmContainerDelivery);
                            }
                            else
                            {
                                findAscmContainerDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                findAscmContainerDelivery.quantity = ascmContainerDelivery.quantity;
                                listAscmContainerDeliveryUpdate.Add(findAscmContainerDelivery);
                            }
                        }
                        if (_listAscmContainerDelivery != null)
                        {
                            foreach (AscmContainerDelivery ascmContainerDelivery in _listAscmContainerDelivery)
                            {
                                AscmContainerDelivery findAscmContainerDelivery = listAscmContainerDelivery.Find(P => P.id == ascmContainerDelivery.id);
                                if (findAscmContainerDelivery == null)
                                    listAscmContainerDeliveryDelete.Add(ascmContainerDelivery);
                            }
                        }
                    }
                    #endregion

                    #region 托盘备料
                    List<AscmPalletDelivery> listAscmPalletDelivery = ascmDeliBatSumAllDetail.listAscmPalletDelivery;
                    List<AscmPalletDelivery> listAscmPalletDeliverySaveOrUpdate = null;
                    List<AscmPalletDelivery> listAscmPalletDeliveryDelete = null;
                    if (listAscmPalletDelivery != null)
                    {
                        listAscmPalletDeliverySaveOrUpdate = new List<AscmPalletDelivery>();
                        listAscmPalletDeliveryDelete = new List<AscmPalletDelivery>();

                        List<AscmPalletDelivery> _listAscmPalletDelivery = null;
                        string sql = "from AscmPalletDelivery where batSumMainId=" + ascmDeliBatSumAllDetail.sumMainId;
                        IList<AscmPalletDelivery> ilistAscmPalletDelivery = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmPalletDelivery>(sql);
                        if (ilistAscmPalletDelivery != null && ilistAscmPalletDelivery.Count > 0)
                            _listAscmPalletDelivery = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmPalletDelivery>(ilistAscmPalletDelivery);
                        int maxId = YnDaoHelper.GetInstance().nHibernateHelper.GetMaxId("select max(id) from AscmPalletDelivery");
                        foreach (AscmPalletDelivery ascmPalletDelivery in listAscmPalletDelivery)
                        {
                            AscmPalletDelivery findAscmPalletDelivery = null;
                            if (_listAscmPalletDelivery != null)
                                findAscmPalletDelivery = _listAscmPalletDelivery.Find(P => P.id == ascmPalletDelivery.id);
                            //if (findAscmPalletDelivery == null)
                            //    ascmPalletDelivery.id = ++maxId;
                            //listAscmPalletDeliverySaveOrUpdate.Add(ascmPalletDelivery);
                            if (findAscmPalletDelivery == null)
                            {
                                ascmPalletDelivery.id = ++maxId;
                                ascmPalletDelivery.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                ascmPalletDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                listAscmPalletDeliverySaveOrUpdate.Add(ascmPalletDelivery);
                            }
                            else
                            {
                                findAscmPalletDelivery.modifyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                findAscmPalletDelivery.quantity = findAscmPalletDelivery.quantity;
                                listAscmPalletDeliverySaveOrUpdate.Add(findAscmPalletDelivery);
                            }

                        }
                        if (_listAscmPalletDelivery != null)
                        {
                            foreach (AscmPalletDelivery ascmPalletDelivery in _listAscmPalletDelivery)
                            {
                                AscmPalletDelivery findAscmPalletDelivery = listAscmPalletDelivery.Find(P => P.id == ascmPalletDelivery.id);
                                if (findAscmPalletDelivery == null)
                                    listAscmPalletDeliveryDelete.Add(ascmPalletDelivery);
                            }
                        }
                    }
                    #endregion

                    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                    {
                        try
                        {
                            if (listAscmContainerDeliverySave != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmContainerDeliverySave);
                            if (listAscmContainerDeliveryUpdate != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.UpdateList(listAscmContainerDeliveryUpdate);
                            if (listAscmContainerDeliveryDelete != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmContainerDeliveryDelete);

                            if (listAscmPalletDeliverySaveOrUpdate != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.SaveOrUpdateList(listAscmPalletDeliverySaveOrUpdate);
                            if (listAscmPalletDeliveryDelete != null)
                                YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmPalletDeliveryDelete);

                            tx.Commit();//正确执行提交
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();//回滚
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDeliBatSumAllDetail)", ex);
                throw ex;
            }
        }
        /// <summary>供应商车辆入厂</summary>
        public AscmDeliBatSumMain GetEnterByDriverId(int driverId)
        {
            AscmDeliBatSumMain ascmDeliBatSumMain = null;
            try
            {
                List<AscmDeliBatSumMain> list = GetByDriverId(driverId);
                if (list != null && list.Count > 0)
                {
                    ascmDeliBatSumMain = list[0];
                }
                if (ascmDeliBatSumMain != null)
                {
                    ascmDeliBatSumMain.ascmDriver = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDriver>(driverId);
                    ascmDeliBatSumMain.ascmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmSupplier>(ascmDeliBatSumMain.supplierId);
                    MideaAscm.Services.Base.AscmSupplierService.GetInstance().SetSupplierAddress(new List<AscmSupplier> { ascmDeliBatSumMain.ascmSupplier });
                    GetSupplierUnloadingPoint(ascmDeliBatSumMain);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return ascmDeliBatSumMain;
        }
        /// <summary>供应商车辆出厂</summary>
        public AscmDeliBatSumMain GetOutByDriverId(int driverId)
        {
            AscmDeliBatSumMain ascmDeliBatSumMain = null;
            try
            {
                ascmDeliBatSumMain = GetByDriverId(driverId, AscmDeliBatSumMain.StatusDefine.inPlant);
                if (ascmDeliBatSumMain != null)
                {
                    ascmDeliBatSumMain.ascmDriver = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDriver>(driverId);
                    ascmDeliBatSumMain.ascmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmSupplier>(ascmDeliBatSumMain.supplierId);
                    MideaAscm.Services.Base.AscmSupplierService.GetInstance().SetSupplierAddress(new List<AscmSupplier> { ascmDeliBatSumMain.ascmSupplier });
                    GetSupplierUnloadingPoint(ascmDeliBatSumMain);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDeliBatSumMain)", ex);
                throw ex;
            }
            return ascmDeliBatSumMain;
        }
        public void GetSupplierUnloadingPoint(AscmDeliBatSumMain ascmDeliBatSumMain)
        {
            try
            {
                bool isOnTime = true;
                DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmDeliBatSumMain");
                DateTime beginTime = DateTime.Now, endTime = DateTime.Now;
                if (DateTime.TryParse(ascmDeliBatSumMain.appointmentStartTime, out beginTime))
                {
                    //早于最早到货时间
                    if (dtServerTime.CompareTo(beginTime) < 0)
                    {
                        isOnTime = false;
                    }
                }
                if (DateTime.TryParse(ascmDeliBatSumMain.appointmentEndTime, out endTime))
                {
                    //晚于最晚到货时间
                    if (dtServerTime.CompareTo(endTime) > 0)
                    {
                        isOnTime = false;
                    }
                }
                ascmDeliBatSumMain.isOnTime = isOnTime;

                string sql = "from AscmUnloadingPoint";
                string where = string.Empty;
                string whereWarehouse = "warehouseId in(select warehouseId from AscmDeliveryOrderBatch where id in({0}))";
                whereWarehouse = string.Format(whereWarehouse, "select batchId from AscmDeliBatSumDetail where mainId=" + ascmDeliBatSumMain.id);

                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereWarehouse);
                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }
                IList<AscmUnloadingPoint> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPoint>(sql);
                if (ilist != null && ilist.Count > 0)
                {
                    List<AscmUnloadingPoint> list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPoint>(ilist);
                    //分配卸货点
                    AscmUnloadingPoint ascmUnloadingPoint = list.Find(P => P.status == AscmUnloadingPoint.StatusDefine.idle);
                    if (ascmUnloadingPoint != null)
                    {
                        //更改卸货点状态为“预定”
                        ascmUnloadingPoint.status = AscmUnloadingPoint.StatusDefine.reserve;
                        dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmDeliBatSumMain");
                        ascmUnloadingPoint.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                        YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmUnloadingPoint);
                        //为入厂供应商车辆预分配一个卸货点，同时列出其它卸货点的使用情况
                        ascmDeliBatSumMain.unloadingPointName = ascmUnloadingPoint.name;
                    }
                    ascmDeliBatSumMain.listUnloadingPoint = list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
