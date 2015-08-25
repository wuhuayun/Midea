using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MideaAscm.Dal.Base.Entities;
using MideaAscm.Dal;
using YnBaseDal;
using NHibernate;
using MideaAscm.Dal.SupplierPreparation.Entities;
using MideaAscm.Services.Vehicle;
using MideaAscm.Dal.Vehicle.Entities;

namespace MideaAscm.Services.SupplierPreparation
{
    public class AscmDriverService
    {
        private static AscmDriverService ascmDriverServices;
        public static AscmDriverService GetInstance()
        {
            if (ascmDriverServices == null)
                ascmDriverServices = new AscmDriverService();
            return ascmDriverServices;
        }

        public AscmDriver Get(int id)
        {
            AscmDriver ascmDriver = null;
            try
            {
                ascmDriver = YnDaoHelper.GetInstance().nHibernateHelper.Get<AscmDriver>(id);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Get AscmDriver)", ex);
                throw ex;
            }
            return ascmDriver;
        }
        public AscmDriver GetByDriverSn(string driverSn)
        {
            AscmDriver ascmDriver = null;
            try
            {
                string sql = "from AscmDriver where sn='" + driverSn + "'";
                IList<AscmDriver> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriver>(sql);
                if (ilist != null && ilist.Count>0)
                {
                    //list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriver>(ilist);
                    ascmDriver= ilist[0];
                    SetSupplier(new List<AscmDriver> { ascmDriver });
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDriver)", ex);
                throw ex;
            }
            return ascmDriver;
        }
        public List<AscmDriver> GetList(string sql)
        {
            List<AscmDriver> list = null;
            try
            {
                IList<AscmDriver> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriver>(sql);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriver>(ilist);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDriver)", ex);
                throw ex;
            }
            return list;
        }
        public List<AscmDriver> GetList(YnPage ynPage, string sortName, string sortOrder, string queryWord, string whereOther)
        {
            List<AscmDriver> list = null;
            try
            {
                string sort = " order by sn ";
                if (!string.IsNullOrEmpty(sortName))
                {
                    sort = " order by " + sortName.Trim() + " ";
                    if (!string.IsNullOrEmpty(sortOrder))
                        sort += sortOrder.Trim();
                }
                string sql = "from AscmDriver ";

                string where = "", whereQueryWord = "";
                if (!string.IsNullOrEmpty(queryWord))
                {
                    whereQueryWord = " (sn like '%" + queryWord.Trim() + "%') or (name like '%" + queryWord.Trim() + "%') or (plateNumber like '%" + queryWord.Trim() + "%')";
                }
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereQueryWord);
                where = YnBaseClass2.Helper.StringHelper.SqlWhereAndAdd(where, whereOther);

                if (!string.IsNullOrEmpty(where))
                    sql += " where " + where;
                IList<AscmDriver> ilist = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmDriver>(sql + sort, sql, ynPage);
                if (ilist != null)
                {
                    list = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmDriver>(ilist);
                    SetSupplier(list);
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("查询失败(Find AscmDriver)", ex);
                throw ex;
            }
            return list;
        }
        public void Save(List<AscmDriver> listAscmDriver)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.SaveList(listAscmDriver);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriver)", ex);
                throw ex;
            }
        }
        public void Save(AscmDriver ascmDriver)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmDriver);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("保存失败(Save AscmDriver)", ex);
                throw ex;
            }
        }
        public void Update(AscmDriver ascmDriver)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.Update<AscmDriver>(ascmDriver);
                        tx.Commit();//正确执行提交
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();//回滚
                        YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Update AscmDriver)", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("修改失败(Save AscmDriver)", ex);
                throw ex;
            }
        }
        public void Delete(int id)
        {
            try
            {
                AscmDriver ascmDriver = Get(id);
                Delete(ascmDriver);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(AscmDriver ascmDriver)
        {
            try
            {
                YnDaoHelper.GetInstance().nHibernateHelper.Delete<AscmDriver>(ascmDriver);
            }
            catch (Exception ex)
            {
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDriver)", ex);
                throw ex;
            }
        }
        public void Delete(List<AscmDriver> listAscmDriver)
        {
            try
            {
                using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                {
                    try
                    {
                        YnDaoHelper.GetInstance().nHibernateHelper.DeleteList(listAscmDriver);
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
                YnBaseClass2.Helper.LogHelper.GetLog().Error("删除失败(Delete AscmDriver)", ex);
                throw ex;
            }
        }

        private void SetSupplier(List<AscmDriver> list)
        {
            if (list != null && list.Count > 0)
            {
                string ids = string.Empty;
                foreach (AscmDriver ascmDriver in list)
                {
                    if (!string.IsNullOrEmpty(ids))
                        ids += ",";
                    ids += "" + ascmDriver.supplierId + "";
                }
                string sql = "from AscmSupplier where id in (" + ids + ")";
                IList<AscmSupplier> ilistAscmSupplier = YnDaoHelper.GetInstance().nHibernateHelper.Find<AscmSupplier>(sql);
                if (ilistAscmSupplier != null && ilistAscmSupplier.Count > 0)
                {
                    List<AscmSupplier> listAscmSupplier = YnBaseClass2.Helper.ConvertHelper.ConvertIListToList<AscmSupplier>(ilistAscmSupplier);
                    MideaAscm.Services.Base.AscmSupplierService.GetInstance().SetSupplierAddress(listAscmSupplier);
                    foreach (AscmDriver ascmDriver in list)
                    {
                        ascmDriver.supplier = listAscmSupplier.Find(e => e.id == ascmDriver.supplierId);
                    }
                }
            }
        }


        //车辆入厂
        public void InOutPlant(int doorId, string readingHead, int driverId, bool inPlant, string direction, bool onTime,ref string allocateOutDoor)
        {
            try
            {
                AscmDriver ascmDriver = Get(driverId);
                if (ascmDriver != null)
                {
                    int batSumMainId = 0;
                    string batSumDocNumber = string.Empty;
                    string appointmentStartTime = string.Empty;
                    string appointmentEndTime = string.Empty;

                    SetSupplier(new List<AscmDriver> { ascmDriver });

                    List<AscmDeliBatSumMain> list = AscmDeliBatSumMainService.GetInstance().GetByDriverId(ascmDriver.id);
                    if (list != null)
                    {
                        foreach (AscmDeliBatSumMain ascmDeliBatSumMain in list)
                        {
                            if (inPlant)
                            {
                                ascmDeliBatSumMain.status = AscmDeliBatSumMain.StatusDefine.inPlant;
                                ascmDeliBatSumMain.toPlantTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                ascmDeliBatSumMain.allocateOutDoor = "北门";
                                if (ascmDeliBatSumMain.warehouseId != "W112材料")
                                {
                                    #region 取上一次送货合单
                                    YnPage ynPage = new YnPage();
                                    ynPage.SetPageSize(1);
                                    ynPage.SetCurrentPage(1);
                                    //string sort = " order by toPlantTime desc ";
                                    string where = " toPlantTime is not null and status='" + AscmDeliBatSumMain.StatusDefine.inPlant + "'";
                                    List<AscmDeliBatSumMain> listAscmDeliBatSumMainPreviousInPlant = AscmDeliBatSumMainService.GetInstance().GetList(ynPage, "toPlantTime", "desc", where);
                                    if (listAscmDeliBatSumMainPreviousInPlant != null && listAscmDeliBatSumMainPreviousInPlant.Count > 0)
                                    {
                                        AscmDeliBatSumMain ascmDeliBatSumMain_tmp = listAscmDeliBatSumMainPreviousInPlant[0];
                                        if (ascmDeliBatSumMain_tmp.allocateOutDoor == "北门")
                                            ascmDeliBatSumMain.allocateOutDoor = "西门";
                                    }
                                    #endregion
                                }
                                batSumMainId = ascmDeliBatSumMain.id;
                                batSumDocNumber = ascmDeliBatSumMain.docNumber;
                                appointmentStartTime = ascmDeliBatSumMain.appointmentStartTime;
                                appointmentEndTime = ascmDeliBatSumMain.appointmentEndTime;
                                allocateOutDoor = ascmDeliBatSumMain.allocateOutDoor;
                            }
                            else
                            {
                            }

                            DateTime createTime = DateTime.Now;
                            string description = ascmDriver.supplierName + ":" + ascmDriver.name + ":" + ascmDriver.plateNumber;
                            AscmTruckSwipeLog ascmTruckSwipeLog = AscmTruckSwipeLogService.GetInstance().GetAddLog(doorId, readingHead, ascmDriver.rfid, ascmDriver.supplierId, driverId,
                                ascmDriver.supplierName, ascmDriver.name, ascmDriver.plateNumber, true, description, createTime, direction, batSumMainId, batSumDocNumber,
                                onTime, appointmentStartTime, appointmentEndTime);
                            using (ITransaction tx = YnDaoHelper.GetInstance().nHibernateHelper.GetCurrentSession().BeginTransaction())
                            {
                                try
                                {
                                    if (ascmTruckSwipeLog != null)
                                        YnDaoHelper.GetInstance().nHibernateHelper.Save(ascmTruckSwipeLog);
                                    if (ascmDeliBatSumMain != null)
                                        YnDaoHelper.GetInstance().nHibernateHelper.Update(ascmDeliBatSumMain);
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
