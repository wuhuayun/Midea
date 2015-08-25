using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Vehicle.Entities;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.SupplierPreparation;

namespace MideaAscm.Services.Vehicle
{
    public class AscmUnloadingPointService
    {
        private static AscmUnloadingPointService ascmUnloadingPointServices;
        public static AscmUnloadingPointService GetInstance()
        {
            if (ascmUnloadingPointServices == null)
                ascmUnloadingPointServices = new AscmUnloadingPointService();
            return ascmUnloadingPointServices;
        }

        public AscmUnloadingPoint Get(int id)
        {
            AscmUnloadingPoint ascmUnloadingPoint = null;
            try
            {
                ascmUnloadingPoint = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUnloadingPoint>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmUnloadingPoint)", ex);
                throw ex;
            }
            return ascmUnloadingPoint;
        }
        public List<AscmUnloadingPoint> GetList(string sql, string sessionKey)
        {
            List<AscmUnloadingPoint> list = null;
            try
            {
                IList<AscmUnloadingPoint> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPoint>(sql, sessionKey);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPoint>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPoint)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmUnloadingPoint> GetList(string sql, bool isSetWarehouse = false, bool isSetUnloadingPointController = false)
        {
            List<AscmUnloadingPoint> list = null;
            try
            {
                IList<AscmUnloadingPoint> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPoint>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPoint>(ilist);
                    if (isSetWarehouse)
                        SetWarehouse(list);
                    if (isSetUnloadingPointController)
                        SetUnloadingPointController(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPoint)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmUnloadingPoint> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmUnloadingPoint> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmUnloadingPoint ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (name like '%" + queryWord.Trim() + "%') or (warehouseId like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;

                IList<AscmUnloadingPoint> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPoint>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPoint>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPoint>(ilist);
                    SetWarehouse(list);
                    SetUnloadingPointController(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmUnloadingPoint)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmUnloadingPoint> listAscmUnloadingPoint)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmUnloadingPoint);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPoint)", ex);
                throw ex;
            }
        }
        public void Save(AscmUnloadingPoint ascmUnloadingPoint)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmUnloadingPoint);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmUnloadingPoint)", ex);
                throw ex;
            }
        }
        public void Update(AscmUnloadingPoint ascmUnloadingPoint)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmUnloadingPoint>(ascmUnloadingPoint);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmUnloadingPoint)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmUnloadingPoint)", ex);
                throw ex;
            }
        }
        public void UpdateStatus(int id, string status)
        {
            try
            {
                AscmUnloadingPoint ascmUnloadingPoint = Get(id);
                if (ascmUnloadingPoint == null)
                    throw new Exception("找不到卸货点");
                ascmUnloadingPoint.status = status;
                ascmUnloadingPoint.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:ss");

                AscmUnloadingPointLog ascmUnloadingPointLog = 
                    AscmUnloadingPointLogService.GetInstance().GetAddLog(ascmUnloadingPoint.id, ascmUnloadingPoint.name, ascmUnloadingPoint.sn, ascmUnloadingPoint.status, DateTime.Now);
                
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmUnloadingPoint);
                        if (ascmUnloadingPointLog != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmUnloadingPointLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改状态失败(Update AscmUnloadingPoint Status)", ex);
                throw ex;
            }
        }
        public void UpdateStatus(int id, string status, string sessionKey)
        {
            try
            {
                AscmUnloadingPoint ascmUnloadingPoint = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmUnloadingPoint>(id, sessionKey);
                if (ascmUnloadingPoint == null)
                    throw new Exception("找不到卸货点");
                ascmUnloadingPoint.status = status;
                ascmUnloadingPoint.modifyTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:ss");

                DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmUnloadingPointLog", sessionKey);

                AscmUnloadingPointLog ascmUnloadingPointLog = new AscmUnloadingPointLog();
                ascmUnloadingPointLog.createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ascmUnloadingPointLog.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");
                ascmUnloadingPointLog.unloadingPointId = ascmUnloadingPoint.id;
                ascmUnloadingPointLog.unloadingPointName = ascmUnloadingPoint.name;
                ascmUnloadingPointLog.unloadingPointSn = ascmUnloadingPoint.sn;
                ascmUnloadingPointLog.unloadingPointStatus = ascmUnloadingPoint.status;

                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession(sessionKey).BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmUnloadingPoint, sessionKey);
                        if (ascmUnloadingPointLog != null)
                            YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmUnloadingPointLog, sessionKey);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改状态失败(Update AscmUnloadingPoint Status)", ex);
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                AscmUnloadingPoint ascmUnloadingPoint = Get(id);
                Delete(ascmUnloadingPoint);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmUnloadingPoint ascmUnloadingPoint)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmUnloadingPoint>(ascmUnloadingPoint);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPoint)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmUnloadingPoint> listAscmUnloadingPoint)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmUnloadingPoint);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmUnloadingPoint)", ex);
                throw ex;
            }
        }

        private void SetWarehouse(List<AscmUnloadingPoint> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmUnloadingPoint ascmUnloadingPoint in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "'" + ascmUnloadingPoint.warehouseId + "'";
                }
                string sql = "from AscmWarehouse where id in (" + ids + ")";
                IList<AscmWarehouse> ilistAscmWarehouse = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmWarehouse>(sql);
                if (ilistAscmWarehouse != null && ilistAscmWarehouse.Count > 0)
                {
                    List<AscmWarehouse> listAscmWarehouse = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmWarehouse>(ilistAscmWarehouse);
                    foreach (AscmUnloadingPoint ascmUnloadingPoint in list)
                    {
                        ascmUnloadingPoint.ascmWarehouse = listAscmWarehouse.Find(e => e.id == ascmUnloadingPoint.warehouseId);
                    }
                }
            }
        }
        private void SetUnloadingPointController(List<AscmUnloadingPoint> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmUnloadingPoint ascmUnloadingPoint in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmUnloadingPoint.controllerId;
                }
                string sql = "from AscmUnloadingPointController where id in (" + ids + ")";
                IList<AscmUnloadingPointController> ilistAscmUnloadingPointController = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmUnloadingPointController>(sql);
                if (ilistAscmUnloadingPointController != null && ilistAscmUnloadingPointController.Count > 0)
                {
                    List<AscmUnloadingPointController> listAscmUnloadingPointController = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmUnloadingPointController>(ilistAscmUnloadingPointController);
                    foreach (AscmUnloadingPoint ascmUnloadingPoint in list)
                    {
                        ascmUnloadingPoint.ascmUnloadingPointController = listAscmUnloadingPointController.Find(e => e.id == ascmUnloadingPoint.controllerId);
                    }
                }
            }
        }

        #region 货车入厂LED显示
        //忙时：根据司机RFID，显示供应商名称、预约送货时间（最早、预约）、卸货点
        public void GetLedBusyTimeShow(string driverRfid)
        {
            if (!string.IsNullOrEmpty(driverRfid))
            {
                AscmDriver ascmDriver = AscmDriverService.GetInstance().GetByDriverSn(driverRfid);
                if (ascmDriver != null)
                {
                    string supplierName = ascmDriver.supplierName;
                    string plateNumber = ascmDriver.plateNumber;
                    

                }
            }
        }
        #endregion
    }
}
