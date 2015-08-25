using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Services.Vehicle
{
    public class AscmTruckSwipeLogService
    {
        private static AscmTruckSwipeLogService ascmTruckSwipeLogServices;
        public static AscmTruckSwipeLogService GetInstance()
        {
            if (ascmTruckSwipeLogServices == null)
                ascmTruckSwipeLogServices = new AscmTruckSwipeLogService();
            return ascmTruckSwipeLogServices;
        }
        public AscmTruckSwipeLog Get(string id)
        {
            AscmTruckSwipeLog ascmTruckSwipeLog = null;
            try
            {
                ascmTruckSwipeLog = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmTruckSwipeLog>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmTruckSwipeLog)", ex);
                throw ex;
            }
            return ascmTruckSwipeLog;
        }
        ///<summary>获取卡信息列表</summary>
        ///<returns>返回卡信息列表</returns>
        public List<AscmTruckSwipeLog> GetList(string sql)
        {
            List<AscmTruckSwipeLog> list = null;
            try
            {
                IList<AscmTruckSwipeLog> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmTruckSwipeLog>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmTruckSwipeLog>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmTruckSwipeLog)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmTruckSwipeLog> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord,string whereOther)
        {
            List<AscmTruckSwipeLog> list = null;
            try
            {
                string sort = " order by id ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmTruckSwipeLog ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    queryWord = queryWord.Trim().Replace(" ", "%");
                    whereQueryWord = " (supplierName like '%" + queryWord.Trim() + "%' or driverName like '%" + queryWord.Trim() + "%'  or plateNumber like '%" + queryWord.Trim() + "%' or rfid like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmTruckSwipeLog> ilist = null;
                if (ynPage != null)
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmTruckSwipeLog>(sql + sort, sql, ynPage);
                else
                    ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmTruckSwipeLog>(sql + sort);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmTruckSwipeLog>(ilist);
                    SetDoor(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmTruckSwipeLog)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmTruckSwipeLog> listAscmTruckSwipeLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmTruckSwipeLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmTruckSwipeLog)", ex);
                throw ex;
            }
        }
        public void Save(AscmTruckSwipeLog ascmTruckSwipeLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmTruckSwipeLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmTruckSwipeLog)", ex);
                throw ex;
            }
        }
        public void Update(AscmTruckSwipeLog ascmTruckSwipeLog)
        {
            //int count = YnDaoHelper.GetInstance().nHibernateHelper.GetCount("select count(*) from AscmTruckSwipeLog where id<>" + ascmTruckSwipeLog.id + " and docNumber='" + ascmTruckSwipeLog.docNumber + "'");
            //if (count == 0)
            //{
            //    using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
            //    {
            //        try
            //        {
            //            YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmTruckSwipeLog>(ascmTruckSwipeLog);
            //            tx.Commit();//正确执行提交
            //        }
            //        catch (Exception ex)
            //        {
            //            tx.Rollback();//回滚
            //            YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmTruckSwipeLog)", ex);
            //            throw ex;
            //        }
            //    }
            //}
            //else
            //{
            //    throw new Exception("已经存在员工编号\"" + ascmTruckSwipeLog.name + "\"！");
            //}
        }
        public void Delete(string id)
        {
            try
            {
                AscmTruckSwipeLog ascmTruckSwipeLog = Get(id);
                Delete(ascmTruckSwipeLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmTruckSwipeLog ascmTruckSwipeLog)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmTruckSwipeLog>(ascmTruckSwipeLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmTruckSwipeLog)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmTruckSwipeLog> listAscmTruckSwipeLog)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmTruckSwipeLog);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmTruckSwipeLog)", ex);
                throw ex;
            }
        }
        public void AddLog(int doorId, string readingHead, string rfid, int supplierId, int driverId, string supplierName, string driverName,
            string plateNumber, bool pass, string description, DateTime createTime, string direction, int batSumMainId, string batSumDocNumber, bool onTime,
            string appointmentStartTime, string appointmentEndTime)
        {
            try
            {
                AscmTruckSwipeLog ascmTruckSwipeLog = GetAddLog(doorId, readingHead, rfid, supplierId, driverId, supplierName, driverName, 
                    plateNumber, pass, description, createTime, direction, batSumMainId, batSumDocNumber, onTime, appointmentStartTime, appointmentEndTime);

                Save(ascmTruckSwipeLog);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("增加失败(Add AscmTruckSwipeLog)", ex);
                throw ex;
            }
        }
        public AscmTruckSwipeLog GetAddLog(int doorId, string readingHead, string rfid, int supplierId, int driverId, string supplierName, string driverName,
            string plateNumber, bool pass, string description, DateTime createTime, string direction, int batSumMainId, string batSumDocNumber, bool onTime,
            string appointmentStartTime, string appointmentEndTime)
        {
            try
            {
                DateTime dtServerTime = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentDate("AscmTruckSwipeLog");

                AscmTruckSwipeLog ascmTruckSwipeLog = new AscmTruckSwipeLog();
                ascmTruckSwipeLog.createTime = createTime.ToString("yyyy-MM-dd HH:mm:ss");
                ascmTruckSwipeLog.modifyTime = dtServerTime.ToString("yyyy-MM-dd HH:mm:ss");

                ascmTruckSwipeLog.doorId = doorId;
                ascmTruckSwipeLog.readingHeadId = 0;
                ascmTruckSwipeLog.readingHead = readingHead;
                ascmTruckSwipeLog.rfid = rfid;
                ascmTruckSwipeLog.supplierId = supplierId;
                ascmTruckSwipeLog.driverId = driverId;
                ascmTruckSwipeLog.supplierName = supplierName;
                ascmTruckSwipeLog.driverName = driverName;
                ascmTruckSwipeLog.plateNumber = plateNumber;

                ascmTruckSwipeLog.status = 1;
                ascmTruckSwipeLog.pass = pass;
                ascmTruckSwipeLog.direction = direction;
                ascmTruckSwipeLog.description = description;

                ascmTruckSwipeLog.batSumMainId = batSumMainId;
                ascmTruckSwipeLog.batSumDocNumber = batSumDocNumber;
                ascmTruckSwipeLog.onTime = onTime;
                ascmTruckSwipeLog.appointmentStartTime = appointmentStartTime;
                ascmTruckSwipeLog.appointmentEndTime = appointmentEndTime;

                return ascmTruckSwipeLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetDoor(List<AscmTruckSwipeLog> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmTruckSwipeLog ascmTruckSwipeLog in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += ascmTruckSwipeLog.doorId;
                }
                string sql = "from AscmDoor where id in (" + ids + ")";
                IList<AscmDoor> ilistAscmDoor = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDoor>(sql);
                if (ilistAscmDoor != null && ilistAscmDoor.Count > 0)
                {
                    List<AscmDoor> listAscmDoor = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDoor>(ilistAscmDoor);
                    foreach (AscmTruckSwipeLog ascmTruckSwipeLog in list)
                    {
                        ascmTruckSwipeLog.ascmDoor = listAscmDoor.Find(e => e.id == ascmTruckSwipeLog.doorId);
                    }
                }
            }
        }
    }
}
